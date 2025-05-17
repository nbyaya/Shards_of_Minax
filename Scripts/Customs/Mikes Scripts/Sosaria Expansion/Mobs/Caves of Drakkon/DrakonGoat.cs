using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a drakon goat corpse")]
    public class DrakonGoat : BaseCreature
    {
        // Cooldown timers for special abilities
        private DateTime m_NextTriangularBreath;
        private DateTime m_NextThunderousStomp;
        private DateTime m_NextAcidicMist;
        private DateTime m_NextSpiralCharge;
        private DateTime m_NextVortexGaze;

        // Unique emerald‑purple hue for the Drakon Goat’s effects
        private const int UniqueHue = 1157;

        [Constructable]
        public DrakonGoat()
            : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.3, 0.6)
        {
            Name           = "Drakon Goat";
            Body           = 88;      // Mountain goat body
            BaseSoundID    = 0x99;    // Mountain goat sounds
            Hue            = UniqueHue;

            // Enhanced attributes
            SetStr(500, 600);
            SetDex(200, 250);
            SetInt(300, 350);

            SetHits(1200, 1400);
            SetMana(800, 900);

            SetDamage(25, 35);

            // Resistances
            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire,     70, 80);
            SetResistance(ResistanceType.Cold,     40, 50);
            SetResistance(ResistanceType.Poison,   60, 70);
            SetResistance(ResistanceType.Energy,   55, 65);

            // Skills
            SetSkill(SkillName.Magery,      90.0, 110.0);
            SetSkill(SkillName.EvalInt,     85.0, 100.0);
            SetSkill(SkillName.MagicResist, 100.0,120.0);
            SetSkill(SkillName.Tactics,     80.0, 100.0);
            SetSkill(SkillName.Wrestling,   85.0, 105.0);

            Fame           = 20000;
            Karma          = -20000;
            VirtualArmor  = 70;

            // Initialize cooldowns
            m_NextTriangularBreath = DateTime.UtcNow;
            m_NextThunderousStomp   = DateTime.UtcNow;
            m_NextAcidicMist        = DateTime.UtcNow;
            m_NextSpiralCharge      = DateTime.UtcNow;
            m_NextVortexGaze        = DateTime.UtcNow;
        }

        public DrakonGoat(Serial serial)
            : base(serial)
        {
        }

        // --- Properties ---
        public override int Meat           => 4;
        public override int Hides          => 25;
        public override HideType HideType  => HideType.Barbed;
        public override FoodType FavoriteFood => FoodType.GrainsAndHay | FoodType.FruitsAndVegies;
        public override Poison PoisonImmune => Poison.Lethal;
        public override Poison HitPoison    => Poison.Deadly;

        public override bool ReacquireOnMovement => true;
        public override bool AutoDispel         => false;

        // --- Loot ---
        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich);
            AddLoot(LootPack.Gems, 5);

            if (Utility.RandomDouble() < 0.02) // 2% chance
                PackItem(new LandmineTile());  // rare trap tile
            if (Utility.RandomDouble() < 0.05)
                PackItem(new TreasureMap(5, Map));
        }

        // --- Sounds ---
        public override int GetIdleSound() => 0x99;
        public override int GetHurtSound() => 0x9A;
        public override int GetDeathSound() => 0x9B;

        // --- Special Abilities ---

        // 1. Triangular Breath: fan of three converging beams (triangle shape)
        public void TriangularBreath()
        {
            var map = Map; if (map == null || Combatant == null) return;
            if (!(Combatant is Mobile target)) return;

            // Only if in front cone
            if (!this.InRange(target, 12)) return;

            Effects.PlaySound(Location, map, 0x228);
            Effects.SendLocationParticles(EffectItem.Create(Location, map, EffectItem.DefaultDuration),
                                          0x3709, 15, 30, UniqueHue, 0, 2030, 0);

            // Fire three beams fanning out
            Direction dir = GetDirectionTo(target);
            var directions = new Direction[] {
                dir,
                (Direction)(((int)dir + 1) % 8),
                (Direction)(((int)dir + 7) % 8)
            };

            foreach (var d in directions)
            {
                int dx = 0, dy = 0;
                Movement.Movement.Offset(d, ref dx, ref dy);
                for (int i = 1; i <= 12; i++)
                {
                    var p = new Point3D(X + dx * i, Y + dy * i, Z);
                    if (!map.CanFit(p, 16, false, false)) break;

                    Effects.SendLocationEffect(p, map, 0x3818, 12, UniqueHue, 0);
                    var e = map.GetMobilesInRange(p, 0);
                    foreach (Mobile m in e)
                    {
                        if (m != this && (m is PlayerMobile || (m is BaseCreature bc && bc.Team != Team)))
                        {
                            DoHarmful(m);
                            AOS.Damage(m, this, 30, 0, 0, 0, 0, 100);
                        }
                    }
                    e.Free();
                }
            }

            m_NextTriangularBreath = DateTime.UtcNow + TimeSpan.FromSeconds(12);
        }

        // 2. Thunderous Stomp: circular shockwave
        public void ThunderousStomp()
        {
            var map = Map; if (map == null) return;

            Effects.PlaySound(Location, map, 0x2F3);
            Effects.SendLocationParticles(EffectItem.Create(Location, map, EffectItem.DefaultDuration),
                                          0x3709, 20, 50, UniqueHue, 0, 5040, 0);

            int radius = 6;
            for (int x = -radius; x <= radius; x++)
            for (int y = -radius; y <= radius; y++)
            {
                var p = new Point3D(X + x, Y + y, Z);
                if (!Utility.InRange(Location, p, radius)) continue;

                Effects.SendLocationEffect(p, map, 0x3709, 8, UniqueHue, 0);
                var e = map.GetMobilesInRange(p, 0);
                foreach (Mobile m in e)
                {
                    if (m != this && (m is PlayerMobile || (m is BaseCreature bc && bc.Team != Team)))
                    {
                        DoHarmful(m);
                        AOS.Damage(m, this, 35, 100, 0, 0, 0, 0);
                        m.Stam -= 20; // brief knock‑back fatigue
                    }
                }
                e.Free();
            }

            m_NextThunderousStomp = DateTime.UtcNow + TimeSpan.FromSeconds(18);
        }

        // 3. Acidic Mist: lingering poison cloud in a circle
        public void AcidicMist()
        {
            var map = Map; if (map == null) return;

            Effects.PlaySound(Location, map, 0x658);
            Effects.SendLocationParticles(EffectItem.Create(Location, map, EffectItem.DefaultDuration),
                                          0x36B0, 12, 12, UniqueHue, 0, 2023, 0);

            int radius = 4;
            for (int x = -radius; x <= radius; x++)
            for (int y = -radius; y <= radius; y++)
            {
                var p = new Point3D(X + x, Y + y, Z);
                if (!Utility.InRange(Location, p, radius)) continue;

                if (Utility.RandomDouble() < 0.4)
                {
                    // spawn a PoisonTile
                    // var t = new PoisonTile();
                    // t.MoveToWorld(p, map);
                }

                var e = map.GetMobilesInRange(p, 0);
                foreach (Mobile m in e)
                {
                    if (m != this && (m is PlayerMobile || (m is BaseCreature bc && bc.Team != Team)))
                    {
                        DoHarmful(m);
                        AOS.Damage(m, this, 20, 0, 0, 100, 0, 0);
                        m.ApplyPoison(this, Poison.Deadly);
                    }
                }
                e.Free();
            }

            m_NextAcidicMist = DateTime.UtcNow + TimeSpan.FromSeconds(15);
        }

        // 4. Spiral Horn Charge: line attack with rotating wave
        public void SpiralCharge()
        {
            var map = Map; if (map == null || Combatant == null) return;
            if (!(Combatant is Mobile target)) return;
            if (!this.InRange(target, 10)) return;

            Effects.PlaySound(Location, map, 0x213);
            Effects.SendLocationParticles(EffectItem.Create(Location, map, EffectItem.DefaultDuration),
                                          0x3728, 8, 20, UniqueHue, 0, 2035, 0);

            Direction dir = GetDirectionTo(target);
            int dx = 0, dy = 0;
            Movement.Movement.Offset(dir, ref dx, ref dy);

            var path = new List<Point3D>();
            for (int i = 1; i <= 10; i++)
            {
                var p = new Point3D(X + dx * i, Y + dy * i, Z);
                if (!map.CanFit(p, 16, false, false)) break;
                path.Add(p);
            }

            for (int i = 0; i < path.Count; i++)
            {
                var p = path[i];
                // rotating spark effect
                Effects.SendLocationParticles(null, 0x3818, 0, UniqueHue, 2030);

                var e = map.GetMobilesInRange(p, 0);
                foreach (Mobile m in e)
                {
                    if (m != this && (m is PlayerMobile || (m is BaseCreature bc && bc.Team != Team)))
                    {
                        DoHarmful(m);
                        AOS.Damage(m, this, 28, 0, 0, 0, 30, 70);
                    }
                }
                e.Free();
            }

            m_NextSpiralCharge = DateTime.UtcNow + TimeSpan.FromSeconds(10);
        }

        // 5. Vortex Gaze: forward cone that slows
        public void VortexGaze()
        {
            var map = Map; if (map == null || Combatant == null) return;
            if (!(Combatant is Mobile target)) return;
            if (!this.InRange(target, 8)) return;

            Effects.PlaySound(Location, map, 0x55A);
            Effects.SendLocationParticles(EffectItem.Create(Location, map, EffectItem.DefaultDuration),
                                          0x37C4, 12, 12, UniqueHue, 0, 9909, 0);

            Direction dir = GetDirectionTo(target);
            int range = 8, width = 3;
            int dx = 0, dy = 0;
            Movement.Movement.Offset(dir, ref dx, ref dy);

            var cone = new List<Point3D>();
            for (int i = 1; i <= range; i++)
            {
                for (int j = -width; j <= width; j++)
                {
                    int tx = X + dx * i + (dy == 0 ? j : 0);
                    int ty = Y + dy * i + (dx == 0 ? j : 0);
                    var p = new Point3D(tx, ty, Z);
                    if (map.CanFit(p, 16, false, false) && Utility.InRange(Location, p, range))
                        cone.Add(p);
                }
            }

            foreach (var p in cone)
            {
                Effects.SendLocationParticles(EffectItem.Create(p, map, EffectItem.DefaultDuration),
                                              0x36B0, 8, 8, UniqueHue, 0, 2025, 0);

                var e = map.GetMobilesInRange(p, 0);
                foreach (Mobile m in e)
                {
                    if (m != this && (m is PlayerMobile || (m is BaseCreature bc && bc.Team != Team)))
                    {
                        DoHarmful(m);
                        AOS.Damage(m, this, 22, 0, 0, 0, 50, 50);
                        m.Stam -= 30; // slow effect
                    }
                }
                e.Free();
            }

            m_NextVortexGaze = DateTime.UtcNow + TimeSpan.FromSeconds(14);
        }

        // --- AI Logic ---
        public override void OnThink()
        {
            base.OnThink();

            if (Combatant is Mobile target)
            {
                if (DateTime.UtcNow >= m_NextTriangularBreath && this.InRange(target, 12))
                    TriangularBreath();
                else if (DateTime.UtcNow >= m_NextThunderousStomp)
                    ThunderousStomp();
                else if (DateTime.UtcNow >= m_NextAcidicMist)
                    AcidicMist();
                else if (DateTime.UtcNow >= m_NextSpiralCharge && this.InRange(target, 10))
                    SpiralCharge();
                else if (DateTime.UtcNow >= m_NextVortexGaze && this.InRange(target, 8))
                    VortexGaze();
            }
        }

        // --- Death Explosion ---
        public override void OnDeath(Container c)
        {
            if (Map != null)
            {
                Effects.PlaySound(Location, Map, 0x214);
                Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                                              0x3709, 15, 40, UniqueHue, 0, 5052, 0);

                // scatter hazard tiles
                for (int i = 0; i < 15; i++)
                {
                    var p = GetRandomValidLocation(Location, 6, Map);
                    if (p != Point3D.Zero)
                    {
                        // var tile = new FlamestrikeHazardTile();
                        // tile.MoveToWorld(p, Map);
                        Effects.SendLocationParticles(EffectItem.Create(p, Map, EffectItem.DefaultDuration),
                                                      0x3709, 8, 20, UniqueHue, 0, 5016, 0);
                    }
                }
            }

            base.OnDeath(c);
        }

        // Helper for OnDeath
        private Point3D GetRandomValidLocation(Point3D center, int radius, Map map)
        {
            for (int i = 0; i < 10; i++)
            {
                int x = Utility.RandomMinMax(-radius, radius);
                int y = Utility.RandomMinMax(-radius, radius);
                var p = new Point3D(center.X + x, center.Y + y, center.Z);
                if (map.CanFit(p, 16, false, false) && Utility.InRange(center, p, radius))
                    return p;
                int z = map.GetAverageZ(p.X, p.Y);
                var p2 = new Point3D(p.X, p.Y, z);
                if (map.CanFit(p2, 16, false, false) && Utility.InRange(center, p2, radius))
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
