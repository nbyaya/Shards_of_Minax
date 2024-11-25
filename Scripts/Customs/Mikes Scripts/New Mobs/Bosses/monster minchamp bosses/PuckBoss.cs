using System;
using Server.Items;
using Server.Network;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a Puck corpse")]
    public class PuckBoss : Puck
    {
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public PuckBoss()
            : base()
        {
            Name = "Puck";
            Title = "the Mischief Maker"; // Adjusted title for the boss tier version

            // Enhanced stats for the boss version
            SetStr(1200, 1500); // Higher strength
            SetDex(255, 300); // Higher dexterity
            SetInt(250, 350); // Higher intelligence

            SetHits(12000); // Increased health to boss levels

            SetDamage(35, 50); // Increased damage range

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 75, 90); // Increased resistances
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 60, 75);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.Anatomy, 50.0, 75.0);
            SetSkill(SkillName.EvalInt, 100.0, 120.0);
            SetSkill(SkillName.Magery, 110.0, 120.0);
            SetSkill(SkillName.Meditation, 50.0, 75.0);
            SetSkill(SkillName.MagicResist, 150.0, 200.0); // Increased Magic Resist
            SetSkill(SkillName.Tactics, 100.0, 120.0);
            SetSkill(SkillName.Wrestling, 100.0, 120.0);

            Fame = 24000;
            Karma = -24000;

            VirtualArmor = 100; // Increased virtual armor for higher defense

            Tamable = false; // Boss version is not tamable
            ControlSlots = 0; // Boss doesn't need control slots

            m_AbilitiesInitialized = false; // Initialize flag
            XmlAttach.AttachTo(this, new XmlRandomAbility()); PackItem(new BossTreasureBox());
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot();

            PackItem(new BossTreasureBox());
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }

            this.AddLoot(LootPack.FilthyRich, 2); // Richer loot pack for a boss
            this.AddLoot(LootPack.Rich);
            this.AddLoot(LootPack.Gems, 10); // Increased gems drop
        }

        public override void OnThink()
        {
            base.OnThink();
            // Additional boss logic could be added here, if necessary
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

            m_AbilitiesInitialized = false; // Reset flag on deserialize
        }
    }
}
