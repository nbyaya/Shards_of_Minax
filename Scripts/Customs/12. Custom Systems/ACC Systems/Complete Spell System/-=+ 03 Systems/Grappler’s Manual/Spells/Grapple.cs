using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;

namespace Server.ACC.CSS.Systems.WrestlingMagic
{
    public class Grapple : WrestlingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Grapple", "Headlock!",
            21004, 9300);

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 0.0; } }
        public override int RequiredMana { get { return 15; } }

        public Grapple(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private Grapple m_Owner;

            public InternalTarget(Grapple owner) : base(12, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Mobile target && from.CanBeHarmful(target))
                {
                    m_Owner.Target(target);
                }
                else
                {
                    from.SendLocalizedMessage(500237); // Target can not be seen.
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

        public void Target(Mobile target)
        {
            if (Caster.CanBeHarmful(target) && CheckSequence())
            {
                // Apply paralysis effect
                Caster.DoHarmful(target);
                target.Paralyze(TimeSpan.FromSeconds(5.0));

                // Play visual and sound effects
                Effects.SendTargetParticles(target, 0x376A, 10, 15, 5021, EffectLayer.Waist);
                target.PlaySound(0x204);

                // Message to players
                Caster.SendMessage("You grapple your target, paralyzing them!");
                target.SendMessage("You have been grappled and paralyzed!");

                FinishSequence();
            }
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(2.0);
        }
    }
}
