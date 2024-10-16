using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("an inferno sentinel corpse")]
    public class InfernoSentinel : BaseCreature
    {
        private DateTime m_NextFlameBurst;
        private DateTime m_NextHeatShield;
        private DateTime m_NextFlameAura;
        private DateTime m_NextSelfDestruct;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public InfernoSentinel()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "an inferno sentinel";
            Body = 0x2F4; // ExodusOverseer body
            Hue = 2290; // Unique hue for the Inferno Sentinel

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

            m_AbilitiesInitialized = false; // Initialize the flag
        }

        public InfernoSentinel(Serial serial)
            : base(serial)
        {
        }
        public override int GetIdleSound()
        {
            return 0xFD;
        }

        public override int GetAngerSound()
        {
            return 0x26C;
        }

        public override int GetDeathSound()
        {
            return 0x211;
        }

        public override int GetAttackSound()
        {
            return 0x23B;
        }

        public override int GetHurtSound()
        {
            return 0x140;
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
                    m_NextFlameBurst = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextHeatShield = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_NextFlameAura = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextSelfDestruct = DateTime.UtcNow + TimeSpan.FromMinutes(1); // Self-destruct in 1 minute
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextFlameBurst)
                {
                    FlameBurst();
                }

                if (DateTime.UtcNow >= m_NextHeatShield)
                {
                    HeatShield();
                }

                if (DateTime.UtcNow >= m_NextFlameAura)
                {
                    FlameAura();
                }

                if (DateTime.UtcNow >= m_NextSelfDestruct)
                {
                    SelfDestruct();
                }
            }
        }

        private void FlameBurst()
        {
            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m.Alive && !m.Hidden)
                {
                    m.FixedEffect(0x3709, 10, 16); // Flames shooting effect
                    m.SendMessage("You are engulfed in a burst of flames!");
                    AOS.Damage(m, this, Utility.RandomMinMax(20, 30), 0, 0, 100, 0, 0);
                }
            }

            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Unleashes a burst of flames! *");
            m_NextFlameBurst = DateTime.UtcNow + TimeSpan.FromSeconds(30); // 30 second cooldown
        }

        private void HeatShield()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Activates heat shield! *");
            FixedParticles(0x3735, 1, 30, 0x251F, EffectLayer.Waist); // Heat shield effect
            PlaySound(0x1F8); // Shield activation sound

            this.VirtualArmor += 20; // Temporary damage reduction
            this.SendMessage("The Inferno Sentinel's heat shield is active!");
            Timer.DelayCall(TimeSpan.FromSeconds(10), () =>
            {
                this.VirtualArmor -= 20; // Shield deactivation
                this.SendMessage("The Inferno Sentinel's heat shield has faded.");
            });

            m_NextHeatShield = DateTime.UtcNow + TimeSpan.FromMinutes(2); // 2 minute cooldown
        }

        private void FlameAura()
        {
            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive)
                {
                    m.FixedEffect(0x3709, 10, 16); // Flames aura effect
                    m.SendMessage("You are scorched by the Inferno Sentinel's aura!");
                    AOS.Damage(m, this, Utility.RandomMinMax(10, 15), 0, 0, 100, 0, 0);
                }
            }

            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Emits a fiery aura! *");
            m_NextFlameAura = DateTime.UtcNow + TimeSpan.FromSeconds(20); // 20 second cooldown
        }

        private void SelfDestruct()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Initiating self-destruct sequence! *");
            FixedEffect(0x36D4, 10, 16); // Explosion effect
            PlaySound(0x1F5); // Explosion sound

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive)
                {
                    m.SendMessage("You are caught in the Inferno Sentinel's self-destruct explosion!");
                    AOS.Damage(m, this, Utility.RandomMinMax(40, 60), 0, 0, 100, 0, 0);
                }
            }

            Delete(); // Remove the Inferno Sentinel after the explosion
        }

        public override void OnGotMeleeAttack(Mobile attacker)
        {
            base.OnGotMeleeAttack(attacker);

            if (this.VirtualArmor > 50)
            {
                attacker.SendMessage("The heat shield reflects some of the fire damage!");
                AOS.Damage(attacker, this, Utility.RandomMinMax(5, 15), 0, 0, 100, 0, 0);
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

            m_AbilitiesInitialized = false; // Reset the initialization flag
        }
    }
}
