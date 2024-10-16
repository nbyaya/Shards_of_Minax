using System;
using Server;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a cancer shellbear corpse")]
    public class CancerShellBear : BaseCreature
    {
        private bool m_ShellGuardActive;
        private DateTime m_NextShellGuard;
        private DateTime m_NextRegenerativeEmbrace;
        private DateTime m_NextLunarRoar;

        private bool m_AbilitiesInitialized;

        [Constructable]
        public CancerShellBear()
            : base(AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            Name = "a Cancer ShellBear";
            Body = 212; // GrizzlyBear body
            Hue = 2024; // Unique hue for the shell bear
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

            m_AbilitiesInitialized = false; // Initialize flag
        }

        public CancerShellBear(Serial serial)
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
                    m_NextShellGuard = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(10, 30)); // Random interval for ShellGuard
                    m_NextRegenerativeEmbrace = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(5, 15)); // Random interval for RegenerativeEmbrace
                    m_NextLunarRoar = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(20, 40)); // Random interval for LunarRoar
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextShellGuard)
                {
                    ShellGuard();
                }

                if (DateTime.UtcNow >= m_NextRegenerativeEmbrace)
                {
                    RegenerativeEmbrace();
                }

                if (DateTime.UtcNow >= m_NextLunarRoar && Utility.RandomDouble() < 0.2) // 20% chance
                {
                    LunarRoar();
                }
            }
        }

        private void ShellGuard()
        {
            if (!m_ShellGuardActive)
            {
                m_ShellGuardActive = true;
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Cancer ShellBear retreats into its shell and starts emitting a protective aura! *");
                FixedEffect(0x376A, 10, 16); // Shell effect
                Timer.DelayCall(TimeSpan.FromSeconds(5), new TimerCallback(EndShellGuard));

                // Slow effect for enemies within range
                foreach (Mobile m in GetMobilesInRange(5))
                {
                    if (m != this && m.Alive && m is PlayerMobile)
                    {
                        m.SendMessage("You feel the aura of the Cancer ShellBear slowing you down!");
                        m.SendMessage("Your movements are hindered by the bear's shell!");
                        m.Damage(0); // Placeholder for the actual slow effect
                        // Apply a debuff effect here
                    }
                }

                m_NextShellGuard = DateTime.UtcNow + TimeSpan.FromSeconds(30);
            }
        }

        private void EndShellGuard()
        {
            m_ShellGuardActive = false;
        }

        private void RegenerativeEmbrace()
        {
            if (Hits < HitsMax)
            {
                Hits += 5;
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Cancer ShellBear is healing rapidly! *");
                FixedEffect(0x373A, 10, 16); // Healing effect

                // Apply a debuff to nearby enemies
                foreach (Mobile m in GetMobilesInRange(3))
                {
                    if (m != this && m.Alive && m is PlayerMobile)
                    {
                        m.SendMessage("You feel weakened by the Cancer ShellBear's regenerative aura!");
                        // Apply a debuff effect here
                    }
                }

                m_NextRegenerativeEmbrace = DateTime.UtcNow + TimeSpan.FromSeconds(15); // Adjusted cooldown
            }
        }

        private void LunarRoar()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Cancer ShellBear lets out a powerful roar! *");
            FixedEffect(0x374A, 10, 16); // Roar effect
            PlaySound(0x50A); // Roar sound

            foreach (Mobile m in GetMobilesInRange(7))
            {
                if (m != this && m.Alive && m is PlayerMobile)
                {
                    m.SendMessage("You are stunned by the Cancer ShellBear's roar!");
                    m.Freeze(TimeSpan.FromSeconds(2)); // Stun effect

                    int damage = Utility.RandomMinMax(10, 20);
                    AOS.Damage(m, this, damage, 0, 0, 100, 0, 0);
                }
            }

            m_NextLunarRoar = DateTime.UtcNow + TimeSpan.FromSeconds(40);
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (m_ShellGuardActive)
            {
                int damage = (int)(defender.Hits * 0.5);
                defender.SendMessage("You are hurt by the Cancer ShellBear's shell!");
                AOS.Damage(defender, this, damage, 0, 0, 100, 0, 0);
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
