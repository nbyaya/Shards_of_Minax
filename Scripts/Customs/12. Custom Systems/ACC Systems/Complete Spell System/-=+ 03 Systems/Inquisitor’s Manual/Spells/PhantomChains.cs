using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.DetectHiddenMagic
{
    public class PhantomChains : DetectHiddenSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Phantom Chains", "Bindo Chani",
            21004, // Gump ID
            9300,  // Sound ID
            Reagent.BlackPearl,
            Reagent.Bloodmoss,
            Reagent.MandrakeRoot
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 65.0; } }
        public override int RequiredMana { get { return 35; } }

        public PhantomChains(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private PhantomChains m_Owner;

            public InternalTarget(PhantomChains owner) : base(12, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Mobile)
                {
                    Mobile target = (Mobile)targeted;

                    if (from.CanBeHarmful(target) && from.InLOS(target) && m_Owner.CheckSequence())
                    {
                        from.DoHarmful(target);
                        target.RevealingAction();

                        Effects.SendLocationParticles(EffectItem.Create(target.Location, target.Map, EffectItem.DefaultDuration), 0x376A, 9, 32, 1108, 3, 9914, 0);
                        target.PlaySound(0x204);

                        target.SendMessage("Spectral chains bind you, dragging you out of hiding!");
                        
                        Timer.DelayCall(TimeSpan.FromSeconds(1.0), TimeSpan.FromSeconds(1.0), 10, () =>
                        {
                            if (target != null && !target.Deleted && target.Alive)
                            {
                                target.Damage(5, from);
                                Effects.SendLocationParticles(EffectItem.Create(target.Location, target.Map, EffectItem.DefaultDuration), 0x376A, 9, 32, 1108, 3, 9914, 0);
                                target.PlaySound(0x204);
                            }
                        });
                    }
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
