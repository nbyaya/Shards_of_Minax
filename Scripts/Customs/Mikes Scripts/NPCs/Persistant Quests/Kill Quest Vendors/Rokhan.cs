using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class RokhanOstardQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title => "The Wrath of Rokhan";
        public override object Description => 
            "Ah, friend. I am Rokhan, once master of the wild, now guardian of its vengeance.\n\n" +
            "The Frenzied Ostard has been hunted to near extinction in the wild—slain by glory seekers and poachers. " +
            "But those you now see are not wild... they are twisted. Corrupted by foul magics.\n\n" +
            "I need your aid to cull the madness. Hunt down 500 Frenzied Ostards. Bring balance back.\n\n" +
            "Do this, and I will grant you access to my stables and enchanted wares.";

        public override object Refuse => "A wise decision, perhaps. These beasts are relentless.";
        public override object Uncomplete => "You have not yet slain enough of the frenzied. The wild still suffers.";
        public override object Complete =>
            "I feel the balance shift. The corruption wanes.\n\n" +
            "You have done well, warrior. My trust—and my wares—are yours. Choose wisely.";

        public RokhanOstardQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(FrenziedOstard), "Frenzied Ostards", 500));
            AddReward(new BaseReward(typeof(Gold), 1000, "1000 Gold"));
        }

        public override void OnCompleted()
        {
            base.OnCompleted();

            var profile = Owner.AcquireTalents();
            if (!profile.Talents.ContainsKey(TalentID.FrenziedOstardSlayerQuest))
                profile.Talents[TalentID.FrenziedOstardSlayerQuest] = new Talent(TalentID.FrenziedOstardSlayerQuest);

            profile.Talents[TalentID.FrenziedOstardSlayerQuest].Points = 1;

            Owner.SendMessage(0x23, "You have culled 500 Frenzied Ostards and earned Rokhan's favor!");
            Owner.PlaySound(CompleteSound);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class Rokhan : MondainQuester
    {
        public override bool IsActiveVendor => true;

        private List<SBInfo> m_SBInfos = new List<SBInfo>();

        protected override List<SBInfo> SBInfos => m_SBInfos;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBRokhan());
        }

        [Constructable]
        public Rokhan() : base("Rokhan", "Ostard Tamer")
        {
        }

        public Rokhan(Serial serial) : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 25);

            Female = false;
            Race = Race.Human;

            Hue = Utility.RandomSkinHue();
            HairItemID = Utility.RandomList(0x203B, 0x2048);
            HairHue = Utility.RandomHairHue();
            FacialHairItemID = 0x2041;
            FacialHairHue = HairHue;
        }

        public override void InitOutfit()
        {
            AddItem(new Backpack());
            AddItem(new LeatherGloves());
            AddItem(new StuddedGorget());
            AddItem(new BearMask());
            AddItem(new LeatherChest());
            AddItem(new StuddedLegs());
            AddItem(new Boots());
            AddItem(new Cloak(Utility.RandomNeutralHue()));
        }

        public override void VendorBuy(Mobile from)
        {
            PlayerMobile player = from as PlayerMobile;
            if (player != null)
            {
                var profile = player.AcquireTalents();
                if (profile.Talents.TryGetValue(TalentID.FrenziedOstardSlayerQuest, out Talent talent) && talent.Points > 0)
                {
                    SayTo(from, "You’ve earned my trust. Browse as you please.");
                    base.VendorBuy(from);
                }
                else
                {
                    SayTo(from, "Only those who have culled the corruption may trade with me.");
                }
            }
        }

        public override Type[] Quests => new Type[] { typeof(RokhanOstardQuest) };

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

    public class SBRokhan : SBInfo
    {
        private List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public override List<GenericBuyInfo> BuyInfo => m_BuyInfo;
        public override IShopSellInfo SellInfo => m_SellInfo;

        public class InternalBuyInfo : List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                Add(new AnimalBuyInfo(1, typeof(FrenziedOstard), 5000, 10, 8493, 0));

                Add(new GenericBuyInfo(typeof(StealingAugmentCrystal), 5000, 20, 0x2FB7, 0));
                Add(new GenericBuyInfo(typeof(StealthAugmentCrystal), 5000, 20, 0x2FB7, 0));
                Add(new GenericBuyInfo(typeof(SwingSpeedAugmentCrystal), 5000, 20, 0x2FB7, 0));
                Add(new GenericBuyInfo(typeof(TacticsAugmentCrystal), 5000, 20, 0x2FB7, 0));
                Add(new GenericBuyInfo(typeof(TailoringAugmentCrystal), 5000, 20, 0x2FB7, 0));
                Add(new GenericBuyInfo(typeof(TalismanSlotChangeDeed), 5000, 10, 0x14F0, 0));
                Add(new GenericBuyInfo(typeof(TasteIDAugmentCrystal), 5000, 20, 0x2FB7, 0));
                Add(new GenericBuyInfo(typeof(ThrowingAugmentCrystal), 5000, 20, 0x2FB7, 0));
                Add(new GenericBuyInfo(typeof(TinkeringAugmentCrystal), 5000, 20, 0x2FB7, 0));
                Add(new GenericBuyInfo(typeof(TrackingAugmentCrystal), 5000, 20, 0x2FB7, 0));
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
