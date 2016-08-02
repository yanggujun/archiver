using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Archiver.Common;
using Commons.Messaging;
using Commons.Messaging.Cache;
using Microsoft.AspNetCore.Http;

namespace Archiver.MessageServer
{
    public static class PipelineBuilder
    {
        public static IMessageController<HttpContext> Build()
        {
            var contextCache = new ContextCache();
            var router = new TypedMessageRouter();
            var worker = new ArchiverMessageWorker();
            var outbound = new OutboundController(contextCache);
            var dispatcher = new QueueDispatcher(worker, outbound);
            var assemblyCache = new AssemblyCache();
            var typeCache = new TypeCache();
            var typeLoader = new TypeLoader(assemblyCache, typeCache);
            router.AddTarget(typeof(ArchiverMsg), dispatcher);
            var inbound = new InboundController(router, contextCache, typeLoader);
            return inbound;
        }
    }
}
