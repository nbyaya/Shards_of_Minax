using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class GatorMcScalesQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Gator McScales' Swamp Safari"; } }

        public override object Description
        {
            get
            {
                return 
                    "Howdy, adventurer! I'm Gator McScales, the swamp's premier alligator enthusiast. " +
                    "To prove your skills and gain access to my exclusive shop, you must show your prowess " +
                    "by hunting down the mightiest gators of the swamp. Complete these tasks:\n\n" +
                    "1. Slay an Acidic Alligator.\n" +
                    "2. Defeat an Ancient Alligator.\n" +
                    "3. Conquer a Firebreath Alligator.\n" +
                    "4. Overcome a Frostbite Alligator.\n" +
                    "5. Destroy an Illusionary Alligator.\n" +
                    "6. Vanquish a Raging Alligator.\n" +
                    "7. Subdue a Shadow Alligator.\n" +
                    "8. Eradicate a Storm Alligator.\n" +
                    "9. Defeat a Toxic Alligator.\n" +
                    "10. Exterminate a Venomous Alligator.\n\n" +
                    "Complete these tasks, and I will grant you access to my alligator shop!";
            }
        }

        public override object Refuse { get { return "Alright, come back when you're ready to tackle the swamp's fiercest gators."; } }

        public override object Uncomplete { get { return "You haven't completed all the tasks yet. Keep hunting those gators!"; } }

        public override object Complete { get { return "Well done! You've proven yourself as a true gator hunter. My shop is now open to you!"; } }

        public GatorMcScalesQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(AcidicAlligator), "Acidic Alligators", 1));
            AddObjective(new SlayObjective(typeof(AncientAlligator), "Ancient Alligators", 1));
            AddObjective(new SlayObjective(typeof(FirebreathAlligator), "Firebreath Alligators", 1));
            AddObjective(new SlayObjective(typeof(FrostbiteAlligator), "Frostbite Alligators", 1));
            AddObjective(new SlayObjective(typeof(IllusionaryAlligator), "Illusionary Alligators", 1));
            AddObjective(new SlayObjective(typeof(RagingAlligator), "Raging Alligators", 1));
            AddObjective(new SlayObjective(typeof(ShadowAlligator), "Shadow Alligators", 1));
            AddObjective(new SlayObjective(typeof(StormAlligator), "Storm Alligators", 1));
            AddObjective(new SlayObjective(typeof(ToxicAlligator), "Toxic Alligators", 1));
            AddObjective(new SlayObjective(typeof(VenomousAlligator), "Venomous Alligators", 1));

            AddReward(new BaseReward(typeof(Gold), 1000, "1000 Gold"));
            AddReward(new BaseReward(typeof(GatorToken), 1, "Gator Token"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed Gator McScales' challenge!");
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


    public class GatorMcScales : MondainQuester
    {
        public override bool IsActiveVendor { get { return true; } }

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBGatorMcScales());
        }

        [Constructable]
        public GatorMcScales()
            : base("Gator McScales", "Master of the Swamp Gators")
        {
        }

        public GatorMcScales(Serial serial)
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
                Item token = player.Backpack.FindItemByType(typeof(GatorToken));
				Item masterToken = player.Backpack.FindItemByType(typeof(MasterToken));

                if (token != null || masterToken != null)
                {
                    SayTo(from, "Welcome to my alligator shop!");
                    base.VendorBuy(from);
                }
                else
                {
                    SayTo(from, "You must have a Gator Token to access my shop.");
                }
            }
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(GatorMcScalesQuest)
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


    public class SBGatorMcScales : SBInfo
    {
        private System.Collections.Generic.List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public SBGatorMcScales()
        {
        }

        public override System.Collections.Generic.List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }
        public override IShopSellInfo SellInfo { get { return m_SellInfo; } }

        public class InternalBuyInfo : System.Collections.Generic.List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                // Add the alligator-themed creatures here
                Add(new AnimalBuyInfo(1, typeof(AcidicAlligator), 1000, 10, 13, 0));
                Add(new AnimalBuyInfo(1, typeof(AncientAlligator), 1200, 10, 13, 0));
                Add(new AnimalBuyInfo(1, typeof(FirebreathAlligator), 1300, 10, 13, 0));
                Add(new AnimalBuyInfo(1, typeof(FrostbiteAlligator), 1400, 10, 13, 0));
                Add(new AnimalBuyInfo(1, typeof(IllusionaryAlligator), 1100, 10, 13, 0));
                Add(new AnimalBuyInfo(1, typeof(RagingAlligator), 1500, 10, 13, 0));
                Add(new AnimalBuyInfo(1, typeof(ShadowAlligator), 1600, 10, 13, 0));
                Add(new AnimalBuyInfo(1, typeof(StormAlligator), 1700, 10, 13, 0));
                Add(new AnimalBuyInfo(1, typeof(ToxicAlligator), 1200, 10, 13, 0));
                Add(new AnimalBuyInfo(1, typeof(VenomousAlligator), 1300, 10, 13, 0));
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
