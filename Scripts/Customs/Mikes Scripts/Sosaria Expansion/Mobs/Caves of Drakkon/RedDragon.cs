using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;   // For SpellHelper, if needed

namespace Server.Mobiles
{
    [CorpseName("the smoldering remains of a red dragon")]
    public class RedDragon : BaseCreature
    {
        // Cooldown timers for special abilities
        private DateTime m_NextCrimsonWave;
        private DateTime m_NextShardShower;
        private DateTime m_NextBloodRoar;
        private DateTime m_NextFireWard;
        private DateTime m_NextEmberStorm;

        // Unique red hue
        private const int UniqueHue = 1161;

        [Constructable]
        public RedDragon()
            : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name = "a red dragon";
            Body = Utility.RandomList(12, 59);
            BaseSoundID = 362;
            Hue = UniqueHue;

            // Enhanced stats
            SetStr(1300, 1500);
            SetDex(150, 200);
            SetInt(900, 1100);

            SetHits(1500, 2000);
            SetDamage(40, 55);

            // Resistances
            SetResistance(ResistanceType.Physical, 75, 85);
            SetResistance(ResistanceType.Fire, 95, 100);
            SetResistance(ResistanceType.Cold, 60, 75);
            SetResistance(ResistanceType.Poison, 60, 75);
            SetResistance(ResistanceType.Energy, 70, 85);

            // Skills
            SetSkill(SkillName.EvalInt,    120.0, 140.0);
            SetSkill(SkillName.Magery,     120.0, 140.0);
            SetSkill(SkillName.Meditation,  80.0, 100.0);
            SetSkill(SkillName.MagicResist,140.0, 180.0);
            SetSkill(SkillName.Tactics,    120.0, 140.0);
            SetSkill(SkillName.Wrestling,  120.0, 140.0);

            Fame = 35000;
            Karma = -35000;
            VirtualArmor = 90;

            // Loot
            m_NextCrimsonWave   = DateTime.UtcNow;
            m_NextShardShower   = DateTime.UtcNow;
            m_NextBloodRoar     = DateTime.UtcNow;
            m_NextFireWard      = DateTime.UtcNow;
            m_NextEmberStorm    = DateTime.UtcNow;
        }

        public RedDragon(Serial serial) : base(serial) { }

        // --- Properties ---
        public override bool ReacquireOnMovement => true;
        public override bool AutoDispel         => true;

        public override HideType HideType       => HideType.Barbed;
        public override int       Hides         => 60;
        public override int       Meat          => 25;
        public override int       Scales        => 20;
        public override ScaleType ScaleType     => ScaleType.Red;

        public override Poison PoisonImmune     => Poison.Lethal;
        public override Poison HitPoison        => Poison.Deadly;

        public override int TreasureMapLevel    => 7;
        public override bool CanFly             => true;

