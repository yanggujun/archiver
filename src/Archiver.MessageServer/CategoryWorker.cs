﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Archiver.Common;
using Commons.Collections.Sequence;
using Commons.Json;
using Commons.Messaging;
using Commons.Messaging.Cache;

namespace Archiver.MessageServer
{
    public class CategoryWorker : IWorker<CategoryReqMsg, string>
    {
        private readonly ICache<string, string> catCache;
        private readonly ICache<string, List<Item>> catItemCache;
        private readonly ICache<long, Item> itemCache;
        private ISequence sequence;
        private List<FileSystemWatcher> watchers;
        public CategoryWorker(ICache<string, string> catCache, ICache<string, List<Item>> catItemCache, ICache<long, Item> itemCache, ISequence sequence)
        {
            this.catCache = catCache;
            this.sequence = sequence;
            this.catItemCache = catItemCache;
            this.itemCache = itemCache;
            watchers = new List<FileSystemWatcher>();
            foreach (var kvp in catCache)
            {
                var path = kvp.Value;
                if (Directory.Exists(path))
                {
                    var fsw = new FileSystemWatcher(path);
                    fsw.Changed += new FileSystemEventHandler(OnFileChanged);
                    fsw.Created += new FileSystemEventHandler(OnFileChanged);
                    fsw.Deleted += new FileSystemEventHandler(OnFileChanged);
                    fsw.Renamed += new RenamedEventHandler(OnFileChanged);
                    fsw.EnableRaisingEvents = true;
                    watchers.Add(fsw);
                }
            }
        }

        private void OnFileChanged(object sender, FileSystemEventArgs e)
        {
            catItemCache.Clear();
            itemCache.Clear();
        }

        public string Do(CategoryReqMsg message)
        {
            var catName = message.Name;
            var json = "{}";
            if (catCache.Contains(catName))
            {
                if (!catItemCache.Contains(catName))
                { 
                    var folder = catCache.From(catName);
                    var items = folder.Read(sequence);
                    if (items != null)
                    {
                        catItemCache.Add(catName, items);
                        json = JsonMapper.ToJson(items);
                        foreach (var item in items)
                        {
                            itemCache.Add(item.Id, item);
                        }
                    }
                }
                else
                {
                    json = JsonMapper.ToJson(catItemCache.From(catName));
                }
            }

            return json;
        }
    }
}
