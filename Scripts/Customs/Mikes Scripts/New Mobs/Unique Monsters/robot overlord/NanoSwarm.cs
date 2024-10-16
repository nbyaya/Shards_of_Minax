using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a nano swarm corpse")]
    public class NanoSwarm : BaseCreature
    {
        private DateTime m_NextNanobotSwarm;
        private DateTime m_NextRepairProtocol;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public NanoSwarm()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a nano swarm";
            Body = 0x2F4; // Exodus Overseer body
            Hue = 2288; // Unique hue for the Nano Swarm

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

        public NanoSwarm(Serial serial)
            : base(serial)
        {
        }
        public override int GetIdleSound()
        {
            return 0xFD;
        }

        public override int GetAngerSound()
        {
            return 0x26C;
        }

        public override int GetDeathSound()
        {
            return 0x211;
        }

        public override int GetAttackSound()
        {
            return 0x23B;
        }

        public override int GetHurtSound()
        {
            return 0x140;
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
                    m_NextNanobotSwarm = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextRepairProtocol = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextNanobotSwarm)
                {
                    NanobotSwarm();
                }

                if (DateTime.UtcNow >= m_NextRepairProtocol)
                {
                    RepairProtocol();
                }
            }
        }

        private void NanobotSwarm()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Nano Swarm activated! *");
            FixedEffect(0x376A, 10, 16);
            PlaySound(0x20D); // Sound for nanobot swarm

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive)
                {
                    m.SendMessage("You are engulfed in a swirling cloud of nanobots!");
                    // Apply damage
                    m.Damage(Utility.RandomMinMax(15, 25), this);

                    // Apply confusion
                    m.SendMessage("The nanobots confuse your senses!");
                    m.Freeze(TimeSpan.FromSeconds(2));

                    // Apply slow effect
                    m.Dex = Math.Max(m.Dex - 20, 10);
                }
            }

            // Reset the next ability use time with a cooldown
            Random rand = new Random();
            m_NextNanobotSwarm = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(30, 60));
        }

        private void RepairProtocol()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Repair Protocol activated! *");
            FixedEffect(0x376A, 10, 16);
            PlaySound(0x20E); // Sound for repair protocol

            // Heal itself
            Hits = Math.Min(Hits + 30, HitsMax);

            // Heal nearby allies
            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m is BaseCreature ally && ally.Alive && ally != this)
                {
                    ally.Hits = Math.Min(ally.Hits + 20, ally.HitsMax);
                    m.SendMessage("Nanobots repair your wounds!");
                }
            }

            // Reset the next ability use time with a cooldown
            Random rand = new Random();
            m_NextRepairProtocol = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(45, 75));
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version

            // Serialize ability timings
            writer.Write(m_NextNanobotSwarm);
            writer.Write(m_NextRepairProtocol);
            writer.Write(m_AbilitiesInitialized);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Deserialize ability timings
            m_NextNanobotSwarm = reader.ReadDateTime();
            m_NextRepairProtocol = reader.ReadDateTime();
            m_AbilitiesInitialized = reader.ReadBool();
        }
    }
}
