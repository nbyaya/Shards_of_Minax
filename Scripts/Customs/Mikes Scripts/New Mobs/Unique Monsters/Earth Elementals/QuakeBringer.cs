using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a quake bringer corpse")]
    public class QuakeBringer : BaseCreature
    {
        private DateTime m_NextTremor;
        private DateTime m_NextEarthquake;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public QuakeBringer()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a quake bringer";
            Body = 14; // Earth elemental body
            BaseSoundID = 268; // Earth elemental sound ID
            Hue = 1492; // Mud color

            SetStr(1000, 1200);
            SetDex(177, 255);
            SetInt(151, 250);
			
            SetHits(700, 1200);
			
            SetDamage(29, 35);
			
            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 65, 80);
            SetResistance(ResistanceType.Fire, 60, 80);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 65, 80);
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.Anatomy, 25.1, 50.0);
            SetSkill(SkillName.EvalInt, 90.1, 100.0);
            SetSkill(SkillName.Magery, 95.5, 100.0);
            SetSkill(SkillName.Meditation, 25.1, 50.0);
            SetSkill(SkillName.MagicResist, 100.5, 150.0);
            SetSkill(SkillName.Tactics, 90.1, 100.0);
            SetSkill(SkillName.Wrestling, 90.1, 100.0);

            Fame = 24000;
            Karma = -24000;

            VirtualArmor = 90;
			
            Tamable = true;
            ControlSlots = 3;
            MinTameSkill = 93.9;

            m_AbilitiesInitialized = false; // Initialize flag
        }

        public QuakeBringer(Serial serial)
            : base(serial)
        {
        }

        public override bool ReacquireOnMovement => !Controlled;
        public override bool AutoDispel => !Controlled;
        public override int TreasureMapLevel => 5;
		public override bool CanAngerOnTame => true;
		public override void GenerateLoot()
		{
			this.AddLoot(LootPack.FilthyRich, 2);
			this.AddLoot(LootPack.Rich);
			this.AddLoot(LootPack.Gems, 8);
		}	

        public override double DispelDifficulty
        {
            get { return 120.0; }
        }

        public override double DispelFocus
        {
            get { return 50.0; }
        }

        public override bool BleedImmune
        {
            get { return true; }
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    Random rand = new Random();
                    m_NextTremor = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextEarthquake = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextTremor)
                {
                    PerformTremor();
                }

                if (DateTime.UtcNow >= m_NextEarthquake)
                {
                    PerformEarthquake();
                }
            }
        }

        private void PerformTremor()
        {
            foreach (Mobile m in GetMobilesInRange(4))
            {
                if (m != this && m.Player)
                {
                    m.SendMessage("The ground shakes violently under your feet!");
                    m.Damage(10, this); // Small damage
                    m.Freeze(TimeSpan.FromSeconds(2)); // Stunned
                }
            }

            Random rand = new Random();
            m_NextTremor = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(10, 30)); // Random cooldown between 10 and 30 seconds
        }

        private void PerformEarthquake()
        {
            foreach (Mobile m in GetMobilesInRange(6))
            {
                if (m != this && m.Player)
                {
                    m.SendMessage("An earthquake shakes the ground violently!");
                    m.Damage(30, this); // Significant damage
                    m.Freeze(TimeSpan.FromSeconds(5)); // Lose footing
                }
            }

            Effects.PlaySound(this.Location, this.Map, 0x54C); // Earthquake sound effect

            Random rand = new Random();
            m_NextEarthquake = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(30, 60)); // Random cooldown between 30 and 60 seconds
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

            m_AbilitiesInitialized = false; // Reset flag on deserialize
        }
    }
}
