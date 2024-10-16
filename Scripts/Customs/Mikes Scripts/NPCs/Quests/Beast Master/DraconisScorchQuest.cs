using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class DraconisScorchQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Draconis Scorch's Dragon Hunt"; } }

        public override object Description
        {
            get
            {
                return 
                    "Greetings, adventurer! I am Draconis Scorch, renowned for my dragon taming prowess. " +
                    "To prove your worth and gain access to my exclusive dragon shop, you must demonstrate your " +
                    "bravery by slaying the following dragons:\n\n" +
                    "1. Ancient Dragon\n" +
                    "2. Blood Dragon\n" +
                    "3. Celestial Dragon\n" +
                    "4. Crystal Dragon\n" +
                    "5. Ethereal Dragon\n" +
                    "6. Frost Drakon\n" +
                    "7. Inferno Drakon\n" +
                    "8. Nature Dragon\n" +
                    "9. Shadow Dragon\n" +
                    "10. Storm Dragon\n" +
                    "11. Venomous Dragon\n\n" +
                    "Complete these tasks, and I will reward you with a Draconis Token and access to my dragon shop!";
            }
        }

        public override object Refuse { get { return "Very well. Return when you are ready to face the dragons."; } }

        public override object Uncomplete { get { return "You haven't completed all the tasks yet. Keep hunting the dragons!"; } }

        public override object Complete { get { return "Excellent work, brave adventurer! You have proven yourself against the mightiest of dragons. My shop is now open to you!"; } }

        public DraconisScorchQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(AncientDragon), "Ancient Dragons", 1));
            AddObjective(new SlayObjective(typeof(BloodDragon), "Blood Dragons", 1));
            AddObjective(new SlayObjective(typeof(CelestialDragon), "Celestial Dragons", 1));
            AddObjective(new SlayObjective(typeof(CrystalDragon), "Crystal Dragons", 1));
            AddObjective(new SlayObjective(typeof(EtherealDragon), "Ethereal Dragons", 1));
            AddObjective(new SlayObjective(typeof(FrostDrakon), "Frost Drakons", 1));
            AddObjective(new SlayObjective(typeof(InfernoDrakon), "Inferno Drakons", 1));
            AddObjective(new SlayObjective(typeof(NatureDragon), "Nature Dragons", 1));
            AddObjective(new SlayObjective(typeof(ShadowDragon), "Shadow Dragons", 1));
            AddObjective(new SlayObjective(typeof(StormDragon), "Storm Dragons", 1));
            AddObjective(new SlayObjective(typeof(VenomousDragon), "Venomous Dragons", 1));

            AddReward(new BaseReward(typeof(Gold), 1500, "1500 Gold"));
            AddReward(new BaseReward(typeof(DraconisToken), 1, "Draconis Token"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations on defeating the dragons!");
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


    public class DraconisScorch : MondainQuester
    {
        public override bool IsActiveVendor { get { return true; } }

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBDraconisScorch());
        }

        [Constructable]
        public DraconisScorch()
            : base("Draconis Scorch", "Master of Dragons")
        {
        }

        public DraconisScorch(Serial serial)
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
                Item token = player.Backpack.FindItemByType(typeof(DraconisToken));
				Item masterToken = player.Backpack.FindItemByType(typeof(MasterToken));

                if (token != null || masterToken != null)
                {
                    SayTo(from, "Welcome to my dragon shop!");
                    base.VendorBuy(from);
                }
                else
                {
                    SayTo(from, "You must have a Draconis Token to access my shop.");
                }
            }
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(DraconisScorchQuest)
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


    public class SBDraconisScorch : SBInfo
    {
        private System.Collections.Generic.List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public SBDraconisScorch()
        {
        }

        public override System.Collections.Generic.List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }
        public override IShopSellInfo SellInfo { get { return m_SellInfo; } }

        public class InternalBuyInfo : System.Collections.Generic.List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                // Add the dragon-themed creatures here
                Add(new AnimalBuyInfo(1, typeof(AncientDragon), 10000, 10, 59, 0));
                Add(new AnimalBuyInfo(1, typeof(BloodDragon), 12000, 10, 59, 0));
                Add(new AnimalBuyInfo(1, typeof(CelestialDragon), 13000, 10, 59, 0));
                Add(new AnimalBuyInfo(1, typeof(CrystalDragon), 11000, 10, 59, 0));
                Add(new AnimalBuyInfo(1, typeof(EtherealDragon), 14000, 10, 59, 0));
                Add(new AnimalBuyInfo(1, typeof(FrostDrakon), 15000, 10, 59, 0));
                Add(new AnimalBuyInfo(1, typeof(InfernoDrakon), 16000, 10, 59, 0));
                Add(new AnimalBuyInfo(1, typeof(NatureDragon), 17000, 10, 59, 0));
                Add(new AnimalBuyInfo(1, typeof(ShadowDragon), 18000, 10, 59, 0));
                Add(new AnimalBuyInfo(1, typeof(StormDragon), 19000, 10, 59, 0));
                Add(new AnimalBuyInfo(1, typeof(VenomousDragon), 20000, 10, 59, 0));
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
