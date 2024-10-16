using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a void stalker corpse")]
    public class VoidStalker : BaseCreature
    {
        private DateTime m_NextShadowStep;
        private DateTime m_NextShadowStrike;
        private DateTime m_NextEclipse;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public VoidStalker()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a void stalker";
            Body = 9; // Daemon body
            Hue = 1462; // Unique dark hue
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

        public VoidStalker(Serial serial)
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
            get { return 125.0; }
        }

        public override double DispelFocus
        {
            get { return 45.0; }
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
                    m_NextShadowStep = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextShadowStrike = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextEclipse = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextShadowStep)
                {
                    ShadowStep();
                }

                if (DateTime.UtcNow >= m_NextShadowStrike)
                {
                    ShadowStrike();
                }

                if (DateTime.UtcNow >= m_NextEclipse)
                {
                    Eclipse();
                }
            }
        }

        private void ShadowStep()
        {
            if (Combatant != null)
            {
                Point3D newLocation = GetSpawnPosition(5);

                if (newLocation != Point3D.Zero)
                {
                    this.Location = newLocation;
                    this.Map = Map;
                    PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Void Stalker fades into the shadows and reappears elsewhere! *");
                    Effects.SendLocationParticles(EffectItem.Create(newLocation, Map, EffectItem.DefaultDuration), 0x3709, 10, 30, 2115);

                    m_NextShadowStep = DateTime.UtcNow + TimeSpan.FromSeconds(30 + Utility.RandomMinMax(0, 15)); // Random cooldown between 30 and 45 seconds
                }
            }
        }

        private void ShadowStrike()
        {
            if (Combatant != null)
            {
                Mobile target = Combatant as Mobile;
                if (target != null && target.Alive)
                {
                    target.Damage(Utility.RandomMinMax(20, 30), this);
                    PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Void Stalker strikes from the darkness with a powerful blow! *");
                    m_NextShadowStrike = DateTime.UtcNow + TimeSpan.FromSeconds(20 + Utility.RandomMinMax(0, 10)); // Random cooldown between 20 and 30 seconds
                }
            }
        }

        private void Eclipse()
        {
            foreach (Mobile m in GetMobilesInRange(10))
            {
                if (m != this && m.Player)
                {
                    m.SendMessage("The shadows around you grow darker, making it harder to see and fight!");
                    m.SendMessage("You are affected by the Void Stalker's Eclipse!");
                    // Adjust visibility or combat effectiveness here if desired
                }
            }

            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Void Stalker conjures an eclipse, shrouding the area in darkness! *");
            m_NextEclipse = DateTime.UtcNow + TimeSpan.FromMinutes(2 + Utility.RandomMinMax(0, 1)); // Random cooldown between 2 and 3 minutes
        }

        private Point3D GetSpawnPosition(int range)
        {
            for (int i = 0; i < 10; i++)
            {
                int x = X + Utility.RandomMinMax(-range, range);
                int y = Y + Utility.RandomMinMax(-range, range);
                int z = Map.GetAverageZ(x, y);

                Point3D p = new Point3D(x, y, z);

                if (Map.CanSpawnMobile(p))
                    return p;
            }

            return Point3D.Zero;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            var version = reader.ReadInt();
            m_AbilitiesInitialized = false; // Reset flag on deserialize
        }
    }
}
