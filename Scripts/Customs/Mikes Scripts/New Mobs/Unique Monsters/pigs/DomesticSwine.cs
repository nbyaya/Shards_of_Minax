using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a domestic swine corpse")]
    public class DomesticSwine : BaseCreature
    {
        private DateTime m_NextTacticalRetreat;
        private DateTime m_NextDefensiveStomp;
        private bool m_IsRetreating;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public DomesticSwine()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a domestic swine";
            Body = 0xCB; // Pig body
            Hue = 2201; // White hue
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

        public DomesticSwine(Serial serial)
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

        public override int Meat { get { return 3; } }
        public override FoodType FavoriteFood { get { return FoodType.FruitsAndVegies | FoodType.GrainsAndHay; } }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    Random rand = new Random();
                    m_NextTacticalRetreat = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextDefensiveStomp = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextTacticalRetreat && !m_IsRetreating)
                {
                    TacticalRetreat();
                }

                if (DateTime.UtcNow >= m_NextDefensiveStomp)
                {
                    DefensiveStomp();
                }
            }

            if (m_IsRetreating && DateTime.UtcNow >= m_NextTacticalRetreat)
            {
                m_IsRetreating = false;
            }
        }

        private void TacticalRetreat()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Squeals and retreats! *");
            PlaySound(0xC4); // Pig squeal

            m_IsRetreating = true;
            ActiveSpeed = 0.2;
            PassiveSpeed = 0.4;

            if (Combatant != null)
            {
                Direction = (Direction)((int)GetDirectionTo(Combatant.Location) + 4 % 8);
            }

            Timer.DelayCall(TimeSpan.FromSeconds(5), () =>
            {
                ActiveSpeed = 0.2;
                PassiveSpeed = 0.4;
            });

            m_NextTacticalRetreat = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown for Tactical Retreat
        }

        private void DefensiveStomp()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Stomps the ground! *");
            PlaySound(0x2F4); // Stomp sound
            FixedEffect(0x376A, 10, 15);

            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m.Alive && CanBeHarmful(m))
                {
                    DoHarmful(m);
                    m.Damage(Utility.RandomMinMax(10, 20), this);
                    m.SendLocalizedMessage(1072221); // The force of the stomp slams you back.

                    Point3D knockbackLocation = GetKnockbackLocation(m);
                    m.MoveToWorld(knockbackLocation, m.Map);
                }
            }

            m_NextDefensiveStomp = DateTime.UtcNow + TimeSpan.FromSeconds(20); // Cooldown for Defensive Stomp
        }

        private Point3D GetKnockbackLocation(Mobile m)
        {
            int x = m.X;
            int y = m.Y;
            int z = m.Z;

            Direction d = GetDirectionTo(m);

            switch (d)
            {
                case Direction.North:
                    y -= 2;
                    break;
                case Direction.South:
                    y += 2;
                    break;
                case Direction.East:
                    x += 2;
                    break;
                case Direction.West:
                    x -= 2;
                    break;
                case Direction.Up:
                    x -= 2;
                    y -= 2;
                    break;
                case Direction.Down:
                    x += 2;
                    y += 2;
                    break;
                case Direction.Left:
                    x -= 2;
                    y += 2;
                    break;
                case Direction.Right:
                    x += 2;
                    y -= 2;
                    break;
            }

            return new Point3D(x, y, z);
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

            m_AbilitiesInitialized = false; // Reset the initialization flag
        }
    }
}