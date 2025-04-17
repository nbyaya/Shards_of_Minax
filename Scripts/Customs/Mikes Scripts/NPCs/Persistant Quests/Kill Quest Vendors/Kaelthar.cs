using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class KaeltharDragonQuest : BaseQuest
    {
        public override bool DoneOnce => true;

        public override object Title => "Wyrmscourge: Slay 500 Dragons";

        public override object Description => 
            "I am Kaelthar, once a dragon-tamer, now their eternal foe. \n\n" +
            "The skies burn with fire, the ground trembles beneath scaled tyrants. " +
            "Slay 500 Dragons and prove your strength. Only then will I entrust you with the power to command them.";

        public override object Refuse => 
            "Then go. Let the fire consume you, coward.";

        public override object Uncomplete => 
            "Their wings still darken the skies. 500 dragons must fall before I acknowledge your might.";

        public override object Complete => 
            "The heavens grow quiet. You have brought ruin to the draconic scourge.\n\n" +
            "As promised, I shall teach you to command their kind—and more. My shop is now open to you.";

        public KaeltharDragonQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(Dragon), "Dragons", 500));
            AddReward(new BaseReward(typeof(Gold), 10000, "10,000 Gold"));
        }

        public override void OnCompleted()
        {
            base.OnCompleted();

            var profile = Owner.AcquireTalents();
            if (!profile.Talents.ContainsKey(TalentID.DragonSlayerQuest))
                profile.Talents[TalentID.DragonSlayerQuest] = new Talent(TalentID.DragonSlayerQuest);
            
            profile.Talents[TalentID.DragonSlayerQuest].Points = 1;

            Owner.SendMessage(0x23, "You have completed Kaelthar’s challenge!");
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
            reader.ReadInt();
        }
    }

    public class Kaelthar : MondainQuester
    {
        private List<SBInfo> m_SBInfos = new List<SBInfo>();

        public override bool IsActiveVendor => true;
        protected override List<SBInfo> SBInfos => m_SBInfos;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBKaelthar());
        }

        [Constructable]
        public Kaelthar() : base("Kaelthar", "The Wyrmscourge")
        {
        }

        public Kaelthar(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(150, 150, 100);
            Female = false;
            Race = Race.Human;

            Hue = Utility.RandomSkinHue();
            HairItemID = Utility.RandomList(0x203B, 0x2048);
            HairHue = Utility.RandomHairHue();
            FacialHairItemID = Utility.RandomList(0x203E, 0x2040);
            FacialHairHue = HairHue;
        }

        public override void InitOutfit()
        {
            AddItem(new Backpack());
            AddItem(new DragonChest());
            AddItem(new Boots(1109));
            AddItem(new PlateLegs());
            AddItem(new Cloak(1157));
        }

        public override void VendorBuy(Mobile from)
        {
            PlayerMobile player = from as PlayerMobile;
            if (player != null)
            {
                var profile = player.AcquireTalents();
                if (profile.Talents.TryGetValue(TalentID.DragonSlayerQuest, out var talent) && talent.Points > 0)
                {
                    SayTo(from, "You have proven your might. Browse the relics of my hoard.");
                    base.VendorBuy(from);
                }
                else
                {
                    SayTo(from, "Only those who’ve slain 500 Dragons may earn my trust.");
                }
            }
        }

        public override Type[] Quests => new Type[] { typeof(KaeltharDragonQuest) };

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

    public class SBKaelthar : SBInfo
    {
        private List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public override List<GenericBuyInfo> BuyInfo => m_BuyInfo;
        public override IShopSellInfo SellInfo => m_SellInfo;

        public class InternalBuyInfo : List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                Add(new AnimalBuyInfo(1, typeof(Dragon), 5000, 5, 0xD0A, 0)); // Tamed Dragon

                Add(new GenericBuyInfo(typeof(BanishingOrb), 5000, 10, 0xF8D, 0));
                Add(new GenericBuyInfo(typeof(BanishingRod), 5000, 10, 0x1F2B, 0));
                Add(new GenericBuyInfo(typeof(BeggingAugmentCrystal), 5000, 10, 0x1F19, 0));
                Add(new GenericBuyInfo(typeof(BeltSlotChangeDeed), 5000, 10, 0x14F0, 0));
                Add(new GenericBuyInfo(typeof(BlacksmithyAugmentCrystal), 5000, 10, 0x1F19, 0));
                Add(new GenericBuyInfo(typeof(BootsOfCommand), 5000, 10, 0x170B, 0));
                Add(new GenericBuyInfo(typeof(BraceletSlotChangeDeed), 5000, 10, 0x14F0, 0));
                Add(new GenericBuyInfo(typeof(CapacityIncreaseDeed), 5000, 10, 0x14F0, 0));
                Add(new GenericBuyInfo(typeof(ChestSlotChangeDeed), 5000, 10, 0x14F0, 0));
                Add(new GenericBuyInfo(typeof(DetectingHiddenAugmentCrystal), 5000, 10, 0x1F19, 0));
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
