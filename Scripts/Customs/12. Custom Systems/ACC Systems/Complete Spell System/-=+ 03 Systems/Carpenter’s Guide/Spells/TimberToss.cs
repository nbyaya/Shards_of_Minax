using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using Server.Misc;
using Server.ACC.CSS.Systems;

namespace Server.ACC.CSS.Systems.CarpentryMagic
{
    public class TimberToss : CarpentrySpell
    {
        // Corrected: Initialized an empty array for reagent types instead of null.
        private static SpellInfo m_Info = new SpellInfo(
            "Timber Toss",        // Spell Name
            "Toss Log",           // Spell Description
            21004,                // Sound ID
            9300,                 // Effect ID
            false,                // Not a harmful spell
            new Type[0]           // Empty reagent array instead of null
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Second; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 30.0; } }
        public override int RequiredMana { get { return 25; } }

        public TimberToss(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private TimberToss m_Owner;

            public InternalTarget(TimberToss owner) : base(12, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Mobile target)
                {
                    if (!from.CanBeHarmful(target))
                    {
                        from.SendLocalizedMessage(1060508); // Invalid Target
                    }
                    else if (m_Owner.CheckSequence())
                    {
                        from.DoHarmful(target);
                        SpellHelper.Turn(from, target);

                        // Effects
                        Effects.SendMovingEffect(from, target, 0x009D, 10, 0, false, false, 0, 0); // Log moving effect
                        from.PlaySound(21004); // Log toss sound

                        // Knockback effect
                        Point3D newLocation = target.Location;
                        newLocation.X += Utility.RandomMinMax(-2, 2); // Slight random displacement
                        newLocation.Y += Utility.RandomMinMax(-2, 2); // Slight random displacement
                        target.Location = newLocation;

                        // Stunning effect
                        target.Paralyze(TimeSpan.FromSeconds(3.0)); // Stun duration
                        target.SendLocalizedMessage(1060165); // You have been stunned!

                        // Visual effects on target
                        target.FixedParticles(0x373A, 10, 15, 5013, EffectLayer.Waist);
                        target.PlaySound(9300); // Stun sound effect
                    }
                }
                else
                {
                    from.SendLocalizedMessage(1060508); // Invalid Target
                }

                m_Owner.FinishSequence();
            }
        }
    }
}
