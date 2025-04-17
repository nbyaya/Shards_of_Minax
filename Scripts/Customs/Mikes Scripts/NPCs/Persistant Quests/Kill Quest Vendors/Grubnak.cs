using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;

namespace Server.Engines.Quests
{
    // ==================== QUEST DEFINITION ====================
    public class GrubnakQuest : BaseQuest
    {
        public override bool DoneOnce => true;

        public override object Title => "The Cleansing of the Swamp";

        public override object Description => "These Giant Toads are out of control...\n\nFor decades, the Swamps have been my home. But the balance is broken. The Giant Toads multiply unchecked, feasting on eggs, snakes, and even the weak of our kin. I am Grubnak, the Swampcaller.\n\nBring balance back. Slay 500 of the bloated beasts and prove yourself a true guardian of the wild. Then, and only then, shall you earn my trust—and my wares.";

        public override object Refuse => "Then begone. The swamp does not tolerate hesitation.";

        public override object Uncomplete => "You have not yet culled enough of the toads. The swamp still suffers.";

        public override object Complete => "You have done it... I can feel the waters calm. The swamp thanks you. Now, you may trade with me. Choose wisely—the spirits still watch.";

        public GrubnakQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(GiantToad), "Giant Toads", 500));
        }

        public override void OnCompleted()
        {
            base.OnCompleted();

            var profile = Owner.AcquireTalents();
            if (!profile.Talents.ContainsKey(TalentID.ToadSlayerQuest))
                profile.Talents[TalentID.ToadSlayerQuest] = new Talent(TalentID.ToadSlayerQuest);

            profile.Talents[TalentID.ToadSlayerQuest].Points = 1;

            Owner.SendMessage(0x23, "You have slain 500 Giant Toads for Grubnak!");
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

    // ==================== NPC VENDOR ====================
    public class Grubnak : MondainQuester
    {
        private List<SBInfo> m_SBInfos = new List<SBInfo>();

        protected override List<SBInfo> SBInfos => m_SBInfos;

        public override bool IsActiveVendor => true;

        public override Type[] Quests => new Type[] { typeof(GrubnakQuest) };

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBGrubnak());
        }

        [Constructable]
        public Grubnak() : base("Grubnak", "Swampcaller")
        {
        }

        public Grubnak(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(150, 100, 100);
            Hue = 0;
            Female = false;
            Race = Race.Human;
            HairItemID = 0x2049;
            HairHue = 0;
        }

        public override void InitOutfit()
        {
            AddItem(new HoodedShroudOfShadows());
            AddItem(new Sandals());
        }

        public override void VendorBuy(Mobile from)
        {
            if (from is PlayerMobile player)
            {
                var profile = player.AcquireTalents();
                if (profile.Talents.TryGetValue(TalentID.ToadSlayerQuest, out var talent) && talent.Points > 0)
                {
                    SayTo(player, "You have earned the swamp's favor. Choose what you will.");
                    base.VendorBuy(from);
                }
                else
                {
                    SayTo(player, "You have not yet proven your worth. Return after you have slain 500 Giant Toads.");
                }
            }
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

    // ==================== SHOP DEFINITION ====================
    public class SBGrubnak : SBInfo
    {
        private List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public override List<GenericBuyInfo> BuyInfo => m_BuyInfo;
        public override IShopSellInfo SellInfo => m_SellInfo;

        public SBGrubnak() { }

        public class InternalBuyInfo : List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                Add(new AnimalBuyInfo(1, typeof(GiantToad), 10000, 10, 0xD7, 0)); // Adjust cost, stock as needed

                Add(new GenericBuyInfo(typeof(CivilWorChest), 5000, 20, 0xE43, 0));
                Add(new GenericBuyInfo(typeof(ConfederationCache), 5000, 20, 0xE43, 0));
                Add(new GenericBuyInfo(typeof(ConquistadorsHoard), 5000, 20, 0xE43, 0));
                Add(new GenericBuyInfo(typeof(CovenTreasuresChest), 5000, 20, 0xE43, 0));
                Add(new GenericBuyInfo(typeof(CyberneticCache), 5000, 20, 0xE43, 0));
                Add(new GenericBuyInfo(typeof(DesertPharaohChest), 5000, 20, 0xE43, 0));
                Add(new GenericBuyInfo(typeof(DinerDelightChest), 5000, 20, 0xE43, 0));
                Add(new GenericBuyInfo(typeof(DojoLegacyChest), 5000, 20, 0xE43, 0));
                Add(new GenericBuyInfo(typeof(DragonGuardiansHoardChest), 5000, 20, 0xE43, 0));
                Add(new GenericBuyInfo(typeof(DragonHoardChest), 5000, 20, 0xE43, 0));
            }
        }

        public class InternalSellInfo : GenericSellInfo
        {
            public InternalSellInfo() { }
        }
    }
}
