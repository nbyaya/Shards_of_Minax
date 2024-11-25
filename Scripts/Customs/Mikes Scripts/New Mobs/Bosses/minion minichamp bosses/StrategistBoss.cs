using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the master strategist")]
    public class StrategistBoss : Strategist
    {
        [Constructable]
        public StrategistBoss() : base()
        {
            Name = "Master Strategist";
            Title = "the Unstoppable Tactician";

            // Update stats to match or exceed Barracoon or other boss standards
            SetStr(800); // Enhanced strength
            SetDex(200); // Enhanced dexterity
            SetInt(700); // Enhanced intelligence

            SetHits(12000); // Boss-level health
            SetDamage(20, 30); // Higher damage range

            SetDamageType(ResistanceType.Physical, 70); // More physical damage
            SetDamageType(ResistanceType.Cold, 15); // Reduced cold damage
            SetDamageType(ResistanceType.Poison, 15); // Reduced poison damage

            SetResistance(ResistanceType.Physical, 70, 80); // Higher physical resistance
            SetResistance(ResistanceType.Fire, 60, 70); // Better fire resistance
            SetResistance(ResistanceType.Cold, 80, 90); // Better cold resistance
            SetResistance(ResistanceType.Poison, 70, 80); // Better poison resistance
            SetResistance(ResistanceType.Energy, 60, 70); // Better energy resistance

            SetSkill(SkillName.Anatomy, 75.0, 100.0); // Higher skill in anatomy
            SetSkill(SkillName.EvalInt, 110.0, 120.0); // Stronger evalint
            SetSkill(SkillName.Magery, 110.0, 120.0); // Stronger magery
            SetSkill(SkillName.Meditation, 75.0, 100.0); // Stronger meditation
            SetSkill(SkillName.MagicResist, 150.0, 175.0); // Better magic resistance
            SetSkill(SkillName.Tactics, 120.0, 130.0); // Stronger tactics
            SetSkill(SkillName.Wrestling, 100.0, 120.0); // Higher wrestling skill

            Fame = 25000; // Higher fame for a boss
            Karma = -25000; // Negative karma for the boss

            VirtualArmor = 80; // Higher virtual armor

            // Attach the XmlRandomAbility
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

        public StrategistBoss(Serial serial) : base(serial)
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
