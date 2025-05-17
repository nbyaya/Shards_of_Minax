using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class InfernalOrcQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Cut Down the InfernalOrc"; } }

        public override object Description
        {
            get
            {
                return
                    "You find *Ulric Oakhelm*, the weathered forester of West Montor.\n\n" +
                    "His cloak, lined with pine-needle embroidery, flutters slightly as he tightens his grip on a gnarled axe, eyes burning with a mix of sorrow and fury.\n\n" +
                    "“Damn the *InfernalOrc*! The beast trampled my best pines—trees nurtured since my father’s time. They say it's holed up near the Gate road, basking in the firelight of that cursed valley.”\n\n" +
                    "“I can't let it stand. My family's land borders those paths. Every night, I hear it roaring through the timber. It's not just my loss—soon, the whole town could feel its wrath.”\n\n" +
                    "**Slay the InfernalOrc** and avenge the Oakhelm groves. I’ll see you rightly rewarded.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then leave me to mourn the trees alone. But I fear the InfernalOrc won't stop at my pines.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "It still walks? Every hour it lives, another tree falls. And soon, lives with them.";
            }
        }

        public override object Complete
        {
            get
            {
                return "You’ve done what I could not. The timberlands will regrow, in time—but thanks to you, they’ll grow free of that beast’s shadow.\n\n" +
                       "Take these: *ArkainesValorArms*. May they guard you as fiercely as you guarded my lands.";
            }
        }

        public InfernalOrcQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(InfernalOrc), "InfernalOrc", 1));
            AddReward(new BaseReward(typeof(ArkainesValorArms), 1, "ArkainesValorArms"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Cut Down the InfernalOrc'!");
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

    public class UlricOakhelm : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(InfernalOrcQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBRanger()); // Closest fit for a Forester
        }

        [Constructable]
        public UlricOakhelm()
            : base("the Forester", "Ulric Oakhelm")
        {
        }

        public UlricOakhelm(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(90, 100, 75);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = 1150; // Dark forest green
            FacialHairItemID = Race.RandomFacialHair(this);
            FacialHairHue = 1150;
        }

        public override void InitOutfit()
        {
            AddItem(new StuddedChest() { Hue = 1821, Name = "Oak-Bound Jerkin" }); // Earthy brown
            AddItem(new LeatherLegs() { Hue = 1820, Name = "Pine-Leather Breeches" });
            AddItem(new LeatherGloves() { Hue = 1809, Name = "Sap-Stained Gloves" });
            AddItem(new BearMask() { Hue = 2001, Name = "Helm of the Woodwarden" }); // Rustic animal helm
            AddItem(new Cloak() { Hue = 1420, Name = "Verdant Watcher's Cloak" }); // Deep green cloak
            AddItem(new Boots() { Hue = 1811, Name = "Rootwalker Boots" });

            AddItem(new DoubleAxe() { Hue = 2101, Name = "Timberfell" });

            Backpack backpack = new Backpack();
            backpack.Hue = 2000;
            backpack.Name = "Forester's Pack";
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
