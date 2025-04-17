using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Engines.Quests;

namespace Server.Engines.Quests
{
    public class KashtiQuest : BaseQuest
    {
        public override bool DoneOnce => true;

        public override object Title => "The Golden Hunt";

        public override object Description => 
            "Greetings, traveler. I am Kashti, the Golden Seeker.\n\n" +
            "Lions once roamed free and proud across the eastern savannahs. Now they are corrupted—twisted by dark magics and unnatural rage.\n\n" +
            "I need a champion with the courage to cull their numbers. Slay *five hundred* Lions and restore balance.\n\n" +
            "If you succeed, I shall open my vaults to you—riches and rare items from across the realm await.";

        public override object Refuse => "Not all are meant for the hunt. Return when your resolve is sharpened.";

        public override object Uncomplete => "The lions still howl. The balance has not yet been restored.";

        public override object Complete =>
            "You return bathed in sunlight and silence—the roar of lions no longer echoes.\n\n" +
            "Your service is honored, and the Seeker's vault is now open to you.";

        public KashtiQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(Lion), "Lions", 500));
            AddReward(new BaseReward(typeof(Gold), 1000, "1000 Gold"));
        }

        public override void OnCompleted()
        {
            base.OnCompleted();

            var profile = Owner.AcquireTalents();
            if (!profile.Talents.ContainsKey(TalentID.LionSlayerQuest))
                profile.Talents[TalentID.LionSlayerQuest] = new Talent(TalentID.LionSlayerQuest);
            profile.Talents[TalentID.LionSlayerQuest].Points = 1;

            Owner.SendMessage(0x23, "You have slain 500 Lions for Kashti!");
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

    public class Kashti : MondainQuester
    {
        public override bool IsActiveVendor => true;
        private List<SBInfo> m_SBInfos = new List<SBInfo>();

        protected override List<SBInfo> SBInfos => m_SBInfos;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBKashti());
        }

        [Constructable]
        public Kashti() : base("Kashti", "the Golden Seeker")
        {
        }

        public Kashti(Serial serial) : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 25);
            Female = true;
            Race = Race.Human;
            Hue = Utility.RandomSkinHue();
            HairItemID = 0x2048;
            HairHue = Utility.RandomHairHue();
        }

        public override void InitOutfit()
        {
            AddItem(new Robe(2213)); // Golden robe
            AddItem(new Sandals(Utility.RandomNeutralHue()));
            AddItem(new Cloak(2213));
            AddItem(new Backpack());
        }

        public override void VendorBuy(Mobile from)
        {
            PlayerMobile player = from as PlayerMobile;
            if (player != null)
            {
                var profile = player.AcquireTalents();
                if (profile.Talents.TryGetValue(TalentID.LionSlayerQuest, out var talent) && talent.Points > 0)
                {
                    SayTo(from, "Welcome, Lion Slayer. The vault is yours to browse.");
                    base.VendorBuy(from);
                }
                else
                {
                    SayTo(from, "You must slay 500 Lions before I will trade with you.");
                }
            }
        }

        public override Type[] Quests => new Type[] { typeof(KashtiQuest) };

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

    public class SBKashti : SBInfo
    {
        private List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public override List<GenericBuyInfo> BuyInfo => m_BuyInfo;
        public override IShopSellInfo SellInfo => m_SellInfo;

        public class InternalBuyInfo : List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                Add(new AnimalBuyInfo(1, typeof(Lion), 5000, 10, 201, 0)); // Tamed Lion
                Add(new GenericBuyInfo(typeof(WatermelonSliced), 5000, 20, 0xC51, 0));
                Add(new GenericBuyInfo(typeof(ImperiumCrest), 5000, 20, 0x1F14, 0));
                Add(new GenericBuyInfo(typeof(ExoticWoods), 5000, 20, 0x1BD7, 0));
                Add(new GenericBuyInfo(typeof(ZombieHand), 5000, 20, 0x1CED, 0));
                Add(new GenericBuyInfo(typeof(EasterDayEgg), 5000, 20, 0x9B5, 0));
                Add(new GenericBuyInfo(typeof(FancyXmasTree), 5000, 20, 0x2374, 0));
                Add(new GenericBuyInfo(typeof(CrabBushel), 5000, 20, 0x9AC, 0));
                Add(new GenericBuyInfo(typeof(PersonalCannon), 5000, 20, 0xE94, 0));
                Add(new GenericBuyInfo(typeof(GalvanizedTub), 5000, 20, 0xB41, 0));
                Add(new GenericBuyInfo(typeof(FixedScales), 5000, 20, 0x14F5, 0));
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
