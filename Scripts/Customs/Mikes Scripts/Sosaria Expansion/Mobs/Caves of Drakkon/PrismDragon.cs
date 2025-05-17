using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a prismatic dragon corpse")]
    public class PrismDragon : BaseCreature
    {
        // Cooldown timers for special abilities
        private DateTime m_NextPrismBeam;
        private DateTime m_NextChromaticBurst;
        private DateTime m_NextSpectrumVortex;
        private DateTime m_NextPolarizingField;
        private DateTime m_NextShatterStorm;

        // Unique prismatic hue
        private const int UniqueHue = 2119;

        [Constructable]
        public PrismDragon()
            : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name = "a prismatic dragon";
            Body = Utility.RandomList(12, 59);
            BaseSoundID = 362;
            Hue = UniqueHue;

            // Enhanced stats
            SetStr(1400, 1600);
            SetDex(150, 250);
            SetInt(1200, 1400);

            SetHits(1500, 2000);
            SetDamage(40, 55);

            // Mixed damage types
            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 10);
            SetDamageType(ResistanceType.Cold, 10);
            SetDamageType(ResistanceType.Poison, 10);
            SetDamageType(ResistanceType.Energy, 20);

            // Strong resistances
            SetResistance(ResistanceType.Physical, 80, 90);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 80, 90);
            SetResistance(ResistanceType.Poison, 70, 80);
            SetResistance(ResistanceType.Energy, 80, 90);

            // High skills
            SetSkill(SkillName.EvalInt,    120.0, 150.0);
            SetSkill(SkillName.Magery,     120.0, 150.0);
            SetSkill(SkillName.Meditation, 100.0, 120.0);
            SetSkill(SkillName.MagicResist,140.0, 180.0);
            SetSkill(SkillName.Tactics,    120.0, 140.0);
            SetSkill(SkillName.Wrestling,  120.0, 140.0);

            Fame = 35000;
            Karma = -35000;
            VirtualArmor = 90;

            // Initialize cooldowns
            m_NextPrismBeam        = DateTime.UtcNow;
            m_NextChromaticBurst   = DateTime.UtcNow;
            m_NextSpectrumVortex   = DateTime.UtcNow;
            m_NextPolarizingField  = DateTime.UtcNow;
            m_NextShatterStorm     = DateTime.UtcNow;
        }

        public PrismDragon(Serial serial)
            : base(serial)
        {
        }

        // Properties
        public override bool ReacquireOnMovement => true;
        public override bool AutoDispel          => true;
        public override Poison PoisonImmune      => Poison.Lethal;
        public override Poison HitPoison         => Poison.Deadly;
        public override int Hides                => 60;
        public override int Scales               => 20;
        public override ScaleType ScaleType      => (ScaleType)Utility.Random(4);
        public override int Meat                 => 30;
        public override int TreasureMapLevel     => 7;
        public override bool CanFly              => true;

        // Loot
        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 4);
            AddLoot(LootPack.Gems, 10);

            if (Utility.RandomDouble() < 0.02) // 2% for a unique Prism Shard
                PackItem(new DrakkonsFlamebinders());   // Assume PrismShard exists
            if (Utility.RandomDouble() < 0.05)
                PackItem(new TreasureMap(7, Map));
        }

        // Sounds
        public override int GetIdleSound() { return 0x2D3; }
        public override int GetHurtSound() { return 0x2D1; }
        public override int GetAngerSound(){ return 0x2D2; }
        public override int GetDeathSound(){ return 0x2D0; }

        // --- Special Abilities ---

        // 1. Prism Beam: straight line energy beam
        public void PrismBeamAttack()
        {
            if (!(Combatant is Mobile target)) return;

            Map map = Map;
            if (map == null || !InRange(target, 18)) return;

            Direction d = GetDirectionTo(target);
			int dx = 0, dy = 0;
			Movement.Movement.Offset(d, ref dx, ref dy);

            int length = 18, damage = 60;

            // Beam start effect
            Effects.PlaySound(Location, map, 0x66C);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, map, EffectItem.DefaultDuration),
                0x373A, 12, 12, UniqueHue, 0, 2030, 0);

            for (int i = 1; i <= length; i++)
            {
                Point3D p = new Point3D(X + dx * i, Y + dy * i, Z);
                if (!map.CanFit(p, 16, false, false)) break;

                // Beam visuals
                Effects.SendLocationEffect(p, map, 0x3820, 16, UniqueHue, 0);

                // Damage
                foreach (Mobile m in map.GetMobilesInRange(p, 0))
                {
                    if (m != this && (m is PlayerMobile || (m is BaseCreature bc && bc.Team != Team)))
                    {
                        DoHarmful(m);
                        AOS.Damage(m, this, damage, 0, 0, 0, 0, 100); // Energy
                    }
                }
            }

            m_NextPrismBeam = DateTime.UtcNow.AddSeconds(12);
        }

        // 2. Chromatic Burst: radial colorful orbs + LightningStormTiles
        public void ChromaticBurstAttack()
        {
            if (!(Combatant is Mobile target)) return;

            Map map = Map;
            if (map == null || !InRange(target, 12)) return;

            int radius = 6, damage = 40;

            Effects.PlaySound(Location, map, 0x208);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, map, EffectItem.DefaultDuration),
                0x372A, 8, 12, UniqueHue, 0, 2025, 0);

            for (int x = -radius; x <= radius; x++)
            for (int y = -radius; y <= radius; y++)
            {
                Point3D p = new Point3D(X + x, Y + y, Z);
                if (!map.CanFit(p, 16, false, false) || !Utility.InRange(Location, p, radius))
                    continue;

                // Orb visuals
                Effects.SendLocationParticles(
                    EffectItem.Create(p, map, EffectItem.DefaultDuration),
                    0x36C6, 6, 6, UniqueHue, 0, 2026, 0);

                // Spawn lightning tile
                if (Utility.RandomDouble() < 0.3)
                {
                    var tile = new LightningStormTile();
                    tile.MoveToWorld(p, map);
                }

                // Damage
                foreach (Mobile m in map.GetMobilesInRange(p, 0))
                {
                    if (m != this && (m is PlayerMobile || (m is BaseCreature bc && bc.Team != Team)))
                    {
                        DoHarmful(m);
                        AOS.Damage(m, this, damage, 0, 0, 0, 0, 100);
                    }
                }
            }

            m_NextChromaticBurst = DateTime.UtcNow.AddSeconds(18);
        }

        // 3. Spectrum Vortex: swirling VortexTiles around self
        public void SpectrumVortexAttack()
        {
            if (!(Combatant is Mobile target)) return;

            Map map = Map;
            if (map == null || !InRange(target, 10)) return;

            int radius = 5, damage = 25;

            Effects.PlaySound(Location, map, 0x22F);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, map, EffectItem.DefaultDuration),
                0x372F, 10, 20, UniqueHue, 0, 2027, 0);

            for (int angle = 0; angle < 360; angle += 30)
            {
                double rad = angle * (Math.PI / 180);
                int dx = (int)(Math.Cos(rad) * radius);
                int dy = (int)(Math.Sin(rad) * radius);
                Point3D p = new Point3D(X + dx, Y + dy, Z);

                if (!map.CanFit(p, 16, false, false)) continue;

                Effects.SendLocationParticles(
                    EffectItem.Create(p, map, EffectItem.DefaultDuration),
                    0x37DD, 8, 8, UniqueHue, 0, 2028, 0);

                var vortex = new VortexTile();
                vortex.MoveToWorld(p, map);

                foreach (Mobile m in map.GetMobilesInRange(p, 0))
                {
                    if (m != this && (m is PlayerMobile || (m is BaseCreature bc && bc.Team != Team)))
                    {
                        DoHarmful(m);
                        AOS.Damage(m, this, damage, 0, 0, 0, 0, 100);
                    }
                }
            }

            m_NextSpectrumVortex = DateTime.UtcNow.AddSeconds(20);
        }

        // 4. Polarizing Field: spawns healing and poison tiles in a circle
        public void PolarizingFieldAbility()
        {
            Map map = Map;
            if (map == null) return;

            int radius = 4;

            Effects.PlaySound(Location, map, 0x229);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, map, EffectItem.DefaultDuration),
                0x3735, 6, 10, UniqueHue, 0, 2029, 0);

            for (int x = -radius; x <= radius; x++)
            for (int y = -radius; y <= radius; y++)
            {
                Point3D p = new Point3D(X + x, Y + y, Z);
                if (!map.CanFit(p, 16, false, false) || !Utility.InRange(Location, p, radius))
                    continue;

                Effects.SendLocationParticles(
                    EffectItem.Create(p, map, EffectItem.DefaultDuration),
                    0x36C0, 4, 8, UniqueHue, 0, 2031, 0);

                if (Utility.RandomDouble() < 0.4)
                {
                    if (Utility.RandomBool())
                        new HealingPulseTile().MoveToWorld(p, map);
                    else
                        new PoisonTile().MoveToWorld(p, map);
                }
            }

            m_NextPolarizingField = DateTime.UtcNow.AddSeconds(25);
        }

        // 5. Shatter Storm: sends IceShardTiles and LandmineTiles in a diamond
        public void ShatterStormAttack()
        {
            if (!(Combatant is Mobile target)) return;

            Map map = Map;
            if (map == null || !InRange(target, 12)) return;

            Effects.PlaySound(Location, map, 0x1FC);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, map, EffectItem.DefaultDuration),
                0x36B1, 8, 16, UniqueHue, 0, 2032, 0);

            int range = 6, damage = 35;
            for (int dx = -range; dx <= range; dx++)
            for (int dy = -range; dy <= range; dy++)
            {
                if (Math.Abs(dx) + Math.Abs(dy) > range) continue;

                Point3D p = new Point3D(X + dx, Y + dy, Z);
                if (!map.CanFit(p, 16, false, false)) continue;

                Effects.SendLocationParticles(
                    EffectItem.Create(p, map, EffectItem.DefaultDuration),
                    0x37A9, 6, 12, UniqueHue, 0, 2033, 0);

                if (Utility.RandomDouble() < 0.3)
                {
                    if (Utility.RandomBool())
                        new IceShardTile().MoveToWorld(p, map);
                    else
                        new LandmineTile().MoveToWorld(p, map);
                }

                foreach (Mobile m in map.GetMobilesInRange(p, 0))
                {
                    if (m != this && (m is PlayerMobile || (m is BaseCreature bc && bc.Team != Team)))
                    {
                        DoHarmful(m);
                        AOS.Damage(m, this, damage, 0, 0, 100, 0, 0);
                    }
                }
            }

            m_NextShatterStorm = DateTime.UtcNow.AddSeconds(15);
        }

        // AI Logic
        public override void OnThink()
        {
            base.OnThink();

            if (Combatant is Mobile target)
            {
                if (DateTime.UtcNow >= m_NextPrismBeam        && InRange(target, 18)) PrismBeamAttack();
                else if (DateTime.UtcNow >= m_NextChromaticBurst && InRange(target, 12)) ChromaticBurstAttack();
                else if (DateTime.UtcNow >= m_NextSpectrumVortex && InRange(target, 10)) SpectrumVortexAttack();
                else if (DateTime.UtcNow >= m_NextShatterStorm   && InRange(target, 12)) ShatterStormAttack();
                else if (DateTime.UtcNow >= m_NextPolarizingField)                     PolarizingFieldAbility();
            }
        }

        // OnDeath: massive prism explosion scattering random tiles
        public override void OnDeath(Container c)
        {
            if (Map == null)
            {
                base.OnDeath(c);
                return;
            }

            Effects.PlaySound(Location, Map, 0x208);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                0x373D, 14, 40, UniqueHue, 0, 2034, 0);

            int count = 30, radius = 6;
            for (int i = 0; i < count; i++)
            {
                Point3D p = GetRandomValidLocation(Location, radius, Map);
                if (p == Point3D.Zero) continue;

                Item tile;
                switch (Utility.Random(5))
                {
                    case 0: tile = new ChaoticTeleportTile(); break;
                    case 1: tile = new VortexTile();            break;
                    case 2: tile = new LightningStormTile();    break;
                    case 3: tile = new TrapWeb();               break;
                    default: tile = new VortexTile();        break;
                }
                tile.MoveToWorld(p, Map);

                Effects.SendLocationParticles(
                    EffectItem.Create(p, Map, EffectItem.DefaultDuration),
                    0x372A, 8, 20, UniqueHue, 0, 2035, 0);
            }

            base.OnDeath(c);
        }

        // Helper for OnDeath
        private Point3D GetRandomValidLocation(Point3D center, int radius, Map map)
        {
            for (int i = 0; i < 10; i++)
            {
                int x = center.X + Utility.RandomMinMax(-radius, radius);
                int y = center.Y + Utility.RandomMinMax(-radius, radius);
                int z = center.Z;
                Point3D p = new Point3D(x, y, z);

                if (map.CanFit(p, 16, false, false) && Utility.InRange(center, p, radius))
                    return p;

                int avgZ = map.GetAverageZ(x, y);
                Point3D p2 = new Point3D(x, y, avgZ);
                if (map.CanFit(p2, 16, false, false) && Utility.InRange(center, p2, radius))
                    return p2;
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
