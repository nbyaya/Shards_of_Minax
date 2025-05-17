using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;      // For visual & sound effects
using Server.Spells;       // If you want to reference spell helpers or effects
using Server.Spells.Seventh; // Example for chain effects

namespace Server.Mobiles
{
    [CorpseName("a sandwurm corpse")]
    public class Sandwurm : BaseCreature
    {
        private DateTime _nextSandSpout;
        private DateTime _nextBurrow;
        private DateTime _nextQuicksand;
        private Point3D _lastLocation;

        // A warm golden‑sand hue
        private const int UniqueHue = 2590;

        [Constructable]
        public Sandwurm()
            : base(AIType.AI_Melee, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name = "a Sandwurm";
            Body = 732;
            Hue  = UniqueHue;

            // ——— Attributes ———
            SetStr(600, 700);
            SetDex(100, 150);
            SetInt(100, 150);

            SetHits(1000, 1200);
            SetStam(200, 250);

            SetDamage(20, 30);

            // ——— Damage Types ———
            SetDamageType(ResistanceType.Physical, 60);
            SetDamageType(ResistanceType.Fire,     20);
            SetDamageType(ResistanceType.Poison,   20);

            // ——— Resistances ———
            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire,     30, 40);
            SetResistance(ResistanceType.Cold,     40, 50);
            SetResistance(ResistanceType.Poison,   60, 70);
            SetResistance(ResistanceType.Energy,   30, 40);

            // ——— Skills ———
            SetSkill(SkillName.MagicResist,  80.0,  90.0);
            SetSkill(SkillName.Tactics,     100.0, 110.0);
            SetSkill(SkillName.Wrestling,   100.0, 110.0);

            Fame = 25000;
            Karma = -25000;

            VirtualArmor = 90;
            ControlSlots = 5;

