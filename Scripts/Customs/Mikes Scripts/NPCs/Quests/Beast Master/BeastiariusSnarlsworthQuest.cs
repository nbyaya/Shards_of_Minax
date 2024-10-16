using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class BeastiariusSnarlsworthQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Dr. Beastiarius Snarlsworth's Menacing Menagerie"; } }

        public override object Description
        {
            get
            {
                return 
                    "Greetings, brave adventurer! I am Dr. Beastiarius Snarlsworth, " +
                    "and I seek the most formidable and intriguing beasts for my collection. " +
                    "To gain access to my unique beast shop, you must defeat the following creatures:\n\n" +
                    "1. Abyssal Tide\n" +
                    "2. Azure Mirage\n" +
                    "3. Coral Sentinel\n" +
                    "4. Frost Warden\n" +
                    "5. Hydrokinetic Warden\n" +
                    "6. Mire Spawner\n" +
                    "7. Steam Leviathan\n" +
                    "8. Stormcaller\n" +
                    "9. Tsunami Titan\n" +
                    "10. Vortex Wraith\n\n" +
                    "Show me you have conquered these beasts, and you shall gain access to my exclusive shop!";
            }
        }

        public override object Refuse { get { return "Very well. Return when you are ready to face these beasts."; } }

        public override object Uncomplete { get { return "You haven't completed all the tasks yet. Continue hunting these beasts!"; } }

        public override object Complete { get { return "Remarkable! You have proven yourself worthy. My shop is now open to you!"; } }

        public BeastiariusSnarlsworthQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(AbyssalTide), "Abyssal Tides", 1));
            AddObjective(new SlayObjective(typeof(AzureMirage), "Azure Mirages", 1));
            AddObjective(new SlayObjective(typeof(CoralSentinel), "Coral Sentinels", 1));
            AddObjective(new SlayObjective(typeof(FrostWarden), "Frost Wardens", 1));
            AddObjective(new SlayObjective(typeof(HydrokineticWarden), "Hydrokinetic Wardens", 1));
            AddObjective(new SlayObjective(typeof(MireSpawner), "Mire Spawners", 1));
            AddObjective(new SlayObjective(typeof(SteamLeviathan), "Steam Leviathans", 1));
            AddObjective(new SlayObjective(typeof(Stormcaller), "Stormcallers", 1));
            AddObjective(new SlayObjective(typeof(TsunamiTitan), "Tsunami Titans", 1));
            AddObjective(new SlayObjective(typeof(VortexWraith), "Vortex Wraiths", 1));

            AddReward(new BaseReward(typeof(Gold), 1000, "1000 Gold"));
            AddReward(new BaseReward(typeof(BeastiariusToken), 1, "Beastiarius Token"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You have completed Dr. Beastiarius Snarlsworth's challenge!");
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

    public class DrBeastiariusSnarlsworth : MondainQuester
    {
        public override bool IsActiveVendor { get { return true; } }

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBDrBeastiariusSnarlsworth());
        }

        [Constructable]
        public DrBeastiariusSnarlsworth()
            : base("Dr. Beastiarius Snarlsworth", "Master of Menacing Beasts")
        {
        }

        public DrBeastiariusSnarlsworth(Serial serial)
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
                Item token = player.Backpack.FindItemByType(typeof(BeastiariusToken));
				Item masterToken = player.Backpack.FindItemByType(typeof(MasterToken));

                if (token != null || masterToken != null)
                {
                    SayTo(from, "Welcome to my menacing beast shop!");
                    base.VendorBuy(from);
                }
                else
                {
                    SayTo(from, "You must have a Beastiarius Token to access my shop.");
                }
            }
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(BeastiariusSnarlsworthQuest)
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
	
    public class SBDrBeastiariusSnarlsworth : SBInfo
    {
        private System.Collections.Generic.List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public SBDrBeastiariusSnarlsworth()
        {
        }

        public override System.Collections.Generic.List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }
        public override IShopSellInfo SellInfo { get { return m_SellInfo; } }

        public class InternalBuyInfo : System.Collections.Generic.List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                // Add the beast-themed creatures here
                Add(new AnimalBuyInfo(1, typeof(AbyssalTide), 1000, 10, 16, 0));
                Add(new AnimalBuyInfo(1, typeof(AzureMirage), 1200, 10, 16, 0));
                Add(new AnimalBuyInfo(1, typeof(CoralSentinel), 900, 10, 16, 0));
                Add(new AnimalBuyInfo(1, typeof(FrostWarden), 950, 10, 16, 0));
                Add(new AnimalBuyInfo(1, typeof(HydrokineticWarden), 1100, 10, 16, 0));
                Add(new AnimalBuyInfo(1, typeof(MireSpawner), 1300, 10, 16, 0));
                Add(new AnimalBuyInfo(1, typeof(SteamLeviathan), 1400, 10, 16, 0));
                Add(new AnimalBuyInfo(1, typeof(Stormcaller), 1250, 10, 16, 0));
                Add(new AnimalBuyInfo(1, typeof(TsunamiTitan), 1500, 10, 16, 0));
                Add(new AnimalBuyInfo(1, typeof(VortexWraith), 1350, 10, 16, 0));
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
