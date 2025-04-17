using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class KazuoHiryuQuest : BaseQuest
    {
        public override bool DoneOnce => true;

        public override object Title => "The Wrath of 500 Hiryus";

        public override object Description => "Kazuo the Skyblade bows solemnly. \n\n" +
            "\"The skies are no longer ours. The Hiryus—once noble spirits of the wind—have turned savage. " +
            "They soar the islands like storms of steel and fang. We must restore balance.\"\n\n" +
            "\"Bring down *five hundred* of these corrupted beasts. Then, I will entrust you with my most prized tames and exotic artifacts.\"";

        public override object Refuse => "\"I understand. May the winds guide you elsewhere.\"";

        public override object Uncomplete => "\"The skies still scream with Hiryu cries. You have more work yet.\"";

        public override object Complete => 
            "\"The winds speak your name now, dragon slayer. The skies are calm once more. " +
            "My shop is open to you—tamed Hiryus and rare items await. Use them with honor.\"";

        public KazuoHiryuQuest()
        {
            AddObjective(new SlayObjective(typeof(Hiryu), "Hiryus", 500));
        }

        public override void OnCompleted()
        {
            base.OnCompleted();

            var profile = Owner.AcquireTalents();
            if (!profile.Talents.ContainsKey(TalentID.HiryuSlayerQuest))
                profile.Talents[TalentID.HiryuSlayerQuest] = new Talent(TalentID.HiryuSlayerQuest);
            
            profile.Talents[TalentID.HiryuSlayerQuest].Points = 1;

            Owner.SendMessage(0x23, "You have slain 500 Hiryus for Kazuo the Skyblade!");
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
            int version = reader.ReadInt();
        }
    }

    public class Kazuo : MondainQuester
    {
        public override bool IsActiveVendor => true;
        private List<SBInfo> m_SBInfos = new List<SBInfo>();

        protected override List<SBInfo> SBInfos => m_SBInfos;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBKazuo());
        }

        [Constructable]
        public Kazuo() : base("Kazuo", "the Skyblade")
        {
        }

        public Kazuo(Serial serial) : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 25);

            Female = false;
            Race = Race.Human;

            Hue = Utility.RandomSkinHue();
            HairItemID = 0x203B;
            HairHue = Utility.RandomHairHue();
        }

        public override void InitOutfit()
        {
            AddItem(new Backpack());
            AddItem(new HakamaShita(1153));
            AddItem(new Hakama(1153));
            AddItem(new SamuraiTabi(1109));
            AddItem(new LeatherDo(1109));
            AddItem(new Cloak(1157));
        }

        public override void VendorBuy(Mobile from)
        {
            PlayerMobile player = from as PlayerMobile;
            if (player != null)
            {
                var profile = player.AcquireTalents();
                if (profile.Talents.TryGetValue(TalentID.HiryuSlayerQuest, out var talent) && talent.Points > 0)
                {
                    SayTo(from, "Welcome, sky-warrior. Browse my prized collection.");
                    base.VendorBuy(from);
                }
                else
                {
                    SayTo(from, "Only those who have slain 500 Hiryus may trade with me.");
                }
            }
        }

        public override Type[] Quests => new Type[] { typeof(KazuoHiryuQuest) };

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class SBKazuo : SBInfo
    {
        private List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public override List<GenericBuyInfo> BuyInfo => m_BuyInfo;
        public override IShopSellInfo SellInfo => m_SellInfo;

        public class InternalBuyInfo : List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                // Tamed Hiryu for sale
                Add(new AnimalBuyInfo(1, typeof(Hiryu), 5000, 10, 17, 0));

                // Items for 5000gp each
                Add(new GenericBuyInfo("Distillation Flask", typeof(DistillationFlask), 5000, 10, 0xE2D, 0));
                Add(new GenericBuyInfo("Sex Whip", typeof(SexWhip), 5000, 10, 0x13F6, 0));
                Add(new GenericBuyInfo("Frost Token", typeof(FrostToken), 5000, 10, 0x2AAA, 0));
                Add(new GenericBuyInfo("Soft Towel", typeof(SoftTowel), 5000, 10, 0x14F0, 0));
                Add(new GenericBuyInfo("Wedding Day Cake", typeof(WeddingDayCake), 5000, 10, 0x9E9, 0));
                Add(new GenericBuyInfo("Large Tome", typeof(LargeTome), 5000, 10, 0x1F4C, 0));
                Add(new GenericBuyInfo("Gargish Totem", typeof(GargishTotem), 5000, 10, 0x2B61, 0));
                Add(new GenericBuyInfo("Inscription Talisman", typeof(InscriptionTalisman), 5000, 10, 0x2F5B, 0));
                Add(new GenericBuyInfo("Heavy Anchor", typeof(HeavyAnchor), 5000, 10, 0xFAF, 0));
                Add(new GenericBuyInfo("Punishment Stocks", typeof(PunishmentStocks), 5000, 10, 0x12AB, 0));
            }
        }

        public class InternalSellInfo : GenericSellInfo
        {
            public InternalSellInfo()
            {
                // Optional: Add selling info
            }
        }
    }
}
