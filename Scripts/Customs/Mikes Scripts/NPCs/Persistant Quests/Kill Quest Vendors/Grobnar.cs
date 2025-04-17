using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class GrobnarQuest : BaseQuest
    {
        public override bool DoneOnce => true;

        public override object Title => "Purge the Orc Captain Horde";

        public override object Description => 
            "I am Grobnar, once a warrior, now a watcher.\n\n" +
            "The Orc Captains grow bolder, gathering forces and raiding with impunity.\n\n" +
            "Bring me the heads of 500 Orc Captains. Only then will I trust you with what remains of my trade.";

        public override object Refuse => 
            "Bah. You are not the one, then. Leave me be.";

        public override object Uncomplete => 
            "You have not yet slain enough Orc Captains. Their war drums still thunder.";

        public override object Complete =>
            "So... you did it. Their captains are no more. I am impressed.\n\n" +
            "As promised, my wares are now yours to purchase. May they serve you well.";

        public GrobnarQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(OrcCaptain), "Orc Captains", 500));
        }

        public override void OnCompleted()
        {
            base.OnCompleted();

            var profile = Owner.AcquireTalents();

            if (!profile.Talents.ContainsKey(TalentID.OrcCaptainSlayerQuest))
                profile.Talents[TalentID.OrcCaptainSlayerQuest] = new Talent(TalentID.OrcCaptainSlayerQuest);

            profile.Talents[TalentID.OrcCaptainSlayerQuest].Points = 1;

            Owner.SendMessage(0x23, "You have slain 500 Orc Captains for Grobnar!");
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

    public class Grobnar : MondainQuester
    {
        public override bool IsActiveVendor => true;

        private List<SBInfo> m_SBInfos = new List<SBInfo>();
        protected override List<SBInfo> SBInfos => m_SBInfos;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBGrobnar());
        }

        [Constructable]
        public Grobnar() : base("Grobnar", "Veteran of Orc Wars")
        {
        }

        public Grobnar(Serial serial) : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 25);
            Female = false;
            Race = Race.Human;
            Hue = Utility.RandomSkinHue();
            HairItemID = Utility.RandomList(0x2047, 0x2049);
            HairHue = Utility.RandomHairHue();
            FacialHairItemID = Utility.RandomList(0x203E, 0x2040);
            FacialHairHue = HairHue;
        }

        public override void InitOutfit()
        {
            AddItem(new Backpack());
            AddItem(new LeatherChest());
            AddItem(new LeatherLegs());
            AddItem(new Boots(Utility.RandomNeutralHue()));
            AddItem(new Cloak(Utility.RandomNeutralHue()));
            AddItem(new VikingSword());
        }

        public override void VendorBuy(Mobile from)
        {
            if (from is PlayerMobile player)
            {
                var profile = player.AcquireTalents();

                if (profile.Talents.TryGetValue(TalentID.OrcCaptainSlayerQuest, out var talent) && talent.Points > 0)
                {
                    SayTo(from, "You've proven yourself. My goods are yours to browse.");
                    base.VendorBuy(from);
                }
                else
                {
                    SayTo(from, "I deal only with those who have slain 500 Orc Captains. You are not yet worthy.");
                }
            }
        }

        public override Type[] Quests => new Type[] { typeof(GrobnarQuest) };

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

    public class SBGrobnar : SBInfo
    {
        private List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public override List<GenericBuyInfo> BuyInfo => m_BuyInfo;
        public override IShopSellInfo SellInfo => m_SellInfo;

        public class InternalBuyInfo : List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                Add(new AnimalBuyInfo(1, typeof(OrcCaptain), 5000, 10, 17, 0)); // Orc Captain pet

                Add(new GenericBuyInfo(typeof(LeatherStrapBelt), 5000, 20, 0, 0));
                Add(new GenericBuyInfo(typeof(JackPumpkin), 5000, 20, 0, 0));
                Add(new GenericBuyInfo(typeof(MiniYew), 5000, 20, 0, 0));
                Add(new GenericBuyInfo(typeof(ImportantFlag), 5000, 20, 0, 0));
                Add(new GenericBuyInfo(typeof(HutFlower), 5000, 20, 0, 0));
                Add(new GenericBuyInfo(typeof(CookingTalisman), 5000, 20, 0, 0));
                Add(new GenericBuyInfo(typeof(LampPostA), 5000, 20, 0, 0));
                Add(new GenericBuyInfo(typeof(LampPostB), 5000, 20, 0, 0));
                Add(new GenericBuyInfo(typeof(MasterGyro), 5000, 20, 0, 0));
                Add(new GenericBuyInfo(typeof(MemorialStone), 5000, 20, 0, 0));
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
