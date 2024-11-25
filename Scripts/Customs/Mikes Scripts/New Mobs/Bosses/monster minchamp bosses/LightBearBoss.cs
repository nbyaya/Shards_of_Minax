using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Spells;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a light bear overlord corpse")]
    public class LightBearBoss : LightBear
    {
        private bool m_AbilitiesInitialized;

        [Constructable]
        public LightBearBoss() : base()
        {
            Name = "Light Bear Overlord";
            Title = "the Radiant Guardian";
            
            // Set enhanced stats to match or exceed Barracoon's values
            SetStr(1200); // High strength value
            SetDex(255);  // Max dexterity
            SetInt(250);  // Max intelligence

            SetHits(12000); // Boss health
            SetDamage(35, 45); // Enhanced damage

            // Enhanced resistances (more robust than the original LightBear)
            SetResistance(ResistanceType.Physical, 75, 90);
            SetResistance(ResistanceType.Fire, 75, 90);
            SetResistance(ResistanceType.Cold, 60, 80);
            SetResistance(ResistanceType.Poison, 120);
            SetResistance(ResistanceType.Energy, 60, 80);

            SetSkill(SkillName.Magery, 120.0, 130.0);
            SetSkill(SkillName.MagicResist, 150.0, 180.0);
            SetSkill(SkillName.Tactics, 110.0, 120.0);
            SetSkill(SkillName.Wrestling, 110.0, 120.0);

            Fame = 24000;
            Karma = -24000;

            VirtualArmor = 100; // Boss-level armor

            Tamable = false;
            ControlSlots = 3;
            MinTameSkill = 100.0; // Higher taming skill required

            m_AbilitiesInitialized = false; // Ensure abilities are initialized
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot();

            // Add 5 MaxxiaScrolls to loot
			PackItem(new BossTreasureBox());
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }
        }

        public override void OnThink()
        {
            base.OnThink();
            // Additional boss logic could be added here
        }

        
        public LightBearBoss(Serial serial) : base(serial)
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
        }
    }
}
