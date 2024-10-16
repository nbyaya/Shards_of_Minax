using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a blood dragon corpse")]
    public class BloodDragon : BaseCreature
    {
        private DateTime m_NextBreathTime;
        private DateTime m_NextBleedTime;
        private bool m_AbilitiesInitialized;
        private static readonly int HueValue = 1487; // Blood red hue

        [Constructable]
        public BloodDragon() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a blood dragon";
            Body = 49; // Dragon body
            BaseSoundID = 362;
            Hue = HueValue;

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

        public BloodDragon(Serial serial) : base(serial)
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
                    m_NextBreathTime = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(5, 20));
                    m_NextBleedTime = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(10, 30));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextBreathTime)
                {
                    BreathSpecialAttack();
                    m_NextBreathTime = DateTime.UtcNow + TimeSpan.FromSeconds(30 + Utility.RandomDouble() * 30); // Random cooldown between 30 and 60 seconds
                }

                if (DateTime.UtcNow >= m_NextBleedTime && Utility.RandomDouble() < 0.15)
                {
                    InflictBleed(Combatant as Mobile);
                    m_NextBleedTime = DateTime.UtcNow + TimeSpan.FromSeconds(15 + Utility.RandomDouble() * 15); // Random cooldown between 15 and 30 seconds
                }
            }
        }

        private void BreathSpecialAttack()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Blood Dragon unleashes a massive blood breath! *");

            Map map = this.Map;
            if (map == null)
                return;

            Direction d = this.Direction;
            int range = 5;
            int damage = Utility.RandomMinMax(40, 50);
            List<Point3D> targets = new List<Point3D>();

            // Calculate the cone area for the breath attack
            int dx = 0, dy = 0;
            switch (d & Direction.Mask)
            {
                case Direction.North:
                    dy = -1;
                    break;
                case Direction.East:
                    dx = 1;
                    break;
                case Direction.South:
                    dy = 1;
                    break;
                case Direction.West:
                    dx = -1;
                    break;
                case Direction.Right: // North-East
                    dx = 1;
                    dy = -1;
                    break;
                case Direction.Down: // South-East
                    dx = 1;
                    dy = 1;
                    break;
                case Direction.Left: // South-West
                    dx = -1;
                    dy = 1;
                    break;
                case Direction.Up: // North-West
                    dx = -1;
                    dy = -1;
                    break;
            }

            for (int i = 1; i <= range; i++)
            {
                int perpendicularRange = i;
                int baseX = this.X + i * dx;
                int baseY = this.Y + i * dy;

                for (int j = -perpendicularRange; j <= perpendicularRange; j++)
                {
                    int targetX = baseX;
                    int targetY = baseY;

                    if (Math.Abs(dx) != Math.Abs(dy))
                    {
                        // Handling cardinal directions (North, East, South, West)
                        if (dx == 0)
                        {
                            targetX += j;
                        }
                        else if (dy == 0)
                        {
                            targetY += j;
                        }
                    }
                    else
                    {
                        // Handling intercardinal directions (NE, SE, SW, NW)
                        if (dx * dy > 0) // Moving in SE or NW direction
                        {
                            targetX += j;
                            targetY -= j; // Invert the increment for one axis to spread perpendicularly
                        }
                        else // Moving in NE or SW direction
                        {
                            targetX += j;
                            targetY += j; // Both axes increment in the same direction for NE and SW
                        }
                    }

                    if (map.CanFit(targetX, targetY, this.Z, 16, false, false))
                        targets.Add(new Point3D(targetX, targetY, this.Z));
                    else
                    {
                        int targetZ = map.GetAverageZ(targetX, targetY);
                        if (map.CanFit(targetX, targetY, targetZ, 16, false, false))
                            targets.Add(new Point3D(targetX, targetY, targetZ));
                    }
                }
            }

            foreach (Point3D p in targets)
            {
                Effects.SendLocationEffect(p, map, 0x36BD, 16, 10, HueValue, 0);
                IPooledEnumerable eable = map.GetMobilesInRange(p, 0);
                foreach (Mobile m in eable)
                {
                    if (m != this)
                    {
                        m.SendMessage("You are hit by the Blood Dragon's breath attack!");
                        m.Damage(damage, this);
                    }
                }
                eable.Free();
            }
        }

        private void InflictBleed(Mobile target)
        {
            if (target == null || !target.Alive)
                return;

            int damage = Utility.RandomMinMax(40, 50);
            target.SendMessage("You are bleeding from the Blood Dragon's attack!");
            target.FixedEffect(0x376A, 10, 16);
            target.Damage(damage, this);

            Timer.DelayCall(TimeSpan.FromSeconds(1), () =>
            {
                if (target != null && target.Alive)
                {
                    target.Damage(damage / 2, this);
                    target.SendMessage("You continue to bleed!");
                }
            });
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
            m_AbilitiesInitialized = false; // Reset flag on deserialize
        }
    }
}
