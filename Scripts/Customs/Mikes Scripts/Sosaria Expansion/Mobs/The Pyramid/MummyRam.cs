using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a mummified ram corpse")]
    public class MummyRam : BaseCreature
    {
        // Cooldown trackers
        private DateTime _nextChargeTime;
        private DateTime _nextSummonTime;
        private DateTime _nextDustTime;
        private DateTime _nextAuraTime;
        private Point3D _lastLocation;

        // A sand‑worn golden hue
        private const int UniqueHue = 2401;

        [Constructable]
        public MummyRam()
            : base(AIType.AI_Melee, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name = "a Mummy Ram";
            Body = 0x591;               // reuse the Ossein Ram body
            BaseSoundID = 263;          // same grave‑clatter sound
            Hue = UniqueHue;

            // — Statistics —
            SetStr(500, 650);
            SetDex(150, 200);
            SetInt(80, 120);

            SetHits(2000, 2400);
            SetStam(200, 250);
            SetMana(100, 150);

            SetDamage(20, 30);

            // — Damage Types —
            SetDamageType(ResistanceType.Physical, 60);
            SetDamageType(ResistanceType.Cold, 20);
            SetDamageType(ResistanceType.Poison, 20);

            // — Resistances —
            SetResistance(ResistanceType.Physical, 55, 65);
            SetResistance(ResistanceType.Fire, 20, 30);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 40, 50);

            // — Skills —
            SetSkill(SkillName.Wrestling, 100.1, 115.0);
            SetSkill(SkillName.Tactics, 100.1, 115.0);
            SetSkill(SkillName.MagicResist, 100.0, 120.0);
            SetSkill(SkillName.Necromancy, 90.0, 100.0);
            SetSkill(SkillName.SpiritSpeak, 80.0, 90.0);
            SetSkill(SkillName.Anatomy, 80.0, 90.0);

            Fame = 18000;
            Karma = -18000;

            VirtualArmor = 70;
            ControlSlots = 5;

            // Initialize ability cooldowns
            var now = DateTime.UtcNow;
            _nextChargeTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            _nextSummonTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            _nextDustTime   = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));


            // Basic loot
            PackItem(new Bone(Utility.RandomMinMax(10, 20)));
            PackItem(new Bandage(Utility.RandomMinMax(5, 10)));

            _lastLocation = this.Location;
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || !Alive || Map == null || Map == Map.Internal)
                return;

            var now = DateTime.UtcNow;

            // Charge Attack
            if (now >= _nextChargeTime && InRange(Combatant.Location, 8))
            {
                DoChargeAttack();
                _nextChargeTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            }
            // Summon skeletal minions
            else if (now >= _nextSummonTime && InRange(Combatant.Location, 12))
            {
                DoSummonBoneRams();
                _nextSummonTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
            // Sandstorm hazard
            else if (now >= _nextDustTime)
            {
                DoDustStorm();
                _nextDustTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
            }
        }

        // — Charge: high‑damage melee knock‑stun —
        private void DoChargeAttack()
        {
            if (Combatant is Mobile target && CanBeHarmful(target, false))
            {
                Say("*BAAAHH!*");
                PlaySound(0x2F3);
                Effects.SendMovingParticles(
                    new Entity(Serial.Zero, this.Location, this.Map),
                    new Entity(Serial.Zero, target.Location, target.Map),
                    0x3778, 7, 14, false, false, UniqueHue, 0, 9502, 1, 0, EffectLayer.Head, 0);

                DoHarmful(target);
                int damage = Utility.RandomMinMax(50, 80);
                AOS.Damage(target, this, damage, 100, 0, 0, 0, 0);

                // Paralyze briefly
                target.Paralyze(TimeSpan.FromSeconds(3));
            }
        }

        // — Summon 1–2 OsseinRam minions nearby —
        private void DoSummonBoneRams()
        {
            Say("*Rise, my children!*");
            PlaySound(0x212);

            int count = Utility.RandomMinMax(1, 2);
            for (int i = 0; i < count; i++)
            {
                var boneRam = new OsseinRam();
                boneRam.Name = "skeletal ram";
                boneRam.Hue = UniqueHue;
                boneRam.MoveToWorld(GetRandomNearbyLocation(2), this.Map);
            }
        }

        // — Create quicksand patches around opponent —
        private void DoDustStorm()
        {
            if (Combatant is Mobile target && CanBeHarmful(target, false))
            {
                Say("*The sands consume you!*");
                PlaySound(0x1F7);

                for (int i = 0; i < 3; i++)
                {
                    var loc = new Point3D(
                        target.X + Utility.RandomMinMax(-2, 2),
                        target.Y + Utility.RandomMinMax(-2, 2),
                        target.Z);

                    if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                        loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                    var qs = new QuicksandTile();
                    qs.Hue = UniqueHue;
                    qs.MoveToWorld(loc, this.Map);
                }
            }
        }

        // — Poisonous necrotic aura on nearby movement —
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            base.OnMovement(m, oldLocation);

            if (m != this && m.Map == this.Map && m.InRange(Location, 3) && Alive && m.Alive && CanBeHarmful(m, false))
            {
                if (m is Mobile target)
                {
                    DoHarmful(target);
                    int damage = Utility.RandomMinMax(10, 20);
                    AOS.Damage(target, this, damage, 0, 0, 0, 100, 0); // 100% Poison
                    target.ApplyPoison(this, Poison.Deadly);
                    target.SendMessage("You feel necrotic energy seeping into your flesh!");
                    target.FixedParticles(0x376A, 10, 15, 5032, UniqueHue, 0, EffectLayer.Head);
                    target.PlaySound(0x227);
                }
            }
        }

        // — Death: burst of Necromantic flamestrikes —
        public override void OnDeath(Container c)
        {
            if (Map != null)
            {
                Say("*Dust to dust!*");
                PlaySound(0x212);
                Effects.SendLocationParticles(
                    EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                    0x3709, 8, 30, UniqueHue, 0, 5052, 0);

                for (int i = 0; i < Utility.RandomMinMax(4, 6); i++)
                {
                    var loc = new Point3D(
                        X + Utility.RandomMinMax(-3, 3),
                        Y + Utility.RandomMinMax(-3, 3),
                        Z);

                    if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                        loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                    var flame = new NecromanticFlamestrikeTile();
                    flame.Hue = UniqueHue;
                    flame.MoveToWorld(loc, this.Map);
                }
            }

            base.OnDeath(c);
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.Gems,     Utility.RandomMinMax(5, 10));
            AddLoot(LootPack.MedScrolls, Utility.RandomMinMax(1, 3));
        }

        // Necessary overrides
        public override bool BleedImmune      { get { return true; } }
        public override int TreasureMapLevel { get { return 6;    } }
        public override double DispelDifficulty { get { return 130.0; } }
        public override double DispelFocus      { get { return 60.0;  } }

        // Helpers
        private Point3D GetRandomNearbyLocation(int range)
        {
            return new Point3D(
                X + Utility.RandomMinMax(-range, range),
                Y + Utility.RandomMinMax(-range, range),
                Z);
        }

        public MummyRam(Serial serial)
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

            // Reset cooldowns on load
            var now = DateTime.UtcNow;
            _nextChargeTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            _nextSummonTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            _nextDustTime   = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
            _nextAuraTime   = now + TimeSpan.FromSeconds(10);
        }
    }
}
