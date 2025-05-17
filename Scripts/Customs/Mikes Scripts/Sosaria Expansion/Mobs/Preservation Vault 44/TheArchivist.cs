using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Spells.Seventh; // for Chain Lightning visuals

namespace Server.Mobiles
{
    [CorpseName("the archivist's data husk")]
    public class TheArchivist : BaseCreature
    {
        private DateTime m_NextArchivePulse;
        private DateTime m_NextMemoryRift;
        private DateTime m_NextDataCascade;
        private Point3D m_LastLocation;

        // Deep teal‐cyan to suggest arcane data streams
        private const int UniqueHue = 1175;

        [Constructable]
        public TheArchivist() : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name           = "The Archivist";
            Body           = 78;   // same as Ancient Lich
            BaseSoundID    = 412;  // same as Ancient Lich
            Hue            = UniqueHue;

            // Statted for a high‐end boss
            SetStr(350, 400);
            SetDex(150, 200);
            SetInt(1000, 1100);

            SetHits(1200, 1400);
            SetStam(200, 250);
            SetMana(800, 900);

            SetDamage(10, 20);
            SetDamageType(ResistanceType.Physical, 10);
            SetDamageType(ResistanceType.Energy, 90);

            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 60, 70);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 40, 50);
            SetResistance(ResistanceType.Energy, 90, 100);

            SetSkill(SkillName.EvalInt,      120.1, 130.0);
            SetSkill(SkillName.Magery,       120.1, 130.0);
            SetSkill(SkillName.MagicResist,  140.2, 160.0);
            SetSkill(SkillName.Meditation,   110.1, 120.0);
            SetSkill(SkillName.Tactics,       90.1, 100.0);
            SetSkill(SkillName.Wrestling,     80.1,  90.0);
            SetSkill(SkillName.Necromancy,   120.1, 130.0);
            SetSkill(SkillName.SpiritSpeak,  120.1, 130.0);

            Fame           = 25000;
            Karma         = -25000;
            VirtualArmor   = 90;
            ControlSlots   = 6;

            // initial cooldowns
            var now = DateTime.UtcNow;
            m_NextArchivePulse = now + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextMemoryRift   = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            m_NextDataCascade  = now + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));

            m_LastLocation = this.Location;

            // Basic loot
            PackNecroReg(200, 300);
            PackItem(new BlackPearl(Utility.RandomMinMax(15, 25)));
            PackItem(new Bloodmoss(Utility.RandomMinMax(15, 25)));
        }

        public TheArchivist(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            var version = reader.ReadInt();

            // reset cooldowns on load
            var now = DateTime.UtcNow;
            m_NextArchivePulse = now + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextMemoryRift   = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            m_NextDataCascade  = now + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
        }

        public override bool BleedImmune       => true;
        public override Poison PoisonImmune    => Poison.Lethal;
        public override double DispelDifficulty => 150.0;
        public override double DispelFocus      => 75.0;
        public override int TreasureMapLevel    => 7;
        public override TribeType Tribe         => TribeType.Undead;
        public override OppositionGroup OppositionGroup => OppositionGroup.FeyAndUndead;
        public override bool Unprovokable       => true;

        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            base.OnMovement(m, oldLocation);

            // trailing data echo: leave brief VortexTile on move
            if (this.Map != null && this.Location != m_LastLocation && Utility.RandomDouble() < 0.20)
            {
                var dropLoc = m_LastLocation;
                m_LastLocation = this.Location;

                if (!Map.CanFit(dropLoc.X, dropLoc.Y, dropLoc.Z, 16, false, false))
                    dropLoc.Z = Map.GetAverageZ(dropLoc.X, dropLoc.Y);

                var echo = new VortexTile();
                echo.Hue = UniqueHue;
                echo.MoveToWorld(dropLoc, this.Map);
            }
            else
            {
                m_LastLocation = this.Location;
            }
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || !Alive || Map == null || Map == Map.Internal)
                return;

            var now = DateTime.UtcNow;
            var target = Combatant as Mobile;

            if (target != null && now >= m_NextArchivePulse && InRange(target, 8))
            {
                ArchivePulse();
                m_NextArchivePulse = now + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            }
            else if (target != null && now >= m_NextMemoryRift && InRange(target, 12))
            {
                MemoryRift();
                m_NextMemoryRift = now + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 40));
            }
            else if (target != null && now >= m_NextDataCascade && InRange(target, 10))
            {
                DataCascade();
                m_NextDataCascade = now + TimeSpan.FromSeconds(Utility.RandomMinMax(35, 45));
            }
        }

        // AoE that blurs vision and drains mana
        private void ArchivePulse()
        {
            PlaySound(0x212);
            FixedParticles(0x374A, 12, 20, 5032, UniqueHue, 0, EffectLayer.Waist);
            var list = new List<Mobile>();
            foreach (Mobile m in Map.GetMobilesInRange(Location, 6))
            {
                if (m != this && CanBeHarmful(m, false) && m is Mobile targetMobile)
                    list.Add(targetMobile);
            }

            foreach (var m in list)
            {
                DoHarmful(m);
                // Blind for a short time
                m.SendMessage(0x22, "Your vision blurs as data fragments swirl around you!");
                m.FixedParticles(0x3779, 10, 15, 5032, UniqueHue, 0, EffectLayer.Head);

                // Mana drain
                int drain = Utility.RandomMinMax(30, 50);
                if (m.Mana >= drain)
                    m.Mana -= drain;
            }
        }

        // Creates a hazardous tile at the target's location
        private void MemoryRift()
        {
            if (!(Combatant is Mobile target))
                return;

            Say("*Fragments of forgotten memory!*");
            PlaySound(0x22F);
            var loc = target.Location;

            // brief warning
            Effects.SendLocationParticles(
                EffectItem.Create(loc, Map, EffectItem.DefaultDuration),
                0x3728, 8, 8, UniqueHue, 0, 5039, 0);

            Timer.DelayCall(TimeSpan.FromSeconds(0.7), () =>
            {
                if (Map == null) return;
                var rift = new NecromanticFlamestrikeTile();
                rift.Hue = UniqueHue;
                rift.MoveToWorld(loc, Map);
                Effects.PlaySound(loc, Map, 0x208);
            });
        }

        // Chain‐style arcane bolts arc between nearby enemies
        private void DataCascade()
        {
            if (!(Combatant is Mobile first)) return;
            Say("*Synaptic overload!*");
            PlaySound(0x20A);

            var targets = new List<Mobile> { first };
            int max = 6, range = 6;

            for (int i = 0; i < max - 1; i++)
            {
                var last = targets[targets.Count - 1];
                Mobile next = null;
                double best = double.MaxValue;

                foreach (Mobile m in Map.GetMobilesInRange(last.Location, range))
                {
                    if (m != this && !targets.Contains(m) && CanBeHarmful(m, false) && last.InLOS(m))
                    {
                        double d = last.GetDistanceToSqrt(m);
                        if (d < best)
                        {
                            best = d;
                            next = m;
                        }
                    }
                }
                if (next == null) break;
                targets.Add(next);
            }

            for (int i = 0; i < targets.Count; i++)
            {
                var src = i == 0 ? this : targets[i - 1];
                var dst = targets[i];

                Effects.SendMovingParticles(
                    new Entity(Serial.Zero, src.Location, src.Map),
                    new Entity(Serial.Zero, dst.Location, dst.Map),
                    0x3818, 7, 0, false, false, UniqueHue, 0, 9502, 1, 0, EffectLayer.Head, 0x100);

                int delay = i * 150;
                Timer.DelayCall(TimeSpan.FromMilliseconds(delay), () =>
                {
                    if (CanBeHarmful(dst, false))
                    {
                        DoHarmful(dst);
                        AOS.Damage(dst, this, Utility.RandomMinMax(30, 45), 0, 0, 0, 0, 100);
                        dst.FixedParticles(0x374A, 8, 12, 9502, UniqueHue, 0, EffectLayer.Waist);
                    }
                });
            }
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);
			
			if (Map == null) return;

            Say("*All archives... lost...*");
            PlaySound(0x211);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                0x3709, 12, 60, UniqueHue, 0, 5052, 0);

            // Scatter data shards
            for (int i = 0; i < Utility.RandomMinMax(5, 8); i++)
            {
                var x = X + Utility.RandomMinMax(-3, 3);
                var y = Y + Utility.RandomMinMax(-3, 3);
                var z = Z;
                var loc = new Point3D(x, y, z);
                if (!Map.CanFit(x, y, z, 16, false, false))
                    loc.Z = Map.GetAverageZ(x, y);

                var shard = new ManaDrainTile();
                shard.Hue = UniqueHue;
                shard.MoveToWorld(loc, Map);
                Effects.SendLocationParticles(
                    EffectItem.Create(loc, Map, EffectItem.DefaultDuration),
                    0x376A, 8, 20, UniqueHue, 0, 5039, 0);
            }
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.HighScrolls, Utility.RandomMinMax(1, 2));
            AddLoot(LootPack.Gems, Utility.RandomMinMax(10, 15));

            // 3% chance for the Archivist's Master Tome
            if (Utility.RandomDouble() < 0.03)
                PackItem(new Noiseblight()); // Placeholder for a unique tome
        }
    }
}
