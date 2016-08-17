using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Archiver.Common;
using Commons.Collections.Sequence;

namespace Archiver.MessageServer
{
    internal static class ServerExtensions
    {
        public static List<Item> Read(this string folder, ISequence sequence)
        {
            var items = new List<Item>();
            var dir = new DirectoryInfo(folder);
            if (!dir.Exists)
            {
                return null;
            }
            foreach (var f in dir.GetFiles())
            {
                if ((f.Attributes & FileAttributes.Hidden) == 0 && (f.Attributes & FileAttributes.System) == 0)
                {
                    var item = new Item
                    {
                        Id = sequence.Next(),
                        Name = f.Name,
                        Path = f.FullName,
                        IsFolder = false
                    };
                    items.Add(item);
                }
            }

            foreach (var d in dir.GetDirectories())
            {
                var item = new Item
                {
                    Id = sequence.Next(),
                    Name = d.Name,
                    Path = d.FullName,
                    IsFolder = true
                };
                items.Add(item);
            }

            return items;
        }
    }
}
