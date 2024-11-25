using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the spear overlord")]
    public class SpearSentryBoss : SpearSentry
    {
        [Constructable]
        public SpearSentryBoss() : base()
        {
            Name = "Spear Overlord";
            Title = "the Supreme Sentry";

            // Enhance stats to match or exceed Barracoon's
            SetStr(800); // Increased strength
            SetDex(200); // Increased dexterity
            SetInt(150); // Increased intelligence

            SetHits(12000); // Increased health to match a boss
            SetDamage(25, 35); // Enhanced damage range for boss difficulty

            SetResistance(ResistanceType.Physical, 75, 80);
            SetResistance(ResistanceType.Fire, 60, 80);
            SetResistance(ResistanceType.Cold, 60, 80);
            SetResistance(ResistanceType.Poison, 70, 80);
            SetResistance(ResistanceType.Energy, 60, 80);

            SetSkill(SkillName.Anatomy, 100.0, 120.0); // Increased skill for boss
            SetSkill(SkillName.Tactics, 100.0, 120.0); // Increased skill for boss
            SetSkill(SkillName.Wrestling, 85.0, 105.0); // Increased skill for boss
            SetSkill(SkillName.Fencing, 100.0, 120.0); // Increased skill for boss

            Fame = 22500; // Higher fame
            Karma = -22500; // Negative karma (boss-tier)

            VirtualArmor = 70; // Increased virtual armor

            // Attach the XmlRandomAbility for a random ability
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
            // Additional logic for boss could be added here (e.g., more complex speech or behavior)
        }

        public SpearSentryBoss(Serial serial) : base(serial)
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
