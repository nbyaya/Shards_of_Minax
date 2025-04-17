using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class UltharQuest : BaseQuest
    {
        public override bool DoneOnce => true;

        public override object Title => "The Wrangling of 500 Forest Ostards";

        public override object Description =>
            "Well met, wanderer. I am Ulthar, the Tamer of Untamed Things.\n\n" +
            "My life's work is the study and control of Forest Ostards—noble beasts with fire in their veins. " +
            "But lately, their numbers have surged beyond the bounds of balance.\n\n" +
            "Slay 500 of these wild beasts and restore the equilibrium of the wilds. In return, " +
            "I will open my stable and sell to you trained Forest Ostards—and curiosities for the cunning.\n\n" +
            "Tread carefully, for the forest is always watching.";

        public override object Refuse =>
            "Then the wild shall remain wild. Return if your heart yearns for the hunt.";

        public override object Uncomplete =>
            "The Ostards still rule the forest. Hunt them down, all five hundred.";

        public override object Complete =>
            "Your hands reek of forest blood, and your eyes hold the clarity of purpose.\n" +
            "The balance is restored, and your deeds are etched in the wind.\n\n" +
            "My stable is yours to browse. Choose wisely—these wares are for hunters true.";

        public UltharQuest()
        {
            AddObjective(new SlayObjective(typeof(ForestOstard), "Forest Ostards", 500));
            AddReward(new BaseReward(typeof(Gold), 1000, "1000 Gold Coins"));
        }

        public override void OnCompleted()
        {
            base.OnCompleted();

            var profile = Owner.AcquireTalents();
            if (!profile.Talents.ContainsKey(TalentID.ForestOstardHunter))
                profile.Talents[TalentID.ForestOstardHunter] = new Talent(TalentID.ForestOstardHunter);

            profile.Talents[TalentID.ForestOstardHunter].Points = 1;

            Owner.SendMessage(0x23, "You have slain 500 Forest Ostards for Ulthar!");
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
            reader.ReadInt();
        }
    }

    public class Ulthar : MondainQuester
    {
        private List<SBInfo> m_SBInfos = new List<SBInfo>();

        protected override List<SBInfo> SBInfos => m_SBInfos;

        public override bool IsActiveVendor => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBUlthar());
        }

        [Constructable]
        public Ulthar() : base("Ulthar", "Beastmaster of the Wilds")
        {
        }

        public Ulthar(Serial serial) : base(serial)
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
            AddItem(new LeatherChest());
            AddItem(new LeatherLegs());
            AddItem(new Boots());
            AddItem(new Cloak(Utility.RandomNeutralHue()));
        }

        public override void VendorBuy(Mobile from)
        {
            if (from is PlayerMobile player)
            {
                var profile = player.AcquireTalents();
                if (profile.Talents.TryGetValue(TalentID.ForestOstardHunter, out Talent talent) && talent.Points > 0)
                {
                    SayTo(from, "You've earned my respect. Browse my beasts and trinkets.");
                    base.VendorBuy(from);
                }
                else
                {
                    SayTo(from, "Earn my trust first. Kill 500 Forest Ostards, then we’ll talk.");
                }
            }
        }

        public override Type[] Quests => new Type[] { typeof(UltharQuest) };

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

    public class SBUlthar : SBInfo
    {
        private List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public override List<GenericBuyInfo> BuyInfo => m_BuyInfo;
        public override IShopSellInfo SellInfo => m_SellInfo;

        private class InternalBuyInfo : List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                Add(new AnimalBuyInfo(1, typeof(ForestOstard), 5000, 5, 8499, 0));
                Add(new GenericBuyInfo(typeof(ProvocationAugmentCrystal), 5000, 20, 0x2FB7, 0));
                Add(new GenericBuyInfo(typeof(RemoveTrapAugmentCrystal), 5000, 20, 0x2FB7, 0));
                Add(new GenericBuyInfo(typeof(ResistingSpellsAugmentCrystal), 5000, 20, 0x2FB7, 0));
                Add(new GenericBuyInfo(typeof(RingSlotChangeDeed), 5000, 20, 0x14F0, 0));
                Add(new GenericBuyInfo(typeof(RodOfOrcControl), 5000, 20, 0x1F52, 0));
                Add(new GenericBuyInfo(typeof(ShirtSlotChangeDeed), 5000, 20, 0x2799, 0));
                Add(new GenericBuyInfo(typeof(SnoopingAugmentCrystal), 5000, 20, 0x2FB7, 0));
                Add(new GenericBuyInfo(typeof(SpellweavingAugmentCrystal), 5000, 20, 0x2FB7, 0));
                Add(new GenericBuyInfo(typeof(SpiritSpeakAugmentCrystal), 5000, 20, 0x2FB7, 0));
                Add(new GenericBuyInfo(typeof(StaminaLeechAugmentCrystal), 5000, 20, 0x2FB7, 0));
            }
        }

        private class InternalSellInfo : GenericSellInfo
        {
            public InternalSellInfo()
            {
                // No selling to this vendor
            }
        }
    }
}
