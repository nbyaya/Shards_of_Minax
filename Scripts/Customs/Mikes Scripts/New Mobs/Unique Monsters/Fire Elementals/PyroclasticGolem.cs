using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("a pyroclastic golem corpse")]
    public class PyroclasticGolem : BaseCreature
    {
        private DateTime m_NextMagmaSlam;
        private DateTime m_NextLavaArmor;
        private DateTime m_NextEruption;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public PyroclasticGolem()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            this.Name = "a pyroclastic golem";
            this.Body = 15; // Fire Elemental body
            this.Hue = 1596; // Unique hue for the Pyroclastic Golem
            this.BaseSoundID = 838;

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

            // Initialize ability cooldowns with random intervals
            Random rand = new Random();
            m_NextMagmaSlam = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
            m_NextLavaArmor = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
            m_NextEruption = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 50));
            m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
        }

        public PyroclasticGolem(Serial serial)
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
                if (DateTime.UtcNow >= m_NextMagmaSlam)
                {
                    MagmaSlam();
                }

                if (DateTime.UtcNow >= m_NextLavaArmor)
                {
                    LavaArmor();
                }

                if (DateTime.UtcNow >= m_NextEruption)
                {
                    Eruption();
                }
            }
        }

        private void MagmaSlam()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Pyroclastic Golem slams the ground, causing molten eruptions! *");

            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m.Player)
                {
                    m.SendMessage("You are engulfed in a molten eruption!");
                    int damage = Utility.RandomMinMax(20, 35);
                    m.Damage(damage, this); // Apply fire damage
                }
            }

            Random rand = new Random();
            m_NextMagmaSlam = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(30, 60));
        }

        private void LavaArmor()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Pyroclastic Golem encases itself in molten rock! *");
            this.VirtualArmor += 20;

            Timer.DelayCall(TimeSpan.FromSeconds(20), () =>
            {
                this.VirtualArmor -= 20;
            });

            Random rand = new Random();
            m_NextLavaArmor = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(60, 120));
        }

        private void Eruption()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Pyroclastic Golem causes a volcanic eruption! *");

            for (int i = 0; i < 3; i++)
            {
                Point3D loc = GetSpawnPosition(5);
                if (loc != Point3D.Zero)
                {
                    Fireball fireball = new Fireball();
                    fireball.MoveToWorld(loc, Map);
                }
            }

            Random rand = new Random();
            m_NextEruption = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(120, 180));
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

            // Reinitialize the ability cooldowns with random intervals on deserialization
            Random rand = new Random();
            m_NextMagmaSlam = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
            m_NextLavaArmor = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
            m_NextEruption = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 50));
            m_AbilitiesInitialized = true;
        }
    }

    public class Fireball : Item
    {
        private Timer m_Timer;
        private Timer m_DeleteTimer;
        public int Damage { get; set; }
        public TimeSpan AutoDelete { get; set; } // Duration before auto-deletion

        [Constructable]
        public Fireball() : base(0x122A) // Use the appropriate item ID for a lava tile
        {
            Movable = false;
            Name = "a hot lava tile";
            Hue = 0x48; // Lava color

            Damage = 10; // Default damage

            m_Timer = Timer.DelayCall(TimeSpan.Zero, TimeSpan.FromSeconds(2), CheckForPlayers);

            AutoDelete = TimeSpan.FromSeconds(15); // Set the auto-delete time (15 seconds)
            StartDeleteTimer();
        }

        private void CheckForPlayers()
        {
            if (Deleted) return;

            IPooledEnumerable eable = GetMobilesInRange(0);
            foreach (Mobile m in eable)
            {
                m.Damage(Damage, m);
            }
            eable.Free();
        }

        private void StartDeleteTimer()
        {
            if (m_DeleteTimer != null)
                m_DeleteTimer.Stop();

            m_DeleteTimer = Timer.DelayCall(AutoDelete, DeleteTimer);
        }

        private void DeleteTimer()
        {
            this.Delete(); // Automatically delete the tile
        }

        public Fireball(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int) 0); // version
            writer.Write(Damage);
            writer.Write(AutoDelete);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            Damage = reader.ReadInt();
            AutoDelete = reader.ReadTimeSpan();
            m_Timer = Timer.DelayCall(TimeSpan.Zero, TimeSpan.FromSeconds(2), CheckForPlayers);
            StartDeleteTimer(); // Restart the delete timer on server restart
        }
    }
}
