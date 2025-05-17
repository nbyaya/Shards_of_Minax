using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class BronzeBehemothQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Bronze Behemoth"; } }

        public override object Description
        {
            get
            {
                return
                    "Fenwyn Bronzesmith, renowned metalsmith of Castle British, stands grimly near his tarnished copper vats.\n\n" +
                    "His sleeves are rolled high, stained with soot and acid, eyes narrowed with concern.\n\n" +
                    "\"A beast—**the CuPatternVatbeast**—it’s fouling my copper vats. Corrosive enzymes are spilling into the ore. I’ve lost spouts, lost good copper! I can't let this ruin the next batch.\"\n\n" +
                    "\"That monster crawled from Preservation Vault 44, or so I reckon. And it won’t stop ‘til all’s corroded.\"\n\n" +
                    "\"Kill it, and I’ll make sure you’re dressed in something finer than grime-stained armor. Something worthy of the light.\"\n\n" +
                    "**Slay the CuPatternVatbeast** before it destroys Fenwyn’s craft and poisons Castle British’s forges.";
            }
        }

        public override object Refuse
        {
            get { return "Then may rust take us all. The vats won’t hold without your help."; }
        }

        public override object Uncomplete
        {
            get { return "It’s still alive? Then I’ve already lost more copper... Hurry!"; }
        }

        public override object Complete
        {
            get
            {
                return
                    "You’ve done it? The beast’s gone?\n\n" +
                    "Bless you. You’ve saved not just my vats, but **the future of our forge**.\n\n" +
                    "Take this—**MeadowlightDress**. I crafted it from copper strands, infused with morning dew magic. May it shine where you walk.";
            }
        }

        public BronzeBehemothQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(CuPatternVatbeast), "CuPatternVatbeast", 1));
            AddReward(new BaseReward(typeof(MeadowlightDress), 1, "MeadowlightDress"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Bronze Behemoth'!");
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

    public class FenwynBronzesmith : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(BronzeBehemothQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBSmithTools());
        }

        [Constructable]
        public FenwynBronzesmith()
            : base("the Metalsmith", "Fenwyn Bronzesmith")
        {
        }

        public FenwynBronzesmith(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(90, 85, 35);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = 1025; // Weathered tan skin
            HairItemID = 0x203B; // Short hair
            HairHue = 2419; // Bronze hue
            FacialHairItemID = 0x2041; // Full beard
            FacialHairHue = 2419; // Bronze hue
        }

        public override void InitOutfit()
        {
            AddItem(new StuddedDo() { Hue = 2413, Name = "Bronze-Forged Vest" });
            AddItem(new StuddedLegs() { Hue = 2413, Name = "Metalworker's Leggings" });
            AddItem(new StuddedGloves() { Hue = 2413, Name = "Vat-Hardened Gloves" });
            AddItem(new HalfApron() { Hue = 2205, Name = "Soot-Stained Apron" });
            AddItem(new Boots() { Hue = 1109, Name = "Forge-Tread Boots" });
            AddItem(new LeatherCap() { Hue = 1819, Name = "Bronzeworker's Cap" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1153;
            backpack.Name = "Smith's Pack";
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
