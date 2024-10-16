using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.WrestlingMagic
{
    public class BreakFree : WrestlingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Break Free", "Liber",
            21004,
            9300
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Second; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 20.0; } }
        public override int RequiredMana { get { return 15; } }

        public BreakFree(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private BreakFree m_Owner;

            public InternalTarget(BreakFree owner) : base(12, false, TargetFlags.Beneficial)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object target)
            {
                if (target is Mobile)
                {
                    Mobile targetMobile = (Mobile)target;

                    if (targetMobile.Paralyzed)
                    {
                        if (m_Owner.CheckSequence())
                        {
                            // Remove paralyze
                            targetMobile.Paralyzed = false;

                            // Visual and sound effects
                            Effects.SendTargetParticles(targetMobile, 0x376A, 10, 15, 5021, EffectLayer.Waist);
                            targetMobile.PlaySound(0x214);

                            // Additional visual effect around caster
                            Effects.SendLocationParticles(
                                EffectItem.Create(targetMobile.Location, targetMobile.Map, EffectItem.DefaultDuration),
                                0x376A, 10, 15, 5023);

                            targetMobile.SendMessage("You feel a sudden surge of energy and break free from your restraints!");
                        }
                    }
                    else
                    {
                        from.SendMessage("Your target is not paralyzed.");
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
