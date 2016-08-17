using System;
using System.Collections.Generic;
using System.Reflection;
using Archiver.Common;
using Commons.Collections.Map;
using Commons.Collections.Sequence;
using Commons.Json;
using Commons.Messaging;
using Commons.Messaging.Cache;
using Microsoft.AspNetCore.Http;

namespace Archiver.MessageServer
{
    public static class PipelineBuilder
    {
        public static IMessageController<HttpContext> Build()
        {
            JsonMapper.UseLowerCaseBool().For<Item>()
                                         .Not.MapProperty(x => x.Path)
                                         .Not.MapProperty(x => x.SubItems);

            var cacheFactory = new CacheFactory();
            var cacheManager = new CacheManager(cacheFactory);
            var catCache = cacheManager.NewCache<string, string>("category");
            var contextCache = cacheManager.NewCache<long, HttpContext>("context");
            var typeCache = cacheManager.NewCache<string, Type>("type");
            var catItemCache = cacheManager.NewCache<string, List<Item>>("catitem");
            var itemCache = cacheManager.NewCache<long, Item>("item");

            var contextSeq = new AtomicSequence();

            var categorySeq = new SimpleSequence();

            var router = new TypedMessageRouter();

            router.AddTarget(typeof(CategoryListReqMsg), new SimpleDispatcher(new CategoryListWorker(catCache)));
            router.AddTarget(typeof(CategoryReqMsg), new SimpleDispatcher(new CategoryWorker(catCache, catItemCache, itemCache, categorySeq)));
            router.AddTarget(typeof(ItemReqMsg), new SimpleDispatcher(new ItemWorker(itemCache, categorySeq)));

            var outbound = new OutboundController(contextCache);
            var assemblyCache = cacheManager.NewCache<string, Assembly>("asssembly");
            var typeLoader = new TypeLoader(assemblyCache, typeCache);
            var inbound = new InboundController(router, typeLoader, contextSeq);
            return inbound;
        }
    }
}
