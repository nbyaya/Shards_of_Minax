using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.TrackingMagic
{
    public class AmbushStrike : TrackingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Ambush Strike", "Strike from Shadows",
            21004, // Spell icon
            9300,  // Cast animation
            false
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

        public override double CastDelay { get { return 0.5; } } // Fast casting
        public override double RequiredSkill { get { return 60.0; } } // Required skill to use
        public override int RequiredMana { get { return 30; } } // Mana cost

        public AmbushStrike(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (Caster.Hidden || !Caster.CanSee(Caster.Combatant)) // Check if the caster is hidden or the target cannot see the caster
            {
                Caster.Target = new InternalTarget(this);
            }
            else
            {
                Caster.SendMessage("You must be hidden or undetected to perform an ambush strike!");
                FinishSequence();
            }
        }

        private class InternalTarget : Target
        {
            private AmbushStrike m_Owner;

            public InternalTarget(AmbushStrike owner) : base(10, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object target)
            {
                if (target is Mobile targetMobile)
                {
                    if (!from.CanBeHarmful(targetMobile))
                    {
                        from.SendMessage("You cannot attack that.");
                        return;
                    }

                    from.RevealingAction(); // Reveal the caster after the attack
                    from.DoHarmful(targetMobile);

                    int baseDamage = Utility.RandomMinMax(30, 45); // Base damage range
                    double damageMultiplier = from.Hidden ? 1.5 : 1.0; // Increased damage if hidden

                    int finalDamage = (int)(baseDamage * damageMultiplier);
                    targetMobile.Damage(finalDamage, from);

                    // Play flashy effects and sound
                    Effects.SendLocationEffect(targetMobile.Location, targetMobile.Map, 0x36BD, 20, 10, 0, 0); // Blood effect
                    Effects.PlaySound(targetMobile.Location, targetMobile.Map, 0x29A); // Sword swoosh sound

                    from.SendMessage("You strike from the shadows, dealing " + finalDamage + " damage!");
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
