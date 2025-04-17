using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class ZarethDrakeQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title => "The Drake Reckoning";
        public override object Description =>
            "I am Zareth, once a caller of storms and flames... now exiled. " +
            "The Drakes I once commanded turned upon me. They grow too many, too wild. " +
            "If you are brave enough to cull *five hundred*, then I will share the remnants of my power.\n\n" +
            "Slay them. All of them. Bring balance to the sky again.";

        public override object Refuse => "Cowardice will not cool the fire, mortal. Return when you have courage.";
        public override object Uncomplete => "The skies still scream. You have not slain enough Drakes.";
        public override object Complete =>
            "You have done it... the skies are quieter now. The flames listen once more.\n\n" +
            "You have earned my trust—and my wares. Use them wisely, flamebearer.";

        public ZarethDrakeQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(Drake), "Drakes", 500));
            AddReward(new BaseReward(typeof(Gold), 1000, "1000 Gold"));
        }

        public override void OnCompleted()
        {
            base.OnCompleted();

            var profile = Owner.AcquireTalents();
            if (!profile.Talents.ContainsKey(TalentID.DrakeSlayerQuest))
                profile.Talents[TalentID.DrakeSlayerQuest] = new Talent(TalentID.DrakeSlayerQuest);
            profile.Talents[TalentID.DrakeSlayerQuest].Points = 1;

            Owner.SendMessage(0x23, "You have slain 500 Drakes for Zareth!");
            Owner.PlaySound(CompleteSound);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt();
        }
    }

    public class Zareth : MondainQuester
    {
        public override bool IsActiveVendor => true;
        private List<SBInfo> m_SBInfos = new List<SBInfo>();

        protected override List<SBInfo> SBInfos => m_SBInfos;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBZareth());
        }

        [Constructable]
        public Zareth() : base("Zareth", "the Flamecaller")
        {
        }

        public Zareth(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(100, 100, 25);
            Female = false;
            Race = Race.Human;
            Hue = Utility.RandomSkinHue();
            HairItemID = Utility.RandomList(0x2048, 0x2049);
            HairHue = Utility.RandomHairHue();
            FacialHairItemID = 0x2041;
            FacialHairHue = HairHue;
        }

        public override void InitOutfit()
        {
            AddItem(new Backpack());
            AddItem(new Robe { Hue = 1359 });
            AddItem(new Sandals { Hue = 1359 });
            AddItem(new WizardsHat { Hue = 1359 });
            AddItem(new GnarledStaff());
        }

        public override void VendorBuy(Mobile from)
        {
            if (from is PlayerMobile player)
            {
                var profile = player.AcquireTalents();
                if (profile.Talents.TryGetValue(TalentID.DrakeSlayerQuest, out var talent) && talent.Points > 0)
                {
                    SayTo(from, "You carry the flame well. Choose from my wares.");
                    base.VendorBuy(from);
                }
                else
                {
                    SayTo(from, "No, no... you are not yet worthy. Slay 500 Drakes, then return.");
                }
            }
        }

        public override Type[] Quests => new Type[] { typeof(ZarethDrakeQuest) };

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt();
        }
    }

    public class SBZareth : SBInfo
    {
        private List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public override List<GenericBuyInfo> BuyInfo => m_BuyInfo;
        public override IShopSellInfo SellInfo => m_SellInfo;

        public class InternalBuyInfo : List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                Add(new AnimalBuyInfo(1, typeof(Drake), 5000, 5, 0x20D6, 0));

                Add(new GenericBuyInfo(typeof(DiscordanceAugmentCrystal), 5000, 10, 0x1F2E, 0));
                Add(new GenericBuyInfo(typeof(DispelAugmentCrystal), 5000, 10, 0x1F2E, 0));
                Add(new GenericBuyInfo(typeof(EarringSlotChangeDeed), 5000, 10, 0x14F0, 0));
                Add(new GenericBuyInfo(typeof(EnergyHitAreaCrystal), 5000, 10, 0x1F2E, 0));
                Add(new GenericBuyInfo(typeof(EnergyResistAugmentCrystal), 5000, 10, 0x1F2E, 0));
                Add(new GenericBuyInfo(typeof(FatigueAugmentCrystal), 5000, 10, 0x1F2E, 0));
                Add(new GenericBuyInfo(typeof(FencingAugmentCrystal), 5000, 10, 0x1F2E, 0));
                Add(new GenericBuyInfo(typeof(FireballAugmentCrystal), 5000, 10, 0x1F2E, 0));
                Add(new GenericBuyInfo(typeof(FireHitAreaCrystal), 5000, 10, 0x1F2E, 0));
                Add(new GenericBuyInfo(typeof(FireResistAugmentCrystal), 5000, 10, 0x1F2E, 0));
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
