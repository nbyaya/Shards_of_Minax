using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a grotesque of Rouen corpse")]
    public class GrotesqueOfRouen : BaseCreature
    {
        private DateTime m_NextDistortedReality;
        private DateTime m_NextCacophony;
        private DateTime m_NextNightmareVisage;
        private DateTime m_NextIllusoryMinions;

        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public GrotesqueOfRouen()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a grotesque of Rouen";
            Body = 4; // Gargoyle body
            Hue = 1670; // Unique hue for a twisted appearance
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

        public GrotesqueOfRouen(Serial serial)
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

        public override int Meat { get { return 1; } }
        public override bool CanFly { get { return true; } }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    // Set random intervals for abilities
                    Random rand = new Random();
                    m_NextDistortedReality = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextCacophony = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextNightmareVisage = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_NextIllusoryMinions = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 50));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextDistortedReality)
                {
                    DistortedReality();
                }

                if (DateTime.UtcNow >= m_NextCacophony)
                {
                    Cacophony();
                }

                if (DateTime.UtcNow >= m_NextNightmareVisage)
                {
                    NightmareVisage();
                }

                if (DateTime.UtcNow >= m_NextIllusoryMinions)
                {
                    IllusoryMinions();
                }
            }
        }

        private void DistortedReality()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The battlefield warps and shifts wildly! *");

            foreach (Mobile m in GetMobilesInRange(10))
            {
                if (m != this && m.Alive)
                {
                    int x = X + Utility.RandomMinMax(-5, 5);
                    int y = Y + Utility.RandomMinMax(-5, 5);
                    int z = Map.GetAverageZ(x, y);
                    Point3D loc = new Point3D(x, y, z);

                    if (Map.CanSpawnMobile(loc))
                    {
                        m.Location = loc;
                    }
                }
            }

            m_NextDistortedReality = DateTime.UtcNow + TimeSpan.FromSeconds(20);
        }

        private void Cacophony()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* A horrifying scream echoes across the battlefield! *");

            foreach (Mobile m in GetMobilesInRange(10))
            {
                if (m != this && m.Alive)
                {
                    m.SendMessage("You are overwhelmed with confusion!");
                    m.Freeze(TimeSpan.FromSeconds(5));
                    
                    // Optional: You can add special effects or additional behavior for players if needed
                    // For example:
                    if (m is PlayerMobile)
                    {
                        // Add any specific behavior or effects for players here if needed
                    }
                }
            }

            m_NextCacophony = DateTime.UtcNow + TimeSpan.FromMinutes(2);
        }

        private void NightmareVisage()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Nightmarish visions cloud your mind! *");

            foreach (Mobile m in GetMobilesInRange(10))
            {
                if (m != this && m.Alive)
                {
                    new Illusion(m).MoveToWorld(m.Location, m.Map);
                }
            }

            m_NextNightmareVisage = DateTime.UtcNow + TimeSpan.FromMinutes(1);
        }

        private void IllusoryMinions()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Dark shadows gather and form into monstrous shapes! *");

            for (int i = 0; i < 3; i++)
            {
                Point3D loc = GetSpawnPosition(5);

                if (loc != Point3D.Zero)
                {
                    Illusion minion = new Illusion(this);
                    minion.MoveToWorld(loc, Map);
                    minion.Combatant = Combatant;
                }
            }

            m_NextIllusoryMinions = DateTime.UtcNow + TimeSpan.FromMinutes(3);
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

        private class Illusion : BaseCreature
        {
            public Illusion(Mobile master)
                : base(AIType.AI_Melee, FightMode.None, 10, 1, 0.2, 0.4)
            {
                Body = master.Body;
                Hue = master.Hue;
                Name = master.Name;

                SetStr(1);
                SetDex(1);
                SetInt(1);

                SetHits(1);
                SetDamage(0);

                SetResistance(ResistanceType.Physical, 100);
                SetResistance(ResistanceType.Fire, 100);
                SetResistance(ResistanceType.Cold, 100);
                SetResistance(ResistanceType.Poison, 100);
                SetResistance(ResistanceType.Energy, 100);

                VirtualArmor = 100;
            }

            public Illusion(Serial serial)
                : base(serial)
            {
            }

            public override bool DeleteCorpseOnDeath { get { return true; } }

            public override void OnThink()
            {
                if (Combatant == null)
                    Delete();
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

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Reset ability timers and initialization flag on deserialization
            m_AbilitiesInitialized = false;
        }
    }
}
