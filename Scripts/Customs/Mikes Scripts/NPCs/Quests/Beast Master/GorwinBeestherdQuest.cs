using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class GorwinBeestherdQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Gorwin Beestherd's Beastly Bounty"; } }

        public override object Description
        {
            get
            {
                return 
                    "Greetings, adventurer! I am Gorwin Beestherd, master of the wild and whimsical. " +
                    "To gain access to my extraordinary shop of exotic creatures, you must hunt down the " +
                    "following rare beasts and present their tokens to me:\n\n" +
                    "1. Slay a BeardedGoat.\n" +
                    "2. Defeat a Chamois.\n" +
                    "3. Conquer a CliffGoat.\n" +
                    "4. Overcome a DallSheep.\n" +
                    "5. Destroy a FaintingGoat.\n" +
                    "6. Vanquish a Goral.\n" +
                    "7. Subdue an Ibex.\n" +
                    "8. Eradicate a Markhor.\n" +
                    "9. Defeat a SableAntelope.\n" +
                    "10. Exterminate a Tahr.\n\n" +
                    "Complete these tasks, and I will grant you access to my unique beast shop!";
            }
        }

        public override object Refuse { get { return "Very well. Return when you are ready to face the beasts."; } }

        public override object Uncomplete { get { return "You haven't completed all the tasks yet. Keep hunting!"; } }

        public override object Complete { get { return "Splendid! You have proven yourself a true beast hunter. My shop is now open to you!"; } }

        public GorwinBeestherdQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(BeardedGoat), "BeardedGoats", 1));
            AddObjective(new SlayObjective(typeof(Chamois), "Chamois", 1));
            AddObjective(new SlayObjective(typeof(CliffGoat), "CliffGoats", 1));
            AddObjective(new SlayObjective(typeof(DallSheep), "DallSheeps", 1));
            AddObjective(new SlayObjective(typeof(FaintingGoat), "FaintingGoats", 1));
            AddObjective(new SlayObjective(typeof(Goral), "Gorals", 1));
            AddObjective(new SlayObjective(typeof(Ibex), "Ibex", 1));
            AddObjective(new SlayObjective(typeof(Markhor), "Markhors", 1));
            AddObjective(new SlayObjective(typeof(SableAntelope), "SableAntelopes", 1));
            AddObjective(new SlayObjective(typeof(Tahr), "Tahrs", 1));

            AddReward(new BaseReward(typeof(Gold), 1000, "1000 Gold"));
            AddReward(new BaseReward(typeof(GoatToken), 1, "Beast Master Token"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You have completed Gorwin Beestherd's beastly bounty!");
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

    public class GorwinBeestherd : MondainQuester
    {
        public override bool IsActiveVendor { get { return true; } }

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBGorwinBeestherd());
        }

        [Constructable]
        public GorwinBeestherd()
            : base("Gorwin Beestherd", "Master of the Wild and Whimsical")
        {
        }

        public GorwinBeestherd(Serial serial)
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
                Item token = player.Backpack.FindItemByType(typeof(GoatToken));
				Item masterToken = player.Backpack.FindItemByType(typeof(MasterToken));

                if (token != null || masterToken != null)
                {
                    SayTo(from, "Welcome to my beastly shop!");
                    base.VendorBuy(from);
                }
                else
                {
                    SayTo(from, "You must have a Beast Master Token to access my shop.");
                }
            }
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(GorwinBeestherdQuest)
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

    public class SBGorwinBeestherd : SBInfo
    {
        private System.Collections.Generic.List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public SBGorwinBeestherd()
        {
        }

        public override System.Collections.Generic.List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }
        public override IShopSellInfo SellInfo { get { return m_SellInfo; } }

        public class InternalBuyInfo : System.Collections.Generic.List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                // Add the beast-themed creatures here
                Add(new AnimalBuyInfo(1, typeof(BeardedGoat), 500, 10, 209, 0));
                Add(new AnimalBuyInfo(1, typeof(Chamois), 600, 10, 209, 0));
                Add(new AnimalBuyInfo(1, typeof(CliffGoat), 550, 10, 209, 0));
                Add(new AnimalBuyInfo(1, typeof(DallSheep), 650, 10, 209, 0));
                Add(new AnimalBuyInfo(1, typeof(FaintingGoat), 700, 10, 209, 0));
                Add(new AnimalBuyInfo(1, typeof(Goral), 750, 10, 209, 0));
                Add(new AnimalBuyInfo(1, typeof(Ibex), 800, 10, 209, 0));
                Add(new AnimalBuyInfo(1, typeof(Markhor), 850, 10, 209, 0));
                Add(new AnimalBuyInfo(1, typeof(SableAntelope), 900, 10, 209, 0));
                Add(new AnimalBuyInfo(1, typeof(Tahr), 950, 10, 209, 0));
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
