using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("Raistlin's corpse")]
    public class RaistlinMajere : BaseCreature
    {
        private DateTime m_NextMeteorSwarm;
        private DateTime m_NextStaffOfMagius;
        private DateTime m_NextArcaneExplosion;
        private DateTime m_NextManaShield;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public RaistlinMajere()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Raistlin Majere";
            Body = 78; // Ancient Lich body
            Hue = 2095; // Unique hue for aura effect
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

            m_AbilitiesInitialized = false;
        }

        public RaistlinMajere(Serial serial)
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
                    Random rand = new Random();
                    m_NextMeteorSwarm = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextStaffOfMagius = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextArcaneExplosion = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 25));
                    m_NextManaShield = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextMeteorSwarm)
                {
                    MeteorSwarm();
                }

                if (DateTime.UtcNow >= m_NextStaffOfMagius)
                {
                    StaffOfMagius();
                }

                if (DateTime.UtcNow >= m_NextArcaneExplosion)
                {
                    ArcaneExplosion();
                }

                if (DateTime.UtcNow >= m_NextManaShield)
                {
                    ManaShield();
                }
            }
        }

        private void MeteorSwarm()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Raistlin Majere calls down a barrage of meteors! *");
            PlaySound(0x20C); // Meteor sound

            for (int i = 0; i < 5; i++)
            {
                Point3D loc = new Point3D(this.X + Utility.RandomMinMax(-5, 5), this.Y + Utility.RandomMinMax(-5, 5), this.Z);
                Timer.DelayCall(TimeSpan.FromSeconds(i * 0.5), () =>
                {
                    Effects.SendLocationEffect(loc, Map, 0x36BD, 20, 10);
                    foreach (Mobile m in GetMobilesInRange(3))
                    {
                        if (m != this && m.Alive && !m.IsDeadBondedPet)
                        {
                            int damage = Utility.RandomMinMax(40, 60);
                            AOS.Damage(m, this, damage, 0, 100, 0, 0, 0);
                            m.SendMessage("You are struck by a meteor!");

                            // Fire Trap effect
                            Timer.DelayCall(TimeSpan.FromSeconds(1), () =>
                            {
                                if (Utility.RandomDouble() < 0.5) // 50% chance to create a fire trap
                                {
                                    Effects.SendLocationEffect(m.Location, Map, 0x36BD, 20, 10); // Fire trap visual
                                    AOS.Damage(m, this, Utility.RandomMinMax(10, 20), 0, 0, 0, 0, 0); // Fire damage
                                    m.SendMessage("You are scorched by a fire trap!");
                                }
                            });
                        }
                    }
                });
            }

            m_NextMeteorSwarm = DateTime.UtcNow + TimeSpan.FromSeconds(60); // Cooldown for MeteorSwarm
        }

        private void StaffOfMagius()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Raistlin Majere releases a burst of energy from his staff! *");
            PlaySound(0x20C); // Energy burst sound

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet)
                {
                    AOS.Damage(m, this, Utility.RandomMinMax(20, 40), 0, 100, 0, 0, 0);
                    m.SendMessage("You are hit by a powerful burst of arcane energy!");
                    m.PlaySound(0x20C); // Knockback sound
                    m.Location = new Point3D(m.X + Utility.RandomMinMax(-2, 2), m.Y + Utility.RandomMinMax(-2, 2), m.Z);

                    // Area effect reducing combat effectiveness
                    Timer.DelayCall(TimeSpan.FromSeconds(1), () =>
                    {
                        if (m.Alive)
                        {
                            m.SendMessage("You feel weakened by the arcane energy!");
                            m.Damage(5, this); // Additional damage
                        }
                    });
                }
            }

            m_NextStaffOfMagius = DateTime.UtcNow + TimeSpan.FromSeconds(45); // Cooldown for StaffOfMagius
        }

		private void ArcaneExplosion()
		{
			PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Raistlin Majere unleashes an arcane explosion! *");
			PlaySound(0x1F1); // Explosion sound

			Effects.SendLocationEffect(Location, Map, 0x36BD, 20, 10);

			foreach (Mobile m in GetMobilesInRange(3))
			{
				if (m != this && m.Alive && !m.IsDeadBondedPet)
				{
					AOS.Damage(m, this, Utility.RandomMinMax(30, 50), 0, 100, 0, 0, 0);
					m.SendMessage("You are hit by a powerful arcane explosion and feel disoriented!");
					m.PlaySound(0x1F1); // Explosion sound

					// Simulate stun effect
					Timer.DelayCall(TimeSpan.FromSeconds(1), () =>
					{
						if (m.Alive)
						{
							m.SendMessage("You feel disoriented and confused!");
							
							// Prevent mobile from moving and performing actions
							m.Frozen = true;

							// After a short duration, unfreeze the mobile
							Timer.DelayCall(TimeSpan.FromSeconds(2), () =>
							{
								if (m.Alive)
								{
									m.Frozen = false;
								}
							});
						}
					});
				}
			}

			m_NextArcaneExplosion = DateTime.UtcNow + TimeSpan.FromSeconds(45); // Cooldown for ArcaneExplosion
		}


        private void ManaShield()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Raistlin Majere activates his Mana Shield! *");
            PlaySound(0x20E); // Shield sound

            FixedParticles(0x36D4, 9, 32, 5006, EffectLayer.Waist); // Shield effect

            int shieldAmount = (int)(Mana * 0.5); // Mana shield absorbs 50% of damage
            Mana -= shieldAmount;
            Hits += shieldAmount;

            m_NextManaShield = DateTime.UtcNow + TimeSpan.FromSeconds(60); // Cooldown for ManaShield
        }

        public override void OnDamage(int amount, Mobile from, bool willKill)
        {
            base.OnDamage(amount, from, willKill);

            if (!willKill)
            {
                // Ensure the aura is visible
                FixedParticles(0x36D4, 9, 32, 5006, EffectLayer.Waist);
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
            m_AbilitiesInitialized = false;
        }
    }
}
