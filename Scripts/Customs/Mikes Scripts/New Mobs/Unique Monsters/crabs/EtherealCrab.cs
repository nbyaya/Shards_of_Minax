using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("an ethereal crab corpse")]
    public class EtherealCrab : CoconutCrab
    {
        private DateTime m_NextPhantomGrasp;
        private DateTime m_NextSpectralSlam;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public EtherealCrab()
            : base("Ethereal Crab")
        {
            // Override hue for the ghostly appearance
            Hue = 1460; // Light blue-green for spectral look
			BaseSoundID = 0x4F2;
			
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

            m_AbilitiesInitialized = false; // Initialize flag
        }

        public EtherealCrab(Serial serial)
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
                if (!m_AbilitiesInitialized)
                {
                    // Initialize random start times for abilities
                    Random rand = new Random();
                    m_NextPhantomGrasp = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextSpectralSlam = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 45));
                    m_AbilitiesInitialized = true; // Set flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextPhantomGrasp)
                {
                    PerformPhantomGrasp();
                }

                if (DateTime.UtcNow >= m_NextSpectralSlam)
                {
                    PerformSpectralSlam();
                }
            }
        }

        private void PerformPhantomGrasp()
        {
            // Display message for Phantom Grasp ability
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Ethereal Crab uses Phantom Grasp! *");

            Mobile target = Combatant as Mobile;
            if (target != null && target.Alive)
            {
                // Pull the target in
                target.SendMessage("You are pulled in by the ethereal grip!");
                target.FixedParticles(0x374A, 10, 30, 5052, EffectLayer.Waist);
                target.PlaySound(0x1F2);

                // Reduce target's damage output by 25% for 10 seconds
                Timer.DelayCall(TimeSpan.FromSeconds(10), () => {
                    if (target != null && target.Alive)
                    {
                        target.SendMessage("The phantom grasp fades away.");
                    }
                });

                // Set next use
                m_NextPhantomGrasp = DateTime.UtcNow + TimeSpan.FromMinutes(1);
            }
        }

        private void PerformSpectralSlam()
        {
            // Display message for Spectral Slam ability
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Ethereal Crab performs a Spectral Slam! *");

            Mobile target = Combatant as Mobile;
            if (target != null && target.Alive)
            {
                int damage = Utility.RandomMinMax(10, 20);

                // Deal additional damage if the target's damage output is reduced
                if (target is PlayerMobile && !target.IsDeadBondedPet)
                {
                    damage += 10; // Additional damage if target's damage is reduced
                }

                // Apply damage
                AOS.Damage(target, this, damage, 0, 100, 0, 0, 0);

                // Display effect
                target.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.LeftFoot);
                target.PlaySound(0x1F2);

                // Set next use
                m_NextSpectralSlam = DateTime.UtcNow + TimeSpan.FromMinutes(1);
            }
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
