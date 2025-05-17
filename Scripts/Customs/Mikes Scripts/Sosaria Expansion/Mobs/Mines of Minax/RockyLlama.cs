using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a rocky llama corpse")]
    public class RockyLlama : BaseCreature
    {
        // Cooldowns for special abilities
        private DateTime m_NextStomp;
        private DateTime m_NextSpit;
        private DateTime m_NextBarrage;

        // A dusty‐stone hue
        private const int UniqueHue = 2301;

        [Constructable]
        public RockyLlama() 
            : base(AIType.AI_Melee, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name = "a rocky llama";
            Body = 0xDC;
            BaseSoundID = 0x3F3;
            Hue = UniqueHue;

            // Stats
            SetStr(300, 350);
            SetDex(100, 150);
            SetInt(50,  80);

            SetHits(2000, 2500);
            SetStam(150,  200);
            SetMana(0);

            SetDamage(18, 25);

            // All physical damage
            SetDamageType(ResistanceType.Physical, 100);

            // Resistances
            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire,     30, 40);
            SetResistance(ResistanceType.Cold,     30, 40);
            SetResistance(ResistanceType.Poison,   25, 35);
            SetResistance(ResistanceType.Energy,   20, 30);

            // Combat skills
            SetSkill(SkillName.Tactics,      100.1, 120.0);
            SetSkill(SkillName.Wrestling,    100.1, 120.0);
            SetSkill(SkillName.MagicResist,   80.0, 100.0);

            Fame = 18000;
            Karma = -18000;
            VirtualArmor = 60;
            ControlSlots = 5;

            // Schedule first uses
            var now = DateTime.UtcNow;
            m_NextStomp   = now + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextSpit    = now + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            m_NextBarrage = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));

            // Dungeon‐themed loot
            PackGold(1000, 2000);
            PackItem(new DullCopperOre(Utility.RandomMinMax(10, 20)));
            PackItem(new IronOre(Utility.RandomMinMax(10, 20)));
            AddLoot(LootPack.Rich);
        }

        // Creates landmine hazards under those who get too close
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            base.OnMovement(m, oldLocation);

            if (m is Mobile target
             && target != this
             && target.Map == this.Map
             && target.InRange(this.Location, 2)
             && target.Alive
             && CanBeHarmful(target, false)
             && Utility.RandomDouble() < 0.20)
            {
                var mine = new LandmineTile();
                mine.Hue = UniqueHue;

                if (Map.CanFit(target.Location.X, target.Location.Y, target.Location.Z, 16, false, false))
                    mine.MoveToWorld(target.Location, this.Map);
            }
        }

        // Main AI loop for special attacks
        public override void OnThink()
        {
            base.OnThink();

            if (!(Combatant is Mobile target) || !Alive || Map == null || Map == Map.Internal)
                return;

            var now = DateTime.UtcNow;

            // Prioritize by cooldown and range
            if (now >= m_NextSpit && InRange(target.Location, 12))
            {
                SpikedSpit(target);
                m_NextSpit = now + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            }
            else if (now >= m_NextBarrage && InRange(target.Location, 10))
            {
                BoulderBarrage(target);
                m_NextBarrage = now + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
            }
            else if (now >= m_NextStomp && InRange(target.Location, 6))
            {
                EarthquakeStomp();
                m_NextStomp = now + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));
            }
        }

        // --- Ability #1: Spiked Spit (single‐target ranged attack) ---
        private void SpikedSpit(Mobile target)
        {
            if (!CanBeHarmful(target, false)) return;

            Say("*Khaaa!*");
            PlaySound(0x23D); // spit sound

            // Projectile effect from llama to target
            Effects.SendMovingParticles(
                new Entity(Serial.Zero, this.Location, this.Map),
                new Entity(Serial.Zero, target.Location, target.Map),
                0x36D4, 7, 0, false, false, UniqueHue, 0, 9502, 0, 0, EffectLayer.Head, 0);

            DoHarmful(target);
            AOS.Damage(target, this, Utility.RandomMinMax(30, 45), 100, 0, 0, 0, 0);
        }

        // --- Ability #2: Earthquake Stomp (AoE around self) ---
        private void EarthquakeStomp()
        {
            Say("*Raaaargh!*");
            PlaySound(0x2F3); // heavy stomp

            // Ground‐shaking effect at llama's feet
            Effects.SendLocationParticles(
                EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration),
                0x374A, 10, 30, UniqueHue, 0, 5039, 0);

            // Damage and tile spawn
            var list = new List<Mobile>();
            foreach (Mobile m in Map.GetMobilesInRange(this.Location, 5))
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                    list.Add(m);
            }

            foreach (Mobile m in list)
            {
                DoHarmful(m);
                AOS.Damage(m, this, Utility.RandomMinMax(20, 35), 100, 0, 0, 0, 0);

                // Spawn an earthquake hazard under each target
                var quake = new EarthquakeTile();
                quake.Hue = UniqueHue;

                if (Map.CanFit(m.Location.X, m.Location.Y, m.Location.Z, 16, false, false))
                    quake.MoveToWorld(m.Location, this.Map);
            }
        }

        // --- Ability #3: Boulder Barrage (chain‐like multi‐target physical) ---
        private void BoulderBarrage(Mobile initialTarget)
        {
            if (!CanBeHarmful(initialTarget, false)) return;

            Say("*Stompa barrage!*");
            PlaySound(0x4F1); // rock toss

            var targets = new List<Mobile> { initialTarget };
            int maxBounces = 4, range = 6;

            // Find additional nearby targets
            for (int i = 0; i < maxBounces; i++)
            {
                var last = targets[targets.Count - 1];
                Mobile next = null;
                double closest = double.MaxValue;

                foreach (Mobile m in Map.GetMobilesInRange(last.Location, range))
                {
                    if (m != this && m != last && !targets.Contains(m)
                     && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(last, m)
                     && last.InLOS(m))
                    {
                        double d = last.GetDistanceToSqrt(m);
                        if (d < closest) { closest = d; next = m; }
                    }
                }
                if (next != null) targets.Add(next);
                else break;
            }

            // Fire off boulders one after another
            for (int i = 0; i < targets.Count; i++)
            {
                var src = i == 0 ? this : targets[i - 1];
                var dst = targets[i];

                Effects.SendMovingParticles(
                    new Entity(Serial.Zero, src.Location, src.Map),
                    new Entity(Serial.Zero, dst.Location, dst.Map),
                    0x36B0, 5, 0, false, false, UniqueHue, 0, 9502, 0, 0, EffectLayer.Head, 0);

                var victim = dst; // capture
                Timer.DelayCall(TimeSpan.FromSeconds(i * 0.2), () =>
                {
                    if (CanBeHarmful(victim, false))
                    {
                        DoHarmful(victim);
                        AOS.Damage(victim, this, Utility.RandomMinMax(25, 40), 100, 0, 0, 0, 0);
                    }
                });
            }
        }

        // --- Death: spawn quicksand hazards around the corpse ---
        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            Say("*...crumble...*");
            PlaySound(0x214);

            int count = Utility.RandomMinMax(3, 5);
            for (int i = 0; i < count; i++)
            {
                int xOff = Utility.RandomMinMax(-3, 3);
                int yOff = Utility.RandomMinMax(-3, 3);
                var loc = new Point3D(X + xOff, Y + yOff, Z);

                if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                    loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                var sand = new QuicksandTile();
                sand.Hue = UniqueHue;
                sand.MoveToWorld(loc, this.Map);

                Effects.SendLocationParticles(
                    EffectItem.Create(loc, this.Map, EffectItem.DefaultDuration),
                    0x376A, 8, 20, UniqueHue, 0, 5039, 0);
            }
        }

		public RockyLlama(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Reset cooldowns
            var now = DateTime.UtcNow;
            m_NextStomp   = now + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextSpit    = now + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            m_NextBarrage = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
        }
    }
}
