using System;
using Server;
using Server.Mobiles;
using Server.Engines.Quests;
using Server.Items;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class KazuroQuest : BaseQuest
    {
        public override bool DoneOnce => true;

        public override object Title => "The Wrath of 500 Lesser Hiryu";

        public override object Description => "I am Kazuro, once a noble beast tamer, now left broken by the havoc of Lesser Hiryu. " +
            "\n\nThey have scattered my prized herd, slain my companions, and stained the skies with their screeches." +
            "\n\nBring justice to these beasts. Slay 500 of them, and I will reward you with access to my most exotic wares—including a few tamed Lesser Hiryu of your own.";

        public override object Refuse => "Cowardice does not tame the skies. Return when you find your courage.";

        public override object Uncomplete => "You’ve not yet brought down enough of the beasts. Keep hunting the Lesser Hiryu.";

        public override object Complete => "You return... bloodied, but whole. The skies sing quieter now. As promised, my shop is open to you.";

        public KazuroQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(LesserHiryu), "Lesser Hiryu", 500));
            AddReward(new BaseReward(typeof(Gold), 2000, "2000 Gold"));
        }

        public override void OnCompleted()
        {
            base.OnCompleted();

            var profile = Owner.AcquireTalents();

            if (!profile.Talents.ContainsKey(TalentID.LesserHiryuSlayerQuest))
                profile.Talents[TalentID.LesserHiryuSlayerQuest] = new Talent(TalentID.LesserHiryuSlayerQuest);

            profile.Talents[TalentID.LesserHiryuSlayerQuest].Points = 1;

            Owner.SendMessage(0x23, "You have slain 500 Lesser Hiryu for Kazuro!");
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

    public class Kazuro : MondainQuester
    {
        public override bool IsActiveVendor => true;

        private List<SBInfo> m_SBInfos = new List<SBInfo>();

        protected override List<SBInfo> SBInfos => m_SBInfos;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBKazuro());
        }

        [Constructable]
        public Kazuro() : base("Kazuro", "Beast-Hunter of the Wind")
        {
        }

        public Kazuro(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(100, 100, 25);

            Female = false;
            Race = Race.Human;

            Hue = Utility.RandomSkinHue();
            HairItemID = 0x203C;
            HairHue = Utility.RandomHairHue();
            FacialHairItemID = 0x204B;
            FacialHairHue = HairHue;
        }

        public override void InitOutfit()
        {
            AddItem(new Backpack());
            AddItem(new Boots(Utility.RandomNeutralHue()));
            AddItem(new LeatherChest());
            AddItem(new LeatherLegs());
            AddItem(new BearMask());
            AddItem(new Cloak(0x455));
        }

        public override void VendorBuy(Mobile from)
        {
            if (from is PlayerMobile player)
            {
                var profile = player.AcquireTalents();
                if (profile.Talents.TryGetValue(TalentID.LesserHiryuSlayerQuest, out Talent talent) && talent.Points > 0)
                {
                    SayTo(from, "You've earned this. Browse with pride.");
                    base.VendorBuy(from);
                }
                else
                {
                    SayTo(from, "Return when you have slain 500 Lesser Hiryu.");
                }
            }
        }

        public override Type[] Quests => new Type[] { typeof(KazuroQuest) };

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

    public class SBKazuro : SBInfo
    {
        private List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public override List<GenericBuyInfo> BuyInfo => m_BuyInfo;
        public override IShopSellInfo SellInfo => m_SellInfo;

        public class InternalBuyInfo : List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                // Custom mount
                Add(new AnimalBuyInfo(1, typeof(LesserHiryu), 5000, 10, 17, 0));

                // Exotic vendor items
                Add(new GenericBuyInfo(typeof(SpiderTree), 5000, 20, 0x0D25, 0));
                Add(new GenericBuyInfo(typeof(RopeSpindle), 5000, 20, 0x0FA0, 0));
                Add(new GenericBuyInfo(typeof(DailyNewspaper), 5000, 20, 0x1ECD, 0));
                Add(new GenericBuyInfo(typeof(FunPumpkinCannon), 5000, 20, 0x0E81, 0));
                Add(new GenericBuyInfo(typeof(VirtueRune), 5000, 20, 0x1F14, 0));
                Add(new GenericBuyInfo(typeof(BonFire), 5000, 20, 0x0F7A, 0));
                Add(new GenericBuyInfo(typeof(MiniCherryTree), 5000, 20, 0x0D15, 0));
                Add(new GenericBuyInfo(typeof(SkullShield), 5000, 20, 0x1B74, 0));
                Add(new GenericBuyInfo(typeof(UnluckyDice), 5000, 20, 0x0FA7, 0));
                Add(new GenericBuyInfo(typeof(ImportantBooks), 5000, 20, 0x1F4C, 0));
            }
        }

        public class InternalSellInfo : GenericSellInfo
        {
            public InternalSellInfo() { }
        }
    }
}
