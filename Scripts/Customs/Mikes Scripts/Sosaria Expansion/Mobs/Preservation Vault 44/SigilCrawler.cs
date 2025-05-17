using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Spells.Seventh; // if you want to reuse Chain Lightning visuals
using Server.Items;        // for hazard tiles and reagents

namespace Server.Mobiles
{
    [CorpseName("a sigil-crawler corpse")]
    public class SigilCrawler : BaseCreature
    {
        private DateTime _nextBurstTime;
        private DateTime _nextWebTime;
        private DateTime _nextLinkTime;
        private Point3D _lastLocation;

        // Unique arcane‐sigil green
        private const int UniqueHue = 1261;

        [Constructable]
        public SigilCrawler()
            : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name = "a sigil-crawler";
            Body = 244;               // reuse rune beetle body
            BaseSoundID = 0x4E7;      // idle sound set here; overrides in methods
            Hue = UniqueHue;

            // --- Core Stats ---
            SetStr(500, 600);
            SetDex(200, 260);
            SetInt(400, 500);

            SetHits(800, 1000);
            SetStam(250, 300);
            SetMana(400, 500);

            SetDamage(20, 30);

            // --- Damage Types ---
            SetDamageType(ResistanceType.Physical, 20);
            SetDamageType(ResistanceType.Energy,   50);
            SetDamageType(ResistanceType.Poison,   30);

            // --- Resistances ---
            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire,     50, 60);
            SetResistance(ResistanceType.Cold,     50, 60);
            SetResistance(ResistanceType.Poison,   80, 90);
            SetResistance(ResistanceType.Energy,   70, 80);

            // --- Skills ---
            SetSkill(SkillName.EvalInt,     110.0, 125.0);
            SetSkill(SkillName.Magery,      110.0, 125.0);
            SetSkill(SkillName.Poisoning,   120.0, 140.0);
            SetSkill(SkillName.MagicResist, 115.0, 130.0);
            SetSkill(SkillName.Tactics,     90.0, 100.0);
            SetSkill(SkillName.Wrestling,   90.0, 100.0);

            Fame = 18000;
            Karma = -18000;

            VirtualArmor = 70;
            ControlSlots = 4;

