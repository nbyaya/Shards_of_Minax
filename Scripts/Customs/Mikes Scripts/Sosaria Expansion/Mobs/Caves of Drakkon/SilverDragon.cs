using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells; // For SpellHelper, if needed

namespace Server.Mobiles
{
    [CorpseName("a silver dragon corpse")]
    public class SilverDragon : BaseCreature
    {
        // Cooldown timers for special abilities
        private DateTime m_NextRadiantBeam;
        private DateTime m_NextShimmeringBreath;
        private DateTime m_NextGlacialRing;
        private DateTime m_NextMirrorWard;
        private DateTime m_NextLightningCascade;

        // Unique silver hue
        private const int UniqueHue = 1150;

        [Constructable]
        public SilverDragon()
            : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.3, 0.5)
        {
            Name = "a silver dragon";
            Body = Utility.RandomList(12, 59);
            BaseSoundID = 362;
            Hue = UniqueHue;

            // Enhanced stats
            SetStr(1500, 1800);
            SetDex(150, 200);
            SetInt(800, 1000);

            SetHits(2000, 3000);
            SetDamage(40, 55);

            // Damage types
            SetDamageType(ResistanceType.Physical, 40);
            SetDamageType(ResistanceType.Cold, 60);
            SetDamageType(ResistanceType.Energy, 40);

            // Resistances
            SetResistance(ResistanceType.Physical, 80, 95);
            SetResistance(ResistanceType.Fire, 75, 90);
            SetResistance(ResistanceType.Cold, 75, 90);
            SetResistance(ResistanceType.Poison, 60, 80);
            SetResistance(ResistanceType.Energy, 70, 85);

            // Skills
            SetSkill(SkillName.EvalInt, 120.0, 150.0);
            SetSkill(SkillName.Magery, 120.0, 150.0);
            SetSkill(SkillName.Meditation, 75.0, 100.0);
            SetSkill(SkillName.MagicResist, 140.0, 180.0);
            SetSkill(SkillName.Tactics, 120.0, 140.0);
            SetSkill(SkillName.Wrestling, 120.0, 140.0);

            Fame = 35000;
            Karma = -35000;
            VirtualArmor = 100;

            // Not tamable by players
            Tamable = false;
            ControlSlots = 6;
            MinTameSkill = 120.0;

            // Initialize cooldowns
            m_NextRadiantBeam      = DateTime.UtcNow;
            m_NextShimmeringBreath = DateTime.UtcNow;
            m_NextGlacialRing      = DateTime.UtcNow;
            m_NextMirrorWard       = DateTime.UtcNow;
            m_NextLightningCascade = DateTime.UtcNow;
        }

        public SilverDragon(Serial serial) : base(serial) { }

        // Override basic properties
        public override bool ReacquireOnMovement => true;
        public override bool AutoDispel        => true;
        public override bool CanFly            => true;

        public override int TreasureMapLevel => 6;
        public override int Meat             => 25;
        public override int Hides            => 50;
        public override HideType HideType    => HideType.Barbed;
        public override int Scales           => 15;
        public override ScaleType ScaleType  => (ScaleType)Utility.Random(4);
        public override Poison PoisonImmune  => Poison.Lethal;
        public override Poison HitPoison     => Utility.RandomBool() ? Poison.Deadly : Poison.Lethal;

