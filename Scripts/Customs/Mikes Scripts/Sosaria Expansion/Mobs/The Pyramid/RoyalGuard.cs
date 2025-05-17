using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;      // for AOS.Damage
using Server;            // for Timer, ResistanceMod, TimerStateCallback

namespace Server.Mobiles
{
    [CorpseName("a royal guard corpse")]
    public class RoyalGuard : BaseCreature
    {
        // Timers for special abilities
        private DateTime m_NextSandstormTime;
        private DateTime m_NextCurseTime;
        private DateTime m_NextShieldTime;

        // For movement‐based aura
        private Point3D m_LastLocation;

        // Unique Hue – a regal gold
        private const int UniqueHue = 1150;

        [Constructable]
        public RoyalGuard()
            : base(AIType.AI_Melee, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name = "a royal guard";
            Body = 71;
            BaseSoundID = 594;
            Hue = UniqueHue;

            SetStr(300, 350);
            SetDex(120, 150);
            SetInt(100, 120);

            SetHits(200, 300);
            SetStam(180, 220);
            SetMana(300, 400);

            SetDamage(25, 35);

            SetDamageType(ResistanceType.Physical, 70);
            SetDamageType(ResistanceType.Fire,     15);
            SetDamageType(ResistanceType.Cold,     15);

            SetResistance(ResistanceType.Physical, 55, 65);
            SetResistance(ResistanceType.Fire,     45, 55);
            SetResistance(ResistanceType.Cold,     45, 55);
            SetResistance(ResistanceType.Poison,   35, 45);
            SetResistance(ResistanceType.Energy,   45, 55);

            SetSkill(SkillName.Wrestling,   110.0, 120.0);
            SetSkill(SkillName.Tactics,     110.0, 120.0);
            SetSkill(SkillName.MagicResist, 110.0, 120.0);
            SetSkill(SkillName.Meditation,   90.0, 100.0);

            Fame           = 30000;
            Karma          = -30000;
            VirtualArmor   = 85;
            ControlSlots   = 4;

            m_NextSandstormTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 16));
            m_NextCurseTime     = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            m_NextShieldTime    = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));

            m_LastLocation = this.Location;

            PackGold(1000, 1500);
            PackGem();
            PackGem();
        }

        // ** Aura: Stamina Drain + Minor Cold Damage when creatures come near **
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            base.OnMovement(m, oldLocation);

            if (!Alive || m == this || Map == Map.Internal)
                return;

            if (Utility.RandomDouble() < 0.25 && m.InRange(this.Location, 2) && CanBeHarmful(m, false))
            {
                DoHarmful(m);

                int drain = Utility.RandomMinMax(10, 20);
                if (m.Stam >= drain)
                {
                    m.Stam -= drain;
                    m.FixedParticles(0x374A, 10, 15, 5032, EffectLayer.Waist);
                    m.SendMessage("The royal presence saps your strength!");
                }

                AOS.Damage(m, this, Utility.RandomMinMax(5, 15), 0, 0, 50, 0, 50);
            }
        }

        public override void OnThink()
        {
            base.OnThink();

            if (!Alive || Map == null || Map == Map.Internal || Combatant == null)
                return;

            if (DateTime.UtcNow >= m_NextSandstormTime && InRange(Combatant.Location, 8))
            {
                DoSandstorm();
                m_NextSandstormTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
            else if (DateTime.UtcNow >= m_NextCurseTime && InRange(Combatant.Location, 12))
            {
                DoRoyalCurse();
                m_NextCurseTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));
            }
            else if (DateTime.UtcNow >= m_NextShieldTime)
            {
                DoDefensiveShield();
                m_NextShieldTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 40));
            }
        }

        // --- Ability #1: Sandstorm (6‐tile AoE Fire/Cold burst + freeze) ---
        private void DoSandstorm()
        {
            Say("*By royal decree, be buried in sand!*");
            PlaySound(0x213);
            FixedParticles(0x3709, 1, 30, 9502, UniqueHue, 0, EffectLayer.Head);  // ← AboveHead → Head

            var targets = new List<Mobile>();
            foreach (Mobile m in Map.GetMobilesInRange(Location, 6))
            {
                if (m != this && CanBeHarmful(m, false))
                    targets.Add(m);
            }

            foreach (var t in targets)
            {
                DoHarmful(t);

                AOS.Damage(t, this, Utility.RandomMinMax(30, 50), 0, 0, 50, 0, 50);
                t.Freeze(TimeSpan.FromSeconds(1.0));                            // ← Stun → Freeze
                t.FixedParticles(0x379A, 10, 15, 5032, UniqueHue, 0, EffectLayer.Head);
            }
        }

        // --- Ability #2: Royal Curse (single‐target resist debuff) ---
        private void DoRoyalCurse()
        {
            if (!(Combatant is Mobile target) || !CanBeHarmful(target, false))
                return;

            Say("By pharaoh's will, your defenses crumble!");
            PlaySound(0x22E);
            target.SendMessage("You feel your defenses weaken under the royal curse!");
            target.FixedParticles(0x3740, 20, 35, 5032, UniqueHue, 0, EffectLayer.Head);

            // Apply -15 to each resistance for 10s, then auto‐remove
            foreach (ResistanceType type in new[] {
                ResistanceType.Physical,
                ResistanceType.Fire,
                ResistanceType.Cold,
                ResistanceType.Poison,
                ResistanceType.Energy
            })
            {
                var mod = new ResistanceMod(type, -15);
                target.AddResistanceMod(mod);

                // Schedule removal after 10 seconds
                Timer.DelayCall(
                    TimeSpan.FromSeconds(10.0),
                    new TimerStateCallback(RemoveResistanceModCallback),
                    Tuple.Create(target, mod)
                );
            }
        }

        // --- Ability #3: Defensive Shield (self‐buff to resistances) ---
        private void DoDefensiveShield()
        {
            Say("*The crown protects its own!*");
            PlaySound(0x1F7);
            FixedParticles(0x376A, 1, 50, 9502, UniqueHue, 0, EffectLayer.Waist);

            // +30 to three skills for 8s, then auto‐remove
            var mods = new List<SkillMod> {
                new DefaultSkillMod(SkillName.MagicResist, true, 30.0),   // ← drop duration param
                new DefaultSkillMod(SkillName.Wrestling,   true, 30.0),
                new DefaultSkillMod(SkillName.Tactics,     true, 30.0)
            };

            foreach (var mod in mods)
            {
                AddSkillMod(mod);

                // Schedule removal after 8 seconds
                Timer.DelayCall(
                    TimeSpan.FromSeconds(8.0),
                    new TimerStateCallback(RemoveSkillModCallback),
                    new object[] { this, mod }
                );
            }
        }

        // --- Death Effect: Scatter Landmines around the corpse ---
        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            if (Map == null)
                return;

            Say("*Justice... eternal...*");
            PlaySound(0x213);
            FixedParticles(0x3709, 1, 30, 9502, UniqueHue, 0, EffectLayer.Head);

            int count = Utility.RandomMinMax(4, 6);
            for (int i = 0; i < count; i++)
            {
                int xOff = Utility.RandomMinMax(-3, 3), yOff = Utility.RandomMinMax(-3, 3);
                var loc = new Point3D(X + xOff, Y + yOff, Z);

                if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                    loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                var mine = new LandmineTile();
                mine.Hue = UniqueHue;
                mine.MoveToWorld(loc, Map);

                Effects.SendLocationParticles(
                    EffectItem.Create(loc, Map, EffectItem.DefaultDuration),
                    0x376A, 10, 20, UniqueHue, 0, 5039, 0
                );
            }
        }

        // --- Callbacks to clean up the temporary mods ---
        private static void RemoveResistanceModCallback(object state)
        {
            var tup = (Tuple<Mobile, ResistanceMod>)state;
            tup.Item1.RemoveResistanceMod(tup.Item2);
        }

        private static void RemoveSkillModCallback(object state)
        {
            var arr = (object[])state;
            var guard = (RoyalGuard)arr[0];
            var mod   = (SkillMod)arr[1];
            guard.RemoveSkillMod(mod);
        }

        // --- Standard Overrides & Loot ---
        public override bool BleedImmune => true;
        public override int  TreasureMapLevel => 6;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.HighScrolls, Utility.RandomMinMax(1, 2));
            AddLoot(LootPack.Gems,        Utility.RandomMinMax(5, 8));

            if (Utility.RandomDouble() < 0.03)
                PackItem(new Pulsebreaker());
        }

        public RoyalGuard(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_NextSandstormTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 16));
            m_NextCurseTime     = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            m_NextShieldTime    = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
        }
    }
}
