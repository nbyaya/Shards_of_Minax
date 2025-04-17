using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;

namespace Server.Engines.Quests
{
    public class KaeltharonQuest : BaseQuest
    {
        public override bool DoneOnce => true;

        public override object Title => "Infernal Reckoning: The 500 Balron Slain";

        public override object Description => 
            "Kael'tharon, an infernal broker of demonic pacts, has set a deadly task before you.\n\n" +
            "He seeks the death of 500 Balrons — demonic overlords of chaos — for reasons known only to him.\n\n" +
            "Bring ruin to their kind, and he promises to open his forbidden shop, where bound Balrons and rare arcane items await.\n\n" +
            "Let their howls echo in the abyss as proof of your power.";

        public override object Refuse =>
            "Hmph. Then begone, coward. Let others brave the inferno in your stead.";

        public override object Uncomplete =>
            "You are not yet wreathed in the blood of 500 Balrons. Return when the tally is met.";

        public override object Complete =>
            "It is done. I feel the tremor in the void... their screams echo in eternity.\n\n" +
            "You have fulfilled the pact. My shop is open to you now. Spend wisely, mortal — these are the spoils of abyssal conquest.";

        public KaeltharonQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(Balron), "Balrons", 500));
            AddReward(new BaseReward(typeof(Gold), 5000, "5,000 Gold"));
        }

        public override void OnCompleted()
        {
            base.OnCompleted();

            var profile = Owner.AcquireTalents();
            if (!profile.Talents.ContainsKey(TalentID.BalronSlayerQuest))
                profile.Talents[TalentID.BalronSlayerQuest] = new Talent(TalentID.BalronSlayerQuest);

            profile.Talents[TalentID.BalronSlayerQuest].Points = 1;

            Owner.SendMessage(0x23, "You have slain 500 Balrons for Kael'tharon!");
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

    public class Kaeltharon : MondainQuester
    {
        public override bool IsActiveVendor => true;

        private List<SBInfo> m_SBInfos = new List<SBInfo>();
        protected override List<SBInfo> SBInfos => m_SBInfos;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBKaeltharon());
        }

        [Constructable]
        public Kaeltharon() : base("Kael'tharon", "The Infernal Broker")
        {
        }

        public Kaeltharon(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(100, 100, 25);
            Female = false;
            Race = Race.Human;
            Hue = 0x8404; // Deep red/demonic tone
            HairItemID = 0x2047;
            HairHue = 1150;
            FacialHairItemID = 0x2041;
            FacialHairHue = HairHue;
        }

        public override void InitOutfit()
        {
            AddItem(new Robe { Hue = 1154 });
            AddItem(new Sandals { Hue = 0x455 });
            AddItem(new Cloak { Hue = 1109 });
            AddItem(new GnarledStaff());
        }

        public override void VendorBuy(Mobile from)
        {
            if (from is PlayerMobile player)
            {
                var profile = player.AcquireTalents();
                if (profile.Talents.TryGetValue(TalentID.BalronSlayerQuest, out var talent) && talent.Points > 0)
                {
                    SayTo(from, "The pact is sealed. Choose your reward.");
                    base.VendorBuy(from);
                }
                else
                {
                    SayTo(from, "You have not yet proven your worth. Slay 500 Balrons, then return.");
                }
            }
        }

        public override Type[] Quests => new Type[] { typeof(KaeltharonQuest) };

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

    public class SBKaeltharon : SBInfo
    {
        private List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public SBKaeltharon() { }

        public override List<GenericBuyInfo> BuyInfo => m_BuyInfo;
        public override IShopSellInfo SellInfo => m_SellInfo;

        public class InternalBuyInfo : List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                Add(new AnimalBuyInfo(1, typeof(Balron), 5000, 5, 0x190, 0));

                Add(new GenericBuyInfo(typeof(SommelierBodySash), 5000, 10, 0x2798, 0));
                Add(new GenericBuyInfo(typeof(SurgeonsInsightfulMask), 5000, 10, 0x154B, 0));
                Add(new GenericBuyInfo(typeof(TidecallersSandals), 5000, 10, 0x170D, 0));
                Add(new GenericBuyInfo(typeof(BulKathosTribalGuardian), 5000, 10, 0x13BF, 0));
                Add(new GenericBuyInfo(typeof(FangOfStorms), 5000, 10, 0x13B9, 0));
                Add(new GenericBuyInfo(typeof(HeartbreakerSunder), 5000, 10, 0x143E, 0));
                Add(new GenericBuyInfo(typeof(MasamuneBlade), 5000, 10, 0x13B5, 0));
                Add(new GenericBuyInfo(typeof(NeptunesTrident), 5000, 10, 0xE87, 0));
                Add(new GenericBuyInfo(typeof(ProhibitionClub), 5000, 10, 0x13B4, 0));
                Add(new GenericBuyInfo(typeof(SamsonsJawbone), 5000, 10, 0x13F8, 0));
            }
        }

        public class InternalSellInfo : GenericSellInfo
        {
            public InternalSellInfo() { }
        }
    }
}
