using System;
using Server.Mobiles;
using Server.Network;
using Server.Items;
using Server.Spells;
using Server.Targeting;

namespace Server.ACC.CSS.Systems.FletchingMagic
{
    public class EntanglingArrow : FletchingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Entangling Arrow", "Ex Vincla",
            21005,
            9400,
            false
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; } // Adjust the circle as needed for balance
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 20.0; } }
        public override int RequiredMana { get { return 20; } }

        public EntanglingArrow(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private EntanglingArrow m_Owner;

            public InternalTarget(EntanglingArrow owner) : base(12, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile)
                {
                    Mobile target = (Mobile)o;

                    if (m_Owner.CheckHSequence(target))
                    {
                        // Play arrow firing sound and effect
                        from.PlaySound(0x145); // Bow firing sound
                        Effects.SendMovingEffect(from, target, 0xF42, 10, 0, false, false, 0, 0);

                        // Delay to simulate arrow travel time
                        Timer.DelayCall(TimeSpan.FromSeconds(0.5), () =>
                        {
                            if (target == null || target.Deleted || !from.CanBeHarmful(target))
                                return;

                            from.DoHarmful(target);
                            
                            // Play net release effect
                            Effects.SendLocationEffect(target.Location, target.Map, 0x376A, 30, 10, 0, 0);

                            // Play entangling sound
                            target.PlaySound(0x205);

                            // Apply entangling effect
                            target.Freeze(TimeSpan.FromSeconds(5.0));
                            target.SendMessage("You have been entangled and cannot move!");

                            // Apply visual effect to indicate entanglement
                            target.FixedParticles(0x3779, 1, 15, 9921, 1153, 0, EffectLayer.Waist);
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
