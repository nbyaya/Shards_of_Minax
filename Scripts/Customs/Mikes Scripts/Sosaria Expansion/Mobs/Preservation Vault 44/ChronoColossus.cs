using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a Chrono-Colossus corpse")]
    public class ChronoColossus : BaseCreature
    {
        // Timers for special abilities
        private DateTime m_NextTimeAura;
        private DateTime m_NextTemporalSpike;
        private DateTime m_NextParadoxWave;
        private Point3D m_LastLocation;

        // Unique Hue – a surreal, time‑tinted blue
        private const int UniqueHue = 1158;

        [Constructable]
        public ChronoColossus() 
            : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.3, 0.5)
        {
            Name            = "Chrono-Colossus";
            ControlSlots    = 6;
            VirtualArmor    = 90;
            Fame            = 30000;
            Karma           = -30000;

            // — Body & Sound Variants (same as StoneMonster) —
            switch ( Utility.Random(6) )
            {
                default:
                case 0: Body = 86;  BaseSoundID = 634;  break;
                case 1: Body = 722; BaseSoundID = 372;  break;
                case 2: Body = 59;  BaseSoundID = 362;  break;
                case 3: Body = 85;  BaseSoundID = 639;  break;
                case 4: Body = 310; BaseSoundID = 0x482;break;
                case 5: Body = 83;  BaseSoundID = 427;  break;
            }

            Hue = UniqueHue;

            // — Core Stats —
            SetStr(900, 1100);
            SetDex(150, 200);
            SetInt(900, 1100);

            SetHits(2000, 2300);
            SetStam(300, 400);
            SetMana(800, 1000);

            SetDamage(25, 35);

            // — Damage Types —
            SetDamageType(ResistanceType.Physical, 20);
            SetDamageType(ResistanceType.Energy, 80);
            SetDamageType(ResistanceType.Cold, 20);

            // — Resistances —
            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 40, 50);
            SetResistance(ResistanceType.Cold, 40, 50);
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 85, 95);

            // — Skills —
            SetSkill(SkillName.Magery,       120.1, 140.0);
            SetSkill(SkillName.EvalInt,      120.0, 130.0);
            SetSkill(SkillName.MagicResist,  120.0, 130.0);
            SetSkill(SkillName.Meditation,   110.0, 120.0);
            SetSkill(SkillName.Tactics,      100.0, 110.0);
            SetSkill(SkillName.Wrestling,    100.0, 110.0);

            // — Ability Cooldowns —
            m_NextTimeAura       = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            m_NextTemporalSpike  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(18, 24));
            m_NextParadoxWave    = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 36));
            m_LastLocation       = this.Location;

            // — Loot & Rewards —
            PackGold(Utility.RandomMinMax(2000, 3500));
            PackItem(new BlackPearl(Utility.RandomMinMax(10, 15)));
            PackItem(new Bloodmoss(Utility.RandomMinMax(10, 15)));
            PackItem(new SpidersSilk(Utility.RandomMinMax(10, 15)));
            // 3% chance to drop a rare time‑shard artifact
            if (Utility.RandomDouble() < 0.03)
                PackItem(new KnightsBreathDoublet()); // assume TimeShard is defined elsewhere
        }

        // — Aura: Temporal Flux — slows and drains stamina of anyone moving nearby
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (m != this && m.Map == this.Map && m.InRange(this.Location, 2) && Alive && m.Alive && CanBeHarmful(m, false))
            {
                if (m is Mobile target)
                {
                    DoHarmful(target);

                    // Drain Stamina
                    int drain = Utility.RandomMinMax(10, 20);
                    if (target.Stam >= drain)
                    {
                        target.Stam -= drain;
                        target.SendMessage(0x35, "Time itself leeches your strength!");
                        target.FixedParticles(0x375A, 10, 15, 5032, UniqueHue, 0, EffectLayer.Waist);
                        target.PlaySound(0x1F4);
                    }

                    // Minor Cold Damage
                    AOS.Damage(target, this, Utility.RandomMinMax(5, 10), 0, 0, Utility.RandomMinMax(0,10), 0, 90);
                }
            }

            base.OnMovement(m, oldLocation);
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || Map == null || Map == Map.Internal || !Alive)
                return;

            DateTime now = DateTime.UtcNow;

            // Temporal Flux Aura (periodic reminder to allow pulses even if nobody moves)
            if (now >= m_NextTimeAura)
            {
                // Simply reset for next pulse
                m_NextTimeAura = now + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            }

            // If in range, cast abilities by priority
            if (InRange(Combatant.Location, 6) && now >= m_NextTemporalSpike)
            {
                TemporalSpikeAttack();
                m_NextTemporalSpike = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 24));
            }
            else if (now >= m_NextParadoxWave && InRange(Combatant.Location, 12))
            {
                ParadoxWaveAttack();
                m_NextParadoxWave = now + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 36));
            }
        }

        // — AoE: Temporal Spike — burst of searing chronal energy + brief stun
        public void TemporalSpikeAttack()
        {
            if (Map == null) return;

            Say("*Time fractures!*");
            PlaySound(0x209);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                0x3709, 15, 60, UniqueHue, 0, 5052, 0);

            var list = new List<Mobile>();
            foreach (Mobile m in Map.GetMobilesInRange(Location, 6))
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                    list.Add(m);
            }

            foreach (Mobile m in list)
            {
                if (m is Mobile target)
                {
                    DoHarmful(target);
                    int dmg = Utility.RandomMinMax(40, 60);
                    AOS.Damage(target, this, dmg, 0, 0, 0, 0, 100);

                    // Brief stun
                    Frozen = true;
                    Timer.DelayCall(TimeSpan.FromSeconds(0.5), () => { if (Alive) Frozen = false; });
                    target.Freeze(TimeSpan.FromSeconds(1.5));
                }
            }
        }

        // — Chain: Paradox Wave — bounces a pulse of energy between nearby foes
        public void ParadoxWaveAttack()
        {
            if (!(Combatant is Mobile first) || Map == null) return;

            Say("*Paradox unfolds!*");
            PlaySound(0x20A);

            var targets = new List<Mobile> { first };
            int max = 5, rng = 5;

            for (int i = 1; i < max; i++)
            {
                Mobile prev = targets[i - 1], next = null;
                double best = -1;

                foreach (Mobile m in Map.GetMobilesInRange(prev.Location, rng))
                {
                    if (m != this && m != prev && !targets.Contains(m) 
                        && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(prev, m) 
                        && prev.InLOS(m))
                    {
                        double d = prev.GetDistanceToSqrt(m);
                        if (best < 0 || d < best) { best = d; next = m; }
                    }
                }

                if (next != null) targets.Add(next);
                else break;
            }

            for (int i = 0; i < targets.Count; i++)
            {
                var src = (i == 0 ? this : targets[i - 1]);
                var tgt = targets[i];

                Effects.SendMovingParticles(
                    new Entity(Serial.Zero, src.Location, src.Map),
                    new Entity(Serial.Zero, tgt.Location, tgt.Map),
                    0x3818, 7, 0, false, false,
                    UniqueHue, 0, 9502, 1, 0, EffectLayer.Head, 0x100);

                var captured = tgt;
                Timer.DelayCall(TimeSpan.FromSeconds(i * 0.15), () =>
                {
                    if (CanBeHarmful(captured, false))
                    {
                        DoHarmful(captured);
                        int dmg = Utility.RandomMinMax(30, 45);
                        AOS.Damage(captured, this, dmg, 0, 0, 0, 0, 100);

                        // 30% chance to slow
                        if (Utility.RandomDouble() < 0.3)
                            captured.Freeze(TimeSpan.FromSeconds(2));
                    }
                });
            }
        }

        // — On Death: Chronal Shatter — spawns temporal hazard tiles around corpse
        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            if (Map == null) return;

            Say("*Time... undone!*");
            PlaySound(0x211);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                0x3709, 12, 60, UniqueHue, 0, 5052, 0);

            int count = Utility.RandomMinMax(4, 7);
            for (int i = 0; i < count; i++)
            {
                int x = X + Utility.RandomMinMax(-4, 4);
                int y = Y + Utility.RandomMinMax(-4, 4);
                int z = Z;
                var loc = new Point3D(x, y, z);

                if (!Map.CanFit(x, y, z, 16, false, false))
                    loc.Z = Map.GetAverageZ(x, y);

                // Use QuicksandTile as a temporal slowing hazard
                var tile = new QuicksandTile();
                tile.Hue = UniqueHue;
                tile.MoveToWorld(loc, Map);

                Effects.SendLocationParticles(
                    EffectItem.Create(loc, Map, EffectItem.DefaultDuration),
                    0x376A, 8, 20, UniqueHue, 0, 5039, 0);
            }
        }

        // — Immunities & LootMap Level —
        public override bool BleedImmune    { get { return true;  } }
        public override Poison PoisonImmune { get { return Poison.Lethal; } }
        public override int TreasureMapLevel{ get { return 6; } }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.HighScrolls, Utility.RandomMinMax(1, 2));
            AddLoot(LootPack.Gems,       Utility.RandomMinMax(6, 10));
        }

        public ChronoColossus(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Re‑init timers if needed
            m_NextTimeAura      = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            m_NextTemporalSpike = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(18, 24));
            m_NextParadoxWave   = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 36));
            m_LastLocation      = this.Location;
        }
    }
}
