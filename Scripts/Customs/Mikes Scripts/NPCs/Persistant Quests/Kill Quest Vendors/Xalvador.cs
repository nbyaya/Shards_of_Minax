using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    // ==========================
    // Quest Definition
    // ==========================
    public class XalvadorQuest : BaseQuest
    {
        public override bool DoneOnce => true;

        public override object Title => "Cleansing the Skies";

        public override object Description => 
            "You there! Yes, you! I've watched the skies darken with the flapping wings of Gargoyles. " +
            "Twisted abominations of magic and stone—every last one a blight on this realm.\n\n" +
            "Slay *five hundred* Gargoyles and prove you're not just another bootlicker in polished armor.\n\n" +
            "Do that, and I will let you see the true fruits of my craft.";

        public override object Refuse => 
            "Hah! Run back to your mother, then. The skies will not clear themselves.";

        public override object Uncomplete => 
            "You're still breathing, and yet the Gargoyles still fly. Return when their corpses litter the ground.";

        public override object Complete =>
            "The wind smells cleaner... less wings in the sky. You've done it, haven't you?\n\n" +
            "Well then. You've earned this. My shop is yours to browse, but no haggling. Gold only.";

        public XalvadorQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(Gargoyle), "Gargoyles", 500));
        }

        public override void OnCompleted()
        {
            base.OnCompleted();

            var profile = Owner.AcquireTalents();
            if (!profile.Talents.ContainsKey(TalentID.GargoyleSlayerQuest))
                profile.Talents[TalentID.GargoyleSlayerQuest] = new Talent(TalentID.GargoyleSlayerQuest);

            profile.Talents[TalentID.GargoyleSlayerQuest].Points = 1;

            Owner.SendMessage(0x23, "You have slain 500 Gargoyles for Xalvador!");
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

    // ==========================
    // NPC Definition
    // ==========================
    public class Xalvador : MondainQuester
    {
        public override bool IsActiveVendor => true;
        private List<SBInfo> m_SBInfos = new List<SBInfo>();

        protected override List<SBInfo> SBInfos => m_SBInfos;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBXalvador());
        }

        [Constructable]
        public Xalvador() : base("Xalvador", "Beastmaster of the Skies")
        {
        }

        public Xalvador(Serial serial) : base(serial)
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
            FacialHairItemID = 0x204B;
            FacialHairHue = HairHue;
        }

        public override void InitOutfit()
        {
            AddItem(new Backpack());
            AddItem(new Sandals(0x455));
            AddItem(new Robe(0x455));
            AddItem(new BoneHelm());
        }

        public override void VendorBuy(Mobile from)
        {
            PlayerMobile player = from as PlayerMobile;
            if (player != null)
            {
                var profile = player.AcquireTalents();
                if (profile.Talents.TryGetValue(TalentID.GargoyleSlayerQuest, out Talent t) && t.Points > 0)
                {
                    SayTo(from, "You've earned the right. Spend wisely.");
                    base.VendorBuy(from);
                }
                else
                {
                    SayTo(from, "Slay 500 Gargoyles, then we can talk business.");
                }
            }
        }

        public override Type[] Quests => new Type[]
        {
            typeof(XalvadorQuest)
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

    // ==========================
    // Shop Inventory
    // ==========================
    public class SBXalvador : SBInfo
    {
        private List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public SBXalvador() { }

        public override List<GenericBuyInfo> BuyInfo => m_BuyInfo;
        public override IShopSellInfo SellInfo => m_SellInfo;

        public class InternalBuyInfo : List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                Add(new AnimalBuyInfo(1, typeof(Gargoyle), 5000, 10, 0x31FE, 0)); // Tamed Gargoyle
                Add(new GenericBuyInfo(typeof(VeterinaryAugmentCrystal), 5000, 10, 0x2F5F, 0));
                Add(new GenericBuyInfo(typeof(WeaponSpeedAugmentCrystal), 5000, 10, 0x2F5F, 0));
                Add(new GenericBuyInfo(typeof(WrestlingAugmentCrystal), 5000, 10, 0x2F5F, 0));
                Add(new GenericBuyInfo(typeof(BootsOfCommand), 5000, 10, 0x1711, 0));
                Add(new GenericBuyInfo(typeof(GlovesOfCommand), 5000, 10, 0x1414, 0));
                Add(new GenericBuyInfo(typeof(GrandmastersRobe), 5000, 10, 0x1F03, 0));
                Add(new GenericBuyInfo(typeof(JesterHatOfCommand), 5000, 10, 0x171C, 0));
                Add(new GenericBuyInfo(typeof(PlateLeggingsOfCommand), 5000, 10, 0x1411, 0));
                Add(new GenericBuyInfo(typeof(AshAxe), 5000, 10, 0x13FB, 0));
                Add(new GenericBuyInfo(typeof(BraceletOfNaturesBounty), 5000, 10, 0x1086, 0));
            }
        }

        public class InternalSellInfo : GenericSellInfo
        {
            public InternalSellInfo() { }
        }
    }
}
