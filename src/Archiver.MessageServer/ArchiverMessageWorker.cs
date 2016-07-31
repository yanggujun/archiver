using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Archiver.Common;
using Commons.Messaging;

namespace Archiver.MessageServer
{
    public class ArchiverMessageWorker : IWorker<ArchiverMsg, string>
    {
        public string Do(ArchiverMsg message)
        {
            Console.WriteLine("Hello, I am running");
            return "null";
        }
    }
}