        // Loot
        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 4);
            AddLoot(LootPack.Gems,       8);

            if (Utility.RandomDouble() < 0.02) // 2% for unique item
                PackItem(new Diamond(10));     // Example special drop

            if (Utility.RandomDouble() < 0.05) // 5% for high‐level map
                PackItem(new TreasureMap(6, Map));
        }

        // --- Special Abilities ---

        // 1) Radiant Beam: Long line AOE of energy
        public void RadiantBeamAttack()
        {
            Map map = Map;
            if (map == null || Combatant == null) return;
            if (!(Combatant is Mobile target)) return;

            const int range   = 16;
            const int damage  = 50;
            const int soundID = 0x5C3;

            if (!Utility.InRange(Location, target.Location, range))
                return;

            // Direction vector
            Direction d = GetDirectionTo(target);
            int dx = 0, dy = 0;
            Movement.Movement.Offset(d, ref dx, ref dy);

            // Play start effect
            Effects.PlaySound(Location, map, soundID);
            Effects.SendLocationParticles(EffectItem.Create(Location, map, EffectItem.DefaultDuration),
                                          0x3709, 8, 20, UniqueHue, 0, 0, 0);

            // Fire beam
            for (int i = 1; i <= range; i++)
            {
                Point3D p = new Point3D(X + dx * i, Y + dy * i, Z);
                if (!map.CanFit(p, 16, false, false)) break;

                Effects.SendLocationEffect(p, map, 0x3818, 16, UniqueHue, 0);
                foreach (Mobile m in map.GetMobilesInRange(p, 0))
                {
                    if (m == this) continue;
                    if (m is PlayerMobile || (m is BaseCreature bc && bc.Team != Team))
                    {
                        DoHarmful(m);
                        AOS.Damage(m, this, damage, 0, 0, 0, 0, 100);
                    }
                }
            }

            m_NextRadiantBeam = DateTime.UtcNow + TimeSpan.FromSeconds(12);
        }

        // 2) Shimmering Frost Breath: Cone of chilling cold + IceShardTiles
        public void ShimmeringFrostBreath()
        {
            Map map = Map;
            if (map == null || Combatant == null) return;
            if (!(Combatant is Mobile target)) return;

            const int coneRange  = 8;
            const int coneWidth  = 4;
            const int damage     = 30;

            // Play breath effect
            Effects.PlaySound(Location, map, BaseSoundID);
            Effects.SendLocationParticles(EffectItem.Create(Location, map, EffectItem.DefaultDuration),
                                          0x36B0, 12, 15, UniqueHue, 0, 5057, 0);

            Direction d = GetDirectionTo(target);
            int dx = 0, dy = 0;
            Movement.Movement.Offset(d, ref dx, ref dy);

            var coneLocations = new List<Point3D>();
            for (int i = 1; i <= coneRange; i++)
            {
                int widthAtI = (int)(coneWidth * (i / (double)coneRange));
                for (int offset = -widthAtI; offset <= widthAtI; offset++)
                {
                    int tx = X + dx * i;
                    int ty = Y + dy * i;

                    // Adjust for cardinal vs diagonal
                    if (dx == 0)       tx += offset;
                    else if (dy == 0)  ty += offset;
                    else               { tx += offset; ty += offset; }

                    var p = new Point3D(tx, ty, Z);
                    if (map.CanFit(p, 16, false, false))
                        coneLocations.Add(p);
                }
            }

            foreach (var p in coneLocations)
            {
                Effects.SendLocationParticles(EffectItem.Create(p, map, EffectItem.DefaultDuration),
                                              0x3728, 8, 10, UniqueHue, 0, 5057, 0);

                foreach (Mobile m in map.GetMobilesInRange(p, 0))
                {
                    if (m == this) continue;
                    if (m is PlayerMobile || (m is BaseCreature bc && bc.Team != Team))
                    {
                        DoHarmful(m);
                        AOS.Damage(m, this, damage, 0, 0, 100, 0, 0);
                    }
                }

                // 30% chance to leave a shard tile
                if (Utility.RandomDouble() < 0.3)
                {
                    // IceShardTile shard = new IceShardTile();
                    // shard.MoveToWorld(p, map);
                }
            }

            m_NextShimmeringBreath = DateTime.UtcNow + TimeSpan.FromSeconds(14);
        }

        // 3) Glacial Ring: Radial ice ring around self
        public void GlacialRingAttack()
        {
            Map map = Map;
            if (map == null) return;

            const int radius = 6;
            const int damage = 35;

            Effects.PlaySound(Location, map, 0x5C5);
            Effects.SendLocationParticles(EffectItem.Create(Location, map, EffectItem.DefaultDuration),
                                          0x3709, 10, 25, UniqueHue, 0, 0, 0);

            for (int x = -radius; x <= radius; x++)
            {
                for (int y = -radius; y <= radius; y++)
                {
                    var p = new Point3D(X + x, Y + y, Z);
                    if (!Utility.InRange(Location, p, radius)) continue;
                    if (!map.CanFit(p, 16, false, false)) continue;

                    Effects.SendLocationEffect(p, map, 0x37C4, 12, UniqueHue, 0);
                    foreach (Mobile m in map.GetMobilesInRange(p, 0))
                    {
                        if (m == this) continue;
                        if (m is PlayerMobile || (m is BaseCreature bc && bc.Team != Team))
                        {
                            DoHarmful(m);
                            AOS.Damage(m, this, damage, 0, 100, 0, 0, 0);
                        }
                    }
                }
            }

            m_NextGlacialRing = DateTime.UtcNow + TimeSpan.FromSeconds(18);
        }

        // 4) Mirror Ward: Protective aura that drains mana of intruders
        public void MirrorWardAbility()
        {
            Map map = Map;
            if (map == null) return;

            const int radius = 5;

            Effects.PlaySound(Location, map, 0x1F2);
            Effects.SendLocationParticles(EffectItem.Create(Location, map, EffectItem.DefaultDuration),
                                          0x37CB, 1, 12, UniqueHue, 0, 9909, 0);

            foreach (Mobile m in map.GetMobilesInRange(Location, radius))
            {
                if (m == this) continue;
                if (m is PlayerMobile || (m is BaseCreature bc && bc.Team != Team))
                {
                    // Drain a bit of mana
                    int drained = Math.Min(m.Mana, 50);
                    m.Mana -= drained;
                    m.SendMessage($"Your mind reels as silver light drains {drained} mana!");
                }
            }

            m_NextMirrorWard = DateTime.UtcNow + TimeSpan.FromSeconds(20);
        }

		public static Mobile GetNextMob(Mobile from, List<Mobile> exclude, Map map, int range)
		{
			if (from == null || map == null)
				return null;

			Mobile closest = null;
			double closestDist = double.MaxValue;

			foreach (Mobile m in from.GetMobilesInRange(range))
			{
				if (m == from || exclude.Contains(m) || !from.CanBeHarmful(m) || !from.InLOS(m))
					continue;

				double dist = from.GetDistanceToSqrt(m);
				if (dist < closestDist)
				{
					closest = m;
					closestDist = dist;
				}
			}

			return closest;
		}


        // 5) Lightning Cascade: Chain lightning jumping between targets
		public void LightningCascadeAttack()
		{
			Map map = Map;
			if (map == null || Combatant == null) return;
			if (!(Combatant is Mobile start)) return;

			const int maxJumps = 4;
			const int damage   = 45;

			Effects.PlaySound(start.Location, map, 0x1FE);
			Effects.SendLocationParticles(EffectItem.Create(start.Location, map, EffectItem.DefaultDuration),
										  0x3779, 10, 15, UniqueHue, 0, 0, 0);

			var victims = new List<Mobile> { start };
			Mobile current = start;

			for (int i = 0; i < maxJumps; i++)
			{
				Mobile next = GetNextMob(current, victims, map, 8); // Use your new helper method
				if (next == null) break;

				Effects.PlaySound(next.Location, map, 0x1F7);
				Effects.SendMovingParticles(current, next, 0x3779, 5, 0, false, false, UniqueHue, 0, 9502, 0);

				DoHarmful(next);
				AOS.Damage(next, this, damage, 0, 0, 0, 0, 100);

				victims.Add(next);
				current = next;
			}

			m_NextLightningCascade = DateTime.UtcNow + TimeSpan.FromSeconds(16);
		}


        // AI: Choose which ability to use
        public override void OnThink()
        {
            base.OnThink();

            if (Combatant is Mobile target)
            {
                if (DateTime.UtcNow >= m_NextRadiantBeam && InRange(target, 16))
                    RadiantBeamAttack();
                else if (DateTime.UtcNow >= m_NextShimmeringBreath && InRange(target, 8))
                    ShimmeringFrostBreath();
                else if (DateTime.UtcNow >= m_NextGlacialRing)
                    GlacialRingAttack();
                else if (DateTime.UtcNow >= m_NextLightningCascade && InRange(target, 10))
                    LightningCascadeAttack();
                else if (DateTime.UtcNow >= m_NextMirrorWard)
                    MirrorWardAbility();
            }
        }

        // Extra effect on death
        public override void OnDeath(Container c)
        {
            if (Map != null)
            {
                Effects.PlaySound(Location, Map, 0x2F5);
                Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                                              0x3709, 12, 40, UniqueHue, 0, 5016, 0);

                for (int i = 0; i < 15; i++)
                {
                    Point3D p = GetRandomValidLocation(Location, 6, Map);
                    if (p == Point3D.Zero) continue;

                    // Spawn some shard and toxic tiles
                    // IceShardTile shard = new IceShardTile();
                    // shard.MoveToWorld(p, Map);

                    if (Utility.RandomBool())
                    {
                        // PoisonTile pt = new PoisonTile();
                        // pt.MoveToWorld(p, Map);
                    }
                }
            }

            base.OnDeath(c);
        }

        // Helper to find valid points
        private Point3D GetRandomValidLocation(Point3D center, int radius, Map map)
        {
            for (int i = 0; i < 10; i++)
            {
                int x = center.X + Utility.RandomMinMax(-radius, radius);
                int y = center.Y + Utility.RandomMinMax(-radius, radius);
                int z = map.GetAverageZ(x, y);
                var p = new Point3D(x, y, z);

                if (map.CanFit(p, 16, false, false) && Utility.InRange(center, p, radius))
                    return p;
            }
            return Point3D.Zero;
        }

        // Serialization
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
