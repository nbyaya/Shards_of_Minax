using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Spells.Fifth;    // for Teleport
using Server.Spells.Seventh;  // for Chain Lightning effects

namespace Server.Mobiles
{
    [CorpseName("an Anachronaut corpse")]
    public class Anachronaut : BaseCreature
    {
        private DateTime m_NextSurge;
        private DateTime m_NextShift;
        private DateTime m_NextBarrage;
        private Point3D m_LastLocation;

        private const int UniqueHue = 1266; // Neon‐cyan temporal glow

        [Constructable]
        public Anachronaut()
            : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name = "an Anachronaut";
            Body = 400;               // Ethereal/construct‐style body
            BaseSoundID = 0x22F;      // Sci‐fi warp sound
            Hue = UniqueHue;

            // ——— Stats ———
            SetStr(350, 450);
            SetDex(200, 250);
            SetInt(600, 700);

            SetHits(1300, 1600);
            SetStam(200, 250);
            SetMana(800, 950);

            SetDamage(18, 24);

            // ——— Damage Types ———
            SetDamageType(ResistanceType.Physical, 20);
            SetDamageType(ResistanceType.Energy, 80);

            // ——— Resistances ———
            SetResistance(ResistanceType.Physical, 45, 55);
            SetResistance(ResistanceType.Fire, 60, 70);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 35, 45);
            SetResistance(ResistanceType.Energy, 90, 95);

            // ——— Skills ———
            SetSkill(SkillName.EvalInt,    115.0, 130.0);
            SetSkill(SkillName.Magery,     115.0, 130.0);
            SetSkill(SkillName.MagicResist,120.0, 135.0);
            SetSkill(SkillName.Meditation, 105.0, 115.0);
            SetSkill(SkillName.Tactics,     95.0, 105.0);
            SetSkill(SkillName.Wrestling,   95.0, 105.0);

            Fame = 25000;
            Karma = -25000;

            VirtualArmor = 90;
            ControlSlots = 5;

