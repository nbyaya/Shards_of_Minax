using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a celestial dragon corpse")]
    public class CelestialDragonBoss : CelestialDragon
    {
        [Constructable]
        public CelestialDragonBoss()
            : base()
        {
            Name = "Celestial Dragon Overlord";
            Title = "the Divine Wyrm";

            // Update stats to make the boss stronger
            SetStr(1200, 1500); // Higher strength for the boss
            SetDex(255, 300);   // Higher dexterity for the boss
            SetInt(250, 350);   // Higher intelligence for the boss

            SetHits(15000);     // Increased health
            SetDamage(50, 60);  // Increased damage range

            // Enhance resistances for the boss
            SetResistance(ResistanceType.Physical, 80, 90);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 70, 80);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 60, 80);

            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);
            SetSkill(SkillName.Magery, 120.0);
            SetSkill(SkillName.Meditation, 100.0);

            Fame = 50000;   // Increased fame
            Karma = -50000; // Increased karma

            VirtualArmor = 120;

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
        }

        public override void OnThink()
        {
            base.OnThink();

            // Boss-specific logic can go here, e.g., abilities or behavior enhancements
        }

        public CelestialDragonBoss(Serial serial)
            : base(serial)
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
