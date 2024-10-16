using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.StealthMagic
{
    public class Ambush : StealthSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Ambush", "Attxu Shd",
            21010, // Animation or effect ID for starting animation
            9209,  // Sound ID for stealth sound effect
            false
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Third; }
        }

        public override double RequiredSkill { get { return 70.0; } } // Required skill level to use this ability
        public override int RequiredMana { get { return 30; } }       // Mana cost to use the ability

        // If the base class StealthSpell has no Cooldown property, you may need to remove this or add it in the base class
        // public override TimeSpan Cooldown { get { return TimeSpan.FromSeconds(30.0); } } // 30 seconds cooldown

        public Ambush(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        public void Target(Mobile target)
        {
            if (target == null || !Caster.CanSee(target))
            {
                Caster.SendLocalizedMessage(500237); // Target cannot be seen.
            }
            else if (CheckSequence())
            {
                SpellHelper.Turn(Caster, target);

                // Play stealth sound and visual effects
                Caster.PlaySound(9209); // Stealth sound effect
                Caster.FixedParticles(0x376A, 10, 15, 5030, EffectLayer.Waist); // Shadowy effect around caster

                // Apply stun effect to the target
                target.Freeze(TimeSpan.FromSeconds(5.0)); // 5 seconds stun duration
                target.SendLocalizedMessage(1060085); // You have been stunned!

                // Play a visual and sound effect on the target to signify the attack
                target.FixedEffect(0x37B9, 10, 16, 1149, 3); // Effect ID, speed, duration, hue, layer
                target.PlaySound(0x3E3); // Attack sound effect

                // Additional effects (e.g., minor damage, debuffs) can be added here

                FinishSequence();
            }
        }

        private class InternalTarget : Target
        {
            private Ambush m_Owner;

            public InternalTarget(Ambush owner) : base(1, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile target)
                    m_Owner.Target(target);
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

        // Implement the Circle property if required by the base class

        // Override OnCastFailed only if it exists in the base class and should be overridden
        // If it does not exist, you should remove this method
        // public override void OnCastFailed()
        // {
        //     Caster.SendLocalizedMessage(502632); // The spell fizzles.
        //     base.OnCastFailed();
        // }
    }
}
