using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class WebOfTheWastesQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Web of the Wastes"; } }

        public override object Description
        {
            get
            {
                return
                    "You travel these dunes, stranger? Then listen well.\n\n" +
                    "I’m Tarek Sandrunner, guide of the sacred pilgrim trails through the Moon's shadow. But lately, " +
                    "our path has grown perilous. CamelSpiders—monsters of fang and silk—burrow beneath the sand, " +
                    "ambushing our caravans when night falls. Travelers fear to tread.\n\n" +
                    "**Hunt down 8 CamelSpiders** that infest the caravan routes beneath Moon’s light, and restore the sands to the faithful.";
            }
        }

        public override object Refuse { get { return "Then may the sands swallow your path, wanderer."; } }

        public override object Uncomplete { get { return "The webs still cling to the trails. You’ve more spiders to slay."; } }

        public override object Complete { get { return "The dunes thank you. So do the pilgrims. Take this—it’s woven for those who brave the wastes."; } }

        public WebOfTheWastesQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(CamelSpider), "CamelSpiders", 8));

            AddReward(new BaseReward(typeof(PilgrimsRopewalkers), 1, "PilgrimsRopewalkers"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Web of the Wastes'!");
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

    public class TarekSandrunner : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(WebOfTheWastesQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBRancher()); 
        }

        [Constructable]
        public TarekSandrunner() : base("Tarek Sandrunner", "Desert Guide")
        {
            Title = "Desert Guide";
			Body = 0x190; // Male human

            // Outfit
            AddItem(new Tunic { Hue = 2213, Name = "Dune-Worn Tunic" }); // Light tan, sun-bleached look
            AddItem(new LeatherGloves { Hue = 2101, Name = "Sunveil Cloak" }); // Muted golden hue, protective desert cloak
            AddItem(new LeatherLegs { Hue = 2406, Name = "Traveler's Leathers" }); // Dusty brown for rugged terrain
            AddItem(new Sandals { Hue = 2309, Name = "Wanderer's Sandals" }); // Worn leather
            AddItem(new Bandana { Hue = 2205, Name = "Veil of the Moon" }); // Light cloth headwrap, pale blue tint

            // Gear for flair
            AddItem(new QuarterStaff { Hue = 2412, Name = "Guide's Carved Staff" }); // Carved with desert symbols
            AddItem(new Backpack { Hue = 2115, Name = "Trail Pouch" }); // Small storage for maps or herbs

            SetStr(75, 85);
            SetDex(90, 100);
            SetInt(60, 70);

            SetDamage(5, 10);
            SetHits(200, 220);
        }

        public TarekSandrunner(Serial serial) : base(serial) { }

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

    // Example custom item classes, if you want to define unique visuals
    public class SandColoredTunic : Tunic
    {
        public SandColoredTunic()
        {
            Weight = 3.0;
        }

        public SandColoredTunic(Serial serial) : base(serial) { }

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

    public class DesertWraps : Cloak
    {
        public DesertWraps()
        {
            Weight = 5.0;
        }

        public DesertWraps(Serial serial) : base(serial) { }

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
