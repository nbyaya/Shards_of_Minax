using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;       // For SpellHelper, if you add spell‐based effects
using Server.Movement;     // For Movement.Movement.Offset

namespace Server.Mobiles
{
    [CorpseName("a drakons tiger corpse")]
    public class DrakonsTiger : BaseCreature
    {
        // Cooldown timers for special abilities
        private DateTime m_NextRoaringMaul;
        private DateTime m_NextFrostArc;
        private DateTime m_NextShadowPounce;
        private DateTime m_NextSpiralFlame;

        // Unique hue (burnt orange stripes)
        private const int UniqueHue = 1158;

        [Constructable]
        public DrakonsTiger()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Drakons Tiger";
            Body = 0x588;             // Same body as saber‐toothed tiger
            Hue = UniqueHue;

            // Significantly enhanced stats
            SetStr(800, 900);
            SetDex(400, 450);
            SetInt(600, 700);

            SetHits(1000, 1100);
            SetDamage(30, 40);

            // Resistances
            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire,     70, 80);
            SetResistance(ResistanceType.Cold,     50, 60);
            SetResistance(ResistanceType.Poison,   60, 70);
            SetResistance(ResistanceType.Energy,   50, 60);

            // Skills
            SetSkill(SkillName.Parry,        110.0, 120.0);
            SetSkill(SkillName.Tactics,      120.0, 130.0);
            SetSkill(SkillName.Wrestling,    120.0, 130.0);
            SetSkill(SkillName.MagicResist,  140.0, 150.0);
            SetSkill(SkillName.DetectHidden,  95.0);
            SetSkill(SkillName.Focus,        120.0, 130.0);

            Fame = 20000;
            Karma = -20000;
            VirtualArmor = 60;
            Tamable = false;

