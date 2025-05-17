using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;   // For SpellHelper if needed

namespace Server.Mobiles
{
    [CorpseName("a golden dragon corpse")]
    public class GoldDragon : BaseCreature
    {
        // Cooldown timers
        private DateTime m_NextSolarBeam;
        private DateTime m_NextGildedCone;
        private DateTime m_NextSunflareRing;
        private DateTime m_NextRadiantNova;
        private DateTime m_NextReflectiveWard;

        // Unique gold hue
        private const int UniqueHue = 2101;

        [Constructable]
        public GoldDragon()
            : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name = "Gold Dragon";
            Body = Utility.RandomList(12, 59);
            BaseSoundID = 362;
            Hue = UniqueHue;

            // Stats
            SetStr(1300, 1500);
            SetDex(120, 200);
            SetInt(900, 1000);

            SetHits(1500, 2000);
            SetDamage(50, 60);

            // Damage distribution
            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire,     50);

            // Resistances
            SetResistance(ResistanceType.Physical, 80, 90);
            SetResistance(ResistanceType.Fire,     100, 100);
            SetResistance(ResistanceType.Cold,     80, 90);
            SetResistance(ResistanceType.Poison,   80, 90);
            SetResistance(ResistanceType.Energy,   75, 85);

            // Skills
            SetSkill(SkillName.EvalInt,   120.0, 140.0);
            SetSkill(SkillName.Magery,    120.0, 140.0);
            SetSkill(SkillName.Meditation,100.0, 120.0);
            SetSkill(SkillName.MagicResist,140.0, 180.0);
            SetSkill(SkillName.Tactics,   120.0, 150.0);
            SetSkill(SkillName.Wrestling, 120.0, 150.0);

            Fame = 40000;
            Karma = -40000;
            VirtualArmor = 100;

