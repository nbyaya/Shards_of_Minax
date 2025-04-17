using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    // Zarrel's Juka Hunt Quest
    public class ZarrelQuest : BaseQuest
    {
        public override bool DoneOnce => true;

        public override object Title => "Purge the Juka";

        public override object Description => 
            "I am Zarrel of Minoc, and I seek vengeance.\n\n" +
            "The Juka warriors overrun the wilds with cruelty and savagery. My village fell to their blades—my kin, lost in smoke and screams.\n\n" +
            "I offer no gold, no praise—only purpose. Slay *five hundred* Juka Warriors. Cleanse this world of their presence.\n\n" +
            "Only then shall I offer you access to my stock—rare curios and one-of-a-kind prizes taken from my time in the deep ruins.\n\n" +
            "Return only when you are drenched in Juka blood.";

        public override object Refuse => "Vengeance waits for no coward. Return when you find your spine.";

        public override object Uncomplete => "You have not yet spilled enough Juka blood. Keep hunting.";

        public override object Complete =>
            "You reek of war and retribution. The Juka tremble, their dead countless.\n\n" +
            "Very well. You may now browse what little I have left. May it serve your journey.";

        public ZarrelQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(JukaWarrior), "Juka Warriors", 500));
            AddReward(new BaseReward(typeof(Gold), 1000, "A token of vengeance"));
        }

        public override void OnCompleted()
        {
            base.OnCompleted();

            var profile = Owner.AcquireTalents();
            if (!profile.Talents.ContainsKey(TalentID.JukaSlayerQuest))
                profile.Talents[TalentID.JukaSlayerQuest] = new Talent(TalentID.JukaSlayerQuest);

            profile.Talents[TalentID.JukaSlayerQuest].Points = 1;

            Owner.SendMessage(0x23, "You have slain 500 Juka Warriors for Zarrel!");
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
            int version = reader.ReadInt();
        }
    }

    public class Zarrel : MondainQuester
    {
        public override bool IsActiveVendor => true;
        private List<SBInfo> m_SBInfos = new List<SBInfo>();

        protected override List<SBInfo> SBInfos => m_SBInfos;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBZarrel());
        }

        [Constructable]
        public Zarrel() : base("Zarrel", "The Vengeful Relic-Keeper")
        {
        }

        public Zarrel(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(100, 100, 25);
            Female = false;
            Race = Race.Human;
            Hue = Utility.RandomSkinHue();
            HairItemID = Utility.RandomList(0x203B, 0x2049, 0x2048);
            HairHue = Utility.RandomHairHue();
            FacialHairItemID = Utility.RandomList(0x203E, 0x203F, 0x2040);
            FacialHairHue = HairHue;
        }

        public override void InitOutfit()
        {
            AddItem(new Backpack());
            AddItem(new Robe(Utility.RandomNeutralHue()));
            AddItem(new Boots(Utility.RandomNeutralHue()));

        }

        public override void VendorBuy(Mobile from)
        {
            if (from is PlayerMobile player)
            {
                var profile = player.AcquireTalents();
                if (profile.Talents.TryGetValue(TalentID.JukaSlayerQuest, out var talent) && talent.Points > 0)
                {
                    SayTo(from, "You've earned the right. Take what you will.");
                    base.VendorBuy(from);
                }
                else
                {
                    SayTo(from, "Only those who’ve slain 500 Juka may trade with me.");
                }
            }
        }

        public override Type[] Quests => new Type[] { typeof(ZarrelQuest) };

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

    public class SBZarrel : SBInfo
    {
        private List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public override List<GenericBuyInfo> BuyInfo => m_BuyInfo;
        public override IShopSellInfo SellInfo => m_SellInfo;

        public class InternalBuyInfo : List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                Add(new AnimalBuyInfo(1, typeof(JukaWarrior), 5000, 10, 17, 0));
                Add(new GenericBuyInfo(typeof(UncrackedGeode), 5000, 10, 0x1F19, 0));
                Add(new GenericBuyInfo(typeof(DrapedBlanket), 5000, 10, 0x2B00, 0));
                Add(new GenericBuyInfo(typeof(BlacksmithTalisman), 5000, 10, 0x2F58, 0));
                Add(new GenericBuyInfo(typeof(CityBanner), 5000, 10, 0x23E3, 0));
                Add(new GenericBuyInfo(typeof(BookTwentyfive), 5000, 10, 0xFF1, 0));
                Add(new GenericBuyInfo(typeof(WelcomeMat), 5000, 10, 0xA91, 0));
                Add(new GenericBuyInfo(typeof(HolidayCandleArran), 5000, 10, 0xA28, 0));
                Add(new GenericBuyInfo(typeof(ValentineTeddybear), 5000, 10, 0x2328, 0));
                Add(new GenericBuyInfo(typeof(WitchesCauldron), 5000, 10, 0x184A, 0));
                Add(new GenericBuyInfo(typeof(FireRelic), 5000, 10, 0x1844, 0));
            }
        }

        public class InternalSellInfo : GenericSellInfo
        {
            public InternalSellInfo() { }
        }
    }
}
