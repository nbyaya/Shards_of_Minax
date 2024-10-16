using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class BeatrixFelineQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Beatrix Feline's Exotic Challenge"; } }

        public override object Description
        {
            get
            {
                return 
                    "Greetings, brave adventurer! I am Beatrix Feline, the eccentric Beast Master. " +
                    "To prove your prowess and gain access to my unique shop, you must hunt and " +
                    "collect tokens from these fantastical beasts:\n\n" +
                    "1. Abyssal Panther\n" +
                    "2. Celestial Horror\n" +
                    "3. Cosmic Stalker\n" +
                    "4. Ethereal Panthra\n" +
                    "5. Nebula Cat\n" +
                    "6. Nightmare Panther\n" +
                    "7. Phantom Panther\n" +
                    "8. Starborn Predator\n" +
                    "9. Void Cat\n\n" +
                    "Complete these tasks, and you will be rewarded with access to my exotic beast shop!";
            }
        }

        public override object Refuse { get { return "Very well. Return when you are ready to prove your skill with these exotic beasts."; } }

        public override object Uncomplete { get { return "You haven't completed all the tasks yet. Keep hunting those exotic beasts!"; } }

        public override object Complete { get { return "Excellent work! You have proven yourself worthy. My shop is now open to you!"; } }

        public BeatrixFelineQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(AbyssalPanther), "Abyssal Panthers", 1));
            AddObjective(new SlayObjective(typeof(CelestialHorror), "Celestial Horrors", 1));
            AddObjective(new SlayObjective(typeof(CosmicStalker), "Cosmic Stalkers", 1));
            AddObjective(new SlayObjective(typeof(EtherealPanthra), "Ethereal Panthras", 1));
            AddObjective(new SlayObjective(typeof(NebulaCat), "Nebula Cats", 1));
            AddObjective(new SlayObjective(typeof(NightmarePanther), "Nightmare Panthers", 1));
            AddObjective(new SlayObjective(typeof(PhantomPanther), "Phantom Panthers", 1));
            AddObjective(new SlayObjective(typeof(StarbornPredator), "Starborn Predators", 1));
            AddObjective(new SlayObjective(typeof(VoidCat), "Void Cats", 1));

            AddReward(new BaseReward(typeof(Gold), 1000, "1000 Gold"));
            AddReward(new BaseReward(typeof(BeatrixToken), 1, "Beatrix Token"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You have completed Beatrix Feline's challenge!");
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


    public class BeatrixFeline : MondainQuester
    {
        public override bool IsActiveVendor { get { return true; } }

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBBeatrixFeline());
        }

        [Constructable]
        public BeatrixFeline()
            : base("Beatrix Feline", "Eccentric Beast Master")
        {
        }

        public BeatrixFeline(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 25);

            Female = true;
            Race = Race.Human;

            Hue = Utility.RandomSkinHue();
            HairItemID = Utility.RandomList(0x203B, 0x2049, 0x2048);
            HairHue = Utility.RandomHairHue();
            FacialHairItemID = -1; // Female NPCs don't have facial hair
        }

        public override void InitOutfit()
        {
            AddItem(new Backpack());
            AddItem(new Boots(Utility.RandomNeutralHue()));
            AddItem(new Skirt(Utility.RandomNeutralHue())); // Using skirt instead of long pants for female
            AddItem(new FancyShirt(Utility.RandomNeutralHue()));
            AddItem(new Cloak(Utility.RandomNeutralHue()));
        }

        public override void VendorBuy(Mobile from)
        {
            PlayerMobile player = from as PlayerMobile;
            if (player != null)
            {
                Item token = player.Backpack.FindItemByType(typeof(BeatrixToken));
				Item masterToken = player.Backpack.FindItemByType(typeof(MasterToken));

                if (token != null || masterToken != null)
                {
                    SayTo(from, "Welcome to my exotic beast shop!");
                    base.VendorBuy(from);
                }
                else
                {
                    SayTo(from, "You must have a Beatrix Token to access my shop.");
                }
            }
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(BeatrixFelineQuest)
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


    public class SBBeatrixFeline : SBInfo
    {
        private System.Collections.Generic.List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public SBBeatrixFeline()
        {
        }

        public override System.Collections.Generic.List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }
        public override IShopSellInfo SellInfo { get { return m_SellInfo; } }

        public class InternalBuyInfo : System.Collections.Generic.List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                // Add the exotic-themed creatures here
                Add(new AnimalBuyInfo(1, typeof(AbyssalPanther), 1000, 10, 201, 0));
                Add(new AnimalBuyInfo(1, typeof(CelestialHorror), 1200, 10, 201, 0));
                Add(new AnimalBuyInfo(1, typeof(CosmicStalker), 1100, 10, 201, 0));
                Add(new AnimalBuyInfo(1, typeof(EtherealPanthra), 1050, 10, 201, 0));
                Add(new AnimalBuyInfo(1, typeof(NebulaCat), 950, 10, 201, 0));
                Add(new AnimalBuyInfo(1, typeof(NightmarePanther), 1300, 10, 201, 0));
                Add(new AnimalBuyInfo(1, typeof(PhantomPanther), 1250, 10, 201, 0));
                Add(new AnimalBuyInfo(1, typeof(StarbornPredator), 1400, 10, 201, 0));
                Add(new AnimalBuyInfo(1, typeof(VoidCat), 1150, 10, 201, 0));
            }
        }

        public class InternalSellInfo : GenericSellInfo
        {
            public InternalSellInfo()
            {
            }
        }
    }

    // Custom Token Item
    public class BeatrixToken : Item
    {
        [Constructable]
        public BeatrixToken() : base(0x2D11) // A unique item ID for the token
        {
            Name = "Beatrix Token";
            Weight = 1.0;
        }

        public BeatrixToken(Serial serial) : base(serial)
        {
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
}
