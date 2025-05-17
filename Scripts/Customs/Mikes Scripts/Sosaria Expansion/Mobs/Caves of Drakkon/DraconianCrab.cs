using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a Draconian Crab corpse")]
    public class DraconianCrab : BaseCreature
    {
        // Cooldown timers for special abilities
        private DateTime m_NextPincerFury;
        private DateTime m_NextSandTsunami;
        private DateTime m_NextAcidGeyser;
        private DateTime m_NextGravPull;
        private DateTime m_NextShellWard;

        // Unique crimson hue for Draconian Crab
        private const int UniqueHue = 1176;

        [Constructable]
        public DraconianCrab()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Draconian Crab";
            Body = 1510;               // CoconutCrab body
            BaseSoundID = 0x4F2;       // CoconutCrab sound
            Hue = UniqueHue;

            // Stats
            SetStr(1100, 1200);
            SetDex(200, 250);
            SetInt(300, 350);

            SetHits(1000, 1200);
            SetMana(500, 600);

            SetDamage(30, 40);

            // Resistances tuned for an ice‑dwelling crustacean
            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire, 40, 50);
            SetResistance(ResistanceType.Cold, 70, 80);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 50, 60);

            // Skills
            SetSkill(SkillName.EvalInt, 80.0, 100.0);
            SetSkill(SkillName.Magery, 90.0, 110.0);
            SetSkill(SkillName.MagicResist, 120.0, 150.0);
            SetSkill(SkillName.Tactics, 100.0, 120.0);
            SetSkill(SkillName.Wrestling, 100.0, 120.0);

            Fame = 25000;
            Karma = -25000;
            VirtualArmor = 60;

            // Initialize cooldowns so it can act immediately
            m_NextPincerFury   = DateTime.UtcNow;
            m_NextSandTsunami  = DateTime.UtcNow;
            m_NextAcidGeyser   = DateTime.UtcNow;
            m_NextGravPull     = DateTime.UtcNow;
            m_NextShellWard    = DateTime.UtcNow;
        }

        public DraconianCrab(Serial serial)
            : base(serial)
        {
        }

        // --- Properties ---
        public override bool ReacquireOnMovement => true;
        public override bool AutoDispel        => false;
        public override HideType HideType     => HideType.Barbed;
        public override int Hides             => 50;
        public override int Meat              => 20;
        public override int Scales            => 10;
        public override ScaleType ScaleType   => (ScaleType)Utility.Random(4);
        public override Poison PoisonImmune    => Poison.Lethal;
        public override Poison HitPoison       => Poison.Deadly;
        public override int TreasureMapLevel   => 6;
        public override bool CanFly            => false;

        // --- Loot ---
        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 3);
            AddLoot(LootPack.Gems, 6);

            if (Utility.RandomDouble() < 0.02) // 2% for a rare claw
                ; // PackItem( new DraconianCrabClaw() );
        }

        // --- Sounds ---
        public override int GetIdleSound() => 0x4F2;
        public override int GetAngerSound() => 0x4F2;
        public override int GetHurtSound() => 0x4F2;
        public override int GetDeathSound() => 0x4F2;

        // --- Special Abilities ---

        // 1) Pincer Fury: Cross‑shaped shockwave in 4 directions
        public void PincerFury()
        {
            var map = Map;
			if (map == null || !(Combatant is Mobile)) return;

			Mobile target = (Mobile)Combatant;


            Effects.PlaySound(Location, map, 0x2F0); // thunderclap
            Effects.SendLocationParticles(EffectItem.Create(Location, map, EffectItem.DefaultDuration),
                0x3728, 8, 8, UniqueHue, 0, 5025, 0);

            int length = 4;
            int damage = 25;

            for (int dir = 0; dir < 4; dir++)
            {
                int dx = 0, dy = 0;
                Movement.Movement.Offset((Direction)(dir * 2), ref dx, ref dy);

                for (int i = 1; i <= length; i++)
                {
                    var p = new Point3D(X + dx * i, Y + dy * i, Z);
                    if (!map.CanFit(p, 16, false, false)) break;

                    Effects.SendLocationParticles(EffectItem.Create(p, map, EffectItem.DefaultDuration),
                        0x3790, 6, 6, UniqueHue, 0, 2031, 0);

                    foreach (Mobile m in map.GetMobilesInRange(p, 0))
                    {
                        if (m != this && (m is PlayerMobile || (m is BaseCreature bc && bc.Team != Team)))
                        {
                            DoHarmful(m);
                            AOS.Damage(m, this, damage, 100, 0, 0, 0, 0);
                        }
                    }
                }
            }

            m_NextPincerFury = DateTime.UtcNow + TimeSpan.FromSeconds(12);
        }

        // 2) Sand Tsunami: Wide cone of tumbling sand that leaves quicksand
        public void SandTsunami()
        {
            var map = Map;
            if (map == null || !(Combatant is Mobile)) return;

			Mobile target = (Mobile)Combatant;


            Effects.PlaySound(Location, map, 0x258);
            Effects.SendLocationParticles(EffectItem.Create(Location, map, EffectItem.DefaultDuration),
                0x3709, 10, 30, UniqueHue, 0, 5050, 0);

            Direction d = GetDirectionTo(target);
            int dx = 0, dy = 0;
            Movement.Movement.Offset(d, ref dx, ref dy);

            int range = 8;
            int width = 4;
            int damage = 20;

            var cone = new List<Point3D>();
            for (int i = 1; i <= range; i++)
            {
                int span = (int)(i * (width / (double)range));
                for (int offset = -span; offset <= span; offset++)
                {
                    int tx = X + dx * i, ty = Y + dy * i;
                    if (dx == 0) tx += offset;
                    else if (dy == 0) ty += offset;
                    else { tx += offset; ty += offset; }

                    var p = new Point3D(tx, ty, Z);
                    if (map.CanFit(p, 16, false, false))
                        cone.Add(p);
                }
            }

            foreach (var p in cone)
            {
                Effects.SendLocationParticles(EffectItem.Create(p, map, EffectItem.DefaultDuration),
                    0x36B0, 8, 8, UniqueHue, 0, 2023, 0);

                foreach (Mobile m in map.GetMobilesInRange(p, 0))
                {
                    if (m != this && (m is PlayerMobile || (m is BaseCreature bc && bc.Team != Team)))
                    {
                        DoHarmful(m);
                        AOS.Damage(m, this, damage, 0, 0, 0, 100, 0);
                    }
                }

                if (Utility.RandomDouble() < 0.3)
                {
                    var qs = new QuicksandTile();
                    qs.MoveToWorld(p, map);
                }
            }

            m_NextSandTsunami = DateTime.UtcNow + TimeSpan.FromSeconds(15);
        }

        // 3) Acid Geyser: Rings of acid columns around the crab
        public void AcidGeyser()
        {
            var map = Map;
            if (map == null) return;

            Effects.PlaySound(Location, map, 0x3E9);
            Effects.SendLocationParticles(EffectItem.Create(Location, map, EffectItem.DefaultDuration),
                0x372A, 12, 12, UniqueHue, 0, 5048, 0);

            int radius = 5;
            int damage = 30;
            var ring = new List<Point3D>();

            for (int x = -radius; x <= radius; x++)
            for (int y = -radius; y <= radius; y++)
            {
                if (Utility.InRange(Location, new Point3D(X + x, Y + y, Z), radius) &&
                    Math.Abs(Math.Sqrt(x*x + y*y) - radius) < 1.0)
                {
                    var p = new Point3D(X + x, Y + y, Z);
                    if (map.CanFit(p, 16, false, false))
                        ring.Add(p);
                }
            }

            foreach (var p in ring)
            {
                Effects.SendLocationParticles(EffectItem.Create(p, map, EffectItem.DefaultDuration),
                    0x36B0, 10, 10, UniqueHue, 0, 5055, 0);

                foreach (Mobile m in map.GetMobilesInRange(p, 0))
                {
                    if (m != this && (m is PlayerMobile || (m is BaseCreature bc && bc.Team != Team)))
                    {
                        DoHarmful(m);
                        AOS.Damage(m, this, damage, 0, 0, 100, 0, 0);
                    }
                }

                if (Utility.RandomBool())
                {
                    var acid = new ToxicGasTile();
                    acid.MoveToWorld(p, map);
                }
            }

            m_NextAcidGeyser = DateTime.UtcNow + TimeSpan.FromSeconds(18);
        }

        // 4) Gravitational Pull: Draws targets into center, leaves vortexes
        public void GravitationalPull()
        {
            var map = Map;
            if (map == null) return;

            Effects.PlaySound(Location, map, 0x1F1);
            Effects.SendLocationParticles(EffectItem.Create(Location, map, EffectItem.DefaultDuration),
                0x37C4, 1, 10, UniqueHue, 0, 9909, 0);

            int radius = 6;
            foreach (Mobile m in map.GetMobilesInRange(Location, radius))
            {
                if (m != this && (m is PlayerMobile || (m is BaseCreature bc && bc.Team != Team)))
                {
                    // Pull closer
                    int nx = X + (m.X < X ? 1 : m.X > X ? -1 : 0);
                    int ny = Y + (m.Y < Y ? 1 : m.Y > Y ? -1 : 0);
                    m.Location = new Point3D(nx, ny, Z);

                    DoHarmful(m);
                    AOS.Damage(m, this, 20, 0, 100, 0, 0, 0);
                }
            }

            // Spawn vortex tiles
            for (int i = 0; i < 8; i++)
            {
                var p = GetRandomValidLocation(Location, 5, map);
                if (p != Point3D.Zero)
                {
                    var vortex = new VortexTile();
                    vortex.MoveToWorld(p, map);
                }
            }

            m_NextGravPull = DateTime.UtcNow + TimeSpan.FromSeconds(25);
        }

        // 5) Shell Ward: Protective/damaging shell zone
        public void ShellWard()
        {
            var map = Map;
            if (map == null) return;

            Effects.PlaySound(Location, map, 0x208);
            Effects.SendLocationParticles(EffectItem.Create(Location, map, EffectItem.DefaultDuration),
                0x37CB, 1, 10, UniqueHue, 0, 9910, 0);

            int radius = 4;
            for (int x = -radius; x <= radius; x++)
            for (int y = -radius; y <= radius; y++)
            {
                var p = new Point3D(X + x, Y + y, Z);
                if (Utility.InRange(Location, p, radius) && map.CanFit(p, 16, false, false))
                {
                    Effects.SendLocationParticles(EffectItem.Create(p, map, EffectItem.DefaultDuration),
                        0x37CB, 1, 8, UniqueHue, 0, 9910, 0);

                    if (Utility.RandomDouble() < 0.3)
                    {
                        if (Utility.RandomBool())
                        {
                            var heal = new HealingPulseTile();
                            heal.MoveToWorld(p, map);
                        }
                        else
                        {
                            var mana = new ManaDrainTile();
                            mana.MoveToWorld(p, map);
                        }
                    }
                }
            }

            m_NextShellWard = DateTime.UtcNow + TimeSpan.FromSeconds(20);
        }

        // --- AI Logic ---
        public override void OnThink()
        {
            base.OnThink();

            if (Combatant is Mobile target)
            {
                if (DateTime.UtcNow >= m_NextPincerFury && InRange(target, 6))
                    PincerFury();
                else if (DateTime.UtcNow >= m_NextSandTsunami && InRange(target, 10))
                    SandTsunami();
                else if (DateTime.UtcNow >= m_NextAcidGeyser)
                    AcidGeyser();
                else if (DateTime.UtcNow >= m_NextGravPull)
                    GravitationalPull();
                else if (DateTime.UtcNow >= m_NextShellWard)
                    ShellWard();
            }
        }

        // --- Death Explosion ---
        public override void OnDeath(Container c)
        {
            var map = Map;
            if (map != null)
            {
                Effects.PlaySound(Location, map, 0x2A3);
                Effects.SendLocationParticles(EffectItem.Create(Location, map, EffectItem.DefaultDuration),
                    0x3709, 10, 60, UniqueHue, 0, 5052, 0);

                for (int i = 0; i < 15; i++)
                {
                    var p = GetRandomValidLocation(Location, 5, map);
                    if (p == Point3D.Zero) continue;

                    if (Utility.RandomBool())
                    {
                        var land = new LandmineTile();
                        land.MoveToWorld(p, map);
                    }
                    else
                    {
                        var web = new TrapWeb();
                        web.MoveToWorld(p, map);
                    }

                    Effects.SendLocationParticles(EffectItem.Create(p, map, EffectItem.DefaultDuration),
                        0x3709, 6, 20, UniqueHue, 0, 5016, 0);
                }
            }

            base.OnDeath(c);
        }

        // Helper for random valid point within radius
        private Point3D GetRandomValidLocation(Point3D center, int radius, Map map)
        {
            for (int i = 0; i < 10; i++)
            {
                int dx = Utility.RandomMinMax(-radius, radius);
                int dy = Utility.RandomMinMax(-radius, radius);
                var p = new Point3D(center.X + dx, center.Y + dy, center.Z);
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
