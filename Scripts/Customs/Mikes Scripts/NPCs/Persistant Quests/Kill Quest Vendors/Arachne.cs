using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class ArachneQuest : BaseQuest
    {
        public override bool DoneOnce => true;

        public override object Title => "The Silk Reaping";

        public override object Description => 
            "The world trembles under the crawling menace of the Dread Spiders.\n\n" +
            "I am Arachne, once a tamer of beasts... now a hunter of nightmares.\n\n" +
            "Bring me the corpses of five hundred Dread Spiders. Slay them in droves—" +
            "let their ichor coat the soil! Only then shall you earn access to my wares... exotic, arcane, and most valuable.";

        public override object Refuse => "Then you are not yet ready to face true darkness. Return when fear no longer rules you.";

        public override object Uncomplete => "The webs still tremble... You have not yet culled enough of their foul kin.";

        public override object Complete =>
            "Your hands are stained with venom, your breath stills with exertion.\n\n" +
            "Well done, spider-slayer. My collection is now open to you—rare augment crystals, tamed Dread Spiders, and tools for masters of the arcane and primal arts.\n\n" +
            "Spend wisely. Death is not cheap.";

        public ArachneQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(DreadSpider), "Dread Spiders", 500));
        }

        public override void OnCompleted()
        {
            base.OnCompleted();

            var profile = Owner.AcquireTalents();
            if (!profile.Talents.ContainsKey(TalentID.DreadSpiderQuest))
                profile.Talents[TalentID.DreadSpiderQuest] = new Talent(TalentID.DreadSpiderQuest);

            profile.Talents[TalentID.DreadSpiderQuest].Points = 1;

            Owner.SendMessage(0x23, "You have completed Arachne's challenge and may now trade with her.");
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

    public class Arachne : MondainQuester
    {
        public override bool IsActiveVendor => true;

        private List<SBInfo> m_SBInfos = new List<SBInfo>();
        protected override List<SBInfo> SBInfos => m_SBInfos;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBArachne());
        }

        [Constructable]
        public Arachne() : base("Arachne", "Silk Reaper")
        {
        }

        public Arachne(Serial serial) : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 25);
            Female = true;
            Race = Race.Human;
            Hue = Utility.RandomSkinHue();
            HairItemID = Utility.RandomList(0x203B, 0x2049);
            HairHue = Utility.RandomHairHue();
        }

        public override void InitOutfit()
        {
            AddItem(new Backpack());
            AddItem(new Sandals());
            AddItem(new Robe(Utility.RandomNeutralHue()));
            AddItem(new Cloak(Utility.RandomNeutralHue()));
        }

        public override void VendorBuy(Mobile from)
        {
            PlayerMobile player = from as PlayerMobile;
            if (player != null)
            {
                var profile = player.AcquireTalents();
                if (profile.Talents.TryGetValue(TalentID.DreadSpiderQuest, out Talent talent) && talent.Points > 0)
                {
                    SayTo(from, "You have earned your place. Browse my rare goods.");
                    base.VendorBuy(from);
                }
                else
                {
                    SayTo(from, "You must slay 500 Dread Spiders before I do business with you.");
                }
            }
        }

        public override Type[] Quests => new Type[] { typeof(ArachneQuest) };

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

    public class SBArachne : SBInfo
    {
        private List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public SBArachne() { }

        public override List<GenericBuyInfo> BuyInfo => m_BuyInfo;
        public override IShopSellInfo SellInfo => m_SellInfo;

        public class InternalBuyInfo : List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                Add(new AnimalBuyInfo(1, typeof(DreadSpider), 5000, 5, 0x9EC, 0));
                Add(new GenericBuyInfo(typeof(FishingAugmentCrystal), 5000, 10, 0x1F2B, 0));
                Add(new GenericBuyInfo(typeof(FletchingAugmentCrystal), 5000, 10, 0x1F2B, 0));
                Add(new GenericBuyInfo(typeof(FocusAugmentCrystal), 5000, 10, 0x1F2B, 0));
                Add(new GenericBuyInfo(typeof(FootwearSlotChangeDeed), 5000, 10, 0x14F0, 0));
                Add(new GenericBuyInfo(typeof(ForensicEvaluationAugmentCrystal), 5000, 10, 0x1F2B, 0));
                Add(new GenericBuyInfo(typeof(GlovesOfCommand), 5000, 5, 0x13C6, 0));
                Add(new GenericBuyInfo(typeof(HarmAugmentCrystal), 5000, 10, 0x1F2B, 0));
                Add(new GenericBuyInfo(typeof(HeadSlotChangeDeed), 5000, 10, 0x14F0, 0));
                Add(new GenericBuyInfo(typeof(HealingAugmentCrystal), 5000, 10, 0x1F2B, 0));
                Add(new GenericBuyInfo(typeof(HerdingAugmentCrystal), 5000, 10, 0x1F2B, 0));
            }
        }

        public class InternalSellInfo : GenericSellInfo
        {
            public InternalSellInfo() { }
        }
    }
}
