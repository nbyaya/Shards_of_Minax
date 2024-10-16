using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Misc;
using System.Collections;

namespace Server.ACC.CSS.Systems.MacingMagic
{
    public class QuickReflexes : MacingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Quick Reflexes", "Celeris Agilitas",
                                                        21004,
                                                        9300
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Second; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 20.0; } }
        public override int RequiredMana { get { return 20; } }

        public QuickReflexes(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.Target = new InternalTarget(this);
            }

            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private QuickReflexes m_Owner;

            public InternalTarget(QuickReflexes owner) : base(12, false, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Mobile)
                {
                    Mobile target = (Mobile)targeted;

                    if (!m_Owner.Caster.CanBeBeneficial(target))
                    {
                        m_Owner.Caster.SendLocalizedMessage(1060508); // You cannot bless that target.
                    }
                    else
                    {
                        m_Owner.Caster.DoBeneficial(target);
                        Effects.PlaySound(target.Location, target.Map, 0x28E); // Dodge sound effect
                        target.FixedParticles(0x376A, 10, 15, 5030, EffectLayer.Waist); // Flashy dodge effect

                        target.SendMessage("You feel your reflexes sharpen!");

                        BuffInfo.AddBuff(target, new BuffInfo(BuffIcon.Agility, 1060641, 1075643, 30)); // Displaying agility buff for 30 seconds

                        Timer.DelayCall(TimeSpan.FromSeconds(30), () => RemoveQuickReflexes(target));
                    }
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

        public static void RemoveQuickReflexes(Mobile target)
        {
            if (target != null && !target.Deleted)
            {
                target.SendMessage("Your quick reflexes fade away.");
                BuffInfo.RemoveBuff(target, BuffIcon.Agility);
            }
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(2.0);
        }
    }
}
