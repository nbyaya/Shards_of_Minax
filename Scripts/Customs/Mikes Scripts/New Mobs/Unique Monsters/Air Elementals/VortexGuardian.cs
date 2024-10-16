using System;
using Server.Items;
using Server.Network;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a vortex guardian corpse")]
    public class VortexGuardian : BaseCreature
    {
        private DateTime m_NextVortexPull;
        private DateTime m_NextCyclonicArmor;
        private DateTime m_NextGaleStrike;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public VortexGuardian()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a vortex guardian";
            Body = 13; // Airy, swirling body
            Hue = 1153; // Airy hue
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

            // Initialize with default values
            m_NextVortexPull = DateTime.UtcNow;
            m_NextCyclonicArmor = DateTime.UtcNow;
            m_NextGaleStrike = DateTime.UtcNow;
            m_AbilitiesInitialized = false;
        }

        public VortexGuardian(Serial serial)
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
                    // Randomly set initial activation times
                    Random rand = new Random();
                    m_NextVortexPull = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextCyclonicArmor = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextGaleStrike = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));

                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextVortexPull)
                {
                    VortexPull();
                }

                if (DateTime.UtcNow >= m_NextCyclonicArmor)
                {
                    CyclonicArmor();
                }

                if (DateTime.UtcNow >= m_NextGaleStrike)
                {
                    GaleStrike();
                }
            }
        }

        private void VortexPull()
        {
            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m != Combatant && m.Alive)
                {
                    Effects.SendLocationParticles(EffectItem.Create(m.Location, Map, EffectItem.DefaultDuration), 0x3709, 10, 30, 2115);
                    m.MoveToWorld(Location, Map);
                    m.SendMessage("You are pulled into a powerful vortex!");
                    m.Damage(Utility.RandomMinMax(10, 15), this);
                }
            }

            m_NextVortexPull = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Reset with fixed interval
        }

        private void CyclonicArmor()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The vortex guardian is surrounded by a swirling vortex! *");

            // Increase defense
            this.VirtualArmor = 60;

            // Deal damage to nearby enemies
            foreach (Mobile m in GetMobilesInRange(2))
            {
                if (m != this && m != Combatant && m.Alive)
                {
                    m.SendMessage("You are damaged by the cyclonic vortex!");
                    m.Damage(Utility.RandomMinMax(5, 10), this);
                }
            }

            // Reset defense after a duration
            Timer.DelayCall(TimeSpan.FromSeconds(10), () =>
            {
                this.VirtualArmor = 50;
            });

            m_NextCyclonicArmor = DateTime.UtcNow + TimeSpan.FromMinutes(1); // Reset with fixed interval
        }

        private void GaleStrike()
        {
            if (Combatant != null)
            {
                Mobile target = Combatant as Mobile;
                if (target != null && target.Alive)
                {
                    Effects.SendLocationParticles(EffectItem.Create(target.Location, Map, EffectItem.DefaultDuration), 0x3709, 10, 30, 2115);
                    target.SendMessage("You are struck by a powerful gale!");
                    target.Damage(Utility.RandomMinMax(15, 20), this);

                    // Knock back effect
                    Point3D newLocation = target.Location;
                    newLocation.X += Utility.RandomMinMax(-2, 2);
                    newLocation.Y += Utility.RandomMinMax(-2, 2);

                    if (Map.CanSpawnMobile(newLocation))
                        target.Location = newLocation;
                }

                m_NextGaleStrike = DateTime.UtcNow + TimeSpan.FromSeconds(20); // Reset with fixed interval
            }
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

            // Reset ability initialization flag
            m_AbilitiesInitialized = false;
        }
    }
}
