using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a continuant core")]
    public class Continuant : BaseCreature
    {
        // Timers for special abilities
        private DateTime m_NextFluxTime;
        private DateTime m_NextRiftTime;
        private DateTime m_NextSwarmTime;
        private DateTime m_NextDrainTime;
        private Point3D m_LastLocation;

        // Unique turquoise hue for temporal distortion
        private const int UniqueHue = 1258;

        [Constructable]
        public Continuant() : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.1, 0.2)
        {
            Name           = "a Continuant";
            Body           = 772;
            Hue            = UniqueHue;
            BaseSoundID    = 263; // matched to arcane drone

            // Core attributes
            SetStr(350, 450);
            SetDex(200, 250);
            SetInt(800, 900);

            SetHits(1200, 1500);
            SetStam(200, 250);
            SetMana(1000, 1150);

            SetDamage(18, 24);

            // Damage profile: mostly Energy
            SetDamageType(ResistanceType.Physical, 10);
            SetDamageType(ResistanceType.Energy, 90);

            // Resistances
            SetResistance(ResistanceType.Physical, 40, 50);
            SetResistance(ResistanceType.Fire,     50, 60);
            SetResistance(ResistanceType.Cold,     50, 60);
            SetResistance(ResistanceType.Poison,   30, 40);
            SetResistance(ResistanceType.Energy,   85, 95);

            // Skills
            SetSkill(SkillName.EvalInt,     120.1, 135.0);
            SetSkill(SkillName.Magery,      120.1, 135.0);
            SetSkill(SkillName.MagicResist, 120.1, 135.0);
            SetSkill(SkillName.Meditation,  110.0, 120.0);
            SetSkill(SkillName.Tactics,     90.1, 100.0);
            SetSkill(SkillName.Wrestling,   90.1, 100.0);

            Fame            = 25000;
            Karma           = -25000;
            VirtualArmor    = 80;
            ControlSlots    = 5;

            // Loot: high‑tech reagents and rare cores
            PackItem(new PowerCrystal(Utility.RandomMinMax(3, 6)));
            PackItem(new MaxxiaScroll(Utility.RandomMinMax(1, 2)));
            
            // Initialize ability cooldowns
            m_NextFluxTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextRiftTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(18, 25));
            m_NextSwarmTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
            m_NextDrainTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));

            m_LastLocation = this.Location;
        }

        // ─── Aura: Temporal Feedback (drains mana/stam on movement) ───
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            base.OnMovement(m, oldLocation);

            if (!Alive || m == null || m == this || m.Map != Map || !CanBeHarmful(m, false))
                return;

            if (m.InRange(Location, 2) && Utility.RandomDouble() < 0.15)
            {
                if (m is Mobile target)
                {
                    DoHarmful(target);

                    // Drain mana
                    int manaDrained = Utility.RandomMinMax(15, 25);
                    if (target.Mana >= manaDrained)
                    {
                        target.Mana -= manaDrained;
                        target.SendMessage("Temporal feedback disrupts your focus!");
                        target.FixedParticles(0x374A, 10, 15, 5032, EffectLayer.Head);
                        target.PlaySound(0x1F8);
                    }

                    // Drain stamina
                    int stamDrained = Utility.RandomMinMax(10, 20);
                    if (target.Stam >= stamDrained)
                    {
                        target.Stam -= stamDrained;
                        target.SendMessage("Your limbs feel sluggish!");
                    }
                }
            }
        }

        // ─── Main Think Loop ───
        public override void OnThink()
        {
            base.OnThink();

            if (!Alive || Map == null || Combatant == null)
                return;

            // Time Flux Wave: short‑range AoE slow + energy burst
            if (DateTime.UtcNow >= m_NextFluxTime && InRange(Combatant.Location, 6))
            {
                TimeFluxWave();
                m_NextFluxTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            }
            // Chrono Rift: spawns a random teleport hazard under its target
            else if (DateTime.UtcNow >= m_NextRiftTime && InRange(Combatant.Location, 12))
            {
                ChronoRift();
                m_NextRiftTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 30));
            }
            // Nano Swarm: chains nanobot strikes between up to 5 foes
            else if (DateTime.UtcNow >= m_NextSwarmTime && InRange(Combatant.Location, 10))
            {
                NanoSwarm();
                m_NextSwarmTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 40));
            }
            // Data Drain: single‑target heavy mana & hit‑point leech
            else if (DateTime.UtcNow >= m_NextDrainTime && InRange(Combatant.Location, 8))
            {
                DataDrain();
                m_NextDrainTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            }
        }

        // ─── Ability #1: Time Flux Wave ───
        private void TimeFluxWave()
        {
            Say("*Chronal energies converge!*");
            PlaySound(0x211);
            Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration), 0x376A, 12, 60, UniqueHue, 0, 5039, 0);

            var list = new List<Mobile>();
            foreach (Mobile m in Map.GetMobilesInRange(Location, 6))
            {
                if (m != this && CanBeHarmful(m, false))
                    list.Add(m);
            }

            foreach (Mobile m in list)
            {
                DoHarmful(m);
                // Energy damage
                int dmg = Utility.RandomMinMax(30, 45);
                AOS.Damage(m, this, dmg, 0, 0, 0, 0, 100);

                // Slow effect via VortexTile
                if (Map.CanFit(m.Location.X, m.Location.Y, m.Location.Z, 16, false, false))
                {
                    var tile = new VortexTile();
                    tile.Hue = UniqueHue;
                    tile.MoveToWorld(m.Location, Map);
                }
            }
        }

        // ─── Ability #2: Chrono Rift ───
        private void ChronoRift()
        {
            if (Combatant is Mobile target && CanBeHarmful(target, false))
            {
                Say("*Initiating temporal breach!*");
                PlaySound(0x22F);

                // Pre‑rift flare
                Effects.SendLocationParticles(EffectItem.Create(target.Location, Map, EffectItem.DefaultDuration), 0x3728, 8, 20, UniqueHue, 0, 5039, 0);

                Timer.DelayCall(TimeSpan.FromSeconds(0.7), () =>
                {
                    if (Map == null) return;

                    var loc = target.Location;
                    if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                        loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                    var tile = new ChaoticTeleportTile();
                    tile.Hue = UniqueHue;
                    tile.MoveToWorld(loc, Map);

                    Effects.PlaySound(loc, Map, 0x1F6);
                });
            }
        }

        // ─── Ability #3: Nano Swarm ───
        private void NanoSwarm()
        {
            if (!(Combatant is Mobile initial) || !CanBeHarmful(initial, false))
                return;

            Say("*Nanites online!*");
            PlaySound(0x20A);

            var hits = new List<Mobile> { initial };
            int max = 5, range = 6;

            for (int i = 0; i < max - 1; i++)
            {
                var last = hits[hits.Count - 1];
                Mobile next = null; double best = double.MaxValue;

                foreach (Mobile m in Map.GetMobilesInRange(last.Location, range))
                {
                    if (m != this && !hits.Contains(m) && CanBeHarmful(m, false) && last.InLOS(m))
                    {
                        double d = last.GetDistanceToSqrt(m);
                        if (d < best) { best = d; next = m; }
                    }
                }
                if (next != null) hits.Add(next);
                else break;
            }

            for (int i = 0; i < hits.Count; i++)
            {
                var src = (i == 0 ? this : hits[i - 1]);
                var tgt = hits[i];
                Effects.SendMovingParticles(
                    new Entity(Serial.Zero, src.Location, src.Map),
                    new Entity(Serial.Zero, tgt.Location, tgt.Map),
                    0x3820, 5, 0, false, false, UniqueHue, 0, 9533, 1, 0, EffectLayer.Waist, 0);

                Timer.DelayCall(TimeSpan.FromSeconds(i * 0.1), () =>
                {
                    if (CanBeHarmful(tgt, false))
                    {
                        DoHarmful(tgt);
                        int dmg = Utility.RandomMinMax(20, 35);
                        AOS.Damage(tgt, this, dmg, 0, 0, 0, 0, 100);
                    }
                });
            }
        }

        // ─── Ability #4: Data Drain ───
        private void DataDrain()
        {
            if (Combatant is Mobile target && CanBeHarmful(target, false))
            {
                Say("*Assimilating bio‑arcane data!*");
                PlaySound(0x1FB);

                // Particle chain
                target.MovingParticles(this, 0x36FA, 1, 0, false, false, 1108, 0, 9533, 1, 0, (EffectLayer)255, 0x100);

                Timer.DelayCall(TimeSpan.FromSeconds(1.0), () =>
                {
                    if (CanBeHarmful(target, false))
                    {
                        // Leech hit points
                        int leech = Utility.RandomMinMax(50, 70);
                        AOS.Damage(target, this, leech, 100, 0, 0, 0, 0);
                        Hits += leech;

                        // Leech mana
                        int mana = Utility.RandomMinMax(40, 60);
                        if (target.Mana >= mana) target.Mana -= mana;

                        target.SendMessage("Your essence fuels the Continuant!");
                        PlaySound(0x209);
                    }
                });
            }
        }

        // ─── Death Explosion ───
        public override void OnDeath(Container c)
        {
            base.OnDeath(c);
			if (Map == null) return;

            Say("*Temporal matrix... collapsing...*");
            PlaySound(0x211);
            Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration), 0x3709, 12, 60, UniqueHue, 0, 5052, 0);

            // Scatter random anomalies
            int count = Utility.RandomMinMax(3, 6);
            for (int i = 0; i < count; i++)
            {
                int x = X + Utility.RandomMinMax(-3, 3);
                int y = Y + Utility.RandomMinMax(-3, 3);
                int z = Z;
                var loc = new Point3D(x, y, z);
                
                if (!Map.CanFit(x, y, z, 16, false, false))
                    loc.Z = Map.GetAverageZ(x, y);

                Item[] choices = {
                    new ManaDrainTile(),
                    new VortexTile(),
                    new ChaoticTeleportTile()
                };
                var tile = choices[Utility.Random(choices.Length)];
                tile.Hue = UniqueHue;
                tile.MoveToWorld(loc, Map);
            }
        }

        // ─── Sound Overrides ───
        public override int GetHurtSound()   => 0x167;
        public override int GetDeathSound()  => 0xBC;
        public override int GetAttackSound() => 0x28B;

        public override bool BleedImmune   => true;
        public override int TreasureMapLevel => 6;
        public override double DispelDifficulty => 160.0;
        public override double DispelFocus     => 80.0;

        public Continuant(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Re‑init timers
            m_NextFluxTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextRiftTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(18, 25));
            m_NextSwarmTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
            m_NextDrainTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
        }
    }
}
