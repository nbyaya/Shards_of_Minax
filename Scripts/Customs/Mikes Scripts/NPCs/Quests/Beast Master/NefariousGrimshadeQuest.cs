using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class NefariousGrimshadeQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Nefarious Grimshade's Lich Hunt"; } }

        public override object Description
        {
            get
            {
                return 
                    "Greetings, brave adventurer! I am Nefarious Grimshade, and I have a rather peculiar task for you. " +
                    "To prove your mettle and gain access to my exclusive lich shop, you must track down and defeat " +
                    "these elusive liches. Complete these tasks:\n\n" +
                    "1. Slay a BloodLich.\n" +
                    "2. Defeat a FrostLich.\n" +
                    "3. Conquer an InfernalLich.\n" +
                    "4. Overcome a NecroticLich.\n" +
                    "5. Destroy a PlagueLich.\n" +
                    "6. Vanquish a ShadowLich.\n" +
                    "7. Subdue a SoulEaterLich.\n" +
                    "8. Eradicate a StormLich.\n" +
                    "9. Defeat a ToxicLich.\n" +
                    "10. Exterminate a WraithLich.\n\n" +
                    "Complete these tasks, and I will grant you access to my lich-laden shop!";
            }
        }

        public override object Refuse { get { return "Very well. Return when you are ready to face the liches."; } }

        public override object Uncomplete { get { return "You haven't completed all the tasks yet. Keep hunting those liches!"; } }

        public override object Complete { get { return "Remarkable! You've proven yourself worthy against the liches. My shop is now open to you!"; } }

        public NefariousGrimshadeQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(BloodLich), "Blood Liches", 1));
            AddObjective(new SlayObjective(typeof(FrostLich), "Frost Liches", 1));
            AddObjective(new SlayObjective(typeof(InfernalLich), "Infernal Liches", 1));
            AddObjective(new SlayObjective(typeof(NecroticLich), "Necrotic Liches", 1));
            AddObjective(new SlayObjective(typeof(PlagueLich), "Plague Liches", 1));
            AddObjective(new SlayObjective(typeof(ShadowLich), "Shadow Liches", 1));
            AddObjective(new SlayObjective(typeof(SoulEaterLich), "SoulEater Liches", 1));
            AddObjective(new SlayObjective(typeof(StormLich), "Storm Liches", 1));
            AddObjective(new SlayObjective(typeof(ToxicLich), "Toxic Liches", 1));
            AddObjective(new SlayObjective(typeof(WraithLich), "Wraith Liches", 1));

            AddReward(new BaseReward(typeof(Gold), 1500, "1500 Gold"));
            AddReward(new BaseReward(typeof(LichToken), 1, "Lich Token"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You have completed Nefarious Grimshade's challenge!");
            Owner.PlaySound(CompleteSound);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }


    public class NefariousGrimshade : MondainQuester
    {
        public override bool IsActiveVendor { get { return true; } }

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBNefariousGrimshade());
        }

        [Constructable]
        public NefariousGrimshade()
            : base("Nefarious Grimshade", "Master of Liches")
        {
        }

        public NefariousGrimshade(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 25);

            Female = false;
            Race = Race.Human;

            Hue = Utility.RandomSkinHue();
            HairItemID = Utility.RandomList(0x203B, 0x2049, 0x2048);
            HairHue = Utility.RandomHairHue();
            FacialHairItemID = Utility.RandomList(0x203E, 0x203F, 0x2040, 0x2041, 0x204B, 0x204C, 0x204D);
            FacialHairHue = HairHue;
        }

        public override void InitOutfit()
        {
            AddItem(new Backpack());
            AddItem(new Boots(Utility.RandomNeutralHue()));
            AddItem(new LongPants(Utility.RandomNeutralHue()));
            AddItem(new FancyShirt(Utility.RandomNeutralHue()));
            AddItem(new Cloak(Utility.RandomNeutralHue()));
        }

        public override void VendorBuy(Mobile from)
        {
            PlayerMobile player = from as PlayerMobile;
            if (player != null)
            {
                Item token = player.Backpack.FindItemByType(typeof(LichToken));
				Item masterToken = player.Backpack.FindItemByType(typeof(MasterToken));

                if (token != null || masterToken != null)
                {
                    SayTo(from, "Welcome to my lich collection shop!");
                    base.VendorBuy(from);
                }
                else
                {
                    SayTo(from, "You must have a Lich Token to access my shop.");
                }
            }
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(NefariousGrimshadeQuest)
                };
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }


    public class SBNefariousGrimshade : SBInfo
    {
        private System.Collections.Generic.List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public SBNefariousGrimshade()
        {
        }

        public override System.Collections.Generic.List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }
        public override IShopSellInfo SellInfo { get { return m_SellInfo; } }

        public class InternalBuyInfo : System.Collections.Generic.List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                // Add the lich-themed creatures here
                Add(new AnimalBuyInfo(1, typeof(BloodLich), 1000, 10, 13, 0));
                Add(new AnimalBuyInfo(1, typeof(FrostLich), 1200, 10, 13, 0));
                Add(new AnimalBuyInfo(1, typeof(InfernalLich), 1500, 10, 13, 0));
                Add(new AnimalBuyInfo(1, typeof(NecroticLich), 1300, 10, 13, 0));
                Add(new AnimalBuyInfo(1, typeof(PlagueLich), 1400, 10, 13, 0));
                Add(new AnimalBuyInfo(1, typeof(ShadowLich), 1600, 10, 13, 0));
                Add(new AnimalBuyInfo(1, typeof(SoulEaterLich), 1700, 10, 13, 0));
                Add(new AnimalBuyInfo(1, typeof(StormLich), 1800, 10, 13, 0));
                Add(new AnimalBuyInfo(1, typeof(ToxicLich), 1900, 10, 13, 0));
                Add(new AnimalBuyInfo(1, typeof(WraithLich), 2000, 10, 13, 0));
            }
        }

        public class InternalSellInfo : GenericSellInfo
        {
            public InternalSellInfo()
            {
            }
        }
    }
}
