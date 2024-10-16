using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a solar elemental corpse")]
    public class SolarElemental : BaseCreature
    {
        private DateTime m_NextSolarFlare;
        private DateTime m_NextRadiantBurst;
        private DateTime m_NextSunsEmbrace;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public SolarElemental()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            this.Name = "a solar elemental";
            this.Body = 15; // Fire Elemental body
            this.Hue = 1595; // Unique hue for the Solar Elemental
            this.BaseSoundID = 838;

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

            this.PackItem(new SulfurousAsh(5));

            m_AbilitiesInitialized = false; // Initialize flag
        }

        public SolarElemental(Serial serial)
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
                    // Set random intervals for abilities
                    Random rand = new Random();
                    m_NextSolarFlare = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextRadiantBurst = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextSunsEmbrace = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextSolarFlare)
                {
                    CastSolarFlare();
                }

                if (DateTime.UtcNow >= m_NextRadiantBurst)
                {
                    PerformRadiantBurst();
                }

                if (DateTime.UtcNow >= m_NextSunsEmbrace)
                {
                    ActivateSunsEmbrace();
                }
            }
        }

        private void CastSolarFlare()
        {
            if (Combatant != null)
            {
                Point3D start = this.Location;
                Point3D end = Combatant.Location;
                Effects.SendLocationParticles(EffectItem.Create(start, Map, EffectItem.DefaultDuration), 0x36D4, 10, 30, 2115);
                Timer.DelayCall(TimeSpan.FromSeconds(1), new TimerCallback(delegate
                {
                    if (Combatant != null && Combatant.Map == this.Map)
                    {
                        Combatant.Damage(Utility.RandomMinMax(20, 30), this);
                    }
                }));
                m_NextSolarFlare = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Reset cooldown
            }
        }

        private void PerformRadiantBurst()
        {
            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Player)
                {
                    m.SendMessage("You are blinded by a burst of intense light!");
                    m.Damage(Utility.RandomMinMax(15, 25), this);
                }
            }

            Effects.SendLocationParticles(EffectItem.Create(this.Location, Map, EffectItem.DefaultDuration), 0x3709, 10, 30, 2115);
            m_NextRadiantBurst = DateTime.UtcNow + TimeSpan.FromSeconds(40); // Reset cooldown
        }

        private void ActivateSunsEmbrace()
        {
            Effects.SendLocationParticles(EffectItem.Create(this.Location, Map, EffectItem.DefaultDuration), 0x3709, 10, 30, 2115);

            Timer.DelayCall(TimeSpan.FromSeconds(1), new TimerCallback(delegate
            {
                foreach (Mobile m in GetMobilesInRange(3))
                {
                    if (m != this && m.Player)
                    {
                        m.Damage(Utility.RandomMinMax(5, 15), this);
                    }
                }
            }));

            Timer.DelayCall(TimeSpan.FromSeconds(10), new TimerCallback(delegate
            {
                this.Hue = 1358; // Reset hue to original after the effect
            }));

            m_NextSunsEmbrace = DateTime.UtcNow + TimeSpan.FromMinutes(2); // Reset cooldown
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
