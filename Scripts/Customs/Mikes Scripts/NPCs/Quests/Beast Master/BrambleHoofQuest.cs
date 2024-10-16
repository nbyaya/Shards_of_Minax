using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class BrambleHoofQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Bramble Hoof's Rare Steed Hunt"; } }

        public override object Description
        {
            get
            {
                return 
                    "Greetings, brave adventurer! I am Bramble Hoof, the connoisseur of rare and elusive steeds. " +
                    "To prove your worth and gain access to my unique stable, you must embark on a quest to " +
                    "collect the following legendary steeds for me:\n\n" +
                    "1. Slay an Inferno Stallion.\n" +
                    "2. Defeat an Iron Steed.\n" +
                    "3. Conquer a Metallic Windsteed.\n" +
                    "4. Overcome a Stone Steed.\n" +
                    "5. Destroy a Tidal Mare.\n" +
                    "6. Vanquish a Volcanic Charger.\n" +
                    "7. Subdue a Woodland Charger.\n" +
                    "8. Eradicate a Woodland Spirit Horse.\n" +
                    "9. Defeat a Yang Stallion.\n" +
                    "10. Exterminate a Yin Steed.\n\n" +
                    "Once you have gathered all these creatures, I shall open my unique stable to you!";
            }
        }

        public override object Refuse { get { return "Very well. Return when you are ready to hunt for these rare steeds."; } }

        public override object Uncomplete { get { return "You haven't completed all the tasks yet. Keep seeking these legendary steeds!"; } }

        public override object Complete { get { return "Fantastic! You have proven yourself worthy. My stable of rare steeds is now open to you!"; } }

        public BrambleHoofQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(InfernoStallion), "Inferno Stallions", 1));
            AddObjective(new SlayObjective(typeof(IronSteed), "Iron Steeds", 1));
            AddObjective(new SlayObjective(typeof(MetallicWindsteed), "Metallic Windsteeds", 1));
            AddObjective(new SlayObjective(typeof(StoneSteed), "Stone Steeds", 1));
            AddObjective(new SlayObjective(typeof(TidalMare), "Tidal Mares", 1));
            AddObjective(new SlayObjective(typeof(VolcanicCharger), "Volcanic Chargers", 1));
            AddObjective(new SlayObjective(typeof(WoodlandCharger), "Woodland Chargers", 1));
            AddObjective(new SlayObjective(typeof(WoodlandSpiritHorse), "Woodland Spirit Horses", 1));
            AddObjective(new SlayObjective(typeof(YangStallion), "Yang Stallions", 1));
            AddObjective(new SlayObjective(typeof(YinSteed), "Yin Steeds", 1));

            AddReward(new BaseReward(typeof(Gold), 1000, "1000 Gold"));
            AddReward(new BaseReward(typeof(BrambleToken), 1, "Bramble Token"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You have completed Bramble Hoof's challenge!");
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


    public class BrambleHoof : MondainQuester
    {
        public override bool IsActiveVendor { get { return true; } }

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBBrambleHoof());
        }

        [Constructable]
        public BrambleHoof()
            : base("Bramble Hoof", "Master of Rare Steeds")
        {
        }

        public BrambleHoof(Serial serial)
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
                Item token = player.Backpack.FindItemByType(typeof(BrambleToken));

                if (token != null)
                {
                    SayTo(from, "Welcome to my stable of rare steeds!");
                    base.VendorBuy(from);
                }
                else
                {
                    SayTo(from, "You must have a Bramble Token to access my stable.");
                }
            }
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(BrambleHoofQuest)
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


    public class SBBrambleHoof : SBInfo
    {
        private System.Collections.Generic.List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public SBBrambleHoof()
        {
        }

        public override System.Collections.Generic.List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }
        public override IShopSellInfo SellInfo { get { return m_SellInfo; } }

        public class InternalBuyInfo : System.Collections.Generic.List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                // Add the legendary steeds here
                Add(new AnimalBuyInfo(1, typeof(InfernoStallion), 2000, 10, 13, 0));
                Add(new AnimalBuyInfo(1, typeof(IronSteed), 1800, 10, 13, 0));
                Add(new AnimalBuyInfo(1, typeof(MetallicWindsteed), 2200, 10, 13, 0));
                Add(new AnimalBuyInfo(1, typeof(StoneSteed), 2100, 10, 13, 0));
                Add(new AnimalBuyInfo(1, typeof(TidalMare), 2300, 10, 13, 0));
                Add(new AnimalBuyInfo(1, typeof(VolcanicCharger), 2500, 10, 13, 0));
                Add(new AnimalBuyInfo(1, typeof(WoodlandCharger), 2400, 10, 13, 0));
                Add(new AnimalBuyInfo(1, typeof(WoodlandSpiritHorse), 2600, 10, 13, 0));
                Add(new AnimalBuyInfo(1, typeof(YangStallion), 2700, 10, 13, 0));
                Add(new AnimalBuyInfo(1, typeof(YinSteed), 2800, 10, 13, 0));
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
