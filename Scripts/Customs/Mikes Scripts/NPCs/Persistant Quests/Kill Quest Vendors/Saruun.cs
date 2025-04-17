using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;

namespace Server.Engines.Quests
{
    public class SaruunDesertOstardQuest : BaseQuest
    {
        public override bool DoneOnce => true;

        public override object Title => "Culling the Wild Herd";

        public override object Description => 
            "Ah, greetings wanderer. I am Saruun, once a master tamer of the eastern sands.\n\n" +
            "The wild Desert Ostards have multiplied beyond reckoning. Their hunger strips the dunes bare, and their shrieks echo across the dunes.\n\n" +
            "Slay *five hundred* of the beasts and return to me. Only then shall you earn the trust of a true tamer... and access to my exotic wares.";

        public override object Refuse => "Then go. Let the sands bury your cowardice.";

        public override object Uncomplete => "The dunes still roar with the cries of living Ostards. Return when your hunt is complete.";

        public override object Complete => 
            "The sands are quieter now. You’ve done well, hunter. As promised, I now offer you my treasures — tamed creatures and rare earths from forgotten depths.\n\n" +
            "Browse them well. These wares are not for the unproven.";

        public SaruunDesertOstardQuest()
        {
            AddObjective(new SlayObjective(typeof(DesertOstard), "Desert Ostards", 500));
            AddReward(new BaseReward(typeof(Gold), 1000, "1000 Gold"));
        }

        public override void OnCompleted()
        {
            base.OnCompleted();

            var profile = Owner.AcquireTalents();
            if (!profile.Talents.ContainsKey(TalentID.DesertOstardQuest))
                profile.Talents[TalentID.DesertOstardQuest] = new Talent(TalentID.DesertOstardQuest);
            
            profile.Talents[TalentID.DesertOstardQuest].Points = 1;

            Owner.SendMessage(0x23, "You have slain 500 Desert Ostards for Saruun!");
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

    public class Saruun : MondainQuester
    {
        public override bool IsActiveVendor => true;
        private List<SBInfo> m_SBInfos = new List<SBInfo>();

        protected override List<SBInfo> SBInfos => m_SBInfos;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBSaruun());
        }

        [Constructable]
        public Saruun() : base("Saruun", "Tamer of Dust")
        {
        }

        public Saruun(Serial serial) : base(serial)
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
            FacialHairItemID = 0x204C;
            FacialHairHue = HairHue;
        }

        public override void InitOutfit()
        {
            AddItem(new Sandals(Utility.RandomNeutralHue()));
            AddItem(new Robe(Utility.RandomYellowHue()));
            AddItem(new Cloak(Utility.RandomYellowHue()));
            AddItem(new GnarledStaff());
        }

        public override void VendorBuy(Mobile from)
        {
            if (from is PlayerMobile player)
            {
                var profile = player.AcquireTalents();
                if (profile.Talents.TryGetValue(TalentID.DesertOstardQuest, out var talent) && talent.Points > 0)
                {
                    SayTo(from, "You have proven yourself. My wares are yours to browse.");
                    base.VendorBuy(from);
                }
                else
                {
                    SayTo(from, "The herd still runs wild. Return after you’ve slain 500 Desert Ostards.");
                }
            }
        }

        public override Type[] Quests => new Type[]
        {
            typeof(SaruunDesertOstardQuest)
        };

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

    public class SBSaruun : SBInfo
    {
        private List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public SBSaruun()
        {
        }

        public override List<GenericBuyInfo> BuyInfo => m_BuyInfo;
        public override IShopSellInfo SellInfo => m_SellInfo;

        public class InternalBuyInfo : List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                Add(new AnimalBuyInfo(1, typeof(DesertOstard), 5000, 10, 0x211F, 0));

                Add(new GenericBuyInfo(typeof(AncientSapphire), 5000, 20, 0xF21, 0));
                Add(new GenericBuyInfo(typeof(MythicSkull), 5000, 20, 0x1AE0, 0));
                Add(new GenericBuyInfo(typeof(AncientSkull), 5000, 20, 0x1AE0, 0));
                Add(new GenericBuyInfo(typeof(LegendarySkull), 5000, 20, 0x1AE0, 0));
                Add(new GenericBuyInfo(typeof(GlimmeringGranite), 5000, 20, 0x1779, 0));
                Add(new GenericBuyInfo(typeof(GlimmeringClay), 5000, 20, 0x1666, 0));
                Add(new GenericBuyInfo(typeof(GlimmeringHeartstone), 5000, 20, 0xF8B, 0));
                Add(new GenericBuyInfo(typeof(GlimmeringGypsum), 5000, 20, 0xF8C, 0));
                Add(new GenericBuyInfo(typeof(GlimmeringIronOre), 5000, 20, 0x19B9, 0));
                Add(new GenericBuyInfo(typeof(GlimmeringOnyx), 5000, 20, 0x319A, 0));
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
