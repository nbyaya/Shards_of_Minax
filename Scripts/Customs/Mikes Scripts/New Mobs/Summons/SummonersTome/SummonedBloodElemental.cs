using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a summoned blood elemental corpse")]
    public class SummonedBloodElemental : BaseCreature, IBloodCreature
    {
        private Mobile _summoner;
        private double _spiritSpeakBonus;

        [Constructable]
        public SummonedBloodElemental()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a summoned blood elemental";
            Body = 159;
            BaseSoundID = 278;



            _spiritSpeakBonus = 0;

            int str = (int)(550 + (150 * _spiritSpeakBonus));
            int dex = (int)(70 + (40 * _spiritSpeakBonus));
            int intel = (int)(300 + (100 * _spiritSpeakBonus));

            SetStr(str);
            SetDex(dex);
            SetInt(intel);

            int hits = (int)(350 + (200 * _spiritSpeakBonus));
            SetHits(hits);

            SetDamage(18, 30);

            SetDamageType(ResistanceType.Physical, 0);
            SetDamageType(ResistanceType.Poison, 50);
            SetDamageType(ResistanceType.Energy, 50);

            SetResistance(ResistanceType.Physical, 55 + (int)(10 * _spiritSpeakBonus), 65 + (int)(10 * _spiritSpeakBonus));
            SetResistance(ResistanceType.Fire, 25, 35);
            SetResistance(ResistanceType.Cold, 45, 55);
            SetResistance(ResistanceType.Poison, 55, 65);
            SetResistance(ResistanceType.Energy, 35, 45);

            SetSkill(SkillName.EvalInt, 100.0);
            SetSkill(SkillName.Magery, 100.0);
            SetSkill(SkillName.MagicResist, 90.0);
            SetSkill(SkillName.Tactics, 90.0);
            SetSkill(SkillName.Wrestling, 100.0);

            Fame = 15000;
            Karma = -15000;

            VirtualArmor = 65;

            ControlSlots = 3;
            Summoned = true;


            Timer.DelayCall(TimeSpan.FromSeconds(1), DoSpecialAbilities); // Start ticking specials
        }

        public override void OnThink()
        {
            base.OnThink();
        }

        private void DoSpecialAbilities()
        {
            if (Deleted || !Alive || _summoner == null || !_summoner.Alive)
                return;

            // 20% chance to trigger blood nova every 10 seconds
            if (Utility.RandomDouble() < 0.2)
            {
                BloodNova();
            }

            Timer.DelayCall(TimeSpan.FromSeconds(10), DoSpecialAbilities);
        }

        private void BloodNova()
        {
            FixedParticles(0x374A, 10, 15, 5036, EffectLayer.Waist);
            PlaySound(0x231);

            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m != _summoner && SpellHelper.ValidIndirectTarget(_summoner, m) && _summoner.CanBeHarmful(m))
                {
                    _summoner.DoHarmful(m);
                    AOS.Damage(m, _summoner, 15 + (int)(15 * _spiritSpeakBonus), 0, 40, 0, 40, 0);

                    // Debuff resistances
                    m.FixedParticles(0x3709, 10, 30, 5018, 0x140, 3, EffectLayer.Head);
                    m.SendMessage("You feel your resistances weakening!");


                }
            }
        }

        public override bool OnBeforeDeath()
        {
            if (_summoner != null)
            {
                _summoner.SendMessage("Your summoned blood elemental dissolves into crimson mist.");
            }

            return base.OnBeforeDeath();
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (_summoner != null && Utility.RandomDouble() < 0.3)
            {
                int drain = Utility.RandomMinMax(8, 16) + (int)(10 * _spiritSpeakBonus);
                _summoner.Hits += drain;
                defender.FixedParticles(0x36BD, 20, 10, 5044, EffectLayer.Head);
                defender.SendMessage("Your blood is drawn into the summoned elemental!");
                _summoner.SendMessage($"You siphon {drain} health through the Blood Elemental.");
            }
        }

        public SummonedBloodElemental(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
