using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Spells.Necromancy;

namespace Server.Mobiles
{
    [CorpseName("a nachtbram corpse")]
    public class Nachtbram : BaseCreature
    {
        private DateTime _NextShadowBurst;
        private readonly TimeSpan ShadowBurstCooldown = TimeSpan.FromSeconds(15.0);
        private readonly TimeSpan ShadowBurstWindup = TimeSpan.FromSeconds(2.0);
        private const int ShadowBurstRange = 6;

        private DateTime _NextMeleeSpecial;
        private readonly TimeSpan MeleeSpecialCooldown = TimeSpan.FromSeconds(8.0);

        [Constructable]
        public Nachtbram()
            : base(AIType.AI_NecroMage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a Nachtbram";
            Body = 78;
            BaseSoundID = 0x3E9;
            Hue = 1801;

            SetStr(250, 300);
            SetDex(120, 150);
            SetInt(350, 400);
            SetHits(500, 600);
            SetDamage(30, 40);

            SetDamageType(ResistanceType.Physical, 20);
            SetDamageType(ResistanceType.Cold, 40);
            SetDamageType(ResistanceType.Energy, 40);
            SetDamageType(ResistanceType.Poison, 0);

            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire, 40, 50);
            SetResistance(ResistanceType.Cold, 70, 80);
            SetResistance(ResistanceType.Poison, 40, 50);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.EvalInt, 110.0, 120.0);
            SetSkill(SkillName.Magery, 110.0, 120.0);
            SetSkill(SkillName.Meditation, 100.0, 110.0);
            SetSkill(SkillName.MagicResist, 100.0, 110.0);
            SetSkill(SkillName.Tactics, 90.0, 100.0);
            SetSkill(SkillName.Wrestling, 90.0, 100.0);
            SetSkill(SkillName.Necromancy, 110.0, 120.0);
            SetSkill(SkillName.SpiritSpeak, 100.0, 110.0);
            SetSkill(SkillName.DetectHidden, 80.0, 90.0);

            Fame = 10000;
            Karma = -10000;
            VirtualArmor = 65;
        }

        public Nachtbram(Serial serial) : base(serial) { }

        public override void OnActionCombat()
        {
            base.OnActionCombat();

            if (Combatant is Mobile target && DateTime.UtcNow >= _NextShadowBurst
                && !target.Deleted && target.Map == Map
                && InRange(target, ShadowBurstRange + 2)
                && CanBeHarmful(target) && InLOS(target))
            {
                PerformShadowBurst(target);
            }
        }

        private void PerformShadowBurst(Mobile target)
        {
            Animate(11, 5, 1, true, false, 0);
            PlaySound(0x210);

            Effects.SendLocationParticles(
                EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                0x3728, 10, 10, 1801, 0, 9502, 0
            );

            PublicOverheadMessage(MessageType.Regular, 1801, false,
                "*The Nachtbram gathers dark energy!*");

            _NextShadowBurst = DateTime.UtcNow + ShadowBurstCooldown;

            Timer.DelayCall(ShadowBurstWindup, () =>
            {
                if (Deleted || !Alive || Combatant == null || Combatant.Deleted || Combatant.Map != Map)
                    return;

                var ents = Map.GetMobilesInRange(Location, ShadowBurstRange);
                var targets = new List<Mobile>();

                foreach (Mobile m in ents)
                    if (m != this && CanBeHarmful(m, false))
                        targets.Add(m);

                ents.Free();

                if (targets.Count == 0) return;

                PlaySound(0x22C);
                Effects.SendLocationParticles(
                    EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                    0x376A, 9, 32, 1801, 0, 9502, 0
                );

                foreach (Mobile m in targets)
                {
                    DoHarmful(m);

                    int baseDamage = Utility.RandomMinMax(30, 45);
                    int coldDamage = baseDamage / 2;
                    int energyDamage = baseDamage / 2;

                    AOS.Damage(m, this, baseDamage, 0, 0, coldDamage, 0, energyDamage);

                    if (Utility.RandomDouble() < 0.6)
                    {
                        m.ApplyPoison(this, Poison.Lethal);
                        m.SendAsciiMessage("Shadowy brambles pierce you!");
                        Effects.SendTargetParticles(m, 0x374A, 10, 15, 1801, 0, 9502, 0, 0);
                    }
                }
            });
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (defender is Mobile m && DateTime.UtcNow >= _NextMeleeSpecial
                && Utility.RandomDouble() < 0.3)
            {
                DoHarmful(m);
                _NextMeleeSpecial = DateTime.UtcNow + MeleeSpecialCooldown;

                switch (Utility.Random(2))
                {
                    case 0:
                        {
                            int dMana = Utility.RandomMinMax(15, 25);
                            int dStam = Utility.RandomMinMax(15, 25);

                            m.Mana -= dMana;
                            m.Stam -= dStam;

                            m.SendAsciiMessage("The Nachtbram drains your vital energy!");
                            PlaySound(0x231);
                            Effects.SendTargetParticles(m, 0x377A, 10, 15, 1801, 0, 9502, 0, 0);
                            break;
                        }
                    case 1:
                        {
                            m.AddStatMod(new StatMod(StatType.Dex, "NachtbramDexDrain", -10, TimeSpan.FromSeconds(10.0)));
                            m.AddStatMod(new StatMod(StatType.Int, "NachtbramIntDrain", -10, TimeSpan.FromSeconds(10.0)));

                            m.AddResistanceMod(new ResistanceMod(ResistanceType.Cold, -10));
                            m.AddResistanceMod(new ResistanceMod(ResistanceType.Energy, -10));

                            m.SendAsciiMessage("The Nachtbram's touch weakens your defenses!");
                            PlaySound(0x1DE);
                            Effects.SendTargetParticles(m, 0x37B9, 10, 15, 1801, 0, 9502, 0, 0);
                            break;
                        }
                }
            }
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich);
            AddLoot(LootPack.HighScrolls, 2);

            int bpCount = Utility.RandomMinMax(15, 25);
            for (int i = 0; i < bpCount; i++)
                PackItem(new BlackPearl());

            int gdCount = Utility.RandomMinMax(15, 25);
            for (int i = 0; i < gdCount; i++)
                PackItem(new GraveDust());

            if (Utility.RandomDouble() < 0.005)
                PackItem(new Gutterthorn());
        }

        public override OppositionGroup OppositionGroup => OppositionGroup.FeyAndUndead;
        public override int TreasureMapLevel => Utility.RandomMinMax(4, 5);
        public override bool BleedImmune => true;
        public override Poison PoisonImmune => Poison.Lethal;
        public override bool Unprovokable => true;
        public override bool CanRummageCorpses => true;
        public override int Meat => 0;

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.WriteEncodedInt(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadEncodedInt();
        }
    }
}
