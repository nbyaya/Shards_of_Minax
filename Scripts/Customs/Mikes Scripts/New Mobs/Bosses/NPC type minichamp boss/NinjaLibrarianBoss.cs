using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the ninja librarian overlord")]
    public class NinjaLibrarianBoss : NinjaLibrarian
    {
        [Constructable]
        public NinjaLibrarianBoss() : base()
        {
            Name = "Ninja Librarian Overlord";
            Title = "the Master of Silence";

            // Update stats to match or exceed the boss standard (Barracoon)
            SetStr(1200); // Matching Barracoon's upper strength
            SetDex(255);  // Using the higher dexterity value from the original
            SetInt(250);  // Matching or exceeding Barracoon's intelligence

            SetHits(12000); // Higher health for the boss
            SetDamage(29, 38); // Matching Barracoon's damage range

            SetResistance(ResistanceType.Physical, 75);
            SetResistance(ResistanceType.Fire, 80);
            SetResistance(ResistanceType.Cold, 60);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 50);

            SetSkill(SkillName.MagicResist, 150.0); // Enhanced magic resistance
            SetSkill(SkillName.Tactics, 120.0); // Increased tactics
            SetSkill(SkillName.Wrestling, 120.0); // Increased wrestling

            Fame = 22500;
            Karma = -22500;

            VirtualArmor = 75; // Increased virtual armor

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
            // Additional boss logic can be added here, such as unique abilities or attacks
        }

        public NinjaLibrarianBoss(Serial serial) : base(serial)
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
