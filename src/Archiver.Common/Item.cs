using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Archiver.Common
{
    public class Item
    {
        public string Name { get; set; }
        public long Id { get; set; }
        public string Path { get; set; }
        public bool IsFolder { get; set; }
        public List<Item> SubItems { get; set; }
    }
}
