using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a frostbound behemoth corpse")]
    public class FrostboundBehemoth : BaseCreature
    {
        private DateTime m_NextFrostBreath;
        private DateTime m_NextGlacialShield;
        private DateTime m_NextIcyGrasp;

        private bool m_AbilitiesInitialized; // Flag to check if abilities are initialized

        [Constructable]
        public FrostboundBehemoth()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a frostbound behemoth";
            Body = 9; // Daemon body
            Hue = 1469; // Unique hue for ice theme
            BaseSoundID = 357;

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

        public FrostboundBehemoth(Serial serial)
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

        public override double DispelDifficulty
        {
            get { return 150.0; }
        }

        public override double DispelFocus
        {
            get { return 50.0; }
        }

        public override bool CanRummageCorpses
        {
            get { return true; }
        }

        public override bool CanFly
        {
            get { return false; }
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    Random rand = new Random();
                    m_NextFrostBreath = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextGlacialShield = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_NextIcyGrasp = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 90));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextFrostBreath)
                {
                    UseFrostBreath();
                }

                if (DateTime.UtcNow >= m_NextGlacialShield)
                {
                    UseGlacialShield();
                }

                if (DateTime.UtcNow >= m_NextIcyGrasp)
                {
                    UseIcyGrasp();
                }
            }
        }

        private void UseFrostBreath()
        {
            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && InLOS(m))
                {
                    m.SendMessage("You are hit by a blast of frost!");
                    m.Damage(Utility.RandomMinMax(20, 30), this); // Direct damage
                    m.Freeze(TimeSpan.FromSeconds(3));
                }
            }

            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Frostbound Behemoth breathes a chilling frost! *");

            m_NextFrostBreath = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Resetting with fixed cooldown
        }

        private void UseGlacialShield()
        {
            // Increase virtual armor temporarily
            this.VirtualArmor += 50; 
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Frostbound Behemoth creates a shimmering ice shield! *");

            Timer.DelayCall(TimeSpan.FromMinutes(1), () => 
            {
                // Revert virtual armor back to normal after the shield expires
                this.VirtualArmor -= 50; 
            });

            m_NextGlacialShield = DateTime.UtcNow + TimeSpan.FromMinutes(1); // Resetting with fixed cooldown
        }

        private void UseIcyGrasp()
        {
            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m.Alive && InLOS(m))
                {
                    m.SendMessage("You feel the icy grip of the Frostbound Behemoth!");
                    m.Freeze(TimeSpan.FromSeconds(5));
                }
            }

            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Frostbound Behemoth's icy grasp freezes its foes! *");

            m_NextIcyGrasp = DateTime.UtcNow + TimeSpan.FromMinutes(1); // Resetting with fixed cooldown
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_AbilitiesInitialized = false; // Reset flag on deserialize
        }
    }
}
