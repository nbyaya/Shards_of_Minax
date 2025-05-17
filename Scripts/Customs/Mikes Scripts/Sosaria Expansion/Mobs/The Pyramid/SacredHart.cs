using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Spells.Seventh; // for potential spell visuals

namespace Server.Mobiles
{
    [CorpseName("a sacred hart corpse")]
    [TypeAlias("Server.Mobiles.SacredHart")]
    public class SacredHart : BaseCreature
    {
        private DateTime m_NextSandBurst;
        private DateTime m_NextCurse;
        private DateTime m_NextSummon;
        private DateTime m_NextCharge;
        private Point3D m_LastLocation;

        // A golden‑bone tone for the Pyramid’s undead guardian
        private const int UniqueHue = 2006;

        [Constructable]
        public SacredHart()
            : base(AIType.AI_Melee, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name = "a Sacred Hart";
            Body = 0xEA;
            Hue = UniqueHue;
            BaseSoundID = 0x82;

            // Stats
            SetStr(600, 700);
            SetDex(180, 220);
            SetInt(250, 300);

            SetHits(2000, 2500);
            SetStam(200, 250);
            SetMana(400, 500);

            SetDamage(25, 35);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire,     25);
            SetDamageType(ResistanceType.Energy,   25);

            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire,     70, 80);
            SetResistance(ResistanceType.Cold,     40, 50);
            SetResistance(ResistanceType.Poison,   50, 60);
            SetResistance(ResistanceType.Energy,   60, 70);

            SetSkill(SkillName.EvalInt,     100.0, 110.0);
            SetSkill(SkillName.Magery,      100.0, 110.0);
            SetSkill(SkillName.MagicResist, 120.0, 130.0);
            SetSkill(SkillName.Meditation,   90.0, 100.0);
            SetSkill(SkillName.Tactics,     100.0, 110.0);
            SetSkill(SkillName.Wrestling,   100.0, 110.0);

            Fame = 24000;
            Karma = -24000;

            VirtualArmor = 100;
            ControlSlots = 6;

