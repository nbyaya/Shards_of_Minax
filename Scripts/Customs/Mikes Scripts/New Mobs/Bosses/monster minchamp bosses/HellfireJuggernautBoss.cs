using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a hellfire juggernaut corpse")]
    public class HellfireJuggernautBoss : HellfireJuggernaut
    {
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public HellfireJuggernautBoss() : base()
        {
            Name = "Hellfire Juggernaut";
            Title = "the Infernal Overlord";
            Hue = 1468; // Unique hue

            // Update stats to match or exceed Barracoon's
            SetStr(1200); // Upper strength
            SetDex(255);  // Upper dexterity
            SetInt(250);  // Upper intelligence

            SetHits(12000); // High health for boss-tier difficulty

            SetDamage(35, 50); // Increased damage

            SetDamageType(ResistanceType.Physical, 60);
            SetDamageType(ResistanceType.Fire, 30);
            SetDamageType(ResistanceType.Energy, 10);

            SetResistance(ResistanceType.Physical, 70, 85);
            SetResistance(ResistanceType.Fire, 80, 100);
            SetResistance(ResistanceType.Cold, 60, 80);
            SetResistance(ResistanceType.Poison, 70, 85);
            SetResistance(ResistanceType.Energy, 50, 70);

            SetSkill(SkillName.MagicResist, 120.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);

            Fame = 30000; // Increased fame
            Karma = -30000; // Increased karma penalty

            VirtualArmor = 100; // Boss-tier virtual armor

            Tamable = false; // Boss creatures are not tamable
            ControlSlots = 0; // No control slots

            m_AbilitiesInitialized = false; // Initialize flag

            // Attach a random ability (via XML)
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

            // Additional loot if required
            this.AddLoot(LootPack.FilthyRich, 2);
            this.AddLoot(LootPack.Rich);
            this.AddLoot(LootPack.Gems, 8);
        }

        public override void OnThink()
        {
            base.OnThink();
            // Additional boss logic can be added here if needed
        }

        public HellfireJuggernautBoss(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            var version = reader.ReadInt();

            m_AbilitiesInitialized = false; // Reset flag on deserialize
        }
    }
}
