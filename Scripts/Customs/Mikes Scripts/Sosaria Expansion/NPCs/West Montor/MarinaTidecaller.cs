using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class DrownTheLavaDolphinQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Drown the LavaDolphin"; } }

        public override object Description
        {
            get
            {
                return
                    "You approach *Marina Tidecaller*, a weather-worn fisherwoman and former guide of Renika’s deep-sea divers.\n\n" +
                    "Her eyes, pale as seafoam, scan the scorched riverbanks. Her nets hang empty, her buckets charred.\n\n" +
                    "“These waters used to sing,” she mutters. “Now they scald. My catch comes up red... cooked before it hits the air.”\n\n" +
                    "**A beast**—they call it a LavaDolphin—feeds on molten vents, poisoning the rivers near the Gate of Hell. It shouldn’t be here. It’s not of this world.”\n\n" +
                    "She grips a harpoon, its shaft etched with wave-like runes.\n\n" +
                    "“I once guided sailors through storm-ripped currents. I’ve faced worse. But this thing... it mocks the sea. It **burns** what should be pure.”\n\n" +
                    "**Slay the LavaDolphin** and cleanse the river, before the molten tide claims West Montor’s lifeblood.";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then may the rivers take their revenge on us all. I can’t fish fire, friend.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still it feeds? The river boils, and I see ash where lilies once bloomed.";
            }
        }

        public override object Complete
        {
            get
            {
                return "You’ve done it... the river cools, and I can breathe again.\n\n" +
                       "Here, take these: *ShadowWalkersTabi.* They’ll help you tread softly where fire meets tide.\n\n" +
                       "**And thank you**. You’ve not just saved my catch—you’ve saved what little peace this town still has.";
            }
        }

        public DrownTheLavaDolphinQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(LavaDolphin), "LavaDolphin", 1));
            AddReward(new BaseReward(typeof(ShadowWalkersTabi), 1, "ShadowWalkersTabi"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Drown the LavaDolphin'!");
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

    public class MarinaTidecaller : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(DrownTheLavaDolphinQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBFisherman());
        }

        [Constructable]
        public MarinaTidecaller()
            : base("the Tidecaller", "Marina Tidecaller")
        {
        }

        public MarinaTidecaller(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 80, 25);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = 1002; // Tanned skin from sea winds
            HairItemID = 0x203B; // Long hair
            HairHue = 1153; // Sea-blue
        }

        public override void InitOutfit()
        {
            AddItem(new HoodedShroudOfShadows() { Hue = 1109, Name = "Storm-Wrap Cloak" }); // Dark gray cloak
            AddItem(new Skirt() { Hue = 1260, Name = "Salt-Stained Skirt" }); // Sea green skirt
            AddItem(new ElvenShirt() { Hue = 1154, Name = "Tidecaller’s Blouse" }); // Ocean blue shirt
            AddItem(new LeatherGloves() { Hue = 1102, Name = "Net-Mender’s Gloves" });
            AddItem(new Sandals() { Hue = 2406, Name = "Riverwalker’s Sandals" });
            AddItem(new BodySash() { Hue = 1150, Name = "Wave-Braided Sash" }); // Light blue sash
            AddItem(new FeatheredHat() { Hue = 1153, Name = "Mariner’s Crest" }); // Sea blue feathered hat

            AddItem(new FishermansTrident() { Hue = 1175, Name = "Ashpiercer Harpoon" }); // Dark steel harpoon

            Backpack backpack = new Backpack();
            backpack.Hue = 1160;
            backpack.Name = "Tidecaller’s Pack";
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
