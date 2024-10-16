using System;
using Server;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a bone golem corpse")]
    public class BoneGolem : BaseCreature
    {
        private DateTime m_NextBoneShardToss;
        private DateTime m_NextSkeletonArmy;
        private DateTime m_NextCursedAura;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public BoneGolem()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.3, 0.6)
        {
            Name = "a Bone Golem";
            Body = 752; // Golem body
            Hue = 2120; // Bone-like hue
			BaseSoundID = 357;

            SetStr(300);
            SetDex(80);
            SetInt(100);

            SetDamage(18, 30);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 50, 70);
            SetResistance(ResistanceType.Fire, 30, 50);
            SetResistance(ResistanceType.Cold, 40, 60);
            SetResistance(ResistanceType.Energy, 25, 45);
            SetResistance(ResistanceType.Poison, 50, 70);

            SetSkill(SkillName.MagicResist, 80.0);
            SetSkill(SkillName.Tactics, 80.0);
            SetSkill(SkillName.Wrestling, 80.0);

            Fame = 2000;
            Karma = -2000;

            VirtualArmor = 50;

            m_AbilitiesInitialized = false; // Initialize flag
        }

        public BoneGolem(Serial serial)
            : base(serial)
        {
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
                    m_NextBoneShardToss = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextSkeletonArmy = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_NextCursedAura = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextBoneShardToss)
                {
                    BoneShardToss();
                }

                if (DateTime.UtcNow >= m_NextSkeletonArmy)
                {
                    SkeletonArmy();
                }

                if (DateTime.UtcNow >= m_NextCursedAura)
                {
                    CursedAura();
                }
            }
        }

        private void BoneShardToss()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Bone Golem hurls bone shards! *");
            Effects.SendLocationEffect(Location, Map, 0x3709, 10, 16, 0, 0);

            foreach (Mobile m in GetMobilesInRange(10))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet)
                {
                    int damage = Utility.RandomMinMax(15, 25);
                    AOS.Damage(m, this, damage, 0, 0, 0, 0, 0);

                    m.SendMessage("Bone shards pierce through you!");
                }
            }

            m_NextBoneShardToss = DateTime.UtcNow + TimeSpan.FromSeconds(15); // Set cooldown
        }

        private void SkeletonArmy()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Bone Golem summons skeletal minions! *");
            Effects.SendLocationEffect(Location, Map, 0x3709, 10, 16, 0, 0);

            for (int i = 0; i < 3; i++)
            {
                Skeleton skeleton = new Skeleton();
                Point3D spawnLocation = GetSpawnPosition(5);

                if (spawnLocation != Point3D.Zero)
                {
                    skeleton.MoveToWorld(spawnLocation, Map);
                    skeleton.Combatant = Combatant;
                }
            }

            m_NextSkeletonArmy = DateTime.UtcNow + TimeSpan.FromMinutes(1); // Set cooldown
        }

        private void CursedAura()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Bone Golem radiates a cursed aura! *");
            Effects.SendLocationEffect(Location, Map, 0x36BD, 10, 30, 0, 0);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet)
                {
                    m.SendMessage("You feel the weight of the curse upon you!");

                    m.Damage((int)(m.Hits * 0.1), this); // Increase damage taken
                    m.Skills[SkillName.Tactics].Base -= 10; // Reduce stats
                }
            }

            m_NextCursedAura = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Set cooldown
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

            m_AbilitiesInitialized = false; // Reset flag on deserialize
        }
    }
}
