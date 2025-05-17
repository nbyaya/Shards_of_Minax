using System;
using System.Collections.Generic;
using System.Linq;                // ← for LINQ if you ever need it elsewhere
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Spells.Seventh;       // for chaining if desired

namespace Server.Mobiles
{
    [CorpseName("an obsidian serpent corpse")]
    public class ObsidianSerpent : BaseCreature
    {
        // Timers for special abilities
        private DateTime m_NextSpikeBarrage;
        private DateTime m_NextMagmaEruption;
        private DateTime m_NextVenomWave;
        private Point3D m_LastLocation;

        // Unique obsidian‐black hue
        private const int UniqueHue = 1175;

        [Constructable]
        public ObsidianSerpent()
            : base(AIType.AI_Melee, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name = "an Obsidian Serpent";
            Body = 90;               // same body as Lava Serpent
            BaseSoundID = 219;       // same sound
            Hue = UniqueHue;

            // --- Stats ---
            SetStr(450, 550);
            SetDex(100, 150);
            SetInt(100, 150);

            SetHits(1800, 2000);
            SetStam(200, 250);
            SetMana(0);

            SetDamage(20, 30);

            // --- Damage Types ---
            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Poison, 25);

            // --- Resistances ---
            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 40, 50);
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 40, 50);

            // --- Skills ---
            SetSkill(SkillName.Wrestling,     120.0, 130.0);
            SetSkill(SkillName.Tactics,       120.0, 130.0);
            SetSkill(SkillName.MagicResist,   120.0, 130.0);
            SetSkill(SkillName.Anatomy,       110.0, 120.0);

            VirtualArmor = 90;
            Fame = 25000;
            Karma = -25000;
            ControlSlots = 5;

            // Initialize ability cooldowns
            var now = DateTime.UtcNow;
            m_NextSpikeBarrage  = now + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextMagmaEruption = now + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextVenomWave     = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            m_LastLocation      = this.Location;

            // Loot
            PackItem(new ObsidianShard(Utility.RandomMinMax(5, 10)));
            PackGold(3000, 5000);
        }

        public ObsidianSerpent(Serial serial)
            : base(serial)
        {
        }

        // --- Venomous Aura on Movement ---
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            base.OnMovement(m, oldLocation);

            if (m == this || !Alive || !m.Alive || m.Map != this.Map)
                return;

            if (m.InRange(this.Location, 2) && CanBeHarmful(m, false))
            {
                DoHarmful(m);
                m.SendMessage("The air around the serpent grows toxic!");
                m.ApplyPoison(this, Poison.Lethal);

                // Spawn a localized cloud of toxic gas
                var tile = new ToxicGasTile();
                tile.Hue = UniqueHue;
                tile.MoveToWorld(m.Location, Map);
            }
        }

        public override void OnThink()
        {
            base.OnThink();

            if (!Alive || Combatant == null || Map == null || Map == Map.Internal)
                return;

            // Leave behind cracked obsidian shards as traps
            if (this.Location != m_LastLocation && Utility.RandomDouble() < 0.15)
            {
                var old = m_LastLocation;
                m_LastLocation = this.Location;

                Point3D dropLoc = old;
                if (!Map.CanFit(dropLoc.X, dropLoc.Y, dropLoc.Z, 16, false, false))
                    dropLoc.Z = Map.GetAverageZ(dropLoc.X, dropLoc.Y);

                var mine = new LandmineTile();
                mine.Hue = UniqueHue;
                mine.MoveToWorld(dropLoc, Map);
            }
            else
            {
                m_LastLocation = this.Location;
            }

            var now = DateTime.UtcNow;

            // Abilities prioritized by range & cooldown
            if (now >= m_NextSpikeBarrage && this.InRange(Combatant.Location, 12))
            {
                SpikeBarrage();
                m_NextSpikeBarrage = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
            else if (now >= m_NextMagmaEruption && this.InRange(Combatant.Location, 8))
            {
                MagmaEruption();
                m_NextMagmaEruption = now + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            }
            else if (now >= m_NextVenomWave && this.InRange(Combatant.Location, 6))
            {
                VenomWave();
                m_NextVenomWave = now + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
            }
        }

        // --- Obsidian Spike Barrage (Ranged AoE) ---
        private void SpikeBarrage()
        {
            if (!(Combatant is Mobile target) || !CanBeHarmful(target, false))
                return;

            Say("*Rrrraaah!*");
            PlaySound(0x227);

            // Fire moving shard‐like effects
            for (int i = 0; i < 6; i++)
            {
                Timer.DelayCall(TimeSpan.FromSeconds(i * 0.2), () =>
                {
                    // positional call includes the missing 'explodes' bool
                    Effects.SendMovingEffect(
                        this,                // from
                        target,              // to
                        0xF8B,               // itemID
                        7,                   // speed
                        0,                   // duration
                        false,               // fixedDirection
                        false,               // explodes
                        UniqueHue,           // hue
                        0                    // renderMode
                    );
                });
            }

            // After projectiles land, deal burst damage at the impact zone
            Timer.DelayCall(TimeSpan.FromSeconds(1.5), () =>
            {
                if (target.Map == null) return;

                Effects.SendLocationParticles(
                    EffectItem.Create(target.Location, Map, EffectItem.DefaultDuration),
                    0x3709,  // explosion effect
                    10, 30, UniqueHue, 0, 5032, 0);

                // IPooledEnumerable<Mobile> → just iterate directly
                foreach (Mobile m in Map.GetMobilesInRange(target.Location, 3))
                {
                    if (m != this && CanBeHarmful(m, false))
                    {
                        DoHarmful(m);
                        AOS.Damage(m, this, Utility.RandomMinMax(40, 60), 100, 0, 0, 0, 0);
                    }
                }
            });
        }

        // --- Magma Eruption (Ground Hazard) ---
        private void MagmaEruption()
        {
            if (!(Combatant is Mobile target) || !CanBeHarmful(target, false))
                return;

            Say("*Feel the fury of the earth!*");
            PlaySound(0x208);

            var center = target.Location;
            for (int dx = -2; dx <= 2; dx++)
            for (int dy = -2; dy <= 2; dy++)
            {
                if (Math.Abs(dx) == 2 || Math.Abs(dy) == 2)
                {
                    var loc = new Point3D(center.X + dx, center.Y + dy, center.Z);
                    if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                        loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                    var lava = new HotLavaTile();
                    lava.Hue = UniqueHue;
                    lava.MoveToWorld(loc, Map);
                }
            }
        }

        // --- Venom Wave (Cone + Poison) ---
        private void VenomWave()
        {
            if (!(Combatant is Mobile target) || !CanBeHarmful(target, false))
                return;

            Say("*Ssss… taste the poison!*");
            PlaySound(0x23B);

            var origin = this.Location;
            var dir = this.GetDirectionTo(target.Location);

            // Directly iterate—you already have Mobiles
            foreach (Mobile m in Map.GetMobilesInRange(origin, 5))
            {
                if (m == this || !CanBeHarmful(m, false)) continue;

                if (this.InLOS(m) && Utility.GetDirection(origin, m.Location) == dir)
                {
                    DoHarmful(m);
                    m.ApplyPoison(this, Poison.Lethal);
                    m.SendMessage("Venomous mist coats you!");
                }
            }

            for (int i = 1; i <= 3; i++)
            {
                var loc = origin;
                switch (dir)
                {
                    case Direction.North: loc.Y -= i; break;
                    case Direction.South: loc.Y += i; break;
                    case Direction.West:  loc.X -= i; break;
                    default:              loc.X += i; break;
                }

                if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                    loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                var gas = new PoisonTile();
                gas.Hue = UniqueHue;
                gas.MoveToWorld(loc, Map);
            }
        }

        // --- Death Explosion & Hazards ---
        public override void OnDeath(Container c)
        {
            if (Map != null)
            {
                Say("*…Rrroooaaar…*");
                PlaySound(0x22F);
                Effects.SendLocationParticles(
                    EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                    0x3709, 20, 30, UniqueHue, 0, 5052, 0);

                for (int i = 0; i < 5; i++)
                {
                    var dx = Utility.RandomMinMax(-3, 3);
                    var dy = Utility.RandomMinMax(-3, 3);
                    var loc = new Point3D(X + dx, Y + dy, Z);

                    if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                        loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                    var quake = new EarthquakeTile();
                    quake.Hue = UniqueHue;
                    quake.MoveToWorld(loc, Map);
                }
            }

            base.OnDeath(c);
        }

        // --- Loot & Properties ---
        public override bool BleedImmune => true;
        public override int TreasureMapLevel => 6;
        public override double DispelDifficulty => 145.0;
        public override double DispelFocus     => 75.0;

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Reset timers
            var now = DateTime.UtcNow;
            m_NextSpikeBarrage  = now + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextMagmaEruption = now + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextVenomWave     = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            m_LastLocation      = this.Location;
        }
    }
}
