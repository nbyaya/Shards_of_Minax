using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a dragon corpse")]
    public class YellowDragon : BaseCreature
    {
        // Cooldown timers for special abilities
        private DateTime m_NextSolarFlare;
        private DateTime m_NextThunderRoar;
        private DateTime m_NextRadiantCharge;
        private DateTime m_NextMiasma;
        private DateTime m_NextGoldenVortex;

        // Unique bright-yellow hue
        private const int UniqueHue = 2209;

        [Constructable]
        public YellowDragon()
            : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name = "a yellow dragon";
            Body = Utility.RandomList(12, 59);
            BaseSoundID = 362;
            Hue = UniqueHue;

            // Massive stats
            SetStr(1400, 1600);
            SetDex(200, 250);
            SetInt(900, 1100);

            SetHits(1200, 1600);
            SetDamage(45, 60);

            // Tanky resistances
            SetResistance(ResistanceType.Physical, 75, 85);
            SetResistance(ResistanceType.Fire, 100, 100);
            SetResistance(ResistanceType.Cold, 80, 90);
            SetResistance(ResistanceType.Poison, 70, 80);
            SetResistance(ResistanceType.Energy, 70, 85);

            // Elite skills
            SetSkill(SkillName.EvalInt, 120.0, 140.0);
            SetSkill(SkillName.Magery, 120.0, 140.0);
            SetSkill(SkillName.Meditation, 100.0, 120.0);
            SetSkill(SkillName.MagicResist, 140.0, 180.0);
            SetSkill(SkillName.Tactics, 120.0, 140.0);
            SetSkill(SkillName.Wrestling, 120.0, 140.0);

            Fame = 35000;
            Karma = -35000;
            VirtualArmor = 90;

            // Loot & materials
            PackItem(new TreasureMap(7, Map));
            AddLoot(LootPack.UltraRich, 4);
            AddLoot(LootPack.Gems, 10);

            // Initialize cooldowns
            m_NextSolarFlare    = DateTime.UtcNow;
            m_NextThunderRoar   = DateTime.UtcNow;
            m_NextRadiantCharge = DateTime.UtcNow;
            m_NextMiasma        = DateTime.UtcNow;
            m_NextGoldenVortex  = DateTime.UtcNow;
        }

        public YellowDragon(Serial serial)
            : base(serial)
        {
        }

        // --- Basic Overrides ---
        public override bool CanFly => true;
        public override HideType HideType => HideType.Barbed;
        public override int Hides => 75;
        public override int Meat => 30;
        public override int Scales => 20;
        public override ScaleType ScaleType => ScaleType.Blue;
        public override int TreasureMapLevel => 7;
        public override Poison PoisonImmune => Poison.Lethal;
        public override Poison HitPoison => Poison.Lethal;

        public override void GenerateLoot()
        {
            // Already packed in ctor
        }

        public override int GetIdleSound() => 0x2D3;
        public override int GetHurtSound() => 0x2D1;

        // --- Special Abilities ---

        // 1) Solar Flare: Huge radial fireburst + FlamestrikeHazard tiles
        public void SolarFlareAttack()
        {
            if (!(Combatant is Mobile _)) return; // guard
            Map map = Map;
            if (map == null) return;

            Effects.PlaySound(Location, map, 0x2A5); // blazing roar
            Effects.SendLocationParticles(
                EffectItem.Create(Location, map, EffectItem.DefaultDuration),
                0x36BD, 20, 30, UniqueHue, 0, 5045, 0
            );

            int radius = 6;
            int damage = 50;
            for (int x = -radius; x <= radius; x++)
            {
                for (int y = -radius; y <= radius; y++)
                {
                    Point3D p = new Point3D(X + x, Y + y, Z);
                    if (!Utility.InRange(Location, p, radius)) continue;

                    // Show flame effect on each tile
                    Effects.SendLocationEffect(p, map, 0x3709, 16, UniqueHue, 0);

                    // Damage any mobiles standing here
                    foreach (Mobile m in map.GetMobilesInRange(p, 0))
                    {
                        if (m == this || !m.Alive) continue;
                        DoHarmful(m);
                        AOS.Damage(m, this, damage, 0, 100, 0, 0, 0); // Fire
                    }

                    // 30% chance to leave a lingering Flamestrike tile
                    if (Utility.RandomDouble() < 0.3)
                    {
                        // var tile = new FlamestrikeHazardTile();
                        // tile.MoveToWorld(p, map);
                    }
                }
            }

            m_NextSolarFlare = DateTime.UtcNow + TimeSpan.FromSeconds(20);
        }

        // 2) Thunderous Roar: Outward ring of sonic blasts + TrapWebs
        public void ThunderousRoar()
        {
            if (!(Combatant is Mobile _)) return;
            Map map = Map;
            if (map == null) return;

            Effects.PlaySound(Location, map, 0x55F); // thunder clap
            int ringRadius = 8;
            int dmg = 30;

            for (int i = 0; i < 360; i += 30)
            {
                double rad = i * (Math.PI / 180);
                int tx = X + (int)(ringRadius * Math.Cos(rad));
                int ty = Y + (int)(ringRadius * Math.Sin(rad));
                Point3D p = new Point3D(tx, ty, Z);

                // Ring effect
                Effects.SendLocationParticles(null, 0x3818, 0, UniqueHue, 2030);

                // Damage + slow/stun
                foreach (Mobile m in map.GetMobilesInRange(p, 0))
                {
                    if (m == this || !m.Alive) continue;
                    DoHarmful(m);
                    AOS.Damage(m, this, dmg, 100, 0, 0, 0, 0); // Physical

                    // 20% chance to drop a TrapWeb under them
                    if (Utility.RandomBool())
                    {
                        // var web = new TrapWeb();
                        // web.MoveToWorld(p, map);
                    }
                }
            }

            m_NextThunderRoar = DateTime.UtcNow + TimeSpan.FromSeconds(18);
        }

        // 3) Radiant Charge: Straight beam of lightning + LightningStorm tiles
        public void RadiantChargeAttack()
        {
            if (!(Combatant is Mobile target)) return;
            Map map = Map;
            if (map == null || target.Map != map) return;

            int range = 15;
            if (!Utility.InRange(Location, target.Location, range)) return;

            Direction d = GetDirectionTo(target);
			int dx = 0, dy = 0;
			Movement.Movement.Offset(d, ref dx, ref dy);

            Effects.PlaySound(Location, map, 0x23C);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, map, EffectItem.DefaultDuration),
                0x3728, 10, 10, UniqueHue, 0, 2023, 0
            );

            int dmg = 40;
            for (int i = 1; i <= range; i++)
            {
                Point3D p = new Point3D(X + dx * i, Y + dy * i, Z);
                if (!map.CanFit(p, 16, false, false)) break;

                Effects.SendLocationEffect(p, map, 0x3818, 16, UniqueHue, 0);

                foreach (Mobile m in map.GetMobilesInRange(p, 0))
                {
                    if (m == this || !m.Alive) continue;
                    DoHarmful(m);
                    AOS.Damage(m, this, dmg, 0, 0, 0, 0, 100); // Energy
                }

                // 25% chance to spawn a lightning storm at each tile
                if (Utility.RandomDouble() < 0.25)
                {
                    // var storm = new LightningStormTile();
                    // storm.MoveToWorld(p, map);
                }
            }

            m_NextRadiantCharge = DateTime.UtcNow + TimeSpan.FromSeconds(10);
        }

        // 4) Miasma of Stillness: Radial mana/stamina drain + ManaDrain tiles
        public void MiasmaOfStillness()
        {
            if (!(Combatant is Mobile _)) return;
            Map map = Map;
            if (map == null) return;

            Effects.PlaySound(Location, map, 0x1F7);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, map, EffectItem.DefaultDuration),
                0x37C4, 1, 10, UniqueHue, 0, 9909, 0
            );

            int radius = 5;
            foreach (Mobile m in map.GetMobilesInRange(Location, radius))
            {
                if (m == this || !m.Alive) continue;
                DoHarmful(m);
                // Drain
                m.Stam = Math.Max(0, m.Stam - 50);
                m.Mana = Math.Max(0, m.Mana - 50);
                m.SendMessage("The miasma saps your strength and will!");

                // 40% chance to leave a mana-drain tile
                if (Utility.RandomDouble() < 0.4)
                {
                    // var manaTile = new ManaDrainTile();
                    // manaTile.MoveToWorld(m.Location, map);
                }
            }

            m_NextMiasma = DateTime.UtcNow + TimeSpan.FromSeconds(25);
        }

        // 5) Golden Vortex: Pulling whirlpool + Vortex tiles
        public void GoldenVortexAttack()
        {
            if (!(Combatant is Mobile _)) return;
            Map map = Map;
            if (map == null) return;

            Effects.PlaySound(Location, map, 0x2C5);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, map, EffectItem.DefaultDuration),
                0x375A, 8, 20, UniqueHue, 0, 5018, 0
            );

            int radius = 4;
            int dmg = 35;
            foreach (Mobile m in map.GetMobilesInRange(Location, radius))
            {
                if (m == this || !m.Alive) continue;
                DoHarmful(m);

                // Pull closer
                int pushX = X - m.X, pushY = Y - m.Y;
                m.MoveToWorld(new Point3D(m.X + Math.Sign(pushX), m.Y + Math.Sign(pushY), m.Z), map);

                AOS.Damage(m, this, dmg, 0, 0, 100, 0, 0); // Poison
            }

            // Scatter several vortex tiles
            for (int i = 0; i < 10; i++)
            {
                Point3D p = GetRandomValidLocation(Location, radius, map);
                if (p != Point3D.Zero)
                {
                    // var vortex = new VortexTile();
                    // vortex.MoveToWorld(p, map);
                }
            }

            m_NextGoldenVortex = DateTime.UtcNow + TimeSpan.FromSeconds(22);
        }

        // --- AI Loop ---
        public override void OnThink()
        {
            base.OnThink();

            if (Combatant is Mobile target)
            {
                // Solar Flare if in 12 tiles
                if (DateTime.UtcNow >= m_NextSolarFlare && InRange(target, 12))
                    SolarFlareAttack();
                // Thunder Roar if in 10–16 tiles
                else if (DateTime.UtcNow >= m_NextThunderRoar && Utility.InRange(Location, target.Location, 10))
                    ThunderousRoar();
                // Radiant Charge if in 15 tiles
                else if (DateTime.UtcNow >= m_NextRadiantCharge && InRange(target, 15))
                    RadiantChargeAttack();
                // Miasma always when free
                else if (DateTime.UtcNow >= m_NextMiasma)
                    MiasmaOfStillness();
                // Vortex if in melee range
                else if (DateTime.UtcNow >= m_NextGoldenVortex && InRange(target, 6))
                    GoldenVortexAttack();
            }
        }

        // --- Death Explosion ---
        public override void OnDeath(Container c)
        {
            Map map = Map;
            if (map != null)
            {
                Effects.PlaySound(Location, map, 0x218);
                Effects.SendLocationParticles(
                    EffectItem.Create(Location, map, EffectItem.DefaultDuration),
                    0x3709, 10, 60, UniqueHue, 0, 5052, 0
                );

                // Scatter hazard tiles on death
                for (int i = 0; i < 20; i++)
                {
                    Point3D p = GetRandomValidLocation(Location, 6, map);
                    if (p == Point3D.Zero) continue;

                    // Alternate between Flamestrike and LightningStorm
                    if (Utility.RandomBool())
                    {
                        // var f = new FlamestrikeHazardTile();
                        // f.MoveToWorld(p, map);
                    }
                    else
                    {
                        // var l = new LightningStormTile();
                        // l.MoveToWorld(p, map);
                    }

                    Effects.SendLocationParticles(
                        EffectItem.Create(p, map, EffectItem.DefaultDuration),
                        0x3709, 10, 20, UniqueHue, 0, 5016, 0
                    );
                }
            }

            base.OnDeath(c);
        }

        // Helper: find a valid nearby spot
        private Point3D GetRandomValidLocation(Point3D center, int radius, Map map)
        {
            for (int i = 0; i < 10; i++)
            {
                int dx = Utility.RandomMinMax(-radius, radius);
                int dy = Utility.RandomMinMax(-radius, radius);
                Point3D p = new Point3D(center.X + dx, center.Y + dy, center.Z);
                if (map.CanFit(p, 16, false, false) && Utility.InRange(center, p, radius))
                    return p;

                int z = map.GetAverageZ(p.X, p.Y);
                Point3D p2 = new Point3D(p.X, p.Y, z);
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
