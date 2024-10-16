using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a howler monkey corpse")]
    public class HowlerMonkey : BaseCreature
    {
        private static readonly string HowlMessage = "* The Howler Monkey's terrifying howl echoes through the forest! *";
        private static readonly string EnragedMessage = "* The Howler Monkey goes berserk with rage! *";

        private DateTime m_NextHowlTime;
        private DateTime m_NextMonkeySwarmTime;
        private bool m_IsEnraged;
        private bool m_AbilitiesInitialized;

        private TimeSpan HowlCooldown = TimeSpan.FromSeconds(30);
        private TimeSpan MonkeySwarmCooldown = TimeSpan.FromMinutes(1);

        [Constructable]
        public HowlerMonkey()
            : base(AIType.AI_Melee, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            Name = "a Howler Monkey";
            Body = 0x1D; // Gorilla body
            Hue = 1962; // Unique hue, adjust as needed
			this.BaseSoundID = 0x9E;

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

        public HowlerMonkey(Serial serial)
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
                    m_NextHowlTime = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextMonkeySwarmTime = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextHowlTime)
                {
                    HowlOfTerror();
                    m_NextHowlTime = DateTime.UtcNow + HowlCooldown;
                }

                if (DateTime.UtcNow >= m_NextMonkeySwarmTime)
                {
                    MonkeySwarm();
                    m_NextMonkeySwarmTime = DateTime.UtcNow + MonkeySwarmCooldown;
                }

                // Enraged state when health is below 50%
                if (Hits < HitsMax / 2 && !m_IsEnraged)
                {
                    EnragedState();
                }

                // Detect hidden creatures
                foreach (Mobile m in GetMobilesInRange(10))
                {
                    if (m.Hidden || !m.Alive)
                    {
                        this.Combatant = m;
                        break;
                    }
                }
            }
        }

        private void HowlOfTerror()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, HowlMessage);
            FixedEffect(0x376A, 10, 16);
            PlaySound(0x1F2); // Howl sound effect

            foreach (Mobile m in GetMobilesInRange(10))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet)
                {
                    m.Freeze(TimeSpan.FromSeconds(3));
                    m.SendMessage("You are paralyzed with fear by the Howler Monkey's howl!");
                    AOS.Damage(m, this, Utility.RandomMinMax(10, 20), 0, 100, 0, 0, 0);
                }
            }
        }

        private void MonkeySwarm()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Howler Monkey summons a swarm of clones! *");
            PlaySound(0x21E); // Swarm sound effect

            for (int i = 0; i < 3; i++)
            {
                Point3D loc = new Point3D(X + Utility.RandomMinMax(-3, 3), Y + Utility.RandomMinMax(-3, 3), Z);
                if (Map.CanSpawnMobile(loc))
                {
                    MonkeyClone clone = new MonkeyClone(this);
                    clone.MoveToWorld(loc, Map);
                }
            }
        }

        private void EnragedState()
        {
            m_IsEnraged = true;
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, EnragedMessage);
            PlaySound(0x482); // Enraged sound effect

            SetDamage(20, 25);
            SetSkill(SkillName.Tactics, 90.0, 100.0);
            SetSkill(SkillName.Wrestling, 90.0, 100.0);
        }

        public override void OnGotMeleeAttack(Mobile attacker)
        {
            base.OnGotMeleeAttack(attacker);

            if (Utility.RandomDouble() < 0.15) // 15% chance to stun
            {
                attacker.SendMessage("You are stunned by the Howler Monkey's powerful attack!");
                attacker.Freeze(TimeSpan.FromSeconds(2));
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_IsEnraged);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_IsEnraged = reader.ReadBool();
            m_AbilitiesInitialized = false; // Reset flag on deserialize
        }
    }

    public class MonkeyClone : BaseCreature
    {
        private Mobile m_Master;

        public MonkeyClone(Mobile master)
            : base(AIType.AI_Melee, FightMode.None, 10, 1, 0.2, 0.4)
        {
            m_Master = master;

            Body = master.Body;
            Hue = master.Hue;
            Name = "a Howler Monkey clone";

            SetStr(50);
            SetDex(50);
            SetInt(10);

            SetHits(50);

            SetDamage(5, 10);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 50);
            SetResistance(ResistanceType.Fire, 30);
            SetResistance(ResistanceType.Cold, 30);

            VirtualArmor = 30;
        }

        public MonkeyClone(Serial serial)
            : base(serial)
        {
        }

        public override void OnThink()
        {
            base.OnThink();

            if (m_Master == null || m_Master.Deleted || !m_Master.Alive)
            {
                Delete();
                return;
            }

            if (Combatant == null)
                Combatant = m_Master.Combatant;
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
