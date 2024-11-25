using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a steel bear overlord corpse")]
    public class SteelBearBoss : SteelBear
    {
        [Constructable]
        public SteelBearBoss()
            : base()
        {
            Name = "Steel Bear Overlord";
            Title = "the Master of Metal";

            // Update stats to match or exceed Barracoon
            SetStr(1200); // Increased strength for the boss
            SetDex(255);  // Maximum dexterity for the boss
            SetInt(250);  // Increased intelligence for the boss

            SetHits(12000); // Increased health for the boss
            SetDamage(40, 50); // Increased damage for the boss

            // Resistances enhanced to make the boss more difficult
            SetResistance(ResistanceType.Physical, 80, 100);
            SetResistance(ResistanceType.Fire, 80, 100);
            SetResistance(ResistanceType.Cold, 60, 80);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 60, 80);

            // Enhanced skills for the boss
            SetSkill(SkillName.Anatomy, 75.0, 100.0);
            SetSkill(SkillName.EvalInt, 100.0, 120.0);
            SetSkill(SkillName.Magery, 100.0, 120.0);
            SetSkill(SkillName.Meditation, 50.0, 75.0);
            SetSkill(SkillName.MagicResist, 150.0, 175.0);
            SetSkill(SkillName.Tactics, 100.0, 120.0);
            SetSkill(SkillName.Wrestling, 100.0, 120.0);

            Fame = 32000;
            Karma = -32000;

            VirtualArmor = 100;

            Tamable = false; // Boss can't be tamed
            ControlSlots = 3;

            // Attach the XmlRandomAbility for additional effects
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
            // Additional boss logic could be added here
        }

        public SteelBearBoss(Serial serial)
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
