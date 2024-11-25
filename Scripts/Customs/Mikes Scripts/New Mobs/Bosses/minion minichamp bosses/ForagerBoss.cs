using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the foraging overlord")]
    public class ForagerBoss : Forager
    {
        [Constructable]
        public ForagerBoss() : base()
        {
            Name = "Foraging Overlord";
            Title = "the Supreme Healer";

            // Update stats to match or exceed Barracoon's or improve where needed
            SetStr(500); // Enhanced strength
            SetDex(200); // Enhanced dexterity
            SetInt(300); // Enhanced intelligence

            SetHits(12000); // Boss-level health
            SetDamage(10, 20); // Enhanced damage range

            // Enhanced resistances, matching or improving Barracoon's
            SetResistance(ResistanceType.Physical, 70, 80);
            SetResistance(ResistanceType.Fire, 60, 70);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 70, 80);
            SetResistance(ResistanceType.Energy, 60, 70);

            // Enhanced skills
            SetSkill(SkillName.Anatomy, 100.0);
            SetSkill(SkillName.EvalInt, 80.0);
            SetSkill(SkillName.Healing, 100.0);
            SetSkill(SkillName.MagicResist, 100.0);
            SetSkill(SkillName.Tactics, 80.0);
            SetSkill(SkillName.Wrestling, 100.0);

            Fame = 22500;
            Karma = -22500;

            VirtualArmor = 50;

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
            // Additional boss logic could be added here, such as special abilities or behaviors
        }

        public ForagerBoss(Serial serial) : base(serial)
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
