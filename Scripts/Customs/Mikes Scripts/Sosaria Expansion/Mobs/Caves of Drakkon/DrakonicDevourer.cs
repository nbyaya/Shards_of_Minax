using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a drakonic devourer corpse")]
    public class DrakonicDevourer : BaseCreature
    {
        // Cooldown timers for special abilities
        private DateTime m_NextSoulEaterCone;
        private DateTime m_NextShadowShardRing;
        private DateTime m_NextFallenTriangleVolley;
        private DateTime m_NextVoidTremor;
        private DateTime m_NextAbyssalChasm;

        // Unique hue for the Drakonic Devourer (deep spectral purple)
        private const int UniqueHue = 1175;

        [Constructable]
        public DrakonicDevourer()
            : base(AIType.AI_NecroMage, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name = "Drakonic Devourer";
            Body = 303;                 // Use Devourer body
            BaseSoundID = 357;          // Use Devourer sounds
            Hue = UniqueHue;            // Spectral purple

            // Enhanced stats
            SetStr(1500, 1700);
            SetDex(200, 250);
            SetInt(500, 600);

            SetHits(1200, 1400);
            SetDamage(40, 55);

            // Damage types
            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Energy, 30);
            SetDamageType(ResistanceType.Cold, 20);

            // Resistances
            SetResistance(ResistanceType.Physical, 60, 75);
            SetResistance(ResistanceType.Fire, 40, 50);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 80, 90);

            // Skills
            SetSkill(SkillName.Necromancy, 110.0, 130.0);
            SetSkill(SkillName.SpiritSpeak, 110.0, 130.0);
            SetSkill(SkillName.EvalInt, 100.0, 120.0);
            SetSkill(SkillName.Magery, 100.0, 120.0);
            SetSkill(SkillName.MagicResist, 120.0, 150.0);
            SetSkill(SkillName.Tactics, 100.0, 120.0);
            SetSkill(SkillName.Wrestling, 100.0, 120.0);

            Fame = 35000;
            Karma = -35000;
            VirtualArmor = 90;

            // Initialize ability cooldowns
            m_NextSoulEaterCone       = DateTime.UtcNow;
            m_NextShadowShardRing     = DateTime.UtcNow;
            m_NextFallenTriangleVolley = DateTime.UtcNow;
            m_NextVoidTremor          = DateTime.UtcNow;
            m_NextAbyssalChasm        = DateTime.UtcNow;
        }

        public DrakonicDevourer(Serial serial)
            : base(serial)
        {
        }

        // --- Properties ---
        public override bool ReacquireOnMovement => true;
        public override Poison PoisonImmune => Poison.Lethal;
        public override Poison HitPoison => Poison.Lethal;
        public override int Meat => 5;
        public override int TreasureMapLevel => 5;
        public override int GetIdleSound() => BaseSoundID;
        public override int GetHurtSound() => BaseSoundID;

        // --- Loot Generation ---
        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 3);
            PackNecroReg(30, 60);

            if (Utility.RandomDouble() < 0.005) // 0.5% chance
                PackItem(new DevourerCloak());

            if (Utility.RandomDouble() < 0.02)  // 2% chance
                PackItem(new TreasureMap(5, Map));
        }

        // --- Special Abilities ---

        // 1. Soul Eater Cone: a forward-facing necrotic cone that damages and heals
        public void SoulEaterConeAttack()
        {
            if (!(Combatant is Mobile target)) return;
            Map map = this.Map;
            if (map == null) return;

            const int coneRange = 10;
            const int coneWidth = 4;
            const int damage = 40;
            const int healAmount = damage / 2;

            if (!Utility.InRange(this.Location, target.Location, coneRange))
                return;

            // Play sound and central effect
            Effects.PlaySound(this.Location, map, 0x225);
            Effects.SendLocationParticles(
                EffectItem.Create(this.Location, map, EffectItem.DefaultDuration),
                0x36BD, 1, 10, UniqueHue, 0, 2025, 0);

            Direction d = GetDirectionTo(target);
            int dx = 0, dy = 0;
            Movement.Movement.Offset(d, ref dx, ref dy);

            List<Point3D> coneTiles = new List<Point3D>();

            for (int i = 1; i <= coneRange; i++)
            {
                int sideOffset = (int)(i * (coneWidth / (double)coneRange));
                for (int j = -sideOffset; j <= sideOffset; j++)
                {
                    int x = this.X + i * dx;
                    int y = this.Y + i * dy;
                    if (dx == 0) x += j;
                    else if (dy == 0) y += j;
                    else
                    {
                        // simple diamond fallback
                        if (Math.Abs(j) > sideOffset) continue;
                        x += j * Math.Sign(dy);
                        y += j * -Math.Sign(dx);
                    }

                    Point3D p = new Point3D(x, y, this.Z);
                    if (map.CanFit(p, 16, false, false))
                        coneTiles.Add(p);
                    else
                    {
                        int z2 = map.GetAverageZ(x, y);
                        Point3D p2 = new Point3D(x, y, z2);
                        if (map.CanFit(p2, 16, false, false))
                            coneTiles.Add(p2);
                    }
                }
            }

            foreach (Point3D p in coneTiles)
            {
                Effects.SendLocationParticles(
                    EffectItem.Create(p, map, EffectItem.DefaultDuration),
                    0x3709, 5, 15, UniqueHue, 0, 5054, 0);

                IPooledEnumerable eable = map.GetMobilesInRange(p, 0);
                foreach (Mobile m in eable)
                {
                    if (m == this) continue;
                    if (m is PlayerMobile || (m is BaseCreature bc && bc.Team != this.Team))
                    {
                        DoHarmful(m);
                        AOS.Damage(m, this, damage, 0, 0, 100, 0, 0);
                    }
                }
                eable.Free();
            }

            // Heal self
            this.Hits += healAmount;

            m_NextSoulEaterCone = DateTime.UtcNow + TimeSpan.FromSeconds(12);
        }

        // 2. Shadow Shard Ring: circular burst of shards + necromantic flamestrike tiles
        public void ShadowShardRingAttack()
        {
            Map map = this.Map;
            if (map == null) return;

            const int radius = 6;
            const int damage = 30;

            Effects.PlaySound(this.Location, map, 0x209);
            Effects.SendLocationParticles(
                EffectItem.Create(this.Location, map, EffectItem.DefaultDuration),
                0x376A, 10, 30, UniqueHue, 0, 5060, 0);

            List<Point3D> ringTiles = new List<Point3D>();
            for (int x = -radius; x <= radius; x++)
            for (int y = -radius; y <= radius; y++)
            {
                Point3D p = new Point3D(this.X + x, this.Y + y, this.Z);
                if (Utility.InRange(this.Location, p, radius))
                {
                    if (map.CanFit(p, 16, false, false))
                        ringTiles.Add(p);
                    else
                    {
                        int z2 = map.GetAverageZ(p.X, p.Y);
                        Point3D p2 = new Point3D(p.X, p.Y, z2);
                        if (map.CanFit(p2, 16, false, false))
                            ringTiles.Add(p2);
                    }
                }
            }

            foreach (Point3D p in ringTiles)
            {
                Effects.SendLocationEffect(p, map, 0x3709, 16, UniqueHue, 0);
                IPooledEnumerable eable = map.GetMobilesInRange(p, 0);
                foreach (Mobile m in eable)
                {
                    if (m == this) continue;
                    if (m is PlayerMobile || (m is BaseCreature bc && bc.Team != this.Team))
                    {
                        DoHarmful(m);
                        AOS.Damage(m, this, damage, 0, 0, 0, 0, 100);
                    }
                }
                eable.Free();

                if (Utility.RandomDouble() < 0.3)
                {
                    // NecromanticFlamestrikeTile assumed defined elsewhere
                    // NecromanticFlamestrikeTile tile = new NecromanticFlamestrikeTile();
                    // tile.MoveToWorld(p, map);
                }
            }

            m_NextShadowShardRing = DateTime.UtcNow + TimeSpan.FromSeconds(15);
        }

        // 3. Fallen Triangle Volley: triangular barrage of ethereal comets + landmines
        public void FallenTriangleVolleyAttack()
        {
            if (!(Combatant is Mobile target)) return;
            Map map = this.Map;
            if (map == null) return;

            const int range = 12;
            const int maxWidth = 6;
            const int damage = 25;

            if (!Utility.InRange(this.Location, target.Location, range))
                return;

            Effects.PlaySound(this.Location, map, 0x22D);
            Effects.SendLocationParticles(
                EffectItem.Create(this.Location, map, EffectItem.DefaultDuration),
                0x3728, 8, 12, UniqueHue, 0, 2027, 0);

            Direction d = GetDirectionTo(target);
            int dx = 0, dy = 0;
            Movement.Movement.Offset(d, ref dx, ref dy);

            List<Point3D> volleyTiles = new List<Point3D>();
            for (int i = 1; i <= range; i++)
            {
                int width = (int)((i / (double)range) * maxWidth);
                for (int j = -width; j <= width; j++)
                {
                    int x = this.X + i * dx;
                    int y = this.Y + i * dy;
                    if (dx == 0) x += j;
                    else if (dy == 0) y += j;
                    else
                    {
                        x += j * Math.Sign(dy);
                        y += j * -Math.Sign(dx);
                    }

                    Point3D p = new Point3D(x, y, this.Z);
                    if (map.CanFit(p, 16, false, false))
                        volleyTiles.Add(p);
                    else
                    {
                        int z2 = map.GetAverageZ(x, y);
                        Point3D p2 = new Point3D(x, y, z2);
                        if (map.CanFit(p2, 16, false, false))
                            volleyTiles.Add(p2);
                    }
                }
            }

            foreach (Point3D p in volleyTiles)
            {
                Effects.SendLocationParticles(
                    EffectItem.Create(p, map, EffectItem.DefaultDuration),
                    0x36B0, 5, 10, UniqueHue, 0, 2023, 0);

                IPooledEnumerable eable = map.GetMobilesInRange(p, 0);
                foreach (Mobile m in eable)
                {
                    if (m == this) continue;
                    if (m is PlayerMobile || (m is BaseCreature bc && bc.Team != this.Team))
                    {
                        DoHarmful(m);
                        AOS.Damage(m, this, damage, 0, 100, 0, 0, 0);
                    }
                }
                eable.Free();

                if (Utility.RandomDouble() < 0.4)
                {
                    // LandmineTile assumed defined elsewhere
                    // LandmineTile mine = new LandmineTile();
                    // mine.MoveToWorld(p, map);
                }
            }

            m_NextFallenTriangleVolley = DateTime.UtcNow + TimeSpan.FromSeconds(8);
        }

        // 4. Void Tremor: linear chasm of dark vortex + vortex tiles
        public void VoidTremorAttack()
        {
            if (!(Combatant is Mobile target)) return;
            Map map = this.Map;
            if (map == null) return;

            const int length = 15;
            const int damage = 50;

            if (!Utility.InRange(this.Location, target.Location, length))
                return;

            Direction d = GetDirectionTo(target);
            int dx = 0, dy = 0;
            Movement.Movement.Offset(d, ref dx, ref dy);

            List<Point3D> lineTiles = new List<Point3D>();
            for (int i = 1; i <= length; i++)
            {
                int x = this.X + i * dx;
                int y = this.Y + i * dy;
                Point3D p = new Point3D(x, y, this.Z);
                if (map.CanFit(p, 16, false, false))
                    lineTiles.Add(p);
                else
                {
                    int z2 = map.GetAverageZ(x, y);
                    Point3D p2 = new Point3D(x, y, z2);
                    if (map.CanFit(p2, 16, false, false))
                        lineTiles.Add(p2);
                    else
                        break;
                }
            }

            Effects.PlaySound(this.Location, map, 0x20A);
            Effects.SendLocationParticles(
                EffectItem.Create(this.Location, map, EffectItem.DefaultDuration),
                0x370A, 5, 10, UniqueHue, 0, 5062, 0);

            foreach (Point3D p in lineTiles)
            {
                Effects.SendLocationEffect(p, map, 0x3818, 16, UniqueHue, 0);

                IPooledEnumerable eable = map.GetMobilesInRange(p, 0);
                foreach (Mobile m in eable)
                {
                    if (m == this) continue;
                    if (m is PlayerMobile || (m is BaseCreature bc && bc.Team != this.Team))
                    {
                        DoHarmful(m);
                        AOS.Damage(m, this, damage, 0, 0, 0, 0, 100);
                    }
                }
                eable.Free();

                if (Utility.RandomDouble() < 0.3)
                {
                    // VortexTile assumed defined elsewhere
                    // VortexTile vortex = new VortexTile();
                    // vortex.MoveToWorld(p, map);
                }
            }

            m_NextVoidTremor = DateTime.UtcNow + TimeSpan.FromSeconds(10);
        }

        // 5. Abyssal Chasm: large circle of Abyssal energy + quicksand and vortex
        public void AbyssalChasmAbility()
        {
            Map map = this.Map;
            if (map == null) return;

            const int radius = 4;
            const int damage = 20;

            Effects.PlaySound(this.Location, map, 0x22B);
            Effects.SendLocationParticles(
                EffectItem.Create(this.Location, map, EffectItem.DefaultDuration),
                0x37C4, 1, 10, UniqueHue, 0, 5058, 0);

            List<Point3D> areaTiles = new List<Point3D>();
            for (int x = -radius; x <= radius; x++)
            for (int y = -radius; y <= radius; y++)
            {
                Point3D p = new Point3D(this.X + x, this.Y + y, this.Z);
                if (Utility.InRange(this.Location, p, radius))
                {
                    if (map.CanFit(p, 16, false, false))
                        areaTiles.Add(p);
                    else
                    {
                        int z2 = map.GetAverageZ(p.X, p.Y);
                        Point3D p2 = new Point3D(p.X, p.Y, z2);
                        if (map.CanFit(p2, 16, false, false))
                            areaTiles.Add(p2);
                    }
                }
            }

            foreach (Point3D p in areaTiles)
            {
                Effects.SendLocationParticles(
                    EffectItem.Create(p, map, EffectItem.DefaultDuration),
                    0x3709, 5, 10, UniqueHue, 0, 5030, 0);

                IPooledEnumerable eable = map.GetMobilesInRange(p, 0);
                foreach (Mobile m in eable)
                {
                    if (m == this) continue;
                    if (m is PlayerMobile || (m is BaseCreature bc && bc.Team != this.Team))
                    {
                        DoHarmful(m);
                        AOS.Damage(m, this, damage, 100, 0, 0, 0, 0);
                    }
                }
                eable.Free();

                if (Utility.RandomDouble() < 0.3)
                {
                    if (Utility.RandomBool())
                    {
                        // QuicksandTile assumed defined elsewhere
                        // QuicksandTile sand = new QuicksandTile();
                        // sand.MoveToWorld(p, map);
                    }
                    else
                    {
                        // VortexTile assumed defined elsewhere
                        // VortexTile vortex = new VortexTile();
                        // vortex.MoveToWorld(p, map);
                    }
                }
            }

            m_NextAbyssalChasm = DateTime.UtcNow + TimeSpan.FromSeconds(20);
        }

        // --- AI Logic ---
        public override void OnThink()
        {
            base.OnThink();

            if (Combatant is Mobile target)
            {
                if (DateTime.UtcNow >= m_NextSoulEaterCone && Utility.InRange(this.Location, target.Location, 10))
                {
                    SoulEaterConeAttack();
                }
                else if (DateTime.UtcNow >= m_NextFallenTriangleVolley && Utility.InRange(this.Location, target.Location, 12))
                {
                    FallenTriangleVolleyAttack();
                }
                else if (DateTime.UtcNow >= m_NextVoidTremor && Utility.InRange(this.Location, target.Location, 15))
                {
                    VoidTremorAttack();
                }
                else if (DateTime.UtcNow >= m_NextShadowShardRing)
                {
                    ShadowShardRingAttack();
                }
                else if (DateTime.UtcNow >= m_NextAbyssalChasm)
                {
                    AbyssalChasmAbility();
                }
            }
        }

        // --- OnDeath Effect ---
        public override void OnDeath(Container c)
        {
            if (Map != null)
            {
                const int tilesToScatter = 15;
                const int scatterRadius = 6;

                Effects.PlaySound(this.Location, this.Map, 0x215);
                Effects.SendLocationParticles(
                    EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration),
                    0x3709, 10, 50, UniqueHue, 0, 5052, 0);

                for (int i = 0; i < tilesToScatter; i++)
                {
                    Point3D p = GetRandomValidLocation(this.Location, scatterRadius, this.Map);
                    if (p != Point3D.Zero)
                    {
                        if (Utility.RandomBool())
                        {
                            // PoisonTile assumed defined elsewhere
                            // PoisonTile pt = new PoisonTile();
                            // pt.MoveToWorld(p, this.Map);
                        }
                        else
                        {
                            // NecromanticFlamestrikeTile assumed defined elsewhere
                            // NecromanticFlamestrikeTile nft = new NecromanticFlamestrikeTile();
                            // nft.MoveToWorld(p, this.Map);
                        }

                        Effects.SendLocationParticles(
                            EffectItem.Create(p, this.Map, EffectItem.DefaultDuration),
                            0x3709, 5, 20, UniqueHue, 0, 5032, 0);
                    }
                }
            }

            base.OnDeath(c);
        }

        // Helper to find a valid random location within radius
        private Point3D GetRandomValidLocation(Point3D center, int radius, Map map)
        {
            for (int i = 0; i < 10; i++)
            {
                int dx = Utility.RandomMinMax(-radius, radius);
                int dy = Utility.RandomMinMax(-radius, radius);
                Point3D p = new Point3D(center.X + dx, center.Y + dy, center.Z);

                if (Utility.InRange(center, p, radius) && map.CanFit(p, 16, false, false))
                    return p;

                int z2 = map.GetAverageZ(p.X, p.Y);
                Point3D p2 = new Point3D(p.X, p.Y, z2);
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
