using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.AlchemyMagic
{
    public class Antidote : AlchemySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Antidote", "Neutralizes poisons and toxins, curing the drinker of poison effects.",
            21005, // ButtonID
            9301   // IconID
        );
		
        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 30.0; } }
        public override int RequiredMana { get { return 20; } }

        public Antidote(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private void Target(Mobile target)
        {
            if (!Caster.CanSee(target))
            {
                Caster.SendLocalizedMessage(500237); // Target cannot be seen.
            }
            else if (CheckSequence())
            {
                if (target.Poison != null)
                {
                    target.Poison = null; // Cure poison effect
                    target.SendLocalizedMessage(1010059); // You feel yourself starting to recover.

                    // Play cure effect and sound
                    Effects.SendTargetParticles(target, 0x373A, 10, 15, 5013, EffectLayer.Waist);
                    target.PlaySound(0x1F2);

                    // Create a flashy visual effect
                    Effects.SendLocationParticles(
                        EffectItem.Create(target.Location, target.Map, EffectItem.DefaultDuration), 
                        0x374A, 10, 30, 5052
                    );
                    Effects.PlaySound(target.Location, target.Map, 0x1E0);
                }
                else
                {
                    Caster.SendMessage("The target is not poisoned.");
                }
            }

            FinishSequence();
        }

        private bool CheckBenevolent(Mobile caster, Mobile target)
        {
            // Placeholder logic: Always returns true. Replace with actual logic if needed.
            return caster.Alive && target.Alive;
        }

        private class InternalTarget : Target
        {
            private Antidote m_Owner;

            public InternalTarget(Antidote owner) : base(12, false, TargetFlags.Beneficial)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile target)
                {
                    m_Owner.Target(target);
                }
                else
                {
                    from.SendLocalizedMessage(500909); // You can only target living things.
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
