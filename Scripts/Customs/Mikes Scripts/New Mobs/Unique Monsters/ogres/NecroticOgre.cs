using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a necrotic ogre corpse")]
    public class NecroticOgre : BaseCreature
    {
        private DateTime m_NextDeathTouch;
        private DateTime m_NextSummonUndead;
        private DateTime m_NextDecayAura;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public NecroticOgre()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a necrotic ogre";
            Body = 1; // Ogre body
            Hue = 2173; // Dark necrotic hue
			BaseSoundID = 427;

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

            m_AbilitiesInitialized = false; // Set the flag to false initially
        }

        public NecroticOgre(Serial serial)
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
                    Random rand = new Random();
                    m_NextDeathTouch = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextSummonUndead = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_NextDecayAura = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 45));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextDeathTouch)
                {
                    DeathTouch();
                }

                if (DateTime.UtcNow >= m_NextSummonUndead)
                {
                    SummonUndead();
                }

                if (DateTime.UtcNow >= m_NextDecayAura)
                {
                    DecayAura();
                }
            }
        }

        private void DeathTouch()
        {
            Mobile target = Combatant as Mobile;
            if (target != null && target.Alive)
            {
                int damage = Utility.RandomMinMax(20, 40);
                target.Damage(damage, this);

                // Simulate reduction in max health
                int newHitsMax = Math.Max(target.HitsMax - 10, 1); // Ensure it doesn't go below 1
                target.Hits = Math.Min(target.Hits, newHitsMax); // Ensure current hits don't exceed new max
                target.SendMessage("Your maximum health has been reduced!");

                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Necrotic touch!*");
                target.FixedEffect(0x376A, 10, 16);

                m_NextDeathTouch = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Reset cooldown
            }
        }

        private void SummonUndead()
        {
            Point3D loc = GetSpawnPosition(5);

            if (loc != Point3D.Zero)
            {
                UndeadMinion minion = new UndeadMinion();
                minion.MoveToWorld(loc, Map);

                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Summoning undead minions!*");

                m_NextSummonUndead = DateTime.UtcNow + TimeSpan.FromMinutes(2); // Reset cooldown
            }
        }

        private void DecayAura()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Decay aura engulfs the area!*");
            FixedEffect(0x376A, 10, 16);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m != Combatant && m.Alive)
                {
                    m.Damage(5, this);
                }
            }

            m_NextDecayAura = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Reset cooldown
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
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_AbilitiesInitialized = false; // Reset initialization flag
        }
    }

    public class UndeadMinion : BaseCreature
    {
        public UndeadMinion()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "an undead minion";
            Body = 0x3D; // Skeleton body
            Hue = 0x4001; // Dark necrotic hue

            SetStr(100);
            SetDex(60);
            SetInt(50);

            SetHits(100);
            SetMana(0);

            SetDamage(10, 20);

            SetDamageType(ResistanceType.Physical, 0);
            SetDamageType(ResistanceType.Poison, 100);

            SetResistance(ResistanceType.Physical, 30, 40);
            SetResistance(ResistanceType.Fire, 20, 30);
            SetResistance(ResistanceType.Cold, 20, 30);
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 20, 30);

            SetSkill(SkillName.MagicResist, 50.0, 70.0);
            SetSkill(SkillName.Tactics, 60.0, 80.0);
            SetSkill(SkillName.Wrestling, 70.0, 90.0);

            VirtualArmor = 40;
        }

        public UndeadMinion(Serial serial)
            : base(serial)
        {
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
        }
    }
}
