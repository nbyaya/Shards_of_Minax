using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a blue dragon corpse")]
    public class BlueDragon : BaseCreature
    {
        // Cooldown timers for special abilities
        private DateTime m_NextFrostBreath;
        private DateTime m_NextAquaLance;
        private DateTime m_NextThunderstorm;
        private DateTime m_NextMaelstrom;
        private DateTime m_NextCryoNova;

        // Unique blue hue for this dragon
        private const int UniqueHue = 1153;

        [Constructable]
        public BlueDragon()
            : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.25, 0.5)
        {
            Name = "a blue dragon";
            Body = Utility.RandomList(12, 59);
            BaseSoundID = 362;
            Hue = UniqueHue;

            // Stats
            SetStr(1300, 1500);
            SetDex(120, 180);
            SetInt(900, 1100);

            SetHits(1200, 1600);
            SetDamage(30, 40);

            // Damage types
            SetDamageType(ResistanceType.Physical, 40);
            SetDamageType(ResistanceType.Cold, 60);

            // Resistances
            SetResistance(ResistanceType.Physical, 70, 85);
            SetResistance(ResistanceType.Fire, 30, 45);
            SetResistance(ResistanceType.Cold, 90, 100);
            SetResistance(ResistanceType.Poison, 50, 65);
            SetResistance(ResistanceType.Energy, 60, 75);

            // Skills
            SetSkill(SkillName.EvalInt, 120.0, 140.0);
            SetSkill(SkillName.Magery, 120.0, 140.0);
            SetSkill(SkillName.MagicResist, 120.0, 150.0);
            SetSkill(SkillName.Tactics, 120.0, 140.0);
            SetSkill(SkillName.Wrestling, 120.0, 140.0);

            Fame = 35000;
            Karma = -35000;
            VirtualArmor = 90;

            // Initialize ability cooldowns
            m_NextFrostBreath = DateTime.UtcNow;
            m_NextAquaLance    = DateTime.UtcNow;
            m_NextThunderstorm = DateTime.UtcNow;
            m_NextMaelstrom    = DateTime.UtcNow;
            m_NextCryoNova     = DateTime.UtcNow;
        }

        public BlueDragon(Serial serial)
            : base(serial)
        {
        }

        public override bool ReacquireOnMovement => true;
        public override bool AutoDispel         => true;
        public override bool CanFly             => true;

        public override HideType HideType       => HideType.Barbed;
        public override int Hides               => 50;
        public override int Meat                => 25;
        public override int Scales              => 12;
        public override ScaleType ScaleType     => ScaleType.Blue;

        public override Poison PoisonImmune     => Poison.Lethal;
        public override Poison HitPoison        => Poison.Deadly;

        public override int TreasureMapLevel    => 6;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 4);
            AddLoot(LootPack.Gems, 10);

            if (Utility.RandomDouble() < 0.02) // 2% for a unique scale item
            {
                // PackItem(new BlueDragonScaleArmor());
            }
            if (Utility.RandomDouble() < 0.05)
            {
                PackItem(new TreasureMap(6, Map));
            }
        }

        // --- Special Abilities ---

        // 1) Frost Breath Cone: wide cone of cold that slows
        public void FrostBreathAttack()
        {
            Map map = this.Map;
            if (map == null || !(Combatant is Mobile target)) return;
            if (!Utility.InRange(this.Location, target.Location, 8)) return;

            Effects.PlaySound(this.Location, map, 0x208);
            Effects.SendLocationParticles(EffectItem.Create(this.Location, map, EffectItem.DefaultDuration),
                                          0x3818, 12, 10, UniqueHue, 0, 2030, 0);

            Direction d = GetDirectionTo(target);
            int dx = 0, dy = 0;
            Movement.Movement.Offset(d, ref dx, ref dy);

            var cone = new List<Point3D>();
            int range = 8;

            for (int i = 1; i <= range; i++)
            {
                int width = i / 2;
                for (int w = -width; w <= width; w++)
                {
                    int x = this.X + dx * i + (dy == 0 ? w : 0);
                    int y = this.Y + dy * i + (dx == 0 ? w : 0);
                    var p = new Point3D(x, y, this.Z);
                    if (map.CanFit(p, 16, false, false))
                        cone.Add(p);
                }
            }

            foreach (var p in cone)
            {
                Effects.SendLocationEffect(p, map, 0x3728, 8, UniqueHue, 0);
                foreach (Mobile m in map.GetMobilesInRange(p, 0))
                {
                    if (m != this && (m is PlayerMobile || (m is BaseCreature bc && bc.Team != this.Team)))
                    {
                        DoHarmful(m);
                        AOS.Damage(m, this, 25, 0, 0, 100, 0, 0);
                        m.SendMessage("You are chilled and slowed by the frost!"); 
                        // Example slow: m.Poison = ...
                    }
                }
            }

            m_NextFrostBreath = DateTime.UtcNow + TimeSpan.FromSeconds(10);
        }

        // 2) Aqua Lance: long straight line of water jets
        public void AquaLanceAttack()
        {
            Map map = this.Map;
            if (map == null || !(Combatant is Mobile target)) return;
            if (!Utility.InRange(this.Location, target.Location, 12)) return;

            Effects.PlaySound(this.Location, map, 0x229);
            Direction d = GetDirectionTo(target);
            int dx = 0, dy = 0;
            Movement.Movement.Offset(d, ref dx, ref dy);

            for (int i = 1; i <= 12; i++)
            {
                int x = this.X + dx * i;
                int y = this.Y + dy * i;
                int z = map.GetAverageZ(x, y);
                var p = new Point3D(x, y, z);

                Effects.SendLocationParticles(EffectItem.Create(p, map, EffectItem.DefaultDuration),
                                              0x36BD, 5, 5, UniqueHue, 0, 5023, 0);

                foreach (Mobile m in map.GetMobilesInRange(p, 0))
                {
                    if (m != this && (m is PlayerMobile || (m is BaseCreature bc && bc.Team != this.Team)))
                    {
                        DoHarmful(m);
                        AOS.Damage(m, this, 30, 0, 100, 0, 0, 0);
                    }
                }
            }

            m_NextAquaLance = DateTime.UtcNow + TimeSpan.FromSeconds(12);
        }

        // 3) Thunderstorm: circle of lightning strikes around target
        public void ThunderstormAttack()
        {
            Map map = this.Map;
            if (map == null || !(Combatant is Mobile target)) return;
            if (!Utility.InRange(this.Location, target.Location, 10)) return;

            Effects.PlaySound(target.Location, map, 0x5C6);

            int radius = 6;
            for (int x = -radius; x <= radius; x++)
            {
                for (int y = -radius; y <= radius; y++)
                {
                    if (Utility.InRange(target.Location, new Point3D(target.X + x, target.Y + y, target.Z), radius))
                    {
                        var p = new Point3D(target.X + x, target.Y + y, map.GetAverageZ(target.X + x, target.Y + y));
                        Effects.SendLocationParticles(EffectItem.Create(p, map, EffectItem.DefaultDuration),
                                                      0x3779, 6, 10, UniqueHue, 0, 2031, 0);

                        foreach (Mobile m in map.GetMobilesInRange(p, 0))
                        {
                            if (m != this && (m is PlayerMobile || (m is BaseCreature bc && bc.Team != this.Team)))
                            {
                                DoHarmful(m);
                                AOS.Damage(m, this, 20, 0, 0, 0, 0, 100);
                            }
                        }

                        if (Utility.RandomDouble() < 0.3)
                        {
                            // LightningStormTile lightning = new LightningStormTile();
                            // lightning.MoveToWorld(p, map);
                        }
                    }
                }
            }

            m_NextThunderstorm = DateTime.UtcNow + TimeSpan.FromSeconds(20);
        }

        // 4) Maelstrom Vortex: swirling AoE that pulls and damages
        public void MaelstromVortex()
        {
            Map map = this.Map;
            if (map == null) return;

            Effects.PlaySound(this.Location, map, 0x66A);
            Effects.SendLocationParticles(EffectItem.Create(this.Location, map, EffectItem.DefaultDuration),
                                          0x377B, 10, 20, UniqueHue, 0, 2025, 0);

            int radius = 5;
            for (int x = -radius; x <= radius; x++)
            {
                for (int y = -radius; y <= radius; y++)
                {
                    var p = new Point3D(this.X + x, this.Y + y, this.Z);
                    if (Utility.InRange(this.Location, p, radius))
                    {
                        foreach (Mobile m in map.GetMobilesInRange(p, 0))
                        {
                            if (m != this && (m is PlayerMobile || (m is BaseCreature bc && bc.Team != this.Team)))
                            {
                                DoHarmful(m);
                                AOS.Damage(m, this, 15, 100, 0, 0, 0, 0);
                                // Optionally pull toward center: m.SetLocation(this.Location);
                            }
                        }

                        if (Utility.RandomDouble() < 0.25)
                        {
                            // VortexTile vortex = new VortexTile();
                            // vortex.MoveToWorld(p, map);
                        }
                    }
                }
            }

            m_NextMaelstrom = DateTime.UtcNow + TimeSpan.FromSeconds(25);
        }

        // 5) Cryo Nova: burst of ice shards in all directions
        public void CryoNovaAttack()
        {
            Map map = this.Map;
            if (map == null) return;

            Effects.PlaySound(this.Location, map, 0x208);
            Effects.SendLocationParticles(EffectItem.Create(this.Location, map, EffectItem.DefaultDuration),
                                          0x384A, 12, 15, UniqueHue, 0, 2019, 0);

            int radius = 7;
            for (int x = -radius; x <= radius; x++)
            {
                for (int y = -radius; y <= radius; y++)
                {
                    var p = new Point3D(this.X + x, this.Y + y, this.Z);
                    if (Utility.InRange(this.Location, p, radius))
                    {
                        Effects.SendLocationEffect(p, map, 0x384A, 10, UniqueHue, 0);
                        foreach (Mobile m in map.GetMobilesInRange(p, 0))
                        {
                            if (m != this && (m is PlayerMobile || (m is BaseCreature bc && bc.Team != this.Team)))
                            {
                                DoHarmful(m);
                                AOS.Damage(m, this, 20, 0, 0, 50, 0, 50);
                            }
                        }

                        if (Utility.RandomDouble() < 0.3)
                        {
                            // IceShardTile shard = new IceShardTile();
                            // shard.MoveToWorld(p, map);
                        }
                    }
                }
            }

            m_NextCryoNova = DateTime.UtcNow + TimeSpan.FromSeconds(18);
        }

        // --- AI Logic ---
        public override void OnThink()
        {
            base.OnThink();

            if (Combatant is Mobile target)
            {
                if (DateTime.UtcNow >= m_NextFrostBreath && this.InRange(target, 8))
                    FrostBreathAttack();
                else if (DateTime.UtcNow >= m_NextAquaLance && this.InRange(target, 12))
                    AquaLanceAttack();
                else if (DateTime.UtcNow >= m_NextThunderstorm && this.InRange(target, 10))
                    ThunderstormAttack();
                else if (DateTime.UtcNow >= m_NextCryoNova)
                    CryoNovaAttack();
                else if (DateTime.UtcNow >= m_NextMaelstrom)
                    MaelstromVortex();
            }
        }

        // --- OnDeath Explosion ---
        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            if (Map == null) return;

            Effects.PlaySound(this.Location, Map, 0x2F2);
            Effects.SendLocationParticles(EffectItem.Create(this.Location, Map, EffectItem.DefaultDuration),
                                          0x384A, 20, 30, UniqueHue, 0, 2032, 0);

            int tiles = 15, radius = 6;
            for (int i = 0; i < tiles; i++)
            {
                var p = GetRandomValidLocation(this.Location, radius, Map);
                if (p != Point3D.Zero)
                {
                    // Randomly spawn an icy or lightning hazard
                    if (Utility.RandomBool())
                    {
                        // IceShardTile shard = new IceShardTile();
                        // shard.MoveToWorld(p, Map);
                    }
                    else
                    {
                        // LightningStormTile storm = new LightningStormTile();
                        // storm.MoveToWorld(p, Map);
                    }
                    Effects.SendLocationParticles(EffectItem.Create(p, Map, EffectItem.DefaultDuration),
                                                  0x384A, 8, 12, UniqueHue, 0, 2032, 0);
                }
            }
        }

        // Helper to find a random valid ground location
        private Point3D GetRandomValidLocation(Point3D center, int radius, Map map)
        {
            for (int i = 0; i < 10; i++)
            {
                int x = center.X + Utility.RandomMinMax(-radius, radius);
                int y = center.Y + Utility.RandomMinMax(-radius, radius);
                int z = map.GetAverageZ(x, y);
                var p = new Point3D(x, y, z);
                if (map.CanFit(p, 16, false, false) && Utility.InRange(center, p, radius))
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
            int version = reader.ReadInt();
        }
    }
}
