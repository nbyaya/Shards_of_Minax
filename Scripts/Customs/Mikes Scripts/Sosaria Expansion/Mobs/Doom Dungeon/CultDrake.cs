using System;
using System.Collections.Generic;
using System.Linq;                       // FIX: needed for .Any() (or .Count()) on IPooledEnumerable
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Spells.Fifth;     // for Poison spells
using Server.Spells.Sixth;     // for Paralyze      
using Server.Spells.Seventh;   // for Chain Lightning base & Explosion
using Server.Misc;             // for Utility
using Server.Targeting;        // for summoning placement

namespace Server.Mobiles
{
    [CorpseName("a corrupted drake corpse")]
    public class CultDrake : BaseCreature
    {
        // Ability timers
        private DateTime m_NextBreath;
        private DateTime m_NextSummon;
        private DateTime m_NextInvocation;
        private DateTime m_NextAuraPulse;

        // Tracks last position for aura ticks
        private Point3D m_LastLocation;

        // A sickly green‑black cult tint
        private const int CultHue = 1175;

        [Constructable]
        public CultDrake()
            : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name = "a Cult Drake";
            Body = 0x58C;       // same as most element drakes
            Hue  = CultHue;
            BaseSoundID = 362;

            // —— Attributes ——
            SetStr(450, 500);
            SetDex(200, 250);
            SetInt(300, 350);

            SetHits(1200, 1400);
            SetStam(200, 250);
            SetMana(400, 500);

            SetDamage(18, 24);

            // Mix of physical + necrotic/poison
            SetDamageType(ResistanceType.Physical, 30);
            SetDamageType(ResistanceType.Poison,   40);
            SetDamageType(ResistanceType.Energy,   30);

            // Resistances
            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire,     40, 50);
            SetResistance(ResistanceType.Cold,     40, 50);
            SetResistance(ResistanceType.Poison,   70, 80);
            SetResistance(ResistanceType.Energy,   60, 70);

            // Skills
            SetSkill(SkillName.EvalInt,    100.0, 115.0);
            SetSkill(SkillName.Magery,     100.0, 115.0);
            SetSkill(SkillName.MagicResist,110.0, 125.0);
            SetSkill(SkillName.Meditation,  90.0, 100.0);
            SetSkill(SkillName.Tactics,     95.0, 110.0);
            SetSkill(SkillName.Wrestling,   95.0, 110.0);

            Fame = 25000;
            Karma = -25000;
            VirtualArmor = 90;
            ControlSlots = 6;

