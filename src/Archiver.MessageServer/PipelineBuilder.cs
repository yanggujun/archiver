using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Archiver.Common;
using Commons.Messaging;
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
            router.AddTarget(typeof(ArchiverMsg), dispatcher);
            var inbound = new InboundController(router, contextCache);
            return inbound;
        }
    }
}
