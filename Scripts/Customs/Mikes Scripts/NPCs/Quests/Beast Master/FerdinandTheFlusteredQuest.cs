using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class FerdinandTheFlusteredQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Ferdinand the Flustered's Beast Hunt"; } }

        public override object Description
        {
            get
            {
                return 
                    "Greetings, adventurer! I am Ferdinand the Flustered, and I have a rather peculiar problem. " +
                    "You see, I've been trying to tame these beasts for my shop, but they're all quite unruly. " +
                    "If you can manage to defeat them, I might just give you a special token that grants access to my beast shop. " +
                    "Here’s what you need to do:\n\n" +
                    "1. Slay IshKarTheForgotten.\n" +
                    "2. Defeat NyxRith.\n" +
                    "3. Conquer QuorZael.\n" +
                    "4. Overcome RathZorTheShattered.\n" +
                    "5. Destroy ThulGorTheForsaken.\n" +
                    "6. Vanquish UruKoth.\n" +
                    "7. Subdue Vorgath.\n" +
                    "8. Eradicate XalRath.\n" +
                    "9. Defeat ZelVrak.\n" +
                    "10. Exterminate ZorThul.\n\n" +
                    "Complete these tasks, and I'll reward you with a special token to access my exclusive beast shop!";
            }
        }

        public override object Refuse { get { return "Alright then, come back when you're ready to tackle these beasts."; } }

        public override object Uncomplete { get { return "You haven't finished all the tasks yet. Keep hunting those beasts!"; } }

        public override object Complete { get { return "Well done! You’ve managed to handle all those troublesome beasts. Here’s your token!"; } }

        public FerdinandTheFlusteredQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(IshKarTheForgotten), "IshKarTheForgotten", 1));
            AddObjective(new SlayObjective(typeof(NyxRith), "NyxRith", 1));
            AddObjective(new SlayObjective(typeof(QuorZael), "QuorZael", 1));
            AddObjective(new SlayObjective(typeof(RathZorTheShattered), "RathZorTheShattered", 1));
            AddObjective(new SlayObjective(typeof(ThulGorTheForsaken), "ThulGorTheForsaken", 1));
            AddObjective(new SlayObjective(typeof(UruKoth), "UruKoth", 1));
            AddObjective(new SlayObjective(typeof(Vorgath), "Vorgath", 1));
            AddObjective(new SlayObjective(typeof(XalRath), "XalRath", 1));
            AddObjective(new SlayObjective(typeof(ZelVrak), "ZelVrak", 1));
            AddObjective(new SlayObjective(typeof(ZorThul), "ZorThul", 1));

            AddReward(new BaseReward(typeof(Gold), 1500, "1500 Gold"));
            AddReward(new BaseReward(typeof(BeastMasterToken), 1, "Beast Master Token"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations on completing Ferdinand's challenge!");
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

    public class FerdinandTheFlustered : MondainQuester
    {
        public override bool IsActiveVendor { get { return true; } }

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SB_FerdinandTheFlustered());
        }

        [Constructable]
        public FerdinandTheFlustered()
            : base("Ferdinand the Flustered", "Master of the Beasts")
        {
        }

        public FerdinandTheFlustered(Serial serial)
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
                Item token = player.Backpack.FindItemByType(typeof(BeastMasterToken));
				Item masterToken = player.Backpack.FindItemByType(typeof(MasterToken));

                if (token != null || masterToken != null)
                {
                    SayTo(from, "Welcome to my beast shop! Take a look at my collection.");
                    base.VendorBuy(from);
                }
                else
                {
                    SayTo(from, "You need a Beast Master Token to access my shop.");
                }
            }
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(FerdinandTheFlusteredQuest)
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

    public class SB_FerdinandTheFlustered : SBInfo
    {
        private System.Collections.Generic.List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public SB_FerdinandTheFlustered()
        {
        }

        public override System.Collections.Generic.List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }
        public override IShopSellInfo SellInfo { get { return m_SellInfo; } }

        public class InternalBuyInfo : System.Collections.Generic.List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                // Add the beast-themed creatures here
                Add(new AnimalBuyInfo(1, typeof(IshKarTheForgotten), 1500, 10, 22, 0));
                Add(new AnimalBuyInfo(1, typeof(NyxRith), 1600, 10, 22, 0));
                Add(new AnimalBuyInfo(1, typeof(QuorZael), 1700, 10, 22, 0));
                Add(new AnimalBuyInfo(1, typeof(RathZorTheShattered), 1800, 10, 22, 0));
                Add(new AnimalBuyInfo(1, typeof(ThulGorTheForsaken), 1900, 10, 22, 0));
                Add(new AnimalBuyInfo(1, typeof(UruKoth), 2000, 10, 22, 0));
                Add(new AnimalBuyInfo(1, typeof(Vorgath), 2100, 10, 22, 0));
                Add(new AnimalBuyInfo(1, typeof(XalRath), 2200, 10, 22, 0));
                Add(new AnimalBuyInfo(1, typeof(ZelVrak), 2300, 10, 22, 0));
                Add(new AnimalBuyInfo(1, typeof(ZorThul), 2400, 10, 22, 0));
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
