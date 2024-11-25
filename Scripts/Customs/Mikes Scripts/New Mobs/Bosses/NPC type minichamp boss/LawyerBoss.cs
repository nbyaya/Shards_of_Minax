using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a lawyer's corpse")]
    public class LawyerBoss : Lawyer
    {
        [Constructable]
        public LawyerBoss() : base()
        {
            Name = "Lawyer Overlord";
            Title = "the Supreme Attorney";

            // Update stats to match or exceed Barracoon-like power
            SetStr(1200); // High strength for a boss-level challenge
            SetDex(255);  // High dexterity to improve its agility
            SetInt(250);  // High intelligence for better spellcasting and defense

            SetHits(12000); // High health for a boss encounter
            SetDamage(25, 40); // Increased damage range to match a stronger boss

            SetResistance(ResistanceType.Physical, 75, 85);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 60, 80);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.Magery, 120.0); // High magery skill for a potent spellcaster
            SetSkill(SkillName.MagicResist, 150.0); // Stronger resistance to magic
            SetSkill(SkillName.Tactics, 100.0); // Boss-level tactics skill
            SetSkill(SkillName.Wrestling, 100.0); // High wrestling for physical defense

            Fame = 22500; // Matching Barracoon's fame
            Karma = -22500; // Negative karma for the villainous nature

            VirtualArmor = 80; // High virtual armor for better defense

            // Attach a random ability to make the boss more unpredictable
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

        public LawyerBoss(Serial serial) : base(serial)
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
