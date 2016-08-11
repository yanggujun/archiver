using System;
using System.Reflection;
using Archiver.Common;
using Commons.Collections.Map;
using Commons.Collections.Sequence;
using Commons.Messaging;
using Commons.Messaging.Cache;
using Microsoft.AspNetCore.Http;

namespace Archiver.MessageServer
{
    public static class PipelineBuilder
    {
        public static IMessageController<HttpContext> Build()
        {
            var cacheFactory = new CacheFactory();
            var cacheManager = new CacheManager(cacheFactory);
            var catCache = cacheManager.NewCache<string, string>("category");
            var contextCache = cacheManager.NewCache<long, HttpContext>("context");
            var typeCache = cacheManager.NewCache<string, Type>("type");

            var contextSeq = new AtomicSequence();

            var categorySeq = new SimpleSequence();

            var router = new TypedMessageRouter();

            router.AddTarget(typeof(CategoryListReqMsg), new SimpleDispatcher(new CategoryListWorker(catCache)));
            router.AddTarget(typeof(CategoryReqMsg), new SimpleDispatcher(new CategoryWorker(catCache, categorySeq)));

            var outbound = new OutboundController(contextCache);
            var assemblyCache = cacheManager.NewCache<string, Assembly>("asssembly");
            var typeLoader = new TypeLoader(assemblyCache, typeCache);
            var inbound = new InboundController(router, typeLoader, contextSeq);
            return inbound;
        }
    }
}
