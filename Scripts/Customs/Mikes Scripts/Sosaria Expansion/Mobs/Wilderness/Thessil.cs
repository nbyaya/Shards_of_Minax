using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Targeting;
using Server.Network;
using Server.Spells;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("a thessil corpse")]
    public class Thessil : BaseCreature
    {
        private InternalAOETimer _AOETimer;
        private DateTime _NextAOE;
        private DateTime _NextRegen;

        [Constructable]
        public Thessil()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Thessil";
            Body = 52;
            BaseSoundID = 0xDB;
            Hue = 0x489;

            SetStr(500, 700);
            SetDex(300, 450);
            SetInt(100, 200);

            SetHits(1500, 2000);
            SetMana(200, 300);
            SetStam(300, 400);

            SetDamage(15, 30);
            SetDamageType(ResistanceType.Physical, 40);
            SetDamageType(ResistanceType.Poison, 60);

            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire, 30, 40);
            SetResistance(ResistanceType.Cold, 30, 40);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 30, 40);

            SetSkill(SkillName.Poisoning, 120.0, 130.0);
            SetSkill(SkillName.MagicResist, 100.0, 110.0);
            SetSkill(SkillName.Tactics, 100.0, 110.0);
            SetSkill(SkillName.Wrestling, 110.0, 120.0);
            SetSkill(SkillName.Anatomy, 80.0, 90.0);

            Fame = 15000;
            Karma = -15000;
            VirtualArmor = 50;

            Tamable = false;
            ControlSlots = 5;
            MinTameSkill = 120.0;

            // Initial cooldowns
            _NextAOE   = DateTime.UtcNow;
            _NextRegen = DateTime.UtcNow + TimeSpan.FromMinutes(1);
        }

        public Thessil(Serial serial)
            : base(serial)
        {
        }

        public override Poison PoisonImmune { get { return Poison.Greater; } }
        public override Poison HitPoison    { get { return Poison.Lethal; } }

        public override int Meat               { get { return 5; } }
        public override FoodType FavoriteFood  { get { return FoodType.Eggs | FoodType.Meat; } }
        public override int TreasureMapLevel   { get { return 5; } }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich);
            AddLoot(LootPack.FilthyRich);
            AddLoot(LootPack.Gems);
            AddLoot(LootPack.Potions);

            if (Utility.RandomDouble() < 0.01)
                PackItem(new SerpentFang());

            if (Utility.RandomDouble() < 0.005)
                PackItem(new SnakeSkinBoots());
        }

        public void SerpentEmbrace(Mobile target)
        {
            if (target == null || target.Deleted || target.Map != Map || !InRange(target, 10) || !CanBeHarmful(target) || !InLOS(target))
            {
                _AOETimer?.Stop();
                _AOETimer = null;
                return;
            }

            _AOETimer?.Stop();

            PublicOverheadMessage(MessageType.Regular, 0x3B2, false, "*The Thessil rears back, a sickening mist forming around it!*");
            FixedParticles(0x376A, 9, 32, 5005, 0x112, 0, EffectLayer.Waist);
            PlaySound(0x674);

            _AOETimer = new InternalAOETimer(this, target, TimeSpan.FromSeconds(3.0));
            _AOETimer.Start();

            _NextAOE = DateTime.UtcNow + TimeSpan.FromSeconds(15.0 + (10.0 * Utility.RandomDouble()));
        }

        private class InternalAOETimer : Timer
        {
            private Thessil m_Thessil;
            private Mobile m_Target;

            public InternalAOETimer(Thessil thessil, Mobile target, TimeSpan delay)
                : base(delay)
            {
                m_Thessil = thessil;
                m_Target  = target;
                Priority  = TimerPriority.TwentyFiveMS;
            }

            protected override void OnTick()
            {
                if (m_Thessil == null || m_Thessil.Deleted
                    || m_Target == null || m_Target.Deleted
                    || m_Target.Map != m_Thessil.Map
                    || !m_Thessil.CanBeHarmful(m_Target)
                    || !m_Thessil.InRange(m_Target, 15))
                {
                    m_Thessil._AOETimer = null;
                    return;
                }

                m_Thessil.DoHarmful(m_Target);

                Effects.PlaySound(m_Target.Location, m_Target.Map, 0x228);

                // NEW: use the 5‑arg overload with an EffectItem
                Effects.SendLocationParticles(
                    EffectItem.Create(m_Target.Location, m_Target.Map, EffectItem.DefaultDuration),
                    0x376A, 9, 20, 5042
                );

                var victims = new List<Mobile> { m_Target };

                foreach (var mob in m_Target.GetMobilesInRange(4))
                {
                    if (mob != m_Thessil && !victims.Contains(mob) && m_Thessil.CanBeHarmful(mob))
                        victims.Add(mob);
                }

                foreach (var victim in victims)
                {
                    m_Thessil.DoHarmful(victim);

                    int rawDamage = Utility.RandomMinMax(20, 40);
                    rawDamage = (int)(rawDamage * (victims.Count > 1 ? 0.75 : 1.0));

                    int physDamage   = (int)(rawDamage * 0.4 * (1.0 - (victim.PhysicalResistance / 100.0)));
                    int poisonDamage = (int)(rawDamage * 0.6 * (1.0 - (victim.PoisonResistance  / 100.0)));

                    int totalReduced = Math.Max(1, physDamage + poisonDamage);

                    AOS.Damage(victim, m_Thessil, totalReduced, 40, 0, 0, 60, 0);

                    // NEW: check the ApplyPoisonResult enum
                    if (victim.ApplyPoison(m_Thessil, Poison.Deadly) == ApplyPoisonResult.Poisoned)
                        victim.SendAsciiMessage("You are engulfed by the Thessil's poisonous mist!");
                    else
                        victim.SendAsciiMessage("You resist the worst of the Thessil's poisonous mist!");
                }

                m_Thessil._AOETimer = null;
            }
        }

        public override void OnActionCombat()
        {
            base.OnActionCombat();

            if (Combatant is Mobile target)
            {
                if (DateTime.UtcNow >= _NextAOE && InRange(target, 10) && CanBeHarmful(target) && InLOS(target))
                    SerpentEmbrace(target);

                if (DateTime.UtcNow >= _NextRegen && Hits < HitsMax * 0.8)
                    MoltingRegeneration();
            }
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (Utility.RandomDouble() < 0.25 && defender is Mobile target)
            {
                int amount   = Utility.RandomMinMax(15, 25);
                var duration = TimeSpan.FromSeconds(10);

                switch (Utility.Random(3))
                {
                    case 0:
                        // NEW: correct StatMod signature (type, name, offset, duration)
                        target.AddStatMod(new StatMod(StatType.Str, "VenomStr", -amount, duration));
                        target.SendAsciiMessage("The Thessil's venom saps your strength!");
                        break;
                    case 1:
                        target.AddStatMod(new StatMod(StatType.Dex, "VenomDex", -amount, duration));
                        target.SendAsciiMessage("The Thessil's venom numbs your reflexes!");
                        break;
                    case 2:
                        target.AddStatMod(new StatMod(StatType.Int, "VenomInt", -amount, duration));
                        target.SendAsciiMessage("The Thessil's venom clouds your mind!");
                        break;
                }

                PlaySound(0x51D);
            }
        }

        public void MoltingRegeneration()
        {
            if (Hits >= HitsMax)
                return;

            PublicOverheadMessage(MessageType.Regular, 0x3B2, false, "*The Thessil begins to shed its skin, regenerating!*");
            FixedParticles(0x373A, 10, 15, 5012, EffectLayer.Waist);
            PlaySound(0x1E3);

            int healAmount = Math.Max(1, (int)(HitsMax * 0.25));
            Heal(healAmount);

            _NextRegen = DateTime.UtcNow + TimeSpan.FromMinutes(1.5 + (1.0 * Utility.RandomDouble()));
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
        }

        public override void Delete()
        {
            _AOETimer?.Stop();
            _AOETimer = null;
            base.Delete();
        }
    }
}

namespace Server.Items
{
    public class SerpentFang : Item
    {
        [Constructable]
        public SerpentFang() : base(0xF14)
        {
            Name     = "Serpent Fang";
            Hue      = 0x489;
            LootType = LootType.Blessed;
            Weight   = 0.1;
        }

        public SerpentFang(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
