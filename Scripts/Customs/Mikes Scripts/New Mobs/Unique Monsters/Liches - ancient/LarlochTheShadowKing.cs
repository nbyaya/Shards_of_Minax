using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("Larloch's corpse")]
    public class LarlochTheShadowKing : BaseCreature
    {
        private DateTime m_NextShadowBinding;
        private DateTime m_NextNecroticShield;
        private DateTime m_NextShadowBlast;
        private bool m_ShardShieldActive;
        private Timer m_ShieldTimer;

        [Constructable]
        public LarlochTheShadowKing()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Larloch, the Shadow King";
            Body = 78; // Ancient Lich body
            Hue = 2097; // Dark purple hue
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
            PackNecroReg(150, 250);

            m_ShardShieldActive = false;
        }

        public LarlochTheShadowKing(Serial serial)
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

        public override TribeType Tribe { get { return TribeType.Undead; } }

        public override OppositionGroup OppositionGroup
        {
            get
            {
                return OppositionGroup.FeyAndUndead;
            }
        }

        public override bool Unprovokable => true;
        public override bool BleedImmune => true;
        public override Poison PoisonImmune => Poison.Lethal;

        public override int GetIdleSound() => 0x19D;
        public override int GetAngerSound() => 0x175;
        public override int GetDeathSound() => 0x108;
        public override int GetAttackSound() => 0xE2;
        public override int GetHurtSound() => 0x28B;

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (DateTime.UtcNow >= m_NextShadowBinding)
                {
                    ShadowBinding();
                }

                if (DateTime.UtcNow >= m_NextNecroticShield && !m_ShardShieldActive)
                {
                    NecroticShield();
                }

                if (DateTime.UtcNow >= m_NextShadowBlast)
                {
                    ShadowBlast();
                }
            }
        }

		private void ShadowBinding()
		{
			if (Combatant != null)
			{
				// Cast Combatant to Mobile
				Mobile target = Combatant as Mobile;
				
				if (target != null)
				{
					PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Larloch summons shadowy tendrils to bind you! *");
					PlaySound(0x20C); // Dark spell sound

					// Create a shadow binding effect
					Effects.SendTargetEffect(target, 0x39F, 16);

					// Apply paralysis
					target.Freeze(TimeSpan.FromSeconds(5));

					m_NextShadowBinding = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown for Shadow Binding
				}
			}
		}


        private void NecroticShield()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Larloch envelops himself in a shield of necrotic energy! *");
            PlaySound(0x1F2); // Dark spell sound

            // Create a necrotic shield effect
            FixedParticles(0x376A, 9, 32, 5030, EffectLayer.Waist);
            m_ShardShieldActive = true;

            // Timer to handle shield damage and expiration
            m_ShieldTimer = Timer.DelayCall(TimeSpan.FromSeconds(10), () => DeactivateNecroticShield());

            m_NextNecroticShield = DateTime.UtcNow + TimeSpan.FromSeconds(60); // Cooldown for Necrotic Shield
        }

        private void DeactivateNecroticShield()
        {
            if (m_ShardShieldActive)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The necrotic shield around Larloch shatters, causing a burst of dark energy! *");
                PlaySound(0x1F2); // Explosion sound

                Effects.SendLocationEffect(Location, Map, 0x36BD, 20, 10); // Burst effect

                foreach (Mobile m in GetMobilesInRange(3))
                {
                    if (m != this && m.Alive && !m.IsDeadBondedPet)
                    {
                        int damage = Utility.RandomMinMax(50, 80);
                        AOS.Damage(m, this, damage, 0, 100, 0, 0, 0);
                    }
                }

                m_ShardShieldActive = false;
                m_ShieldTimer.Stop();
            }
        }

        private void ShadowBlast()
        {
            if (Combatant != null)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Larloch unleashes a blast of shadowy energy! *");
                PlaySound(0x1F3); // Dark spell sound

                // Shadow blast effect
                Effects.SendLocationEffect(Location, Map, 0x36BD, 20, 10); // Explosion effect

                foreach (Mobile m in GetMobilesInRange(5))
                {
                    if (m != this && m.Alive && !m.IsDeadBondedPet)
                    {
                        int damage = Utility.RandomMinMax(25, 45);
                        AOS.Damage(m, this, damage, 0, 100, 0, 0, 0);
                    }
                }

                m_NextShadowBlast = DateTime.UtcNow + TimeSpan.FromSeconds(45); // Cooldown for Shadow Blast
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
        }
    }
}
