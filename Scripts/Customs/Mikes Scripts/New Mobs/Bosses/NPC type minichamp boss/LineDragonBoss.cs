using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the line dragon overlord")]
    public class LineDragonBoss : LineDragon
    {
        [Constructable]
        public LineDragonBoss() : base()
        {
            Name = "Line Dragon Overlord";
            Title = "the Supreme Dragon";

            // Update stats to match or exceed the original LineDragon
            SetStr(1200); // Exceeding original strength
            SetDex(175);  // Maximizing dexterity
            SetInt(800);  // Maximizing intelligence

            SetHits(12000); // Boss-level health
            SetDamage(40, 55); // Increased damage for the boss

            SetResistance(ResistanceType.Physical, 75);
            SetResistance(ResistanceType.Fire, 80);
            SetResistance(ResistanceType.Cold, 75);
            SetResistance(ResistanceType.Poison, 80);
            SetResistance(ResistanceType.Energy, 80);

            SetSkill(SkillName.MagicResist, 100.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);

            Fame = 22500; // High fame for a boss
            Karma = -22500; // Evil boss karma

            VirtualArmor = 80; // Enhanced armor for tanking

            // Attach the XmlRandomAbility for random abilities
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

        public LineDragonBoss(Serial serial) : base(serial)
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
