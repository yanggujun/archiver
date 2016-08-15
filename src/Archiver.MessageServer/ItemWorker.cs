using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Archiver.Common;
using Commons.Messaging;
using Commons.Messaging.Cache;

namespace Archiver.MessageServer
{
    public class ItemWorker : IWorker<ItemReqMsg, string>
    {
        private readonly ICache<long, List<Item>> itemCache;

        public ItemWorker(ICache<long, List<Item>> itemCache)
        {
            this.itemCache = itemCache;
        }
        public string Do(ItemReqMsg message)
        {
            throw new NotImplementedException();
        }
    }
}
