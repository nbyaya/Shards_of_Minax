using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a shadow dragon corpse")]
    public class ShadowDragon : BaseCreature
    {
        private DateTime m_NextShadowBreath;
        private DateTime m_NextShadowStep;
        private DateTime m_NextDarkVeil;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public ShadowDragon()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a shadow dragon";
            Body = 59; // Using dragon body
            Hue = 1478; // Dark hue for shadow effect
            BaseSoundID = 362;

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

        public ShadowDragon(Serial serial)
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
                    m_NextShadowBreath = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(5, 30));
                    m_NextShadowStep = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(10, 40));
                    m_NextDarkVeil = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(20, 60));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextShadowBreath)
                {
                    ShadowBreath();
                }

                if (DateTime.UtcNow >= m_NextShadowStep)
                {
                    ShadowStep();
                }

                if (DateTime.UtcNow >= m_NextDarkVeil)
                {
                    DarkVeil();
                }
            }
        }

        private void ShadowBreath()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Shadow Dragon exhales a dark breath *");
            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive)
                {
                    int damage = Utility.RandomMinMax(20, 30);
                    m.Damage(damage, this);
                    m.SendMessage("You are engulfed in shadow and blinded!");
                    m.SendMessage("You are blinded by the shadow breath!");
                    m.SendLocalizedMessage(1060044); // Blinded
                }
            }
            m_NextShadowBreath = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        private void ShadowStep()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Shadow Dragon vanishes into the shadows *");
            int range = 5;
            int x = X + Utility.RandomMinMax(-range, range);
            int y = Y + Utility.RandomMinMax(-range, range);
            int z = Map.GetAverageZ(x, y);

            Point3D newLocation = new Point3D(x, y, z);
            if (Map.CanSpawnMobile(newLocation))
            {
                MoveToWorld(newLocation, Map);
            }
            m_NextShadowStep = DateTime.UtcNow + TimeSpan.FromSeconds(20);
        }

        private void DarkVeil()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Shadow Dragon is enveloped in darkness *");
            this.Hidden = true;
            Timer.DelayCall(TimeSpan.FromSeconds(10), () => this.Hidden = false);
            m_NextDarkVeil = DateTime.UtcNow + TimeSpan.FromMinutes(2);
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

            m_AbilitiesInitialized = false; // Reset initialization flag
        }
    }
}
