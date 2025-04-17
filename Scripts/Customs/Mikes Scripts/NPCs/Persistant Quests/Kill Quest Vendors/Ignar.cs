using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class IgnarFirelordQuest : BaseQuest
    {
        public override bool DoneOnce => true;

        public override object Title => "Trial by Flame";

        public override object Description =>
            "I am Ignar, Firelord and Keeper of the Inner Flame.\n\n" +
            "Long ago, rogue Fire Elementals broke from the core and began scorching the realms unchecked. " +
            "Their presence threatens the delicate balance of fire and earth.\n\n" +
            "Bring me proof of your strength: slay *five hundred* Fire Elementals. " +
            "Only then will you be deemed worthy to wield the flame and command its gifts.";

        public override object Refuse => "Then leave, before the fire consumes you.";

        public override object Uncomplete => "The flames still rage unchecked. Prove yourself.";

        public override object Complete =>
            "You return, seared and tempered by fire.\n\n" +
            "The rogue elementals no longer threaten us.\n" +
            "As promised, my shop is now open. Embrace the flame, and choose your reward wisely.";

        public IgnarFirelordQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(FireElemental), "Fire Elementals", 500));
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
        }

        public override void OnCompleted()
        {
            base.OnCompleted();

            var profile = Owner.AcquireTalents();
            if (!profile.Talents.ContainsKey(TalentID.FireElementalSlayerQuest))
                profile.Talents[TalentID.FireElementalSlayerQuest] = new Talent(TalentID.FireElementalSlayerQuest);

            profile.Talents[TalentID.FireElementalSlayerQuest].Points = 1;

            Owner.SendMessage(0x23, "You have slain 500 Fire Elementals for Ignar!");
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
            reader.ReadInt(); // version
        }
    }

    public class Ignar : MondainQuester
    {
        public override bool IsActiveVendor => true;

        private List<SBInfo> m_SBInfos = new List<SBInfo>();
        protected override List<SBInfo> SBInfos => m_SBInfos;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBIgnar());
        }

        [Constructable]
        public Ignar() : base("Ignar", "Firelord")
        {
        }

        public Ignar(Serial serial) : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(150, 150, 100);
            Female = false;
            Race = Race.Human;
            Hue = 0x497;

            HairItemID = 0x2048;
            HairHue = 1358;
            FacialHairItemID = 0x204C;
            FacialHairHue = HairHue;
        }

        public override void InitOutfit()
        {
            AddItem(new Robe { Hue = 1358 });
            AddItem(new Sandals { Hue = 1358 });
            AddItem(new Cloak { Hue = 0x489 });
        }

        public override void VendorBuy(Mobile from)
        {
            PlayerMobile player = from as PlayerMobile;
            if (player != null)
            {
                var profile = player.AcquireTalents();
                if (profile.Talents.TryGetValue(TalentID.FireElementalSlayerQuest, out Talent talent) && talent.Points > 0)
                {
                    SayTo(from, "You have earned the right. Browse my wares.");
                    base.VendorBuy(from);
                }
                else
                {
                    SayTo(from, "The flame rejects you. Return after you have slain 500 Fire Elementals.");
                }
            }
        }

        public override Type[] Quests => new Type[] { typeof(IgnarFirelordQuest) };

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt(); // version
        }
    }

    public class SBIgnar : SBInfo
    {
        private List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public override List<GenericBuyInfo> BuyInfo => m_BuyInfo;
        public override IShopSellInfo SellInfo => m_SellInfo;

        public class InternalBuyInfo : List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                Add(new AnimalBuyInfo(1, typeof(FireElemental), 5000, 5, 0x210, 0));
                Add(new GenericBuyInfo(typeof(NecromancyAugmentCrystal), 5000, 10, 0x1F19, 0));
                Add(new GenericBuyInfo(typeof(NinjitsuAugmentCrystal), 5000, 10, 0x1F19, 0));
                Add(new GenericBuyInfo(typeof(OneHandedTransformDeed), 5000, 10, 0x14F0, 0));
                Add(new GenericBuyInfo(typeof(ParryingAugmentCrystal), 5000, 10, 0x1F19, 0));
                Add(new GenericBuyInfo(typeof(PeacemakingAugmentCrystal), 5000, 10, 0x1F19, 0));
                Add(new GenericBuyInfo(typeof(PhysicalHitAreaCrystal), 5000, 10, 0x1F19, 0));
                Add(new GenericBuyInfo(typeof(PlateLeggingsOfCommand), 5000, 10, 0x1411, 0));
                Add(new GenericBuyInfo(typeof(PoisonHitAreaCrystal), 5000, 10, 0x1F19, 0));
                Add(new GenericBuyInfo(typeof(PoisoningAugmentCrystal), 5000, 10, 0x1F19, 0));
                Add(new GenericBuyInfo(typeof(PoisonResistAugmentCrystal), 5000, 10, 0x1F19, 0));
            }
        }

        public class InternalSellInfo : GenericSellInfo
        {
            public InternalSellInfo()
            {
                // Optional: Add items this vendor will buy from players
            }
        }
    }
}
