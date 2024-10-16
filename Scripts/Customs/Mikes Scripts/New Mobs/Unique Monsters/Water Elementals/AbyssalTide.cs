using System;
using Server.Items;
using Server.Network;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("an abyssal tide corpse")]
    public class AbyssalTide : BaseCreature
    {
        private DateTime m_NextAbyssalWave;
        private DateTime m_NextTidalPull;
        private DateTime m_NextDepthsEmbrace;

        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public AbyssalTide()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "an abyssal tide";
            Body = 16; // Water Elemental body
            BaseSoundID = 278;
			Hue = 2555; // Blue hue for storm effect

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
            this.CanSwim = true;

            m_AbilitiesInitialized = false; // Initialize flag
        }

        public AbyssalTide(Serial serial)
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
            get
            {
                return 117.5;
            }
        }
        public override double DispelFocus
        {
            get
            {
                return 45.0;
            }
        }
        public override bool BleedImmune
        {
            get
            {
                return true;
            }
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    Random rand = new Random();
                    m_NextAbyssalWave = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextTidalPull = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_NextDepthsEmbrace = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextAbyssalWave)
                {
                    AbyssalWave();
                }

                if (DateTime.UtcNow >= m_NextTidalPull)
                {
                    TidalPull();
                }

                if (DateTime.UtcNow >= m_NextDepthsEmbrace)
                {
                    DepthsEmbrace();
                }
            }
        }

        private void AbyssalWave()
        {
            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Player)
                {
                    m.Damage(Utility.RandomMinMax(15, 25), this);
                    m.SendMessage("You are struck by a wave of dark water!");
                    m.Location = new Point3D(m.X + Utility.RandomMinMax(-2, 2), m.Y + Utility.RandomMinMax(-2, 2), m.Z);
                }
            }

            m_NextAbyssalWave = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown for AbyssalWave
        }

        private void TidalPull()
        {
            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Player)
                {
                    m.Location = this.Location; // Pulls the target to itself
                    m.SendMessage("You are pulled toward the Abyssal Tide!");
                }
            }

            m_NextTidalPull = DateTime.UtcNow + TimeSpan.FromSeconds(40); // Cooldown for TidalPull
        }

        private void DepthsEmbrace()
        {
            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m.Player)
                {
                    m.SendMessage("You are caught in the Abyssal Tide's vortex!");
                    m.Damage(0); // Optional: apply any debuff or status effect
                }
            }

            // Create a visual effect
            Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration), 0x1F4, 10, 30, 1153);

            m_NextDepthsEmbrace = DateTime.UtcNow + TimeSpan.FromMinutes(2); // Cooldown for DepthsEmbrace
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)1); // version

            // Serialize additional fields if needed
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_AbilitiesInitialized = false; // Reset flag on deserialize
        }
    }
}
