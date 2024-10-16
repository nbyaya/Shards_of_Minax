using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a ragdoll guardian corpse")]
    public class RagdollGuardian : BaseCreature
    {
        private DateTime m_NextTangle;
        private DateTime m_NextGuardiansShield;
        private DateTime m_NextRejuvenate;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public RagdollGuardian()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a ragdoll guardian";
            Body = 0xC9; // Cat body
            Hue = 1298; // Unique hue
			BaseSoundID = 0x69;

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
            SetResistance(ResistanceType.Poison, 100);
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

        public RagdollGuardian(Serial serial)
            : base(serial)
        {
        }

		public override bool ReacquireOnMovement
		{
			get
			{
				return !Controlled;
			}
		}
		public override bool AutoDispel
		{
			get
			{
				return !Controlled;
			}
		}

		public override int TreasureMapLevel
		{
			get
			{
				return 5;
			}
		}
		
		public override bool CanAngerOnTame
		{
			get
			{
				return true;
			}
		}

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
                    m_NextTangle = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextGuardiansShield = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextRejuvenate = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextTangle)
                {
                    UseTangle();
                }

                if (DateTime.UtcNow >= m_NextGuardiansShield)
                {
                    UseGuardiansShield();
                }

                if (DateTime.UtcNow >= m_NextRejuvenate)
                {
                    UseRejuvenate();
                }
            }
        }

        private void UseTangle()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Ragdoll Guardian summons protective vines to entrap its foes! *");
            FixedEffect(0x373A, 10, 16);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && m.InRange(this, 3))
                {
                    // Immobilize the target
                    m.Freeze(TimeSpan.FromSeconds(5));
                }
            }

            m_NextTangle = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Reset cooldown for next use
        }

        private void UseGuardiansShield()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Ragdoll Guardian summons a protective shield! *");
            FixedEffect(0x373A, 10, 16);

            // Apply shield effect, increase virtual armor
            VirtualArmor += 20;
            Timer.DelayCall(TimeSpan.FromSeconds(15), () => VirtualArmor -= 20); // Remove shield after 15 seconds

            m_NextGuardiansShield = DateTime.UtcNow + TimeSpan.FromSeconds(40); // Reset cooldown for next use
        }

        private void UseRejuvenate()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Ragdoll Guardian rejuvenates itself and its allies! *");
            FixedEffect(0x373A, 10, 16);

            this.Hits = Math.Min(this.Hits + 50, this.HitsMax);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive)
                {
                    m.Hits = Math.Min(m.Hits + 20, m.HitsMax);
                }
            }

            m_NextRejuvenate = DateTime.UtcNow + TimeSpan.FromSeconds(50); // Reset cooldown for next use
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
