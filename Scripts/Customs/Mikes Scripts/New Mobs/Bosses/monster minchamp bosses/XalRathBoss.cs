using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a blighted corpse")]
    public class XalRathBoss : XalRath
    {
        [Constructable]
        public XalRathBoss() : base()
        {
            Name = "Xal'Rath the Blighted Overlord";
            Title = "the Supreme Blighted";

            // Update stats to match or exceed boss-tier (Barracoon-level) strength
            SetStr(1200, 1500); // Enhanced strength for a boss
            SetDex(255); // Maximum dexterity for increased speed
            SetInt(250); // Maximum intelligence for stronger magic

            SetHits(12000); // Matching or exceeding Barracoon's health
            SetDamage(35, 45); // Enhanced damage range

            SetResistance(ResistanceType.Physical, 80, 90); // Max resistance for bosses
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 70, 80);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 60, 75);

            SetSkill(SkillName.MagicResist, 150.0); // Higher resistance to magic
            SetSkill(SkillName.Tactics, 120.0); // Improved tactics for a stronger boss
            SetSkill(SkillName.Wrestling, 120.0); // Enhanced wrestling skill for better combat

            Fame = 30000; // Increased fame
            Karma = -30000; // Increased karma for a stronger boss

            VirtualArmor = 100; // Stronger virtual armor

            // Attach the XmlRandomAbility to provide dynamic abilities
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

        public XalRathBoss(Serial serial) : base(serial)
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
