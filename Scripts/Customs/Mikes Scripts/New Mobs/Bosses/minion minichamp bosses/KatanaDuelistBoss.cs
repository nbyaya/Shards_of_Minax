using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the katana master")]
    public class KatanaDuelistBoss : KatanaDuelist
    {
        [Constructable]
        public KatanaDuelistBoss() : base()
        {
            Name = "Katana Master";
            Title = "the Supreme Duelist";

            // Update stats to match or exceed Barracoon's stats
            SetStr(800); // Higher strength for a boss-tier challenge
            SetDex(250); // Increased dexterity for agility and speed
            SetInt(100); // Higher intelligence for additional skills

            SetHits(12000); // Boss-level health to make it a tough fight
            SetDamage(30, 45); // Increased damage for tougher combat

            SetResistance(ResistanceType.Physical, 70, 85); // Tougher physical resistance
            SetResistance(ResistanceType.Fire, 50, 70); // Fire resistance
            SetResistance(ResistanceType.Cold, 50, 70); // Cold resistance
            SetResistance(ResistanceType.Poison, 60, 80); // Poison resistance
            SetResistance(ResistanceType.Energy, 50, 70); // Energy resistance

            SetSkill(SkillName.Anatomy, 100.0, 120.0); // Higher skill for better attack precision
            SetSkill(SkillName.Fencing, 120.0, 140.0); // Enhanced fencing for more powerful strikes
            SetSkill(SkillName.Tactics, 120.0, 140.0); // Tactics increased for smarter combat
            SetSkill(SkillName.MagicResist, 100.0, 120.0); // Higher magic resistance for better defense
            SetSkill(SkillName.Bushido, 120.0, 140.0); // Bushido skill for advanced combat techniques

            Fame = 25000; // Increased fame for a boss-level enemy
            Karma = -25000; // Negative karma for being an enemy of the player

            VirtualArmor = 70; // Higher virtual armor for better defense

            // Attach the XmlRandomAbility for a random ability each time it's spawned
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
            // Additional boss logic could be added here for enhanced combat
        }

        public KatanaDuelistBoss(Serial serial) : base(serial)
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
