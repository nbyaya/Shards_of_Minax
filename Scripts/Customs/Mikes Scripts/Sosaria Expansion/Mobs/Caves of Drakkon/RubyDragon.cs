using System;
using System.Collections.Generic;

using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a ruby dragon corpse")]
    public class RubyDragon : BaseCreature
    {
        // Cooldown timers
        private DateTime m_NextInfernoCone;
        private DateTime m_NextCrystalShardRain;
        private DateTime m_NextPrismBurst;
        private DateTime m_NextRubyResonance;
        private DateTime m_NextTeleportPulse;

        // Unique ruby hue
        private const int RubyHue = 1154;

        [Constructable]
        public RubyDragon()
            : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name = "a ruby dragon";
            Body = Utility.RandomList(12, 59);
            BaseSoundID = 362;
            Hue = RubyHue;

            // Enhanced attributes
            SetStr(1500, 1600);
            SetDex(150, 200);
            SetInt(900, 1000);

            SetHits(1500, 1800);
            SetDamage(40, 50);

            // Resistances
            SetResistance(ResistanceType.Physical, 80, 90);
            SetResistance(ResistanceType.Fire, 100, 110);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 70, 80);
            SetResistance(ResistanceType.Energy, 70, 80);

            // Skills
            SetSkill(SkillName.EvalInt, 120.0, 140.0);
            SetSkill(SkillName.Magery, 120.0, 140.0);
            SetSkill(SkillName.MagicResist, 130.0, 170.0);
            SetSkill(SkillName.Tactics, 110.0, 130.0);
            SetSkill(SkillName.Wrestling, 110.0, 130.0);

            Fame = 40000;
            Karma = -40000;
            VirtualArmor = 100;

            // Cooldowns start now
            var now = DateTime.UtcNow;
            m_NextInfernoCone      = now;
            m_NextCrystalShardRain = now;
            m_NextPrismBurst       = now;
            m_NextRubyResonance    = now;
            m_NextTeleportPulse    = now;
        }

        public RubyDragon(Serial serial)
            : base(serial)
        {
        }

        // Basic overrides
        public override bool ReacquireOnMovement => true;
        public override bool AutoDispel         => true;

        public override HideType HideType      => HideType.Barbed;
        public override int Hides              => 60;
        public override int Meat               => 30;
        public override int Scales             => 12;
        public override ScaleType ScaleType    => ScaleType.Red;

        public override Poison PoisonImmune    => Poison.Lethal;
        public override Poison HitPoison       => Poison.Deadly;

        public override int TreasureMapLevel   => 7;
        public override bool CanFly            => true;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 5);
            AddLoot(LootPack.Gems, 10);

            if (Utility.RandomDouble() < 0.02) // 2% chance for a special ruby-plate
                PackItem(new ThreshkingsHat());

            if (Utility.RandomDouble() < 0.10) // 10% chance for a level‑7 map
                PackItem(new TreasureMap(7, Map));
        }

        public override int GetIdleSound() => 362;
        public override int GetHurtSound() => 363;
        public override int GetDeathSound() => 364;

        // --- Special Abilities ---

        // 1) Inferno Cone: wide flame cone that leaves burning lava tiles
        public void InfernoConeAttack()
        {
			if (!(Combatant is Mobile)) return;
			Mobile target = (Mobile)Combatant;

            var map = Map;
            if (map == null) return;

            int range = 10, width = 4, damage = 30;
            Effects.PlaySound(Location, map, 0x208);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, map, EffectItem.DefaultDuration),
                0x3709, 12, 30, RubyHue, 0, 5052, 0);

            Direction dir = GetDirectionTo(target);
			int dx = 0, dy = 0;
			Movement.Movement.Offset(dir, ref dx, ref dy);


            for (int i = 1; i <= range; i++)
            {
                int radius = (i * width) / range;
                for (int offset = -radius; offset <= radius; offset++)
                {
                    int x = X + i*dx;
                    int y = Y + i*dy;
                    // spread perpendicular
                    if (dx == 0) x += offset; else if (dy == 0) y += offset;
                    else { x += offset; y += offset; }

                    var p = new Point3D(x, y, Z);
                    if (!map.CanFit(p, 16, false, false))
                        p = new Point3D(x, y, map.GetAverageZ(x,y));

                    Effects.SendLocationParticles(
                        EffectItem.Create(p, map, EffectItem.DefaultDuration),
                        0x374A, 8, 8, RubyHue, 0, 2023, 0);

                    // leave lava
                    if (Utility.RandomDouble() < 0.4)
                    {
                        var lava = new HotLavaTile();
                        lava.MoveToWorld(p, map);
                    }

                    // damage
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
            }

            m_NextInfernoCone = DateTime.UtcNow.AddSeconds(12);
        }

        // 2) Crystal Shard Rain: radial burst of piercing shards (landmines/ice shards)
        public void CrystalShardRainAttack()
        {
            var map = Map;
            if (map == null) return;

            Effects.PlaySound(Location, map, 0x1FE);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, map, EffectItem.DefaultDuration),
                0x3728, 10, 12, RubyHue, 0, 2023, 0);

            int shards = 20, radius = 8, damage = 25;
            for (int i = 0; i < shards; i++)
            {
                double theta = Utility.RandomDouble() * Math.PI * 2;
                int dist = Utility.RandomMinMax(1, radius);
                int px = X + (int)(Math.Cos(theta)*dist);
                int py = Y + (int)(Math.Sin(theta)*dist);
                var p = new Point3D(px, py, Z);
                if (!map.CanFit(p,16,false,false))
                    p = new Point3D(px, py, map.GetAverageZ(px,py));

                // spawn either ice shard or landmine
                if (Utility.RandomBool())
                {
                    var ice = new IceShardTile();
                    ice.MoveToWorld(p, map);
                }
                else
                {
                    var mine = new LandmineTile();
                    mine.MoveToWorld(p, map);
                }

                // small impact effect
                Effects.SendLocationParticles(
                    EffectItem.Create(p, map, EffectItem.DefaultDuration),
                    0x36BD, 6, 6, RubyHue, 0, 5029, 0);

                // damage anyone standing
                foreach (Mobile m in map.GetMobilesInRange(p, 0))
                {
                    if (m == this) continue;
                    if (m is PlayerMobile || (m is BaseCreature bc && bc.Team != Team))
                    {
                        DoHarmful(m);
                        AOS.Damage(m, this, damage, 100, 0, 0, 0, 0);
                    }
                }
            }

            m_NextCrystalShardRain = DateTime.UtcNow.AddSeconds(15);
        }

        // 3) Prism Burst: line of pure energy that spawns lightning storms
        public void PrismBurstAttack()
        {
			if (!(Combatant is Mobile)) return;
			Mobile target = (Mobile)Combatant;
            var map = Map;
            if (map == null) return;

            int range = 20, damage = 35;
            Effects.PlaySound(Location, map, 0x20A);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, map, EffectItem.DefaultDuration),
                0x373A, 15, 15, RubyHue, 0, 9934, 0);

            Direction dir = GetDirectionTo(target);
			int dx = 0, dy = 0;
			Movement.Movement.Offset(dir, ref dx, ref dy);


            for (int i = 1; i <= range; i++)
            {
                var p = new Point3D(X + i*dx, Y + i*dy, Z);
                if (!map.CanFit(p,16,false,false))
                    p = new Point3D(p.X, p.Y, map.GetAverageZ(p.X,p.Y));

                Effects.SendLocationEffect(p, map, 0x3818, 10, RubyHue, 0);

                // spawn random storm tile
                if (Utility.RandomBool())
                {
                    var storm = new LightningStormTile();
                    storm.MoveToWorld(p, map);
                }
                else
                {
                    var vortex = new VortexTile();
                    vortex.MoveToWorld(p, map);
                }

                foreach (Mobile m in map.GetMobilesInRange(p, 0))
                {
                    if (m == this) continue;
                    if (m is PlayerMobile || (m is BaseCreature bc && bc.Team != Team))
                    {
                        DoHarmful(m);
                        AOS.Damage(m, this, damage, 0, 0, 0, 100, 0);
                    }
                }
            }

            m_NextPrismBurst = DateTime.UtcNow.AddSeconds(10);
        }

        // 4) Ruby Resonance: radial pulse that drains mana and poisons
        public void RubyResonanceAbility()
        {
            var map = Map;
            if (map == null) return;

            int radius = 6;
            Effects.PlaySound(Location, map, 0x1F1);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, map, EffectItem.DefaultDuration),
                0x37C4, 8, 20, RubyHue, 0, 9909, 0);

            foreach (Mobile m in map.GetMobilesInRange(Location, radius))
            {
                if (m == this) continue;
                if (m is PlayerMobile || (m is BaseCreature bc && bc.Team != Team))
                {
                    DoHarmful(m);
                    int drain = Utility.RandomMinMax(20, 40);
                    m.Mana = Math.Max(0, m.Mana - drain);
                    // spawn mana drain tile
                    if (Utility.RandomDouble() < 0.3)
                    {
                        var mana = new ManaDrainTile();
                        mana.MoveToWorld(m.Location, map);
                    }
                    // apply poison
                    m.ApplyPoison(this, Poison.Deadly);
                }
                // spawn healing pulse near self
                else if (m == this && Utility.RandomDouble() < 0.2)
                {
                    var heal = new HealingPulseTile();
                    heal.MoveToWorld(Location, map);
                }
            }

            m_NextRubyResonance = DateTime.UtcNow.AddSeconds(20);
        }

        // 5) Teleport Pulse: blink & explosive area
        public void TeleportPulseAbility()
        {
			if (!(Combatant is Mobile)) return;
			Mobile target = (Mobile)Combatant;

            var map = Map;
            if (map == null) return;

            // blink behind the target
            int tx = target.X, ty = target.Y;
            var dest = new Point3D(tx - (target.X - X), ty - (target.Y - Y), target.Z);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, map, EffectItem.DefaultDuration),
                0x3728, 8, 8, RubyHue, 0, 2033, 0);
            Location = dest;
            Effects.PlaySound(Location, map, 0x1FE);

            // small radial burst
            int radius = 4, damage = 50;
            for (int x = -radius; x <= radius; x++)
            for (int y = -radius; y <= radius; y++)
            {
                var p = new Point3D(X + x, Y + y, Z);
                if (!map.CanFit(p,16,false,false) || !Utility.InRange(Location, p, radius))
                    continue;

                Effects.SendLocationParticles(
                    EffectItem.Create(p, map, EffectItem.DefaultDuration),
                    0x3709, 8, 12, RubyHue, 0, 5052, 0);

                // spawn necro flame
                if (Utility.RandomBool())
                {
                    var nf = new NecromanticFlamestrikeTile();
                    nf.MoveToWorld(p, map);
                }

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

            m_NextTeleportPulse = DateTime.UtcNow.AddSeconds(18);
        }

        // AI logic
        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null) return;
            var target = Combatant as Mobile;
            if (target == null) return;

            var now = DateTime.UtcNow;
            if (now >= m_NextPrismBurst && InRange(target, 20))
                PrismBurstAttack();
            else if (now >= m_NextInfernoCone && InRange(target, 10))
                InfernoConeAttack();
            else if (now >= m_NextCrystalShardRain && InRange(target, 8))
                CrystalShardRainAttack();
            else if (now >= m_NextTeleportPulse && InRange(target, 5))
                TeleportPulseAbility();
            else if (now >= m_NextRubyResonance)
                RubyResonanceAbility();
        }

        // Scatter shards on death
        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            if (Map == null) return;
            Effects.PlaySound(Location, Map, 0x208);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                0x3709, 16, 60, RubyHue, 0, 5052, 0);

            // drop a few hot lava & landmine shards
            for (int i = 0; i < 15; i++)
            {
                var p = GetRandomDirectionLocation(Location, 6);
                if (p == Point3D.Zero) continue;

                if (Utility.RandomBool())
                {
                    var lava = new HotLavaTile();
                    lava.MoveToWorld(p, Map);
                }
                else
                {
                    var mine = new LandmineTile();
                    mine.MoveToWorld(p, Map);
                }

                Effects.SendLocationParticles(
                    EffectItem.Create(p, Map, EffectItem.DefaultDuration),
                    0x373A, 8, 12, RubyHue, 0, 2023, 0);
            }
        }

        // helper for death scatter
        private Point3D GetRandomDirectionLocation(Point3D center, int radius)
        {
            for (int i = 0; i < 10; i++)
            {
                int dx = Utility.RandomMinMax(-radius, radius);
                int dy = Utility.RandomMinMax(-radius, radius);
                var p = new Point3D(center.X + dx, center.Y + dy, center.Z);
                if (Map.CanFit(p, 16, false, false) && Utility.InRange(center, p, radius))
                    return p;

                p = new Point3D(p.X, p.Y, Map.GetAverageZ(p.X, p.Y));
                if (Map.CanFit(p, 16, false, false) && Utility.InRange(center, p, radius))
                    return p;
            }
            return Point3D.Zero;
        }

        // Serialization
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            _ = reader.ReadInt();
        }
    }
}
