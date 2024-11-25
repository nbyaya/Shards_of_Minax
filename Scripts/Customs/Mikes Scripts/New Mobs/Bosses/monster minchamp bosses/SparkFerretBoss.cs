using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a spark ferret boss corpse")]
    public class SparkFerretBoss : SparkFerret
    {
        [Constructable]
        public SparkFerretBoss() : base()
        {
            Name = "Spark Ferret Overlord";
            Title = "the Supreme Surge";

            // Update stats to make it a boss-tier creature
            SetStr(1200); // Higher strength than original
            SetDex(255); // Max dexterity for a quick and agile boss
            SetInt(250); // High intelligence for potent abilities

            SetHits(12000); // Boss-level health
            SetDamage(35, 45); // Increased damage range to match a tougher boss

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 75, 85); // Increased resistances
            SetResistance(ResistanceType.Fire, 70, 85);
            SetResistance(ResistanceType.Cold, 60, 75);
            SetResistance(ResistanceType.Poison, 75, 85);
            SetResistance(ResistanceType.Energy, 50, 65);

            SetSkill(SkillName.Anatomy, 50.0, 100.0); // Stronger skill in Anatomy for better defense
            SetSkill(SkillName.EvalInt, 110.0, 150.0); // Boosted magical abilities
            SetSkill(SkillName.Magery, 120.0, 150.0); // Stronger Magery skill for enhanced attacks
            SetSkill(SkillName.Meditation, 50.0, 100.0); // Better meditation for enhanced mana regeneration
            SetSkill(SkillName.MagicResist, 150.0, 200.0); // Higher magic resistance
            SetSkill(SkillName.Tactics, 120.0, 140.0); // Improved tactics for more strategic fighting
            SetSkill(SkillName.Wrestling, 100.0, 150.0); // Improved physical combat skills

            Fame = 24000; // Fame increased for boss-tier creature
            Karma = -24000;

            VirtualArmor = 100; // Increased virtual armor for better defense

            // Attach the random ability from XmlSpawner
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

            // You can add additional boss logic or abilities here if needed
        }

        public SparkFerretBoss(Serial serial) : base(serial)
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