            // Initialize cooldowns
            m_NextRoaringMaul   = DateTime.UtcNow;
            m_NextFrostArc      = DateTime.UtcNow;
            m_NextShadowPounce  = DateTime.UtcNow;
            m_NextSpiralFlame   = DateTime.UtcNow;
        }

        public DrakonsTiger(Serial serial)
            : base(serial)
        {
        }

        public override HideType HideType => HideType.Barbed;
        public override int Hides => 20;
        public override int Meat => 15;
        public override FoodType FavoriteFood => FoodType.Meat;
        public override int TreasureMapLevel => 5;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich, 2);
            AddLoot(LootPack.Gems, 5);

            // 2% chance for a special fang drop (implement your item)
            if (Utility.RandomDouble() < 0.02)
            {
                // PackItem(new DrakonsTigerFang());
            }

            // 5% chance for a level‑5 treasure map
            if (Utility.RandomDouble() < 0.05)
            {
                PackItem(new TreasureMap(5, Map));
            }
        }

        public override int GetIdleSound()  => 0x673;
        public override int GetAngerSound() => 0x670;
        public override int GetHurtSound()  => 0x672;
        public override int GetDeathSound() => 0x671;

        // --- Special Abilities ---

        // 1) Roaring Maul: Physical cone attack in front of the tiger
        public void RoaringMaulAttack()
        {
            if (!(Combatant is Mobile target)) return;
            var map = Map;
            if (map == null) return;

            // Play roar & ground-shock effect
            Effects.PlaySound(Location, map, 0x354);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, map, EffectItem.DefaultDuration),
                0x3709, 6, 30, UniqueHue, 0, 9910, 0
            );

            // Build a cone (range 4, width 2)
            var cone = new List<Point3D>();
            Direction d = GetDirectionTo(target);
            int dx = 0, dy = 0;
            Movement.Movement.Offset(d, ref dx, ref dy);

            for (int i = 1; i <= 4; i++)
            {
                for (int j = -2; j <= 2; j++)
                {
                    int x = X + dx * i - dy * j;
                    int y = Y + dy * i + dx * j;
                    var p = new Point3D(x, y, Z);
                    if (!map.CanFit(p, 16, false, false))
                    {
                        int z2 = map.GetAverageZ(x, y);
                        p = new Point3D(x, y, z2);
                        if (!map.CanFit(p, 16, false, false))
                            continue;
                    }
                    cone.Add(p);
                }
            }

            // Apply particle & damage to each tile in the cone
            foreach (var p in cone)
            {
                Effects.SendLocationParticles(
                    EffectItem.Create(p, map, EffectItem.DefaultDuration),
                    0x36BD, 1, 9, UniqueHue, 0, 2032, 0
                );

                foreach (Mobile m in map.GetMobilesInRange(p, 0))
                {
                    if (m != this && (m is PlayerMobile || (m is BaseCreature bc && bc.Team != Team)))
                    {
                        DoHarmful(m);
                        AOS.Damage(m, this, 40, 100, 0, 0, 0, 0); // 100% physical
                    }
                }
            }

            m_NextRoaringMaul = DateTime.UtcNow + TimeSpan.FromSeconds(12);
        }

        // 2) Frost Arc: A wide cold wave around the tiger
        public void FrostArcAttack()
        {
            if (!(Combatant is Mobile)) return;
            var map = Map;
            if (map == null) return;

            // Play frost sound & center burst
            Effects.PlaySound(Location, map, 0x64A);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, map, EffectItem.DefaultDuration),
                0x37AE, 10, 20, UniqueHue, 0, 2031, 0
            );

            int radius = 6;
            var area = new List<Point3D>();
            for (int dx = -radius; dx <= radius; dx++)
            {
                for (int dy = -radius; dy <= radius; dy++)
                {
                    var p = new Point3D(X + dx, Y + dy, Z);
                    if (Utility.InRange(Location, p, radius))
                    {
                        if (!map.CanFit(p, 16, false, false))
                        {
                            int z2 = map.GetAverageZ(p.X, p.Y);
                            p = new Point3D(p.X, p.Y, z2);
                            if (!map.CanFit(p, 16, false, false))
                                continue;
                        }
                        area.Add(p);
                    }
                }
            }

            foreach (var p in area)
            {
                Effects.SendLocationParticles(
                    EffectItem.Create(p, map, EffectItem.DefaultDuration),
                    0x37AE, 1, 9, UniqueHue, 0, 2031, 0
                );
                foreach (Mobile m in map.GetMobilesInRange(p, 0))
                {
                    if (m != this && (m is PlayerMobile || (m is BaseCreature bc && bc.Team != Team)))
                    {
                        DoHarmful(m);
                        AOS.Damage(m, this, 30, 0, 0, 100, 0, 0); // 100% cold
                        m.SendMessage("You are chilled by the frost arc!");
                        m.Freeze(TimeSpan.FromSeconds(2));
                    }
                }
            }

            m_NextFrostArc = DateTime.UtcNow + TimeSpan.FromSeconds(20);
        }

        // 3) Shadow Pounce: Teleport behind the target then radial slam
        public void ShadowPounceAttack()
        {
            if (!(Combatant is Mobile target)) return;
            var map = Map;
            if (map == null) return;

            // Calculate a point two tiles behind the target
            Direction d = GetDirectionTo(target);
            int dx = 0, dy = 0;
            Movement.Movement.Offset(d, ref dx, ref dy);
            int destX = target.X - dx * 2, destY = target.Y - dy * 2;
            int destZ = map.GetAverageZ(destX, destY);
            var dest = new Point3D(destX, destY, destZ);

            // Departure effect
            Effects.SendLocationParticles(
                EffectItem.Create(Location, map, EffectItem.DefaultDuration),
                0x3779, 1, 29, UniqueHue, 0, 5020, 0
            );
            Effects.PlaySound(Location, map, 0x212);

            // Teleport
            Location = dest;
            ProcessDelta();

            // Arrival effect
            Effects.SendLocationParticles(
                EffectItem.Create(dest, map, EffectItem.DefaultDuration),
                0x3779, 1, 29, UniqueHue, 0, 5021, 0
            );

            // Radial slam (radius 3)
            int radius = 3;
            for (int dx2 = -radius; dx2 <= radius; dx2++)
            {
                for (int dy2 = -radius; dy2 <= radius; dy2++)
                {
                    var p = new Point3D(dest.X + dx2, dest.Y + dy2, dest.Z);
                    if (Utility.InRange(dest, p, radius) && map.CanFit(p, 16, false, false))
                    {
                        Effects.SendLocationParticles(
                            EffectItem.Create(p, map, EffectItem.DefaultDuration),
                            0x36BD, 1, 9, UniqueHue, 0, 2025, 0
                        );
                        foreach (Mobile m in map.GetMobilesInRange(p, 0))
                        {
                            if (m != this && (m is PlayerMobile || (m is BaseCreature bc && bc.Team != Team)))
                            {
                                DoHarmful(m);
                                AOS.Damage(m, this, 50, 0, 100, 0, 0, 0); // 100% cold slam
                                m.SendMessage("A shadowy force pummels you!");
                            }
                        }
                    }
                }
            }

            m_NextShadowPounce = DateTime.UtcNow + TimeSpan.FromSeconds(25);
        }

        // 4) Spiral Flame: Whirling flame pattern around self
        public void SpiralFlameAttack()
        {
            var map = Map;
            if (map == null) return;

            Effects.PlaySound(Location, map, 0x208);
            int points = 8;
            double full = Math.PI * 2;

            for (int i = 0; i < points; i++)
            {
                double angle = full * i / points;
                int r = 1 + (i / 2);
                int x = X + (int)(Math.Cos(angle) * r);
                int y = Y + (int)(Math.Sin(angle) * r);
                var p = new Point3D(x, y, Z);

                if (!map.CanFit(p, 16, false, false))
                {
                    int z2 = map.GetAverageZ(x, y);
                    p = new Point3D(x, y, z2);
                    if (!map.CanFit(p, 16, false, false))
                        continue;
                }

                Effects.SendLocationParticles(
                    EffectItem.Create(p, map, EffectItem.DefaultDuration),
                    0x3709, 1, 9, UniqueHue, 0, 5005, 0
                );
                foreach (Mobile m in map.GetMobilesInRange(p, 0))
                {
                    if (m != this && (m is PlayerMobile || (m is BaseCreature bc && bc.Team != Team)))
                    {
                        DoHarmful(m);
                        AOS.Damage(m, this, 25, 0, 0, 0, 100, 0); // 100% energy
                    }
                }
            }

            m_NextSpiralFlame = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        // --- AI Logic ---
        public override void OnThink()
        {
            base.OnThink();

            if (Combatant is Mobile target)
            {
                if (DateTime.UtcNow >= m_NextRoaringMaul && InRange(target, 4))
                {
                    RoaringMaulAttack();
                }
                else if (DateTime.UtcNow >= m_NextFrostArc && Utility.InRange(Location, target.Location, 8))
                {
                    FrostArcAttack();
                }
                else if (DateTime.UtcNow >= m_NextShadowPounce && InRange(target, 10))
                {
                    ShadowPounceAttack();
                }
                else if (DateTime.UtcNow >= m_NextSpiralFlame)
                {
                    SpiralFlameAttack();
                }
            }
        }

        // --- OnDeath: Scatter hazardous tiles ---
        public override void OnDeath(Container c)
        {
            var map = Map;
            if (map != null)
            {
                int radius = 5;
                for (int i = 0; i < 15; i++)
                {
                    var p = GetRandomValidLocation(Location, radius, map);
                    if (p == Point3D.Zero) continue;

                    // Randomly drop landmine or poison tile
                    if (Utility.RandomBool())
                    {
                        new LandmineTile().MoveToWorld(p, map);
                    }
                    else
                    {
                        new PoisonTile().MoveToWorld(p, map);
                    }

                    Effects.SendLocationParticles(
                        EffectItem.Create(p, map, EffectItem.DefaultDuration),
                        0x3709, 10, 20, UniqueHue, 0, 5016, 0
                    );
                }
            }

            base.OnDeath(c);
        }

        // Helper to find a random valid tile within radius
        private Point3D GetRandomValidLocation(Point3D center, int radius, Map map)
        {
            for (int i = 0; i < 10; i++)
            {
                int x = center.X + Utility.RandomMinMax(-radius, radius);
                int y = center.Y + Utility.RandomMinMax(-radius, radius);
                int z = map.GetAverageZ(x, y);
                var p = new Point3D(x, y, z);

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
            int version = reader.ReadInt();
        }
    }
}
