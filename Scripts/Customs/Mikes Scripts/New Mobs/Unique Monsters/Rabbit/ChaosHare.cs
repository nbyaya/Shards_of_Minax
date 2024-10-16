using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a chaos hare corpse")]
    public class ChaosHare : BaseCreature
    {
        private DateTime m_NextChaoticSurge;
        private DateTime m_NextFrenziedHop;
        private DateTime m_NextDistortionAura;
        private DateTime m_NextColorShift;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public ChaosHare()
            : base(AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            Name = "a chaos hare";
            Body = 205; // Rabbit body
            Hue = Utility.RandomDyedHue(); // Wild color changes

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

        public ChaosHare(Serial serial)
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
        public override int GetAttackSound() 
        { 
            return 0xC9; 
        }

        public override int GetHurtSound() 
        { 
            return 0xCA; 
        }

        public override int GetDeathSound() 
        { 
            return 0xCB; 
        }
        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    Random rand = new Random();
                    m_NextChaoticSurge = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 15));
                    m_NextFrenziedHop = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextDistortionAura = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 25));
                    m_NextColorShift = DateTime.UtcNow + TimeSpan.FromSeconds(5);
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextChaoticSurge)
                {
                    ChaoticSurge();
                }

                if (DateTime.UtcNow >= m_NextFrenziedHop)
                {
                    FrenziedHop();
                }

                if (DateTime.UtcNow >= m_NextDistortionAura)
                {
                    DistortionAura();
                }

                if (DateTime.UtcNow >= m_NextColorShift)
                {
                    ShiftColor();
                }
            }
        }

        private void ChaoticSurge()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Chaos Hare’s surge disrupts everything around it!*");
            PlaySound(0x1F2); // Chaotic energy sound

            // Apply random effects
            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive)
                {
                    int effect = Utility.Random(4);

                    switch (effect)
                    {
                        case 0:
                            AOS.Damage(m, this, Utility.RandomMinMax(15, 25), 0, 0, 100, 0, 0); // Random damage
                            break;
                        case 1:
                            m.SendMessage("You are hit by a burst of chaotic energy!");
                            m.Freeze(TimeSpan.FromSeconds(2)); // Freeze for 2 seconds
                            break;
                        case 2:
                            m.SendMessage("Your senses are overwhelmed by the chaos!");
                            m.SendLocalizedMessage(1042025); // Randomly attacks
                            break;
                        case 3:
                            m.SendMessage("Your vision blurs and you feel disoriented!");
                            m.SendMessage("You are stunned by the chaotic surge!");
                            break;
                    }
                }
            }

            m_NextChaoticSurge = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown for ChaoticSurge
        }

        private void FrenziedHop()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Chaos Hare hops erratically, causing chaos!*");
            PlaySound(0x1F2); // Hopping sound

            foreach (Mobile m in GetMobilesInRange(2))
            {
                if (m != this && m.Alive)
                {
                    int damage = Utility.RandomMinMax(15, 25);
                    AOS.Damage(m, this, damage, 0, 100, 0, 0, 0);
                    m.SendMessage("You are hit by the chaotic hop of the hare!");
                }
            }

            m_NextFrenziedHop = DateTime.UtcNow + TimeSpan.FromSeconds(20); // Cooldown for FrenziedHop
        }

        private void DistortionAura()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Chaos Hare’s aura distorts space around it!*");
            PlaySound(0x1F2); // Aura sound

            // Apply distortion effect
            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive)
                {
                    m.SendMessage("Your attacks are distorted by the chaos around you!");
                    m.SendMessage("The chaos around you makes it hard to focus!");

                    // Randomly apply effects
                    int effect = Utility.Random(4);

                    switch (effect)
                    {
                        case 0:
                            m.SendLocalizedMessage(1042025); // Random attack
                            break;
                        case 1:
                            m.SendMessage("You feel your aim becoming more erratic!");
                            // Decrease hit chance
                            m.Damage(0); // No actual damage, just message
                            break;
                        case 2:
                            m.SendMessage("You feel a surge of chaotic energy affecting you!");
                            break;
                        case 3:
                            m.SendMessage("The chaos around you makes your movements unpredictable!");
                            break;
                    }
                }
            }

            m_NextDistortionAura = DateTime.UtcNow + TimeSpan.FromSeconds(40); // Cooldown for DistortionAura
        }

        private void ShiftColor()
        {
            // Change hue to a new random color to reflect the chaotic nature
            Hue = Utility.RandomDyedHue();
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Chaos Hare’s color shifts wildly!*");
            m_NextColorShift = DateTime.UtcNow + TimeSpan.FromSeconds(10); // Cooldown for color shift
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
