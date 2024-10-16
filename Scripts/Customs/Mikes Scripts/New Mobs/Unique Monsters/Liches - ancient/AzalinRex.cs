using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("Azalin Rex's corpse")]
    public class AzalinRex : AncientLich
    {
        private DateTime m_NextTimeStop;
        private DateTime m_NextNecroticWave;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public AzalinRex()
            : base()
        {
            Name = "Azalin Rex";
            Hue = 2099; // Unique hue for Azalin Rex
			BaseSoundID = 412;

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

        public AzalinRex(Serial serial)
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

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    // Set random intervals for abilities
                    Random rand = new Random();
                    m_NextTimeStop = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextNecroticWave = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextTimeStop)
                {
                    TimeStop();
                }

                if (DateTime.UtcNow >= m_NextNecroticWave)
                {
                    NecroticWave();
                }
            }
        }

		private void TimeStop()
		{
			PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Azalin Rex unleashes a time-stopping spell! *");
			PlaySound(0x1E0); // Dramatic spell sound

			// Effects: Time dilation and slow-motion
			Effects.SendLocationEffect(Location, Map, 0x376A, 20, 10); // Ensure correct method parameters
			foreach (Mobile m in GetMobilesInRange(10))
			{
				if (m != this && m.Alive)
				{
					m.SendMessage("You feel time distorting around you!");
					m.Freeze(TimeSpan.FromSeconds(5)); // Freeze for 5 seconds

					// Increase Azalin Rex's damage temporarily
					this.SetDamage(30, 40);
					Timer.DelayCall(TimeSpan.FromSeconds(5), () =>
					{
						this.SetDamage(20, 30); // Reset damage after time stop
					});
				}
			}

			m_NextTimeStop = DateTime.UtcNow + TimeSpan.FromSeconds(60); // Cooldown for TimeStop
		}


        private void NecroticWave()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Azalin Rex unleashes a devastating necrotic wave! *");
            PlaySound(0x1E1); // Necrotic wave sound

            // Effects: Necrotic wave creating damaging field
            foreach (Mobile m in GetMobilesInRange(10))
            {
                if (m != this && m.Alive && CanBeHarmful(m))
                {
                    AOS.Damage(m, this, Utility.RandomMinMax(30, 50), 0, 100, 0, 0, 0); // Necrotic damage
                    m.SendMessage("You are engulfed by a wave of necrotic energy!");
                    m.SendMessage("You feel yourself slowing down!");
                    m.Paralyze(TimeSpan.FromSeconds(3)); // Slow down effect

                    // Create an area-of-effect field
                    Timer.DelayCall(TimeSpan.FromSeconds(0.5), () =>
                    {
                        if (m != null && m.Alive)
                        {
                            Effects.SendLocationEffect(m.Location, m.Map, 0x376A, 10, 1); // Ensure correct method parameters
                            m.SendMessage("The necrotic wave continues to sap your strength!");
                            AOS.Damage(m, this, Utility.RandomMinMax(10, 20), 0, 100, 0, 0, 0); // Continued necrotic damage
                        }
                    });
                }
            }

            m_NextNecroticWave = DateTime.UtcNow + TimeSpan.FromSeconds(45); // Cooldown for NecroticWave
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