            // Schedule first casts
            var now = DateTime.UtcNow;
            m_NextSandBurst = now + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextCurse     = now + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            m_NextSummon    = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            m_NextCharge    = now + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));

            // Loot setup
            PackItem(new SulfurousAsh(   Utility.RandomMinMax(15, 20)));
            PackItem(new BlackPearl(     Utility.RandomMinMax(15, 20)));
            PackItem(new Nightshade(     Utility.RandomMinMax(15, 20)));
            PackItem(new SpidersSilk(    Utility.RandomMinMax(15, 20)));

            m_LastLocation = this.Location;
        }

        public override int Meat              => 10;
        public override int Hides             => 20;
        public override FoodType FavoriteFood => FoodType.FruitsAndVegies | FoodType.GrainsAndHay;

        public override int GetAttackSound() => 0x82;
        public override int GetHurtSound()   => 0x83;
        public override int GetDeathSound()  => 0x84;

        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            base.OnMovement(m, oldLocation);

            if (Alive && m != this && m.Map == this.Map && m.InRange(this.Location, 2) && CanBeHarmful(m, false))
            {
                if (Map.CanFit(oldLocation.X, oldLocation.Y, oldLocation.Z, 16, false, false))
                {
                    var tile = new PoisonTile { Hue = UniqueHue };
                    tile.MoveToWorld(oldLocation, this.Map);
                }
            }
        }

        public override void OnThink()
        {
            base.OnThink();

            // Leave behind bones if it moves
            if (this.Location != m_LastLocation && Utility.RandomDouble() < 0.20)
            {
                m_LastLocation = this.Location;
                if (Map.CanFit(this.X, this.Y, this.Z, 16, false, false))
                {
                    var bone = new VortexTile { Hue = UniqueHue };
                    bone.MoveToWorld(this.Location, this.Map);
                }
            }
            else
            {
                m_LastLocation = this.Location;
            }

            if (Combatant == null || !Alive || Map == null || Map == Map.Internal)
                return;

            var now = DateTime.UtcNow;

            // Charge attack: leap and knockback
            if (now >= m_NextCharge && InRange(Combatant.Location, 8) && Combatant is Mobile chargeTarget && CanBeHarmful(chargeTarget, false))
            {
                ChargeAt(chargeTarget);
                m_NextCharge = now + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
                return;
            }

            // Sand Burst AoE
            if (now >= m_NextSandBurst && InRange(Combatant.Location, 12))
            {
                SandBurst();
                m_NextSandBurst = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
                return;
            }

            // Curse of the Fallen
            if (now >= m_NextCurse)
            {
                CurseAttack();
                m_NextCurse = now + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
                return;
            }

            // Summon skeletal stags
            if (now >= m_NextSummon)
            {
                SummonSkeletons();
                m_NextSummon = now + TimeSpan.FromSeconds(Utility.RandomMinMax(40, 60));
            }
        }

        // ——— Ability Implementations ———

        private void ChargeAt(Mobile target)
        {
            DoHarmful(target);
            PlaySound(0x23D);
            this.Say("*Resist not the Sacred charge!*");

            // rush effect (9‑arg overload)
            Effects.SendMovingEffect(
                this,
                target,
                0x3B1F,  // rushing wind graphic
                7,       // speed
                0,       // duration
                false,   // fixed direction
                false,   // explode on arrival
                UniqueHue,
                0        // renderMode
            );

            // teleport & damage
            this.Location = target.Location;
            target.Stam = Math.Max(0, target.Stam - Utility.RandomMinMax(20, 40));
            AOS.Damage(target, this, Utility.RandomMinMax(50, 70), 100, 0, 0, 0, 0);
            target.SendMessage(0x22, "You are bowled over by the Sacred Hart's charge!");
        }

        private void SandBurst()
        {
            PlaySound(0x29);
            Effects.SendLocationParticles(
                EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration),
                0x376A, 10, 60, UniqueHue, 0, 5039, 0
            );

            var victims = new List<Mobile>();
            foreach (var m in this.GetMobilesInRange(6))
            {
                if (m != this && CanBeHarmful(m, false) && m is Mobile mv)
                    victims.Add(mv);
            }

            foreach (var mv in victims)
            {
                DoHarmful(mv);
                AOS.Damage(mv, this, Utility.RandomMinMax(40, 60), 50, 50, 0, 0, 0);
                mv.SendMessage(0x35, "You are engulfed in razor‑sharp sand!");
                mv.FixedParticles(0x371A, 10, 30, 0x834, EffectLayer.Waist);

                var qs = new QuicksandTile { Hue = UniqueHue };
                qs.MoveToWorld(mv.Location, this.Map);
            }
        }

        private void CurseAttack()
        {
            if (!(Combatant is Mobile target))
                return;

            DoHarmful(target);
            this.Say("*Feel the weight of eternity!*");
            target.SendMessage(0x22, "A chilling curse seeps into your bones...");
            target.PlaySound(0x482);

            // Apply -30% to all resistances
            var mods = new List<ResistanceMod>();
            for (ResistanceType rt = ResistanceType.Physical; rt <= ResistanceType.Energy; rt++)
            {
                var mod = new ResistanceMod(rt, -30);
                mods.Add(mod);
                target.AddResistanceMod(mod);
            }

            // Restore after 10 seconds
            Timer.DelayCall(TimeSpan.FromSeconds(10.0), () =>
            {
                if (target.Alive)
                {
                    foreach (var mod in mods)
                        target.RemoveResistanceMod(mod);

                    target.SendMessage(0x42, "You feel your vitality return.");
                }
            });
        }

        private void SummonSkeletons()
        {
            this.Say("*Arise, ancient guardians!*");
            PlaySound(0x208);

            for (int i = 0; i < 2; i++)
            {
                var sk = new Skeleton
                {
                    Hue         = UniqueHue,
                    Team        = this.Team,
                    ControlSlots= 0
                };

                // random adjacent spot
                int x = this.X + Utility.RandomMinMax(-1, 1);
                int y = this.Y + Utility.RandomMinMax(-1, 1);
                int z = this.Z;
                if (!Map.CanFit(x, y, z, 16, false, false))
                {
                    x = this.X;
                    y = this.Y;
                }

                sk.MoveToWorld(new Point3D(x, y, z), this.Map);
            }
        }

        public override void OnDeath(Container c)
        {
            if (Map != null)
            {
                this.Say("*Eternal rest... denied!*");
                PlaySound(0x211);
                Effects.SendLocationParticles(
                    EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration),
                    0x3709, 10, 60, UniqueHue, 0, 5052, 0
                );

                for (int i = 0; i < 5; i++)
                {
                    var loc = new Point3D(
                        X + Utility.RandomMinMax(-3, 3),
                        Y + Utility.RandomMinMax(-3, 3),
                        Z
                    );

                    if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                        loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                    var tile = new NecromanticFlamestrikeTile { Hue = UniqueHue };
                    tile.MoveToWorld(loc, this.Map);
                }
            }

            base.OnDeath(c);
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Gems,        Utility.RandomMinMax(10, 15));
            AddLoot(LootPack.UltraRich,   2);
            AddLoot(LootPack.HighScrolls, Utility.RandomMinMax(2, 4));
            AddLoot(LootPack.Rich);
        }

        public override bool BleedImmune       => true;
        public override Poison PoisonImmune    => Poison.Lethal;
        public override int TreasureMapLevel   => 6;
        public override double DispelDifficulty=> 150.0;
        public override double DispelFocus     => 75.0;

        public SacredHart(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Re‑schedule abilities
            var now = DateTime.UtcNow;
            m_NextSandBurst = now + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextCurse     = now + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            m_NextSummon    = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            m_NextCharge    = now + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
        }
    }
}
