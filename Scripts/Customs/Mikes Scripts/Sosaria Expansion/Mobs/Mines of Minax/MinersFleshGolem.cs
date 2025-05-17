using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Spells.Fifth;        // For Earthquake visuals
using Server.Spells.Eighth;       // For Meteor Swarm visuals
using Server.Spells.Seventh;      // For Chain Lightning visuals
using Server.Commands;            // For centering effects

namespace Server.Mobiles
{
    [CorpseName("a miners flesh golem corpse")]
    public class MinersFleshGolem : BaseCreature
    {
        // Ability cooldown trackers
        private DateTime m_NextStompTime;
        private DateTime m_NextShardTime;
        private DateTime m_NextMagnetTime;
        private DateTime m_NextGasTime;
        private Point3D m_LastLocation;

        // Unique rusty‐ore hue
        private const int UniqueHue = 1254;

        [Constructable]
        public MinersFleshGolem()
            : base(AIType.AI_Melee, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name = "a miners flesh golem";
            Body = 304;               // Flesh Golem body
            BaseSoundID = 684;        // Flesh Golem sounds
            Hue = UniqueHue;

            // ——— Core Stats ———
            SetStr(300, 350);
            SetDex(80, 100);
            SetInt(50, 70);

            SetHits(1800, 2100);
            SetStam(150, 200);
            SetMana(100, 150);

            SetDamage(25, 35);

            // ——— Damage Types ———
            SetDamageType(ResistanceType.Physical, 80);
            SetDamageType(ResistanceType.Fire,     10);
            SetDamageType(ResistanceType.Poison,   10);

            // ——— Resistances ———
            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire,     30, 40);
            SetResistance(ResistanceType.Cold,     20, 30);
            SetResistance(ResistanceType.Poison,   80, 90);
            SetResistance(ResistanceType.Energy,   40, 50);

            // ——— Skills ———
            SetSkill(SkillName.MagicResist,  80.0,  95.0);
            SetSkill(SkillName.Tactics,     100.0, 110.0);
            SetSkill(SkillName.Wrestling,   100.0, 110.0);
            SetSkill(SkillName.ArmsLore,     60.0,  75.0);

            Fame = 18000;
            Karma = -18000;

            VirtualArmor = 75;
            ControlSlots = 5;

