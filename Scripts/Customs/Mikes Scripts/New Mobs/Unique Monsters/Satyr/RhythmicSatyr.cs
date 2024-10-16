using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a rhythmic satyr's corpse")]
    public class RhythmicSatyr : BaseCreature
    {
        private DateTime m_NextPercussiveBlast;
        private DateTime m_NextRhythmOfRecovery;
        private DateTime m_NextTempoShift;

        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public RhythmicSatyr()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a rhythmic satyr";
            Body = 271;
            Hue = 2298; // Unique hue for the Rhythmic Satyr
			this.BaseSoundID = 0x586;

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

        public RhythmicSatyr(Serial serial)
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
                    m_NextPercussiveBlast = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextRhythmOfRecovery = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_NextTempoShift = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextPercussiveBlast)
                {
                    PercussiveBlast();
                }

                if (DateTime.UtcNow >= m_NextRhythmOfRecovery)
                {
                    RhythmOfRecovery();
                }

                if (DateTime.UtcNow >= m_NextTempoShift)
                {
                    TempoShift();
                }
            }
        }

        private void PercussiveBlast()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Percussive Blast! *");
            FixedEffect(0x376A, 10, 16);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive)
                {
                    m.SendMessage("You are knocked back and stunned by the rhythmic shockwave!");

                    // Knockback effect
                    Point3D targetLocation = m.Location;
                    m.Location = new Point3D(targetLocation.X + Utility.RandomMinMax(-2, 2), targetLocation.Y + Utility.RandomMinMax(-2, 2), targetLocation.Z);

                    // Stun effect
                    m.Freeze(TimeSpan.FromSeconds(2));
                }
            }

            m_NextPercussiveBlast = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        private void RhythmOfRecovery()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Rhythm of Recovery *");
            FixedEffect(0x3779, 10, 16);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m is RhythmicSatyr && m.Alive)
                {
                    m.Hits = Math.Min(m.Hits + 10, m.HitsMax);
                }
            }

            m_NextRhythmOfRecovery = DateTime.UtcNow + TimeSpan.FromSeconds(40);
        }

        private void TempoShift()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Tempo Shift! *");
            FixedEffect(0x377A, 10, 16);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive)
                {
                    m.SendMessage("The tempo around you shifts, affecting your speed!");
                    m.Freeze(TimeSpan.FromSeconds(2)); // Simulates slowing down
                }
            }

            m_NextTempoShift = DateTime.UtcNow + TimeSpan.FromMinutes(2);
        }

        public override int Meat { get { return 1; } }
        public override int Hides { get { return 6; } }
        public override FoodType FavoriteFood { get { return FoodType.FruitsAndVegies; } }

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
}
