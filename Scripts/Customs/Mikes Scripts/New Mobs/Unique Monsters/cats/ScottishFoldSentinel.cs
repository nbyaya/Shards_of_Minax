using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a Scottish Fold Sentinel corpse")]
    public class ScottishFoldSentinel : BaseCreature
    {
        private DateTime m_NextStoneBarrier;
        private DateTime m_NextGuardianCall;
        private DateTime m_NextProtectiveAura;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized
        private bool m_IsAuraActive;

        [Constructable]
        public ScottishFoldSentinel()
            : base(AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            Name = "a Scottish Fold Sentinel";
            Body = 0xC9; // Cat body
            Hue = 1297; // Unique hue
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
            m_IsAuraActive = false;
        }

        public ScottishFoldSentinel(Serial serial)
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
                    m_NextStoneBarrier = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextGuardianCall = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 45));
                    m_NextProtectiveAura = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextStoneBarrier)
                {
                    UseStoneBarrier();
                }

                if (DateTime.UtcNow >= m_NextGuardianCall)
                {
                    UseGuardianCall();
                }

                if (DateTime.UtcNow >= m_NextProtectiveAura)
                {
                    UseProtectiveAura();
                }
            }
        }

        private void UseStoneBarrier()
        {
            if (Combatant == null) return;

            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Scottish Fold Sentinel's protective barrier shimmers into place! *");
            FixedEffect(0x376A, 10, 16);

            // Stone Barrier effect
            // Note: Implement actual barrier logic (e.g., damage reduction) here

            m_NextStoneBarrier = DateTime.UtcNow + TimeSpan.FromSeconds(45); // Cooldown for StoneBarrier
        }

        private void UseGuardianCall()
        {
            if (Combatant == null) return;

            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Scottish Fold Sentinel calls upon its allies! *");
            FixedEffect(0x37C4, 10, 36);

            // Guardian's Call effect
            // Note: Implement ally summoning logic here

            m_NextGuardianCall = DateTime.UtcNow + TimeSpan.FromSeconds(60); // Cooldown for GuardianCall
        }

        private void UseProtectiveAura()
        {
            if (Combatant == null) return;

            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Scottish Fold Sentinel's protective aura shields its allies! *");
            FixedEffect(0x37C4, 10, 36);

            // Protective Aura effect
            if (!m_IsAuraActive)
            {
                m_IsAuraActive = true;
                Timer.DelayCall(TimeSpan.FromSeconds(15), () => m_IsAuraActive = false);
            }

            m_NextProtectiveAura = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown for ProtectiveAura
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
