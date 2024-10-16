using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;
using Server.Misc;
using System.Collections.Generic;
using Server.Items;

namespace Server.ACC.CSS.Systems.HidingMagic
{
    public class ShadowStep : HidingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Shadow Step", "In Visi Extele",
            21001,
            9200
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Second; }
        }

        public override double CastDelay { get { return 0.2; } } // Shorter delay for quick escape
        public override double RequiredSkill { get { return 30.0; } } // Required skill level
        public override int RequiredMana { get { return 10; } } // Mana cost

        public ShadowStep(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private ShadowStep m_Owner;

            public InternalTarget(ShadowStep owner) : base(10, true, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is IPoint3D point)
                {
                    IPoint3D targetLocation = new Point3D(point);

                    if (!from.InRange(targetLocation, 10))
                    {
                        from.SendMessage("That location is too far away.");
                        return;
                    }

                    if (m_Owner.CheckSequence())
                    {
                        // Perform teleportation
                        Point3D newLocation = new Point3D(targetLocation);
                        Effects.SendLocationParticles(EffectItem.Create(from.Location, from.Map, EffectItem.DefaultDuration), 0x376A, 9, 32, 5008);
                        Effects.PlaySound(from.Location, from.Map, 0x1FE); // Teleport sound
                        from.Location = newLocation;
                        from.ProcessDelta();

                        // Apply invisibility effect
                        from.Hidden = true;
                        Effects.SendLocationParticles(EffectItem.Create(from.Location, from.Map, EffectItem.DefaultDuration), 0x376A, 9, 32, 5008);
                        from.SendMessage("You have stepped into the shadows.");

                        // Timer to remove invisibility after a short duration
                        Timer.DelayCall(TimeSpan.FromSeconds(5.0), () =>
                        {
                            from.RevealingAction();
                            from.SendMessage("You are visible again.");
                        });

                        // Some flashy effects
                        Effects.SendLocationParticles(EffectItem.Create(from.Location, from.Map, EffectItem.DefaultDuration), 0x373A, 1, 15, 0, 0, 9502, 0);
                        Effects.PlaySound(from.Location, from.Map, 0x658); // Flash sound
                    }

                    m_Owner.FinishSequence();
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
