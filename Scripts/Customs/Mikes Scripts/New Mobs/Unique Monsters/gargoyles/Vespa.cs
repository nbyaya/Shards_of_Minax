using System;
using Server;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a hive mind corpse")]
    public class Vespa : BaseCreature
    {
        private DateTime m_NextSwarmControl;
        private DateTime m_NextHiveShield;
        private DateTime m_NextVenomousSting;
        private DateTime m_NextHiveCall;
        private DateTime m_NextCreepingInfestation;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public Vespa()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Vespa the Hive Mind";
            Body = 4; // Gargoyle body
            Hue = 1667; // Unique hue, e.g., dark greenish-blue
			BaseSoundID = 372;

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

        public Vespa(Serial serial)
            : base(serial)
        {
        }

        public override bool ReacquireOnMovement => !Controlled;
        public override bool AutoDispel => !Controlled;
        public override int TreasureMapLevel => 5;
		public override bool CanAngerOnTame => true;
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
                    m_NextSwarmControl = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextHiveShield = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextVenomousSting = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 25));
                    m_NextHiveCall = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_NextCreepingInfestation = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextSwarmControl)
                {
                    SwarmControl();
                }

                if (DateTime.UtcNow >= m_NextHiveShield)
                {
                    HiveShield();
                }

                if (DateTime.UtcNow >= m_NextVenomousSting)
                {
                    VenomousSting();
                }

                if (DateTime.UtcNow >= m_NextHiveCall)
                {
                    HiveCall();
                }

                if (DateTime.UtcNow >= m_NextCreepingInfestation)
                {
                    CreepingInfestation();
                }
            }
        }

        private void SwarmControl()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Summoning a swarm of bees! *");
            BeeSwarm swarm = new BeeSwarm(this);
            swarm.MoveToWorld(Location, Map);
            m_NextSwarmControl = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Reset cooldown after ability use
        }

        private void HiveShield()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Activating Hive Shield! *");
            SetResistance(ResistanceType.Physical, 70, 80);
            SetResistance(ResistanceType.Poison, 90, 100);
            Timer.DelayCall(TimeSpan.FromSeconds(20), () =>
            {
                SetResistance(ResistanceType.Physical, 50, 60);
                SetResistance(ResistanceType.Poison, 80, 90);
            });
            m_NextHiveShield = DateTime.UtcNow + TimeSpan.FromMinutes(1); // Reset cooldown after ability use
        }

        private void VenomousSting()
        {
            if (Combatant != null)
            {
                Mobile target = Combatant as Mobile;

                if (target != null)
                {
                    PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Delivering a venomous sting! *");
                    target.SendMessage("You have been poisoned by Vespa's sting!");
                    target.ApplyPoison(this, Poison.Lethal);
                    m_NextVenomousSting = DateTime.UtcNow + TimeSpan.FromSeconds(20); // Reset cooldown after ability use
                }
            }
        }

        private void CreepingInfestation()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Infesting the ground with poisonous insects! *");
            Point3D location = Location;

            for (int i = 0; i < 3; i++) // Create multiple infestation points
            {
                Point3D offset = new Point3D(Utility.RandomMinMax(-5, 5), Utility.RandomMinMax(-5, 5), 0);
                Point3D targetLocation = new Point3D(location.X + offset.X, location.Y + offset.Y, location.Z);

                if (Map.CanSpawnMobile(targetLocation))
                {
                    Effects.SendLocationParticles(EffectItem.Create(targetLocation, Map, EffectItem.DefaultDuration), 0x376A, 10, 16, 0x1F2);

                    foreach (Mobile m in GetMobilesInRange(2))
                    {
                        if (m != this && m.Alive)
                        {
                            m.SendMessage("You are being bitten by the infesting insects!");
                            m.ApplyPoison(this, Poison.Lethal);
                            m.Damage(10); // Infestation damage
                        }
                    }
                }
            }
            m_NextCreepingInfestation = DateTime.UtcNow + TimeSpan.FromMinutes(1); // Reset cooldown after ability use
        }

        private void HiveCall()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Calling additional swarms! *");
            for (int i = 0; i < 2; i++) // Summon multiple swarms
            {
                BeeSwarm swarm = new BeeSwarm(this);
                swarm.MoveToWorld(Location, Map);
            }
            m_NextHiveCall = DateTime.UtcNow + TimeSpan.FromMinutes(2); // Reset cooldown after ability use
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

    public class BeeSwarm : BaseCreature
    {
        private Mobile m_Master;

        public BeeSwarm(Mobile master)
            : base(AIType.AI_Animal, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            m_Master = master;
            Body = 0xF8; // Bee body
            Hue = 0x45F; // Yellow-black hue

            SetStr(100);
            SetDex(150);
            SetInt(50);

            SetHits(100);

            SetDamage(10, 20);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 40, 50);
            SetResistance(ResistanceType.Fire, 10, 20);
            SetResistance(ResistanceType.Cold, 10, 20);
            SetResistance(ResistanceType.Poison, 70, 80);

            VirtualArmor = 30;
        }

        public BeeSwarm(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
            writer.Write(m_Master);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_Master = reader.ReadMobile();
        }
    }
}