            // ——— Initialize cooldowns ———
            m_NextStompTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            m_NextShardTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextMagnetTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
            m_NextGasTime    = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 30));

            m_LastLocation = this.Location;

            // ——— Starter Loot ———
            PackItem(new IronIngot(Utility.RandomMinMax(20, 30)));
            PackItem(new Pickaxe());              // Themed drop
        }

        // ——— Ground Hazard: Quicksand / Toxic Gas trails as it moves ———
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            base.OnMovement(m, oldLocation);

            if (DateTime.UtcNow < DateTime.UtcNow) return; // no-op to quiet warnings

            // Drop a quicksand tile 20% of moves
            if (this.Location != m_LastLocation && Utility.RandomDouble() < 0.20)
            {
                Point3D dropLoc = m_LastLocation;
                if (Map.CanFit(dropLoc.X, dropLoc.Y, dropLoc.Z, 16, false, false))
                {
                    QuicksandTile qs = new QuicksandTile();
                    qs.Hue = 1854; // sickly brown
                    qs.MoveToWorld(dropLoc, this.Map);
                }
            }

            m_LastLocation = this.Location;
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || !Alive || Map == null || Map == Map.Internal)
                return;

            DateTime now = DateTime.UtcNow;

            // Seismic Stomp: heavy AoE, stuns briefly
            if (now >= m_NextStompTime && InRange(Combatant.Location, 6))
            {
                SeismicStomp();
                m_NextStompTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
                return; // one ability per tick
            }

            // Bursting Ore Shards: ranged scatter damage
            if (now >= m_NextShardTime && InRange(Combatant.Location, 12))
            {
                BurstingOreShards();
                m_NextShardTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(18, 24));
                return;
            }

            // Magnetic Surge: pull and slow
            if (now >= m_NextMagnetTime && InRange(Combatant.Location, 10))
            {
                MagneticSurge();
                m_NextMagnetTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(22, 28));
                return;
            }

            // Toxic Vent: cloud of poison around itself
            if (now >= m_NextGasTime && InRange(Combatant.Location, 8))
            {
                ToxicVent();
                m_NextGasTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 40));
            }
        }

        // ——— Ability #1: Seismic Stomp ———
        private void SeismicStomp()
        {
            this.Say("*ROOOAAAR!*");
            PlaySound(0x2A2); // Earthquake sound

            Effects.SendLocationParticles(
                EffectItem.Create(this.Location, this.Map, TimeSpan.FromSeconds(2)),
                0x3779, 10, 30, UniqueHue, 0, 9502, 0);

            List<Mobile> targets = new List<Mobile>();
            foreach (Mobile m in Map.GetMobilesInRange(Location, 6))
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                    targets.Add(m);
            }

            foreach (Mobile m in targets)
            {
                DoHarmful(m);
                AOS.Damage(m, this, Utility.RandomMinMax(60, 80), 100, 0, 0, 0, 0);

                if (m is Mobile target)
                {
                    target.Freeze(TimeSpan.FromSeconds(2.0));
                    target.SendMessage("The ground shudders and you lose your footing!");
                }
            }
        }

        // ——— Ability #2: Bursting Ore Shards ———
        private void BurstingOreShards()
        {
            if (!(Combatant is Mobile primary)) return;

            this.Say("*Feel the power of the deep!*");
            PlaySound(0x241); // rock smash sound

            for (int i = 0; i < 8; i++)
            {
                Timer.DelayCall(TimeSpan.FromMilliseconds(200 * i), () =>
                {
                    Point3D loc = primary.Location;
                    Effects.SendLocationParticles(
                        EffectItem.Create(loc, Map, TimeSpan.FromSeconds(1)),
                        0x36BD, 8, 10, UniqueHue, 0, 5034, 0);

                    foreach (Mobile m in Map.GetMobilesInRange(loc, 1))
                    {
                        if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                        {
                            DoHarmful(m);
                            AOS.Damage(m, this, Utility.RandomMinMax(20, 30), 100, 0, 0, 0, 0);
                        }
                    }
                });
            }
        }

        // ——— Ability #3: Magnetic Surge ———
        private void MagneticSurge()
        {
            if (!(Combatant is Mobile target)) return;

            this.Say("*Caught in my pull!*");
            PlaySound(0x1F6); // magnetic hum

            // Pull the target 2 tiles closer
            Point3D from = target.Location, to = this.Location;
            int dx = Math.Sign(to.X - from.X), dy = Math.Sign(to.Y - from.Y);
            Point3D newLoc = new Point3D(from.X + dx * 2, from.Y + dy * 2, from.Z);

            if (Map.CanFit(newLoc.X, newLoc.Y, newLoc.Z, 16, false, false))
                target.MoveToWorld(newLoc, Map);

            // Slow effect
            if (target is Mobile m)
            {
                m.SendMessage("You are slowed by a powerful magnetic force!");

            }
        }

        // ——— Ability #4: Toxic Vent ———
        private void ToxicVent()
        {
            this.Say("*Hsssssss…*");
            PlaySound(0x3E3); // poison gas hiss

            List<Mobile> inRange = new List<Mobile>();
            foreach (Mobile m in Map.GetMobilesInRange(Location, 8))
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                    inRange.Add(m);
            }

            foreach (Mobile m in inRange)
            {
                if (m is Mobile target)
                {
                    target.ApplyPoison(this, Poison.Deadly);
                    target.SendMessage("A cloud of toxic gas sears your lungs!");
                }
            }

            // Lay down a PoisonTile
            PoisonTile pt = new PoisonTile();
            pt.Hue = 1903; // sickly green
            pt.MoveToWorld(this.Location, this.Map);
        }

        // ——— Death Effect ———
        public override void OnDeath(Container c)
        {
            if (Map != null)
            {
                this.Say("*…the mine claims another…*");
                PlaySound(0x2A2);
                Effects.SendLocationParticles(
                    EffectItem.Create(Location, Map, TimeSpan.FromSeconds(2)),
                    0x36BD, 20, 30, UniqueHue, 0, 5034, 0);

                // Spawn earthquake hazard
                EarthquakeTile eq = new EarthquakeTile();
                eq.Hue = UniqueHue;
                eq.MoveToWorld(Location, Map);
            }

            base.OnDeath(c);
        }

        // ——— Standard Overrides ———
        public override bool BleedImmune   { get { return true; } }
        public override int  TreasureMapLevel { get { return 5; } }
        public override double DispelDifficulty { get { return 125.0; } }
        public override double DispelFocus      { get { return  60.0; } }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.Gems,     Utility.RandomMinMax(10, 14));
            PackItem(new IronIngot(Utility.RandomMinMax(30, 40)));
            // 5% chance for a rare mining hammer
            if (Utility.RandomDouble() < 0.05)
                PackItem(new Pickaxe());
        }

        public MinersFleshGolem(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Re‑init timers after load
            m_NextStompTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            m_NextShardTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextMagnetTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
            m_NextGasTime    = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 30));
            m_LastLocation   = this.Location;
        }
    }
}
