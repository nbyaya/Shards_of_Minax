using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;

namespace Server.ACC.CSS.Systems.ArmsLoreMagic
{
    public class BlindingStrike : ArmsLoreSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Blinding Strike", "Lux Fero",
            21004,
            9300,
            false,
            Reagent.Nightshade,
            Reagent.SpidersSilk
        );

        public override SpellCircle Circle => SpellCircle.Fourth;

        public override double CastDelay => 0.1;
        public override double RequiredSkill => 60.0;
        public override int RequiredMana => 25;

        public BlindingStrike(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private BlindingStrike m_Owner;

            public InternalTarget(BlindingStrike owner) : base(12, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile target && m_Owner.CheckSequence())
                {
                    SpellHelper.Turn(from, target);

                    // Calculate damage
                    double damage = Utility.RandomMinMax(25, 35); // Heavy damage
                    AOS.Damage(target, from, (int)damage, 100, 0, 0, 0, 0);

                    // Paralyze the target for a short duration
                    target.Freeze(TimeSpan.FromSeconds(3.0));

                    // Play sound and visual effects
                    target.FixedParticles(0x375A, 10, 30, 5037, EffectLayer.Head);
                    target.PlaySound(0x204);

                    from.SendMessage("You deliver a blinding strike, dealing heavy damage and paralyzing your target!");

                    if (m_Owner.Scroll != null)
                        m_Owner.Scroll.Consume();
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
