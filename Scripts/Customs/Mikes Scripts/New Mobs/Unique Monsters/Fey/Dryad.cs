using System;
using Server;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a dryad corpse")]
    public class Dryad : BaseCreature
    {
        private DateTime m_NextRegrowth;
        private DateTime m_NextEntangle;
        private DateTime m_NextCamouflage;
        private DateTime m_CamouflageEnd;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public Dryad()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a dryad";
            Body = 0x191; // GreenGoblin body
            BaseSoundID = 0x57B; // Pixie sound
            Hue = 1590; // Forest green hue

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

            PackItem(new Log(Utility.RandomMinMax(5, 10)));
            PackItem(new Shaft(Utility.RandomMinMax(5, 10)));

            m_AbilitiesInitialized = false; // Initialize flag
        }

        public Dryad(Serial serial)
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

        public override Poison PoisonImmune { get { return Poison.Lesser; } }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    // Set random intervals for abilities
                    Random rand = new Random();
                    m_NextRegrowth = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_NextEntangle = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_NextCamouflage = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 120));
                    m_CamouflageEnd = DateTime.MinValue; // Initial value for camouflage end
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextRegrowth)
                {
                    DoRegrowth();
                }

                if (DateTime.UtcNow >= m_NextEntangle)
                {
                    DoEntangle();
                }

                if (DateTime.UtcNow >= m_NextCamouflage && DateTime.UtcNow >= m_CamouflageEnd)
                {
                    DoCamouflage();
                }
            }

            if (DateTime.UtcNow >= m_CamouflageEnd && Hidden)
            {
                Hidden = false;
            }
        }

        private void DoRegrowth()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, false, "* Channels nature's energy *");
            PlaySound(0x57B);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m is TreefellowGuardian && m.Hits < m.HitsMax)
                {
                    m.Hits = Math.Min(m.Hits + 50, m.HitsMax);
                    m.FixedEffect(0x376A, 9, 32);
                }
            }

            SpawnTreefellowGuardian();

            m_NextRegrowth = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 60));
        }

        private void DoEntangle()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, false, "* Calls upon the plants *");
            PlaySound(0x223);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Player)
                {
                    m.SendLocalizedMessage(1072132); // The dryad's plants grab you, slowing you down!
                    m.Freeze(TimeSpan.FromSeconds(5));
                    m.FixedEffect(0x376A, 9, 32);
                }
            }

            m_NextEntangle = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 40));
        }

        private void DoCamouflage()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, false, "* Blends with nature *");
            PlaySound(0x15F);
            Hidden = true;

            m_CamouflageEnd = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 20));
            m_NextCamouflage = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(60, 120));
        }

        private void SpawnTreefellowGuardian()
        {
            Map map = this.Map;

            if (map == null)
                return;

            int newTreefellows = 0;

            foreach (Mobile m in GetMobilesInRange(10))
            {
                if (m is TreefellowGuardian)
                    newTreefellows++;
            }

            if (newTreefellows < 3)
            {
                BaseCreature treefellow = new TreefellowGuardian();

                Point3D loc = GetSpawnPosition(3);

                if (loc != Point3D.Zero)
                {
                    treefellow.MoveToWorld(loc, map);
                    treefellow.Combatant = Combatant;
                }
                else
                {
                    treefellow.Delete();
                }
            }
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

            m_AbilitiesInitialized = false; // Reset the initialization flag on deserialization
            m_NextRegrowth = DateTime.UtcNow;
            m_NextEntangle = DateTime.UtcNow;
            m_NextCamouflage = DateTime.UtcNow;
            m_CamouflageEnd = DateTime.MinValue;
        }
    }
}
