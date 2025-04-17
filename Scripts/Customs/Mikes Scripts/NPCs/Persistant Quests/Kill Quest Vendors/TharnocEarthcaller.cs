using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class TharnocEarthcallerQuest : BaseQuest
    {
        public override bool DoneOnce => true;

        public override object Title => "Call of the Earth: 500 Elementals";

        public override object Description => "The land speaks, and I, Tharnoc Earthcaller, heed its pain. The Earth Elementals, once protectors, have turned savage—driven mad by imbalance. Destroy 500 of them to restore harmony. Only then shall you gain access to my ancient wares.";

        public override object Refuse => "Then you are not the one the Earth foretold. Leave, and return only when you understand the burden of balance.";

        public override object Uncomplete => "The ground still trembles with their fury. You have not slain enough Earth Elementals.";

        public override object Complete => "The stone is silent. You have answered the call. The Earth thanks you—and so do I. My wares are yours to browse.";

        public TharnocEarthcallerQuest()
        {
            AddObjective(new SlayObjective(typeof(EarthElemental), "Earth Elementals", 500));
            AddReward(new BaseReward(typeof(Gold), 1000, "1000 Gold"));
        }

        public override void OnCompleted()
        {
            base.OnCompleted();

            var profile = Owner.AcquireTalents();
            if (!profile.Talents.ContainsKey(TalentID.EarthElementalSlayerQuest))
                profile.Talents[TalentID.EarthElementalSlayerQuest] = new Talent(TalentID.EarthElementalSlayerQuest);

            profile.Talents[TalentID.EarthElementalSlayerQuest].Points = 1;

            Owner.SendMessage(0x23, "You have slain 500 Earth Elementals for Tharnoc Earthcaller!");
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
            int version = reader.ReadInt();
        }
    }

    public class TharnocEarthcaller : MondainQuester
    {
        private List<SBInfo> m_SBInfos = new List<SBInfo>();

        public override bool IsActiveVendor => true;
        protected override List<SBInfo> SBInfos => m_SBInfos;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBTharnoc());
        }

        [Constructable]
        public TharnocEarthcaller() : base("Tharnoc Earthcaller", "Voice of Stone")
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 25);
            Female = false;
            Race = Race.Human;
            Hue = Utility.RandomSkinHue();
            HairItemID = Utility.RandomList(0x203B, 0x2049);
            HairHue = Utility.RandomHairHue();
        }

        public override void InitOutfit()
        {
            AddItem(new Robe(0x6A7)); // Earthy robe color
            AddItem(new Sandals(0x96D)); // Dark brown
            AddItem(new Cloak(0x842)); // Mossy green
        }

        public override Type[] Quests => new Type[]
        {
            typeof(TharnocEarthcallerQuest)
        };

        public override void VendorBuy(Mobile from)
        {
            if (from is PlayerMobile player)
            {
                var profile = player.AcquireTalents();
                if (profile.Talents.TryGetValue(TalentID.EarthElementalSlayerQuest, out Talent t) && t.Points > 0)
                {
                    SayTo(player, "The Earth recognizes you. Browse what it has to offer.");
                    base.VendorBuy(from);
                }
                else
                {
                    SayTo(player, "Only those who have balanced the rage of Earth Elementals may shop here.");
                }
            }
        }

        public TharnocEarthcaller(Serial serial) : base(serial) { }

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

    public class SBTharnoc : SBInfo
    {
        private List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public override List<GenericBuyInfo> BuyInfo => m_BuyInfo;
        public override IShopSellInfo SellInfo => m_SellInfo;

        public class InternalBuyInfo : List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                // Tamed Earth Elemental - price 5000gp
                Add(new AnimalBuyInfo(1, typeof(EarthElemental), 5000, 10, 0x20D3, 0));

                // Augment Crystals & Deeds
                Add(new GenericBuyInfo(typeof(HidingAugmentCrystal), 5000, 20, 0x1869, 0));
                Add(new GenericBuyInfo(typeof(ImbuingAugmentCrystal), 5000, 20, 0x1869, 0));
                Add(new GenericBuyInfo(typeof(InscriptionAugmentCrystal), 5000, 20, 0x1869, 0));
                Add(new GenericBuyInfo(typeof(ItemIdentificationAugmentCrystal), 5000, 20, 0x1869, 0));
                Add(new GenericBuyInfo(typeof(JesterHatOfCommand), 5000, 10, 0x171C, 0));
                Add(new GenericBuyInfo(typeof(LegsSlotChangeDeed), 5000, 10, 0x14F0, 0));
                Add(new GenericBuyInfo(typeof(LifeLeechAugmentCrystal), 5000, 20, 0x1869, 0));
                Add(new GenericBuyInfo(typeof(LockpickingAugmentCrystal), 5000, 20, 0x1869, 0));
                Add(new GenericBuyInfo(typeof(LowerAttackAugmentCrystal), 5000, 20, 0x1869, 0));
                Add(new GenericBuyInfo(typeof(LuckAugmentCrystal), 5000, 20, 0x1869, 0));
            }
        }

        public class InternalSellInfo : GenericSellInfo
        {
            public InternalSellInfo() { }
        }
    }
}
