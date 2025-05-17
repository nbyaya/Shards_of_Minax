using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Network;
using Server.Mobiles;
using Server.Spells;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("a burrower keppto corpse")]
    public class BurrowerKeppto : BaseCreature
    {
        private DateTime m_NextBurrowTime;
        private DateTime m_NextSporeTime;
        private DateTime m_NextQuakeTime;
        private Point3D m_LastLocation;

        // Deep earthen‐green to evoke its subterranean nature
        private const int UniqueHue = 2627;

        [Constructable]
        public BurrowerKeppto()
            : base(AIType.AI_Melee, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name = "a Burrower Keppto";
            Body = 726;                       // Same body as a standard Kepetch
            Hue = UniqueHue;
            BaseSoundID = 1545;               // We'll override specifics below

            // — Stats —
            SetStr(500, 600);
            SetDex(150, 200);
            SetInt( 80, 120);

            SetHits(1000, 1200);
            SetStam(200, 250);
            SetMana(100, 150);

            SetDamage(10, 20);

            // — Damage Types —
            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Poison, 50);

            // — Resistances —
            SetResistance(ResistanceType.Physical, 70, 80);
            SetResistance(ResistanceType.Fire,     30, 50);
            SetResistance(ResistanceType.Cold,     40, 60);
            SetResistance(ResistanceType.Poison,   60, 80);
            SetResistance(ResistanceType.Energy,   50, 70);

            // — Skills —
            SetSkill(SkillName.Anatomy,      120.0, 130.0);
            SetSkill(SkillName.Tactics,      120.0, 130.0);
            SetSkill(SkillName.Wrestling,    120.0, 130.0);
            SetSkill(SkillName.MagicResist,  100.0, 110.0);
            SetSkill(SkillName.Meditation,    80.0,  90.0);
            SetSkill(SkillName.Poisoning,     90.0, 100.0);
            SetSkill(SkillName.DetectHidden,  50.0);

            Fame = 18000;
            Karma = -18000;

            VirtualArmor = 90;
            ControlSlots = 5;

            // Initialize ability cooldowns
            var now = DateTime.UtcNow;
            m_NextBurrowTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));
            m_NextSporeTime  = now + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 18));
            m_NextQuakeTime  = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));

            // Loot: raw hides, chitin shards, chance at rare drop
            PackItem(new Hides(20));
            PackItem(new Bone(Utility.RandomMinMax(10, 15)));
            if (Utility.RandomDouble() < 0.03) // 3% chance
                PackItem(new SunmirrorDome()); // Placeholder for a unique chitin armor piece

            m_LastLocation = this.Location;
        }

        // — Ambient Movement Effect: Quicksand Patches —
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            base.OnMovement(m, oldLocation);

            if (this.Map == null || this.Map == Map.Internal)
                return;

            // Leave quicksand tile occasionally behind as it skitters
            if (this.Location != m_LastLocation && Utility.RandomDouble() < 0.15)
            {
                var sandLoc = m_LastLocation;
                var map     = this.Map;

                if (!map.CanFit(sandLoc.X, sandLoc.Y, sandLoc.Z, 16, false, false))
                    sandLoc.Z = map.GetAverageZ(sandLoc.X, sandLoc.Y);

                var tile = new QuicksandTile();
                tile.Hue = UniqueHue;
                tile.MoveToWorld(sandLoc, map);
            }

            m_LastLocation = this.Location;
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || !Alive || Map == null || Map == Map.Internal)
                return;

            var now = DateTime.UtcNow;

            // Burrow Ambush
            if (now >= m_NextBurrowTime && this.InRange(Combatant.Location, 12))
            {
                BurrowAmbush();
                m_NextBurrowTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
            }
            // Poison Spore Cloud
            else if (now >= m_NextSporeTime && this.InRange(Combatant.Location, 8))
            {
                SporeCloudAttack();
                m_NextSporeTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 20));
            }
            // Seismic Quake
            else if (now >= m_NextQuakeTime && this.InRange(Combatant.Location, 10))
            {
                EarthquakeAttack();
                m_NextQuakeTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 40));
            }
        }

        private void BurrowAmbush()
        {
            if (!(Combatant is Mobile target) || !CanBeHarmful(target, false))
                return;

            this.Say("*The earth rumbles...*");
            this.PlaySound(0x4F1);      // Burrow‐into‐earth sound
            this.Hidden = true;

            Timer.DelayCall(TimeSpan.FromSeconds(3.0), () =>
            {
                if (!Alive || target.Deleted || target.Map != this.Map)
                    return;

                // Emerge beneath target
                this.Location = new Point3D(target.X, target.Y, target.Z);
                this.Hidden   = false;

                this.PlaySound(0x4F2);    // Emerge sound
                this.FixedParticles(0x3709, 10, 30, 5050, UniqueHue, 0, EffectLayer.Waist);

                DoHarmful(target);
                AOS.Damage(target, this, Utility.RandomMinMax(40, 60), 0, 0, 0, 0, 0, 100);
            });
        }

        private void SporeCloudAttack()
        {
            this.Say("*Toxic spores erupt!*");
            this.PlaySound(0x2C5);

            var list = new List<Mobile>();
            foreach (var m in Map.GetMobilesInRange(this.Location, 6))
            {
                if (m != this && m.Alive && CanBeHarmful(m, false))
                    list.Add(m);
            }

            foreach (var m in list)
            {
                DoHarmful(m);
                AOS.Damage(m, this, Utility.RandomMinMax(20, 35), 0, 0, 20, 0, 80);
                if (m is Mobile mob) mob.ApplyPoison(this, Poison.Greater);

                Effects.SendLocationParticles(
                    EffectItem.Create(m.Location, Map, EffectItem.DefaultDuration),
                    0x370A, 20, 30, UniqueHue, 0, 5032, 0
                );
            }
        }

        private void EarthquakeAttack()
        {
            this.Say("*The earth shatters!*");
            this.PlaySound(0x2A9); // Earthquake rumble

            // Drop several earthquake hazard tiles around self
            for (int i = 0; i < Utility.RandomMinMax(3, 5); i++)
            {
                int dx = Utility.RandomMinMax(-4, 4);
                int dy = Utility.RandomMinMax(-4, 4);
                var loc = new Point3D(X + dx, Y + dy, Z);

                if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                    loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                var tile = new EarthquakeTile();
                tile.Hue = UniqueHue;
                tile.MoveToWorld(loc, Map);

                Effects.SendLocationParticles(
                    EffectItem.Create(loc, Map, EffectItem.DefaultDuration),
                    0x3779, 10, 20, UniqueHue, 0, 5039, 0
                );
            }
        }

        public override void OnDeath(Container c)
        {
            if (Map != null)
            {
                this.Say("*The earth claims my remains...*");
                this.PlaySound(0x4F2);

                // Erupt quicksand hazards around corpse
                for (int i = 0; i < Utility.RandomMinMax(4, 7); i++)
                {
                    int dx = Utility.RandomMinMax(-3, 3);
                    int dy = Utility.RandomMinMax(-3, 3);
                    var loc = new Point3D(X + dx, Y + dy, Z);

                    if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                        loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                    var qs = new QuicksandTile();
                    qs.Hue = UniqueHue;
                    qs.MoveToWorld(loc, Map);
                }
            }

            base.OnDeath(c);
        }

        // — Overrides & Properties —
        public override bool BleedImmune { get { return true; } }
        public override int Meat       { get { return 10; } }
        public override int Hides      { get { return 20; } }
        public override HideType HideType     { get { return HideType.Spined; } }
        public override FoodType FavoriteFood { get { return FoodType.FruitsAndVegies | FoodType.GrainsAndHay; } }

        public override int GetIdleSound()  { return 1545; }
        public override int GetAngerSound() { return 1542; }
        public override int GetHurtSound()  { return 1544; }
        public override int GetDeathSound() { return 1543; }

		public BurrowerKeppto(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version

            // timings not preserved across saves
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Reset timers on load
            var now = DateTime.UtcNow;
            m_NextBurrowTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));
            m_NextSporeTime  = now + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 18));
            m_NextQuakeTime  = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
        }
    }
}
