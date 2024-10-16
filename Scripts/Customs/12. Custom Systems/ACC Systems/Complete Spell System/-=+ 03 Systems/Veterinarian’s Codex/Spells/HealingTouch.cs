using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;

namespace Server.ACC.CSS.Systems.VeterinaryMagic
{
    public class HealingTouch : VeterinarySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Healing Touch", "Sanctus Manus",
            // Optional spell circle definition if needed
            21005,
            9301
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Second; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 20.0; } } // Adjust skill requirement as needed
        public override int RequiredMana { get { return 20; } }

        public HealingTouch(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        public void Target(Mobile target)
        {
            if (!Caster.CanSee(target))
            {
                Caster.SendLocalizedMessage(500237); // Target cannot be seen.
            }
            else if (CheckSequence())
            {
                // Consuming mana
                if (Caster.Mana >= RequiredMana)
                {
                    Caster.Mana -= RequiredMana;

                    // Healing amount calculation (significant health restoration)
                    int healAmount = Utility.RandomMinMax(40, 80); // Heal 40 to 80 health points
                    target.Heal(healAmount);

                    // Visual effects
                    target.FixedParticles(0x376A, 9, 32, 5005, EffectLayer.Waist); // Blue swirls around the target
                    target.PlaySound(0x202); // Healing sound effect

                    // Success message
                    Caster.SendMessage("You have successfully healed the target with Healing Touch.");
                    target.SendMessage("You feel a soothing energy healing your wounds.");
                }
                else
                {
                    Caster.SendMessage("You do not have enough mana to cast this spell.");
                }
            }

            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private HealingTouch m_Owner;

            public InternalTarget(HealingTouch owner) : base(12, false, TargetFlags.Beneficial)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile)
                {
                    m_Owner.Target((Mobile)o);
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
