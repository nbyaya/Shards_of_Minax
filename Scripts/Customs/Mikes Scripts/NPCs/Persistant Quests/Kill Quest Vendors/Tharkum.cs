using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Engines.Quests;

namespace Server.Engines.Quests
{
    public class TharkumQuest : BaseQuest
    {
        public override bool DoneOnce => true;

        public override object Title => "The Minotaur Massacre";

        public override object Description => 
            "Ah, traveler! You smell of steel and courage.\n\n" +
            "The Minotaurs of the Deep Warrens have grown bold. They raid caravans, abduct villagers, and defile sacred ground.\n\n" +
            "I seek a true slayer—one who can cull their numbers.\n\n" +
            "Bring down *five hundred* of the beasts, and I will reward you with more than coin. I will open my vault to you—an arsenal of curios and creatures found nowhere else.\n\n" +
            "Go now, and let the labyrinths run red.";

        public override object Refuse => "Then you are not the warrior I hoped for. Return when your resolve is sharpened.";

        public override object Uncomplete => "You’ve slain many, but not enough. Return when five hundred Minotaurs lie dead.";

        public override object Complete => 
            "The air around you reeks of battle. You’ve done it...\n\n" +
            "The Minotaurs have been broken.\n\n" +
            "As promised, my stock is now yours to peruse. Choose wisely—these are no common wares.";

        public TharkumQuest()
        {
            AddObjective(new SlayObjective(typeof(Minotaur), "Minotaurs", 500));
            AddReward(new BaseReward(typeof(Gold), 1000, "1000 Gold"));
        }

        public override void OnCompleted()
        {
            base.OnCompleted();

            var profile = Owner.AcquireTalents();
            if (!profile.Talents.ContainsKey(TalentID.MinotaurSlayerQuest))
                profile.Talents[TalentID.MinotaurSlayerQuest] = new Talent(TalentID.MinotaurSlayerQuest);

            profile.Talents[TalentID.MinotaurSlayerQuest].Points = 1;

            Owner.SendMessage(0x23, "You have slain 500 Minotaurs for Tharkum!");
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

    public class Tharkum : MondainQuester
    {
        public override bool IsActiveVendor => true;
        private List<SBInfo> m_SBInfos = new List<SBInfo>();

        protected override List<SBInfo> SBInfos => m_SBInfos;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBTharkum());
        }

        [Constructable]
        public Tharkum() : base("Tharkum", "Minotaur Slayer")
        {
        }

        public Tharkum(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(100, 100, 25);
            Female = false;
            Race = Race.Human;
            Hue = Utility.RandomSkinHue();
            HairItemID = Utility.RandomList(0x203B, 0x2049);
            HairHue = Utility.RandomHairHue();
            FacialHairItemID = 0x2040;
            FacialHairHue = HairHue;
        }

        public override void InitOutfit()
        {
            AddItem(new Backpack());
            AddItem(new StuddedGloves());
            AddItem(new RingmailChest());
            AddItem(new StuddedLegs());
            AddItem(new BearMask());
        }

        public override void VendorBuy(Mobile from)
        {
            if (from is PlayerMobile player)
            {
                var profile = player.AcquireTalents();
                if (profile.Talents.TryGetValue(TalentID.MinotaurSlayerQuest, out var talent) && talent.Points > 0)
                {
                    SayTo(from, "The labyrinth is yours to plunder. Browse my wares.");
                    base.VendorBuy(from);
                }
                else
                {
                    SayTo(from, "Prove your strength. Slay 500 Minotaurs, and then we shall speak of trade.");
                }
            }
        }

        public override Type[] Quests => new Type[] { typeof(TharkumQuest) };

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

    public class SBTharkum : SBInfo
    {
        private List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public override List<GenericBuyInfo> BuyInfo => m_BuyInfo;
        public override IShopSellInfo SellInfo => m_SellInfo;

        public class InternalBuyInfo : List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                Add(new AnimalBuyInfo(1, typeof(Minotaur), 5000, 5, 0x190, 0)); // Adjust appearance if needed

                Add(new GenericBuyInfo(typeof(MilkingPail), 5000, 10, 0x14F8, 0));
                Add(new GenericBuyInfo(typeof(MonsterTrophy), 5000, 10, 0x2826, 0));
                Add(new GenericBuyInfo(typeof(DistilledEssence), 5000, 10, 0xF0D, 0));
                Add(new GenericBuyInfo(typeof(WigStand), 5000, 10, 0x1EB9, 0));
                Add(new GenericBuyInfo(typeof(BrassFountain), 5000, 10, 0xE2D, 0));
                Add(new GenericBuyInfo(typeof(HandOfFate), 5000, 10, 0x2B4C, 0));
                Add(new GenericBuyInfo(typeof(NameTapestry), 5000, 10, 0xEF0, 0));
                Add(new GenericBuyInfo(typeof(OpalBranch), 5000, 10, 0x1CD4, 0));
                Add(new GenericBuyInfo(typeof(SilverMirror), 5000, 10, 0x232A, 0));
                Add(new GenericBuyInfo(typeof(MarbleHourglass), 5000, 10, 0x1051, 0));
            }
        }

        public class InternalSellInfo : GenericSellInfo
        {
            public InternalSellInfo() { }
        }
    }
}
