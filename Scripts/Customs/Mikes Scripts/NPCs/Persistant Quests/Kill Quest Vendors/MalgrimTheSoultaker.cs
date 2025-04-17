using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;

namespace Server.Engines.Quests
{
    public class MalgrimQuest : BaseQuest
    {
        public override bool DoneOnce => true;

        public override object Title => "Souls for the Soultaker";

        public override object Description => 
            "You approach Malgrim, the Soultaker. His eyes glow with a cold violet fire.\n\n" +
            "\"I seek power. Power drawn from the death of the Lich Lords who defy me. Slay them—five hundred of them. " +
            "Bring silence to their eternal howls. In return, I shall trade you forbidden things from beyond this world.\"\n\n" +
            "Return to me when their phylacteries lie shattered.";

        public override object Refuse => "Cowardice is unbecoming. Return only if your heart turns black with resolve.";

        public override object Uncomplete => "The air still stinks of undeath. You haven’t finished the task.";

        public override object Complete => 
            "\"The screams have ceased. I feel their souls fueling my ascension. As promised, my wares are now yours to purchase.\"\n\n" +
            "Malgrim gestures and shadowy items appear within a spectral display.";

        public MalgrimQuest()
        {
            AddObjective(new SlayObjective(typeof(LichLord), "Lich Lords", 500));

            // Optional reward
            AddReward(new BaseReward(typeof(Gold), 2500, "2500 Gold"));
        }

        public override void OnCompleted()
        {
            base.OnCompleted();

            var profile = Owner.AcquireTalents();
            if (!profile.Talents.ContainsKey(TalentID.LichLordHunterQuest))
                profile.Talents[TalentID.LichLordHunterQuest] = new Talent(TalentID.LichLordHunterQuest);

            profile.Talents[TalentID.LichLordHunterQuest].Points = 1;

            Owner.SendMessage(0x23, "You have fulfilled Malgrim's dark request.");
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

    public class MalgrimTheSoultaker : MondainQuester
    {
        public override bool IsActiveVendor => true;

        private List<SBInfo> m_SBInfos = new List<SBInfo>();
        protected override List<SBInfo> SBInfos => m_SBInfos;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBMalgrim());
        }

        [Constructable]
        public MalgrimTheSoultaker() : base("Malgrim", "The Soultaker")
        {
        }

		public MalgrimTheSoultaker(Serial serial) : base(serial)
		{
		}

        public override void InitBody()
        {
            InitStats(100, 100, 25);
            Female = false;
            Race = Race.Human;
            Hue = 0x83EA;

            HairItemID = 0x2047;
            HairHue = 1150;
            FacialHairItemID = 0x203E;
            FacialHairHue = HairHue;
        }

        public override void InitOutfit()
        {
            AddItem(new Robe() { Hue = 1109 });
            AddItem(new Sandals() { Hue = 0 });
            AddItem(new Cloak() { Hue = 1175 });
            AddItem(new Backpack());
        }

        public override void VendorBuy(Mobile from)
        {
            PlayerMobile player = from as PlayerMobile;
            if (player != null)
            {
                var profile = player.AcquireTalents();
                if (profile.Talents.TryGetValue(TalentID.LichLordHunterQuest, out Talent talent) && talent.Points > 0)
                {
                    SayTo(from, "The souls serve me well. You may now buy from my forbidden stock.");
                    base.VendorBuy(from);
                }
                else
                {
                    SayTo(from, "You have not yet slain 500 Lich Lords. Return when their screams are no more.");
                }
            }
        }

        public override Type[] Quests => new Type[] { typeof(MalgrimQuest) };

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

    public class SBMalgrim : SBInfo
    {
        private List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public override List<GenericBuyInfo> BuyInfo => m_BuyInfo;
        public override IShopSellInfo SellInfo => m_SellInfo;

        public class InternalBuyInfo : List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                // Each item costs 5000 gold, 10 stock
                Add(new AnimalBuyInfo(1, typeof(LichLord), 5000, 10, 0x25C, 0));
                Add(new GenericBuyInfo("Rare Wire", typeof(RareWire), 5000, 10, 0x1EB6, 0));
                Add(new GenericBuyInfo("Forbidden Tome", typeof(ForbiddenTome), 5000, 10, 0x1C10, 0));
                Add(new GenericBuyInfo("Compound F", typeof(CompoundF), 5000, 10, 0x0F0E, 0));
                Add(new GenericBuyInfo("Monster Bones", typeof(MonsterBones), 5000, 10, 0x1B72, 0));
                Add(new GenericBuyInfo("Magic Orb", typeof(MagicOrb), 5000, 10, 0x0E73, 0));
                Add(new GenericBuyInfo("Rib Eye", typeof(RibEye), 5000, 10, 0x1608, 0));
                Add(new GenericBuyInfo("Butter Churn", typeof(ButterChurn), 5000, 10, 0x0FAF, 0));
                Add(new GenericBuyInfo("Stained Window", typeof(StainedWindow), 5000, 10, 0x12B0, 0));
                Add(new GenericBuyInfo("Mutant Starfish", typeof(MutantStarfish), 5000, 10, 0x0DF7, 0));
            }
        }

        public class InternalSellInfo : GenericSellInfo
        {
            public InternalSellInfo() { }
        }
    }
}
