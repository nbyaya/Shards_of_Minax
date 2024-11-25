using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a Storm Overlord Crab corpse")]
    public class StormCrabBoss : StormCrab
    {
        private DateTime m_NextCycloneGrasp;
        private DateTime m_NextGaleForceStrike;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public StormCrabBoss() : base("Storm Overlord Crab")
        {
            // Update stats to match or exceed Barracoon-style values
            SetStr(1200, 1500); // Much higher strength than original
            SetDex(255, 300); // Higher dexterity
            SetInt(250, 350); // Higher intelligence

            SetHits(15000); // Much higher health than original
            SetDamage(40, 50); // Higher damage range

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 30);
            SetDamageType(ResistanceType.Energy, 20);

            SetResistance(ResistanceType.Physical, 80, 90);
            SetResistance(ResistanceType.Fire, 70, 80);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.Anatomy, 50.0, 70.0);
            SetSkill(SkillName.EvalInt, 100.0, 120.0);
            SetSkill(SkillName.Magery, 120.0, 140.0);
            SetSkill(SkillName.Meditation, 50.0, 75.0);
            SetSkill(SkillName.MagicResist, 150.0, 180.0);
            SetSkill(SkillName.Tactics, 100.0, 120.0);
            SetSkill(SkillName.Wrestling, 100.0, 120.0);

            Fame = 24000;
            Karma = -24000;

            VirtualArmor = 100;

            Tamable = false;
            ControlSlots = 0;

            m_AbilitiesInitialized = false; // Initialize flag
            // Attach a random ability
            XmlAttach.AttachTo(this, new XmlRandomAbility());
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot();
            
            PackItem(new BossTreasureBox());
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }

            // Richer loot pool, increase treasure drop chances
            this.AddLoot(LootPack.FilthyRich, 2);
            this.AddLoot(LootPack.Rich);
            this.AddLoot(LootPack.Gems, 8);
        }

        public override void OnThink()
        {
            base.OnThink();
            // Additional boss logic can be added here if needed
        }

        public StormCrabBoss(Serial serial) : base(serial)
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

            m_AbilitiesInitialized = false; // Reset flag on deserialize
            m_NextCycloneGrasp = DateTime.UtcNow; // Reset timers to ensure correct behavior
            m_NextGaleForceStrike = DateTime.UtcNow;
        }
    }
}
