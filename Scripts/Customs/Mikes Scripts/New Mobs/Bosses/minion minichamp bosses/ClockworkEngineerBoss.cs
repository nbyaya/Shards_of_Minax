using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the clockwork overlord")]
    public class ClockworkEngineerBoss : ClockworkEngineer
    {
        [Constructable]
        public ClockworkEngineerBoss() : base()
        {
            Name = "Clockwork Overlord";
            Title = "the Supreme Engineer";

            // Update stats to match or exceed Barracoon's level of strength and power
            SetStr(700, 1000);  // Enhanced strength
            SetDex(150, 200);   // Enhanced dexterity
            SetInt(300, 400);   // Enhanced intelligence

            SetHits(12000);     // Boosted health to boss level
            SetDamage(25, 40);  // Increased damage range

            SetResistance(ResistanceType.Physical, 70, 80);   // Enhanced resistances
            SetResistance(ResistanceType.Fire, 50, 60);
            SetResistance(ResistanceType.Cold, 40, 50);
            SetResistance(ResistanceType.Poison, 60, 70);
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.Anatomy, 100.0, 120.0);  // Enhanced skills
            SetSkill(SkillName.EvalInt, 100.0, 120.0);
            SetSkill(SkillName.Magery, 100.0, 120.0);
            SetSkill(SkillName.MagicResist, 100.0, 120.0);
            SetSkill(SkillName.Tactics, 100.0, 120.0);
            SetSkill(SkillName.Wrestling, 100.0, 120.0);

            Fame = 22500;
            Karma = -22500;

            VirtualArmor = 80;

            // Attach the random ability XML attachment
            XmlAttach.AttachTo(this, new XmlRandomAbility());
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot();
			PackItem(new RandomMagicWeapon());
			PackItem(new RandomMagicArmor());
			PackItem(new RandomMagicClothing());
			PackItem(new RandomMagicJewelry());
			PackItem(new SalvageMachine());
			PackItem(new TrapSleeves());
			PackItem(new BossTreasureBox());
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }
        }

        public ClockworkEngineerBoss(Serial serial) : base(serial)
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
