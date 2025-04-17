using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;

namespace Server.Engines.Quests
{
    public class GoatSlayerQuest : BaseQuest
    {
        public override bool DoneOnce => true;

        public override object Title => "The Culling of 500 Goats";

        public override object Description => 
            "Bah! Goats everywhere! They chew my fences, eat my laundry, and headbutt the postman.\n\n" +
            "I'm Quillan, once a proud goatherd, now a prisoner in my own barn.\n\n" +
            "I need someone to thin the herd. Bring down *five hundred* goats, and I’ll grant you access to my stash of exotic loot and prized tamed goats.\n\n" +
            "Do this, and I shall open my wares to you!";

        public override object Refuse => "A shame. I thought you had the courage to face the bleating tide.";

        public override object Uncomplete => "Still breathing, eh? Well, the goats are too. Keep culling!";

        public override object Complete =>
            "Ahh, the silence of a goatless field. You’ve done it!\n\n" +
            "My barn is safe again, and now, my wares are open to you. Exotic chests, enchanted loot, even tamed goats. A true hero's prize!";

        public GoatSlayerQuest()
        {
            AddObjective(new SlayObjective(typeof(Goat), "Goats", 500));
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
        }

        public override void OnCompleted()
        {
            base.OnCompleted();

            var profile = Owner.AcquireTalents();
            if (!profile.Talents.ContainsKey(TalentID.GoatSlayerQuest))
                profile.Talents[TalentID.GoatSlayerQuest] = new Talent(TalentID.GoatSlayerQuest);

            profile.Talents[TalentID.GoatSlayerQuest].Points = 1;

            Owner.SendMessage(0x23, "You have slain 500 Goats for Quillan!");
            Owner.PlaySound(CompleteSound);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt(); // version
        }
    }

    public class QuillanTheGoatherd : MondainQuester
    {
        public override bool IsActiveVendor => true;
        private List<SBInfo> m_SBInfos = new List<SBInfo>();

        protected override List<SBInfo> SBInfos => m_SBInfos;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBQuillan());
        }

        [Constructable]
        public QuillanTheGoatherd() : base("Quillan", "The Goat-Hardened")
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 25);
            Female = false;
            Race = Race.Human;

            Hue = Utility.RandomSkinHue();
            HairItemID = 0x203C;
            HairHue = Utility.RandomHairHue();
            FacialHairItemID = 0x2041;
            FacialHairHue = HairHue;
        }

        public override void InitOutfit()
        {
            AddItem(new Backpack());
            AddItem(new Sandals());
            AddItem(new Robe(Utility.RandomNeutralHue()));
            AddItem(new ShepherdsCrook());
        }

        public override void VendorBuy(Mobile from)
        {
            if (from is PlayerMobile player)
            {
                var profile = player.AcquireTalents();
                if (profile.Talents.TryGetValue(TalentID.GoatSlayerQuest, out Talent talent) && talent.Points > 0)
                {
                    SayTo(from, "A goatless world is a better world. Shop freely, friend.");
                    base.VendorBuy(from);
                }
                else
                {
                    SayTo(from, "Only a true Goat-Slayer may browse my wares. Return after your work is done.");
                }
            }
        }

        public override Type[] Quests => new Type[] { typeof(GoatSlayerQuest) };

        public QuillanTheGoatherd(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt(); // version
        }
    }

    public class SBQuillan : SBInfo
    {
        private List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public override List<GenericBuyInfo> BuyInfo => m_BuyInfo;
        public override IShopSellInfo SellInfo => m_SellInfo;

        public class InternalBuyInfo : List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                Add(new AnimalBuyInfo(1, typeof(Goat), 500, 10, 0xD1F, 0));

                Add(new GenericBuyInfo(typeof(DragonHoChest), 5000, 10, 0xE43, 0));
                Add(new GenericBuyInfo(typeof(DriveInTreasureTrove), 5000, 10, 0xE43, 0));
                Add(new GenericBuyInfo(typeof(EdisonsTreasureChest), 5000, 10, 0xE43, 0));
                Add(new GenericBuyInfo(typeof(EgyptianChest), 5000, 10, 0xE43, 0));
                Add(new GenericBuyInfo(typeof(EliteFoursVault), 5000, 10, 0xE43, 0));
                Add(new GenericBuyInfo(typeof(ElvenEnchantressChest), 5000, 10, 0xE43, 0));
                Add(new GenericBuyInfo(typeof(ElvenTreasuryChest), 5000, 10, 0xE43, 0));
                Add(new GenericBuyInfo(typeof(EmeraldIsleChest), 5000, 10, 0xE43, 0));
                Add(new GenericBuyInfo(typeof(EmperorJustinianCache), 5000, 10, 0xE43, 0));
                Add(new GenericBuyInfo(typeof(EmperorLegacyChest), 5000, 10, 0xE43, 0));
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
