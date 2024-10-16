using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class BruinTheBearologistQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Bruin the Bear-ologist's Furry Frenzy"; } }

        public override object Description
        {
            get
            {
                return 
                    "Greetings, adventurer! I am Bruin the Bear-ologist, a connoisseur of all things bear. " +
                    "To prove your worth and gain access to my exclusive bear-themed shop, you must hunt down " +
                    "the most extraordinary bears of the realm. Complete these tasks:\n\n" +
                    "1. Slay a FlameBear.\n" +
                    "2. Defeat a FrostBear.\n" +
                    "3. Conquer a LeafBear.\n" +
                    "4. Overcome a LightBear.\n" +
                    "5. Destroy a RockBear.\n" +
                    "6. Vanquish a ShadowBear.\n" +
                    "7. Subdue a SteelBear.\n" +
                    "8. Eradicate a ThunderBear.\n" +
                    "9. Defeat a VenomBear.\n" +
                    "10. Exterminate a WindBear.\n\n" +
                    "Complete these tasks, and I will grant you access to my bear-tastic shop!";
            }
        }

        public override object Refuse { get { return "Very well. Return when you are ready to face the bears!"; } }

        public override object Uncomplete { get { return "You haven't completed all the tasks yet. Keep hunting those bears!"; } }

        public override object Complete { get { return "Outstanding! You've proven yourself as a true bear aficionado. My shop is now open to you!"; } }

        public BruinTheBearologistQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(FlameBear), "FlameBears", 1));
            AddObjective(new SlayObjective(typeof(FrostBear), "FrostBears", 1));
            AddObjective(new SlayObjective(typeof(LeafBear), "LeafBears", 1));
            AddObjective(new SlayObjective(typeof(LightBear), "LightBears", 1));
            AddObjective(new SlayObjective(typeof(RockBear), "RockBears", 1));
            AddObjective(new SlayObjective(typeof(ShadowBear), "ShadowBears", 1));
            AddObjective(new SlayObjective(typeof(SteelBear), "SteelBears", 1));
            AddObjective(new SlayObjective(typeof(ThunderBear), "ThunderBears", 1));
            AddObjective(new SlayObjective(typeof(VenomBear), "VenomBears", 1));
            AddObjective(new SlayObjective(typeof(WindBear), "WindBears", 1));

            AddReward(new BaseReward(typeof(Gold), 1000, "1000 Gold"));
            AddReward(new BaseReward(typeof(BearToken), 1, "Bear Token"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You have completed Bruin the Bear-ologist's challenge!");
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

    public class BruinTheBearologist : MondainQuester
    {
        public override bool IsActiveVendor { get { return true; } }

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBBruinTheBearologist());
        }

        [Constructable]
        public BruinTheBearologist()
            : base("Bruin the Bear-ologist", "Master of Bears")
        {
        }

        public BruinTheBearologist(Serial serial)
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
                Item token = player.Backpack.FindItemByType(typeof(BearToken));
				Item masterToken = player.Backpack.FindItemByType(typeof(MasterToken));

                if (token != null || masterToken != null)
                {
                    SayTo(from, "Welcome to my bear-tastic shop!");
                    base.VendorBuy(from);
                }
                else
                {
                    SayTo(from, "You must have a Bear Token to access my shop.");
                }
            }
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(BruinTheBearologistQuest)
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

    public class SBBruinTheBearologist : SBInfo
    {
        private System.Collections.Generic.List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public SBBruinTheBearologist()
        {
        }

        public override System.Collections.Generic.List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }
        public override IShopSellInfo SellInfo { get { return m_SellInfo; } }

        public class InternalBuyInfo : System.Collections.Generic.List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                // Add the bear-themed creatures here
                Add(new AnimalBuyInfo(1, typeof(FlameBear), 1000, 10, 167, 0));
                Add(new AnimalBuyInfo(1, typeof(FrostBear), 1200, 10, 167, 0));
                Add(new AnimalBuyInfo(1, typeof(LeafBear), 900, 10, 167, 0));
                Add(new AnimalBuyInfo(1, typeof(LightBear), 950, 10, 167, 0));
                Add(new AnimalBuyInfo(1, typeof(RockBear), 1100, 10, 167, 0));
                Add(new AnimalBuyInfo(1, typeof(ShadowBear), 1300, 10, 167, 0));
                Add(new AnimalBuyInfo(1, typeof(SteelBear), 1400, 10, 167, 0));
                Add(new AnimalBuyInfo(1, typeof(ThunderBear), 1250, 10, 167, 0));
                Add(new AnimalBuyInfo(1, typeof(VenomBear), 1500, 10, 167, 0));
                Add(new AnimalBuyInfo(1, typeof(WindBear), 1350, 10, 167, 0));
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
