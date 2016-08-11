using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Commons.Messaging;

namespace Archiver.MessageServer
{
    public class ActionWorker<T> : IWorker<T, string>
    {
        private readonly Func<T, string> func;
        public ActionWorker(Func<T, string> func)
        {
            this.func = func;
        }

        public string Do(T message)
        {
            return func(message);
        }
    }
}