            // Schedule first uses
            m_NextBreath     = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextSummon     = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            m_NextInvocation = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));
            m_NextAuraPulse  = DateTime.UtcNow;    // aura is continuous

            m_LastLocation = this.Location;

            // Pack some cultish loot
            PackItem(new Bone(Utility.RandomMinMax(20, 30)));
            PackItem(new BlackPearl(Utility.RandomMinMax(15, 20)));
            PackItem(new Garlic(Utility.RandomMinMax(5, 10)));
            if (Utility.RandomDouble() < 0.05)
                PackItem(new GaitOfTheHollowStag());    // rare necromancy scroll

            // Breath-level treasure map (omitted)
        }

        // —— Aura of Decay ——  
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            base.OnMovement(m, oldLocation);

            if (!Alive || this.Map == null) 
                return;

            // If it moves, pulse aura
            if (this.Location != m_LastLocation && Utility.RandomDouble() < 0.33)
            {
                foreach (Mobile friend in this.GetMobilesInRange(3))
                {
                    if (friend != this && CanBeHarmful(friend, false))
                    {
                        DoHarmful(friend);

                        if (friend is Mobile target)
                        {
                            // 20–30 poison damage
                            AOS.Damage(target, this, Utility.RandomMinMax(20, 30),
                                       0, 0, 0, 100, 0);

                            // Healing suppression message
                            target.SendMessage(0x22, "You feel your life force wane beneath the drake's aura.");
                        }
                    }
                }
            }

            m_LastLocation = this.Location;
        }

        public override void OnThink()
        {
            base.OnThink();

            if (!Alive || Combatant == null || this.Map == null || this.Map == Map.Internal)
                return;

            var now = DateTime.UtcNow;

            // —— Rot Breath (AoE Poison) ——
            if (now >= m_NextBreath && this.InRange(Combatant.Location, 8))
            {
                RotBreath();
                m_NextBreath = now + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            }
            // —— Summon Cultists ——
            else if (now >= m_NextSummon && !this.GetMobilesInRange(1).Any()) // FIX: use .Any() instead of .Count
            {
                SummonCultists();
                m_NextSummon = now + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
            }
            // —— Dark Invocation (targeted chain effect) ——
            else if (now >= m_NextInvocation && this.InRange(Combatant.Location, 12))
            {
                DarkInvocation();
                m_NextInvocation = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
        }

        // —— Rot Breath ——  
        private void RotBreath()
        {
            this.Say("*Sssskreeee...*");
            this.PlaySound(0x22);
            this.FixedParticles(0x3709, 12, 20, 5052, CultHue, 0, EffectLayer.Head);

            var targets = new List<Mobile>();
            foreach (Mobile m in GetMobilesInRange(6))
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                    targets.Add(m);
            }

            foreach (var t in targets)
            {
                DoHarmful(t);
                var tile = new PoisonTile { Hue = CultHue };
                tile.MoveToWorld(t.Location, this.Map);

                if (t is Mobile mo)
                    AOS.Damage(mo, this, Utility.RandomMinMax(30, 45),
                               0, 0, 0, 100, 0);
            }
        }

        // —— Summon Cultists ——  
        private void SummonCultists()
        {
            this.Say("*Rise, my followers!*");
            this.PlaySound(0x212);
            int count = Utility.RandomMinMax(2, 4);

            for (int i = 0; i < count; i++)
            {
                Point3D loc = new Point3D(
                    this.X + Utility.RandomMinMax(-2, 2),
                    this.Y + Utility.RandomMinMax(-2, 2),
                    this.Z);

                if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                    loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                var cultist = new Skeleton();  // replace with your NPC class if needed
                cultist.Team = this.Team;
                cultist.MoveToWorld(loc, this.Map);
                cultist.Combatant = this.Combatant;
            }
        }

        // —— Dark Invocation ——  
        private void DarkInvocation()
        {
            this.Say("*Embrace the void!*");
            this.PlaySound(0x1F6);

            var initial = Combatant as Mobile;
            if (initial == null || !CanBeHarmful(initial, false))
                return;

            var targets = new List<Mobile> { initial };
            int max = 4;
            int range = 6;

            // FIX: use Map.GetMobilesInRange for specifying a Point3D center
            for (int i = 1; i < max; i++)
            {
                var last = targets[targets.Count - 1];
                Mobile next = null;
                double best = double.MaxValue;

                foreach (Mobile m in this.Map.GetMobilesInRange(last.Location, range))
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
                var src = (i == 0 ? this : targets[i - 1]);
                var dst = targets[i];

                Effects.SendMovingParticles(
                    new Entity(Serial.Zero, src.Location, src.Map),
                    new Entity(Serial.Zero, dst.Location, dst.Map),
                    0x382A, 7, 0, false, false,
                    CultHue, 0, 9502, 1, 0,
                    (int)EffectLayer.Head   // FIX: cast EffectLayer to int
                );

                Timer.DelayCall(TimeSpan.FromSeconds(0.1 * i), () =>
                {
                    if (CanBeHarmful(dst, false))
                    {
                        DoHarmful(dst);
                        if (dst is Mobile mt)
                            AOS.Damage(mt, this, Utility.RandomMinMax(25, 35),
                                       0, 0, 0, 100, 0);
                    }
                });
            }
        }

        // —— Death Explosion ——  
        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            if (this.Map == null) 
                return;

            this.Say("*Uuuuurgh...*");
            Effects.PlaySound(this.Location, this.Map, 0x211);
            Effects.SendLocationParticles(
                EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration),
                0x3709, 15, 60, CultHue, 0, 5052, 0);

            int count = Utility.RandomMinMax(5, 8);
            for (int i = 0; i < count; i++)
            {
                int dx = Utility.RandomMinMax(-4, 4);
                int dy = Utility.RandomMinMax(-4, 4);
                var loc = new Point3D(this.X + dx, this.Y + dy, this.Z);

                if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                    loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                Type[] tiles = {
                    typeof(NecromanticFlamestrikeTile),
                    typeof(PoisonTile),
                    typeof(VortexTile)
                };
                var tType = tiles[Utility.Random(tiles.Length)];
                var tile = (Item)Activator.CreateInstance(tType);
                tile.Hue = CultHue;
                tile.MoveToWorld(loc, this.Map);
            }
        }

        // —— Standard Overrides ——  
        public override bool BleedImmune => true;
        public override int TreasureMapLevel => 6;
        public override double DispelDifficulty => 150.0;
        public override double DispelFocus     => 75.0;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.Gems, Utility.RandomMinMax(10, 15));
            AddLoot(LootPack.HighScrolls, Utility.RandomMinMax(2, 3));
            if (Utility.RandomDouble() < 0.03)
                PackItem(new FawnweaveJerkin()); // your unique artifact
        }

        // —— Serialization ——  
        public CultDrake(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // reset timers on reload
            m_NextBreath     = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            m_NextSummon     = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            m_NextInvocation = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));
        }
    }
}
