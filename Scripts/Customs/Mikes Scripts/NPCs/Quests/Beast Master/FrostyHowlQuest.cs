using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class FrostyHowlQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Frosty Howl's Lupine Challenge"; } }

        public override object Description
        {
            get
            {
                return
                    "Greetings, adventurer! I am Frosty Howl, the Lupine Lorekeeper. " +
                    "To gain access to my exclusive shop, you must demonstrate your prowess " +
                    "by hunting the most legendary wolves. Here are the beasts you need to defeat:\n\n" +
                    "1. Ancient Wolf\n" +
                    "2. Celestial Wolf\n" +
                    "3. Cursed Wolf\n" +
                    "4. Earthquake Wolf\n" +
                    "5. Ember Wolf\n" +
                    "6. Frostbite Wolf\n" +
                    "7. Gloom Wolf\n" +
                    "8. Shadow Prowler\n" +
                    "9. Storm Wolf\n" +
                    "10. Venomous Wolf\n\n" +
                    "Bring me proof of your victories, and I will grant you access to my special shop!";
            }
        }

        public override object Refuse { get { return "Very well. Return when you are ready to hunt the legendary wolves."; } }

        public override object Uncomplete { get { return "You haven't completed all the tasks yet. Keep hunting those legendary wolves!"; } }

        public override object Complete { get { return "Impressive! You have proven yourself a true hunter of wolves. My shop is now open to you!"; } }

        public FrostyHowlQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(AncientWolf), "Ancient Wolves", 1));
            AddObjective(new SlayObjective(typeof(CelestialWolf), "Celestial Wolves", 1));
            AddObjective(new SlayObjective(typeof(CursedWolf), "Cursed Wolves", 1));
            AddObjective(new SlayObjective(typeof(EarthquakeWolf), "Earthquake Wolves", 1));
            AddObjective(new SlayObjective(typeof(EmberWolf), "Ember Wolves", 1));
            AddObjective(new SlayObjective(typeof(FrostbiteWolf), "Frostbite Wolves", 1));
            AddObjective(new SlayObjective(typeof(GloomWolf), "Gloom Wolves", 1));
            AddObjective(new SlayObjective(typeof(ShadowProwler), "Shadow Prowlers", 1));
            AddObjective(new SlayObjective(typeof(StormWolf), "Storm Wolves", 1));
            AddObjective(new SlayObjective(typeof(VenomousWolf), "Venomous Wolves", 1));

            AddReward(new BaseReward(typeof(Gold), 1000, "1000 Gold"));
            AddReward(new BaseReward(typeof(LupineToken), 1, "Lupine Token"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You have completed Frosty Howl's challenge!");
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

    public class FrostyHowl : MondainQuester
    {
        public override bool IsActiveVendor { get { return true; } }

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBFrostyHowl());
        }

        [Constructable]
        public FrostyHowl()
            : base("Frosty Howl", "Lupine Lorekeeper")
        {
        }

        public FrostyHowl(Serial serial)
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
                Item token = player.Backpack.FindItemByType(typeof(LupineToken));
				Item masterToken = player.Backpack.FindItemByType(typeof(MasterToken));

                if (token != null || masterToken != null)
                {
                    SayTo(from, "Welcome to my lupine beast shop!");
                    base.VendorBuy(from);
                }
                else
                {
                    SayTo(from, "You must have a Lupine Token to access my shop.");
                }
            }
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(FrostyHowlQuest)
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

    public class SBFrostyHowl : SBInfo
    {
        private System.Collections.Generic.List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public SBFrostyHowl()
        {
        }

        public override System.Collections.Generic.List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }
        public override IShopSellInfo SellInfo { get { return m_SellInfo; } }

        public class InternalBuyInfo : System.Collections.Generic.List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                // Add the wolf-themed creatures here
                Add(new AnimalBuyInfo(1, typeof(AncientWolf), 1000, 10, 225, 0));
                Add(new AnimalBuyInfo(1, typeof(CelestialWolf), 1200, 10, 225, 0));
                Add(new AnimalBuyInfo(1, typeof(CursedWolf), 1100, 10, 225, 0));
                Add(new AnimalBuyInfo(1, typeof(EarthquakeWolf), 1300, 10, 225, 0));
                Add(new AnimalBuyInfo(1, typeof(EmberWolf), 1400, 10, 225, 0));
                Add(new AnimalBuyInfo(1, typeof(FrostbiteWolf), 1500, 10, 225, 0));
                Add(new AnimalBuyInfo(1, typeof(GloomWolf), 1600, 10, 225, 0));
                Add(new AnimalBuyInfo(1, typeof(ShadowProwler), 1700, 10, 225, 0));
                Add(new AnimalBuyInfo(1, typeof(StormWolf), 1800, 10, 225, 0));
                Add(new AnimalBuyInfo(1, typeof(VenomousWolf), 1900, 10, 225, 0));
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
