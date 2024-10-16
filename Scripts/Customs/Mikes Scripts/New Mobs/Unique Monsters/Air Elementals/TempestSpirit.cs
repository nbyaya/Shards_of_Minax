using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a tempest spirit corpse")]
    public class TempestSpirit : BaseCreature
    {
        private DateTime m_NextLightningStrike;
        private DateTime m_NextStormCloud;
        private DateTime m_NextWindShield;
        private bool m_AbilitiesActivated; // New flag to track initial ability activation

        [Constructable]
        public TempestSpirit()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a tempest spirit";
            Body = 13;
            Hue = 1095; // Dark stormy hue
            BaseSoundID = 655;

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

            m_NextLightningStrike = DateTime.UtcNow;
            m_NextStormCloud = DateTime.UtcNow;
            m_NextWindShield = DateTime.UtcNow;
            m_AbilitiesActivated = false; // Initialize flag
        }

        public TempestSpirit(Serial serial)
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
            get
            {
                return 120.0;
            }
        }

        public override double DispelFocus
        {
            get
            {
                return 50.0;
            }
        }

        public override bool BleedImmune
        {
            get
            {
                return true;
            }
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesActivated)
                {
                    Random rand = new Random();
                    m_NextLightningStrike = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextStormCloud = DateTime.UtcNow + TimeSpan.FromMinutes(rand.Next(1, 2));
                    m_NextWindShield = DateTime.UtcNow + TimeSpan.FromMinutes(rand.Next(1, 3));

                    m_AbilitiesActivated = true; // Set the flag to prevent re-initializing the times
                }

                if (DateTime.UtcNow >= m_NextLightningStrike)
                {
                    CastLightningStrike();
                }

                if (DateTime.UtcNow >= m_NextStormCloud)
                {
                    SummonStormCloud();
                }

                if (DateTime.UtcNow >= m_NextWindShield)
                {
                    ActivateWindShield();
                }
            }
        }

        private void CastLightningStrike()
        {
            if (Combatant != null)
            {
                Mobile target = Combatant as Mobile;
                if (target != null && target.Alive)
                {
                    int damage = Utility.RandomMinMax(30, 50);
                    target.Damage(damage, this);
                    Effects.SendLocationParticles(EffectItem.Create(target.Location, target.Map, EffectItem.DefaultDuration), 0x1F4, 10, 10, 0);
                    target.SendMessage("You are struck by a powerful lightning bolt!");

                    m_NextLightningStrike = DateTime.UtcNow + TimeSpan.FromSeconds(30);
                }
            }
        }

        private void SummonStormCloud()
        {
            Point3D loc = GetSpawnPosition(5);

            if (loc != Point3D.Zero)
            {
                StormCloud cloud = new StormCloud();
                cloud.MoveToWorld(loc, Map);

                m_NextStormCloud = DateTime.UtcNow + TimeSpan.FromMinutes(2);
            }
        }

        private void ActivateWindShield()
        {
            this.VirtualArmor += 30;
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* A swirling wind shield envelops the Tempest Spirit!");

            Timer.DelayCall(TimeSpan.FromSeconds(30), new TimerCallback(delegate()
            {
                this.VirtualArmor -= 30;
            }));

            m_NextWindShield = DateTime.UtcNow + TimeSpan.FromMinutes(1);
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
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_AbilitiesActivated = false; // Reset flag on deserialization
        }
    }

    public class StormCloud : BaseCreature
    {
        private DateTime m_NextLightningStrike;

        public StormCloud()
            : base(AIType.AI_Mage, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "a storm cloud";
            Body = 13; // Airy, cloud-like body
            Hue = 1152; // Dark stormy hue

            SetStr(1);
            SetDex(1);
            SetInt(1);

            SetHits(1);
            SetDamage(0);

            SetResistance(ResistanceType.Physical, 100);
            SetResistance(ResistanceType.Fire, 100);
            SetResistance(ResistanceType.Cold, 100);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 100);

            VirtualArmor = 100;

            m_NextLightningStrike = DateTime.UtcNow;
        }

        public StormCloud(Serial serial)
            : base(serial)
        {
        }

        public override void OnThink()
        {
            base.OnThink();

            if (DateTime.UtcNow >= m_NextLightningStrike)
            {
                StrikeNearbyEnemies();
                m_NextLightningStrike = DateTime.UtcNow + TimeSpan.FromSeconds(10);
            }
        }

        private void StrikeNearbyEnemies()
        {
            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive)
                {
                    int damage = Utility.RandomMinMax(10, 20);
                    m.Damage(damage, this);
                    Effects.SendLocationParticles(EffectItem.Create(m.Location, m.Map, EffectItem.DefaultDuration), 0x1F4, 10, 10, 0);
                    m.SendMessage("Lightning strikes you from the storm cloud!");
                }
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
        }
    }
}
