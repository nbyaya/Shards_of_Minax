using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the fire lord")]
    public class FirestarterBoss : Firestarter
    {
        [Constructable]
        public FirestarterBoss() : base()
        {
            Name = "Fire Lord";
            Title = "the Inferno";

            // Update stats to match or exceed Barracoon
            SetStr(900, 1200); // Higher strength than original
            SetDex(255); // Higher dexterity than original
            SetInt(250); // Maximum intelligence for increased magic power

            SetHits(12000); // Increased health
            SetDamage(20, 30); // Increased damage output

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 50); // Fire damage dominance

            // Resistance updates for higher boss difficulty
            SetResistance(ResistanceType.Physical, 75, 85);
            SetResistance(ResistanceType.Fire, 90, 95); // Fire resistance maxed for thematic consistency
            SetResistance(ResistanceType.Cold, 40, 50);
            SetResistance(ResistanceType.Poison, 60, 70);
            SetResistance(ResistanceType.Energy, 60, 70);

            // Increase magical skills to make it more challenging
            SetSkill(SkillName.EvalInt, 120.0);
            SetSkill(SkillName.Magery, 120.0);
            SetSkill(SkillName.Meditation, 50.0);
            SetSkill(SkillName.MagicResist, 150.0); // Stronger resistance
            SetSkill(SkillName.Tactics, 100.0);
            SetSkill(SkillName.Wrestling, 100.0);

            Fame = 22500; // Higher fame for a boss
            Karma = -22500; // Boss with negative karma

            VirtualArmor = 70; // Higher virtual armor for the boss

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

        public FirestarterBoss(Serial serial) : base(serial)
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
