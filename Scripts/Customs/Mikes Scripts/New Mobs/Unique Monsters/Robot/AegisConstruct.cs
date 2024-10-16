using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("an aegis construct corpse")]
    public class AegisConstruct : BaseCreature
    {
        private DateTime m_NextDefensiveMatrix;
        private DateTime m_NextCriticalStrike;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public AegisConstruct()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Aegis Construct";
            Body = 0x2F5; // ExodusMinion body
            Hue = 1152; // Unique hue

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

        public AegisConstruct(Serial serial)
            : base(serial)
        {
        }
        public override int GetIdleSound()
        {
            return 0x218;
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
            return 0x232;
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
                    m_NextDefensiveMatrix = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextCriticalStrike = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextDefensiveMatrix)
                {
                    DefensiveMatrix();
                }

                if (DateTime.UtcNow >= m_NextCriticalStrike)
                {
                    CriticalStrike();
                }
            }
        }

        private void DefensiveMatrix()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Activating Defensive Matrix *");
            FixedParticles(0x3735, 1, 30, 0x251F, EffectLayer.Waist);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m is BaseCreature creature && creature != this)
                {
                    creature.FixedParticles(0x376A, 1, 30, 0x2522, EffectLayer.Waist);
                }
            }

            m_NextDefensiveMatrix = DateTime.UtcNow + TimeSpan.FromSeconds(40); // Cooldown for DefensiveMatrix
        }

        private void CriticalStrike()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Critical Strike! *");
            FixedParticles(0x37C4, 1, 36, 0x2530, EffectLayer.Waist);

            if (Combatant != null && Combatant.Alive)
            {
                Mobile target = Combatant as Mobile; // Attempt to cast Combatant to Mobile
                if (target != null)
                {
                    target.Frozen = true;
                    Timer.DelayCall(TimeSpan.FromSeconds(1), delegate { target.Frozen = false; });
                }

                Combatant.Damage(30, this);
            }

            m_NextCriticalStrike = DateTime.UtcNow + TimeSpan.FromSeconds(25); // Cooldown for CriticalStrike
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
