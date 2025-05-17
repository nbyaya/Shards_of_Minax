using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a draconian parrot corpse")]
    public class DraconianParrot : BaseCreature
    {
        // Cooldown timers
        private DateTime m_NextRadiantSquawk;
        private DateTime m_NextChromaticBeam;
        private DateTime m_NextFeatherStorm;
        private DateTime m_NextMirrorMaelstrom;
        private DateTime m_NextZephyrBurst;

        // Rainbow‑like hue
        private const int UniqueHue = 2128;

        [Constructable]
        public DraconianParrot()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name           = "Draconian Parrot";
            Body           = 831;
            Hue            = UniqueHue;
            BaseSoundID    = 0x1C; // Use parrot idle sound as base

            SetStr(200, 250);
            SetDex(200, 250);
            SetInt(500, 600);

            SetHits(1200, 1400);
            SetMana(800, 1000);

            SetDamage(25, 35);
            SetDamageType(ResistanceType.Physical, 40);
            SetDamageType(ResistanceType.Fire,     30);
            SetDamageType(ResistanceType.Cold,     30);

            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire,     50, 60);
            SetResistance(ResistanceType.Cold,     70, 80);
            SetResistance(ResistanceType.Poison,   50, 60);
            SetResistance(ResistanceType.Energy,   80, 90);

            SetSkill(SkillName.EvalInt,     120.0, 140.0);
            SetSkill(SkillName.Magery,      120.0, 140.0);
            SetSkill(SkillName.Meditation,  100.0, 120.0);
            SetSkill(SkillName.MagicResist, 140.0, 160.0);
            SetSkill(SkillName.Tactics,     110.0, 120.0);
            SetSkill(SkillName.Wrestling,   100.0, 110.0);

            Fame         = 35000;
            Karma        = -35000;
            VirtualArmor = 60;

            // Initialize all cooldowns to ready
            m_NextRadiantSquawk   =
            m_NextChromaticBeam   =
            m_NextFeatherStorm    =
            m_NextMirrorMaelstrom =
            m_NextZephyrBurst     = DateTime.UtcNow;
        }

        public DraconianParrot(Serial serial) : base(serial) { }

        public override bool ReacquireOnMovement => true;
        public override bool AutoDispel         => true;

        public override HideType HideType       => HideType.Barbed;
        public override int Hides               => 15;
        public override int Scales              => 20;
        public override ScaleType ScaleType     => (ScaleType)Utility.Random(4);

        public override Poison PoisonImmune => Poison.Lethal;
        public override Poison HitPoison    => Poison.Deadly;
        public override int TreasureMapLevel => 5;
        public override bool CanFly          => true;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 3);
            AddLoot(LootPack.Gems,      6);

            if (Utility.RandomDouble() < 0.05)
                PackItem(new TreasureMap(5, Map));
        }

        // Parrot sounds
        public override int GetAngerSound()  => 0x1B;
        public override int GetIdleSound()   => 0x1C;
        public override int GetAttackSound() => 0x1D;
        public override int GetHurtSound()   => 0x1E;
        public override int GetDeathSound()  => 0x1F;

        // --- Special Abilities ---

        // 1) Radiant Squawk: big radial shockwave + LightningStormTiles
        private void RadiantSquawk()
        {
            var map = Map; if (map == null) return;

            Effects.PlaySound(Location, map, 0x5C9);
            Effects.SendLocationParticles(EffectItem.Create(Location, map, EffectItem.DefaultDuration),
                0x3728, 12, 20, UniqueHue, 0, 5041, 0);

            int radius = 6, damage = 40;
            for (int x = -radius; x <= radius; x++)
            for (int y = -radius; y <= radius; y++)
            {
                var p = new Point3D(X + x, Y + y, Z);
                if (!Utility.InRange(Location, p, radius)) continue;
                if (!map.CanFit(p, 16, false, false))
                    p = new Point3D(p.X, p.Y, map.GetAverageZ(p.X, p.Y));

                Effects.SendLocationParticles(EffectItem.Create(p, map, EffectItem.DefaultDuration),
                    0x3818, 6, 10, UniqueHue, 0, 2034, 0);

                foreach (Mobile m in map.GetMobilesInRange(p, 0))
                {
                    if (m == this) continue;
                    if (m is PlayerMobile || m is BaseCreature bc && bc.Team != Team)
                    {
                        DoHarmful(m);
                        AOS.Damage(m, this, damage, 0, 0, 0, 0, 100);
                        // spawn a LightningStormTile
                        // var tile = new LightningStormTile(); tile.MoveToWorld(p, map);
                    }
                }
            }

            m_NextRadiantSquawk = DateTime.UtcNow + TimeSpan.FromSeconds(8);
        }

        // 2) Chromatic Beam: bright line of colored light + IceShardTiles
        private void ChromaticBeamAttack()
        {
            var map = Map; if (map == null || Combatant == null) return;
            if (!(Combatant is Mobile target)) return;
            if (!Utility.InRange(Location, target.Location, 12)) return;

            Direction d = GetDirectionTo(target);
			int dx = 0, dy = 0;
			Movement.Movement.Offset(d, ref dx, ref dy);

            Effects.PlaySound(Location, map, 0x2EF);
            Effects.SendLocationParticles(EffectItem.Create(Location, map, EffectItem.DefaultDuration),
                0x373A, 10, 1, UniqueHue, 0, 2056, 0);

            for (int i = 1; i <= 12; i++)
            {
                var p = new Point3D(X + dx*i, Y + dy*i, Z);
                if (!map.CanFit(p, 16, false, false))
                    p = new Point3D(p.X, p.Y, map.GetAverageZ(p.X, p.Y));

                Effects.SendLocationEffect(p, map, 0x3818, 16, UniqueHue, 0);

                foreach (Mobile m in map.GetMobilesInRange(p, 0))
                {
                    if (m == this) continue;
                    if (m is PlayerMobile || m is BaseCreature bc && bc.Team != Team)
                    {
                        DoHarmful(m);
                        AOS.Damage(m, this, 35, 0, 100, 0, 0, 0);
                        // spawn IceShardTile
                        // var tile = new IceShardTile(); tile.MoveToWorld(p, map);
                    }
                }
            }

            m_NextChromaticBeam = DateTime.UtcNow + TimeSpan.FromSeconds(10);
        }

        // 3) Feather Storm: spiraling ring burst + VortexTiles
        private void FeatherStorm()
        {
            var map = Map; if (map == null) return;

            Effects.PlaySound(Location, map, 0x6B5);
            Effects.SendLocationParticles(EffectItem.Create(Location, map, EffectItem.DefaultDuration),
                0x375A, 12, 20, UniqueHue, 0, 2048, 0);

            int rings = 3, damage = 25;
            for (int r = 1; r <= rings; r++)
            {
                for (int deg = 0; deg < 360; deg += 30)
                {
                    double rad = deg * (Math.PI/180);
                    int px = X + (int)(Math.Cos(rad)*r*2);
                    int py = Y + (int)(Math.Sin(rad)*r*2);
                    var p = new Point3D(px, py, Z);
                    if (!map.CanFit(p, 16, false, false))
                        p = new Point3D(p.X, p.Y, map.GetAverageZ(p.X, p.Y));

                    Effects.SendLocationParticles(EffectItem.Create(p, map, EffectItem.DefaultDuration),
                        0x36F4, 8, 10, UniqueHue, 0, 5016, 0);

                    foreach (Mobile m in map.GetMobilesInRange(p, 0))
                    {
                        if (m == this) continue;
                        if (m is PlayerMobile || m is BaseCreature bc && bc.Team != Team)
                        {
                            DoHarmful(m);
                            AOS.Damage(m, this, damage, 100, 0, 0, 0, 0);
                            // spawn VortexTile
                            // var tile = new VortexTile(); tile.MoveToWorld(p, map);
                        }
                    }
                }
            }

            m_NextFeatherStorm = DateTime.UtcNow + TimeSpan.FromSeconds(12);
        }

        // 4) Mirror Maelstrom: aura that spawns Poison or ManaDrain tiles
        private void MirrorMaelstrom()
        {
            var map = Map; if (map == null) return;

            Effects.PlaySound(Location, map, 0x2F3);
            Effects.SendLocationParticles(EffectItem.Create(Location, map, EffectItem.DefaultDuration),
                0x37CB, 1, 10, UniqueHue, 0, 9909, 0);

            int radius = 5;
            for (int x = -radius; x <= radius; x++)
            for (int y = -radius; y <= radius; y++)
            {
                var p = new Point3D(X + x, Y + y, Z);
                if (!Utility.InRange(Location, p, radius)) continue;
                if (!map.CanFit(p, 16, false, false))
                    p = new Point3D(p.X, p.Y, map.GetAverageZ(p.X, p.Y));

                Effects.SendLocationParticles(EffectItem.Create(p, map, EffectItem.DefaultDuration),
                    0x37CB, 1, 10, UniqueHue, 0, 9909, 0);

                foreach (Mobile m in map.GetMobilesInRange(p, 0))
                {
                    if (m == this) continue;
                    if (m is PlayerMobile || m is BaseCreature bc && bc.Team != Team)
                    {
                        DoHarmful(m);
                        if (Utility.RandomBool())
                        {
                            // var tile = new ManaDrainTile(); tile.MoveToWorld(p, map);
                        }
                        else
                        {
                            // var tile = new PoisonTile(); tile.MoveToWorld(p, map);
                        }
                    }
                }
            }

            m_NextMirrorMaelstrom = DateTime.UtcNow + TimeSpan.FromSeconds(15);
        }

        // 5) Zephyr Burst: air‑gust AOE at Combatant, spawning Quicksand or TrapWeb
        private void ZephyrBurst()
        {
            var map = Map; if (map == null || Combatant == null) return;
            if (!(Combatant is Mobile target)) return;
            if (!Utility.InRange(Location, target.Location, 10)) return;

            Effects.PlaySound(target.Location, map, 0x4C2);
            Effects.SendLocationParticles(EffectItem.Create(target.Location, map, EffectItem.DefaultDuration),
                0x3CFC, 8, 20, UniqueHue, 0, 2060, 0);

            int burstRadius = 4, damage = 50;
            for (int x = -burstRadius; x <= burstRadius; x++)
            for (int y = -burstRadius; y <= burstRadius; y++)
            {
                var p = new Point3D(target.X + x, target.Y + y, target.Z);
                if (!Utility.InRange(target.Location, p, burstRadius)) continue;
                if (!map.CanFit(p, 16, false, false))
                    p = new Point3D(p.X, p.Y, map.GetAverageZ(p.X, p.Y));

                Effects.SendLocationParticles(EffectItem.Create(p, map, EffectItem.DefaultDuration),
                    0x36BD, 6, 10, UniqueHue, 0, 5018, 0);

                foreach (Mobile m in map.GetMobilesInRange(p, 0))
                {
                    if (m == this) continue;
                    if (m is PlayerMobile || m is BaseCreature bc && bc.Team != Team)
                    {
                        DoHarmful(m);
                        AOS.Damage(m, this, damage, 50, 0, 0, 50, 0);
                        if (Utility.RandomBool())
                        {
                            // var tile = new QuicksandTile(); tile.MoveToWorld(p, map);
                        }
                        else
                        {
                            // var tile = new TrapWeb(); tile.MoveToWorld(p, map);
                        }
                    }
                }
            }

            m_NextZephyrBurst = DateTime.UtcNow + TimeSpan.FromSeconds(20);
        }

        // --- AI Logic ---
        public override void OnThink()
        {
            base.OnThink();
            var now = DateTime.UtcNow;
            if (Combatant == null) return;

            if (now >= m_NextRadiantSquawk)
                RadiantSquawk();
            else if (now >= m_NextChromaticBeam && InRange(Combatant, 12))
                ChromaticBeamAttack();
            else if (now >= m_NextFeatherStorm)
                FeatherStorm();
            else if (now >= m_NextMirrorMaelstrom)
                MirrorMaelstrom();
            else if (now >= m_NextZephyrBurst && InRange(Combatant, 10))
                ZephyrBurst();
        }

        // --- Death Effect Scatter ---
        public override void OnDeath(Container c)
        {
            var map = Map;
            if (map != null)
            {
                Effects.PlaySound(Location, map, 0x55D);
                Effects.SendLocationParticles(EffectItem.Create(Location, map, EffectItem.DefaultDuration),
                    0x3709, 10, 30, UniqueHue, 0, 5052, 0);

                for (int i = 0; i < 15; i++)
                {
                    var p = GetRandomValidLocation(Location, 6, map);
                    if (p == Point3D.Zero) continue;

                    // Spawn a mix of VortexTile and IceShardTile
                    if (Utility.RandomBool())
                    {
                        // var t = new VortexTile(); t.MoveToWorld(p, map);
                    }
                    else
                    {
                        // var t = new IceShardTile(); t.MoveToWorld(p, map);
                    }

                    Effects.SendLocationParticles(EffectItem.Create(p, map, EffectItem.DefaultDuration),
                        0x373A, 6, 10, UniqueHue, 0, 2023, 0);
                }
            }

            base.OnDeath(c);
        }

        private Point3D GetRandomValidLocation(Point3D center, int radius, Map map)
        {
            for (int i = 0; i < 10; i++)
            {
                int dx = Utility.RandomMinMax(-radius, radius);
                int dy = Utility.RandomMinMax(-radius, radius);
                var p = new Point3D(center.X + dx, center.Y + dy, center.Z);
                if (Utility.InRange(center, p, radius) && (map.CanFit(p, 16, false, false)))
                    return p;

                p = new Point3D(p.X, p.Y, map.GetAverageZ(p.X, p.Y));
                if (Utility.InRange(center, p, radius) && map.CanFit(p, 16, false, false))
                    return p;
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
            var version = reader.ReadInt();
        }
    }
}
