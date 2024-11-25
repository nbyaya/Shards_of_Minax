using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the enchanted overlord")]
    public class WeaponEnchanterBoss : WeaponEnchanter
    {
        [Constructable]
        public WeaponEnchanterBoss() : base()
        {
            Name = "Enchanted Overlord";
            Title = "the Supreme Enchanter";

            // Update stats to match or exceed a high-level boss
            SetStr(700, 1000); // Enhanced strength range
            SetDex(150, 200); // Enhanced dexterity range
            SetInt(300, 500); // Enhanced intelligence range

            SetHits(5000, 7000); // Significantly enhanced health

            SetDamage(25, 35); // Enhanced damage range

            SetResistance(ResistanceType.Physical, 75, 85);
            SetResistance(ResistanceType.Fire, 60, 80);
            SetResistance(ResistanceType.Cold, 60, 80);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 60, 80);

            SetSkill(SkillName.EvalInt, 120.0, 140.0);
            SetSkill(SkillName.Magery, 120.0, 140.0);
            SetSkill(SkillName.Meditation, 80.0, 100.0);
            SetSkill(SkillName.MagicResist, 90.0, 110.0);
            SetSkill(SkillName.Tactics, 80.0, 100.0);
            SetSkill(SkillName.Wrestling, 80.0, 100.0);

            Fame = 22500; // High fame, suitable for a boss
            Karma = -22500; // High negative karma

            VirtualArmor = 60; // Enhanced virtual armor

            // Attach a random ability for added complexity
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

        public WeaponEnchanterBoss(Serial serial) : base(serial)
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
