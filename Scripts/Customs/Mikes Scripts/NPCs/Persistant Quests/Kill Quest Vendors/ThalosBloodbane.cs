using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class ThalosBloodbaneQuest : BaseQuest
    {
        public override bool DoneOnce => true;

        public override object Title => "Thalos' Blood Elemental Hunt";

        public override object Description =>
            "So... you've come seeking power. Blood Elementals have plagued these caverns too long.\n\n" +
            "Bring death to *five hundred* of them. I seek vengeance, and you—if you succeed—will gain access " +
            "to forbidden tools and bonded bloodspawn.\n\nReturn when the crimson tide has receded.";

        public override object Refuse => "Then you're not the warrior I hoped for.";

        public override object Uncomplete => "Still the blood flows... return when the ground is soaked in their essence.";

        public override object Complete =>
            "You've done it. The earth groans with the blood you've spilled. The Blood Elementals are broken.\n\n" +
            "My shop is now open to you—tamed Bloodspawn and cursed relics await.\n\nSpend your coin in power.";

        public ThalosBloodbaneQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(BloodElemental), "Blood Elementals", 500));
            AddReward(new BaseReward(typeof(Gold), 1000, "1000 Gold"));
        }

        public override void OnCompleted()
        {
            base.OnCompleted();

            var profile = Owner.AcquireTalents();
            if (!profile.Talents.ContainsKey(TalentID.BloodElementalQuest))
                profile.Talents[TalentID.BloodElementalQuest] = new Talent(TalentID.BloodElementalQuest);

            profile.Talents[TalentID.BloodElementalQuest].Points = 1;

            Owner.SendMessage(0x23, "You have slain 500 Blood Elementals for Thalos Bloodbane!");
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

    public class ThalosBloodbane : MondainQuester
    {
        public override bool IsActiveVendor => true;
        private List<SBInfo> m_SBInfos = new List<SBInfo>();

        protected override List<SBInfo> SBInfos => m_SBInfos;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBThalos());
        }

        [Constructable]
        public ThalosBloodbane() : base("Thalos Bloodbane", "Slayer of Blood Elementals")
        {
        }

        public ThalosBloodbane(Serial serial) : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(150, 150, 100);
            Female = false;
            Race = Race.Human;
            Hue = Utility.RandomSkinHue();
            HairItemID = 0x2048;
            HairHue = Utility.RandomHairHue();
            FacialHairItemID = 0x2041;
            FacialHairHue = HairHue;
        }

        public override void InitOutfit()
        {
            AddItem(new Backpack());
            AddItem(new Robe(0x21)); // Dark red robe
            AddItem(new Sandals(Utility.RandomNeutralHue()));
            AddItem(new GnarledStaff());
        }

        public override void VendorBuy(Mobile from)
        {
            if (from is PlayerMobile player)
            {
                var profile = player.AcquireTalents();
                if (profile.Talents.TryGetValue(TalentID.BloodElementalQuest, out var talent) && talent.Points > 0)
                {
                    SayTo(from, "You have earned my trust. Browse what power I offer.");
                    base.VendorBuy(from);
                }
                else
                {
                    SayTo(from, "You must first slay 500 Blood Elementals. Return when the deed is done.");
                }
            }
        }

        public override Type[] Quests => new Type[] { typeof(ThalosBloodbaneQuest) };

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

    public class SBThalos : SBInfo
    {
        private List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public SBThalos()
        {
        }

        public override List<GenericBuyInfo> BuyInfo => m_BuyInfo;
        public override IShopSellInfo SellInfo => m_SellInfo;

        public class InternalBuyInfo : List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                Add(new AnimalBuyInfo(1, typeof(BloodElemental), 5000, 10, 8450, 0));
                Add(new GenericBuyInfo("Blood Sword", typeof(BloodSword), 5000, 10, 0x13B6, 0));
                Add(new GenericBuyInfo("Bushido Augment Crystal", typeof(BushidoAugmentCrystal), 5000, 10, 0x1F19, 0));
                Add(new GenericBuyInfo("Camping Augment Crystal", typeof(CampingAugmentCrystal), 5000, 10, 0x1F19, 0));
                Add(new GenericBuyInfo("Carpentry Augment Crystal", typeof(CarpentryAugmentCrystal), 5000, 10, 0x1F19, 0));
                Add(new GenericBuyInfo("Cartography Augment Crystal", typeof(CartographyAugmentCrystal), 5000, 10, 0x1F19, 0));
                Add(new GenericBuyInfo("Chivalry Augment Crystal", typeof(ChivalryAugmentCrystal), 5000, 10, 0x1F19, 0));
                Add(new GenericBuyInfo("Cold Hit Area Crystal", typeof(ColdHitAreaCrystal), 5000, 10, 0x1F19, 0));
                Add(new GenericBuyInfo("Cold Resist Augment Crystal", typeof(ColdResistAugmentCrystal), 5000, 10, 0x1F19, 0));
                Add(new GenericBuyInfo("Cooking Augment Crystal", typeof(CookingAugmentCrystal), 5000, 10, 0x1F19, 0));
                Add(new GenericBuyInfo("Curse Augment Crystal", typeof(CurseAugmentCrystal), 5000, 10, 0x1F19, 0));
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
