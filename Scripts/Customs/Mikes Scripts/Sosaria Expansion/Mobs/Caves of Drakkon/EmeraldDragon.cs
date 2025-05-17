using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("an emerald dragon corpse")]
    public class EmeraldDragon : BaseCreature
    {
        // Cooldown timers
        private DateTime m_NextVerdantRoar;
        private DateTime m_NextThornSpiral;
        private DateTime m_NextCelestialBloom;
        private DateTime m_NextNatureVortex;
        private DateTime m_NextEmeraldTempest;

        // Unique emerald hue
        private const int UniqueHue = 1153;

        [Constructable]
        public EmeraldDragon()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.3, 0.5)
        {
            Name = "an emerald dragon";
            Body = Utility.RandomList(12, 59);
            BaseSoundID = 362;
            Hue = UniqueHue;

            // Stats
            SetStr(1300, 1500);
            SetDex(150, 200);
            SetInt(600, 800);

            SetHits(1800, 2200);
            SetDamage(30, 40);

            // Resistances
            SetResistance(ResistanceType.Physical, 70, 85);
            SetResistance(ResistanceType.Fire, 60, 80);
            SetResistance(ResistanceType.Cold, 70, 85);
            SetResistance(ResistanceType.Poison, 90, 100);
            SetResistance(ResistanceType.Energy, 80, 90);

            // Skills
            SetSkill(SkillName.EvalInt, 120.0, 140.0);
            SetSkill(SkillName.Magery, 120.0, 140.0);
            SetSkill(SkillName.Meditation, 80.0, 110.0);
            SetSkill(SkillName.MagicResist, 130.0, 160.0);
            SetSkill(SkillName.Tactics, 120.0, 140.0);
            SetSkill(SkillName.Wrestling, 120.0, 140.0);

            Fame = 35000;
            Karma = -35000;
            VirtualArmor = 80;

            // Initialize cooldowns
            m_NextVerdantRoar     = DateTime.UtcNow;
            m_NextThornSpiral     = DateTime.UtcNow;
            m_NextCelestialBloom  = DateTime.UtcNow;
            m_NextNatureVortex    = DateTime.UtcNow;
            m_NextEmeraldTempest  = DateTime.UtcNow;
        }

        public EmeraldDragon(Serial serial)
            : base(serial)
        {
        }

        // Basic overrides
        public override bool ReacquireOnMovement => true;
        public override bool AutoDispel         => true;
        public override bool CanFly             => true;

        public override HideType HideType => HideType.Barbed;
        public override int Hides          => 60;
        public override int Meat           => 25;
        public override int Scales         => 20;
        public override ScaleType ScaleType => (ScaleType)Utility.Random(4);

        public override Poison PoisonImmune => Poison.Lethal;
        public override Poison HitPoison    => Poison.Deadly;

        public override int TreasureMapLevel => 6;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 4);
            AddLoot(LootPack.Gems, 10);

            if (Utility.RandomDouble() < 0.02) // 2% for a unique plate
                ; // PackItem(new EmeraldDragonScalePlate());

            if (Utility.RandomDouble() < 0.05) // 5% high‑level map
                PackItem(new TreasureMap(6, Map));
        }

        // Sounds
        public override int GetIdleSound()  => 362;
        public override int GetAngerSound() => 362;
        public override int GetHurtSound()  => 362;
        public override int GetDeathSound() => 362;

        // --- Special Abilities ---

        // 1) Verdant Roar: circular poison/energy shockwave + quicksand tiles
        public void VerdantRoar()
        {
            var map = Map;
            if (map == null) return;

            Effects.PlaySound(Location, map, 0x5C3);
            Effects.SendLocationParticles(EffectItem.Create(Location, map, EffectItem.DefaultDuration), 0x373A, 20, 10, UniqueHue, 0, 2035, 0);

            int radius = 6, damage = 35;
            for (int x = -radius; x <= radius; x++)
            for (int y = -radius; y <= radius; y++)
            {
                var p = new Point3D(X + x, Y + y, Z);
                if (Utility.InRange(Location, p, radius))
                {
                    Effects.SendLocationEffect(p, map, 0x3740, 10, UniqueHue, 0);

                    foreach (Mobile m in map.GetMobilesInRange(p, 0))
                    {
                        if (m != this && (m is PlayerMobile || (m is BaseCreature bc && bc.Team != Team)))
                        {
                            DoHarmful(m);
                            AOS.Damage(m, this, damage, 0, 0, 50, 0, 50); // Poison/Energy split
                        }
                    }

                    if (Utility.RandomBool())
                    {
                        // spawn a quicksand trap
                        var tile = new QuicksandTile();
                        tile.MoveToWorld(p, map);
                    }
                }
            }

            m_NextVerdantRoar = DateTime.UtcNow + TimeSpan.FromSeconds(12);
        }

        // 2) Thorn Spiral: diamond‑shaped spike burst + vortex tiles
        public void ThornSpiral()
        {
            var map = Map;
            if (map == null) return;

            Effects.PlaySound(Location, map, 0x5C4);
            int radius = 7, damage = 30;

            for (int x = -radius; x <= radius; x++)
            for (int y = -radius; y <= radius; y++)
            {
                if (Math.Abs(x) + Math.Abs(y) <= radius)
                {
                    var p = new Point3D(X + x, Y + y, Z);
                    Effects.SendLocationParticles(EffectItem.Create(p, map, EffectItem.DefaultDuration), 0x37C0, 5, 10, UniqueHue, 0, 2040, 0);

                    foreach (Mobile m in map.GetMobilesInRange(p, 0))
                    {
                        if (m != this && (m is PlayerMobile || (m is BaseCreature bc && bc.Team != Team)))
                        {
                            DoHarmful(m);
                            AOS.Damage(m, this, damage, 100, 0, 0, 0, 0);
                            m.Stam -= 20; // slow effect
                        }
                    }

                    if (Utility.RandomDouble() < 0.3)
                    {
                        var tile = new VortexTile();
                        tile.MoveToWorld(p, map);
                    }
                }
            }

            m_NextThornSpiral = DateTime.UtcNow + TimeSpan.FromSeconds(15);
        }

        // 3) Celestial Bloom: forward cone of life‑draining petals + healing tiles
        public void CelestialBloom()
        {
            var map = Map;
			if (map == null || !(Combatant is Mobile)) return;
			Mobile target = (Mobile)Combatant;

            Effects.PlaySound(Location, map, 0x20A);
            var dir = GetDirectionTo(target);
			int dx = 0, dy = 0;
			Movement.Movement.Offset(dir, ref dx, ref dy);


            int range = 8, width = 4, damage = 25;
            for (int i = 1; i <= range; i++)
            {
                int span = (int)(i * (width / (double)range));
                for (int j = -span; j <= span; j++)
                {
                    int tx = X + i * dx, ty = Y + i * dy;
                    if (dx == 0) tx += j;
                    else if (dy == 0) ty += j;
                    else { tx += j; ty += j * (dx * dy > 0 ? -1 : 1); }

                    var p = new Point3D(tx, ty, Z);
                    Effects.SendLocationParticles(EffectItem.Create(p, map, EffectItem.DefaultDuration), 0x3745, 8, 8, UniqueHue, 0, 2045, 0);

                    foreach (Mobile m in map.GetMobilesInRange(p, 0))
                    {
                        if (m != this && (m is PlayerMobile || (m is BaseCreature bc && bc.Team != Team)))
                        {
                            DoHarmful(m);
                            AOS.Damage(m, this, damage, 0, 100, 0, 0, 0);
                        }
                    }

                    if (Utility.RandomDouble() < 0.4)
                    {
                        var heal = new HealingPulseTile();
                        heal.MoveToWorld(p, map);
                    }
                }
            }

            m_NextCelestialBloom = DateTime.UtcNow + TimeSpan.FromSeconds(20);
        }

        // 4) Nature’s Vortex: ring‑shaped poison cloud + trap webs
        public void NatureVortex()
        {
            var map = Map;
            if (map == null) return;

            Effects.PlaySound(Location, map, 0x208);
            int inner = 3, outer = 6, damage = 20;

            for (int x = -outer; x <= outer; x++)
            for (int y = -outer; y <= outer; y++)
            {
                int dist = Math.Max(Math.Abs(x), Math.Abs(y));
                if (dist >= inner && dist <= outer)
                {
                    var p = new Point3D(X + x, Y + y, Z);
                    Effects.SendLocationParticles(EffectItem.Create(p, map, EffectItem.DefaultDuration), 0x36B0, 5, 5, UniqueHue, 0, 2050, 0);

                    foreach (Mobile m in map.GetMobilesInRange(p, 0))
                    {
                        if (m != this && (m is PlayerMobile || (m is BaseCreature bc && bc.Team != Team)))
                        {
                            DoHarmful(m);
                            AOS.Damage(m, this, damage, 0, 0, 100, 0, 0);
                            m.ApplyPoison(this, Poison.Deadly);
                        }
                    }

                    if (Utility.RandomBool())
                    {
                        var trap = new TrapWeb();
                        trap.MoveToWorld(p, map);
                    }
                }
            }

            m_NextNatureVortex = DateTime.UtcNow + TimeSpan.FromSeconds(18);
        }

        // 5) Emerald Tempest: line‑based energy storm + lightning tiles
        public void EmeraldTempest()
        {
            var map = Map;
			if (map == null || !(Combatant is Mobile)) return;
			Mobile target = (Mobile)Combatant;


            Effects.PlaySound(Location, map, 0x5C5);
            var dir = GetDirectionTo(target);
			int dx = 0, dy = 0;
			Movement.Movement.Offset(dir, ref dx, ref dy);


            int length = 12, damage = 45;
            for (int i = 1; i <= length; i++)
            {
                var p = new Point3D(X + i*dx, Y + i*dy, Z);
                Effects.SendLocationParticles(EffectItem.Create(p, map, EffectItem.DefaultDuration), 0x3818, 6, 6, UniqueHue, 0, 2060, 0);

                foreach (Mobile m in map.GetMobilesInRange(p, 0))
                {
                    if (m != this && (m is PlayerMobile || (m is BaseCreature bc && bc.Team != Team)))
                    {
                        DoHarmful(m);
                        AOS.Damage(m, this, damage, 0, 0, 0, 0, 100);
                    }
                }

                // randomly spawn a lightning storm or thunderstorm
                if (Utility.RandomBool())
                {
                    var tile = new LightningStormTile();
                    tile.MoveToWorld(p, map);
                }
                else
                {
                    var tile2 = new ThunderstormTile2();
                    tile2.MoveToWorld(p, map);
                }
            }

            m_NextEmeraldTempest = DateTime.UtcNow + TimeSpan.FromSeconds(10);
        }

        // AI: choose abilities based on range & cooldown
        public override void OnThink()
        {
            base.OnThink();

			if (!(Combatant is Mobile)) return;
			Mobile target = (Mobile)Combatant;

			if (Deleted || !Alive)
				return;


            var now = DateTime.UtcNow;
            int dist = (int)GetDistanceToSqrt(target);

            if (now >= m_NextVerdantRoar     && Utility.InRange(Location, target.Location, 6))   VerdantRoar();
            else if (now >= m_NextThornSpiral && Utility.InRange(Location, target.Location, 12))  ThornSpiral();
            else if (now >= m_NextEmeraldTempest && Utility.InRange(Location, target.Location, 15)) EmeraldTempest();
            else if (now >= m_NextCelestialBloom && Utility.InRange(Location, target.Location, 8))  CelestialBloom();
            else if (now >= m_NextNatureVortex)                                                 NatureVortex();
        }

        // Death: massive poison cloud + scattered venom tiles
        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            if (Map == null) return;

            Effects.PlaySound(Location, Map, 0x214);
            Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration), 0x36E4, 30, 20, UniqueHue, 0, 2070, 0);

            int pieces = 25, radius = 8;
            for (int i = 0; i < pieces; i++)
            {
                var p = GetRandomLocation(Location, radius, Map);
                if (p != Point3D.Zero)
                {
                    var tile = new PoisonTile();
                    tile.MoveToWorld(p, Map);
                }
            }
        }

        private Point3D GetRandomLocation(Point3D center, int radius, Map map)
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

        // Serialization
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
