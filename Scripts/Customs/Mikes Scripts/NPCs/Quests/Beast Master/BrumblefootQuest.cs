using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class BrumblefootQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Brumblefoot the Beastly's Ettin Hunt"; } }

        public override object Description
        {
            get
            {
                return 
                    "Greetings, adventurer! I am Brumblefoot the Beastly, and I have a unique " +
                    "challenge for you. My fascination with Ettins has led me to believe that " +
                    "they are the most misunderstood creatures in all the lands. To prove their worth, " +
                    "you must hunt down these monstrous beasts and bring me their tokens. Complete these tasks:\n\n" +
                    "1. Slay a Cerebral Ettin.\n" +
                    "2. Defeat an Earthquake Ettin.\n" +
                    "3. Conquer a Flamewarden Ettin.\n" +
                    "4. Overcome a Frostwarden Ettin.\n" +
                    "5. Destroy an Illusionist Ettin.\n" +
                    "6. Vanquish a Necro Ettin.\n" +
                    "7. Subdue a Stormcaller Ettin.\n" +
                    "8. Eradicate a Tidal Ettin.\n" +
                    "9. Defeat a Twin Terror Ettin.\n" +
                    "10. Exterminate a Venomous Ettin.\n\n" +
                    "Complete these tasks, and I will grant you access to my exclusive Ettin shop!";
            }
        }

        public override object Refuse { get { return "Very well. Return when you are ready to face the Ettin challenge."; } }

        public override object Uncomplete { get { return "You haven't completed all the tasks yet. Keep hunting those Ettins!"; } }

        public override object Complete { get { return "Fantastic! You have proven the worth of the Ettins. My shop is now open to you!"; } }

        public BrumblefootQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(CerebralEttin), "Cerebral Ettins", 1));
            AddObjective(new SlayObjective(typeof(EarthquakeEttin), "Earthquake Ettins", 1));
            AddObjective(new SlayObjective(typeof(FlameWardenEttin), "Flamewarden Ettins", 1));
            AddObjective(new SlayObjective(typeof(FrostWardenEttin), "Frostwarden Ettins", 1));
            AddObjective(new SlayObjective(typeof(IllusionistEttin), "Illusionist Ettins", 1));
            AddObjective(new SlayObjective(typeof(NecroEttin), "Necro Ettins", 1));
            AddObjective(new SlayObjective(typeof(StormcallerEttin), "Stormcaller Ettins", 1));
            AddObjective(new SlayObjective(typeof(TidalEttin), "Tidal Ettins", 1));
            AddObjective(new SlayObjective(typeof(TwinTerrorEttin), "Twin Terror Ettins", 1));
            AddObjective(new SlayObjective(typeof(VenomousEttin), "Venomous Ettins", 1));

            AddReward(new BaseReward(typeof(Gold), 1500, "1500 Gold"));
            AddReward(new BaseReward(typeof(BeastlyToken), 1, "Beastly Token"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You have completed Brumblefoot the Beastly's challenge!");
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

    public class BrumblefootTheBeastly : MondainQuester
    {
        public override bool IsActiveVendor { get { return true; } }

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBBrumblefootTheBeastly());
        }

        [Constructable]
        public BrumblefootTheBeastly()
            : base("Brumblefoot the Beastly", "Master of the Ettins")
        {
        }

        public BrumblefootTheBeastly(Serial serial)
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
                Item token = player.Backpack.FindItemByType(typeof(BeastlyToken));
				Item masterToken = player.Backpack.FindItemByType(typeof(MasterToken));

                if (token != null || masterToken != null)
                {
                    SayTo(from, "Welcome to my Ettin beast shop!");
                    base.VendorBuy(from);
                }
                else
                {
                    SayTo(from, "You must have a Beastly Token to access my shop.");
                }
            }
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(BrumblefootQuest)
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

    public class SBBrumblefootTheBeastly : SBInfo
    {
        private System.Collections.Generic.List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public SBBrumblefootTheBeastly()
        {
        }

        public override System.Collections.Generic.List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }
        public override IShopSellInfo SellInfo { get { return m_SellInfo; } }

        public class InternalBuyInfo : System.Collections.Generic.List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                // Add the Ettin-themed creatures here
                Add(new AnimalBuyInfo(1, typeof(CerebralEttin), 1000, 10, 2, 0));
                Add(new AnimalBuyInfo(1, typeof(EarthquakeEttin), 1200, 10, 2, 0));
                Add(new AnimalBuyInfo(1, typeof(FlameWardenEttin), 1100, 10, 2, 0));
                Add(new AnimalBuyInfo(1, typeof(FrostWardenEttin), 1150, 10, 2, 0));
                Add(new AnimalBuyInfo(1, typeof(IllusionistEttin), 1050, 10, 2, 0));
                Add(new AnimalBuyInfo(1, typeof(NecroEttin), 1300, 10, 2, 0));
                Add(new AnimalBuyInfo(1, typeof(StormcallerEttin), 1400, 10, 2, 0));
                Add(new AnimalBuyInfo(1, typeof(TidalEttin), 1250, 10, 2, 0));
                Add(new AnimalBuyInfo(1, typeof(TwinTerrorEttin), 1500, 10, 2, 0));
                Add(new AnimalBuyInfo(1, typeof(VenomousEttin), 1350, 10, 2, 0));
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
