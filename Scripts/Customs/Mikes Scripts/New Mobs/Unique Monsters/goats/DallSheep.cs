using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a dall sheep corpse")]
    public class DallSheep : BaseCreature
    {
        private DateTime m_NextHornBash;
        private DateTime m_NextSnowShield;
        private DateTime m_NextFrostBreath;
        private DateTime m_NextSummonSnowDrakes;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public DallSheep()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a Dall Sheep";
            Body = 0xD1; // Using goat body
            Hue = 1913; // Unique hue for Dall Sheep
			BaseSoundID = 0x99;

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

            m_AbilitiesInitialized = false; // Initialize the flag
        }

        public DallSheep(Serial serial)
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
                    m_NextHornBash = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextSnowShield = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextFrostBreath = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_NextSummonSnowDrakes = DateTime.UtcNow + TimeSpan.FromMinutes(rand.Next(1, 2)); // Random between 1 and 2 minutes
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextHornBash)
                {
                    HornBash();
                }

                if (DateTime.UtcNow >= m_NextSnowShield)
                {
                    SnowShield();
                }

                if (DateTime.UtcNow >= m_NextFrostBreath)
                {
                    FrostBreath();
                }

                if (DateTime.UtcNow >= m_NextSummonSnowDrakes)
                {
                    SummonSnowDrakes();
                }
            }
        }

        private void HornBash()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Dall Sheep slams its horns into its foe, causing a stunning impact!*");
            Effects.PlaySound(Location, Map, 0x657); // Sound effect for impact

            if (Combatant != null && Combatant.Alive)
            {
                Mobile mobileCombatant = Combatant as Mobile; // Cast Combatant to Mobile

                if (mobileCombatant != null)
                {
                    // Perform the Horn Bash effect (stun and knockback)
                    mobileCombatant.Freeze(TimeSpan.FromSeconds(2));
                    Point3D newLocation = new Point3D(mobileCombatant.X + Utility.RandomMinMax(-1, 1), mobileCombatant.Y + Utility.RandomMinMax(-1, 1), mobileCombatant.Z);
                    mobileCombatant.MoveToWorld(newLocation, Map);
                }
            }

            m_NextHornBash = DateTime.UtcNow + TimeSpan.FromSeconds(20); // Reset interval after use
        }

        private void SnowShield()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Dall Sheep summons a protective snow shield!*");
            Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration), 0x36BD, 10, 30, 1154); // Snow particles

            // Apply a temporary damage reduction effect
            // This is just a visual representation; actual damage reduction needs to be handled in your combat system
            Timer.DelayCall(TimeSpan.FromSeconds(10), new TimerCallback(RemoveSnowShield));
            m_NextSnowShield = DateTime.UtcNow + TimeSpan.FromSeconds(60); // Reset interval after use
        }

        private void RemoveSnowShield()
        {
            // Implement damage reduction removal logic here
        }

        private void FrostBreath()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Dall Sheep breathes out a chilling frost!*");
            Effects.PlaySound(Location, Map, 0x1FC); // Sound effect for frost breath
            Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration), 0x3B5, 10, 30, 1154); // Frost particles

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive)
                {
                    int damage = Utility.RandomMinMax(10, 20);
                    AOS.Damage(m, this, damage, 0, 100, 0, 0, 0);
                    m.SendMessage("You are hit by a blast of frost!");
                }
            }

            m_NextFrostBreath = DateTime.UtcNow + TimeSpan.FromSeconds(40); // Reset interval after use
        }

        private void SummonSnowDrakes()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Dall Sheep summons Snow Drakes to aid it!*");
            Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration), 0x3709, 10, 30, 1154); // Snow particles

            for (int i = 0; i < 2; i++)
            {
                SnowDrake drake = new SnowDrake();
                Point3D spawnLocation = new Point3D(X + Utility.RandomMinMax(-5, 5), Y + Utility.RandomMinMax(-5, 5), Z);
                drake.MoveToWorld(spawnLocation, Map);
                drake.Combatant = Combatant;
            }

            m_NextSummonSnowDrakes = DateTime.UtcNow + TimeSpan.FromMinutes(2); // Reset interval after use
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

            m_AbilitiesInitialized = false; // Reset initialization flag on deserialize
        }
    }

    public class SnowDrake : BaseCreature
    {
        [Constructable]
        public SnowDrake()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a Snow Drake";
            Body = 0xCF; // Snow Drake body
            Hue = 1154; // Unique hue for Snow Drake

            SetStr(80);
            SetDex(60);
            SetInt(30);

            SetHits(50);
            SetMana(0);

            SetDamage(8, 12);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Cold, 50);

            SetResistance(ResistanceType.Physical, 20, 30);
            SetResistance(ResistanceType.Cold, 40, 50);

            SetSkill(SkillName.MagicResist, 30.0, 50.0);
            SetSkill(SkillName.Tactics, 40.0, 60.0);
            SetSkill(SkillName.Wrestling, 40.0, 60.0);

            Fame = 200;
            Karma = 0;

            VirtualArmor = 20;

            Tamable = false;
        }

        public SnowDrake(Serial serial)
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
