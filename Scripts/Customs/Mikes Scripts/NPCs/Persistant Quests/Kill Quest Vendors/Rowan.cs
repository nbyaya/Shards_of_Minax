using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class RowanQuest : BaseQuest
    {
        public override bool DoneOnce => true;

        public override object Title => "The Hunt of 500 GreatHarts";

        public override object Description => 
            "Well met, wanderer. I am Rowan, keeper of beasts and steward of balance.\n\n" +
            "The GreatHarts have grown too many and too bold. Their unchecked numbers threaten both field and forest.\n\n" +
            "Cull 500 of these majestic—but overabundant—creatures, and I shall reward you with rare curios and a chance to tame your own GreatHart companion.";

        public override object Refuse => "Then I fear nature will continue to spiral out of balance. Come back when you are ready.";

        public override object Uncomplete => "You’ve not yet culled enough of the GreatHarts. Nature still waits.";

        public override object Complete => 
            "Your efforts bring harmony to the wilds once more.\n\n" +
            "As promised, I now offer you a selection of rare goods—tools, trophies, and the opportunity to tame a GreatHart of your own.\n\n" +
            "Spend your coin wisely, and walk the wild path with pride.";

        public RowanQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(GreatHart), "GreatHarts", 500));
        }

        public override void OnCompleted()
        {
            base.OnCompleted();

            var profile = Owner.AcquireTalents();
            if (!profile.Talents.ContainsKey(TalentID.GreatHartSlayerQuest))
                profile.Talents[TalentID.GreatHartSlayerQuest] = new Talent(TalentID.GreatHartSlayerQuest);
            
            profile.Talents[TalentID.GreatHartSlayerQuest].Points = 1;

            Owner.SendMessage(0x23, "You have slain 500 GreatHarts for Rowan!");
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
            reader.ReadInt();
        }
    }

    public class Rowan : MondainQuester
    {
        public override bool IsActiveVendor => true;
        private List<SBInfo> m_SBInfos = new List<SBInfo>();

        protected override List<SBInfo> SBInfos => m_SBInfos;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBRowan());
        }

        [Constructable]
        public Rowan() : base("Rowan", "Beastkeeper of the Wild")
        {
        }

        public Rowan(Serial serial) : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 25);

            Female = true;
            Race = Race.Human;

            Hue = Utility.RandomSkinHue();
            HairItemID = Utility.RandomList(0x203B, 0x2049, 0x2048);
            HairHue = Utility.RandomHairHue();
        }

        public override void InitOutfit()
        {
            AddItem(new Backpack());
            AddItem(new LeatherGloves());
            AddItem(new FemaleLeatherChest());
            AddItem(new LeatherSkirt());
            AddItem(new Sandals());
            AddItem(new Cloak(Utility.RandomNeutralHue()));
        }

        public override void VendorBuy(Mobile from)
        {
            if (from is PlayerMobile player)
            {
                var profile = player.AcquireTalents();
                if (profile.Talents.TryGetValue(TalentID.GreatHartSlayerQuest, out Talent talent) && talent.Points > 0)
                {
                    SayTo(from, "You walk the path of the hunter. Choose your reward.");
                    base.VendorBuy(from);
                }
                else
                {
                    SayTo(from, "Only those who’ve culled 500 GreatHarts may browse my collection.");
                }
            }
        }

        public override Type[] Quests => new Type[]
        {
            typeof(RowanQuest)
        };

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

    public class SBRowan : SBInfo
    {
        private List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public override List<GenericBuyInfo> BuyInfo => m_BuyInfo;
        public override IShopSellInfo SellInfo => m_SellInfo;

        public class InternalBuyInfo : List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                Add(new AnimalBuyInfo(1, typeof(GreatHart), 5000, 10, 0x2011, 0));

                Add(new GenericBuyInfo(typeof(LayingChicken), 5000, 10, 0x9B5, 0));
                Add(new GenericBuyInfo(typeof(EssenceOfToad), 5000, 10, 0xF7A, 0));
                Add(new GenericBuyInfo(typeof(SalvageMachine), 5000, 10, 0x1EB9, 0));
                Add(new GenericBuyInfo(typeof(TwentyfiveShield), 5000, 10, 0x1B74, 0));
                Add(new GenericBuyInfo(typeof(DaggerSign), 5000, 10, 0xF52, 0));
                Add(new GenericBuyInfo(typeof(MultiPump), 5000, 10, 0x1F0B, 0));
                Add(new GenericBuyInfo(typeof(HeartPillow), 5000, 10, 0x230A, 0));
                Add(new GenericBuyInfo(typeof(HydroxFluid), 5000, 10, 0x1F95, 0));
                Add(new GenericBuyInfo(typeof(MasterCello), 5000, 10, 0xEB3, 0));
                Add(new GenericBuyInfo(typeof(DressForm), 5000, 10, 0x1F03, 0));
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
