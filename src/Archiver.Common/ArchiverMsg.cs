using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Archiver.Common
{
    public abstract class ArchiverMsg
    {
        public Guid MsgId { get; set; }
    }
}
