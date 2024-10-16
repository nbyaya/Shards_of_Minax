using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class DrakeBeastbaneQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Drake Beastbane's Hunt"; } }

        public override object Description
        {
            get
            {
                return 
                    "Greetings, adventurer! I am Drake Beastbane, the most renowned beast hunter in these lands. " +
                    "To prove your skill and earn the privilege to browse my exclusive collection of beasts, " +
                    "you must hunt down the following formidable creatures:\n\n" +
                    "1. Arcane Sentinel\n" +
                    "2. Flameborne Knight\n" +
                    "3. Frostbound Champion\n" +
                    "4. Grave Knight\n" +
                    "5. Ironclad Defender\n" +
                    "6. Necrotic General\n" +
                    "7. Shadowblade Assassin\n" +
                    "8. Spectral Warden\n" +
                    "9. Storm Bone\n" +
                    "10. Vampiric Blade\n\n" +
                    "Bring proof of your kills to me, and my shop will open its doors to you!";
            }
        }

        public override object Refuse { get { return "Very well. Return when you are ready to face these beasts."; } }

        public override object Uncomplete { get { return "You haven't completed all the tasks yet. Keep hunting!"; } }

        public override object Complete { get { return "Well done! You’ve proven yourself worthy. My shop is now open for you."; } }

        public DrakeBeastbaneQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(ArcaneSentinel), "Arcane Sentinels", 1));
            AddObjective(new SlayObjective(typeof(FlameborneKnight), "Flameborne Knights", 1));
            AddObjective(new SlayObjective(typeof(FrostboundChampion), "Frostbound Champions", 1));
            AddObjective(new SlayObjective(typeof(GraveKnight), "Grave Knights", 1));
            AddObjective(new SlayObjective(typeof(IroncladDefender), "Ironclad Defenders", 1));
            AddObjective(new SlayObjective(typeof(NecroticGeneral), "Necrotic Generals", 1));
            AddObjective(new SlayObjective(typeof(ShadowbladeAssassin), "Shadowblade Assassins", 1));
            AddObjective(new SlayObjective(typeof(SpectralWarden), "Spectral Wardens", 1));
            AddObjective(new SlayObjective(typeof(StormBone), "Storm Bones", 1));
            AddObjective(new SlayObjective(typeof(VampiricBlade), "Vampiric Blades", 1));

            AddReward(new BaseReward(typeof(Gold), 1000, "1000 Gold"));
            AddReward(new BaseReward(typeof(BeastBaneToken), 1, "Beast Token"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You have completed Drake Beastbane's challenge!");
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
            int version = reader.ReadInt();
        }
    }

    public class DrakeBeastbane : MondainQuester
    {
        public override bool IsActiveVendor { get { return true; } }

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBDrakeBeastbane());
        }

        [Constructable]
        public DrakeBeastbane()
            : base("Drake Beastbane", "The Renowned Beast Hunter")
        {
        }

        public DrakeBeastbane(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 25);

            Female = false;
            Race = Race.Human;

            Hue = Utility.RandomSkinHue();
            HairItemID = Utility.RandomList(0x203B, 0x2049, 0x2048);
            HairHue = Utility.RandomHairHue();
            FacialHairItemID = Utility.RandomList(0x203E, 0x203F, 0x2040, 0x2041, 0x204B, 0x204C, 0x204D);
            FacialHairHue = HairHue;
        }

        public override void InitOutfit()
        {
            AddItem(new Backpack());
            AddItem(new Boots(Utility.RandomNeutralHue()));
            AddItem(new LongPants(Utility.RandomNeutralHue()));
            AddItem(new FancyShirt(Utility.RandomNeutralHue()));
            AddItem(new Cloak(Utility.RandomNeutralHue()));
        }

        public override void VendorBuy(Mobile from)
        {
            PlayerMobile player = from as PlayerMobile;
            if (player != null)
            {
                Item token = player.Backpack.FindItemByType(typeof(BeastBaneToken));
				Item masterToken = player.Backpack.FindItemByType(typeof(MasterToken));

                if (token != null || masterToken != null)
                {
                    SayTo(from, "Welcome to my beast shop! Feel free to browse.");
                    base.VendorBuy(from);
                }
                else
                {
                    SayTo(from, "You must have a Beast Token to access my shop.");
                }
            }
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(DrakeBeastbaneQuest)
                };
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class SBDrakeBeastbane : SBInfo
    {
        private System.Collections.Generic.List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public SBDrakeBeastbane()
        {
        }

        public override System.Collections.Generic.List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }
        public override IShopSellInfo SellInfo { get { return m_SellInfo; } }

        public class InternalBuyInfo : System.Collections.Generic.List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                // Add the beast-themed creatures here
                Add(new AnimalBuyInfo(1, typeof(ArcaneSentinel), 1000, 10, 147, 0));
                Add(new AnimalBuyInfo(1, typeof(FlameborneKnight), 1200, 10, 147, 0));
                Add(new AnimalBuyInfo(1, typeof(FrostboundChampion), 1100, 10, 147, 0));
                Add(new AnimalBuyInfo(1, typeof(GraveKnight), 1300, 10, 147, 0));
                Add(new AnimalBuyInfo(1, typeof(IroncladDefender), 1400, 10, 147, 0));
                Add(new AnimalBuyInfo(1, typeof(NecroticGeneral), 1500, 10, 147, 0));
                Add(new AnimalBuyInfo(1, typeof(ShadowbladeAssassin), 1250, 10, 147, 0));
                Add(new AnimalBuyInfo(1, typeof(SpectralWarden), 1350, 10, 147, 0));
                Add(new AnimalBuyInfo(1, typeof(StormBone), 1200, 10, 147, 0));
                Add(new AnimalBuyInfo(1, typeof(VampiricBlade), 1600, 10, 147, 0));
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
