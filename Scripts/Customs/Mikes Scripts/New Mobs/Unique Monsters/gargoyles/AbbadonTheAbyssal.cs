using System;
using Server.Items;
using Server.Network;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("an abyssal corpse")]
    public class AbbadonTheAbyssal : BaseCreature
    {
        private DateTime m_NextAbyssalDrain;
        private DateTime m_NextVoidBurst;
        private DateTime m_NextShadowSlip;
        private DateTime m_NextAbyssalWave;
        private DateTime m_NextDarkPortal;
        private DateTime m_NextAbyssalShield;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public AbbadonTheAbyssal()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Abbadon the Abyssal";
            Body = 4; // Gargoyle body
            Hue = 1757; // Dark hue for abyssal theme
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

        public AbbadonTheAbyssal(Serial serial)
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
                    m_NextAbyssalDrain = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextVoidBurst = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextShadowSlip = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_NextAbyssalWave = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_NextDarkPortal = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_NextAbyssalShield = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 120));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextAbyssalDrain)
                {
                    AbyssalDrain();
                }

                if (DateTime.UtcNow >= m_NextVoidBurst)
                {
                    VoidBurst();
                }

                if (DateTime.UtcNow >= m_NextShadowSlip)
                {
                    ShadowSlip();
                }

                if (DateTime.UtcNow >= m_NextAbyssalWave)
                {
                    AbyssalWave();
                }

                if (DateTime.UtcNow >= m_NextDarkPortal)
                {
                    DarkPortal();
                }

                if (DateTime.UtcNow >= m_NextAbyssalShield)
                {
                    AbyssalShield();
                }
            }
        }

        private void AbyssalDrain()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Abbadon drains the abyss! *");

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m.Player)
                {
                    int manaDrain = Utility.RandomMinMax(15, 25);
                    m.Mana = Math.Max(0, m.Mana - manaDrain);
                    m.SendMessage("You feel a cold drain on your mana from Abbadon!");
                    // Reducing spellcasting effectiveness
                    m.Skills[SkillName.Magery].Base -= 10;
                }
            }

            m_NextAbyssalDrain = DateTime.UtcNow + TimeSpan.FromSeconds(20);
        }

        private void VoidBurst()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Abbadon releases a void burst! *");
            PlaySound(0x20D);
            FixedEffect(0x376A, 10, 16);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive)
                {
                    int damage = Utility.RandomMinMax(20, 30);
                    m.Damage(damage, this);
                    m.SendMessage("You are struck by a burst of dark energy!");
                    // Apply damage boost debuff
                    m.SendMessage("You feel more vulnerable to Abbadon's attacks!");
                }
            }

            m_NextVoidBurst = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        private void ShadowSlip()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Abbadon slips into the shadows! *");
            PlaySound(0x20D);
            FixedEffect(0x373A, 10, 16);

            Point3D newLocation = GetSpawnPosition(5);
            if (newLocation != Point3D.Zero)
            {
                this.Location = newLocation;
                this.Map = Map;

                foreach (Mobile m in GetMobilesInRange(5))
                {
                    if (m != this && m.Alive)
                    {
                        m.SendMessage("You feel a dark presence as Abbadon slips away!");
                    }
                }

                // Leave behind an area of darkness
                Timer.DelayCall(TimeSpan.FromSeconds(2), new TimerCallback(delegate 
                {
                    foreach (Mobile m in GetMobilesInRange(3))
                    {
                        if (m != this && m.Alive)
                        {
                            m.SendMessage("You are caught in the darkness left by Abbadon!");
                            m.Damage(10, this);
                            m.Freeze(TimeSpan.FromSeconds(2));
                        }
                    }
                }));
            }

            m_NextShadowSlip = DateTime.UtcNow + TimeSpan.FromMinutes(1);
        }

        private void AbyssalWave()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Abbadon unleashes an abyssal wave! *");
            PlaySound(0x20D);
            FixedEffect(0x376A, 10, 16);

            foreach (Mobile m in GetMobilesInRange(6))
            {
                if (m != this && m.Alive)
                {
                    int damage = Utility.RandomMinMax(15, 25);
                    m.Damage(damage, this);
                    m.SendMessage("You are hit by a wave of abyssal energy!");
                    m.SendMessage("You feel your movements hindered!");
                }
            }

            m_NextAbyssalWave = DateTime.UtcNow + TimeSpan.FromSeconds(40);
        }

        private void DarkPortal()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Abbadon summons abyssal minions! *");
            PlaySound(0x20D);
            FixedEffect(0x373A, 10, 16);

            for (int i = 0; i < 3; i++)
            {
                AbyssalMinion minion = new AbyssalMinion();
                Point3D spawnLocation = GetSpawnPosition(5);
                if (spawnLocation != Point3D.Zero)
                {
                    minion.MoveToWorld(spawnLocation, Map);
                }
            }

            m_NextDarkPortal = DateTime.UtcNow + TimeSpan.FromMinutes(1);
        }

        private void AbyssalShield()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Abbadon shields itself with abyssal energy! *");
            PlaySound(0x20D);
            FixedEffect(0x373A, 10, 16);

            this.VirtualArmor += 30;
            this.SendMessage("Abbadon's shield absorbs some of your attacks!");

            Timer.DelayCall(TimeSpan.FromSeconds(20), new TimerCallback(delegate
            {
                this.VirtualArmor -= 30;
                this.SendMessage("The abyssal shield fades away.");
            }));

            m_NextAbyssalShield = DateTime.UtcNow + TimeSpan.FromMinutes(2);
        }

        private Point3D GetSpawnPosition(int range)
        {
            int x = Utility.RandomMinMax(Location.X - range, Location.X + range);
            int y = Utility.RandomMinMax(Location.Y - range, Location.Y + range);
            int z = Map.GetAverageZ(x, y);
            Point3D newLocation = new Point3D(x, y, z);

            if (Map.CanSpawnMobile(newLocation))
            {
                return newLocation;
            }
            return Point3D.Zero;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_NextAbyssalDrain);
            writer.Write(m_NextVoidBurst);
            writer.Write(m_NextShadowSlip);
            writer.Write(m_NextAbyssalWave);
            writer.Write(m_NextDarkPortal);
            writer.Write(m_NextAbyssalShield);
            writer.Write(m_AbilitiesInitialized);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_NextAbyssalDrain = reader.ReadDateTime();
            m_NextVoidBurst = reader.ReadDateTime();
            m_NextShadowSlip = reader.ReadDateTime();
            m_NextAbyssalWave = reader.ReadDateTime();
            m_NextDarkPortal = reader.ReadDateTime();
            m_NextAbyssalShield = reader.ReadDateTime();
            m_AbilitiesInitialized = reader.ReadBool();
        }
    }

    public class AbyssalMinion : BaseCreature
    {
        [Constructable]
        public AbyssalMinion() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Abyssal Minion";
            Body = 4; // Gargoyle body
            Hue = 1154; // Dark hue for abyssal theme

            this.SetStr(150);
            this.SetDex(80);
            this.SetInt(50);

            this.SetHits(200);
            this.SetDamage(15, 25);

            this.SetDamageType(ResistanceType.Physical, 50);
            this.SetDamageType(ResistanceType.Energy, 50);

            this.SetResistance(ResistanceType.Physical, 40, 50);
            this.SetResistance(ResistanceType.Fire, 20, 30);
            this.SetResistance(ResistanceType.Cold, 20, 30);
            this.SetResistance(ResistanceType.Poison, 20, 30);
            this.SetResistance(ResistanceType.Energy, 40, 50);

            this.SetSkill(SkillName.EvalInt, 50.0);
            this.SetSkill(SkillName.Magery, 50.0);
            this.SetSkill(SkillName.MagicResist, 50.0);
            this.SetSkill(SkillName.Tactics, 40.0);
            this.SetSkill(SkillName.Wrestling, 40.0);

            this.Fame = 3000;
            this.Karma = -3000;

            this.VirtualArmor = 30;
        }

        public AbyssalMinion(Serial serial) : base(serial)
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
