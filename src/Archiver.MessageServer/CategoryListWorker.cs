﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Archiver.Common;
using Commons.Collections.Map;
using Commons.Json;
using Commons.Messaging;
using Commons.Messaging.Cache;

namespace Archiver.MessageServer
{
    public class CategoryListWorker : IWorker<CategoryListReqMsg, string>
    {
        private readonly ICache<string, string> catCache;
        public CategoryListWorker(ICache<string, string> catCache)
        {
            this.catCache = catCache;
        }

        public string Do(CategoryListReqMsg message)
        {
            var cateFile = Path.Combine(AppContext.BaseDirectory, "cat.json");
            string catJson;
            using (var fs = new FileStream(cateFile, FileMode.Open))
            {
                using (var reader = new StreamReader(fs))
                {
                    catJson = reader.ReadToEnd();
                }
            }

            var cats = JsonMapper.To<HashedMap<string, string>>(catJson);
            foreach(var kvp in cats)
            {
                catCache.Add(kvp.Key, kvp.Value);
            }

            return JsonMapper.ToJson(cats.Keys);
        }
    }
}