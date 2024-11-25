using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the banneret overlord")]
    public class BanneretBoss : Banneret
    {
        [Constructable]
        public BanneretBoss() : base()
        {
            Name = "Banneret Overlord";
            Title = "the Supreme Banneret";

            // Update stats to match or exceed Barracoon's
            SetStr(1200); // Matching Barracoon's upper strength
            SetDex(255); // Maximized dexterity for a boss-tier challenge
            SetInt(250); // Elevated intelligence for strategic effects

            SetHits(12000); // Boss-level health
            SetDamage(29, 38); // Boss-level damage range

            // Resistance improvements for a higher-tier boss
            SetResistance(ResistanceType.Physical, 75, 85);
            SetResistance(ResistanceType.Fire, 70, 80);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 100); // Higher poison resistance for tankiness
            SetResistance(ResistanceType.Energy, 50, 60);

            // Enhanced skills to match Barracoon's proficiency
            SetSkill(SkillName.Anatomy, 50.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Swords, 120.0);
            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Parry, 100.0);

            Fame = 22500;
            Karma = -22500;

            VirtualArmor = 85; // Higher virtual armor for added toughness

            // Attach a random ability to increase difficulty
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
            // Additional boss logic could be added here for special behaviors
        }

        public BanneretBoss(Serial serial) : base(serial)
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
