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
        private ISequence sequence;
        public CategoryWorker(ICache<string, string> catCache, ICache<string, List<Item>> itemCache, ISequence sequence)
        {
            this.catCache = catCache;
            this.sequence = sequence;
            this.catItemCache = itemCache;
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
                    var dir = new DirectoryInfo(folder);
                    if (dir.Exists)
                    {
                        var items = new List<Item>();
                        foreach (var f in dir.GetFiles())
                        {
                            if ((f.Attributes & FileAttributes.Hidden) == 0 && (f.Attributes & FileAttributes.System) == 0)
                            {
                                var item = new Item
                                {
                                    Id = sequence.Next(),
                                    Name = f.Name,
                                    Path = f.FullName,
                                    IsFolder = false
                                };
                                items.Add(item);
                            }
                        }

                        foreach (var d in dir.GetDirectories())
                        {
                            var item = new Item
                            {
                                Id = sequence.Next(),
                                Name = d.Name,
                                Path = dir.FullName,
                                IsFolder = true
                            };
                            items.Add(item);
                        }
                        catItemCache.Add(catName, items);

                        json = JsonMapper.ToJson(items);
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
