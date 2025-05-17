using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a dooms captain corpse")]
    public class DoomsCaptain : BaseCreature
    {
        // Cooldown timers
        private DateTime m_NextShoutTime;
        private DateTime m_NextShieldTime;
        private DateTime m_NextVolleyTime;
        private DateTime m_NextSummonTime;

        // Deep crimson hue
        private const int UniqueHue = 1175;

        [Constructable]
        public DoomsCaptain()
            : base(AIType.AI_Melee, FightMode.Closest, 12, 1, 0.3, 0.6)
        {
            Name = "a Dooms Captain";
            Body = 773;
            BaseSoundID = 0x75;
            Hue = UniqueHue;

            // Core stats
            SetStr(300, 350);
            SetDex(150, 200);
            SetInt(150, 200);

            SetHits(1200, 1400);
            SetStam(300, 350);
            SetMana(200, 250);

            SetDamage(20, 30);

            // Damage types
            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Poison, 50);

            // Resistances
            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire,     30, 40);
            SetResistance(ResistanceType.Cold,     40, 50);
            SetResistance(ResistanceType.Poison,   70, 80);
            SetResistance(ResistanceType.Energy,   45, 55);

            // Skills
            SetSkill(SkillName.Swords,        100.1, 110.0);
            SetSkill(SkillName.Tactics,       100.1, 110.0);
            SetSkill(SkillName.MagicResist,   100.1, 110.0);
            SetSkill(SkillName.Poisoning,      90.0, 100.0);
            SetSkill(SkillName.Wrestling,      90.1,  95.0);

            Fame = 12000;
            Karma = -12000;

            VirtualArmor = 65;
            ControlSlots = 5;

            // Staggered ability timers
            m_NextShoutTime   = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextShieldTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            m_NextVolleyTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            m_NextSummonTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));

            // Loot / equip
            PackItem(new Cutlass());
            PackItem(new Bolt(Utility.RandomMinMax(20, 30)));
            PackItem(new Bolt(Utility.RandomMinMax(20, 30)));
            PackGold(Utility.RandomMinMax(500, 700));
            PackItem(new NoxCrystal()); // rare reagent token
        }

        // Immunities & loot map
        public override bool BleedImmune    => true;
        public override Poison PoisonImmune => Poison.Lethal;
        public override int TreasureMapLevel => 5;
        public override bool AlwaysMurderer  => true;

        // Use same sounds as MeerCaptain
        public override int GetHurtSound()   => 0x14D;
        public override int GetDeathSound()  => 0x314;
        public override int GetAttackSound() => 0x75;

        public override void OnThink()
        {
            base.OnThink();

            // If shield ready and engaged, cast it
            if (Combatant is Mobile engaged && Alive && InRange(engaged.Location, 12))
            {
                if (DateTime.UtcNow >= m_NextShieldTime)
                {
                    m_NextShieldTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 45));
                    ApplyShield();
                }
            }

            // Dread Shout: heavy AoE
            if (DateTime.UtcNow >= m_NextShoutTime && Alive)
            {
                m_NextShoutTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));
                DoDreadShout();
            }

            // Poisonous Volley: rapid bolts + lethal poison
            if (DateTime.UtcNow >= m_NextVolleyTime && Alive)
            {
                m_NextVolleyTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 18));
                DoPoisonousVolley();
            }

            // Summon Skeleton Crew
            if (DateTime.UtcNow >= m_NextSummonTime && Alive)
            {
                m_NextSummonTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(40, 60));
                SummonSkeletonCrew();
            }
        }

        // --- Shield of Dread: temporary magic absorption ---
        private void ApplyShield()
        {
            MagicDamageAbsorb = Utility.RandomMinMax(20, 30);
            FixedParticles(0x375A, 10, 15, 5037, EffectLayer.Waist);
            PlaySound(0x1E9);
        }

        // --- Dread Shout: AoE damage + slow ---
        private void DoDreadShout()
        {
            Say("*Feel the terror of the deep!*");

            var list = new List<Mobile>();
            IPooledEnumerable eable = GetMobilesInRange(8);
            foreach (Mobile m in eable)
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                    list.Add(m);
            }
            eable.Free();

            foreach (Mobile m in list)
            {
                DoHarmful(m);
                AOS.Damage(m, this, Utility.RandomMinMax(30, 50), 0,0,0,0,100);

                if (m is Mobile targetMobile)
                {
                    targetMobile.Stam -= Utility.RandomMinMax(20, 30);
                    targetMobile.SendMessage("Your limbs feel like lead!");
                    targetMobile.FixedParticles(0x3779, 10, 15, 5032, UniqueHue, 0, EffectLayer.Head);
                }
            }
        }

        // --- Poisonous Volley: six staggered bolts applying lethal poison ---
        private void DoPoisonousVolley()
        {
            if (Combatant is Mobile target && CanBeHarmful(target, false))
            {
                Say("*Take this, in the name of doom!*");
                PlaySound(0x1B8);

                for (int i = 0; i < 6; i++)
                {
                    Timer.DelayCall(TimeSpan.FromSeconds(i * 0.3), () =>
                    {
                        if (target.Deleted) return;
                        DoHarmful(target);

                        Effects.SendMovingParticles(
                            new Entity(Serial.Zero, Location, Map),
                            new Entity(Serial.Zero, target.Location, target.Map),
                            0x36D4, 7, 0, false, false,
                            UniqueHue, 0, 9502, 1, 0, EffectLayer.Waist, 0x100
                        );

                        AOS.Damage(target, this, Utility.RandomMinMax(15, 25), 0,0,0,100,0);
                        target.ApplyPoison(this, Poison.Lethal);
                    });
                }
            }
        }

        // --- Summon Skeleton Crew: 2–4 adds tinted to match hue ---
        private void SummonSkeletonCrew()
        {
            Say("*Rise, my crew of the damned!*");

            for (int i = 0; i < Utility.RandomMinMax(2, 4); i++)
            {
                Point3D spawnLoc = new Point3D(
                    X + Utility.RandomMinMax(-2, 2),
                    Y + Utility.RandomMinMax(-2, 2),
                    Z
                );

                if (!Map.CanFit(spawnLoc.X, spawnLoc.Y, spawnLoc.Z, 16, false, false))
                    spawnLoc.Z = Map.GetAverageZ(spawnLoc.X, spawnLoc.Y);

                var skeleton = new SkeletonWarrior();
                skeleton.Hue = UniqueHue;
                skeleton.MoveToWorld(spawnLoc, Map);
                skeleton.Combatant = Combatant as Mobile;
            }
        }

        // --- On death: spawn quicksand hazards ---
        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            if (Map == null) return;
            Effects.PlaySound(Location, Map, 0x1FE);

            for (int i = 0; i < 5; i++)
            {
                int x = X + Utility.RandomMinMax(-3, 3);
                int y = Y + Utility.RandomMinMax(-3, 3);
                var qs = new QuicksandTile();
                qs.Hue = UniqueHue;
                qs.MoveToWorld(new Point3D(x, y, Z), Map);
            }
        }

        // --- Loot Generation ---
        public override void GenerateLoot()
        {
            AddLoot(LootPack.AosSuperBoss);
            PackItem(new ThistlesThorn()); // unique token
            if (Utility.RandomDouble() < 0.01)
                PackItem(new ScaleborneWyrmplate());
        }

        public DoomsCaptain(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Reset ability timers
            m_NextShoutTime   = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextShieldTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            m_NextVolleyTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            m_NextSummonTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
        }
    }
}
