using System;
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
    public class ItemWorker : IWorker<ItemReqMsg, string>
    {
        private readonly ICache<long, Item> itemCache;
        private readonly ISequence sequence;

        public ItemWorker(ICache<long, Item> itemCache, ISequence sequence)
        {
            this.itemCache = itemCache;
            this.sequence = sequence;
        }

        public string Do(ItemReqMsg message)
        {
            var json = "{}";
            if (itemCache.Contains(message.Id))
            {
                var item = itemCache.From(message.Id);
                if (item.IsFolder)
                {
                    if (item.SubItems == null)
                    {
                        var folder = item.Path;
                        item.SubItems = folder.Read(sequence);
                        foreach (var i in item.SubItems)
                        {
                            itemCache.Add(i.Id, i);
                        }
                    }
                    json = JsonMapper.ToJson(item.SubItems);
                }
            }

            return json;
        }
    }
}
