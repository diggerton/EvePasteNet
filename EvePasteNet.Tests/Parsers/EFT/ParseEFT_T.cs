using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using EvePasteNet.Parsers.EFT;

namespace EvePasteNet.Tests.Parsers.EFT
{
    [TestClass]
    public class ParseEFT_T
    {
        [TestMethod]
        public void Parse_Success()
        {
            var eftRaw = @" [Cerberus, small gang anti-frig]

                            Nanofiber Internal Structure II
                            Ballistic Control System II
                            Ballistic Control System II
                            Reactor Control Unit II

                            X-Large Ancillary Shield Booster, Cap Booster 400
                            50MN Cold-Gas Enduring Microwarpdrive
                            Warp Disruptor II
                            X5 Prototype Engine Enervator
                            EM Ward Amplifier II

                            Rapid Light Missile Launcher II, Caldari Navy Scourge Light Missile
                            Rapid Light Missile Launcher II, Caldari Navy Scourge Light Missile
                            Rapid Light Missile Launcher II, Caldari Navy Scourge Light Missile
                            Rapid Light Missile Launcher II, Caldari Navy Scourge Light Missile
                            Rapid Light Missile Launcher II, Caldari Navy Scourge Light Missile
                            Rapid Light Missile Launcher II, Caldari Navy Scourge Light Missile

                            Medium Core Defense Field Extender II
                            Medium Core Defense Field Extender II


                            Acolyte II x3";

            var p = new EvePasteNet.Parsers.EFT.ParseEFT();
            var lines = eftRaw.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();

            var parseResults = p.Parse(lines);

            Assert.IsFalse(string.IsNullOrWhiteSpace(parseResults.Name));
            Assert.IsFalse(string.IsNullOrWhiteSpace(parseResults.EFTName));
            Assert.IsTrue(parseResults.Modules.Count() == 18);
            Assert.AreEqual(3, parseResults.Modules.First(m => m.Name == "Acolyte II").Quantity);
            Assert.AreEqual("Cerberus", parseResults.Name);
            Assert.AreEqual("small gang anti-frig", parseResults.EFTName);
        }

        [TestMethod]
        public void Parse_Success_MissingModules()
        {
            var eftRaw = @" [Cerberus, [[small gang anti-frig]
                            Nanofiber Internal Structure II
                            Ballistic Control System II
                            [empty low slot]
                            Reactor Control Unit II

                            X-Large Ancillary Shield Booster, Cap Booster 400
                            [empty med slot]
                            Warp Disruptor II
                            X5 Prototype Engine Enervator
                            EM Ward Amplifier II

                            Rapid Light Missile Launcher II, Caldari Navy Scourge Light Missile
                            Rapid Light Missile Launcher II, Caldari Navy Scourge Light Missile
                            Rapid Light Missile Launcher II, Caldari Navy Scourge Light Missile
                            Rapid Light Missile Launcher II, Caldari Navy Scourge Light Missile
                            [empty high slot]
                            Rapid Light Missile Launcher II, Caldari Navy Scourge Light Missile

                            Medium Core Defense Field Extender II

                            Acolyte II x3
                            ";

            var p = new EvePasteNet.Parsers.EFT.ParseEFT();
            var lines = eftRaw.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();

            var parseResults = p.Parse(lines);

            Assert.IsFalse(string.IsNullOrWhiteSpace(parseResults.Name));
            Assert.IsFalse(string.IsNullOrWhiteSpace(parseResults.EFTName));
            Assert.IsTrue(parseResults.Modules.Count() == 14);
            Assert.AreEqual(3, parseResults.Modules.First(m => m.Name == "Acolyte II").Quantity);
            Assert.AreEqual("Cerberus", parseResults.Name);
            Assert.AreEqual("[[small gang anti-frig", parseResults.EFTName);
        }

        [TestMethod]
        public void Parse_Success_MissingSubsystems()
        {
            var eftRaw = @" [Legion, New Setup 1]
                            Energized Adaptive Nano Membrane I
                            Energized Adaptive Nano Membrane I
                            [empty low slot]

                            10MN Afterburner II
                            10MN Afterburner II
                            10MN Afterburner II
                            [empty med slot]

                            Focused Modulated Pulse Energy Beam I, Multifrequency M
                            [empty high slot]


                            Legion Defensive - Warfare Processor
                            Legion Electronics - Tactical Targeting Network
                            Legion Engineering - Power Core Multiplier
                            [empty subsystem slot]
                            [empty subsystem slot]

                            ";

            var p = new EvePasteNet.Parsers.EFT.ParseEFT();
            var lines = eftRaw.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();

            var parseResults = p.Parse(lines);

            Assert.IsFalse(string.IsNullOrWhiteSpace(parseResults.Name));
            Assert.IsFalse(string.IsNullOrWhiteSpace(parseResults.EFTName));
            Assert.IsTrue(parseResults.Modules.Count() == 9);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void Parse_Success_BadFormat()
        {
            var eftRaw = @" Legion, New Setup 1]
                            Energized Adaptive Nano Membrane I
                            Energized Adaptive Nano Membrane I
                            [empty low slot]

                            10MN Afterburner II
                            10MN Afterburner II
                            10MN Afterburner II
                            [empty med slot]

                            Focused Modulated Pulse Energy Beam I, Multifrequency M
                            [empty high slot]


                            Legion Defensive - Warfare Processor
                            Legion Electronics - Tactical Targeting Network
                            Legion Engineering - Power Core Multiplier
                            [empty subsystem slot]
                            [empty subsystem slot]

                            ";

            var p = new EvePasteNet.Parsers.EFT.ParseEFT();
            var lines = eftRaw.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();

            var parseResults = p.Parse(lines);
        }

        [TestMethod]
        public void NoModules()
        {
            var eftRaw = @"[Retribution, ,,,ss]]

[Empty Low slot]
[Empty Low slot]
[Empty Low slot]
[Empty Low slot]
[Empty Low slot]

[Empty Med slot]
[Empty Med slot]

[Empty High slot]
[Empty High slot]
[Empty High slot]
[Empty High slot]
[Empty High slot]

[Empty Rig slot]
[Empty Rig slot]";

            var p = new EvePasteNet.Parsers.EFT.ParseEFT();
            var lines = eftRaw.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();
            var parseResults = p.Parse(lines);

            
        }
    }
}
