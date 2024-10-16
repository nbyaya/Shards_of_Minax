using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a sky seraph corpse")]
    public class SkySeraph : BaseCreature
    {
        private DateTime m_NextCelestialWinds;
        private DateTime m_NextHeavenlyStrike;
        private DateTime m_NextAerialGrace;
        private bool m_AbilitiesActivated; // New flag to track initial ability activation

        [Constructable]
        public SkySeraph()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a sky seraph";
            Body = 13; // Air elemental body
            Hue = 1090; // Celestial hue
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

        public SkySeraph(Serial serial)
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

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesActivated)
                {
                    // Randomly set the initial activation times
                    Random rand = new Random();
                    m_NextCelestialWinds = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextHeavenlyStrike = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextAerialGrace = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));

                    m_AbilitiesActivated = true; // Set the flag to prevent re-initializing the times
                }

                if (DateTime.UtcNow >= m_NextCelestialWinds)
                {
                    CastCelestialWinds();
                }

                if (DateTime.UtcNow >= m_NextHeavenlyStrike)
                {
                    PerformHeavenlyStrike();
                }

                if (DateTime.UtcNow >= m_NextAerialGrace)
                {
                    ActivateAerialGrace();
                }
            }
        }

        private void CastCelestialWinds()
        {
            foreach (Mobile m in GetMobilesInRange(10))
            {
                if (m is BaseCreature && m.Alive) // Check if the creature is alive
                {
                    ((BaseCreature)m).Heal(10 + Utility.Random(20));
                    m.SendMessage("You feel the celestial winds healing and strengthening you!");
                }
            }

            m_NextCelestialWinds = DateTime.UtcNow + TimeSpan.FromMinutes(1);
        }

        private void PerformHeavenlyStrike()
        {
            if (Combatant != null)
            {
                Mobile target = Combatant as Mobile;
                if (target != null && target.Alive)
                {
                    // Send a light beam effect
                    Effects.SendTargetParticles(target, 0x1F5, 16, 16, 0, EffectLayer.Waist, 0);
                    target.Damage(40 + Utility.Random(20), this);
                    PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* A beam of light strikes you from the heavens! *");

                    m_NextHeavenlyStrike = DateTime.UtcNow + TimeSpan.FromMinutes(1);
                }
            }
        }

        private void ActivateAerialGrace()
        {
            this.VirtualArmor += 20;
            // Since SetDamage is a method, you should call it with new values
            SetDamage(20, 30 - 5); // Reducing max damage by 5

            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The sky seraph moves with divine grace, evading attacks more effectively! *");

            m_NextAerialGrace = DateTime.UtcNow + TimeSpan.FromMinutes(2);
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

            m_AbilitiesActivated = false; // Reset flag to reinitialize on next OnThink call
        }
    }
}
