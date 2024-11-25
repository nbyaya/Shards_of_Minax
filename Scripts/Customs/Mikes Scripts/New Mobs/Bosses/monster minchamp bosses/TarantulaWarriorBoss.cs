using System;
using Server.Items;
using Server.Network;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a tarantula warrior overlord corpse")]
    public class TarantulaWarriorBoss : TarantulaWarrior
    {
        private DateTime m_NextVenomousBite;
        private DateTime m_NextWebTrap;
        private DateTime m_NextFearsomeRoar;
        private DateTime m_NextSummonSpiders;
        private DateTime m_NextEnrage;

        [Constructable]
        public TarantulaWarriorBoss() : base()
        {
            Name = "Tarantula Warrior Overlord";
            Title = "the Spider King";
            Hue = 1782; // Unique hue for the boss

            // Enhanced stats (using or exceeding original values)
            SetStr(1200); // Exceeds original strength
            SetDex(255); // Exceeds original dexterity
            SetInt(250); // Exceeds original intelligence

            SetHits(15000); // High health to make it a boss-tier fight

            SetDamage(40, 50); // Increased damage range for boss-tier difficulty

            SetResistance(ResistanceType.Physical, 80, 100);
            SetResistance(ResistanceType.Fire, 80, 100);
            SetResistance(ResistanceType.Cold, 70, 90);
            SetResistance(ResistanceType.Poison, 80, 100);
            SetResistance(ResistanceType.Energy, 60, 80);

            SetSkill(SkillName.Anatomy, 50.0, 75.0);
            SetSkill(SkillName.EvalInt, 100.0, 120.0);
            SetSkill(SkillName.Magery, 100.0, 120.0);
            SetSkill(SkillName.MagicResist, 150.0, 200.0); // Enhanced magic resistance for the boss
            SetSkill(SkillName.Tactics, 100.0, 120.0);
            SetSkill(SkillName.Wrestling, 100.0, 120.0);

            Fame = 30000; // Increased fame to reflect the boss's difficulty
            Karma = -30000;

            VirtualArmor = 120; // Stronger virtual armor

            Tamable = false; // Not tamable for the boss version
            ControlSlots = 0; // Not tameable
            MinTameSkill = 0;

            // Attach the random ability for dynamic gameplay
            XmlAttach.AttachTo(this, new XmlRandomAbility());
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot();

            // Drop 5 MaxxiaScrolls in addition to the regular loot
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }
        }

        public override void OnThink()
        {
            base.OnThink();

            // Optional: Additional boss-specific AI or abilities could go here
        }

        public TarantulaWarriorBoss(Serial serial) : base(serial)
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
