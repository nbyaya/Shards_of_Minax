using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the vengeful bounty hunter")]
    public class BountyHunterBoss : BountyHunter
    {
        private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds(10.0); // Delay between speech lines
        public DateTime m_NextSpeechTime;

        [Constructable]
        public BountyHunterBoss() : base()
        {
            Name = "Vengeful Bounty Hunter";
            Title = "the Unstoppable";

            // Update stats to match or exceed Barracoon-style boss-level stats
            SetStr(1200); // Increased Strength
            SetDex(255);  // Max Dex
            SetInt(250);  // Higher Intelligence

            SetHits(12000); // Boss-tier Health
            SetDamage(29, 38); // Higher Damage

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 75); // Enhanced Resists
            SetResistance(ResistanceType.Fire, 80);
            SetResistance(ResistanceType.Cold, 60);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 50);

            SetSkill(SkillName.Anatomy, 50.0, 100.0);
            SetSkill(SkillName.EvalInt, 120.0);
            SetSkill(SkillName.Magery, 120.0);
            SetSkill(SkillName.Meditation, 50.0);
            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);
            SetSkill(SkillName.Archery, 100.0);
            SetSkill(SkillName.Bushido, 100.0);
            SetSkill(SkillName.Chivalry, 100.0);
            SetSkill(SkillName.Fencing, 100.0);

            Fame = 25000; // Increased Fame
            Karma = -25000; // Increased Karma (more evil)

            VirtualArmor = 80; // Enhanced Armor

            // Attach the random ability
            XmlAttach.AttachTo(this, new XmlRandomAbility());
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot();
            
            // Add 5 MaxxiaScrolls in addition to normal loot
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }
        }

        public override void OnThink()
        {
            base.OnThink();
            // Additional boss logic could be added here, if needed
        }

        public BountyHunterBoss(Serial serial) : base(serial)
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
