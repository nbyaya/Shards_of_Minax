using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.TrackingMagic
{
    public class TrackingShot : TrackingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Tracking Shot", "Follow and Strike",
            //SpellCircle.First,
            21004,
            9300
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 30.0; } }
        public override int RequiredMana { get { return 30; } }

        public TrackingShot(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        public void Target(Mobile target)
        {
            if (Caster.CanBeBeneficial(target, false, true) && Caster.CanSee(target))
            {
                if (CheckSequence())
                {
                    // Apply visual and sound effects
                    Caster.PlaySound(0x20F); // Sound of an arrow being shot
                    Effects.SendMovingEffect(Caster, target, 0xF42, 18, 1, false, false, 0, 0); // Visual effect of an arrow

                    // Start the tracking projectile
                    Timer.DelayCall(TimeSpan.FromSeconds(0.5), () => TrackTarget(target));

                    // Reveal target's location
                    target.RevealingAction();
                    target.SendMessage("You have been targeted by a Tracking Shot!");

                    FinishSequence();
                }
            }
            else
            {
                Caster.SendLocalizedMessage(500237); // Target cannot be seen.
            }
        }

        private void TrackTarget(Mobile target)
        {
            if (target != null && !target.Deleted && target.Alive)
            {
                // Keep tracking the target for a short duration
                Effects.SendTargetEffect(target, 0x376A, 10, 10, 0, 0); // Visual effect on the target

                if (Utility.RandomDouble() < 0.8) // 80% chance to hit the target
                {
                    target.Damage(Utility.RandomMinMax(15, 30), Caster); // Deal damage
                    target.FixedParticles(0x374A, 10, 15, 5036, EffectLayer.Head); // More visual effects on hit
                    target.PlaySound(0x28E); // Sound effect on hit
                }
                else
                {
                    target.SendMessage("The tracking shot missed you!");
                }
            }
        }

        private class InternalTarget : Target
        {
            private TrackingShot m_Owner;

            public InternalTarget(TrackingShot owner) : base(12, false, TargetFlags.Harmful)
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
