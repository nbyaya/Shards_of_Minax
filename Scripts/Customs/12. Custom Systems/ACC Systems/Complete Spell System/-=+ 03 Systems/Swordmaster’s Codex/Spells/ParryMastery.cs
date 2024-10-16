using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.SwordsMagic
{
    public class ParryMastery : SwordsSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Parry Mastery", "Vas An Box",
            21008,
            9407,
            false,
            Reagent.BlackPearl,
            Reagent.MandrakeRoot
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Third; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 80.0; } }
        public override int RequiredMana { get { return 20; } }

        public ParryMastery(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private ParryMastery m_Owner;

            public InternalTarget(ParryMastery owner) : base(12, false, TargetFlags.Beneficial)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Mobile target && from.InRange(target, 12))
                {
                    if (from.CanBeBeneficial(target) && m_Owner.CheckSequence())
                    {
                        from.DoBeneficial(target);

                        target.FixedParticles(0x376A, 9, 32, 5008, EffectLayer.Waist);
                        target.PlaySound(0x1F7);

                        target.SendMessage("You feel a surge of defensive energy!");

                        double parryBonus = from.Skills[SkillName.Parry].Value * 0.2;
                        double duration = 10.0 + (from.Skills[SkillName.Magery].Value * 0.1);

                        BuffInfo.AddBuff(target, new BuffInfo(BuffIcon.Bless, 1075815, 1075816, TimeSpan.FromSeconds(duration), target));

                        target.VirtualArmorMod += (int)parryBonus;
                        Timer.DelayCall(TimeSpan.FromSeconds(duration), delegate
                        {
                            target.VirtualArmorMod -= (int)parryBonus;
                            target.SendMessage("The defensive energy fades away.");
                        });
                    }
                }
                else
                {
                    from.SendMessage("You cannot target that.");
                }

                m_Owner.FinishSequence();
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
