using System;
using Server.Items;
using Server.Network;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a spectral automaton corpse")]
    public class SpectralAutomaton : BaseCreature
    {
        private DateTime m_NextPhaseShift;
        private DateTime m_NextHauntingWail;
        private DateTime m_NextEtherealBlast;
        private DateTime m_NextPhaseStrike;
        private bool m_AbilitiesInitialized;
        private bool m_Phased;

        [Constructable]
        public SpectralAutomaton()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "spectral automaton";
            Body = 0x2F4; // Exodus Overseer body
            Hue = 2284; // Ghostly hue

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
            m_Phased = false;
        }

        public SpectralAutomaton(Serial serial)
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
                    Random rand = new Random();
                    m_NextPhaseShift = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextHauntingWail = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_NextEtherealBlast = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_NextPhaseStrike = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 45));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextPhaseShift)
                {
                    PhaseShift();
                }

                if (DateTime.UtcNow >= m_NextHauntingWail)
                {
                    HauntingWail();
                }

                if (DateTime.UtcNow >= m_NextEtherealBlast)
                {
                    EtherealBlast();
                }

                if (DateTime.UtcNow >= m_NextPhaseStrike)
                {
                    PhaseStrike();
                }
            }
        }

        private void PhaseShift()
        {
            if (!m_Phased)
            {
                m_Phased = true;
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The automaton phases out of reality! *");
                FixedParticles(0x3735, 1, 30, 0x251F, EffectLayer.Waist);
                this.Hidden = true; // Make the automaton invisible
                Timer.DelayCall(TimeSpan.FromSeconds(5), () =>
                {
                    this.Hidden = false; // Reappear after 5 seconds
                    m_Phased = false;
                });
                m_NextPhaseShift = DateTime.UtcNow + TimeSpan.FromSeconds(60); // Randomized cooldown
            }
        }

        private void HauntingWail()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The automaton emits a chilling wail! *");
            FixedParticles(0x3735, 1, 30, 0x251F, EffectLayer.Head);
            PlaySound(0x1F5); // Wail sound effect

            foreach (Mobile m in GetMobilesInRange(10))
            {
                if (m != this && m.Alive)
                {
                    m.SendMessage("The spectral automaton's wail chills you to the bone!");
                    m.Freeze(TimeSpan.FromSeconds(3)); // Reduce the target’s combat effectiveness
                    m.Damage(10, this); // Inflict additional damage
                }
            }
            m_NextHauntingWail = DateTime.UtcNow + TimeSpan.FromSeconds(120); // Randomized cooldown
        }

        private void EtherealBlast()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The automaton releases an ethereal blast! *");
            FixedParticles(0x3735, 1, 30, 0x2524, EffectLayer.Head);
            PlaySound(0x1F7); // Blast sound effect

            foreach (Mobile m in GetMobilesInRange(7))
            {
                if (m != this && m.Alive)
                {
                    m.SendMessage("You are hit by an ethereal blast!");
                    m.Damage(20, this); // Inflict damage to all nearby enemies
                }
            }
            m_NextEtherealBlast = DateTime.UtcNow + TimeSpan.FromSeconds(90); // Randomized cooldown
        }

        private void PhaseStrike()
        {
            if (Combatant != null)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The automaton phases and strikes! *");
                FixedParticles(0x3735, 1, 30, 0x251F, EffectLayer.Waist);
                PlaySound(0x1F6); // Strike sound effect

                // Phase strike targets the current combatant
                Mobile target = Combatant as Mobile;
                if (target != null && target.Alive)
                {
                    target.SendMessage("You are struck by the automaton's phase strike!");
                    target.Damage(30, this); // Inflict significant damage
                }
                m_NextPhaseStrike = DateTime.UtcNow + TimeSpan.FromSeconds(90); // Randomized cooldown
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

            m_AbilitiesInitialized = false; // Reset initialization flag on deserialize
        }
    }
}
