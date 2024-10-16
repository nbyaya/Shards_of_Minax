using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a taurus earthbear corpse")]
    public class TaurusEarthBear : GrizzlyBear
    {
        private DateTime m_NextEarthquakeStomp;
        private DateTime m_NextRockSolid;
        private DateTime m_NextStoneFury;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public TaurusEarthBear()
            : base()
        {
            Name = "Taurus EarthBear";
            Body = 212; // GrizzlyBear body
            Hue = 1996; // Unique hue for the Taurus EarthBear
			BaseSoundID = 0xA3;

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

            // Initialize the abilities as not set
            m_AbilitiesInitialized = false;
        }

        public TaurusEarthBear(Serial serial)
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
                    m_NextEarthquakeStomp = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextRockSolid = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextStoneFury = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextEarthquakeStomp)
                {
                    EarthquakeStomp();
                }

                if (DateTime.UtcNow >= m_NextRockSolid)
                {
                    RockSolid();
                }

                if (DateTime.UtcNow >= m_NextStoneFury)
                {
                    StoneFury();
                }
            }
        }

        private void EarthquakeStomp()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Taurus EarthBear causes the ground to quake! *");
            FixedEffect(0x376A, 10, 16);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive)
                {
                    m.SendMessage("You are stunned by the earthquake!");
                    m.Freeze(TimeSpan.FromSeconds(2));

                    int damage = Utility.RandomMinMax(15, 25);
                    AOS.Damage(m, this, damage, 0, 100, 0, 0, 0);
                }
            }

            m_NextEarthquakeStomp = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown for EarthquakeStomp
        }

        private void RockSolid()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Taurus EarthBear becomes rock solid! *");
            this.VirtualArmor += 15; // Increased virtual armor
            this.SetResistance(ResistanceType.Physical, this.GetResistance(ResistanceType.Physical) + 15);

            Timer.DelayCall(TimeSpan.FromSeconds(10), new TimerCallback(() =>
            {
                this.VirtualArmor -= 15; // Reset armor after 10 seconds
                this.SetResistance(ResistanceType.Physical, this.GetResistance(ResistanceType.Physical) - 15);
            }));

            m_NextRockSolid = DateTime.UtcNow + TimeSpan.FromSeconds(60); // Cooldown for RockSolid
        }

        private void StoneFury()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Taurus EarthBear unleashes a fury of stone shards! *");
            FixedEffect(0x374A, 10, 30);

            foreach (Mobile m in GetMobilesInRange(8))
            {
                if (m != this && m.Alive)
                {
                    int damage = Utility.RandomMinMax(10, 20);
                    AOS.Damage(m, this, damage, 0, 100, 0, 0, 0);
                    m.SendMessage("You are hit by a barrage of stone shards!");
                }
            }

            m_NextStoneFury = DateTime.UtcNow + TimeSpan.FromSeconds(45); // Cooldown for StoneFury
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            // Drop a special item on death
            Item loot = new EarthFragment(); // Example item class, replace with actual item
            loot.MoveToWorld(Location, Map);
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Taurus EarthBear drops an Earth Fragment! *");
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

    // Example item class for Earth Fragment
    public class EarthFragment : Item
    {
        [Constructable]
        public EarthFragment() : base(0x1B1D) // Example item ID
        {
            Name = "Earth Fragment";
            Hue = 0x837; // Same hue as Taurus EarthBear
        }

        public EarthFragment(Serial serial) : base(serial)
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
