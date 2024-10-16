using System;
using Server;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a Maine Coon Titan corpse")]
    public class MaineCoonTitan : BaseCreature
    {
        private DateTime m_NextRoar;
        private DateTime m_NextFeralStrength;
        private DateTime m_NextRegenerativeProwess;
        private bool m_FeralStrengthActive;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public MaineCoonTitan()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a Maine Coon Titan";
            Body = 0xC9; // Using cat body
            Hue = 1300; // Unique hue for Maine Coon Titan
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

        public MaineCoonTitan(Serial serial)
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
                    Random rand = new Random();
                    m_NextRoar = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextFeralStrength = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_NextRegenerativeProwess = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextRoar)
                {
                    RoarOfTheWild();
                }

                if (DateTime.UtcNow >= m_NextFeralStrength && !m_FeralStrengthActive)
                {
                    ActivateFeralStrength();
                }

                if (DateTime.UtcNow >= m_NextRegenerativeProwess)
                {
                    RegenerativeProwess();
                }
            }
        }

        private void RoarOfTheWild()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Maine Coon Titan lets out a thunderous roar!*");
            FixedEffect(0x376A, 10, 16);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Player)
                {
                    m.Damage(20, this);
                }
            }

            m_NextRoar = DateTime.UtcNow + TimeSpan.FromSeconds(25);
        }

        private void ActivateFeralStrength()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Maine Coon Titan's strength surges with primal energy!*");
            PlaySound(0x165);
            FixedEffect(0x37C4, 10, 36);

            SetStr(Str + 50);
            SetDamage(DamageMin + 10, DamageMax + 10);

            m_FeralStrengthActive = true;
            Timer.DelayCall(TimeSpan.FromSeconds(15), new TimerCallback(DeactivateFeralStrength));

            m_NextFeralStrength = DateTime.UtcNow + TimeSpan.FromSeconds(40);
        }

        private void DeactivateFeralStrength()
        {
            SetStr(Str - 50);
            SetDamage(DamageMin - 10, DamageMax - 10);
            m_FeralStrengthActive = false;
        }

        private void RegenerativeProwess()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Maine Coon Titan's wounds begin to close as it heals!*");
            Timer.DelayCall(TimeSpan.FromSeconds(10), new TimerCallback(() => 
            {
                Hits = Math.Min(Hits + 50, HitsMax);
            }));

            m_NextRegenerativeProwess = DateTime.UtcNow + TimeSpan.FromSeconds(45);
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
