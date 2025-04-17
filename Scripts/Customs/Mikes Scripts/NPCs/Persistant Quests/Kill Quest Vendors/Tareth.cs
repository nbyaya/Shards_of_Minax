using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;

namespace Server.Engines.Quests
{
    public class TarethDireWolfQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title => "The Hunt of 500 Dire Wolves";

        public override object Description => 
            "Traveler, I am Tareth the Whisperfang, a recluse who once walked with beasts.\n\n" +
            "The Dire Wolves have overrun the Shadowpine Vale. Their numbers threaten balance, and their hunger knows no end.\n\n" +
            "I seek one who can cull the pack—five hundred Dire Wolves must fall. " +
            "In return, I will share secrets once kept for beastmasters and druids alone.\n\n" +
            "Should you succeed, my shop of rare tames and mystic materials will be yours to access.";

        public override object Refuse => "So be it. May you never hear the howls that echo from the vale...";

        public override object Uncomplete => "The hunt continues, friend. Return when the blood of 500 Dire Wolves stains your hands.";

        public override object Complete => 
            "The winds speak of your deeds. The wolves fall silent before your approach.\n\n" +
            "As promised, my wares are now yours to browse. Tread carefully; some things bite.";

        public TarethDireWolfQuest()
        {
            AddObjective(new SlayObjective(typeof(DireWolf), "Dire Wolves", 500));
            AddReward(new BaseReward(typeof(Gold), 2000, "2000 Gold"));
        }

        public override void OnCompleted()
        {
            base.OnCompleted();

            var profile = Owner.AcquireTalents();
            if (!profile.Talents.ContainsKey(TalentID.DireWolfSlayerQuest))
                profile.Talents[TalentID.DireWolfSlayerQuest] = new Talent(TalentID.DireWolfSlayerQuest);

            profile.Talents[TalentID.DireWolfSlayerQuest].Points = 1;

            Owner.SendMessage(0x23, "You have completed Tareth's Dire Wolf hunt!");
            Owner.PlaySound(CompleteSound);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt();
        }
    }

    public class Tareth : MondainQuester
    {
        public override bool IsActiveVendor => true;

        private List<SBInfo> m_SBInfos = new List<SBInfo>();

        protected override List<SBInfo> SBInfos => m_SBInfos;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBTareth());
        }

        [Constructable]
        public Tareth() : base("Tareth", "Whisperfang of the Vale")
        {
        }

        public Tareth(Serial serial) : base(serial)
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
            AddItem(new Robe(Utility.RandomNeutralHue()));
            AddItem(new Sandals(Utility.RandomNeutralHue()));
            AddItem(new Cloak(Utility.RandomNeutralHue()));
        }

        public override void VendorBuy(Mobile from)
        {
            PlayerMobile player = from as PlayerMobile;
            if (player != null)
            {
                var profile = player.AcquireTalents();
                if (profile.Talents.TryGetValue(TalentID.DireWolfSlayerQuest, out var talent) && talent.Points > 0)
                {
                    SayTo(from, "You've earned my trust. Browse the gifts of the wild.");
                    base.VendorBuy(from);
                }
                else
                {
                    SayTo(from, "Only those who have culled 500 Dire Wolves may trade with me.");
                }
            }
        }

        public override Type[] Quests => new Type[] { typeof(TarethDireWolfQuest) };

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt();
        }
    }

    public class SBTareth : SBInfo
    {
        private List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public override List<GenericBuyInfo> BuyInfo => m_BuyInfo;
        public override IShopSellInfo SellInfo => m_SellInfo;

        public class InternalBuyInfo : List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                Add(new AnimalBuyInfo(1, typeof(DireWolf), 5000, 10, 204, 0)); // Tamed DireWolf
                Add(new GenericBuyInfo(typeof(GlimmeringMarble), 5000, 10, 0x1F19, 0));
                Add(new GenericBuyInfo(typeof(GlimmeringPetrifiedWood), 5000, 10, 0x1F19, 0));
                Add(new GenericBuyInfo(typeof(GlimmeringLimestone), 5000, 10, 0x1F19, 0));
                Add(new GenericBuyInfo(typeof(GlimmeringBloodrock), 5000, 10, 0x1F19, 0));
                Add(new GenericBuyInfo(typeof(MythicTourmaline), 5000, 10, 0x1F19, 0));
                Add(new GenericBuyInfo(typeof(LegendaryTourmaline), 5000, 10, 0x1F19, 0));
                Add(new GenericBuyInfo(typeof(AncientTourmaline), 5000, 10, 0x1F19, 0));
                Add(new GenericBuyInfo(typeof(MythicWood), 5000, 10, 0x1F19, 0));
                Add(new GenericBuyInfo(typeof(LegendaryWood), 5000, 10, 0x1F19, 0));
                Add(new GenericBuyInfo(typeof(DapperFedoraOfInsight), 5000, 10, 0x1715, 0));
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
