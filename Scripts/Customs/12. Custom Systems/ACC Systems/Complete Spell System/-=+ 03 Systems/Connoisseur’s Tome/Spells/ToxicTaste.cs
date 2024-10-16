using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;

namespace Server.ACC.CSS.Systems.TasteIDMagic
{
    public class ToxicTaste : TasteIDSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Toxic Taste", "Toxio Tasto",
                                                        21004, 9300
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 30.0; } }
        public override int RequiredMana { get { return 10; } }

        public ToxicTaste(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private void Target(Mobile m)
        {
            if (Caster.CanBeHarmful(m) && CheckSequence())
            {
                if (this.Scroll != null)
                    Scroll.Consume();

                SpellHelper.Turn(Caster, m);

                // Apply poison effect
                Poison poison = Poison.GetPoison(2); // Level 2 poison
                m.ApplyPoison(Caster, poison);

                // Visual and sound effects
                m.FixedParticles(0x374A, 10, 15, 5021, EffectLayer.Waist);
                m.PlaySound(0x205);

                // Send message to target
                m.SendMessage("You feel a toxic substance coursing through your veins!");

                // Send message to caster
                Caster.SendMessage("You have successfully applied a toxic taste to your target!");

                FinishSequence();
            }
        }

        private class InternalTarget : Target
        {
            private ToxicTaste m_Owner;

            public InternalTarget(ToxicTaste owner) : base(12, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Mobile target)
                {
                    m_Owner.Target(target);
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
