using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a phantom automaton corpse")]
    public class PhantomAutomaton : BaseCreature
    {
        private DateTime m_NextPhantomStrike;
        private DateTime m_NextPhaseShift;
        private DateTime m_PhaseShiftEnd;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public PhantomAutomaton()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Phantom Automaton";
            Body = 0x2F5; // ExodusMinion body
            BaseSoundID = 0x2F8;
            Hue = 2500; // Ghostly blue-white hue

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

        public PhantomAutomaton(Serial serial)
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
        public override bool BardImmune { get { return true; } }
        public override Poison PoisonImmune { get { return Poison.Lethal; } }
        public override bool AlwaysMurderer { get { return true; } }



        public override int GetIdleSound()
        {
            return 0x2CC;
        }

        public override int GetAttackSound()
        {
            return 0x2C8;
        }

        public override int GetDeathSound()
        {
            return 0x2C9;
        }

        public override int GetHurtSound()
        {
            return 0x2D1;
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    Random rand = new Random();
                    m_NextPhantomStrike = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextPhaseShift = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextPhantomStrike)
                {
                    DoPhantomStrike();
                }

                if (DateTime.UtcNow >= m_NextPhaseShift && DateTime.UtcNow >= m_PhaseShiftEnd)
                {
                    DoPhaseShift();
                }
            }

            if (DateTime.UtcNow >= m_PhaseShiftEnd && m_PhaseShiftEnd != DateTime.MinValue)
            {
                EndPhaseShift();
            }
        }

        private void DoPhantomStrike()
        {
            if (Combatant is Mobile target && target.Alive)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Phantom Strike *");
                PlaySound(0x510);

                Point3D oldLocation = Location;
                Point3D newLocation = target.Location;

                // Move through the target
                Map.Tiles.GetStaticTiles(X, Y, true);
                MoveToWorld(newLocation, Map);

                // Create ghostly trail effect
                Effects.SendMovingParticles(new Entity(Serial.Zero, oldLocation, Map),
                    new Entity(Serial.Zero, newLocation, Map), 0x375A, 7, 0, false, false, 0, 0, 9502, 1, 0, EffectLayer.Head, 0x100);

                // Deal damage
                int damage = Utility.RandomMinMax(30, 40);
                AOS.Damage(target, this, damage, 50, 0, 0, 0, 50);
            }

            m_NextPhantomStrike = DateTime.UtcNow + TimeSpan.FromSeconds(25);
        }

        private void DoPhaseShift()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Phase Shift *");
            PlaySound(0x217);

            // Fade out effect
            FixedParticles(0x3728, 1, 13, 5042, EffectLayer.Waist);

            m_PhaseShiftEnd = DateTime.UtcNow + TimeSpan.FromSeconds(5);
            m_NextPhaseShift = DateTime.UtcNow + TimeSpan.FromSeconds(40);
        }

        private void EndPhaseShift()
        {
            // Fade in effect
            FixedParticles(0x3728, 1, 13, 5042, EffectLayer.Waist);

            m_PhaseShiftEnd = DateTime.MinValue;
        }

        public override void AlterMeleeDamageFrom(Mobile from, ref int damage)
        {
            if (DateTime.UtcNow < m_PhaseShiftEnd)
            {
                damage = 0;
                from.SendLocalizedMessage(1042075); // Your attack passed through the creature.
            }
        }

        public override void AlterSpellDamageFrom(Mobile from, ref int damage)
        {
            if (DateTime.UtcNow < m_PhaseShiftEnd)
            {
                damage = 0;
                from.SendLocalizedMessage(1042075); // Your attack passed through the creature.
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_AbilitiesInitialized = false; // Reset the initialization flag
        }
    }
}
