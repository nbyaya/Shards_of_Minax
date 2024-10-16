using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;

namespace Server.Engines.Quests
{
    public class GolemGarrisonQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Golem Garrison's Golem Collection"; } }

        public override object Description
        {
            get
            {
                return 
                    "Greetings, brave adventurer! I am Golem Garrison, the golem enthusiast. " +
                    "To prove your worth and gain access to my unique shop, you must assist me " +
                    "in gathering golem samples. Complete these tasks:\n\n" +
                    "1. Slay a Bone Golem.\n" +
                    "2. Defeat a Cheese Golem.\n" +
                    "3. Conquer a Crystal Golem.\n" +
                    "4. Overcome an Iron Golem.\n" +
                    "5. Destroy a Meat Golem.\n" +
                    "6. Vanquish a Muck Golem.\n" +
                    "7. Subdue a Sand Golem.\n" +
                    "8. Eradicate a Shadow Golem.\n" +
                    "9. Defeat a Stone Golem.\n" +
                    "10. Exterminate a Wood Golem.\n\n" +
                    "Complete these tasks, and my exclusive golem shop will be available to you!";
            }
        }

        public override object Refuse { get { return "Very well. Return when you are ready to assist me with my golem collection."; } }

        public override object Uncomplete { get { return "You haven't completed all the tasks yet. Keep hunting those golems!"; } }

        public override object Complete { get { return "Fantastic work! You have proven yourself a true golem hunter. My shop is now open to you!"; } }

        public GolemGarrisonQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(BoneGolem), "Bone Golems", 1));
            AddObjective(new SlayObjective(typeof(CheeseGolem), "Cheese Golems", 1));
            AddObjective(new SlayObjective(typeof(CrystalGolem), "Crystal Golems", 1));
            AddObjective(new SlayObjective(typeof(IronGolem), "Iron Golems", 1));
            AddObjective(new SlayObjective(typeof(MeatGolem), "Meat Golems", 1));
            AddObjective(new SlayObjective(typeof(MuckGolem), "Muck Golems", 1));
            AddObjective(new SlayObjective(typeof(SandGolem), "Sand Golems", 1));
            AddObjective(new SlayObjective(typeof(ShadowGolem), "Shadow Golems", 1));
            AddObjective(new SlayObjective(typeof(StoneGolem), "Stone Golems", 1));
            AddObjective(new SlayObjective(typeof(WoodGolem), "Wood Golems", 1));

            AddReward(new BaseReward(typeof(Gold), 1000, "1000 Gold"));
            AddReward(new BaseReward(typeof(GolemToken), 1, "Golem Token"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed Golem Garrison's quest!");
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

    public class GolemGarrison : MondainQuester
    {
        public override bool IsActiveVendor { get { return true; } }

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBGolemGarrison());
        }

        [Constructable]
        public GolemGarrison()
            : base("Golem Garrison", "Master of Golems")
        {
        }

        public GolemGarrison(Serial serial)
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
                Item token = player.Backpack.FindItemByType(typeof(GolemToken));
				Item masterToken = player.Backpack.FindItemByType(typeof(MasterToken));

                if (token != null || masterToken != null)
                {
                    SayTo(from, "Welcome to my golem shop!");
                    base.VendorBuy(from);
                }
                else
                {
                    SayTo(from, "You must have a Golem Token to access my shop.");
                }
            }
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(GolemGarrisonQuest)
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
	
    public class SBGolemGarrison : SBInfo
    {
        private System.Collections.Generic.List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public SBGolemGarrison()
        {
        }

        public override System.Collections.Generic.List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }
        public override IShopSellInfo SellInfo { get { return m_SellInfo; } }

        public class InternalBuyInfo : System.Collections.Generic.List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                // Add the golem-themed creatures here
                Add(new AnimalBuyInfo(1, typeof(BoneGolem), 1000, 10, 752, 0));
                Add(new AnimalBuyInfo(1, typeof(CheeseGolem), 1200, 10, 752, 0));
                Add(new AnimalBuyInfo(1, typeof(CrystalGolem), 1500, 10, 752, 0));
                Add(new AnimalBuyInfo(1, typeof(IronGolem), 1800, 10, 752, 0));
                Add(new AnimalBuyInfo(1, typeof(MeatGolem), 1100, 10, 752, 0));
                Add(new AnimalBuyInfo(1, typeof(MuckGolem), 1300, 10, 752, 0));
                Add(new AnimalBuyInfo(1, typeof(SandGolem), 1400, 10, 752, 0));
                Add(new AnimalBuyInfo(1, typeof(ShadowGolem), 1600, 10, 752, 0));
                Add(new AnimalBuyInfo(1, typeof(StoneGolem), 1700, 10, 752, 0));
                Add(new AnimalBuyInfo(1, typeof(WoodGolem), 1500, 10, 752, 0));
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


