using EFT.Models;
using EvePasteNet.Parsers.EFT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvePasteNet
{
    public class EvePaste
    {
        private Utils utils = new Utils();

        public Ship ParseEFT(string raw)
        {
            var split = utils.SplitLines(raw);
            var parser = new ParseEFT();
            var ship = parser.Parse(split);
            return ship;
        }
    }
}
