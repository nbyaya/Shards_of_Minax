using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the rune overlord")]
    public class RuneCasterBoss : RuneCaster
    {
        [Constructable]
        public RuneCasterBoss() : base()
        {
            Name = "Rune Overlord";
            Title = "the Supreme Rune Caster";

            // Update stats to match or exceed Barracoon
            SetStr(800); // Strength higher than original
            SetDex(200); // Dexterity higher than original
            SetInt(300); // Intelligence higher than original

            SetHits(12000); // Health boosted to match or exceed Barracoon
            SetDamage(25, 35); // Damage range adjusted for a boss

            SetResistance(ResistanceType.Physical, 75);
            SetResistance(ResistanceType.Fire, 80);
            SetResistance(ResistanceType.Cold, 75);
            SetResistance(ResistanceType.Poison, 75);
            SetResistance(ResistanceType.Energy, 80);

            SetSkill(SkillName.EvalInt, 100.0, 120.0);
            SetSkill(SkillName.Magery, 110.0, 120.0);
            SetSkill(SkillName.Meditation, 100.0, 120.0);
            SetSkill(SkillName.MagicResist, 100.0, 120.0);
            SetSkill(SkillName.Tactics, 90.0, 110.0);
            SetSkill(SkillName.Wrestling, 80.0, 100.0);

            Fame = 22500;
            Karma = -22500;

            VirtualArmor = 80;

            // Attach a random ability for extra challenge
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

        public RuneCasterBoss(Serial serial) : base(serial)
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
