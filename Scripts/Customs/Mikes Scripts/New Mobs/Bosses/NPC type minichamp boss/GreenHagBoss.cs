using System;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the green hag overlord")]
    public class GreenHagBoss : GreenHag
    {
        [Constructable]
        public GreenHagBoss() : base()
        {
            Name = "Green Hag Overlord";
            Title = "the Supreme Witch";

            // Update stats to match or exceed Barracoon's better values
            SetStr(1200); // High strength for a boss
            SetDex(255); // Maximum dexterity
            SetInt(250); // High intelligence

            SetHits(10000); // Boss-level health
            SetDamage(25, 45); // Increased damage range

            SetResistance(ResistanceType.Physical, 80, 90);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 70, 85);
            SetResistance(ResistanceType.Poison, 100); // Max resistance to poison
            SetResistance(ResistanceType.Energy, 80, 90);

            SetSkill(SkillName.MagicResist, 150.0); // Max Magic Resist
            SetSkill(SkillName.Tactics, 120.0); // Boss-level tactics
            SetSkill(SkillName.Wrestling, 120.0); // Boss-level wrestling

            Fame = 22500;
            Karma = -22500;

            VirtualArmor = 80; // Boss-level armor

            // Attach a random ability for added boss uniqueness
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
            // Add extra boss logic or mechanics here if needed
        }

        public GreenHagBoss(Serial serial) : base(serial)
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
