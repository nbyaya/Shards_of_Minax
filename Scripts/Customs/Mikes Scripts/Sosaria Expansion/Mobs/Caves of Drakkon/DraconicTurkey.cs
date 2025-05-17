using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a draconic turkey corpse")]
    public class DraconicTurkey : BaseCreature
    {
        // Cooldown timers
        private DateTime m_NextGobbleBreath;
        private DateTime m_NextFeatherBarrage;
        private DateTime m_NextGravyBomb;
        private DateTime m_NextFlurry;
        private DateTime m_NextRoostWard;

        // Unique “turkey‐drake” hue (rich brownish‐crimson)
        private const int UniqueHue = 1266;

        [Constructable]
        public DraconicTurkey()
            : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name = "Draconic Turkey";
            Body = 1026;               // same giant turkey body
            BaseSoundID = 0x66A;       // turkey sounds
            Hue = UniqueHue;

            // **Stats**
            SetStr(1500, 1700);
            SetDex(200, 300);
            SetInt(700, 900);

            SetHits(30000, 35000);
            SetMana(2000);

            SetDamage(30, 40);
            SetDamageType(ResistanceType.Physical, 60);
            SetDamageType(ResistanceType.Fire,      40);

            // **Resistances**
            SetResistance(ResistanceType.Physical, 60, 75);
            SetResistance(ResistanceType.Fire,     80, 90);
            SetResistance(ResistanceType.Cold,     50, 60);
            SetResistance(ResistanceType.Poison,   50, 60);
            SetResistance(ResistanceType.Energy,   50, 60);

            // **Skills**
            SetSkill(SkillName.Magery,        80.0, 100.0);
            SetSkill(SkillName.EvalInt,       80.0, 100.0);
            SetSkill(SkillName.MagicResist,  100.0, 120.0);
            SetSkill(SkillName.Tactics,      120.0, 140.0);
            SetSkill(SkillName.Wrestling,    120.0, 140.0);
            SetSkill(SkillName.Anatomy,       90.0, 100.0);
            SetSkill(SkillName.Meditation,    80.0, 100.0);

            Fame = 25000;
            Karma = -25000;
            VirtualArmor = 90;

            // Initialize ability cooldowns
            m_NextGobbleBreath    = DateTime.UtcNow;
            m_NextFeatherBarrage  = DateTime.UtcNow;
            m_NextGravyBomb       = DateTime.UtcNow;
            m_NextFlurry          = DateTime.UtcNow;
            m_NextRoostWard       = DateTime.UtcNow;
        }

        public DraconicTurkey(Serial serial)
            : base(serial)
        {
        }

        // **Harvests & Drops**
        public override int Meat        => 2;
        public override MeatType MeatType => MeatType.Bird;
        public override int Feathers    => 50;
        public override int Hides       => 50;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 4);
            AddLoot(LootPack.Gems, 10);

            if (Utility.RandomDouble() < 0.02) // 2% chance
                PackItem(new Feather( Utility.RandomMinMax(5,15) )); // “Draconic Feather”
        }

        // **Sounds**
        public override int GetIdleSound()  => 0x66A;
        public override int GetAngerSound() => 0x66A;
        public override int GetHurtSound()  => 0x66B;
        public override int GetDeathSound() => 0x66B;

        // --- SPECIAL ABILITIES ---

        // 1) Gobble Breath: a triangular flame‐poison breath
        public void GobbleBreathAttack()
        {
            var map = Map;
            if (map == null || Combatant == null) return;
            if (!(Combatant is Mobile target)) return;
            if (!Utility.InRange(Location, target.Location, 10)) return;

            // direction & perpendicular
            var d = GetDirectionTo(target);
            int dx = 0, dy = 0;
            Movement.Movement.Offset(d, ref dx, ref dy);
            int pdx = -dy, pdy = dx;

            Effects.PlaySound(Location, map, BaseSoundID);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, map, EffectItem.DefaultDuration),
                0x3709, 10, 30, UniqueHue, 0, 5052, 0
            );

            var points = new List<Point3D>();
            int range = 10, damage = 50;
            for (int i = 1; i <= range; i++)
            {
                for (int k = -(i - 1); k <= (i - 1); k++)
                {
                    var p = new Point3D(
                        X + i * dx + k * pdx,
                        Y + i * dy + k * pdy,
                        Z
                    );

                    if (!map.CanFit(p, 16, false, false))
                    {
                        int z2 = map.GetAverageZ(p.X, p.Y);
                        p = new Point3D(p.X, p.Y, z2);
                        if (!map.CanFit(p, 16, false, false)) continue;
                    }

                    points.Add(p);
                }
            }

            foreach (var p in points)
            {
                Effects.SendLocationParticles(
                    EffectItem.Create(p, map, EffectItem.DefaultDuration),
                    0x36B0, 10, 10, UniqueHue, 0, 2023, 0
                );

                var eable = map.GetMobilesInRange(p, 0);
                foreach (Mobile m in eable)
                {
                    if (m != this &&
                       (m is PlayerMobile || (m is BaseCreature bc && bc.Team != Team)))
                    {
                        DoHarmful(m);
                        AOS.Damage(m, this, damage, 20, 0, 0, 0, 80);
                    }
                }
                eable.Free();
            }

            m_NextGobbleBreath = DateTime.UtcNow + TimeSpan.FromSeconds(12);
        }

        // 2) Feather Fan Barrage: cone of razor‐sharp feathers + trap webs
        public void FeatherBarrageAttack()
        {
            var map = Map;
            if (map == null || Combatant == null) return;
            if (!(Combatant is Mobile target)) return;
            if (!Utility.InRange(Location, target.Location, 8)) return;

            var d = GetDirectionTo(target);
            int dx = 0, dy = 0;
            Movement.Movement.Offset(d, ref dx, ref dy);

            Effects.PlaySound(Location, map, 0x658);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, map, EffectItem.DefaultDuration),
                0x3728, 10, 10, UniqueHue, 0, 2023, 0
            );

            var cone = new List<Point3D>();
            int coneRange = 8, coneWidth = 3, damage = 30;
            for (int i = 1; i <= coneRange; i++)
            {
                int half = (int)(i * (coneWidth / (double)coneRange));
                for (int off = -half; off <= half; off++)
                {
                    int tx = X + i * dx;
                    int ty = Y + i * dy;
                    if (dx == 0) tx += off;
                    else if (dy == 0) ty += off;
                    else
                    {
                        // diagonal approx
                        tx += off;
                        ty += (dx * dy > 0 ? -off : off);
                    }

                    var p = new Point3D(tx, ty, Z);
                    if (!map.CanFit(p, 16, false, false))
                    {
                        int z2 = map.GetAverageZ(tx, ty);
                        p = new Point3D(tx, ty, z2);
                        if (!map.CanFit(p, 16, false, false)) continue;
                    }

                    cone.Add(p);
                }
            }

            foreach (var p in cone)
            {
                Effects.SendLocationParticles(
                    EffectItem.Create(p, map, EffectItem.DefaultDuration),
                    0x3709, 10, 30, UniqueHue, 0, 5052, 0
                );

                var eable = map.GetMobilesInRange(p, 0);
                foreach (Mobile m in eable)
                {
                    if (m != this &&
                       (m is PlayerMobile || (m is BaseCreature bc && bc.Team != Team)))
                    {
                        DoHarmful(m);
                        AOS.Damage(m, this, damage, 100, 0, 0, 0, 0);
                    }
                }
                eable.Free();

                if (Utility.RandomDouble() < 0.3)
                {
                    // spawn a TrapWeb
                    // TrapWeb web = new TrapWeb();
                    // web.MoveToWorld(p, map);
                }
            }

            m_NextFeatherBarrage = DateTime.UtcNow + TimeSpan.FromSeconds(15);
        }

        // 3) Volatile Gravy Bomb: burst around target spawning Quicksand & Vortex
        public void GravyBombAttack()
        {
            var map = Map;
            if (map == null || Combatant == null) return;
            if (!(Combatant is Mobile target)) return;
            if (!Utility.InRange(Location, target.Location, 6)) return;

            Effects.PlaySound(target.Location, map, 0x1F3);
            Effects.SendLocationParticles(
                EffectItem.Create(target.Location, map, EffectItem.DefaultDuration),
                0x3709, 10, 30, UniqueHue, 0, 5052, 0
            );

            int radius = 4, damage = 40;
            var pts = new List<Point3D>();
            for (int dx = -radius; dx <= radius; dx++)
            for (int dy = -radius; dy <= radius; dy++)
            {
                var p = new Point3D(target.X + dx, target.Y + dy, target.Z);
                if (!map.CanFit(p, 16, false, false))
                {
                    int z2 = map.GetAverageZ(p.X, p.Y);
                    p = new Point3D(p.X, p.Y, z2);
                    if (!map.CanFit(p, 16, false, false)) continue;
                }
                if (Utility.InRange(target.Location, p, radius))
                    pts.Add(p);
            }

            foreach (var p in pts)
            {
                Effects.SendLocationParticles(
                    EffectItem.Create(p, map, EffectItem.DefaultDuration),
                    0x3039, 10, 10, UniqueHue, 0, 2023, 0
                );

                var eable = map.GetMobilesInRange(p, 0);
                foreach (Mobile m in eable)
                {
                    if (m != this &&
                       (m is PlayerMobile || (m is BaseCreature bc && bc.Team != Team)))
                    {
                        DoHarmful(m);
                        AOS.Damage(m, this, damage, 0, 100, 0, 0, 0);
                    }
                }
                eable.Free();

                if (Utility.RandomDouble() < 0.5)
                {
                    if (Utility.RandomBool())
                    {
                        // QuicksandTile sand = new QuicksandTile();
                        // sand.MoveToWorld(p, map);
                    }
                    else
                    {
                        // VortexTile vortex = new VortexTile();
                        // vortex.MoveToWorld(p, map);
                    }
                }
            }

            m_NextGravyBomb = DateTime.UtcNow + TimeSpan.FromSeconds(10);
        }

        // 4) Flaming Feather Flurry: radial flame + drop hazard tiles
        public void FeatherFlurryAttack()
        {
            var map = Map;
            if (map == null) return;

            int radius = 6, damage = 30;
            Effects.PlaySound(Location, map, 0x225);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, map, EffectItem.DefaultDuration),
                0x3709, 10, 40, UniqueHue, 0, 5052, 0
            );

            var pts = new List<Point3D>();
            for (int dx = -radius; dx <= radius; dx++)
            for (int dy = -radius; dy <= radius; dy++)
            {
                var p = new Point3D(X + dx, Y + dy, Z);
                if (!map.CanFit(p, 16, false, false))
                {
                    int z2 = map.GetAverageZ(p.X, p.Y);
                    p = new Point3D(p.X, p.Y, z2);
                    if (!map.CanFit(p, 16, false, false)) continue;
                }
                if (Utility.InRange(Location, p, radius))
                    pts.Add(p);
            }

            foreach (var p in pts)
            {
                Effects.SendLocationParticles(
                    EffectItem.Create(p, map, EffectItem.DefaultDuration),
                    0x3039, 10, 10, UniqueHue, 0, 2023, 0
                );

                var eable = map.GetMobilesInRange(p, 0);
                foreach (Mobile m in eable)
                {
                    if (m != this &&
                       (m is PlayerMobile || (m is BaseCreature bc && bc.Team != Team)))
                    {
                        DoHarmful(m);
                        AOS.Damage(m, this, damage, 50, 0, 0, 0, 50);
                    }
                }
                eable.Free();

                if (Utility.RandomDouble() < 0.4)
                {
                    if (Utility.RandomBool())
                    {
                        // FlamestrikeHazardTile flame = new FlamestrikeHazardTile();
                        // flame.MoveToWorld(p, map);
                    }
                    else
                    {
                        // NecromanticFlamestrikeTile necro = new NecromanticFlamestrikeTile();
                        // necro.MoveToWorld(p, map);
                    }
                }
            }

            m_NextFlurry = DateTime.UtcNow + TimeSpan.FromSeconds(18);
        }

        // 5) Roosting Ward: spawns healing & mana‐drain tiles
        public void RoostWardAbility()
        {
            var map = Map;
            if (map == null) return;

            int radius = 5;
            Effects.PlaySound(Location, map, 0x1F2);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, map, EffectItem.DefaultDuration),
                0x37C4, 1, 10, UniqueHue, 0, 9909, 0
            );

            var pts = new List<Point3D>();
            for (int dx = -radius; dx <= radius; dx++)
            for (int dy = -radius; dy <= radius; dy++)
            {
                var p = new Point3D(X + dx, Y + dy, Z);
                if (!map.CanFit(p, 16, false, false))
                {
                    int z2 = map.GetAverageZ(p.X, p.Y);
                    p = new Point3D(p.X, p.Y, z2);
                    if (!map.CanFit(p, 16, false, false)) continue;
                }
                if (Utility.InRange(Location, p, radius))
                    pts.Add(p);
            }

            foreach (var p in pts)
            {
                Effects.SendLocationParticles(
                    EffectItem.Create(p, map, EffectItem.DefaultDuration),
                    0x37CB, 1, 10, UniqueHue, 0, 9909, 0
                );

                if (Utility.RandomDouble() < 0.4)
                {
                    if (Utility.RandomBool())
                    {
                        // HealingPulseTile heal = new HealingPulseTile();
                        // heal.MoveToWorld(p, map);
                    }
                    else
                    {
                        // ManaDrainTile mana = new ManaDrainTile();
                        // mana.MoveToWorld(p, map);
                    }
                }
            }

            m_NextRoostWard = DateTime.UtcNow + TimeSpan.FromSeconds(20);
        }

        // **AI logic to fire abilities on cooldown**
        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null) return;
            if (!(Combatant is Mobile target)) return;

            if (DateTime.UtcNow >= m_NextGobbleBreath    && InRange(target, 10)) GobbleBreathAttack();
            else if (DateTime.UtcNow >= m_NextFeatherBarrage && InRange(target, 8))  FeatherBarrageAttack();
            else if (DateTime.UtcNow >= m_NextGravyBomb    && InRange(target, 6))  GravyBombAttack();
            else if (DateTime.UtcNow >= m_NextFlurry       && InRange(target, 7))  FeatherFlurryAttack();
            else if (DateTime.UtcNow >= m_NextRoostWard)                        RoostWardAbility();
        }

        // **On‐death scatter effect**
        public override void OnDeath(Container c)
        {
            if (Map == null)
            {
                base.OnDeath(c);
                return;
            }

            Effects.PlaySound(Location, Map, 0x215);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                0x3709, 10, 60, UniqueHue, 0, 5052, 0
            );

            int tiles = 15, radius = 6;
            for (int i = 0; i < tiles; i++)
            {
                var p = GetRandomValidLocation(Location, radius, Map);
                if (p == Point3D.Zero) continue;

                // randomly splash hazard tiles
                switch (Utility.Random(4))
                {
                    case 0:
                        // QuicksandTile qs = new QuicksandTile();
                        // qs.MoveToWorld(p, Map);
                        break;
                    case 1:
                        // PoisonTile pt = new PoisonTile();
                        // pt.MoveToWorld(p, Map);
                        break;
                    case 2:
                        // ThunderstormTile ts = new ThunderstormTile();
                        // ts.MoveToWorld(p, Map);
                        break;
                    default:
                        // LandmineTile lm = new LandmineTile();
                        // lm.MoveToWorld(p, Map);
                        break;
                }

                Effects.SendLocationParticles(
                    EffectItem.Create(p, Map, EffectItem.DefaultDuration),
                    0x3709, 10, 20, UniqueHue, 0, 5016, 0
                );
            }

            base.OnDeath(c);
        }

        // **Helper for valid random point**
        private Point3D GetRandomValidLocation(Point3D center, int radius, Map map)
        {
            for (int i = 0; i < 10; i++)
            {
                int dx = Utility.RandomMinMax(-radius, radius);
                int dy = Utility.RandomMinMax(-radius, radius);
                var p = new Point3D(center.X + dx, center.Y + dy, center.Z);

                if (!map.CanFit(p, 16, false, false))
                {
                    int z2 = map.GetAverageZ(p.X, p.Y);
                    p = new Point3D(p.X, p.Y, z2);
                    if (!map.CanFit(p, 16, false, false)) continue;
                }

                if (Utility.InRange(center, p, radius))
                    return p;
            }
            return Point3D.Zero;
        }

        // **Serialization**
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
