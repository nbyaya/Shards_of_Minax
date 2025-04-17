using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class ZephyrosQuest : BaseQuest
    {
        public override bool DoneOnce => true;

        public override object Title => "The Storm's Judgment";
        public override object Description =>
            "Ah, a brave soul approaches. I am Zephyros, once a summoner of storms, now a guardian of balance.\n\n" +
            "The Air Elementals have grown too wild, too fierce. They threaten not just mortals, but the very harmony of nature.\n\n" +
            "I need a hunter of wind, one who can silence the roar of five hundred Air Elementals.\n\n" +
            "Do this, and I shall open my vaults to you—gifts of arcane and elemental power await.";

        public override object Refuse => "Then may the winds carry you elsewhere. Return when your resolve is sharpened.";
        public override object Uncomplete => "The storm is not yet calmed. Slay more Air Elementals and return.";
        public override object Complete =>
            "The winds whisper of your deeds... You have tamed the chaos.\n\n" +
            "My respect is yours, and so too are my wares. May they serve you well.";

        public ZephyrosQuest()
        {
            AddObjective(new SlayObjective(typeof(AirElemental), "Air Elementals", 500));
            AddReward(new BaseReward(typeof(Gold), 1000, "1000 Gold"));
        }

        public override void OnCompleted()
        {
            base.OnCompleted();

            var profile = Owner.AcquireTalents();
            if (!profile.Talents.ContainsKey(TalentID.AirElementalQuest))
                profile.Talents[TalentID.AirElementalQuest] = new Talent(TalentID.AirElementalQuest);

            profile.Talents[TalentID.AirElementalQuest].Points = 1;

            Owner.SendMessage(0x23, "You have calmed the storm and earned Zephyros' trust.");
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

    public class Zephyros : MondainQuester
    {
        public override bool IsActiveVendor => true;
        private List<SBInfo> m_SBInfos = new List<SBInfo>();

        protected override List<SBInfo> SBInfos => m_SBInfos;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBZephyros());
        }

        [Constructable]
        public Zephyros() : base("Zephyros", "The Windcaller")
        {
        }

        public Zephyros(Serial serial) : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 25);
            Female = false;
            Race = Race.Human;

            Hue = Utility.RandomSkinHue();
            HairItemID = Utility.RandomList(0x2049, 0x2048);
            HairHue = 1150;
            FacialHairItemID = 0x204C;
            FacialHairHue = HairHue;
        }

        public override void InitOutfit()
        {
            AddItem(new Sandals(0x47E));
            AddItem(new Robe(0x481));
            AddItem(new Cloak(0x47E));
        }

        public override void VendorBuy(Mobile from)
        {
            if (from is PlayerMobile player)
            {
                var profile = player.AcquireTalents();
                if (profile.Talents.TryGetValue(TalentID.AirElementalQuest, out Talent talent) && talent.Points > 0)
                {
                    SayTo(from, "You have proven yourself. Browse the winds' bounty.");
                    base.VendorBuy(from);
                }
                else
                {
                    SayTo(from, "Return after you have slain 500 Air Elementals.");
                }
            }
        }

        public override Type[] Quests => new Type[] { typeof(ZephyrosQuest) };

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

    public class SBZephyros : SBInfo
    {
        private List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public override List<GenericBuyInfo> BuyInfo => m_BuyInfo;
        public override IShopSellInfo SellInfo => m_SellInfo;

        public class InternalBuyInfo : List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                Add(new AnimalBuyInfo(1, typeof(AirElemental), 5000, 5, 15, 0));
                Add(new GenericBuyInfo(typeof(SocialMediaMavensChest), 5000, 5, 0xE43, 0));
                Add(new GenericBuyInfo(typeof(StarfleetsVault), 5000, 5, 0xE43, 0));
                Add(new GenericBuyInfo(typeof(TechnicolorTalesChest), 5000, 5, 0xE43, 0));
                Add(new GenericBuyInfo(typeof(FishBasket), 5000, 5, 0x9AA, 0));
                Add(new GenericBuyInfo(typeof(FineSilverWire), 5000, 5, 0x1BD4, 0));
                Add(new GenericBuyInfo(typeof(PlatinumChip), 5000, 5, 0x1BE7, 0));
                Add(new GenericBuyInfo(typeof(TribalHelm), 5000, 5, 0x140E, 0));
                Add(new GenericBuyInfo(typeof(GlassTable), 5000, 5, 0xB74, 0));
                Add(new GenericBuyInfo(typeof(WaterWell), 5000, 5, 0x1005, 0));
                Add(new GenericBuyInfo(typeof(TabulaRasa), 5000, 5, 0xE34, 0));
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
