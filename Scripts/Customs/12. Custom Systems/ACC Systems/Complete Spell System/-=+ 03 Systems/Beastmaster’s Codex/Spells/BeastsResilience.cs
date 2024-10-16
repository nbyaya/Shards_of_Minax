using System;
using System.Collections.Generic;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;
using Server.Items;

namespace Server.ACC.CSS.Systems.AnimalTamingMagic
{
    public class BeastsResilience : AnimalTamingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Beast’s Resilience", "Resili Bestia",
            21004, // Icon ID
            9300,  // Action ID
            false
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Sixth; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 70.0; } }
        public override int RequiredMana { get { return 20; } }

        public BeastsResilience(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        public void Target(IPoint3D p)
        {
            if (!Caster.CanSee(p))
            {
                Caster.SendLocalizedMessage(500237); // Target cannot be seen.
            }
            else if (CheckSequence())
            {
                SpellHelper.Turn(Caster, p);

                // Play visual and sound effects
                Effects.PlaySound(Caster.Location, Caster.Map, 0x299); // Play a beast-like roar sound
                Caster.FixedParticles(0x373A, 10, 30, 5036, EffectLayer.Waist); // Cast particle effect

                // Find all followers within 5 tiles and apply immunity effect
                List<BaseCreature> followers = new List<BaseCreature>();

                foreach (Mobile m in Caster.GetMobilesInRange(5))
                {
                    if (m is BaseCreature creature && creature.Controlled && creature.ControlMaster == Caster)
                    {
                        followers.Add(creature);
                        creature.SendMessage("You feel a surge of resilience!");
                        creature.FixedParticles(0x376A, 9, 32, 5008, EffectLayer.Waist); // Apply visual effect to each follower
                        creature.PlaySound(0x1F2); // Play a sound to each follower
                    }
                }

                // Apply immunity to damage for 10 seconds
                foreach (BaseCreature follower in followers)
                {
                    follower.BeginAction(typeof(BeastsResilience));
                    follower.Frozen = true; // Temporarily make them immune to physical attacks by freezing them

                    Timer.DelayCall(TimeSpan.FromSeconds(10.0), () =>
                    {
                        follower.EndAction(typeof(BeastsResilience));
                        follower.Frozen = false; // Remove the freeze after 10 seconds
                        follower.SendMessage("The resilience fades away.");
                    });
                }

                FinishSequence();
            }
        }

        private class InternalTarget : Target
        {
            private BeastsResilience m_Owner;

            public InternalTarget(BeastsResilience owner) : base(12, true, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is IPoint3D)
                {
                    m_Owner.Target((IPoint3D)targeted);
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
