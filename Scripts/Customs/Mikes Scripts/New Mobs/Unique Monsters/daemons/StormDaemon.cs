using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a storm daemon corpse")]
    public class StormDaemon : BaseCreature
    {
        private DateTime m_NextLightningBolt;
        private DateTime m_NextStormSurge;
        private DateTime m_NextElectrifiedArmor;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public StormDaemon()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a storm daemon";
            Body = 9; // Daemon body
            Hue = 1465; // Unique hue (stormy blue)
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

        public StormDaemon(Serial serial)
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
            get { return 60.0; }
        }

        public override bool CanFly
        {
            get { return true; }
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    Random rand = new Random();
                    m_NextLightningBolt = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextStormSurge = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextElectrifiedArmor = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 25));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextLightningBolt)
                {
                    CastLightningBolt();
                }

                if (DateTime.UtcNow >= m_NextStormSurge)
                {
                    CastStormSurge();
                }

                if (DateTime.UtcNow >= m_NextElectrifiedArmor)
                {
                    ApplyElectrifiedArmor();
                }
            }
        }

        private void CastLightningBolt()
        {
            if (Combatant != null)
            {
                Mobile target = Combatant as Mobile;
                if (target != null && target.Alive)
                {
                    int damage = Utility.RandomMinMax(30, 50);
                    target.Damage(damage, this);
                    PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* A bolt of lightning strikes you! *");
                    Effects.SendTargetEffect(target, 0x3709, 10); // Lightning bolt effect
                }
            }

            m_NextLightningBolt = DateTime.UtcNow + TimeSpan.FromSeconds(15);
        }

        private void CastStormSurge()
        {
            foreach (Mobile m in GetMobilesInRange(8))
            {
                if (m != this && m.Player)
                {
                    int damage = Utility.RandomMinMax(20, 40);
                    m.Damage(damage, this);
                    PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* A storm surge damages you! *");
                    Effects.SendLocationEffect(m.Location, m.Map, 0x3709, 10); // Storm surge effect
                }
            }

            m_NextStormSurge = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        private void ApplyElectrifiedArmor()
        {
            foreach (Mobile m in GetMobilesInRange(1))
            {
                if (m != this && m.Player)
                {
                    int damage = Utility.RandomMinMax(5, 10);
                    m.Damage(damage, this);
                    PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* You are shocked by the storm daemon's electrified armor! *");
                    Effects.SendTargetEffect(m, 0x3709, 10); // Electrified armor effect
                }
            }

            m_NextElectrifiedArmor = DateTime.UtcNow + TimeSpan.FromSeconds(20);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_AbilitiesInitialized = false; // Reset flag on deserialize
        }
    }
}
