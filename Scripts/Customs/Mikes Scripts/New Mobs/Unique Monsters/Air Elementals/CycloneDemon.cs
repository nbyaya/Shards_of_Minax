using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("a cyclone demon corpse")]
    public class CycloneDemon : BaseCreature
    {
        private DateTime m_NextTornadoSpin;
        private DateTime m_NextCycloneCharge;
        private DateTime m_NextWindSlash;
        private bool m_AbilitiesInitialized; // New flag to track initial ability activation

        [Constructable]
        public CycloneDemon()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a cyclone demon";
            Body = 13; // Use a tornado-like body or custom model
            Hue = 1078; // Windy, stormy color
            BaseSoundID = 0x2D1; // Wind sound

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

        public CycloneDemon(Serial serial)
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
                    // Randomly set the initial activation times
                    Random rand = new Random();
                    m_NextTornadoSpin = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextCycloneCharge = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextWindSlash = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));

                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing the times
                }

                if (DateTime.UtcNow >= m_NextTornadoSpin)
                {
                    TornadoSpin();
                }

                if (DateTime.UtcNow >= m_NextCycloneCharge)
                {
                    CycloneCharge();
                }

                if (DateTime.UtcNow >= m_NextWindSlash)
                {
                    WindSlash();
                }
            }
        }

        private void TornadoSpin()
        {
            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Player)
                {
                    m.SendMessage("You are caught in a fierce tornado!");
                    m.Damage(10, this);
                    m.Freeze(TimeSpan.FromSeconds(5)); // Disorient
                }
            }

            m_NextTornadoSpin = DateTime.UtcNow + TimeSpan.FromMinutes(2);
        }

        private void CycloneCharge()
        {
            if (Combatant != null)
            {
                Point3D destination = Combatant.Location;
                this.Location = destination;
                this.Map = Map;

                Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration), 0x3709, 10, 30, 1153);
                if (Combatant is Mobile mob)
                {
                    mob.Damage(20, this);
                    mob.SendMessage("The cyclone demon charges at you!");
                }

                m_NextCycloneCharge = DateTime.UtcNow + TimeSpan.FromMinutes(1);
            }
        }

        private void WindSlash()
        {
            foreach (Mobile m in GetMobilesInRange(4))
            {
                if (m != this && m.Player)
                {
                    m.SendMessage("Sharp winds cut through you!");
                    m.Damage(15, this);
                }
            }

            m_NextWindSlash = DateTime.UtcNow + TimeSpan.FromMinutes(1);
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

            m_AbilitiesInitialized = false; // Reset flag
        }
    }
}
