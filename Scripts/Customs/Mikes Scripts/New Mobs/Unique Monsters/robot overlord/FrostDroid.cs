using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a frost droid corpse")]
    public class FrostDroid : BaseCreature
    {
        private DateTime m_NextIceShardBarrage;
        private DateTime m_NextFreezingAura;
        private DateTime m_NextSummonMinions;
        private DateTime m_NextIcyMist;

        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public FrostDroid()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a frost droid";
            Body = 0x2F4; // ExodusOverseer body
            Hue = 2293; // Unique icy hue

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

        public FrostDroid(Serial serial)
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
                    Random rand = new Random();
                    m_NextIceShardBarrage = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextFreezingAura = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_NextSummonMinions = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 50));
                    m_NextIcyMist = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 35));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextIceShardBarrage)
                {
                    IceShardBarrage();
                }

                if (DateTime.UtcNow >= m_NextFreezingAura)
                {
                    FreezingAura();
                }

                if (DateTime.UtcNow >= m_NextSummonMinions)
                {
                    SummonMinions();
                }

                if (DateTime.UtcNow >= m_NextIcyMist)
                {
                    IcyMist();
                }
            }
        }

        private void IceShardBarrage()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Ice Shard Barrage! *");
            FixedEffect(0x36D4, 10, 16);

            foreach (Mobile m in GetMobilesInRange(6))
            {
                if (m != this && m.Alive)
                {
                    m.SendMessage("Sharp ice shards pierce your body!");
                    m.Damage(Utility.RandomMinMax(15, 20), this);
                    m.Freeze(TimeSpan.FromSeconds(3)); // Apply slow effect
                }
            }

            m_NextIceShardBarrage = DateTime.UtcNow + TimeSpan.FromSeconds(15); // Reset to constant cooldown after use
        }

        private void FreezingAura()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Freezing Aura activated! *");
            FixedEffect(0x37D4, 10, 16);

            foreach (Mobile m in GetMobilesInRange(6))
            {
                if (m != this && m.Alive)
                {
                    m.SendMessage("You are engulfed in a freezing aura!");
                    m.Damage(Utility.RandomMinMax(10, 15), this);
                    m.Freeze(TimeSpan.FromSeconds(4)); // Apply slow effect
                }
            }

            m_NextFreezingAura = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Reset to constant cooldown after use
        }

        private void SummonMinions()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Summoning Ice Minions! *");
            FixedEffect(0x376A, 10, 16);

            for (int i = 0; i < 2; i++)
            {
                Point3D spawnLocation = new Point3D(X + Utility.RandomMinMax(-5, 5), Y + Utility.RandomMinMax(-5, 5), Z);
                FrostDroidMinion minion = new FrostDroidMinion();
                minion.MoveToWorld(spawnLocation, Map);
            }

            m_NextSummonMinions = DateTime.UtcNow + TimeSpan.FromSeconds(45); // Reset to constant cooldown after use
        }

        private void IcyMist()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Icy Mist envelops the area! *");
            FixedEffect(0x3728, 10, 16);

            foreach (Mobile m in GetMobilesInRange(6))
            {
                if (m != this && m.Alive)
                {
                    m.SendMessage("You are caught in the freezing mist!");
                    m.Damage(Utility.RandomMinMax(5, 10), this);
                    if (Utility.RandomBool())
                    {
                        m.Freeze(TimeSpan.FromSeconds(5)); // Chance to freeze
                    }
                }
            }

            m_NextIcyMist = DateTime.UtcNow + TimeSpan.FromSeconds(20); // Reset to constant cooldown after use
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

            m_AbilitiesInitialized = false; // Reset initialization flag on deserialization
        }
    }

    public class FrostDroidMinion : BaseCreature
    {
        [Constructable]
        public FrostDroidMinion()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "an ice minion";
            Body = 0x2F4; // ExodusOverseer body
            Hue = 1153; // Slightly different icy hue

            this.SetStr(150, 200);
            this.SetDex(60, 80);
            this.SetInt(60, 80);

            this.SetHits(100, 150);

            this.SetDamage(10, 15);

            this.SetDamageType(ResistanceType.Cold, 100);

            this.SetResistance(ResistanceType.Physical, 40, 50);
            this.SetResistance(ResistanceType.Fire, 20, 30);
            this.SetResistance(ResistanceType.Cold, 50, 60);
            this.SetResistance(ResistanceType.Poison, 10, 20);
            this.SetResistance(ResistanceType.Energy, 10, 20);

            this.SetSkill(SkillName.MagicResist, 60.0, 80.0);
            this.SetSkill(SkillName.Tactics, 60.0, 80.0);
            this.SetSkill(SkillName.Wrestling, 60.0, 80.0);

            this.Fame = 5000;
            this.Karma = -5000;
            this.VirtualArmor = 30;
        }

        public FrostDroidMinion(Serial serial)
            : base(serial)
        {
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
