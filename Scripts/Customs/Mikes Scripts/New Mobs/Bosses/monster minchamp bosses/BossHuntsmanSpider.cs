using System;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a huntsman spider corpse")]
    public class BossHuntsmanSpider : HuntsmanSpider
    {
        private DateTime m_NextRapidStrike;
        private DateTime m_NextQuickReflexes;
        private DateTime m_NextToxicVenom;
        private DateTime m_NextWebTrap;

        private bool m_AbilitiesInitialized;

        [Constructable]
        public BossHuntsmanSpider() : base()
        {
            Name = "a huntsman spider";
            Title = "the Arachnid King";
            Hue = 1797; // Unique hue for the boss Huntsman Spider
            Body = 28;
            BaseSoundID = 0x388;

            SetStr(1500, 2000); // Increased strength
            SetDex(250, 350); // Increased dexterity
            SetInt(250, 350); // Increased intelligence

            SetHits(15000, 20000); // Increased health

            SetDamage(50, 75); // Increased damage

            SetDamageType(ResistanceType.Physical, 60);
            SetDamageType(ResistanceType.Fire, 20);
            SetDamageType(ResistanceType.Energy, 20);

            SetResistance(ResistanceType.Physical, 80, 90);
            SetResistance(ResistanceType.Fire, 75, 85);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 60, 70);

            SetSkill(SkillName.Anatomy, 100.0, 120.0);
            SetSkill(SkillName.EvalInt, 100.0, 120.0);
            SetSkill(SkillName.Magery, 100.0, 120.0);
            SetSkill(SkillName.Meditation, 50.0, 80.0);
            SetSkill(SkillName.MagicResist, 150.0, 180.0); // Higher magic resist
            SetSkill(SkillName.Tactics, 100.0, 120.0);
            SetSkill(SkillName.Wrestling, 100.0, 120.0);

            Fame = 35000; // Higher fame for a boss
            Karma = -35000; // Boss-level karma

            VirtualArmor = 150; // Higher virtual armor for the boss

            Tamable = false; // Cannot be tamed
            ControlSlots = 3; // Control slots increased

            m_AbilitiesInitialized = false; // Flag to initialize abilities
            XmlAttach.AttachTo(this, new XmlRandomAbility()); PackItem(new BossTreasureBox());
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot(); // Base loot
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll()); // Add 5 MaxxiaScrolls as required
            }
            this.AddLoot(LootPack.FilthyRich, 2); // Rich loot for the boss
            this.AddLoot(LootPack.Rich); // Additional rich loot
            this.AddLoot(LootPack.Gems, 10); // More gems
        }

        public override void OnThink()
        {
            base.OnThink(); // Retain base behavior for special abilities
        }

        public BossHuntsmanSpider(Serial serial) : base(serial)
        {
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
            m_AbilitiesInitialized = false; // Reset initialization flag
        }
    }

    // The WebTrap class remains unchanged
}