        // --- Loot Generation ---
        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 5);
            AddLoot(LootPack.Gems,      10);

            if (Utility.RandomDouble() < 0.02) // 2% chance
                PackItem(new DragonsHeartBreastplate());

            if (Utility.RandomDouble() < 0.05) // 5% chance for level-7 map
                PackItem(new TreasureMap(7, Map));
        }

        // --- Special Abilities ---

        // 1) Crimson Flame Wave: a wide cone of searing flame, leaves HotLavaTile
        public void CrimsonWaveAttack()
        {
            if (!(Combatant is Mobile target)) return;
            Map map = Map;
            if (map == null || !InRange(target, 12)) return;

            int coneRange = 12, coneWidth = 5, damage = 35;
            Effects.PlaySound(Location, map, 0x208);
            Effects.SendLocationParticles(EffectItem.Create(Location, map, EffectItem.DefaultDuration),
                                          0x3709, 12, 30, UniqueHue, 0, 5052, 0);

            Direction d = GetDirectionTo(target);
            int dx = 0, dy = 0;
            Movement.Movement.Offset(d, ref dx, ref dy);

            var coneLocs = new List<Point3D>();
            for (int i = 1; i <= coneRange; i++)
            {
                int halfWidth = (int)(i * (coneWidth / (double)coneRange));
                for (int j = -halfWidth; j <= halfWidth; j++)
                {
                    int tx = X + i*dx, ty = Y + i*dy;
                    if (dx == 0) tx += j;
                    else if (dy == 0) ty += j;
                    else { tx += j; ty += j * (dx*dy > 0 ? -1 : 1); } 

                    var p = new Point3D(tx, ty, Z);
                    if (!map.CanFit(p, 16, false, false))
                    {
                        int z2 = map.GetAverageZ(tx, ty);
                        p = new Point3D(tx, ty, z2);
                        if (!map.CanFit(p, 16, false, false)) continue;
                    }
                    coneLocs.Add(p);
                }
            }

            foreach (var p in coneLocs)
            {
                Effects.SendLocationParticles(EffectItem.Create(p, map, EffectItem.DefaultDuration),
                                              0x36B0, 8, 8, UniqueHue, 0, 2023, 0);

                foreach (Mobile m in map.GetMobilesInRange(p, 0))
                {
                    if (m == this) continue;
                    if (m is PlayerMobile || (m is BaseCreature bc && bc.Team != Team))
                    {
                        DoHarmful(m);
                        AOS.Damage(m, this, damage, 0, 100, 0, 0, 0);
                    }
                }

                if (Utility.RandomDouble() < 0.4) // 40%
                {
                    // new HotLavaTile().MoveToWorld(p, map);
                }
            }

            m_NextCrimsonWave = DateTime.UtcNow + TimeSpan.FromSeconds(10);
        }

        // 2) Molten Shard Shower: radial hail of burning shard‐mines
        public void ShardShowerAttack()
        {
            Map map = Map;
            if (map == null) return;

            int radius = 6, damage = 30;
            Effects.PlaySound(Location, map, 0x225);
            Effects.SendLocationParticles(EffectItem.Create(Location, map, EffectItem.DefaultDuration),
                                          0x373A, 10, 20, UniqueHue, 0, 6001, 0);

            for (int x = -radius; x <= radius; x++)
            for (int y = -radius; y <= radius; y++)
            {
                var p = new Point3D(X + x, Y + y, Z);
                if (!map.CanFit(p,16,false,false) || !Utility.InRange(Location, p, radius))
                {
                    int z2 = map.GetAverageZ(p.X,p.Y);
                    p = new Point3D(p.X,p.Y,z2);
                    if (!map.CanFit(p,16,false,false) || !Utility.InRange(Location,p,radius))
                        continue;
                }

                Effects.SendLocationParticles(EffectItem.Create(p, map, EffectItem.DefaultDuration),
                                              0x36DE, 6, 10, UniqueHue, 0, 2023, 0);

                foreach (Mobile m in map.GetMobilesInRange(p, 0))
                {
                    if (m == this) continue;
                    if (m is PlayerMobile || (m is BaseCreature bc && bc.Team != Team))
                    {
                        DoHarmful(m);
                        AOS.Damage(m, this, damage, 100, 0, 0, 0, 0);
                    }
                }

                if (Utility.RandomDouble() < 0.3)
                {
                    // new LandmineTile().MoveToWorld(p, map);
                }
            }

            m_NextShardShower = DateTime.UtcNow + TimeSpan.FromSeconds(15);
        }

        // 3) Bloodthirst Roar: radial damage + stamina drain + poison
        public void BloodRoarAttack()
        {
            Map map = Map;
            if (map == null) return;

            int radius = 8, damage = 25;
            Effects.PlaySound(Location, map, 0x5C3);
            Effects.SendLocationParticles(EffectItem.Create(Location, map, EffectItem.DefaultDuration),
                                          0x3728, 8, 20, UniqueHue, 0, 2023, 0);

            foreach (Mobile m in map.GetMobilesInRange(Location, radius))
            {
                if (m == this) continue;
                if (m is PlayerMobile || (m is BaseCreature bc && bc.Team != Team))
                {
                    DoHarmful(m);
                    AOS.Damage(m, this, damage, 100, 0, 0, 0, 0);
                    m.Stam = Math.Max(0, m.Stam - Utility.RandomMinMax(20, 30));
                    m.SendMessage("The Red Dragon's roar saps your strength!");
                    m.ApplyPoison(this, Poison.Deadly);
                }
            }

            m_NextBloodRoar = DateTime.UtcNow + TimeSpan.FromSeconds(20);
        }

        // 4) Fire Ward: spawning healing & draining tiles in a circle
        public void FireWardAbility()
        {
            Map map = Map;
            if (map == null) return;

            int radius = 5;
            Effects.PlaySound(Location, map, 0x208);
            Effects.SendLocationParticles(EffectItem.Create(Location, map, EffectItem.DefaultDuration),
                                          0x37C4, 1, 10, UniqueHue, 0, 9909, 0);

            for (int x = -radius; x <= radius; x++)
            for (int y = -radius; y <= radius; y++)
            {
                var p = new Point3D(X + x, Y + y, Z);
                if (!map.CanFit(p,16,false,false) || !Utility.InRange(Location, p, radius))
                {
                    int z2 = map.GetAverageZ(p.X,p.Y);
                    p = new Point3D(p.X,p.Y,z2);
                    if (!map.CanFit(p,16,false,false) || !Utility.InRange(Location,p,radius))
                        continue;
                }

                Effects.SendLocationParticles(EffectItem.Create(p, map, EffectItem.DefaultDuration),
                                              0x37CB, 1, 10, UniqueHue, 0, 9909, 0);

                if (Utility.RandomDouble() < 0.4)
                {
                    if (Utility.RandomBool())
                    {
                        // new HealingPulseTile().MoveToWorld(p, map);
                    }
                    else
                    {
                        // new ManaDrainTile().MoveToWorld(p, map);
                    }
                }
            }

            m_NextFireWard = DateTime.UtcNow + TimeSpan.FromSeconds(25);
        }

        // 5) Ember Storm: rotating ring of lightning/fire around self
        public void EmberStormAttack()
        {
            Map map = Map;
            if (map == null) return;

            int radius = 4, damage = 30;
            Effects.PlaySound(Location, map, 0x207);
            Effects.SendLocationParticles(EffectItem.Create(Location, map, EffectItem.DefaultDuration),
                                          0x3709, 12, 30, UniqueHue, 0, 5052, 0);

            // ring of points
            for (int angle = 0; angle < 360; angle += 30)
            {
                double rad = angle * (Math.PI/180);
                int tx = X + (int)Math.Round(radius * Math.Cos(rad));
                int ty = Y + (int)Math.Round(radius * Math.Sin(rad));
                int tz = map.GetAverageZ(tx, ty);
                var p = new Point3D(tx, ty, tz);

                Effects.SendLocationParticles(EffectItem.Create(p, map, EffectItem.DefaultDuration),
                                              0x3818, 8, 8, UniqueHue, 0, 2023, 0);

                foreach (Mobile m in map.GetMobilesInRange(p, 0))
                {
                    if (m == this) continue;
                    if (m is PlayerMobile || (m is BaseCreature bc && bc.Team != Team))
                    {
                        DoHarmful(m);
                        AOS.Damage(m, this, damage, 0, 0, 0, 0, 100);
                    }
                }

                // new LightningStormTile().MoveToWorld(p, map);
            }

            m_NextEmberStorm = DateTime.UtcNow + TimeSpan.FromSeconds(18);
        }

        // --- AI Logic ---
        public override void OnThink()
        {
            base.OnThink();

            if (Combatant is Mobile target)
            {
                if (DateTime.UtcNow >= m_NextCrimsonWave && InRange(target, 12))
                    CrimsonWaveAttack();
                else if (DateTime.UtcNow >= m_NextShardShower)
                    ShardShowerAttack();
                else if (DateTime.UtcNow >= m_NextBloodRoar)
                    BloodRoarAttack();
                else if (DateTime.UtcNow >= m_NextFireWard)
                    FireWardAbility();
                else if (DateTime.UtcNow >= m_NextEmberStorm)
                    EmberStormAttack();
            }
        }

        // --- OnDeath Explosion ---
        public override void OnDeath(Container c)
        {
            Map map = Map;
            if (map != null)
            {
                Effects.PlaySound(Location, map, 0x214);
                Effects.SendLocationParticles(EffectItem.Create(Location, map, EffectItem.DefaultDuration),
                                              0x3709, 20, 60, UniqueHue, 0, 5052, 0);

                int count = 30, radius = 8;
                for (int i = 0; i < count; i++)
                {
                    var p = GetRandomValidLocation(Location, radius, map);
                    if (p == Point3D.Zero) continue;

                    if (Utility.RandomBool())
                    {
                        // new FlamestrikeHazardTile().MoveToWorld(p, map);
                    }
                    else
                    {
                        // new ToxicGasTile().MoveToWorld(p, map);
                    }

                    Effects.SendLocationParticles(EffectItem.Create(p, map, EffectItem.DefaultDuration),
                                                  0x3709, 10, 20, UniqueHue, 0, 5016, 0);
                }
            }

            base.OnDeath(c);
        }

        // Helper for OnDeath
        private Point3D GetRandomValidLocation(Point3D center, int radius, Map map)
        {
            for (int i = 0; i < 10; i++)
            {
                int dx = Utility.RandomMinMax(-radius, radius);
                int dy = Utility.RandomMinMax(-radius, radius);
                var p = new Point3D(center.X + dx, center.Y + dy, center.Z);

                if (map.CanFit(p, 16, false, false) && Utility.InRange(center, p, radius))
                    return p;

                int z2 = map.GetAverageZ(p.X, p.Y);
                var p2 = new Point3D(p.X, p.Y, z2);

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
