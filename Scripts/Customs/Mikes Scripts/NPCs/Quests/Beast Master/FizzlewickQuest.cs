using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class FizzlewickQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Fizzlewick the Faerie Fancier's Fanciful Quest"; } }

        public override object Description
        {
            get
            {
                return 
                    "Greetings, brave adventurer! I am Fizzlewick the Faerie Fancier, and I have a rather whimsical task for you. " +
                    "To prove your mettle and gain access to my exclusive shop of mystical beasts, you must embark on a quest to " +
                    "bring me tokens from some of the most elusive creatures in the realm. Complete these tasks:\n\n" +
                    "1. Slay a Banshee.\n" +
                    "2. Defeat a Chaneque.\n" +
                    "3. Conquer a Dryad.\n" +
                    "4. Overcome a Fairy.\n" +
                    "5. Destroy a Leprechaun.\n" +
                    "6. Vanquish a Nymph.\n" +
                    "7. Subdue a Puck.\n" +
                    "8. Eradicate a Selkie.\n" +
                    "9. Defeat a Sidhe.\n" +
                    "10. Exterminate a Will-O-The-Wisp.\n\n" +
                    "Complete these tasks, and I shall reward you with access to my delightful shop!";
            }
        }

        public override object Refuse { get { return "Very well. Return when you are ready to face these whimsical creatures."; } }

        public override object Uncomplete { get { return "You haven't completed all the tasks yet. Continue your hunt for the fae and phantoms!"; } }

        public override object Complete { get { return "Marvelous! You have proven your bravery and wit. My shop is now open to you!"; } }

        public FizzlewickQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(Banshee), "Banshees", 1));
            AddObjective(new SlayObjective(typeof(Chaneque), "Chaneques", 1));
            AddObjective(new SlayObjective(typeof(Dryad), "Dryads", 1));
            AddObjective(new SlayObjective(typeof(Fairy), "Fairies", 1));
            AddObjective(new SlayObjective(typeof(Leprechaun), "Leprechauns", 1));
            AddObjective(new SlayObjective(typeof(Nymph), "Nymphs", 1));
            AddObjective(new SlayObjective(typeof(Puck), "Pucks", 1));
            AddObjective(new SlayObjective(typeof(Selkie), "Selkies", 1));
            AddObjective(new SlayObjective(typeof(Sidhe), "Sidhes", 1));
            AddObjective(new SlayObjective(typeof(WillOTheWisp), "Will-O-The-Wisps", 1));

            AddReward(new BaseReward(typeof(Gold), 1000, "1000 Gold"));
            AddReward(new BaseReward(typeof(FizzlewickToken), 1, "Fizzlewick Token"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed Fizzlewick's whimsical challenge!");
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

    public class Fizzlewick : MondainQuester
    {
        public override bool IsActiveVendor { get { return true; } }

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBFizzlewick());
        }

        [Constructable]
        public Fizzlewick()
            : base("Fizzlewick the Faerie Fancier", "Master of Whimsical Beasts")
        {
        }

        public Fizzlewick(Serial serial)
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
                Item token = player.Backpack.FindItemByType(typeof(FizzlewickToken));
				Item masterToken = player.Backpack.FindItemByType(typeof(MasterToken));

                if (token != null || masterToken != null)
                {
                    SayTo(from, "Welcome to my shop of whimsical creatures!");
                    base.VendorBuy(from);
                }
                else
                {
                    SayTo(from, "You must have a Fizzlewick Token to access my shop.");
                }
            }
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(FizzlewickQuest)
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

    public class SBFizzlewick : SBInfo
    {
        private System.Collections.Generic.List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public SBFizzlewick()
        {
        }

        public override System.Collections.Generic.List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }
        public override IShopSellInfo SellInfo { get { return m_SellInfo; } }

        public class InternalBuyInfo : System.Collections.Generic.List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                // Add the whimsical-themed creatures here
                Add(new AnimalBuyInfo(1, typeof(Banshee), 1000, 10, 176, 0));
                Add(new AnimalBuyInfo(1, typeof(Chaneque), 1200, 10, 176, 0));
                Add(new AnimalBuyInfo(1, typeof(Dryad), 900, 10, 176, 0));
                Add(new AnimalBuyInfo(1, typeof(Fairy), 950, 10, 176, 0));
                Add(new AnimalBuyInfo(1, typeof(Leprechaun), 1100, 10, 176, 0));
                Add(new AnimalBuyInfo(1, typeof(Nymph), 1300, 10, 176, 0));
                Add(new AnimalBuyInfo(1, typeof(Puck), 1400, 10, 176, 0));
                Add(new AnimalBuyInfo(1, typeof(Selkie), 1250, 10, 176, 0));
                Add(new AnimalBuyInfo(1, typeof(Sidhe), 1500, 10, 176, 0));
                Add(new AnimalBuyInfo(1, typeof(WillOTheWisp), 1350, 10, 176, 0));
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
