using EvePasteNet.Parsers.EFT.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace EvePasteNet.Parsers.EFT
{
    public class ParseEFT
    {
        string[] blacklist = {"[empty high slot]",
                              "[empty low slot]",
                              "[empty med slot]",
                              "[empty rig slot]",
                              "[empty subsystem slot]" };

        string r_moduleWithCharge = @"^(?<module>[^\[][\S ]+), ?(?<chargeName>[\S ]+)$";
        string r_drone = @"^(?<drone>[\S ]+?) x? ?(?<quantity>[\d,'\.]+)$";
        string r_module = @"^(?<module>[\S ]+)$";

        public Ship Parse(IList<string> lines)
        {
            lines = lines.Where(l => !string.IsNullOrWhiteSpace(l))
                .Where(l => !blacklist.Contains(l.ToLowerInvariant().Trim()))
                .Select(l=>l.Trim())
                .ToList();

            if (!lines.FirstOrDefault().StartsWith("[", StringComparison.InvariantCulture))
                throw new FormatException("Invalid EFT title line.");

            var titleParts = lines[0].Trim(new char[] { '[', ']' }).Split(new char[] { ',' }, 2, StringSplitOptions.RemoveEmptyEntries);
            if(titleParts.Count() != 2)
                throw new FormatException("Invalid EFT title line.");

            var name = titleParts[0].TrimEnd(new char[] { ',' }).Trim();
            var eftName = titleParts[1].Trim();

            var modules = new List<Module>();

            var r = new Regex(r_moduleWithCharge, RegexOptions.IgnoreCase);
            var notMatched = new List<string>();
            foreach (var line in lines.Skip(1))
            {
                var m = r.Match(line);
                if (m.Success)
                    modules.Add(new Module
                    {
                        Name = m.Groups["module"].Value
                    });
                else
                    notMatched.Add(line);
            }

            r = new Regex(r_drone, RegexOptions.IgnoreCase);
            var notMatched2 = new List<string>();
            foreach (var line in notMatched)
            {
                var m = r.Match(line);
                if (m.Success)
                    modules.Add(new Module
                    {
                        Name = m.Groups["drone"].Value,
                        Quantity = int.Parse(m.Groups["quantity"].Value)
                    });
                else
                    notMatched2.Add(line);
            }

            r = new Regex(r_module, RegexOptions.IgnoreCase);
            var notMatched3 = new List<string>();
            foreach (var line in notMatched2)
            {
                var m = r.Match(line);
                if (m.Success)
                    modules.Add(new Module
                    {
                        Name = m.Groups["module"].Value
                    });
                else
                    notMatched3.Add(line);
            }

            Trace.WriteLineIf(notMatched3.Count > 0, $"There were {notMatched3.Count} unmatched line(s) in EFTParser module.  This should not happen.");

            var ship = new Ship { Name = name, EFTName = eftName, Modules = modules };

            return ship;
        }
    }
}
