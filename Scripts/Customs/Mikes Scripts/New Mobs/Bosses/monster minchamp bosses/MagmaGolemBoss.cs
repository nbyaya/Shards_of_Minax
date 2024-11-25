using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a magma overlord corpse")]
    public class MagmaGolemBoss : MagmaGolem
    {
        [Constructable]
        public MagmaGolemBoss() : base()
        {
            Name = "Magma Overlord";
            Title = "the Infernal Golem";

            // Update stats to match or exceed Barracoon's values
            SetStr(1200); // Higher than the original, matching boss-tier strength
            SetDex(255); // Maximizing dexterity for better combat performance
            SetInt(250); // Maintain the same high intelligence for magic-related abilities

            SetHits(12000); // Boss-tier health
            SetDamage(40, 50); // Increased damage to make it more threatening

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 30);
            SetDamageType(ResistanceType.Energy, 20);

            SetResistance(ResistanceType.Physical, 80, 90); // Improved resistances
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 70, 80);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.MagicResist, 150.0); // Boss-tier resistance
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);

            Fame = 24000; // Boss-tier fame
            Karma = -24000; // Negative karma for a villainous boss

            VirtualArmor = 90;

            Tamable = false;
            ControlSlots = 0;

            // Attach the random ability to the boss
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

            // Additional boss logic could be added here (like a special attack phase)
        }

        public MagmaGolemBoss(Serial serial) : base(serial)
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
