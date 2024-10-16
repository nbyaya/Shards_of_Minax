using System;
using Server.Items;
using Server.Network;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("a storm herald corpse")]
    public class StormHerald : BaseCreature
    {
        private DateTime m_NextThunderclap;
        private DateTime m_NextLightningChain;
        private DateTime m_NextTempestRage;
        private bool m_IsRaging;
        private Timer m_RageTimer;
        private bool m_AbilitiesActivated; // Flag to track if abilities have been randomized

        [Constructable]
        public StormHerald()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a storm herald";
            Body = 13; // Wind elemental-like body
            Hue = 1093; // Stormy hue
            BaseSoundID = 655;

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
            SetResistance(ResistanceType.Poison, 100);
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

            m_AbilitiesActivated = false; // Initialize flag
        }

        public StormHerald(Serial serial)
            : base(serial)
        {
        }

		public override bool ReacquireOnMovement
		{
			get
			{
				return !Controlled;
			}
		}
		public override bool AutoDispel
		{
			get
			{
				return !Controlled;
			}
		}

		public override int TreasureMapLevel
		{
			get
			{
				return 5;
			}
		}
		
		public override bool CanAngerOnTame
		{
			get
			{
				return true;
			}
		}

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
                return 120.0;
            }
        }

        public override double DispelFocus
        {
            get
            {
                return 50.0;
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
                if (!m_AbilitiesActivated)
                {
                    Random rand = new Random();
                    m_NextThunderclap = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 15));
                    m_NextLightningChain = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextTempestRage = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 25));

                    m_AbilitiesActivated = true;
                }

                if (DateTime.UtcNow >= m_NextThunderclap)
                {
                    Thunderclap();
                }

                if (DateTime.UtcNow >= m_NextLightningChain)
                {
                    LightningChain();
                }

                if (DateTime.UtcNow >= m_NextTempestRage)
                {
                    TempestRage();
                }
            }
        }

        private void Thunderclap()
        {
            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Player)
                {
                    m.SendMessage("A thunderclap deafens you!");
                    m.Damage(Utility.RandomMinMax(15, 25), this);
                    m.SendMessage("You are deafened by the thunderclap, reducing your spell effectiveness!");
                }
            }

            m_NextThunderclap = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Update for next potential use
        }

        private void LightningChain()
        {
            Mobile target = Combatant as Mobile;
            if (target != null && target.Alive)
            {
                foreach (Mobile m in GetMobilesInRange(10))
                {
                    if (m != this && m.Alive)
                    {
                        Effects.SendTargetParticles(m, 0x3F4D, 1, 30, 0xC0F, 0, 0, 0, 0);
                        m.Damage(Utility.RandomMinMax(10, 20), this);
                        break; // Chain lightning affects up to 3 targets
                    }
                }
            }

            m_NextLightningChain = DateTime.UtcNow + TimeSpan.FromSeconds(40); // Update for next potential use
        }

        private void TempestRage()
        {
            if (m_IsRaging)
                return;

            // Increase damage output
            SetDamage(25, 35);

            // Increase speed by reducing attack delay
            if (m_RageTimer != null)
                m_RageTimer.Stop();

            m_RageTimer = Timer.DelayCall(TimeSpan.FromSeconds(10), new TimerCallback(ResetTempestRage));

            m_IsRaging = true;
            m_NextTempestRage = DateTime.UtcNow + TimeSpan.FromMinutes(2); // Update for next potential use
        }

        private void ResetTempestRage()
        {
            // Reset damage output to normal
            SetDamage(15, 20);

            // Reset attack speed
            if (m_RageTimer != null)
            {
                m_RageTimer.Stop();
                m_RageTimer = null;
            }

            m_IsRaging = false;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_NextThunderclap = DateTime.UtcNow;
            m_NextLightningChain = DateTime.UtcNow;
            m_NextTempestRage = DateTime.UtcNow;
            m_AbilitiesActivated = false; // Reset flag for re-initialization
        }
    }
}