            // Initialize cooldowns so that it can act immediately
            m_NextSolarBeam     = DateTime.UtcNow;
            m_NextGildedCone    = DateTime.UtcNow;
            m_NextSunflareRing  = DateTime.UtcNow;
            m_NextRadiantNova   = DateTime.UtcNow;
            m_NextReflectiveWard= DateTime.UtcNow;
        }

        public GoldDragon(Serial serial) : base(serial) { }

        // --- Properties ---

        public override bool ReacquireOnMovement => true;
        public override bool AutoDispel         => true;
        public override bool CanFly             => true;

        public override HideType HideType => HideType.Barbed;
        public override int Hides        => 50;
        public override int Scales       => 20;
        public override ScaleType ScaleType => ScaleType.Yellow;

        public override Poison PoisonImmune => Poison.Lethal;
        public override Poison HitPoison    => Poison.Deadly;

        public override int TreasureMapLevel => 7;

        // --- Loot ---

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 5);
            AddLoot(LootPack.Gems,        10);
            PackGold(5000, 10000);

            // 2% chance for a unique golden scale item
            if (Utility.RandomDouble() < 0.02)
                PackItem(new ImpalerOfGloomhill()); // define this elsewhere
        }

        // --- Special Abilities ---

        // 1) Solar Beam: long line of radiant energy
        public void SolarBeamAttack()
        {
            if (!(Combatant is Mobile target)) return;
            var map = Map;
            if (map == null || !this.InRange(target, 20)) return;

            // Direction & beam length
            var dir = GetDirectionTo(target);
			int dx = 0, dy = 0;
			Movement.Movement.Offset(dir, ref dx, ref dy);
            int length = 20, dmg = 60;

            Effects.PlaySound(Location, map, 0x658);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, map, EffectItem.DefaultDuration),
                0x3728, 12, 12, UniqueHue, 0, 2032, 0);

            for (int i = 1; i <= length; i++)
            {
                var p = new Point3D(X + dx*i, Y + dy*i, Z);
                if (!map.CanFit(p, 16, false, false)) break;

                Effects.SendLocationEffect(p, map, 0x3818, 16, UniqueHue, 0);
                foreach (var m in map.GetMobilesInRange(p, 0))
                {
                    if (m == this) continue;
                    if (m is PlayerMobile || (m is BaseCreature bc && bc.Team != Team))
                    {
                        DoHarmful(m);
                        AOS.Damage(m, this, dmg, 0, 0, 0, 0, 100);
                    }
                }
            }

            m_NextSolarBeam = DateTime.UtcNow + TimeSpan.FromSeconds(12);
        }

        // 2) Gilded Cone: a golden cone of searing flame
        public void GildedConeAttack()
        {
            if (!(Combatant is Mobile target)) return;
            var map = Map;
            if (map == null || !this.InRange(target, 10)) return;

            int range = 10, width = 4, dmg = 30;
            var dir = GetDirectionTo(target);
			int dx = 0, dy = 0;
			Movement.Movement.Offset(dir, ref dx, ref dy);

            Effects.PlaySound(Location, map, BaseSoundID);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, map, EffectItem.DefaultDuration),
                0x36B0, 12, 12, UniqueHue, 0, 2038, 0);

            var cone = new List<Point3D>();
            for (int i = 1; i <= range; i++)
            {
                int spread = (i * width) / range;
                for (int j = -spread; j <= spread; j++)
                {
                    int tx = X + dx * i + ((dy == 0) ? j : 0);
                    int ty = Y + dy * i + ((dx == 0) ? j : 0);
                    var p = new Point3D(tx, ty, Z);
                    if (!map.CanFit(p, 16, false, false))
                        p = new Point3D(tx, ty, map.GetAverageZ(tx, ty));

                    if (map.CanFit(p, 16, false, false))
                        cone.Add(p);
                }
            }

            foreach (var p in cone)
            {
                Effects.SendLocationParticles(
                    EffectItem.Create(p, map, EffectItem.DefaultDuration),
                    0x3709, 10, 10, UniqueHue, 0, 2044, 0);

                foreach (var m in map.GetMobilesInRange(p, 0))
                {
                    if (m == this) continue;
                    if (m is PlayerMobile || (m is BaseCreature bc && bc.Team != Team))
                    {
                        DoHarmful(m);
                        AOS.Damage(m, this, dmg, 0, 100, 0, 0, 0);
                    }
                }

                // 30% chance to leave a FlamestrikeHazardTile at each point
                if (Utility.RandomDouble() < 0.3)
                {
                    // var tile = new FlamestrikeHazardTile();
                    // tile.MoveToWorld(p, map);
                }
            }

            m_NextGildedCone = DateTime.UtcNow + TimeSpan.FromSeconds(14);
        }

        // 3) Sunflare Ring: ring of lightning around self
        public void SunflareRingAttack()
        {
            var map = Map;
            if (map == null) return;

            int radius = 6, dmg = 35;
            Effects.PlaySound(Location, map, 0x1FE);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, map, EffectItem.DefaultDuration),
                0x37C4, 1, 8, UniqueHue, 0, 9910, 0);

            for (int x = -radius; x <= radius; x++)
            {
                for (int y = -radius; y <= radius; y++)
                {
                    if (Math.Abs(x * x + y * y - radius * radius) <= radius)
                    {
                        var p = new Point3D(X + x, Y + y, Z);
                        if (!map.CanFit(p, 16, false, false))
                            p = new Point3D(p.X, p.Y, map.GetAverageZ(p.X, p.Y));

                        if (!map.CanFit(p, 16, false, false))
                            continue;

                        Effects.SendLocationParticles(
                            EffectItem.Create(p, map, EffectItem.DefaultDuration),
                            0x381C, 6, 6, UniqueHue, 0, 2030, 0);

                        foreach (var m in map.GetMobilesInRange(p, 0))
                        {
                            if (m == this) continue;
                            if (m is PlayerMobile || (m is BaseCreature bc && bc.Team != Team))
                            {
                                DoHarmful(m);
                                AOS.Damage(m, this, dmg, 0, 0, 0, 0, 100);
                            }
                        }

                        // 20% chance to spawn a LightningStormTile
                        if (Utility.RandomDouble() < 0.2)
                        {
                            // var storm = new LightningStormTile();
                            // storm.MoveToWorld(p, map);
                        }
                    }
                }
            }

            m_NextSunflareRing = DateTime.UtcNow + TimeSpan.FromSeconds(18);
        }

        // 4) Radiant Nova: a burst around the dragon, leaves hot lava
        public void RadiantNovaAttack()
        {
            var map = Map;
            if (map == null) return;

            int radius = 5, dmg = 45;
            Effects.PlaySound(Location, map, 0x208);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, map, EffectItem.DefaultDuration),
                0x3709, 12, 24, UniqueHue, 0, 2048, 0);

            for (int x = -radius; x <= radius; x++)
            {
                for (int y = -radius; y <= radius; y++)
                {
                    var p = new Point3D(X + x, Y + y, Z);
                    if (Utility.InRange(Location, p, radius))
                    {
                        if (!map.CanFit(p, 16, false, false))
                            p = new Point3D(p.X, p.Y, map.GetAverageZ(p.X, p.Y));

                        Effects.SendLocationParticles(
                            EffectItem.Create(p, map, EffectItem.DefaultDuration),
                            0x3039, 8, 8, UniqueHue, 0, 2020, 0);

                        foreach (var m in map.GetMobilesInRange(p, 0))
                        {
                            if (m == this) continue;
                            if (m is PlayerMobile || (m is BaseCreature bc && bc.Team != Team))
                            {
                                DoHarmful(m);
                                AOS.Damage(m, this, dmg, 0, 100, 0, 0, 0);
                            }
                        }

                        // 25% chance to spawn a HotLavaTile
                        if (Utility.RandomDouble() < 0.25)
                        {
                            // var lava = new HotLavaTile();
                            // lava.MoveToWorld(p, map);
                        }
                    }
                }
            }

            m_NextRadiantNova = DateTime.UtcNow + TimeSpan.FromSeconds(16);
        }

        // 5) Reflective Ward: zone that spawns healing and mana‐drain tiles
        public void ReflectiveWardAbility()
        {
            var map = Map;
            if (map == null) return;

            int radius = 4;
            Effects.PlaySound(Location, map, 0x1F3);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, map, EffectItem.DefaultDuration),
                0x37CB, 1, 10, UniqueHue, 0, 9911, 0);

            for (int x = -radius; x <= radius; x++)
            {
                for (int y = -radius; y <= radius; y++)
                {
                    var p = new Point3D(X + x, Y + y, Z);
                    if (Utility.InRange(Location, p, radius))
                    {
                        if (!map.CanFit(p, 16, false, false))
                            p = new Point3D(p.X, p.Y, map.GetAverageZ(p.X, p.Y));

                        // 40% chance per tile
                        if (Utility.RandomDouble() < 0.4)
                        {
                            if (Utility.RandomBool())
                            {
                                // var heal = new HealingPulseTile();
                                // heal.MoveToWorld(p, map);
                            }
                            else
                            {
                                // var mana = new ManaDrainTile();
                                // mana.MoveToWorld(p, map);
                            }
                        }
                    }
                }
            }

            m_NextReflectiveWard = DateTime.UtcNow + TimeSpan.FromSeconds(22);
        }

        // --- AI Logic ---
        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                // Prioritize the beam if in long range
                if (DateTime.UtcNow >= m_NextSolarBeam && this.InRange(Combatant, 20))
                    SolarBeamAttack();
                else if (DateTime.UtcNow >= m_NextGildedCone    && this.InRange(Combatant, 10))
                    GildedConeAttack();
                else if (DateTime.UtcNow >= m_NextSunflareRing  && Utility.InRange(Location, Combatant.Location, 8))
                    SunflareRingAttack();
                else if (DateTime.UtcNow >= m_NextRadiantNova   && Utility.InRange(Location, Combatant.Location, 6))
                    RadiantNovaAttack();
                else if (DateTime.UtcNow >= m_NextReflectiveWard)
                    ReflectiveWardAbility();
            }
        }

        // --- OnDeath: golden explosion ---
        public override void OnDeath(Container c)
        {
            if (Map != null)
            {
                Effects.PlaySound(Location, Map, 0x208);
                Effects.SendLocationParticles(
                    EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                    0x3709, 15, 30, UniqueHue, 0, 2050, 0);
            }

            base.OnDeath(c);
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
