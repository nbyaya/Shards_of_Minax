using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("an aries rambear corpse")]
    public class AriesRamBear : GrizzlyBear
    {
        private DateTime m_NextRamCharge;
        private DateTime m_NextBurningPassion;
        private DateTime m_NextAriesRage;

        [Constructable]
        public AriesRamBear() : base()
        {
            Name = "an Aries RamBear";
            Hue = 2065; // Fiery hue for a dramatic effect
            Body = 212;
			BaseSoundID = 0xA3;
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

            m_NextRamCharge = DateTime.UtcNow;
            m_NextBurningPassion = DateTime.UtcNow;
            m_NextAriesRage = DateTime.UtcNow;
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                // Ram Charge ability
                if (DateTime.UtcNow >= m_NextRamCharge && Utility.RandomDouble() < 0.1) // 10% chance
                {
                    RamCharge();
                    m_NextRamCharge = DateTime.UtcNow + TimeSpan.FromSeconds(30); // cooldown
                }

                // Burning Passion ability
                if (DateTime.UtcNow >= m_NextBurningPassion && Utility.RandomDouble() < 0.1) // 10% chance
                {
                    BurningPassion();
                    m_NextBurningPassion = DateTime.UtcNow + TimeSpan.FromMinutes(2); // cooldown
                }

                // Aries Rage ability
                if (DateTime.UtcNow >= m_NextAriesRage && Utility.RandomDouble() < 0.1) // 10% chance
                {
                    AriesRage();
                    m_NextAriesRage = DateTime.UtcNow + TimeSpan.FromMinutes(1); // cooldown
                }
            }
        }

		private void RamCharge()
		{
			PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Aries RamBear charges with unstoppable force! *");

			if (Combatant != null)
			{
				Mobile target = Combatant as Mobile;

				if (target != null)
				{
					int damage = (int)(DamageMax * 2); // double damage
					target.SendMessage("You are charged by the Aries RamBear!");
					
					// Correct usage of AOS.Damage
					AOS.Damage(target, this, damage, 0, 100, 0, 0, 0);

					target.Freeze(TimeSpan.FromSeconds(2)); // stun effect

					// Push the target away
					Point3D newLocation = new Point3D(target.X + Utility.RandomMinMax(-3, 3), target.Y + Utility.RandomMinMax(-3, 3), target.Z);

					// Correct usage of MoveToWorld
					target.MoveToWorld(newLocation, target.Map);
				}
			}

			PlaySound(0x29E); // sound effect for charge
		}


		private void BurningPassion()
		{
			PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Aries RamBear is engulfed in flames of burning passion! *");

			// Create a particle effect at the Aries RamBear's location
			this.FixedParticles(0x36BD, 10, 30, 5052, EffectLayer.Waist);

			// Damage nearby enemies
			foreach (Mobile m in GetMobilesInRange(5))
			{
				if (m != this && m.Player && m.Alive)
				{
					int damage = Utility.RandomMinMax(5, 10);
					AOS.Damage(m, this, damage, 0, 100, 0, 0, 0);
					m.SendMessage("You are burned by the Aries RamBear's passion!");
					m.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.LeftFoot);
				}
			}

			Timer.DelayCall(TimeSpan.FromSeconds(10), new TimerCallback(() => RemoveBurningPassion()));

			PlaySound(0x2B0); // sound effect for rage
		}


        private void RemoveBurningPassion()
        {
            // Reset attack speed or any other effects if needed
        }

        private void AriesRage()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Aries RamBear erupts in a blaze of fury! *");

            // Damage nearby enemies with fire
            foreach (Mobile m in GetMobilesInRange(7))
            {
                if (m != this && m.Player && m.Alive)
                {
                    int damage = Utility.RandomMinMax(15, 25);
                    AOS.Damage(m, this, damage, 0, 100, 0, 0, 0);
                    m.SendMessage("You are scorched by the Aries RamBear's fury!");
                    m.FixedParticles(0x36BD, 10, 30, 5052, EffectLayer.LeftFoot);
                }
            }

            PlaySound(0x2D0); // sound effect for explosion
        }

        public AriesRamBear(Serial serial) : base(serial)
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
