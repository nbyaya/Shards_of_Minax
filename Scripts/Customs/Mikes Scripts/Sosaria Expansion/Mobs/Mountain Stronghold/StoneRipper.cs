using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;    // for SpellHelper.ValidIndirectTarget
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("a stone ripper corpse")]
    public class StoneRipper : BaseCreature
    {
        // Ability cooldown timers
        private DateTime m_NextQuakeTime;
        private DateTime m_NextBoulderTime;
        private DateTime m_NextShrapnelTime;

        // For leaving hazards as it moves
        private Point3D m_LastLocation;

        // Deep granite hue
        private const int UniqueHue = 2410;

        [Constructable]
        public StoneRipper()
            : base(AIType.AI_Melee, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name = "a Stone Ripper";
            Body = 47;
            BaseSoundID = 442;
            Hue = UniqueHue;

            // ——— Stats ———
            SetStr(350, 450);
            SetDex(100, 150);
            SetInt(50,  80);

            SetHits(1500, 1800);
            SetStam(200, 250);
            SetMana(0);

            SetDamage(20, 30);

            // Pure physical damage
            SetDamageType(ResistanceType.Physical, 100);

            // Resistances
            SetResistance(ResistanceType.Physical, 70, 80);
            SetResistance(ResistanceType.Fire,     30, 40);
            SetResistance(ResistanceType.Cold,     40, 50);
            SetResistance(ResistanceType.Poison,   50, 60);
            SetResistance(ResistanceType.Energy,   40, 50);

            // Combat skills
            SetSkill(SkillName.Wrestling,   100.1, 110.0);
            SetSkill(SkillName.Tactics,     100.1, 110.0);
            SetSkill(SkillName.MagicResist,  90.1, 100.0);

            Fame       = 18000;
            Karma      = -18000;
            VirtualArmor = 60;
            ControlSlots = 5;

            // Staggered cooldowns
            m_NextQuakeTime     = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextBoulderTime   = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            m_NextShrapnelTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            m_LastLocation      = this.Location;

            // Loot: heavy metal and gems
            PackItem(new IronIngot(Utility.RandomMinMax(10, 15)));
            PackGem(5, 10);
        }

        // ——— Leave Quicksand in its wake ———
        public override void OnThink()
        {
            base.OnThink();

            // Movement hazard
            if (this.Location != m_LastLocation && Map != null && Map != Map.Internal && Utility.RandomDouble() < 0.30)
            {
                var old = m_LastLocation;
                m_LastLocation = this.Location;

                // Try original Z, else average Z
                if (!Map.CanFit(old.X, old.Y, old.Z, 16, false, false))
                    old.Z = Map.GetAverageZ(old.X, old.Y);

                if (Map.CanFit(old.X, old.Y, old.Z, 16, false, false))
                {
                    var qs = new QuicksandTile { Hue = UniqueHue };
                    qs.MoveToWorld(old, Map);
                }
            }
            else
            {
                m_LastLocation = this.Location;
            }

            // Ability triggers
            if (Combatant == null || !Alive || Map == null || Map == Map.Internal) 
                return;

            if (DateTime.UtcNow >= m_NextQuakeTime 
                && Combatant is Mobile quakeTarget 
                && InRange(quakeTarget.Location, 8))
            {
                SeismicSmash();
                m_NextQuakeTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 16));
            }
            else if (DateTime.UtcNow >= m_NextBoulderTime)
            {
                BoulderToss();
                m_NextBoulderTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(14, 20));
            }
            else if (DateTime.UtcNow >= m_NextShrapnelTime)
            {
                StoneShrapnel();
                m_NextShrapnelTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
            }
        }

        // ——— Seismic Smash: AoE physical + brief stun ———
        public void SeismicSmash()
        {
            if (Map == null) return;
            Say("*The earth trembles!*");
            PlaySound(0x214);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                0x376A, 8, 50, UniqueHue, 0, 5039, 0);

            var targets = new List<Mobile>();
            foreach (Mobile m in Map.GetMobilesInRange(Location, 6))
            {
                if (m != this && m.Alive && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                    targets.Add(m);
            }

            foreach (var m in targets)
            {
                DoHarmful(m);
                int dmg = Utility.RandomMinMax(40, 60);
                AOS.Damage(m, this, dmg, 100, 0, 0, 0, 0);
                m.Freeze(TimeSpan.FromSeconds(1.5));
            }
        }

        // ——— Boulder Toss: single‑target ranged physical ———
        public void BoulderToss()
        {
            if (Combatant is Mobile target && CanBeHarmful(target, false) && InRange(target.Location, 12))
            {
                Say("*Feel my boulder!*");
                PlaySound(0x2A3);

                var from = Location;
                var to   = target.Location;
                Effects.SendMovingParticles(
                    new Entity(Serial.Zero, from, Map),
                    new Entity(Serial.Zero, to,   Map),
                    0x3EB9, 7, 0, false, false,
                    UniqueHue, 0, 9532, 0, 0, EffectLayer.Head, 0x100);

                // Delayed impact
                Timer.DelayCall(TimeSpan.FromSeconds(0.5), () =>
                {
                    if (CanBeHarmful(target, false))
                    {
                        DoHarmful(target);
                        int damage = Utility.RandomMinMax(50, 70);
                        AOS.Damage(target, this, damage, 100, 0, 0, 0, 0);
                    }
                });
            }
        }

        // ——— Stone Shrapnel: spawn landmine hazards around self ———
        public void StoneShrapnel()
        {
            if (Map == null) return;
            Say("*Shrapnel!*");
            PlaySound(0x370);

            for (int i = 0; i < 5; i++)
            {
                int xOff = Utility.RandomMinMax(-3, 3);
                int yOff = Utility.RandomMinMax(-3, 3);
                var loc = new Point3D(X + xOff, Y + yOff, Z);

                if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                    loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                    continue;

                var mine = new LandmineTile { Hue = UniqueHue };
                mine.MoveToWorld(loc, Map);

                Effects.SendLocationParticles(
                    EffectItem.Create(loc, Map, EffectItem.DefaultDuration),
                    0x376A, 5, 20, UniqueHue, 0, 5039, 0);
            }
        }

        // ——— Deathquake: spawn earthquake hazards on death ———
        public override void OnDeath(Container c)
        {
            if (Map != null)
            {
                Say("*The earth claims me...*");
                PlaySound(0x214);

                for (int i = 0; i < 6; i++)
                {
                    int xOff = Utility.RandomMinMax(-4, 4);
                    int yOff = Utility.RandomMinMax(-4, 4);
                    var loc = new Point3D(X + xOff, Y + yOff, Z);

                    if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                        loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                    if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                        continue;

                    var eq = new EarthquakeTile { Hue = UniqueHue };
                    eq.MoveToWorld(loc, Map);

                    Effects.SendLocationParticles(
                        EffectItem.Create(loc, Map, EffectItem.DefaultDuration),
                        0x376A, 10, 30, UniqueHue, 0, 5039, 0);
                }
            }

            base.OnDeath(c);
        }

        // ——— Misc overrides ———
        public override bool BleedImmune   { get { return true; } }
        public override Poison PoisonImmune{ get { return Poison.Greater; } }
        public override int TreasureMapLevel{ get { return 5; } }
        public override double DispelDifficulty { get { return 120.0; } }
        public override double DispelFocus      { get { return 50.0; } }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.Gems, Utility.RandomMinMax(10, 15));
            PackItem(new IronIngot(Utility.RandomMinMax(20, 25)));
            if (Utility.RandomDouble() < 0.05)
                PackItem(new GauntletsOfTheFinalHammer()); // example unique artifact
        }

        // Serialization
        public StoneRipper(Serial serial) : base(serial) { }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Re‑init timers & position
            m_NextQuakeTime     = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextBoulderTime   = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            m_NextShrapnelTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            m_LastLocation      = this.Location;
        }
    }
}
