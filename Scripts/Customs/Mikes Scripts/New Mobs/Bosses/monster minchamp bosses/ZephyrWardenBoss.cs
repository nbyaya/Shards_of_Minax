using System;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a zephyr overlord corpse")]
    public class ZephyrWardenBoss : ZephyrWarden
    {
        [Constructable]
        public ZephyrWardenBoss()
            : base()
        {
            Name = "Zephyr Overlord";
            Title = "the Supreme Warden";

            // Update stats to match or exceed Barracoon's stats
            SetStr(1200); // Enhanced strength
            SetDex(255); // Enhanced dexterity
            SetInt(250); // Enhanced intelligence

            SetHits(12000); // Boss-level health
            SetDamage(35, 45); // Enhanced damage range

            SetResistance(ResistanceType.Physical, 80, 90); // Higher resistances
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 60, 80);
            SetResistance(ResistanceType.Poison, 100); // Same as original
            SetResistance(ResistanceType.Energy, 60, 70);

            SetSkill(SkillName.MagicResist, 150.0); // Higher magic resist skill
            SetSkill(SkillName.Tactics, 110.0); // Higher tactics skill
            SetSkill(SkillName.Wrestling, 110.0); // Higher wrestling skill

            Fame = 35000; // Increased fame
            Karma = -35000; // Increased karma

            VirtualArmor = 120; // Increased virtual armor

            Tamable = false; // Not tamable at boss level
            ControlSlots = 0; // Boss cannot be controlled

            // Attach the XmlRandomAbility for extra randomness in abilities
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

        public ZephyrWardenBoss(Serial serial)
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