            // ——— Initialize Timers ———
            m_NextSurge    = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextShift    = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(18, 24));
            m_NextBarrage  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(22, 30));
            m_LastLocation = this.Location;

            // ——— Loot (tech components + reagents) ———
            PackItem(new PatchbornPandemonium());
            PackItem(new VoidEssence());
            PackItem(new BlackPearl(Utility.RandomMinMax(12, 18)));
            PackItem(new SulfurousAsh(Utility.RandomMinMax(12, 18)));
        }

        // ——— Aura: Temporal Distortion on Movement ———
        public override void OnMovement(Mobile m, Point3D oldLoc)
        {
            base.OnMovement(m, oldLoc);

            if (m != this && m.Map == this.Map && Alive && m.InRange(this, 2) && CanBeHarmful(m, false))
            {
                if (m is Mobile target && SpellHelper.ValidIndirectTarget(this, target))
                {
                    DoHarmful(target);

                    // Slow‐stam drain
                    int drain = Utility.RandomMinMax(5, 12);
                    if (target.Stam >= drain)
                    {
                        target.Stam -= drain;
                        target.SendMessage(0x22, "Your muscles ache as time itself resists you!");
                        target.FixedParticles(0x3779, 10, 15, 5032, UniqueHue, 0, EffectLayer.Head);
                        target.PlaySound(0x208);
                    }

                    AOS.Damage(target, this, Utility.RandomMinMax(4, 8), 0, 0, 0, 0, 100);
                }
            }
        }

        public override void OnThink()
        {
            base.OnThink();

            // ——— Leave a short‐lived temporal rift behind ———
            if (this.Location != m_LastLocation && Utility.RandomDouble() < 0.20)
            {
                var old = m_LastLocation;
                m_LastLocation = this.Location;

                Point3D spawn = old;
                int z = Map.GetAverageZ(spawn.X, spawn.Y);
                if (!Map.CanFit(spawn.X, spawn.Y, spawn.Z, 16, false, false))
                    spawn = new Point3D(spawn.X, spawn.Y, z);

                var tile = new VortexTile();
                tile.Hue = UniqueHue;
                tile.MoveToWorld(spawn, this.Map);
            }
            else
            {
                m_LastLocation = this.Location;
            }

            if (Combatant == null || !Alive || Map == null || Map == Map.Internal)
                return;

            // ——— Abilities by cooldown & range ———
            if (DateTime.UtcNow >= m_NextBarrage && InRange(Combatant.Location, 12))
            {
                ChronoBoltBarrage();
                m_NextBarrage = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 40));
            }
            else if (DateTime.UtcNow >= m_NextShift && InRange(Combatant.Location, 14))
            {
                TemporalShift();
                m_NextShift = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
            else if (DateTime.UtcNow >= m_NextSurge && InRange(Combatant.Location, 8))
            {
                TemporalSurge();
                m_NextSurge = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(14, 22));
            }
        }

        // ——— AoE Stam‐&‐Energy Burst + Slow ———
        public void TemporalSurge()
        {
            PlaySound(0x211);
            FixedParticles(0x3709, 12, 30, 5052, UniqueHue, 0, EffectLayer.Waist);

            var list = new List<Mobile>();
            foreach (var m in Map.GetMobilesInRange(Location, 6))
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                    list.Add(m);
            }

            foreach (var mv in list)
            {
                DoHarmful(mv);
                AOS.Damage(mv, this, Utility.RandomMinMax(30, 50), 0, 0, 0, 0, 100);

                if (mv is Mobile target && Utility.RandomDouble() < 0.5)
                {
                    int drain = Utility.RandomMinMax(20, 40);
                    if (target.Stam >= drain)
                    {
                        target.Stam -= drain;
                        target.SendMessage(0x22, "Time warps — your stamina falters!");
                        target.FixedParticles(0x374A, 10, 15, 5032, UniqueHue, 0, EffectLayer.Head);
                        target.PlaySound(0x1F8);
                    }
                }
            }
        }

        // ——— Quick teleport around the target ———
        public void TemporalShift()
        {
            if (!(Combatant is Mobile target) || !CanBeHarmful(target, false))
                return;

            Say("*Temporal flux!*");
            PlaySound(0x22F);

            // flash effect on self
            FixedParticles(0x376A, 8, 20, 5032, UniqueHue, 0, EffectLayer.Waist);

            // teleport to a random adjacent spot around the target
            var offX = Utility.RandomList(-1, 1);
            var offY = Utility.RandomList(-1, 1);
            var dest = new Point3D(target.X + offX, target.Y + offY, target.Z);
            this.MoveToWorld(dest, this.Map);

        }

        // ——— Chain‐bounce “chrono bolts” ———
        public void ChronoBoltBarrage()
        {
            if (!(Combatant is Mobile initial) || !CanBeHarmful(initial, false))
                return;

            Say("*Echoes of time!*");
            PlaySound(0x20A);

            var targets = new List<Mobile> { initial };
            int max = 5, range = 6;

            for (int i = 1; i < max; i++)
            {
                Mobile last = targets[i - 1], next = null;
                double best = double.MaxValue;

                foreach (var m in Map.GetMobilesInRange(last.Location, range))
                {
                    if (m != this && m != last && !targets.Contains(m)
                     && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(last, m)
                     && last.InLOS(m))
                    {
                        var dist = last.GetDistanceToSqrt(m);
                        if (dist < best)
                        {
                            best = dist;
                            next = m;
                        }
                    }
                }
                if (next == null) break;
                targets.Add(next);
            }

            for (int i = 0; i < targets.Count; i++)
            {
                var src = (i == 0 ? this : targets[i - 1]);
                var tgt = targets[i];

                Effects.SendMovingParticles(
                    new Entity(Serial.Zero, src.Location, src.Map),
                    new Entity(Serial.Zero, tgt.Location, tgt.Map),
                    0x3818, 7, 0, false, false, UniqueHue, 0, 9502, 1, 0, EffectLayer.Head, 0);

                // delayed damage
                var dmgTarget = tgt;
                Timer.DelayCall(TimeSpan.FromSeconds(0.1 * i), () =>
                {
                    if (CanBeHarmful(dmgTarget, false))
                    {
                        DoHarmful(dmgTarget);
                        AOS.Damage(dmgTarget, this, Utility.RandomMinMax(20, 35), 0, 0, 0, 0, 100);
                        dmgTarget.FixedParticles(0x374A, 1, 12, 9502, EffectLayer.Waist);
                    }
                });
            }
        }

        public override void OnDeath(Container c)
        {
            if (Map != null)
            {
                Say("*Time fractures!*");
                Effects.PlaySound(Location, Map, 0x211);
                Effects.SendLocationParticles(
                    EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                    0x3709, 12, 60, UniqueHue, 0, 5052, 0);

                // scatter temporal hazards
                int count = Utility.RandomMinMax(3, 6);
                for (int i = 0; i < count; i++)
                {
                    int x = X + Utility.RandomMinMax(-4, 4);
                    int y = Y + Utility.RandomMinMax(-4, 4);
                    int z = Z;
                    var loc = new Point3D(x, y, z);
                    if (!Map.CanFit(x, y, z, 16, false, false))
                        loc.Z = Map.GetAverageZ(x, y);

                    var tile = new QuicksandTile(); // represents a time‐slow hazard
                    tile.Hue = UniqueHue;
                    tile.MoveToWorld(loc, Map);
                    Effects.SendLocationParticles(
                        EffectItem.Create(loc, Map, EffectItem.DefaultDuration),
                        0x376A, 8, 20, UniqueHue, 0, 5039, 0);
                }
            }

            base.OnDeath(c);
        }

        // ——— Loot & Properties ———
        public override bool BleedImmune   => true;
        public override int TreasureMapLevel => 6;
        public override double DispelDifficulty => 145.0;
        public override double DispelFocus     => 75.0;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.Gems,       Utility.RandomMinMax(6, 10));
            AddLoot(LootPack.MedScrolls, Utility.RandomMinMax(1, 2));
        }

        public Anachronaut(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // re‐init timers
            m_NextSurge   = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextShift   = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(18, 24));
            m_NextBarrage = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(22, 30));
            m_LastLocation = this.Location;
        }
    }
}
