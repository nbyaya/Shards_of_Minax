using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class FarmerMoolinQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Farmer Moolin's Beastly Challenge"; } }

        public override object Description
        {
            get
            {
                return 
                    "Howdy, partner! I'm Farmer Moolin', and I've got a mighty task for ya. " +
                    "My farm animals have turned into monstrous beasts, and I need your help to " +
                    "round 'em up. Complete these tasks and earn access to my unique beast shop:\n\n" +
                    "1. Slay an Angus Berserker.\n" +
                    "2. Defeat a Bison Brute.\n" +
                    "3. Conquer a Dairy Wraith.\n" +
                    "4. Overcome a Guernsey Guardian.\n" +
                    "5. Destroy a Hereford Warlock.\n" +
                    "6. Vanquish a Highland Bull.\n" +
                    "7. Subdue a Jersey Enchantress.\n" +
                    "8. Eradicate a Milking Demon.\n" +
                    "9. Defeat a Sahiwal Shaman.\n" +
                    "10. Exterminate a Zebu Zealot.\n\n" +
                    "Complete these tasks, and I'll open my shop just for you!";
            }
        }

        public override object Refuse { get { return "Well, if you're not ready, I'll be here when you are!"; } }

        public override object Uncomplete { get { return "You haven't finished all the tasks yet. Keep going, cowboy!"; } }

        public override object Complete { get { return "Well done! You've managed to subdue my beasts. My shop is now open to you!"; } }

        public FarmerMoolinQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(AngusBerserker), "Angus Berserkers", 1));
            AddObjective(new SlayObjective(typeof(BisonBrute), "Bison Brutes", 1));
            AddObjective(new SlayObjective(typeof(DairyWraith), "Dairy Wraiths", 1));
            AddObjective(new SlayObjective(typeof(GuernseyGuardian), "Guernsey Guardians", 1));
            AddObjective(new SlayObjective(typeof(HerefordWarlock), "Hereford Warlocks", 1));
            AddObjective(new SlayObjective(typeof(HighlandBull), "Highland Bulls", 1));
            AddObjective(new SlayObjective(typeof(JerseyEnchantress), "Jersey Enchantresses", 1));
            AddObjective(new SlayObjective(typeof(MilkingDemon), "Milking Demons", 1));
            AddObjective(new SlayObjective(typeof(SahiwalShaman), "Sahiwal Shamans", 1));
            AddObjective(new SlayObjective(typeof(ZebuZealot), "Zebu Zealots", 1));

            AddReward(new BaseReward(typeof(Gold), 1000, "1000 Gold"));
            AddReward(new BaseReward(typeof(MoolinToken), 1, "Moolin Token"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've done it! Farmer Moolin's beast shop is now open to you!");
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

    public class FarmerMoolin : MondainQuester
    {
        public override bool IsActiveVendor { get { return true; } }

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBFarmerMoolin());
        }

        [Constructable]
        public FarmerMoolin()
            : base("Farmer Moolin", "Master of Beasts and Dairy")
        {
        }

        public FarmerMoolin(Serial serial)
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
                Item token = player.Backpack.FindItemByType(typeof(MoolinToken));
				Item masterToken = player.Backpack.FindItemByType(typeof(MasterToken));

                if (token != null || masterToken != null)
                {
                    SayTo(from, "Welcome to my beast shop! Have a look at what I have for sale.");
                    base.VendorBuy(from);
                }
                else
                {
                    SayTo(from, "You need a Moolin Token to access my shop.");
                }
            }
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(FarmerMoolinQuest)
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

    public class SBFarmerMoolin : SBInfo
    {
        private System.Collections.Generic.List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public SBFarmerMoolin()
        {
        }

        public override System.Collections.Generic.List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }
        public override IShopSellInfo SellInfo { get { return m_SellInfo; } }

        public class InternalBuyInfo : System.Collections.Generic.List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                // Add the beast-themed creatures here
                Add(new AnimalBuyInfo(1, typeof(AngusBerserker), 1000, 10, 216, 0));
                Add(new AnimalBuyInfo(1, typeof(BisonBrute), 1200, 10, 216, 0));
                Add(new AnimalBuyInfo(1, typeof(DairyWraith), 900, 10, 216, 0));
                Add(new AnimalBuyInfo(1, typeof(GuernseyGuardian), 950, 10, 216, 0));
                Add(new AnimalBuyInfo(1, typeof(HerefordWarlock), 1100, 10, 216, 0));
                Add(new AnimalBuyInfo(1, typeof(HighlandBull), 1300, 10, 216, 0));
                Add(new AnimalBuyInfo(1, typeof(JerseyEnchantress), 1400, 10, 216, 0));
                Add(new AnimalBuyInfo(1, typeof(MilkingDemon), 1250, 10, 216, 0));
                Add(new AnimalBuyInfo(1, typeof(SahiwalShaman), 1500, 10, 216, 0));
                Add(new AnimalBuyInfo(1, typeof(ZebuZealot), 1350, 10, 216, 0));
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
