using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;

namespace Server.Engines.Quests
{
    public class HarpySlayerQuest : BaseQuest
    {
        public override bool DoneOnce => true;

        public override object Title => "Feathers of Vengeance";
        public override object Description => 
            "The skies scream with the cries of Harpies. They have grown bold, defiling sacred peaks and attacking travelers below.\n\n" +
            "I am Nyara, Skycaller of the Eastern Winds. I seek a soul with sharp steel and sharper purpose.\n\n" +
            "Slay *five hundred* Harpies. Only then shall I share with you rare wonders—spoils from the sky and strange relics of wind-born magic.";

        public override object Refuse => 
            "The skies remain troubled. Return when the wind carries purpose in your heart.";

        public override object Uncomplete => 
            "The skies still echo with the shrieks of Harpies. Continue your hunt, windwalker.";

        public override object Complete => 
            "I feel the calm after a storm—your storm.\n\n" +
            "The Harpies fall silent thanks to your hand. As promised, my shop now opens to you. Within: curious things, gathered in my journeying and taming.\n\n" +
            "Choose wisely, for not all treasures are gold.";

        public HarpySlayerQuest()
        {
            AddObjective(new SlayObjective(typeof(Harpy), "Harpies", 500));
        }

        public override void OnCompleted()
        {
            base.OnCompleted();

            var profile = Owner.AcquireTalents();
            if (!profile.Talents.ContainsKey(TalentID.HarpySlayerQuest))
                profile.Talents[TalentID.HarpySlayerQuest] = new Talent(TalentID.HarpySlayerQuest);

            profile.Talents[TalentID.HarpySlayerQuest].Points = 1;

            Owner.SendMessage(0x23, "You have slain 500 Harpies for Nyara!");
            Owner.PlaySound(CompleteSound);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt();
        }
    }

    public class NyaraTheSkycaller : MondainQuester
    {
        public override bool IsActiveVendor => true;
        private List<SBInfo> m_SBInfos = new List<SBInfo>();

        protected override List<SBInfo> SBInfos => m_SBInfos;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBNyara());
        }

        [Constructable]
        public NyaraTheSkycaller() : base("Nyara", "Skycaller of the Eastern Winds")
        {
        }

        public NyaraTheSkycaller(Serial serial) : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 25);
            Female = true;
            Race = Race.Human;
            Hue = 0x83EA;
            HairItemID = 0x2047;
            HairHue = 0x455;
        }

        public override void InitOutfit()
        {
            AddItem(new Backpack());
            AddItem(new Sandals());
            AddItem(new Skirt(0x1BB));
            AddItem(new FancyShirt(0x47E));
            AddItem(new FeatheredHat(0x59B));
        }

        public override void VendorBuy(Mobile from)
        {
            if (from is PlayerMobile player)
            {
                var profile = player.AcquireTalents();
                if (profile.Talents.TryGetValue(TalentID.HarpySlayerQuest, out Talent talent) && talent.Points > 0)
                {
                    SayTo(player, "Welcome, featherbreaker. My curios are yours to explore.");
                    base.VendorBuy(from);
                }
                else
                {
                    SayTo(player, "You must prove yourself. Return when 500 Harpies lie in your wake.");
                }
            }
        }

        public override Type[] Quests => new Type[] { typeof(HarpySlayerQuest) };

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt();
        }
    }

    public class SBNyara : SBInfo
    {
        private List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public override List<GenericBuyInfo> BuyInfo => m_BuyInfo;
        public override IShopSellInfo SellInfo => m_SellInfo;

        public class InternalBuyInfo : List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                Add(new AnimalBuyInfo(1, typeof(Harpy), 5000, 10, 0x5D1, 0));
                Add(new GenericBuyInfo(typeof(FunMushroom), 5000, 10, 0x1718, 0));
                Add(new GenericBuyInfo(typeof(ReactiveHormones), 5000, 10, 0xE2B, 0));
                Add(new GenericBuyInfo(typeof(CandleStick), 5000, 10, 0x1852, 0));
                Add(new GenericBuyInfo(typeof(LuckyDice), 5000, 10, 0xFA7, 0));
                Add(new GenericBuyInfo(typeof(JudasCradle), 5000, 10, 0x9B5, 0));
                Add(new GenericBuyInfo(typeof(GlassFurnace), 5000, 10, 0xFAC, 0));
                Add(new GenericBuyInfo(typeof(LovelyLilies), 5000, 10, 0xC83, 0));
                Add(new GenericBuyInfo(typeof(CupOfSlime), 5000, 10, 0x1F7A, 0));
                Add(new GenericBuyInfo(typeof(GingerbreadCookie), 5000, 10, 0x160B, 0));
                Add(new GenericBuyInfo(typeof(CarvingMachine), 5000, 10, 0x1034, 0));
            }
        }

        public class InternalSellInfo : GenericSellInfo
        {
        }
    }
}
