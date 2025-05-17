using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("an inferno ant corpse")]
    public class InfernoAnt : BaseCreature
    {
        // Cooldown timers
        private DateTime m_NextMagmaSpray;
        private DateTime m_NextEruptionCircle;
        private DateTime m_NextScorchLine;
        private DateTime m_NextAshenSwarm;
        private DateTime m_NextCarapaceAura;

        // Unique fiery hue
        private const int UniqueHue = 2118;

        [Constructable]
        public InfernoAnt()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "an Inferno Ant";
            Body = 738;
            Hue  = UniqueHue;

            // Stats
            SetStr(450, 520);
            SetDex(160, 220);
            SetInt(100, 150);

            SetHits(650, 750);
            SetDamage(25, 35);

            // Resistances
            SetResistance(ResistanceType.Physical, 55, 65);
            SetResistance(ResistanceType.Fire, 100, 100);
            SetResistance(ResistanceType.Cold, 45, 55);
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 40, 50);

            // Skills
            SetSkill(SkillName.Anatomy,        30.0, 50.0);
            SetSkill(SkillName.MagicResist,    80.0,100.0);
            SetSkill(SkillName.Tactics,        80.0,100.0);
            SetSkill(SkillName.Wrestling,      80.0,100.0);

            Fame = 15000;
            Karma = -15000;

            VirtualArmor = 75;

            // Start all abilities off cooldown
            var now = DateTime.UtcNow;
            m_NextMagmaSpray    = now;
            m_NextEruptionCircle= now;
            m_NextScorchLine    = now;
            m_NextAshenSwarm    = now;
            m_NextCarapaceAura  = now;
        }

        public InfernoAnt(Serial serial)
            : base(serial)
        {
        }

        // Loot & corpse
        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich, 2);
            AddLoot(LootPack.Gems, Utility.RandomMinMax(3, 6));

            if (Utility.RandomDouble() < 0.02) // 2% for a rare chitin
                PackItem(new PeltknitBreeches());

            if (Utility.RandomDouble() < 0.005) // 0.5% for a special amulet
                PackItem(new FireAntAmulet());
        }

        public override int TreasureMapLevel => 4;
        public override bool ReacquireOnMovement => true;


        // Use same sounds as base FireAnt
        public override int GetIdleSound()  => 846;
        public override int GetAngerSound() => 849;
        public override int GetHurtSound()  => 852;
        public override int GetDeathSound() => 850;

        // --- Special Abilities ---

        // 1) Magma Spray: wide cone of flames + hot lava tiles
        public void MagmaSprayCone()
        {
            if (!(Combatant is Mobile target)) return;
            if (DateTime.UtcNow < m_NextMagmaSpray) return;
            if (!this.InRange(target, 8)) return;

            var map = Map;
            if (map == null) return;

            // Visual & sound
            Effects.PlaySound(Location, map, 0x227);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, map, TimeSpan.FromSeconds(1.5)),
                0x3709, 20, 30, UniqueHue, 0, 5052, 0);

            // Compute cone
            Direction d = GetDirectionTo(target);
            for (int i = 1; i <= 8; i++)
            {
                int dx = 0, dy = 0;
                Movement.Movement.Offset(d, ref dx, ref dy);
                int width = (int)(i * 0.5);

                for (int w = -width; w <= width; w++)
                {
                    int x = X + i * dx + ((dy == 0) ? w : 0);
                    int y = Y + i * dy + ((dx == 0) ? w : 0);
                    var p = new Point3D(x, y, Z);
                    if (!map.CanFit(p, 16, false, false))
                        p = new Point3D(x, y, map.GetAverageZ(x, y));

                    Effects.SendLocationParticles(
                        EffectItem.Create(p, map, TimeSpan.FromSeconds(1)),
                        0x36B0, 10, 10, UniqueHue, 0, 2023, 0);

                    // Damage & spawn lava
                    foreach (var m in map.GetMobilesInRange(p, 0))
                    {
                        if (m == this) continue;
                        if (m is PlayerMobile || (m is BaseCreature bc && bc.Team != Team))
                        {
                            DoHarmful(m);
                            AOS.Damage(m, this, 20, 0, 0, 100, 0, 0);
                        }
                    }

                    if (Utility.RandomDouble() < 0.4)
                    {
                        var tile = new HotLavaTile();
                        tile.MoveToWorld(p, map);
                    }
                }
            }

            m_NextMagmaSpray = DateTime.UtcNow + TimeSpan.FromSeconds(12);
        }

        // 2) Eruption Circle: radial burst of molten earth
        public void EruptionCircle()
        {
            if (!(Combatant is Mobile)) return;
            if (DateTime.UtcNow < m_NextEruptionCircle) return;

            var map = Map;
            if (map == null) return;

            Effects.PlaySound(Location, map, 0x2F3);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, map, TimeSpan.FromSeconds(1)),
                0x3709, 20, 40, UniqueHue, 0, 5029, 0);

            for (int dx = -5; dx <= 5; dx++)
            for (int dy = -5; dy <= 5; dy++)
            {
                var p = new Point3D(X + dx, Y + dy, Z);
                if (!map.CanFit(p, 16, false, false) || !Utility.InRange(Location, p, 5))
                {
                    p = new Point3D(p.X, p.Y, map.GetAverageZ(p.X, p.Y));
                    if (!map.CanFit(p, 16, false, false) || !Utility.InRange(Location, p, 5))
                        continue;
                }

                Effects.SendLocationEffect(p, map, 0x3709, 16, UniqueHue, 0);
                foreach (var m in map.GetMobilesInRange(p, 0))
                {
                    if (m == this) continue;
                    if (m is PlayerMobile || (m is BaseCreature bc && bc.Team != Team))
                    {
                        DoHarmful(m);
                        AOS.Damage(m, this, 30, 100, 0, 0, 0, 0);
                    }
                }

                if (Utility.RandomDouble() < 0.2)
                {
                    var quake = new EarthquakeTile();
                    quake.MoveToWorld(p, map);
                }
            }

            m_NextEruptionCircle = DateTime.UtcNow + TimeSpan.FromSeconds(18);
        }

        // 3) Scorch Line: a narrow line of superheated chitin shards
        public void ScorchLineAttack()
        {
            if (!(Combatant is Mobile target)) return;
            if (DateTime.UtcNow < m_NextScorchLine) return;
            if (!Utility.InRange(Location, target.Location, 12)) return;

            var map = Map;
            if (map == null) return;
            Direction d = GetDirectionTo(target);
            Effects.PlaySound(Location, map, 0x65A);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, map, TimeSpan.FromSeconds(1)),
                0x3728, 10, 10, UniqueHue, 0, 2023, 0);

            int dx = 0, dy = 0;
            Movement.Movement.Offset(d, ref dx, ref dy);
            for (int i = 1; i <= 12; i++)
            {
                var p = new Point3D(X + i * dx, Y + i * dy, Z);
                if (!map.CanFit(p, 16, false, false))
                    p = new Point3D(p.X, p.Y, map.GetAverageZ(p.X, p.Y));

                Effects.SendLocationEffect(p, map, 0x3818, 16, UniqueHue, 0);

                foreach (var m in map.GetMobilesInRange(p, 0))
                {
                    if (m == this) continue;
                    if (m is PlayerMobile || (m is BaseCreature bc && bc.Team != Team))
                    {
                        DoHarmful(m);
                        AOS.Damage(m, this, 35, 0, 0, 0, 0, 100);
                    }
                }
            }

            m_NextScorchLine = DateTime.UtcNow + TimeSpan.FromSeconds(10);
        }

        // 4) Ashen Swarm: launches embers all around the target
        public void AshenSwarm()
        {
            if (!(Combatant is Mobile target)) return;
            if (DateTime.UtcNow < m_NextAshenSwarm) return;
            if (!Utility.InRange(Location, target.Location, 10)) return;

            var map = Map;
            if (map == null) return;
            Effects.PlaySound(Location, map, 0x208);
            for (int i = 0; i < 15; i++)
            {
                int rx = Utility.RandomMinMax(-4, 4);
                int ry = Utility.RandomMinMax(-4, 4);
                var p = new Point3D(target.X + rx, target.Y + ry, target.Z);
                if (!map.CanFit(p, 16, false, false))
                    p = new Point3D(p.X, p.Y, map.GetAverageZ(p.X, p.Y));

                Effects.SendLocationParticles(
                    EffectItem.Create(p, map, TimeSpan.FromSeconds(0.8)),
                    0x3709, 8, 12, UniqueHue, 0, 5052, 0);

                foreach (var m in map.GetMobilesInRange(p, 0))
                {
                    if (m == this) continue;
                    if (m is PlayerMobile || (m is BaseCreature bc && bc.Team != Team))
                    {
                        DoHarmful(m);
                        AOS.Damage(m, this, 15, 0, 0, 100, 0, 0);
                    }
                }

                if (Utility.RandomDouble() < 0.3)
                {
                    var tile = new FlamestrikeHazardTile();
                    tile.MoveToWorld(p, map);
                }
            }

            m_NextAshenSwarm = DateTime.UtcNow + TimeSpan.FromSeconds(14);
        }

        // 5) Carapace Aura: periodic ring of burning heat around itself
        public void CarapaceAura()
        {
            if (DateTime.UtcNow < m_NextCarapaceAura) return;

            var map = Map;
            if (map == null) return;

            Effects.PlaySound(Location, map, 0x208);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, map, TimeSpan.FromSeconds(1)),
                0x37CB, 1, 12, UniqueHue, 0, 9909, 0);

            foreach (var m in map.GetMobilesInRange(Location, 3))
            {
                if (m == this) continue;
                if (m is PlayerMobile || (m is BaseCreature bc && bc.Team != Team))
                {
                    DoHarmful(m);
                    AOS.Damage(m, this, 10, 0, 0, 0, 0, 100);
                }
            }

            m_NextCarapaceAura = DateTime.UtcNow + TimeSpan.FromSeconds(8);
        }

        // AI Logic
        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                MagmaSprayCone();
                EruptionCircle();
                ScorchLineAttack();
                AshenSwarm();
                CarapaceAura();
            }
        }

        // Volcanic death explosion
        public override void OnDeath(Container c)
        {
            var map = Map;
            if (map != null)
            {
                Effects.PlaySound(Location, map, 0x218);
                Effects.SendLocationParticles(
                    EffectItem.Create(Location, map, TimeSpan.FromSeconds(1.5)),
                    0x3709, 20, 60, UniqueHue, 0, 5052, 0);

                for (int i = 0; i < 25; i++)
                {
                    var p = GetRandomValidLocation(Location, 6, map);
                    if (p == Point3D.Zero) continue;

                    var tile = Utility.RandomBool()
                        ? (IPoint3D)new HotLavaTile()
                        : new FlamestrikeHazardTile();
                    ((Item)tile).MoveToWorld(p, map);

                    Effects.SendLocationParticles(
                        EffectItem.Create(p, map, TimeSpan.FromSeconds(1)),
                        0x3709, 10, 20, UniqueHue, 0, 5016, 0);
                }
            }

            base.OnDeath(c);
        }

        // Helper to find valid spawn point
        private Point3D GetRandomValidLocation(Point3D center, int radius, Map map)
        {
            for (int i = 0; i < 12; i++)
            {
                int dx = Utility.RandomMinMax(-radius, radius);
                int dy = Utility.RandomMinMax(-radius, radius);
                var p = new Point3D(center.X + dx, center.Y + dy, center.Z);
                if (map.CanFit(p, 16, false, false) && Utility.InRange(center, p, radius))
                    return p;

                p = new Point3D(p.X, p.Y, map.GetAverageZ(p.X, p.Y));
                if (map.CanFit(p, 16, false, false) && Utility.InRange(center, p, radius))
                    return p;
            }
            return Point3D.Zero;
        }

        // Serialization
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt();
        }
    }
}
