using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Items;
using Server.Spells;
using Server.Targeting;
using Server.Regions;

namespace Server.Mobiles
{
    [CorpseName("a Sagittarius Harpy corpse")]
    public class SagittariusHarpy : Harpy
    {
        private DateTime m_NextArrowStorm;
        private DateTime m_NextBullseye;
        private DateTime m_NextSummon;
        private DateTime m_NextEnrage;

        private bool m_IsEnraged;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public SagittariusHarpy() : base()
        {
            Name = "Sagittarius Harpy";
            Hue = 2070; // Unique hue resembling arrows
            Body = 30; // Harpy body
            BaseSoundID = 402; // Harpy sound

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

        public SagittariusHarpy(Serial serial) : base(serial)
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
                    // Set random activation times
                    Random rand = new Random();
                    m_NextArrowStorm = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 15));
                    m_NextBullseye = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextSummon = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextEnrage = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing the times
                }

                if (DateTime.UtcNow >= m_NextArrowStorm)
                {
                    ArrowStorm();
                }

                if (DateTime.UtcNow >= m_NextBullseye)
                {
                    Bullseye();
                }

                if (DateTime.UtcNow >= m_NextSummon)
                {
                    SummonMinions();
                }

                if (DateTime.UtcNow >= m_NextEnrage && !m_IsEnraged && Hits < HitsMax * 0.3)
                {
                    Enrage();
                }
            }
        }

        private void ArrowStorm()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Sagittarius Harpy unleashes an Arrow Storm! *");
            Effects.PlaySound(Location, Map, 0x204); // Ranged attack sound

            foreach (Mobile m in GetMobilesInRange(10))
            {
                if (m != this && m.Alive)
                {
                    AOS.Damage(m, this, Utility.RandomMinMax(15, 25), 0, 100, 0, 0, 0);
                    if (m is Mobile mobile)
                    {
                        mobile.SendMessage("You are struck by a flurry of magical arrows!");
                    }
                }
            }

            m_NextArrowStorm = DateTime.UtcNow + TimeSpan.FromSeconds(30); // cooldown
        }

        private void Bullseye()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Sagittarius Harpy targets a weak spot with Bullseye! *");
            Effects.PlaySound(Location, Map, 0x204); // Ranged attack sound

            if (Combatant != null)
            {
                AOS.Damage(Combatant, this, Utility.RandomMinMax(20, 30), 0, 100, 0, 0, 0);
                if (Combatant is Mobile combatant)
                {
                    combatant.SendMessage("A precise arrow strikes a weak spot, dealing extra damage!");
                }
            }

            m_NextBullseye = DateTime.UtcNow + TimeSpan.FromSeconds(60); // cooldown
        }

        private void SummonMinions()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Sagittarius Harpy summons arrow-themed minions! *");
            Effects.PlaySound(Location, Map, 0x204); // Summoning sound

            for (int i = 0; i < 2; i++)
            {
                ArrowMinion minion = new ArrowMinion();
                minion.MoveToWorld(GetSpawnPosition(5), Map);
            }

            m_NextSummon = DateTime.UtcNow + TimeSpan.FromMinutes(1); // cooldown
        }

        private void Enrage()
        {
            m_IsEnraged = true;
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Sagittarius Harpy is enraged! *");
            Effects.PlaySound(Location, Map, 0x204); // Enrage sound

            SetDamage(15, 25);
            SetResistance(ResistanceType.Physical, 50, 60);

            m_NextEnrage = DateTime.UtcNow + TimeSpan.FromMinutes(1); // cooldown
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

            m_NextArrowStorm = DateTime.UtcNow;
            m_NextBullseye = DateTime.UtcNow;
            m_NextSummon = DateTime.UtcNow;
            m_NextEnrage = DateTime.UtcNow;
            m_IsEnraged = false;
            m_AbilitiesInitialized = false; // Reset flag
        }
    }

    public class ArrowMinion : BaseCreature
    {
        [Constructable]
        public ArrowMinion() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Arrow Minion";
            Body = 0x11D; // A small, arrow-themed creature
            Hue = 0x8A5; // Same hue as Sagittarius Harpy

            SetStr(50, 75);
            SetDex(60, 90);
            SetInt(20, 40);

            SetHits(30, 50);
            SetDamage(5, 10);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 10, 20);
            SetResistance(ResistanceType.Fire, 5, 10);
            SetResistance(ResistanceType.Cold, 5, 10);
            SetResistance(ResistanceType.Poison, 5, 10);
            SetResistance(ResistanceType.Energy, 5, 10);

            SetSkill(SkillName.MagicResist, 30.1, 45.0);
            SetSkill(SkillName.Tactics, 40.1, 60.0);
            SetSkill(SkillName.Wrestling, 30.1, 50.0);

            Fame = 1000;
            Karma = -1000;

            VirtualArmor = 20;
        }

        public ArrowMinion(Serial serial) : base(serial)
        {
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);
            c.DropItem(new Arrow(Utility.RandomMinMax(5, 10)));
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
