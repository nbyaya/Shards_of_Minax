using System;
using Server.Items;
using Server.Network;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("a frost serpent corpse")]
    public class FrostSerpent : BaseCreature
    {
        private DateTime m_NextFreezingBreath;
        private DateTime m_NextBlizzardCall;
        private DateTime m_NextFrostNova;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public FrostSerpent()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a Frost Serpent";
            Body = 0x15; // Giant Serpent body
            Hue = 1780; // Icy blue hue
            BaseSoundID = 219;

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

        public FrostSerpent(Serial serial)
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
                    m_NextFreezingBreath = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextBlizzardCall = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextFrostNova = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextFreezingBreath)
                {
                    FreezingBreath();
                }

                if (DateTime.UtcNow >= m_NextBlizzardCall)
                {
                    BlizzardCall();
                }

                if (DateTime.UtcNow >= m_NextFrostNova)
                {
                    FrostNova();
                }
            }
        }

        private void FreezingBreath()
        {
            if (Combatant != null)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "The Frost Serpent exhales a freezing breath!");
                FixedEffect(0x36D4, 10, 16); // Ice breath effect

                foreach (Mobile m in GetMobilesInRange(3))
                {
                    if (m != this && m.Alive)
                    {
                        m.SendMessage("You are caught in the Frost Serpent's freezing breath!");
                        AOS.Damage(m, this, Utility.RandomMinMax(10, 20), 0, 100, 0, 0, 0);
                        m.Freeze(TimeSpan.FromSeconds(3)); // Slows down the target
                        // Apply frostbite effect for damage over time
                        Timer.DelayCall(TimeSpan.FromSeconds(3), () =>
                        {
                            if (m.Alive)
                                AOS.Damage(m, this, Utility.RandomMinMax(5, 10), 0, 100, 0, 0, 0);
                        });
                    }
                }

                m_NextFreezingBreath = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 30));
            }
        }

        private void BlizzardCall()
        {
            if (Combatant != null)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "A blizzard engulfs the area!");
                FixedEffect(0x36D4, 10, 16); // Snowstorm effect

                foreach (Mobile m in GetMobilesInRange(5))
                {
                    if (m != this && m.Alive)
                    {
                        m.SendMessage("The blizzard's cold winds lash at you!");
                        AOS.Damage(m, this, Utility.RandomMinMax(5, 15), 0, 100, 0, 0, 0);
                    }
                }

                m_NextBlizzardCall = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 60));
            }
        }

        private void FrostNova()
        {
            if (Combatant != null)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "The Frost Serpent releases a Frost Nova!");
                FixedEffect(0x36D4, 10, 16); // Frost Nova effect

                foreach (Mobile m in GetMobilesInRange(5))
                {
                    if (m != this && m.Alive)
                    {
                        m.SendMessage("You are struck by the Frost Nova!");
                        AOS.Damage(m, this, Utility.RandomMinMax(20, 30), 0, 100, 0, 0, 0);
                        m.Freeze(TimeSpan.FromSeconds(2)); // Slows down the target
                    }
                }

                m_NextFrostNova = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 45));
            }
        }

        public override void OnGotMeleeAttack(Mobile attacker)
        {
            base.OnGotMeleeAttack(attacker);

            if (attacker != null && !attacker.IsDeadBondedPet)
            {
                // Reflects damage with a chance to freeze
                if (Utility.RandomDouble() < 0.3) // 30% chance
                {
                    PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "The Frost Serpent's scales chill your attacks!");
                    attacker.SendMessage("The Frost Serpent's icy scales reflect some of your attack's damage!");
                    int damage = (int)(attacker.HitsMax * 0.1); // Reflect 10% of the attack damage
                    AOS.Damage(attacker, this, damage, 0, 100, 0, 0, 0);

                    if (Utility.RandomDouble() < 0.5) // 50% chance to freeze attacker
                    {
                        attacker.Freeze(TimeSpan.FromSeconds(2));
                        attacker.SendMessage("You are frozen by the Frost Serpent's icy scales!");
                    }
                }
            }
        }

        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            base.OnMovement(m, oldLocation);

            if (m != this && m.InRange(this, 3))
            {
                // Frostbite Aura effect
                m.SendMessage("You feel the chill of the Frost Serpent's aura!");
                m.SendMessage("You are slowed by the Frost Serpent's aura!");
                m.Dex -= 5; // Reduces attack speed
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
