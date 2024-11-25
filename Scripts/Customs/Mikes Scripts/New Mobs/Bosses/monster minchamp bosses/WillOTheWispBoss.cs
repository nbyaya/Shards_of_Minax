using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a will-o'-the-wisp corpse")]
    public class WillOTheWispBoss : WillOTheWisp
    {
        [Constructable]
        public WillOTheWispBoss() : base()
        {
            Name = "The Will-O'-The-Wisp";
            Title = "the Eternal Light";

            // Update stats to match or exceed Barracoon
            SetStr(1200); // Increased strength for a boss-tier enemy
            SetDex(255); // Maximum dexterity
            SetInt(250); // Maximum intelligence

            SetHits(12000); // Increased hit points for a tougher challenge
            SetDamage(40, 50); // Increased damage range

            // Updated resistances for a stronger boss
            SetResistance(ResistanceType.Physical, 80, 95);
            SetResistance(ResistanceType.Fire, 80, 95);
            SetResistance(ResistanceType.Cold, 60, 80);
            SetResistance(ResistanceType.Poison, 80, 95);
            SetResistance(ResistanceType.Energy, 60, 80);

            SetSkill(SkillName.MagicResist, 150.0); // High magic resist for a tough fight
            SetSkill(SkillName.Tactics, 110.0); // Tactics adjusted for higher challenge
            SetSkill(SkillName.Wrestling, 110.0); // Wrestling skill for additional combat difficulty
            SetSkill(SkillName.Magery, 120.0); // Increased magery for stronger spells

            Fame = 30000; // Increased fame to reflect the difficulty of the boss
            Karma = -30000; // Negative karma for a dark, powerful boss

            VirtualArmor = 110; // High virtual armor to increase defense

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
            // Additional boss logic can be added here
        }

        public WillOTheWispBoss(Serial serial) : base(serial)
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
