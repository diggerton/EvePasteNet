using EvePasteNet.Parsers.EFT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFT.Models
{
    public class Ship
    {
        public string Name { get; set; }
        public string EFTName { get; set; }
        public IEnumerable<Module> Modules { get; set; }
    }
}
