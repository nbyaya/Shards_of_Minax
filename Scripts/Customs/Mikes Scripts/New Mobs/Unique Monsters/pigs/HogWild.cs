using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a hog wild corpse")]
    public class HogWild : BaseCreature
    {
        private DateTime m_NextWildFrenzy;
        private DateTime m_NextBellowingRoar;
        private DateTime m_WildFrenzyEnd;

        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public HogWild()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Hog Wild";
            Body = 0xCB; // Pig body
            Hue = 2191; // Light Brown hue
            BaseSoundID = 0xC4; // Pig sound

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

        public HogWild(Serial serial)
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

        public override int Meat { get { return 5; } }
        public override int Hides { get { return 8; } }
        public override FoodType FavoriteFood { get { return FoodType.Meat | FoodType.Fish; } }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    Random rand = new Random();
                    m_NextWildFrenzy = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextBellowingRoar = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextWildFrenzy && DateTime.UtcNow >= m_WildFrenzyEnd)
                {
                    WildFrenzy();
                }

                if (DateTime.UtcNow >= m_NextBellowingRoar)
                {
                    BellowingRoar();
                }

                if (DateTime.UtcNow >= m_WildFrenzyEnd && m_WildFrenzyEnd != DateTime.MinValue)
                {
                    EndWildFrenzy();
                }
            }
        }

        private void WildFrenzy()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Goes Hog Wild! *");
            PlaySound(0xC4); // Pig squeal

            SetDamage(DamageMin + 5, DamageMax + 5); // Increase damage
            SetDex(Dex + 30);

            m_WildFrenzyEnd = DateTime.UtcNow + TimeSpan.FromSeconds(15);
            m_NextWildFrenzy = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Set next ability time

            FixedEffect(0x376A, 10, 15);
        }

        private void EndWildFrenzy()
        {
            SetDamage(DamageMin - 5, DamageMax - 5); // Reset damage
            SetDex(Dex - 30);

            m_WildFrenzyEnd = DateTime.MinValue;
        }

        private void BellowingRoar()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Lets out a fearsome roar! *");
            PlaySound(0xA8); // Beast roar

            foreach (Mobile m in GetMobilesInRange(8))
            {
                if (m != this && m.Player && m.AccessLevel == AccessLevel.Player)
                {
                    m.SendLocalizedMessage(1005384); // The creature's reverberating roar fills you with terror!
                    m.Combatant = null;
                    m.SendLocalizedMessage(500130); // You flee in terror!
                    WalkMobile(m); // Use the new method
                }
            }

            m_NextBellowingRoar = DateTime.UtcNow + TimeSpan.FromSeconds(25); // Set next ability time
        }

        private void WalkMobile(Mobile m)
        {
            if (m != null && m.Alive)
            {
                // Move the mobile in a random direction
                Direction[] directions = { Direction.North, Direction.South, Direction.East, Direction.West };
                Direction direction = directions[Utility.Random(directions.Length)];

                // Move the mobile in the chosen direction
                m.Direction = direction;
                m.Move(direction);

                // Optionally, clear the combatant after a delay
                Timer.DelayCall(TimeSpan.FromSeconds(10), () =>
                {
                    m.Combatant = null;
                });
            }
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

            m_NextWildFrenzy = DateTime.UtcNow;
            m_NextBellowingRoar = DateTime.UtcNow;
            m_WildFrenzyEnd = DateTime.MinValue;
            m_AbilitiesInitialized = false; // Reset initialization flag
        }
    }
}