            // Initialize cooldowns
            var now = DateTime.UtcNow;
            _nextBurstTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            _nextWebTime   = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
            _nextLinkTime  = now + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 30));

            // Initial location for trailing hazards
            _lastLocation = this.Location;

            // Starter loot
            PackItem(new BlackPearl(Utility.RandomMinMax(15, 25)));
            PackItem(new Bloodmoss(Utility.RandomMinMax(15, 25)));
            PackItem(new EyestoneTuner()); // custom reagent
        }

        // --- Aura: Sigil Corruption ---
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            base.OnMovement(m, oldLocation);

			if (m == null || m == this || m.Map != this.Map || !Alive || !m.Alive || !Utility.InRange(_lastLocation, this.Location, 2))
                return;

            if (m is Mobile target && SpellHelper.ValidIndirectTarget(this, target))
            {
                DoHarmful(target);

                // Corruptive sigil drain
                int drain = Utility.RandomMinMax(15, 25);
                if (target.Mana >= drain)
                {
                    target.Mana -= drain;
                    target.SendMessage(0x22, "Corrupted sigils leech your mana!");
                    target.FixedParticles(0x374A, 10, 15, 5032, EffectLayer.Head);
                    target.PlaySound(0x1F8);
                }

                // Minor poison tick
                target.ApplyPoison(this, Poison.Deadly);
            }
        }

        public override void OnThink()
        {
            base.OnThink();

            if (!Alive || Map == null || Map == Map.Internal || Combatant == null)
                return;

            // Leave LandmineTiles behind
            if (this.Location != _lastLocation && Utility.RandomDouble() < 0.2)
            {
                var old = _lastLocation;
                _lastLocation = this.Location;

                if (Map.CanFit(old.X, old.Y, old.Z, 16, false, false))
                {
                    var tile = new LandmineTile();
                    tile.Hue = UniqueHue;
                    tile.MoveToWorld(old, this.Map);
                }
            }
            else
            {
                _lastLocation = this.Location;
            }

            var now = DateTime.UtcNow;
            if (now >= _nextBurstTime && InRange(Combatant.Location, 8))
            {
                SigilBurstAttack();
                _nextBurstTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            }
            else if (now >= _nextWebTime && InRange(Combatant.Location, 10))
            {
                RuneWebAttack();
                _nextWebTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 30));
            }
            else if (now >= _nextLinkTime && InRange(Combatant.Location, 12))
            {
                SigilLinkAttack();
                _nextLinkTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 40));
            }
        }

        // --- AoE Sigil Burst ---
        public void SigilBurstAttack()
        {
            PlaySound(0x211);
            FixedParticles(0x3709, 12, 30, 5052, UniqueHue, 0, EffectLayer.CenterFeet);

            var list = new List<Mobile>();
            foreach (Mobile m in Map.GetMobilesInRange(Location, 6))
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                    list.Add(m);
            }

            foreach (var mob in list)
            {
                DoHarmful(mob);
                AOS.Damage(mob, this, Utility.RandomMinMax(40, 60), 0, 0, 0, 0, 100);
                mob.FixedParticles(0x3779, 10, 25, 5032, UniqueHue, 0, EffectLayer.Head);
                mob.ApplyPoison(this, Poison.Lethal);
            }
        }

        // --- Rune Web Trap around target ---
        public void RuneWebAttack()
        {
            if (!(Combatant is Mobile target) || !CanBeHarmful(target, false))
                return;

            Say("*Ancient runes ensnare!*");
            PlaySound(0x225);

            for (int dx = -2; dx <= 2; dx++)
            for (int dy = -2; dy <= 2; dy++)
            {
                var loc = new Point3D(target.X + dx, target.Y + dy, target.Z);
                if (Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                {
                    var web = new TrapWeb();
                    web.Hue = UniqueHue;
                    web.MoveToWorld(loc, Map);
                }
            }
        }

        // --- Chain of Sigils (chain damage) ---
        public void SigilLinkAttack()
        {
            if (!(Combatant is Mobile initial) || !CanBeHarmful(initial, false))
                return;

            Say("*The sigils bind!*");
            PlaySound(0x20A);

            var targets = new List<Mobile> { initial };
            var max = 4;

            for (int i = 0; i < max; i++)
            {
                var last = targets[targets.Count - 1];
                Mobile next = null; double dist = double.MaxValue;

                foreach (Mobile m in Map.GetMobilesInRange(last.Location, 6))
                {
                    if (m != this && !targets.Contains(m) && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(last, m) && last.InLOS(m))
                    {
                        var d = last.GetDistanceToSqrt(m);
                        if (d < dist) { dist = d; next = m; }
                    }
                }

                if (next == null) break;
                targets.Add(next);
            }

            for (int i = 0; i < targets.Count; i++)
            {
                var src = (i == 0) ? this : targets[i - 1];
                var tgt = targets[i];

                Effects.SendMovingParticles(
                    new Entity(Serial.Zero, src.Location, src.Map),
                    new Entity(Serial.Zero, tgt.Location, tgt.Map),
                    0x3818, 7, 0, false, false, UniqueHue, 0, 9502, 1, 0, EffectLayer.Head, 0x100);

                Timer.DelayCall(TimeSpan.FromSeconds(i * 0.1), () =>
                {
                    if (CanBeHarmful(tgt, false))
                    {
                        DoHarmful(tgt);
                        AOS.Damage(tgt, this, Utility.RandomMinMax(30, 45), 0, 0, 0, 0, 100);
                        tgt.FixedParticles(0x374A, 1, 15, 9502, EffectLayer.Waist);
                    }
                });
            }
        }

        // --- Death Explosion & Hazards ---
        public override void OnDeath(Container c)
        {
            base.OnDeath(c);
			if (Map == null) return;

            Say("*Sigils... unleashed!*");
            Effects.PlaySound(Location, Map, 0x211);
            Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration), 0x3709, 12, 60, UniqueHue, 0, 5052, 0);

            // Scatter poison tiles
            for (int i = 0; i < Utility.RandomMinMax(4, 7); i++)
            {
                var x = X + Utility.RandomMinMax(-3, 3);
                var y = Y + Utility.RandomMinMax(-3, 3);
                var z = Z;
                if (!Map.CanFit(x, y, z, 16, false, false))
                    z = Map.GetAverageZ(x, y);

                var tile = new PoisonTile();
                tile.Hue = UniqueHue;
                tile.MoveToWorld(new Point3D(x, y, z), Map);
            }
        }

        // --- Standard Overrides ---
        public override bool BleedImmune   => true;
        public override Poison PoisonImmune => Poison.Lethal;
        public override Poison HitPoison    => Poison.Deadly;
        public override int TreasureMapLevel => 6;
        public override double DispelDifficulty => 130.0;
        public override double DispelFocus     => 65.0;

        public override int GetAngerSound()  => 0x4E8;
        public override int GetIdleSound()   => 0x4E7;
        public override int GetAttackSound() => 0x4E6;
        public override int GetHurtSound()   => 0x4E9;
        public override int GetDeathSound()  => 0x4E5;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.Gems, Utility.RandomMinMax(8, 12));
            AddLoot(LootPack.HighScrolls, Utility.RandomMinMax(1, 2));

            if (Utility.RandomDouble() < 0.03)
                PackItem(new SprintersDilemma()); // unique artifact
        }

        public SigilCrawler(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt();
            // Reset cooldowns on load
            var now = DateTime.UtcNow;
            _nextBurstTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            _nextWebTime   = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
            _nextLinkTime  = now + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 30));
            _lastLocation  = Location;
        }
    }
}
