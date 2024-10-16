using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class FerrociousFuzzleQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Ferrocious Fuzzle's Ferret Hunt"; } }

        public override object Description
        {
            get
            {
                return 
                    "Greetings, brave adventurer! I am Ferrocious Fuzzle, the Ferret Enthusiast. " +
                    "My collection of rare ferrets is missing a few key specimens. To gain access to my " +
                    "exclusive shop, you must hunt down and slay these rare ferrets:\n\n" +
                    "1. Bubble Ferret\n" +
                    "2. Dreamy Ferret\n" +
                    "3. Frosty Ferret\n" +
                    "4. Glimmering Ferret\n" +
                    "5. Harmony Ferret\n" +
                    "6. Mystic Ferret\n" +
                    "7. Puffy Ferret\n" +
                    "8. Spark Ferret\n" +
                    "9. Starry Ferret\n" +
                    "10. Sunbeam Ferret\n\n" +
                    "Bring me proof of their defeat, and I shall grant you access to my rare ferret shop!";
            }
        }

        public override object Refuse { get { return "Very well. Return when you are ready to find my rare ferrets."; } }

        public override object Uncomplete { get { return "You haven't completed all the tasks yet. Keep searching for those elusive ferrets!"; } }

        public override object Complete { get { return "Fantastic! You have proven your skill in hunting ferrets. My shop is now open to you!"; } }

        public FerrociousFuzzleQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(BubbleFerret), "Bubble Ferrets", 1));
            AddObjective(new SlayObjective(typeof(DreamyFerret), "Dreamy Ferrets", 1));
            AddObjective(new SlayObjective(typeof(FrostyFerret), "Frosty Ferrets", 1));
            AddObjective(new SlayObjective(typeof(GlimmeringFerret), "Glimmering Ferrets", 1));
            AddObjective(new SlayObjective(typeof(HarmonyFerret), "Harmony Ferrets", 1));
            AddObjective(new SlayObjective(typeof(MysticFerret), "Mystic Ferrets", 1));
            AddObjective(new SlayObjective(typeof(PuffyFerret), "Puffy Ferrets", 1));
            AddObjective(new SlayObjective(typeof(SparkFerret), "Spark Ferrets", 1));
            AddObjective(new SlayObjective(typeof(StarryFerret), "Starry Ferrets", 1));
            AddObjective(new SlayObjective(typeof(SunbeamFerret), "Sunbeam Ferrets", 1));

            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(FerretToken), 1, "Ferret Token"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You have completed Ferrocious Fuzzle's ferret hunt!");
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

    public class FerrociousFuzzle : MondainQuester
    {
        public override bool IsActiveVendor { get { return true; } }

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBFerrociousFuzzle());
        }

        [Constructable]
        public FerrociousFuzzle()
            : base("Ferrocious Fuzzle", "The Ferret Enthusiast")
        {
        }

        public FerrociousFuzzle(Serial serial)
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
                Item token = player.Backpack.FindItemByType(typeof(FerretToken));
				Item masterToken = player.Backpack.FindItemByType(typeof(MasterToken));

                if (token != null || masterToken != null)
                {
                    SayTo(from, "Welcome to my ferret shop!");
                    base.VendorBuy(from);
                }
                else
                {
                    SayTo(from, "You must have a Ferret Token to access my shop.");
                }
            }
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(FerrociousFuzzleQuest)
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

    public class SBFerrociousFuzzle : SBInfo
    {
        private System.Collections.Generic.List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public SBFerrociousFuzzle()
        {
        }

        public override System.Collections.Generic.List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }
        public override IShopSellInfo SellInfo { get { return m_SellInfo; } }

        public class InternalBuyInfo : System.Collections.Generic.List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                // Add the ferret-themed creatures here
                Add(new AnimalBuyInfo(1, typeof(BubbleFerret), 500, 10, 279, 0));
                Add(new AnimalBuyInfo(1, typeof(DreamyFerret), 550, 10, 279, 0));
                Add(new AnimalBuyInfo(1, typeof(FrostyFerret), 600, 10, 279, 0));
                Add(new AnimalBuyInfo(1, typeof(GlimmeringFerret), 650, 10, 279, 0));
                Add(new AnimalBuyInfo(1, typeof(HarmonyFerret), 700, 10, 279, 0));
                Add(new AnimalBuyInfo(1, typeof(MysticFerret), 750, 10, 279, 0));
                Add(new AnimalBuyInfo(1, typeof(PuffyFerret), 800, 10, 279, 0));
                Add(new AnimalBuyInfo(1, typeof(SparkFerret), 850, 10, 279, 0));
                Add(new AnimalBuyInfo(1, typeof(StarryFerret), 900, 10, 279, 0));
                Add(new AnimalBuyInfo(1, typeof(SunbeamFerret), 950, 10, 279, 0));
            }
        }

        public class InternalSellInfo : GenericSellInfo
        {
            public InternalSellInfo()
            {
            }
        }
    }

    public class FerretToken : Item
    {
        [Constructable]
        public FerretToken() : base(0x1B74)
        {
            Name = "Ferret Token";
            Hue = 1152;
        }

        public FerretToken(Serial serial) : base(serial)
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