            // Initialize cooldowns
            _nextSandSpout = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            _nextBurrow    = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            _nextQuicksand = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));

            _lastLocation = this.Location;

            // Starter loot
            PackItem(new SulfurousAsh(Utility.RandomMinMax(20, 30)));
            PackItem(new BlackPearl(Utility.RandomMinMax(20, 30)));
        }

        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            base.OnMovement(m, oldLocation);

            // Leave quicksand under any nearby players
            if (m != this && m.Map == this.Map && m.InRange(this.Location, 3) && m.Alive && CanBeHarmful(m, false))
            {
                if (Utility.RandomDouble() < 0.10) // 10% chance per step
                {
                    var loc = m.Location;
                    // Place a QuicksandTile at the mover's feet
                    var qs = new QuicksandTile();
                    qs.Hue = UniqueHue;
                    qs.MoveToWorld(loc, Map);
                }
            }
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || !Alive || Map == null || Map == Map.Internal)
                return;

            var now = DateTime.UtcNow;

            // Sand Spout AoE
            if (now >= _nextSandSpout && InRange(Combatant.Location, 6))
            {
                SandSpout();
                _nextSandSpout = now + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            }
            // Burrow Ambush
            else if (now >= _nextBurrow && InRange(Combatant.Location, 12))
            {
                BurrowAmbush();
                _nextBurrow = now + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
            }
            // Quicksand Trap at distance
            else if (now >= _nextQuicksand && InRange(Combatant.Location, 15))
            {
                QuicksandTrap();
                _nextQuicksand = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
        }

        private void SandSpout()
        {
            this.PlaySound(0x2A4); // Roar
            Effects.SendLocationParticles(
                EffectItem.Create(this.Location, Map, EffectItem.DefaultDuration),
                0x374A, 12, 20, UniqueHue, 0, 5023, 0);

            var targets = new List<Mobile>();
            foreach (var m in Map.GetMobilesInRange(Location, 5))
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                    targets.Add(m);
            }

            foreach (var t in targets)
            {
                DoHarmful(t);
                AOS.Damage(t, this, Utility.RandomMinMax(40, 60), 100, 0, 0, 0, 0);

                // Knockback
                var pushDir = Utility.RandomBool() ? -1 : 1;
                var newPt = new Point3D(t.X + pushDir, t.Y + pushDir, t.Z);
                if (Map.CanFit(newPt.X, newPt.Y, newPt.Z, 16, false, false))
                    t.MoveToWorld(newPt, Map);
            }
        }

        private void BurrowAmbush()
        {
            if (Combatant is Mobile target && CanBeHarmful(target, false))
            {
                this.Say("*The sands shift beneath you…*");
                this.PlaySound(0x2A5);
                Effects.SendLocationParticles(
                    EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                    0x3709, 6, 30, UniqueHue, 0, 5023, 0);

                // Disappear
                this.Hidden = true;

                Timer.DelayCall(TimeSpan.FromSeconds(1.5), () =>
                {
                    if (!Alive || Map == null) return;

                    // Appear behind target
                    var dx = target.X - X;
                    var dy = target.Y - Y;
                    var behind = new Point3D(target.X - Math.Sign(dx), target.Y - Math.Sign(dy), target.Z);

                    if (!Map.CanFit(behind.X, behind.Y, behind.Z, 16, false, false))
                        behind = target.Location;

                    this.MoveToWorld(behind, Map);
                    this.Hidden = false;

                    this.PlaySound(0x2A6);
                    Effects.SendLocationParticles(
                        EffectItem.Create(behind, Map, EffectItem.DefaultDuration),
                        0x376A, 8, 20, UniqueHue, 0, 5023, 0);

                    // Strike once immediately
                    DoHarmful(target);
                    AOS.Damage(target, this, Utility.RandomMinMax(60, 80), 100, 0, 0, 0, 0);
                });
            }
        }

        private void QuicksandTrap()
        {
            if (Combatant is Mobile target && CanBeHarmful(target, false))
            {
                this.Say("*Sink into oblivion…*");
                PlaySound(0x2A7);

                var loc = target.Location;
                var trap = new QuicksandTile();
                trap.Hue = UniqueHue;
                trap.MoveToWorld(loc, Map);

                Effects.SendLocationParticles(
                    EffectItem.Create(loc, Map, EffectItem.DefaultDuration),
                    0x3728, 10, 12, UniqueHue, 0, 5023, 0);
            }
        }

        public override void OnDeath(Container c)
        {
            if (Map != null)
            {
                this.Say("*The sands reclaim their own…*");
                this.PlaySound(0x2A8);
                Effects.SendLocationParticles(
                    EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                    0x3709, 12, 30, UniqueHue, 0, 5023, 0);

                // Spawn several quicksand patches
                for (int i = 0; i < Utility.RandomMinMax(3, 6); i++)
                {
                    int x = X + Utility.RandomMinMax(-3, 3);
                    int y = Y + Utility.RandomMinMax(-3, 3);
                    int z = Z;

                    if (!Map.CanFit(x, y, z, 16, false, false))
                        z = Map.GetAverageZ(x, y);

                    var qs = new QuicksandTile();
                    qs.Hue = UniqueHue;
                    qs.MoveToWorld(new Point3D(x, y, z), Map);
                }
            }

            base.OnDeath(c);
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.Gems, Utility.RandomMinMax(10, 15));
            AddLoot(LootPack.HighScrolls, Utility.RandomMinMax(2, 4));

            // Rare drop: Sand Core (artifact)
            if (Utility.RandomDouble() < 0.03)
                PackItem(new GildedEveningOfDawn()); // Assume SandCore is defined elsewhere
        }

        // ——— Sounds ———
        public override int GetAngerSound()  => 0x62D;
        public override int GetIdleSound()   => 0x62D;
        public override int GetAttackSound() => 0x62A;
        public override int GetHurtSound()   => 0x62C;
        public override int GetDeathSound()  => 0x62B;

        public override bool BleedImmune => true;
        public override int TreasureMapLevel => 6;
        public override double DispelDifficulty => 150.0;
        public override double DispelFocus     => 80.0;

        public Sandwurm(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Reset cooldowns after load
            _nextSandSpout = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            _nextBurrow    = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            _nextQuicksand = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));
        }
    }
}
