using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class ShatteringFrostQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Shattering Frost"; } }

        public override object Description
        {
            get
            {
                return
                    "Before you stands *Brynja Frostfury*, a towering huntress with frost-bitten gauntlets and eyes like storm clouds.\n\n" +
                    "She clenches a cracked steel trap, jaw tight with frustration.\n\n" +
                    "“For three generations, we’ve hunted ogres. Skinned them. Broken their bones for warmth.”\n\n" +
                    "“But this one, this *Glacial Ogre*, mocks me. Every trap I set—it shatters. Steel like straw.”\n\n" +
                    "“Its club... cleaves stone. One swing near flattened me and my men. I won’t let it end our line.”\n\n" +
                    "**Slay the Glacial Ogre** that haunts the Ice Cavern. Prove frost can break.”";
            }
        }

        public override object Refuse
        {
            get { return "Then go. Leave me to my family’s shame, and may the ogre find you in the cold."; }
        }

        public override object Uncomplete
        {
            get { return "It still breathes? It still mocks us? Come back when it’s DEAD."; }
        }

        public override object Complete
        {
            get
            {
                return "The frost broke, didn’t it? You’ve done my bloodline proud.\n\n" +
                       "Take this: *AtomicRegulator.* A tool for slayers—not hunters. You’ve earned that title now.";
            }
        }

        public ShatteringFrostQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(GlacialOgre), "Glacial Ogre", 1));
            AddReward(new BaseReward(typeof(AtomicRegulator), 1, "AtomicRegulator"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Shattering Frost'!");
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

    public class BrynjaFrostfury : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(ShatteringFrostQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => false;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBFurtrader());
        }

        [Constructable]
        public BrynjaFrostfury()
            : base("the Ogre Hunter", "Brynja Frostfury")
        {
        }

        public BrynjaFrostfury(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(100, 80, 70);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = 8255; // Long hair
            HairHue = 1153; // Ice-blue
            FacialHairItemID = 0; // None
        }

        public override void InitOutfit()
        {
            AddItem(new BearMask() { Hue = 1150, Name = "Frostfang Visor" });
            AddItem(new StuddedDo() { Hue = 1151, Name = "Glacier-Worn Armor" });
            AddItem(new LeatherLegs() { Hue = 1109, Name = "Icebound Greaves" });
            AddItem(new LeatherGloves() { Hue = 1152, Name = "Frostbite Gauntlets" });
            AddItem(new Cloak() { Hue = 2101, Name = "Snowdrift Cloak" });
            AddItem(new FurBoots() { Hue = 2105, Name = "Ogrehide Boots" });

            AddItem(new DoubleAxe() { Hue = 1102, Name = "Ogre Cleaver" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Hunter's Pack";
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
