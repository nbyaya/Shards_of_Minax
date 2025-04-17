using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;

namespace Server.Engines.Quests
{
    public class BeornQuest : BaseQuest
    {
        public override bool DoneOnce => true;

        public override object Title => "The Bear Hunt";
        public override object Description =>
            "Greetings, traveler.\n\n" +
            "I am Beorn, once a keeper of the wilds, now a seeker of vengeance.\n" +
            "A horde of Brown Bears have overrun the glades near my home, driven mad by corruption.\n\n" +
            "Hunt down 500 of these beasts. Only then shall I deem you worthy of my secrets—and wares.";

        public override object Refuse => "So be it. The wilds remain untamed, and the bears unchallenged.";
        public override object Uncomplete => "You have not yet culled enough of the wild. Return when 500 Brown Bears have fallen.";
        public override object Complete =>
            "The air smells of blood and fur... You have done it.\n\n" +
            "As promised, I shall now trade with you. Tamed Brown Bears and powerful crystals await.\n\n" +
            "Spend wisely.";

        public BeornQuest()
        {
            AddObjective(new SlayObjective(typeof(BrownBear), "Brown Bears", 500));
            AddReward(new BaseReward(typeof(Gold), 1000, "1000 Gold"));
        }

        public override void OnCompleted()
        {
            base.OnCompleted();

            var profile = Owner.AcquireTalents();
            if (!profile.Talents.ContainsKey(TalentID.BrownBearSlayerQuest))
                profile.Talents[TalentID.BrownBearSlayerQuest] = new Talent(TalentID.BrownBearSlayerQuest);

            profile.Talents[TalentID.BrownBearSlayerQuest].Points = 1;

            Owner.SendMessage(0x23, "You have completed the Hunt of 500 Brown Bears!");
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
            int version = reader.ReadInt();
        }
    }

    public class Beorn : MondainQuester
    {
        public override bool IsActiveVendor => true;

        private List<SBInfo> m_SBInfos = new List<SBInfo>();

        protected override List<SBInfo> SBInfos => m_SBInfos;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBBeorn());
        }

        [Constructable]
        public Beorn() : base("Beorn", "Warden of the Wilds") { }

        public Beorn(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(100, 100, 25);

            Female = false;
            Race = Race.Human;

            Hue = Utility.RandomSkinHue();
            HairItemID = Utility.RandomList(0x203B, 0x2049, 0x2048);
            HairHue = Utility.RandomHairHue();
            FacialHairItemID = Utility.RandomList(0x203E, 0x2040, 0x204B);
            FacialHairHue = HairHue;
        }

        public override void InitOutfit()
        {
            AddItem(new Backpack());
            AddItem(new FurCape());
            AddItem(new BearMask()); // Custom item or switch if not available
            AddItem(new StuddedChest());
            AddItem(new LeatherLegs());
            AddItem(new Boots());
        }

        public override void VendorBuy(Mobile from)
        {
            if (from is PlayerMobile player)
            {
                var profile = player.AcquireTalents();
                if (profile.Talents.TryGetValue(TalentID.BrownBearSlayerQuest, out var talent) && talent.Points > 0)
                {
                    SayTo(from, "You have proven yourself. My wares are open to you.");
                    base.VendorBuy(from);
                }
                else
                {
                    SayTo(from, "Return once you've slain 500 Brown Bears.");
                }
            }
        }

        public override Type[] Quests => new Type[]
        {
            typeof(BeornQuest)
        };

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class SBBeorn : SBInfo
    {
        private List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public override List<GenericBuyInfo> BuyInfo => m_BuyInfo;
        public override IShopSellInfo SellInfo => m_SellInfo;

        public class InternalBuyInfo : List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                Add(new AnimalBuyInfo(1, typeof(BrownBear), 1000, 10, 0x20D6, 0));

                // Crystals sold for 5000 gp each
                Add(new GenericBuyInfo("Fen Crystal", typeof(FenCrystal), 5000, 20, 0x1F19, 0));
                Add(new GenericBuyInfo("Rho Crystal", typeof(RhoCrystal), 5000, 20, 0x1F19, 0));
                Add(new GenericBuyInfo("Rys Crystal", typeof(RysCrystal), 5000, 20, 0x1F19, 0));
                Add(new GenericBuyInfo("Wyr Crystal", typeof(WyrCrystal), 5000, 20, 0x1F19, 0));
                Add(new GenericBuyInfo("Fre Crystal", typeof(FreCrystal), 5000, 20, 0x1F19, 0));
                Add(new GenericBuyInfo("Tor Crystal", typeof(TorCrystal), 5000, 20, 0x1F19, 0));
                Add(new GenericBuyInfo("Vel Crystal", typeof(VelCrystal), 5000, 20, 0x1F19, 0));
                Add(new GenericBuyInfo("Xen Crystal", typeof(XenCrystal), 5000, 20, 0x1F19, 0));
                Add(new GenericBuyInfo("Pol Crystal", typeof(PolCrystal), 5000, 20, 0x1F19, 0));
                Add(new GenericBuyInfo("Wol Crystal", typeof(WolCrystal), 5000, 20, 0x1F19, 0));
            }
        }

        public class InternalSellInfo : GenericSellInfo
        {
            public InternalSellInfo()
            {
                // Optional: Sell back prices if needed
            }
        }
    }
}
