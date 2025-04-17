using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class MurkwadeQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title => "The Scourge of 500 Alligators";

        public override object Description =>
            "You there. Yes, you. I've lost kin to those scaly beasts lurking in the swamps.\n\n" +
            "The alligators have grown bold—taking livestock, dragging folk off trails, even snapping at my boots.\n\n" +
            "Slay *five hundred* of those wretched creatures, and I'll open my private stock to you. " +
            "Rare wares I’ve collected over the years—fit for a killer of monsters.";

        public override object Refuse => "Hmph. Then stay outta the swamps, lest you end up gator chow.";
        public override object Uncomplete => "Still alive, eh? But the swamps are still crawling. Keep killin'.";
        public override object Complete =>
            "Ha! Look at you! You reek of swamp water and death.\n\n" +
            "Good work, friend. The gator count's dropped, and so has my blood pressure.\n" +
            "As promised, my wares are yours to browse. Spend well, killer.";

        public MurkwadeQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(Alligator), "Alligators", 500));
            AddReward(new BaseReward(typeof(Gold), 2000, "2000 Gold"));
        }

        public override void OnCompleted()
        {
            base.OnCompleted();

            var profile = Owner.AcquireTalents();
            if (!profile.Talents.ContainsKey(TalentID.AlligatorSlayerQuest))
                profile.Talents[TalentID.AlligatorSlayerQuest] = new Talent(TalentID.AlligatorSlayerQuest);

            profile.Talents[TalentID.AlligatorSlayerQuest].Points = 1;

            Owner.SendMessage(0x23, "You have slain 500 Alligators for Murkwade!");
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

    public class Murkwade : MondainQuester
    {
        public override bool IsActiveVendor => true;
        private List<SBInfo> m_SBInfos = new List<SBInfo>();

        protected override List<SBInfo> SBInfos => m_SBInfos;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBMurkwade());
        }

        [Constructable]
        public Murkwade() : base("Murkwade", "Swamp Hunter")
        {
        }

        public Murkwade(Serial serial) : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 25);

            Female = false;
            Race = Race.Human;

            Hue = Utility.RandomSkinHue();
            HairItemID = 0x2049;
            HairHue = Utility.RandomHairHue();
        }

        public override void InitOutfit()
        {
            AddItem(new Backpack());
            AddItem(new Boots(Utility.RandomNeutralHue()));
            AddItem(new LongPants(Utility.RandomNeutralHue()));
            AddItem(new FancyShirt(Utility.RandomNeutralHue()));
            AddItem(new Cloak(Utility.RandomNeutralHue()));
        }

        public override void VendorBuy(Mobile from)
        {
            PlayerMobile player = from as PlayerMobile;
            if (player != null)
            {
                var profile = player.AcquireTalents();
                if (profile.Talents.TryGetValue(TalentID.AlligatorSlayerQuest, out var talent) && talent.Points > 0)
                {
                    SayTo(from, "You earned it. Take a look at what I’ve got.");
                    base.VendorBuy(from);
                }
                else
                {
                    SayTo(from, "You haven’t earned the right to trade with me. Kill more gators.");
                }
            }
        }

        public override Type[] Quests => new Type[] { typeof(MurkwadeQuest) };

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

    public class SBMurkwade : SBInfo
    {
        private List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public SBMurkwade() { }

        public override List<GenericBuyInfo> BuyInfo => m_BuyInfo;
        public override IShopSellInfo SellInfo => m_SellInfo;

        public class InternalBuyInfo : List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                Add(new AnimalBuyInfo(1, typeof(Alligator), 5000, 10, 17, 0));

                Add(new GenericBuyInfo(typeof(MeteorWard), 5000, 10, 0x1B74, 0));
                Add(new GenericBuyInfo(typeof(MonksSoulGloves), 5000, 10, 0x13C6, 0));
                Add(new GenericBuyInfo(typeof(NecromancersHood), 5000, 10, 0x1718, 0));
                Add(new GenericBuyInfo(typeof(DavidsSling), 5000, 10, 0x13B2, 0));
                Add(new GenericBuyInfo(typeof(EarthshakerMaul), 5000, 10, 0x143B, 0));
                Add(new GenericBuyInfo(typeof(CaesarChest), 5000, 10, 0x13BF, 0));
                Add(new GenericBuyInfo(typeof(ColonialPioneersCache), 5000, 10, 0x1E5E, 0));
                Add(new GenericBuyInfo(typeof(DoctorsBag), 5000, 10, 0xE76, 0));
                Add(new GenericBuyInfo(typeof(DynastyRelicsChest), 5000, 10, 0xE43, 0));
                Add(new GenericBuyInfo(typeof(RibbonAward), 5000, 10, 0x1F03, 0));
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
