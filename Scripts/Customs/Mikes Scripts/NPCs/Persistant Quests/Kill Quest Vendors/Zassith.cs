using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;

namespace Server.Engines.Quests
{
    public class ZassithQuest : BaseQuest
    {
        public override bool DoneOnce => true;

        public override object Title => "Purge the Ophidians";
        public override object Description =>
            "Hissss... I am Zassith, last of the Venerated Circle.\n\n" +
            "The Ophidian Warriors were once our proud defenders... now they are corrupted, mindless, lost.\n\n" +
            "Bring death to *five hundred* of them. Cleanse their madness. And I shall reward you with gifts—rare items, and loyal hatchlings.\n\n" +
            "Return when the hiss of five hundred warriors has faded into silence.";

        public override object Refuse => "Then leave, outsider. You are not the purifier we seek.";
        public override object Uncomplete => "The madness still reigns. Purge the corrupted warriors... bring silence.";
        public override object Complete =>
            "Yessss... the cries have fallen silent. You have done well, outsider.\n\n" +
            "Now, I shall share what remains of our sacred things.\n" +
            "Choose wisely. They are not mere trinkets.";

        public ZassithQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(OphidianWarrior), "Ophidian Warriors", 500));
            AddReward(new BaseReward(typeof(Gold), 1000, "1000 Gold"));
        }

        public override void OnCompleted()
        {
            base.OnCompleted();

            var profile = Owner.AcquireTalents();
            if (!profile.Talents.ContainsKey(TalentID.OphidianWarriorSlayer))
                profile.Talents[TalentID.OphidianWarriorSlayer] = new Talent(TalentID.OphidianWarriorSlayer);

            profile.Talents[TalentID.OphidianWarriorSlayer].Points = 1;

            Owner.SendMessage(0x23, "You have slain 500 Ophidian Warriors!");
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

    public class Zassith : MondainQuester
    {
        public override bool IsActiveVendor => true;

        private List<SBInfo> m_SBInfos = new List<SBInfo>();
        protected override List<SBInfo> SBInfos => m_SBInfos;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBZassith());
        }

        [Constructable]
        public Zassith() : base("Zassith", "The Last Venerated")
        {
        }

        public Zassith(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(100, 100, 25);

            Body = 86; // Ophidian body
            Hue = 0x455; // Custom darkened hue
            NameHue = 0x22;
        }

        public override void InitOutfit()
        {
            AddItem(new Backpack());
        }

        public override void VendorBuy(Mobile from)
        {
            if (from is PlayerMobile player)
            {
                var profile = player.AcquireTalents();
                if (profile.Talents.TryGetValue(TalentID.OphidianWarriorSlayer, out var talent) && talent.Points > 0)
                {
                    SayTo(from, "You have purged the madness. My relics are yours to claim.");
                    base.VendorBuy(from);
                }
                else
                {
                    SayTo(from, "You have not yet cleansed enough corrupted warriors. Return when 500 lie dead.");
                }
            }
        }

        public override Type[] Quests => new Type[] { typeof(ZassithQuest) };

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

    public class SBZassith : SBInfo
    {
        private List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public override List<GenericBuyInfo> BuyInfo => m_BuyInfo;
        public override IShopSellInfo SellInfo => m_SellInfo;

        public class InternalBuyInfo : List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                Add(new AnimalBuyInfo(1, typeof(OphidianWarrior), 5000, 5, 500, 0));

                Add(new GenericBuyInfo(typeof(FancyCopperWings), 5000, 10, 0, 0));
                Add(new GenericBuyInfo(typeof(RareGrease), 5000, 10, 0, 0));
                Add(new GenericBuyInfo(typeof(EnchantedRose), 5000, 10, 0, 0));
                Add(new GenericBuyInfo(typeof(HeartChair), 5000, 10, 0, 0));
                Add(new GenericBuyInfo(typeof(PicnicDayBasket), 5000, 10, 0, 0));
                Add(new GenericBuyInfo(typeof(HorseToken), 5000, 10, 0, 0));
                Add(new GenericBuyInfo(typeof(AutoPounder), 5000, 10, 0, 0));
                Add(new GenericBuyInfo(typeof(WatermelonTray), 5000, 10, 0, 0));
                Add(new GenericBuyInfo(typeof(MasterTrumpet), 5000, 10, 0, 0));
                Add(new GenericBuyInfo(typeof(WeaponBottle), 5000, 10, 0, 0));
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
