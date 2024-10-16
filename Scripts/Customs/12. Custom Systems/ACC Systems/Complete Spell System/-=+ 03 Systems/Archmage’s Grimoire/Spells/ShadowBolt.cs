using System;
using Server.Mobiles;
using Server.Targeting;
using Server.Network;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.MageryMagic
{
    public class ShadowBolt : MagerySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Shadow Bolt", "An Corp Xen",
            21004, // Effect ID for spell animation
            9300, // Sound ID for casting sound
            false,
            Reagent.Bloodmoss,
            Reagent.MandrakeRoot,
            Reagent.Nightshade
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Sixth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 60.0; } }
        public override int RequiredMana { get { return 25; } }

        public ShadowBolt(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private ShadowBolt m_Owner;

            public InternalTarget(ShadowBolt owner) : base(12, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile target)
                {
                    if (from.CanBeHarmful(target) && m_Owner.CheckSequence())
                    {
                        from.DoHarmful(target);

                        // Play visual and sound effects
                        Effects.SendMovingEffect(from, target, 0x36E4, 7, 0, false, false, 0, 0);
                        Effects.PlaySound(target.Location, target.Map, 0x20C);

                        // Calculate damage (50% of the target's current health)
                        int damage = (int)(target.Hits * 0.5);

                        // Apply damage
                        target.Damage(damage, from);

                        // Flashy effect on hit
                        Effects.SendLocationParticles(
                            EffectItem.Create(target.Location, target.Map, EffectItem.DefaultDuration),
                            0x374A, 10, 30, 1109, 0, 5029, 0
                        );

                        // Additional sound effect on impact
                        target.PlaySound(0x1F8);
                    }
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
