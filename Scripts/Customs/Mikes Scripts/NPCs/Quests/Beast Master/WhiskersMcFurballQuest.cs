using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class WhiskersMcFurballQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Whiskers McFurball's Feline Frenzy"; } }

        public override object Description
        {
            get
            {
                return 
                    "Meow! I'm Whiskers McFurball, the one and only Beast Master of the feline realm! " +
                    "To prove you have what it takes to earn the rarest of tokens, you'll need to " +
                    "hunt down my special feline friends. Complete these tasks and gain access to my " +
                    "exclusive shop of rare beasties:\n\n" +
                    "1. Slay an Abyssinian Tracker.\n" +
                    "2. Defeat a Bengal Storm.\n" +
                    "3. Conquer a Maine Coon Titan.\n" +
                    "4. Overcome a Persian Shade.\n" +
                    "5. Destroy a Ragdoll Guardian.\n" +
                    "6. Vanquish a Scottish Fold Sentinel.\n" +
                    "7. Subdue a Siamese Illusionist.\n" +
                    "8. Eradicate a Siberian Frostclaw.\n" +
                    "9. Defeat a Sphinx Cat.\n" +
                    "10. Exterminate a Turkish Angora Enchanter.\n\n" +
                    "Complete these tasks, and my exclusive shop will be open to you!";
            }
        }

        public override object Refuse { get { return "Come back when you're ready to tackle my feline challenge!"; } }

        public override object Uncomplete { get { return "You haven't completed all the tasks yet. Keep hunting those felines!"; } }

        public override object Complete { get { return "Fantastic! You've proven yourself worthy. My shop is now open to you!"; } }

        public WhiskersMcFurballQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(AbyssinianTracker), "Abyssinian Trackers", 1));
            AddObjective(new SlayObjective(typeof(BengalStorm), "Bengal Storms", 1));
            AddObjective(new SlayObjective(typeof(MaineCoonTitan), "Maine Coon Titans", 1));
            AddObjective(new SlayObjective(typeof(PersianShade), "Persian Shades", 1));
            AddObjective(new SlayObjective(typeof(RagdollGuardian), "Ragdoll Guardians", 1));
            AddObjective(new SlayObjective(typeof(ScottishFoldSentinel), "Scottish Fold Sentinels", 1));
            AddObjective(new SlayObjective(typeof(SiameseIllusionist), "Siamese Illusionists", 1));
            AddObjective(new SlayObjective(typeof(SiberianFrostclaw), "Siberian Frostclaws", 1));
            AddObjective(new SlayObjective(typeof(SphinxCat), "Sphinx Cats", 1));
            AddObjective(new SlayObjective(typeof(TurkishAngoraEnchanter), "Turkish Angora Enchanters", 1));

            AddReward(new BaseReward(typeof(Gold), 1000, "1000 Gold"));
            AddReward(new BaseReward(typeof(FelineToken), 1, "Feline Token"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed Whiskers McFurball's challenge!");
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

    public class WhiskersMcFurball : MondainQuester
    {
        public override bool IsActiveVendor { get { return true; } }

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBWhiskersMcFurball());
        }

        [Constructable]
        public WhiskersMcFurball()
            : base("Whiskers McFurball", "Master of Feline Beasts")
        {
        }

        public WhiskersMcFurball(Serial serial)
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
                Item token = player.Backpack.FindItemByType(typeof(FelineToken));
				Item masterToken = player.Backpack.FindItemByType(typeof(MasterToken));

                if (token != null || masterToken != null)
                {
                    SayTo(from, "Welcome to my feline beast shop!");
                    base.VendorBuy(from);
                }
                else
                {
                    SayTo(from, "You need a Feline Token to access my shop.");
                }
            }
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(WhiskersMcFurballQuest)
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

    public class SBWhiskersMcFurball : SBInfo
    {
        private System.Collections.Generic.List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public SBWhiskersMcFurball()
        {
        }

        public override System.Collections.Generic.List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }
        public override IShopSellInfo SellInfo { get { return m_SellInfo; } }

        public class InternalBuyInfo : System.Collections.Generic.List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                // Add the feline-themed creatures here
                Add(new AnimalBuyInfo(1, typeof(AbyssinianTracker), 1000, 10, 201, 0));
                Add(new AnimalBuyInfo(1, typeof(BengalStorm), 1200, 10, 201, 0));
                Add(new AnimalBuyInfo(1, typeof(MaineCoonTitan), 1100, 10, 201, 0));
                Add(new AnimalBuyInfo(1, typeof(PersianShade), 950, 10, 201, 0));
                Add(new AnimalBuyInfo(1, typeof(RagdollGuardian), 1300, 10, 201, 0));
                Add(new AnimalBuyInfo(1, typeof(ScottishFoldSentinel), 1400, 10, 201, 0));
                Add(new AnimalBuyInfo(1, typeof(SiameseIllusionist), 1250, 10, 201, 0));
                Add(new AnimalBuyInfo(1, typeof(SiberianFrostclaw), 1350, 10, 201, 0));
                Add(new AnimalBuyInfo(1, typeof(SphinxCat), 1450, 10, 201, 0));
                Add(new AnimalBuyInfo(1, typeof(TurkishAngoraEnchanter), 1500, 10, 201, 0));
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
