using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class JewelThievesQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Jewel Thieves"; } }

        public override object Description
        {
            get
            {
                return
                    "Ferin Gravelborn, Devil Guard’s stern geology expert, stands over scattered shards of ore.\n\n" +
                    "“These Solen weren’t always a problem, but something’s twisted them. They’ve started stealing our best veins—night after night, gone.”\n\n" +
                    "“They’ve tunneled deep, into the heart of the Minax Mines, dragging precious minerals with them. Every CorrodedSolenWorker you kill, that’s a gemstone saved, a legacy protected.”\n\n" +
                    "**Slay CorrodedSolenWorkers** to reclaim the mines—and preserve our craft.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then the Solen keep digging, and we lose more with each day. Hope that someone else cares for Devil Guard’s future.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "They still scuttle in the dark? I hear them—chipping, clawing. The longer we wait, the more they take.";
            }
        }

        public override object Complete
        {
            get
            {
                return "You’ve crushed their little heist? Good. Devil Guard thanks you—and so do the stones themselves.\n\n" +
                       "Take this—*Stormfang*. May it strike as true for you as your blade did for us.";
            }
        }

        public JewelThievesQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(CorrodedSolenWorker), "Corroded Solen Worker", 5));
            AddReward(new BaseReward(typeof(Stormfang), 1, "Stormfang"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Jewel Thieves'!");
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

    public class FerinGravelborn : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(JewelThievesQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBBlacksmith()); // Fits geology/mining expert best
        }

        [Constructable]
        public FerinGravelborn()
            : base("the Geology Expert", "Ferin Gravelborn")
        {
        }

        public FerinGravelborn(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(90, 90, 40);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = 1153; // Slate-blue-gray
            FacialHairItemID = Race.RandomFacialHair(this);
            FacialHairHue = 1153;
        }

        public override void InitOutfit()
        {
            AddItem(new StuddedDo() { Hue = 2406, Name = "Granitebound Chest" }); // Deep stone-gray
            AddItem(new LeatherLegs() { Hue = 2301, Name = "Dustwalker's Greaves" }); // Earthen brown
            AddItem(new StuddedGloves() { Hue = 1821, Name = "Miner’s Claspers" }); // Dirty leather hue
            AddItem(new LeatherCap() { Hue = 1109, Name = "Rockwatcher’s Helm" }); // Dull slate
            AddItem(new HalfApron() { Hue = 2101, Name = "Gemsplitter's Apron" }); // Dark steel-blue
            AddItem(new FurBoots() { Hue = 1812, Name = "Ore-Treader Boots" }); // Coal-black

            AddItem(new Pickaxe() { Hue = 2505, Name = "Shardseeker" }); // Slightly shimmering steel

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Miner’s Satchel";
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
