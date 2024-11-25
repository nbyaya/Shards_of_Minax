using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a vengeful pit viper corpse")]
    public class VengefulPitViperBoss : VengefulPitViper
    {
        [Constructable]
        public VengefulPitViperBoss() : base()
        {
            Name = "Vengeful Pit Viper";
            Title = "the Overlord of the Pit";
            Hue = 1770; // Unique hue for the boss version

            // Enhance stats to match or exceed the boss-level stats
            SetStr(1200); // Match the upper strength
            SetDex(255); // Upper dexterity value
            SetInt(250); // Upper intelligence value

            SetHits(12000); // Boss-level health
            SetDamage(45, 55); // Higher damage range for a boss

            SetDamageType(ResistanceType.Physical, 60);
            SetDamageType(ResistanceType.Fire, 20);
            SetDamageType(ResistanceType.Energy, 20);

            SetResistance(ResistanceType.Physical, 80, 90); // Stronger resistances
            SetResistance(ResistanceType.Fire, 75, 85);
            SetResistance(ResistanceType.Cold, 70, 80);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 60, 75);

            SetSkill(SkillName.Anatomy, 50.0, 100.0); // Enhanced skills
            SetSkill(SkillName.EvalInt, 100.0, 120.0);
            SetSkill(SkillName.Magery, 100.0, 120.0);
            SetSkill(SkillName.Meditation, 50.0, 75.0);
            SetSkill(SkillName.MagicResist, 150.0, 175.0);
            SetSkill(SkillName.Tactics, 120.0, 140.0);
            SetSkill(SkillName.Wrestling, 120.0, 140.0);

            Fame = 25000; // High fame for a boss
            Karma = -25000; // Negative karma for the boss

            VirtualArmor = 90; // Stronger armor for the boss

            Tamable = false; // Not tamable
            ControlSlots = 0; // Not a controllable pet

            // Attach a random ability for the boss
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

        public VengefulPitViperBoss(Serial serial) : base(serial)
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
