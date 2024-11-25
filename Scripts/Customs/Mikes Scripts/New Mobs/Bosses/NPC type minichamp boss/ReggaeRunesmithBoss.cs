using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the reggae runesmith overlord")]
    public class ReggaeRunesmithBoss : ReggaeRunesmith
    {
        [Constructable]
        public ReggaeRunesmithBoss() : base()
        {
            Name = "Reggae Runesmith Overlord";
            Title = "the Master of Rhythms";

            // Update stats to match or exceed Barracoon or the best boss-tier standards
            SetStr(1200); // Increased strength for a boss
            SetDex(255); // Max dexterity for a boss
            SetInt(250); // Max intelligence for a boss

            SetHits(12000); // Much higher health than the regular version

            SetDamage(30, 45); // Higher damage range for a boss

            SetResistance(ResistanceType.Physical, 80, 90);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 70, 80);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 50, 70);

            SetSkill(SkillName.MagicResist, 150.0); // Increased skill for better resist
            SetSkill(SkillName.Tactics, 120.0); // Higher tactics skill for better combat performance
            SetSkill(SkillName.Wrestling, 120.0); // Increased wrestling for better close combat
            SetSkill(SkillName.Magery, 120.0); // Increased magery for better magic use

            Fame = 25000; // Higher fame for a boss NPC
            Karma = -25000; // Negative karma to match boss-tier status

            VirtualArmor = 80; // Increased armor for better defense

            // Attach a random ability to the boss NPC
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
            // Additional boss logic (like special attacks, increased speech frequency, etc.) could be added here
        }

        public ReggaeRunesmithBoss(Serial serial) : base(serial)
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
