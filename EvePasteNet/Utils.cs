using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvePasteNet
{
    public class Utils
    {
        public IList<string> SplitLines(string raw)
        {
            var result = raw.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            return result;
        }
    }
}
