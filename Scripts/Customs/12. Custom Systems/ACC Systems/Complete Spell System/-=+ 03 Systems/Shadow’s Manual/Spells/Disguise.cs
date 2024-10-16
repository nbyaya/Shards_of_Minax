using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;

namespace Server.ACC.CSS.Systems.HidingMagic
{
    public class Disguise : HidingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Disguise", "In Cognito",
            21009, // Animation ID for the spell casting animation
            9208   // Sound ID for the spell casting sound
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Second; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 30; } }

        public Disguise(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                // Create a target for the spell to allow the player to choose their disguise
                Caster.Target = new InternalTarget(this);
            }
            else
            {
                FinishSequence();
            }
        }

        public void Target(Mobile target)
        {
            if (!Caster.CanSee(target))
            {
                Caster.SendLocalizedMessage(500237); // Target can not be seen.
            }
            else if (Caster.InRange(target, 12) && CheckSequence())
            {
                Caster.PlaySound(0x659); // Play sound effect for disguise
                Caster.FixedParticles(0x375A, 10, 15, 5037, EffectLayer.Head); // Show visual effect for disguise

                // Temporarily change the appearance of the caster
                Caster.BodyMod = target.Body; // Copy the body of the target
                Caster.HueMod = target.Hue; // Copy the hue (color) of the target

                // Timer to revert the disguise after a certain duration
                Timer.DelayCall(TimeSpan.FromSeconds(60), () => RevertDisguise(Caster));

                Caster.SendMessage("You have disguised yourself.");
            }
            else
            {
                Caster.SendLocalizedMessage(500237); // Target out of range or not valid.
            }

            FinishSequence();
        }

        private void RevertDisguise(Mobile caster)
        {
            // Revert the disguise effect
            caster.BodyMod = 0;
            caster.HueMod = -1;
            caster.PlaySound(0x1F8); // Play a sound effect when disguise fades
            caster.SendMessage("Your disguise has faded.");
        }

        private class InternalTarget : Target
        {
            private Disguise m_Owner;

            public InternalTarget(Disguise owner) : base(12, false, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile)
                    m_Owner.Target((Mobile)o);
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
