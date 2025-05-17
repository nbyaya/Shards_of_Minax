using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a luck dragon corpse")]
    public class LuckDragon : BaseCreature
    {
        // Cooldown timers for special abilities
        private DateTime m_NextGildedBreath;
        private DateTime m_NextFortuneBurst;
        private DateTime m_NextCoinStorm;
        private DateTime m_NextLuckyBeam;
        private DateTime m_NextJackpotRing;

        // Unique Hue for the Luck Dragon (Emerald Gold)
        private const int UniqueHue = 2126;

        [Constructable]
        public LuckDragon()
            : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.25, 0.45)
        {
            Name = "a luck dragon";
            Body = Utility.RandomList(12, 59);
            BaseSoundID = 362;
            Hue = UniqueHue;

            // Enhanced stats
            SetStr(1400, 1600);
            SetDex(150, 250);
            SetInt(900, 1100);

            SetHits(1200, 1400);

            SetDamage(40, 55);

            // Resistances
            SetResistance(ResistanceType.Physical, 75, 90);
            SetResistance(ResistanceType.Fire, 85, 100);
            SetResistance(ResistanceType.Cold, 70, 85);
            SetResistance(ResistanceType.Poison, 60, 75);
            SetResistance(ResistanceType.Energy, 65, 80);

            // Skills
            SetSkill(SkillName.EvalInt, 120.0, 140.0);
            SetSkill(SkillName.Magery, 120.0, 140.0);
            SetSkill(SkillName.Meditation, 80.0, 110.0);
            SetSkill(SkillName.MagicResist, 140.0, 180.0);
            SetSkill(SkillName.Tactics, 120.0, 140.0);
            SetSkill(SkillName.Wrestling, 120.0, 140.0);

            Fame = 35000;
            Karma = -35000;

            VirtualArmor = 90;

            // Initialize ability cooldowns
            DateTime now = DateTime.UtcNow;
            m_NextGildedBreath = now;
            m_NextFortuneBurst = now;
            m_NextCoinStorm = now;
            m_NextLuckyBeam = now;
            m_NextJackpotRing = now;
        }

        public LuckDragon(Serial serial)
            : base(serial)
        {
        }

        // --- Properties ---
        public override bool ReacquireOnMovement => true;
        public override bool AutoDispel => true;
        public override HideType HideType => HideType.Barbed;
        public override int Hides => 40;
        public override int Meat => 30;
        public override int Scales => 20;
        public override ScaleType ScaleType => (ScaleType)Utility.Random(4);
        public override Poison PoisonImmune => Poison.Lethal;
        public override Poison HitPoison => Utility.RandomBool() ? Poison.Deadly : Poison.Lethal;
        public override bool CanFly => true;
        public override int TreasureMapLevel => 7;

        // --- Loot ---
        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 5);
            AddLoot(LootPack.Gems, 10);

            if (Utility.RandomDouble() < 0.02) // 2% chance for a unique scale item
            {
                // PackItem(new LuckDragonScaleChestplate());
            }
            if (Utility.RandomDouble() < 0.05) // 5% chance for high‑level map
            {
                PackItem(new TreasureMap(7, Map));
            }
        }

        // --- Sounds ---
        public override int GetIdleSound() => 362;
        public override int GetHurtSound() => 362;
        public override int GetDeathSound() => 362;

        // --- Special Abilities ---

        // 1. Gilded Breath: a widening cone of golden dust that damages and spawns magnet tiles
        public void GildedBreathAttack()
        {
            var map = Map;
            if (map == null || !(Combatant is Mobile target)) return;
            if (!(target is PlayerMobile) && !(target is BaseCreature)) return;

            const int coneRange = 6;
            const int coneWidth = 4;
            const int damage = 35;

            // Play sound & effect at self
            Effects.PlaySound(Location, map, 0x4BD); // Coin clink
            Effects.SendLocationParticles(EffectItem.Create(Location, map, EffectItem.DefaultDuration),
                                          0x372A, coneWidth, coneRange * 2, UniqueHue, 0, 5021, 0);

            // Determine direction
            Direction d = GetDirectionTo(target);
            int dx = 0, dy = 0;
            Movement.Movement.Offset(d, ref dx, ref dy);

            var conePoints = new List<Point3D>();
            for (int i = 1; i <= coneRange; i++)
            {
                int spread = (int)(i * (coneWidth / (double)coneRange));
                for (int j = -spread; j <= spread; j++)
                {
                    int x = X + i * dx;
                    int y = Y + i * dy;
                    if (dx == 0) x += j;
                    else if (dy == 0) y += j;
                    else // Diagonal directions approximate with diamond shape
                    {
                        if (Math.Abs(j) + i > coneRange) continue;
                        x += j * (dx > 0 ? 1 : -1);
                        y += j * (dy > 0 ? 1 : -1);
                    }

                    var p = new Point3D(x, y, Z);
                    if (map.CanFit(p, 16, false, false))
                        conePoints.Add(p);
                    else
                    {
                        int z2 = map.GetAverageZ(x, y);
                        var p2 = new Point3D(x, y, z2);
                        if (map.CanFit(p2, 16, false, false))
                            conePoints.Add(p2);
                    }
                }
            }

            foreach (var p in conePoints)
            {
                Effects.SendLocationEffect(p, map, 0x3709, 10, UniqueHue, 0);
                var eable = map.GetMobilesInRange(p, 0);
                foreach (Mobile m in eable)
                {
                    if (m != this && (m is PlayerMobile || (m is BaseCreature bc && bc.Team != Team)))
                    {
                        DoHarmful(m);
                        AOS.Damage(m, this, damage, 0, 100, 0, 0, 0);
                    }
                }
                eable.Free();

                // 30% chance to spawn a magnet tile
                if (Utility.RandomDouble() < 0.3)
                {
                    // MagnetTile tile = new MagnetTile();
                    // tile.MoveToWorld(p, map);
                }
            }

            m_NextGildedBreath = DateTime.UtcNow + TimeSpan.FromSeconds(12);
        }

        // 2. Fortune Burst: radial burst that damages and spawns healing pulses around the dragon
        public void FortuneBurstAttack()
        {
            var map = Map;
            if (map == null) return;

            const int radius = 7;
            const int damage = 25;

            Effects.PlaySound(Location, map, 0x5A5); // Magical chime
            Effects.SendLocationParticles(EffectItem.Create(Location, map, EffectItem.DefaultDuration),
                                          0x37C4, 1, radius * 2, UniqueHue, 0, 9909, 0);

            var points = new List<Point3D>();
            for (int dx = -radius; dx <= radius; dx++)
            {
                for (int dy = -radius; dy <= radius; dy++)
                {
                    var p = new Point3D(X + dx, Y + dy, Z);
                    if (Utility.InRange(Location, p, radius) && map.CanFit(p, 16, false, false))
                        points.Add(p);
                }
            }

            foreach (var p in points)
            {
                Effects.SendLocationEffect(p, map, 0x37CB, 1, UniqueHue, 0);
                var eable = map.GetMobilesInRange(p, 0);
                foreach (Mobile m in eable)
                {
                    if (m != this && (m is PlayerMobile || (m is BaseCreature bc && bc.Team != Team)))
                    {
                        DoHarmful(m);
                        AOS.Damage(m, this, damage, 50, 0, 0, 50, 0);
                    }
                }
                eable.Free();

                // 20% chance to spawn a healing pulse tile
                if (Utility.RandomDouble() < 0.2)
                {
                    // HealingPulseTile heal = new HealingPulseTile();
                    // heal.MoveToWorld(p, map);
                }
            }

            m_NextFortuneBurst = DateTime.UtcNow + TimeSpan.FromSeconds(20);
        }

        // 3. Coin Storm: targeted ring around the combatant that rains coin‑like projectiles
        public void CoinStormAttack()
        {
            var map = Map;
            if (map == null || !(Combatant is Mobile target)) return;
            if (!(target is PlayerMobile) && !(target is BaseCreature)) return;

            const int radius = 5;
            const int damage = 50;

            Effects.PlaySound(target.Location, map, 0x4BD);
            Effects.SendLocationParticles(EffectItem.Create(target.Location, map, EffectItem.DefaultDuration),
                                          0x3760, radius, 20, UniqueHue, 0, 5052, 0);

            var points = new List<Point3D>();
            for (int dx = -radius; dx <= radius; dx++)
            {
                for (int dy = -radius; dy <= radius; dy++)
                {
                    var p = new Point3D(target.X + dx, target.Y + dy, target.Z);
                    if (Utility.InRange(target.Location, p, radius) && map.CanFit(p, 16, false, false))
                        points.Add(p);
                }
            }

            foreach (var p in points)
            {
                Effects.SendLocationParticles(EffectItem.Create(p, map, EffectItem.DefaultDuration),
                                              0x3761, 8, 30, UniqueHue, 0, 2023, 0);
                var eable = map.GetMobilesInRange(p, 0);
                foreach (Mobile m in eable)
                {
                    if (m != this && (m is PlayerMobile || (m is BaseCreature bc && bc.Team != Team)))
                    {
                        DoHarmful(m);
                        AOS.Damage(m, this, damage, 0, 0, 0, 0, 100);
                    }
                }
                eable.Free();

                // 25% chance to spawn a landmine tile
                if (Utility.RandomDouble() < 0.25)
                {
                    // LandmineTile mine = new LandmineTile();
                    // mine.MoveToWorld(p, map);
                }
            }

            m_NextCoinStorm = DateTime.UtcNow + TimeSpan.FromSeconds(15);
        }

        // 4. Lucky Beam: straight line energy beam
        public void LuckyBeamAttack()
        {
            var map = Map;
            if (map == null || !(Combatant is Mobile target)) return;
            if (!(target is PlayerMobile) && !(target is BaseCreature)) return;
            if (!Utility.InRange(Location, target.Location, 15)) return;

            const int beamLength = 15;
            const int damage = 45;

            Direction d = GetDirectionTo(target);
            int dx = 0, dy = 0;
            Movement.Movement.Offset(d, ref dx, ref dy);

            var beam = new List<Point3D>();
            for (int i = 1; i <= beamLength; i++)
            {
                var p = new Point3D(X + i * dx, Y + i * dy, Z);
                if (map.CanFit(p, 16, false, false))
                    beam.Add(p);
                else
                {
                    int z2 = map.GetAverageZ(p.X, p.Y);
                    var p2 = new Point3D(p.X, p.Y, z2);
                    if (map.CanFit(p2, 16, false, false))
                        beam.Add(p2);
                    else break;
                }
            }

            Effects.PlaySound(Location, map, 0x208);
            Effects.SendLocationParticles(EffectItem.Create(Location, map, EffectItem.DefaultDuration),
                                          0x3818, 10, 12, UniqueHue, 0, 2023, 0);

            foreach (var p in beam)
            {
                Effects.SendLocationEffect(p, map, 0x3818, 12, UniqueHue, 0);
                var eable = map.GetMobilesInRange(p, 0);
                foreach (Mobile m in eable)
                {
                    if (m != this && (m is PlayerMobile || (m is BaseCreature bc && bc.Team != Team)))
                    {
                        DoHarmful(m);
                        AOS.Damage(m, this, damage, 0, 0, 100, 0, 0);
                    }
                }
                eable.Free();
            }

            m_NextLuckyBeam = DateTime.UtcNow + TimeSpan.FromSeconds(8);
        }

        // 5. Jackpot Ring: ring of luck‑imbued traps around the dragon
        public void JackpotRingAttack()
        {
            var map = Map;
            if (map == null) return;

            const int ringDist = 4;
            const int damage = 30;

            Effects.PlaySound(Location, map, 0x26A);
            Effects.SendLocationParticles(EffectItem.Create(Location, map, EffectItem.DefaultDuration),
                                          0x3779, 8, 20, UniqueHue, 0, 9909, 0);

            // Eight points in a circle
			int[][] offsets = new int[][]
			{
				new int[] { ringDist, 0 },
				new int[] { ringDist, ringDist },
				new int[] { 0, ringDist },
				new int[] { -ringDist, ringDist },
				new int[] { -ringDist, 0 },
				new int[] { -ringDist, -ringDist },
				new int[] { 0, -ringDist },
				new int[] { ringDist, -ringDist }
			};


            foreach (var off in offsets)
            {
                int px = X + off[0], py = Y + off[1];
                var p = new Point3D(px, py, Z);
                if (!map.CanFit(p, 16, false, false))
                {
                    int z2 = map.GetAverageZ(px, py);
                    p = new Point3D(px, py, z2);
                }

                Effects.SendLocationParticles(EffectItem.Create(p, map, EffectItem.DefaultDuration),
                                              0x3709, 6, 12, UniqueHue, 0, 5016, 0);

                var eable = map.GetMobilesInRange(p, 0);
                foreach (Mobile m in eable)
                {
                    if (m != this && (m is PlayerMobile || (m is BaseCreature bc && bc.Team != Team)))
                    {
                        DoHarmful(m);
                        AOS.Damage(m, this, damage, 100, 0, 0, 0, 0);
                    }
                }
                eable.Free();

                // 30% chance to spawn a trapweb or vortex
                if (Utility.RandomDouble() < 0.3)
                {
                    if (Utility.RandomBool())
                    {
                        // TrapWeb web = new TrapWeb();
                        // web.MoveToWorld(p, map);
                    }
                    else
                    {
                        // VortexTile vortex = new VortexTile();
                        // vortex.MoveToWorld(p, map);
                    }
                }
            }

            m_NextJackpotRing = DateTime.UtcNow + TimeSpan.FromSeconds(18);
        }

        // --- AI Logic ---
        public override void OnThink()
        {
            base.OnThink();

            if (Combatant is Mobile target)
            {
                if (DateTime.UtcNow >= m_NextGildedBreath && this.InRange(target, 6))
                    GildedBreathAttack();
                else if (DateTime.UtcNow >= m_NextFortuneBurst)
                    FortuneBurstAttack();
                else if (DateTime.UtcNow >= m_NextCoinStorm && this.InRange(target, 8))
                    CoinStormAttack();
                else if (DateTime.UtcNow >= m_NextLuckyBeam && this.InRange(target, 15))
                    LuckyBeamAttack();
                else if (DateTime.UtcNow >= m_NextJackpotRing)
                    JackpotRingAttack();
            }
        }

        // --- OnDeath Explosion ---
        public override void OnDeath(Container c)
        {
            var map = Map;
            if (map != null)
            {
                const int shards = 30;
                const int radius = 6;

                Effects.PlaySound(Location, map, 0x214);
                Effects.SendLocationParticles(EffectItem.Create(Location, map, EffectItem.DefaultDuration),
                                              0x3709, 12, 60, UniqueHue, 0, 9909, 0);

                for (int i = 0; i < shards; i++)
                {
                    Point3D p = GetRandomValidLocation(Location, radius, map);
                    if (p == Point3D.Zero) continue;

                    // Randomly drop a ChaoticTeleportTile or MagnetTile
                    if (Utility.RandomBool())
                    {
                        // ChaoticTeleportTile tp = new ChaoticTeleportTile();
                        // tp.MoveToWorld(p, map);
                    }
                    else
                    {
                        // MagnetTile mag = new MagnetTile();
                        // mag.MoveToWorld(p, map);
                    }

                    Effects.SendLocationParticles(EffectItem.Create(p, map, EffectItem.DefaultDuration),
                                                  0x3709, 6, 20, UniqueHue, 0, 5016, 0);
                }
            }

            base.OnDeath(c);
        }

        // Helper: random valid point within radius
        private Point3D GetRandomValidLocation(Point3D center, int radius, Map map)
        {
            for (int i = 0; i < 10; i++)
            {
                int dx = Utility.RandomMinMax(-radius, radius);
                int dy = Utility.RandomMinMax(-radius, radius);
                var p = new Point3D(center.X + dx, center.Y + dy, center.Z);

                if (Utility.InRange(center, p, radius) && map.CanFit(p, 16, false, false))
                    return p;

                int z2 = map.GetAverageZ(p.X, p.Y);
                var p2 = new Point3D(p.X, p.Y, z2);
                if (Utility.InRange(center, p2, radius) && map.CanFit(p2, 16, false, false))
                    return p2;
            }
            return Point3D.Zero;
        }

        // --- Serialization ---
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
