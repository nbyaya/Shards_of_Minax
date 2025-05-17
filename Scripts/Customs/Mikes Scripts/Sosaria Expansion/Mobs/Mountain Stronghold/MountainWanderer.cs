using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a mountain wanderer corpse")]
    public class MountainWanderer : BaseCreature
    {
        // Ability timers
        private DateTime m_NextSeismicTime;
        private DateTime m_NextAvalancheTime;
        private DateTime m_NextPullTime;
        private DateTime m_NextSkinTime;

        // For drop‑trail
        private Point3D m_LastLocation;

        // Unique gray/steel hue
        private const int UniqueHue = 2211;

        [Constructable]
        public MountainWanderer()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name            = "a Mountain Wanderer";
            Body            = 316;
            BaseSoundID     = 377;
            Hue             = UniqueHue;

            // Stats
            SetStr(500, 600);
            SetDex(200, 250);
            SetInt(300, 400);

            SetHits(2000, 2300);
            SetStam(200, 250);
            SetMana(300, 400);

            SetDamage(20, 25);

            // Damage types
            SetDamageType(ResistanceType.Physical, 60);
            SetDamageType(ResistanceType.Cold,     40);

            // Resistances
            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire,     30, 40);
            SetResistance(ResistanceType.Cold,     80, 90);
            SetResistance(ResistanceType.Poison,   50, 60);
            SetResistance(ResistanceType.Energy,   50, 60);

            // Skills
            SetSkill(SkillName.EvalInt,     100.1, 110.0);
            SetSkill(SkillName.Magery,      100.1, 110.0);
            SetSkill(SkillName.MagicResist, 110.2, 125.0);
            SetSkill(SkillName.Meditation,   90.0, 100.0);
            SetSkill(SkillName.Tactics,      90.1, 100.0);
            SetSkill(SkillName.Wrestling,    90.1, 100.0);

            Fame           = 25000;
            Karma          = -25000;
            VirtualArmor   = 90;
            ControlSlots   = 5;

            // Cooldowns
            m_NextSeismicTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextAvalancheTime= DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
            m_NextPullTime     = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextSkinTime     = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 40));

            m_LastLocation = this.Location;

            // Starter loot
            PackItem(new Granite(Utility.RandomMinMax(10, 20)));
            PackItem(new BlackPearl(Utility.RandomMinMax(5, 10)));
        }

        // Chilling aura: whenever someone moves within 2 tiles, drain some stamina & cold‑damage
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (m != null && m != this && m.Map == this.Map && Alive && m.Alive && m.InRange(Location, 2) && CanBeHarmful(m, false))
            {
                if (m is Mobile target)
                {
                    DoHarmful(target);
                    int dmg = Utility.RandomMinMax(5, 10);
                    AOS.Damage(target, this, dmg, 0, 0, dmg, 0, 0);
                    target.Stam = Math.Max(0, target.Stam - Utility.RandomMinMax(10, 20));
                    target.SendMessage(0x482, "The mountain's chill saps your strength!");
                    target.FixedParticles(0x3728, 10, 15, UniqueHue, EffectLayer.Waist);
                    target.PlaySound(0x47E);
                }
            }
            base.OnMovement(m, oldLocation);
        }

        public override void OnThink()
        {
            base.OnThink();

            // Leave a jagged stone tile behind on movement
            if (this.Location != m_LastLocation && Map != null && Map != Map.Internal && Utility.RandomDouble() < 0.20)
            {
                var old = m_LastLocation;
                m_LastLocation = this.Location;

                if (!Map.CanFit(old.X, old.Y, old.Z, 16, false, false))
                    old.Z = Map.GetAverageZ(old.X, old.Y);

                var tile = new EarthquakeTile();
                tile.Hue = UniqueHue;
                tile.MoveToWorld(old, Map);
            }
            else
            {
                m_LastLocation = this.Location;
            }

            if (Combatant is Mobile target && Alive && Map != null && Map != Map.Internal && CanBeHarmful(target, false))
            {
                DateTime now = DateTime.UtcNow;

                if (now >= m_NextAvalancheTime && InRange(target.Location, 8))
                {
                    AvalancheBarrage(target);
                    m_NextAvalancheTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
                }
                else if (now >= m_NextSeismicTime && InRange(target.Location, 6))
                {
                    SeismicSlam();
                    m_NextSeismicTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
                }
                else if (now >= m_NextPullTime && InRange(target.Location, 12))
                {
                    MagneticPull(target);
                    m_NextPullTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(18, 26));
                }
                else if (now >= m_NextSkinTime)
                {
                    StoneSkinBarrier();
                    m_NextSkinTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(40, 50));
                }
            }
        }

        // --- Ability #1: Seismic Slam ---
        // 360° shockwave, physical damage + quake tiles
        public void SeismicSlam()
        {
            this.Say("*Feel the mountain's wrath!*");
            this.PlaySound(0x2A1);
            Effects.SendLocationParticles(
                EffectItem.Create(this.Location, Map, EffectItem.DefaultDuration),
                0x3779, 20, 60, UniqueHue, 0, 5032, 0);

            List<Mobile> list = new List<Mobile>();
            foreach (Mobile m in Map.GetMobilesInRange(Location, 6))
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                    list.Add(m);
            }

            foreach (var m in list)
            {
                DoHarmful(m);
                int dmg = Utility.RandomMinMax(30, 50);
                AOS.Damage(m, this, dmg, 100, 0, 0, 0, 0);

                // Drop quake tile
                var qc = new EarthquakeTile();
                qc.Hue = UniqueHue;
                qc.MoveToWorld(m.Location, Map);
            }
        }

        // --- Ability #2: Avalanche Barrage ---
        // Rains ice‑shard tiles along a line to the target
        public void AvalancheBarrage(Mobile target)
        {
            this.Say("*An avalanche approaches!*");
            this.PlaySound(0x653);

            int steps = 5;
            double dx = (target.X - X) / (double)steps;
            double dy = (target.Y - Y) / (double)steps;

            for (int i = 1; i <= steps; i++)
            {
                Point3D loc = new Point3D(
                    X + (int)Math.Round(dx * i),
                    Y + (int)Math.Round(dy * i),
                    Z);

                Timer.DelayCall(TimeSpan.FromSeconds(0.2 * i), () =>
                {
                    if (Map == null) return;

                    if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                        loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                    var ice = new IceShardTile();
                    ice.Hue = UniqueHue;
                    ice.MoveToWorld(loc, Map);
                    Effects.PlaySound(loc, Map, 0x48D);
                });
            }

            // Final direct hit
            Timer.DelayCall(TimeSpan.FromSeconds(steps * 0.2 + 0.1), () =>
            {
                if (CanBeHarmful(target, false))
                {
                    DoHarmful(target);
                    int dmg = Utility.RandomMinMax(40, 60);
                    AOS.Damage(target, this, dmg, 0, 0, dmg, 0, 0);
                    target.Freeze(TimeSpan.FromSeconds(2));
                    target.SendMessage(0x482, "You're pinned fast in the avalanche's ice!");
                    target.FixedParticles(0x374A, 10, 15, UniqueHue, EffectLayer.Head);
                }
            });
        }

        // --- Ability #3: Magnetic Pull ---
        // Snatches the target to melee range, dealing damage
        public void MagneticPull(Mobile target)
        {
            this.Say("*By iron and stone, come here!*");
            this.PlaySound(0x299);

            // Find an adjacent free spot
            Point3D dest = this.Location;
            foreach (var off in new Point3D[] {
                new Point3D( 1,  0, 0), new Point3D(-1,  0, 0),
                new Point3D( 0,  1, 0), new Point3D( 0, -1, 0)
            })
            {
                Point3D tryLoc = new Point3D(X + off.X, Y + off.Y, Z);
                if (Map.CanFit(tryLoc.X, tryLoc.Y, tryLoc.Z, 16, false, false))
                {
                    dest = tryLoc;
                    break;
                }
            }

            // Yank 'em over
            target.Location = dest;
            target.ProcessDelta();
            Effects.SendMovingParticles(
                new Entity(Serial.Zero, target.Location, Map),
                new Entity(Serial.Zero, this.Location, Map),
                0x36C4, 7, 0, false, false, UniqueHue, 0, 9502, 1, 0, 0);

            // Smash
            DoHarmful(target);
            int dmg = Utility.RandomMinMax(50, 70);
            AOS.Damage(target, this, dmg, 100, 0, 0, 0, 0);
        }

        // --- Ability #4: Stone Skin Barrier ---
        // Heals itself for a big chunk
        public void StoneSkinBarrier()
        {
            this.Say("*I am one with the mountain!*");
            this.PlaySound(0x214);
            this.FixedParticles(0x375A, 10, 15, UniqueHue, EffectLayer.CenterFeet);

            int healAmt = Utility.RandomMinMax(200, 300);
            this.Heal(healAmt);
        }

        // --- Death effect: quicksand hazards around the corpse ---
        public override void OnDeath(Container c)
        {
            if (Map != null)
            {
                this.Say("*The mountain... endures...*");
                Effects.PlaySound(Location, Map, 0x2A1);
                Effects.SendLocationParticles(
                    EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                    0x3779, 20, 60, UniqueHue, 0, 5032, 0);

                int count = Utility.RandomMinMax(4, 6);
                for (int i = 0; i < count; i++)
                {
                    int dx = Utility.RandomMinMax(-3, 3);
                    int dy = Utility.RandomMinMax(-3, 3);
                    Point3D loc = new Point3D(X + dx, Y + dy, Z);
                    if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                        loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                    var qs = new QuicksandTile();
                    qs.Hue = UniqueHue;
                    qs.MoveToWorld(loc, Map);
                }
            }

            base.OnDeath(c);
        }

        // Immunities & loot
        public override bool BleedImmune    => true;
        public override Poison PoisonImmune => Poison.Lethal;
        public override int TreasureMapLevel => 6;
        public override double DispelDifficulty => 150.0;
        public override double DispelFocus      => 75.0;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.Gems, Utility.RandomMinMax(10, 15));
            AddLoot(LootPack.MedScrolls, Utility.RandomMinMax(2, 4));

            if (Utility.RandomDouble() < 0.03) // 3% for unique core
                PackItem(new MeadowlightDress());
        }

        public MountainWanderer(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Reset timers
            m_NextSeismicTime   = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextAvalancheTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
            m_NextPullTime      = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextSkinTime      = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 40));

            m_LastLocation = this.Location;
        }
    }
}
