using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a zel'vrak corpse")]
    public class ZelVrakBoss : ZelVrak
    {
        [Constructable]
        public ZelVrakBoss() : base()
        {
            Name = "Zel'Vrak the Supreme Enigmatic";
            Title = "the Boss of Illusions";

            // Enhanced stats
            SetStr(1200); // Higher strength to make the boss more formidable
            SetDex(255); // Increased dexterity for faster actions
            SetInt(250); // Increased intelligence for stronger spells

            SetHits(12000); // Increased health for the boss-tier challenge
            SetDamage(40, 50); // Higher damage for more threat

            // Enhanced resistances
            SetResistance(ResistanceType.Physical, 80, 90);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 70, 80);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 70, 80);

            // Increased skills for a tougher fight
            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);
            SetSkill(SkillName.Magery, 110.0);

            Fame = 30000;
            Karma = -30000; // Adjusted to represent a more powerful entity

            VirtualArmor = 100;

            // Attach a random ability to the boss
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
            // Additional boss logic or mechanics can be added here
        }

        public ZelVrakBoss(Serial serial) : base(serial)
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
