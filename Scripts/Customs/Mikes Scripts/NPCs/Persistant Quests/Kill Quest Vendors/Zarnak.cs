using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class ZarnakQuest : BaseQuest
    {
        public override bool DoneOnce => true;

        public override object Title => "Mongbat Massacre";

        public override object Description => 
            "The skies above the swamp shriek with the wings of Mongbats.\n\n" +
            "I am Zarnak, once a druid, now a prisoner of their endless chatter. " +
            "I beg you, cull their numbers. Bring balance.\n\n" +
            "Kill 500 Mongbats, and I shall reward you with access to rare artifacts—" +
            "spoils of nature and time itself.";

        public override object Refuse => "Very well. The Mongbats will continue to scream.";

        public override object Uncomplete => "The skies still darken with wings. Return when 500 Mongbats lie still.";

        public override object Complete => 
            "Peace... I can finally hear the wind again.\n" +
            "Your service to nature is complete.\n\n" +
            "I now offer you wares untouched by civilization. Tread with care, and choose wisely.";

        public ZarnakQuest()
        {
            AddObjective(new SlayObjective(typeof(Mongbat), "Mongbats", 500));
            AddReward(new BaseReward(typeof(Gold), 1000, "1000 Gold"));
        }

        public override void OnCompleted()
        {
            base.OnCompleted();

            var profile = Owner.AcquireTalents();
            if (!profile.Talents.ContainsKey(TalentID.MongbatSlayerQuest))
                profile.Talents[TalentID.MongbatSlayerQuest] = new Talent(TalentID.MongbatSlayerQuest);
            profile.Talents[TalentID.MongbatSlayerQuest].Points = 1;

            Owner.SendMessage(0x23, "You have slain 500 Mongbats for Zarnak!");
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

    public class Zarnak : MondainQuester
    {
        public override bool IsActiveVendor => true;

        private List<SBInfo> m_SBInfos = new List<SBInfo>();
        protected override List<SBInfo> SBInfos => m_SBInfos;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBZarnak());
        }

        [Constructable]
        public Zarnak() : base("Zarnak", "Beast Whisperer")
        {
        }

        public Zarnak(Serial serial) : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 25);
            Female = false;
            Race = Race.Human;

            Hue = Utility.RandomSkinHue();
            HairItemID = 0x2044; // Long hair
            HairHue = Utility.RandomHairHue();
        }

        public override void InitOutfit()
        {
            AddItem(new Backpack());
            AddItem(new Sandals());
            AddItem(new Robe(0x59C)); // Natural tone
            AddItem(new WizardsHat(0x59B));
        }

        public override void VendorBuy(Mobile from)
        {
            PlayerMobile player = from as PlayerMobile;
            if (player != null)
            {
                var profile = player.AcquireTalents();
                if (profile.Talents.TryGetValue(TalentID.MongbatSlayerQuest, out var talent) && talent.Points > 0)
                {
                    SayTo(from, "You've earned my respect. Take your pick.");
                    base.VendorBuy(from);
                }
                else
                {
                    SayTo(from, "The Mongbats still screech. Return after 500 of them are gone.");
                }
            }
        }

        public override Type[] Quests => new Type[]
        {
            typeof(ZarnakQuest)
        };

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

    public class SBZarnak : SBInfo
    {
        private List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public override List<GenericBuyInfo> BuyInfo => m_BuyInfo;
        public override IShopSellInfo SellInfo => m_SellInfo;

        public class InternalBuyInfo : List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                Add(new AnimalBuyInfo(1, typeof(Mongbat), 5000, 5, 0xCE, 0)); // Tamed Mongbat
                Add(new GenericBuyInfo(typeof(KnightStone), 5000, 10, 0x1F13, 0));
                Add(new GenericBuyInfo(typeof(MaxxiaDust), 5000, 10, 0xF8F, 0));
                Add(new GenericBuyInfo(typeof(TrexSkull), 5000, 10, 0x1AE0, 0));
                Add(new GenericBuyInfo(typeof(RootThrone), 5000, 10, 0xB2F, 0));
                Add(new GenericBuyInfo(typeof(BabyLavos), 5000, 10, 0x1601, 0));
                Add(new GenericBuyInfo(typeof(CowToken), 5000, 10, 0xFBE, 0));
                Add(new GenericBuyInfo(typeof(WallFlowers), 5000, 10, 0xC96, 0));
                Add(new GenericBuyInfo(typeof(ColoredLamppost), 5000, 10, 0xB21, 0));
                Add(new GenericBuyInfo(typeof(EarthRelic), 5000, 10, 0x1F1D, 0));
                Add(new GenericBuyInfo(typeof(ExoticWhistle), 5000, 10, 0x14ED, 0));
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
