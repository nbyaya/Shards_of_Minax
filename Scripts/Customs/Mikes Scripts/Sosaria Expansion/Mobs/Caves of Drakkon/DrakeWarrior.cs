using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a drake warrior corpse")]
    public class DrakeWarrior : BaseCreature
    {
        // Cooldowns
        private DateTime m_NextFlameBreath;
        private DateTime m_NextEarthshatter;
        private DateTime m_NextToxicCone;
        private DateTime m_NextAetherWard;

        // Unique crimson hue
        private const int UniqueHue = 1154;

        [Constructable]
        public DrakeWarrior()
            : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name = "Drake Warrior";
            Body = 123;                        // same as EtherealWarrior
            Hue = UniqueHue;
            // Stats
            SetStr(800, 900);
            SetDex(300, 350);
            SetInt(600, 700);

            SetHits(2200, 2400);
            SetDamage(25, 35);

            // Resistances
            SetResistance(ResistanceType.Physical, 80, 90);
            SetResistance(ResistanceType.Fire, 60, 70);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 70, 80);
            SetResistance(ResistanceType.Energy, 80, 90);

            // Skills
            SetSkill(SkillName.Anatomy, 100.0, 120.0);
            SetSkill(SkillName.EvalInt, 110.0, 130.0);
            SetSkill(SkillName.Magery, 110.0, 130.0);
            SetSkill(SkillName.Meditation, 100.0, 120.0);
            SetSkill(SkillName.MagicResist, 130.0, 150.0);
            SetSkill(SkillName.Tactics, 110.0, 120.0);
            SetSkill(SkillName.Wrestling, 110.0, 120.0);

            Fame = 20000;
            Karma = -20000;
            VirtualArmor = 100;

            // Start all cooldowns now
            m_NextFlameBreath  = DateTime.UtcNow;
            m_NextEarthshatter = DateTime.UtcNow;
            m_NextToxicCone    = DateTime.UtcNow;
            m_NextAetherWard   = DateTime.UtcNow;
        }

        public DrakeWarrior(Serial serial)
            : base(serial)
        {
        }

        // Sounds from EtherealWarrior
        public override int GetAngerSound()   => 0x2F8;
        public override int GetIdleSound()    => 0x2F8;
        public override int GetAttackSound()  => Utility.Random(0x2F5, 2);
        public override int GetHurtSound()    => 0x2F9;
        public override int GetDeathSound()   => 0x2F7;

        public override int TreasureMapLevel => 5;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 3);
            AddLoot(LootPack.Gems, 5);

            if (Utility.RandomDouble() < 0.05)
                PackItem(new TreasureMap(5, this.Map));
        }

        // ——————————————————————————————————————————————————————
        // |                     SPECIAL ATTACKS                    |
        // ——————————————————————————————————————————————————————

        // 1) Triangular Flame Breath (triangle-shaped cone up to 10 tiles)
        public void FlameBreathAttack()
        {
            if (!(Combatant is Mobile target)) return;
            Map map = Map;
            if (map == null) return;

            const int range = 10;
            const int damage = 30;

            if (!Utility.InRange(Location, target.Location, range))
                return;

            // FX at origin
            Effects.PlaySound(Location, map, 0x227);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, map, TimeSpan.FromSeconds(1.0)),
                0x3709, 20, 30, UniqueHue, 0, 5029, 0);

            // Compute cone as an expanding triangle
            Direction d = GetDirectionTo(target);
			int dx = 0, dy = 0;
			Movement.Movement.Offset(d, ref dx, ref dy);

            for (int i = 1; i <= range; i++)
            {
                int halfWidth = i; // triangular: width grows each step
                for (int off = -halfWidth; off <= halfWidth; off++)
                {
                    int tx = X, ty = Y;
                    if (dx == 0)
                        tx += off;
                    else if (dy == 0)
                        ty += off;
                    else
                    {
                        // diagonal cone
                        if (dx * dy > 0) { tx += off; ty -= off; }
                        else               { tx += off; ty += off; }
                    }

                    tx += i * dx;
                    ty += i * dy;

                    var p = new Point3D(tx, ty, Z);
                    if (!map.CanFit(p, 16, false, false))
                    {
                        p.Z = map.GetAverageZ(tx, ty);
                        if (!map.CanFit(p, 16, false, false))
                            continue;
                    }

                    // flame FX per tile
                    Effects.SendLocationParticles(
                        EffectItem.Create(p, map, TimeSpan.FromSeconds(0.5)),
                        0x36B0, 5, 10, UniqueHue, 0, 2023, 0);

                    // damage any mobiles on that tile
                    var list = map.GetMobilesInRange(p, 0);
                    foreach (Mobile m in list)
                    {
                        if (m != this && (m is PlayerMobile || m is BaseCreature))
                        {
                            DoHarmful(m);
                            AOS.Damage(m, this, damage, 100, 0, 0, 0, 0);
                        }
                    }
                    list.Free();

                    // 20% chance to leave a lingering hazard
                    if (Utility.RandomDouble() < 0.2)
                    {
                        // var tile = new FlamestrikeHazardTile();
                        // tile.MoveToWorld(p, map);
                    }
                }
            }

            m_NextFlameBreath = DateTime.UtcNow + TimeSpan.FromSeconds(12);
        }

        // 2) Earthshatter Line (a straight line of earth spikes)
        public void EarthshatterAttack()
        {
            if (!(Combatant is Mobile target)) return;
            Map map = Map;
            if (map == null) return;

            const int length = 12;
            const int damage = 35;

            // must be within 12 for line
            if (!Utility.InRange(Location, target.Location, length))
                return;

            Direction d = GetDirectionTo(target);
			int dx = 0, dy = 0;
			Movement.Movement.Offset(d, ref dx, ref dy);

            Effects.PlaySound(Location, map, 0x2F3);

            for (int i = 1; i <= length; i++)
            {
                int tx = X + dx * i;
                int ty = Y + dy * i;
                var p = new Point3D(tx, ty, Z);

                if (!map.CanFit(p, 16, false, false))
                {
                    p.Z = map.GetAverageZ(tx, ty);
                    if (!map.CanFit(p, 16, false, false))
                        break; // stop at obstacle
                }

                Effects.SendLocationParticles(
                    EffectItem.Create(p, map, TimeSpan.FromSeconds(0.5)),
                    0x3709, 8, 20, UniqueHue, 0, 5029, 0);

                var list = map.GetMobilesInRange(p, 0);
                foreach (Mobile m in list)
                {
                    if (m != this && (m is PlayerMobile || m is BaseCreature))
                    {
                        DoHarmful(m);
                        AOS.Damage(m, this, damage, 100, 0, 0, 0, 0);
                    }
                }
                list.Free();

                if (Utility.RandomDouble() < 0.15)
                {
                    // var quake = new EarthquakeTile();
                    // quake.MoveToWorld(p, map);
                }
            }

            m_NextEarthshatter = DateTime.UtcNow + TimeSpan.FromSeconds(18);
        }

        // 3) Toxic Cone Barrage (short-range conical poison gas)
        public void ToxicConeAttack()
        {
            if (!(Combatant is Mobile target)) return;
            Map map = Map;
            if (map == null) return;

            const int range = 7;
            const int damage = 25;

            if (!Utility.InRange(Location, target.Location, range))
                return;

            Effects.PlaySound(Location, map, BaseSoundID);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, map, TimeSpan.FromSeconds(1.0)),
                0x3709, 15, 25, UniqueHue, 0, 5052, 0);

            Direction d = GetDirectionTo(target);
			int dx = 0, dy = 0;
			Movement.Movement.Offset(d, ref dx, ref dy);

            // simple cone: width = 3 at max distance
            for (int i = 1; i <= range; i++)
            {
                int half = (int)(i * (3.0 / range));
                for (int off = -half; off <= half; off++)
                {
                    int tx = X + dx * i;
                    int ty = Y + dy * i;
                    if (dx == 0) tx += off;
                    else if (dy == 0) ty += off;
                    else
                    {
                        if (dx * dy > 0) { tx += off; ty -= off; }
                        else               { tx += off; ty += off; }
                    }

                    var p = new Point3D(tx, ty, Z);
                    if (!map.CanFit(p, 16, false, false))
                    {
                        p.Z = map.GetAverageZ(tx, ty);
                        if (!map.CanFit(p, 16, false, false))
                            continue;
                    }

                    Effects.SendLocationParticles(
                        EffectItem.Create(p, map, TimeSpan.FromSeconds(0.5)),
                        0x36B0, 6, 12, UniqueHue, 0, 2023, 0);

                    var list = map.GetMobilesInRange(p, 0);
                    foreach (Mobile m in list)
                    {
                        if (m != this && (m is PlayerMobile || m is BaseCreature))
                        {
                            DoHarmful(m);
                            AOS.Damage(m, this, damage, 0, 0, 100, 0, 0);
                            // m.ApplyPoison(this, Poison.Deadly);
                        }
                    }
                    list.Free();

                    if (Utility.RandomDouble() < 0.3)
                    {
                        // var gas = new ToxicGasTile();
                        // gas.MoveToWorld(p, map);
                    }
                }
            }

            m_NextToxicCone = DateTime.UtcNow + TimeSpan.FromSeconds(14);
        }

        // 4) Aether Ward (buff/debuff zone around self)
        public void AetherWardAbility()
        {
            Map map = Map;
            if (map == null) return;

            const int radius = 5;
            Effects.PlaySound(Location, map, 0x1F2);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, map, TimeSpan.FromSeconds(1.0)),
                0x37C4, 1, 10, UniqueHue, 0, 9909, 0);

            for (int dx = -radius; dx <= radius; dx++)
            {
                for (int dy = -radius; dy <= radius; dy++)
                {
                    var p = new Point3D(X + dx, Y + dy, Z);
                    if (Utility.InRange(Location, p, radius) && map.CanFit(p, 16, false, false))
                    {
                        Effects.SendLocationParticles(
                            EffectItem.Create(p, map, TimeSpan.FromSeconds(0.5)),
                            0x37CB, 1, 10, UniqueHue, 0, 9909, 0);

                        if (Utility.RandomDouble() < 0.3)
                        {
                            // var tile = Utility.RandomBool()
                            //     ? new HealingPulseTile()
                            //     : new ManaDrainTile();
                            // tile.MoveToWorld(p, map);
                        }
                    }
                }
            }

            m_NextAetherWard = DateTime.UtcNow + TimeSpan.FromSeconds(20);
        }

        // ——————————————————————————————————————————————————————
        // |                        AI SELECTION                       |
        // ——————————————————————————————————————————————————————
        public override void OnThink()
        {
            base.OnThink();

            if (Combatant is Mobile target)
            {
                // pick an ability if off cooldown and in range
                if (DateTime.UtcNow >= m_NextFlameBreath && this.InRange(target, 10))
                    FlameBreathAttack();
                else if (DateTime.UtcNow >= m_NextEarthshatter && this.InRange(target, 12))
                    EarthshatterAttack();
                else if (DateTime.UtcNow >= m_NextToxicCone && this.InRange(target, 7))
                    ToxicConeAttack();
                else if (DateTime.UtcNow >= m_NextAetherWard)
                    AetherWardAbility();
            }
        }

        // ——————————————————————————————————————————————————————
        // |                      DEATH EXPLOSION                      |
        // ——————————————————————————————————————————————————————
        public override void OnDeath(Container c)
        {
            if (Map != null)
            {
                Effects.PlaySound(Location, Map, 0x218);
                Effects.SendLocationParticles(
                    EffectItem.Create(Location, Map, TimeSpan.FromSeconds(1.0)),
                    0x3709, 10, 60, UniqueHue, 0, 5052, 0);

                const int shards = 12, rad = 6;
                for (int i = 0; i < shards; i++)
                {
                    var p = GetRandomValidLocation(Location, rad, Map);
                    if (p == Point3D.Zero) continue;

                    // var tile = Utility.RandomBool()
                    //     ? (Tile)new LandmineTile()
                    //     : new TrapWeb();
                    // tile.MoveToWorld(p, Map);

                    Effects.SendLocationParticles(
                        EffectItem.Create(p, Map, TimeSpan.FromSeconds(0.5)),
                        0x3709, 8, 20, UniqueHue, 0, 5016, 0);
                }
            }

            base.OnDeath(c);
        }

        private Point3D GetRandomValidLocation(Point3D center, int radius, Map map)
        {
            for (int i = 0; i < 10; i++)
            {
                int x = center.X + Utility.RandomMinMax(-radius, radius);
                int y = center.Y + Utility.RandomMinMax(-radius, radius);
                int z = center.Z;
                var p = new Point3D(x, y, z);

                if (map.CanFit(p, 16, false, false) && Utility.InRange(center, p, radius))
                    return p;

                z = map.GetAverageZ(x, y);
                p = new Point3D(x, y, z);
                if (map.CanFit(p, 16, false, false) && Utility.InRange(center, p, radius))
                    return p;
            }

            return Point3D.Zero;
        }

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
