using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a pyroclastic golem boss corpse")]
    public class PyroclasticGolemBoss : PyroclasticGolem
    {
        [Constructable]
        public PyroclasticGolemBoss() : base()
        {
            Name = "Pyroclastic Golem";
            Title = "the Volcanic Titan";

            // Upgrade stats to match or exceed Barracoon-like levels
            SetStr(1200, 1500); // Increased Strength
            SetDex(255, 300); // Increased Dexterity
            SetInt(250, 350); // Increased Intelligence

            SetHits(12000); // High health to match a boss tier
            SetDamage(40, 55); // Increased damage range

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 35);
            SetDamageType(ResistanceType.Energy, 15);

            SetResistance(ResistanceType.Physical, 75, 85); // Enhanced resistances
            SetResistance(ResistanceType.Fire, 85, 90);
            SetResistance(ResistanceType.Cold, 70, 80);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 60, 70);

            SetSkill(SkillName.Anatomy, 100.0, 125.0); // Enhanced skill ranges
            SetSkill(SkillName.EvalInt, 110.0, 130.0);
            SetSkill(SkillName.Magery, 110.0, 130.0);
            SetSkill(SkillName.Meditation, 50.0, 70.0);
            SetSkill(SkillName.MagicResist, 150.0, 175.0); // Increased magic resist
            SetSkill(SkillName.Tactics, 110.0, 130.0);
            SetSkill(SkillName.Wrestling, 100.0, 120.0);

            Fame = 30000; // Increased Fame for a boss
            Karma = -30000; // Negative Karma for a boss

            VirtualArmor = 110; // Increased virtual armor for a tanky boss

            Tamable = false; // Boss cannot be tamed
            ControlSlots = 0; // Cannot be controlled

            // Attach a random ability
            XmlAttach.AttachTo(this, new XmlRandomAbility());
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot();
            
            // Drop 5 MaxxiaScrolls in addition to regular loot
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }
        }

        public override void OnThink()
        {
            base.OnThink();
            // Additional boss logic can be added here, such as special abilities or enhanced behavior
        }

        public PyroclasticGolemBoss(Serial serial) : base(serial)
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
