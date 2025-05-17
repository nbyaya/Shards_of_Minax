using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class HellScorpionQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Exterminate the HellScorpion"; } }

        public override object Description
        {
            get
            {
                return
                    "You find yourself in the presence of *Petra Sandquist*, a weather-beaten scavenger, her face shadowed beneath a tattered hood.\n\n" +
                    "Her right eye is missing, the socket covered by a jagged obsidian eyepatch, etched with crude protective runes.\n\n" +
                    "“Death Glutch ain't just fire and ash anymore. That damned **HellScorpion** has made it its nest, hoardin’ the desert gems I dig for.”\n\n" +
                    "She spits to the side, flexing gloved fingers, stained from years of foraging.\n\n" +
                    "“It’s taken enough. My finds melt in its acid. My crew won’t go back. And this—” she taps her eyepatch—“this is what I get for lookin’ too close in a sandstorm.”\n\n" +
                    "“You kill that beast. Burn its nest, crush its gems—I don’t care. Just bring peace back to Glutch so we can scavenge in one piece.”\n\n" +
                    "**Hunt and slay the HellScorpion** lurking in Death Glutch.";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then you best stay out of Glutch too. That thing won't stop until it's the only one left breathing in those sands.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still scuttling? I hear it in the night sometimes—its claws drag through my dreams. Kill it, or don’t bother coming back.";
            }
        }

        public override object Complete
        {
            get
            {
                return "It's dead? Really? Well, I’ll believe it when the sands stop screaming. But here—take this, **DwemerAegis**. Found it deep under ash, figured I’d keep it for something like this.\n\n" +
                       "Now I can get back to scavenging without losing more than my eye.";
            }
        }

        public HellScorpionQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(HellScorpion), "HellScorpion", 1));
            AddReward(new BaseReward(typeof(DwemerAegis), 1, "DwemerAegis"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Exterminate the HellScorpion'!");
            Owner.PlaySound(CompleteSound);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class PetraSandquist : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(HellScorpionQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBProvisioner()); // Scavenger with trade goods
        }

        [Constructable]
        public PetraSandquist()
            : base("the Scavenger", "Petra Sandquist")
        {
        }

        public PetraSandquist(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 80, 30);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = 1102; // Sandy blonde
            // Missing eye detail represented by eyepatch
        }

        public override void InitOutfit()
        {
            AddItem(new LeatherBustierArms() { Hue = 2420, Name = "Ash-Hardened Corset" }); // Charcoal grey
            AddItem(new LeatherSkirt() { Hue = 2207, Name = "Glutch-Walker Skirt" }); // Dusty brown
            AddItem(new LeatherGloves() { Hue = 2101, Name = "Scavenger's Mitts" });
            AddItem(new Bandana() { Hue = 1833, Name = "Wind-Torn Wrap" }); // Dull red, to signify weathering
            AddItem(new Sandals() { Hue = 1109, Name = "Scorched Sandals" });
            AddItem(new BodySash() { Hue = 2112, Name = "Grit-Stained Sash" });
            AddItem(new Cloak() { Hue = 2105, Name = "Tattered Desert Cloak" });

            AddItem(new Pitchfork() { Hue = 2413, Name = "Gem-Scavenger's Fork" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1151; // Dark leather hue
            backpack.Name = "Scavenger's Pack";
            AddItem(backpack);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
