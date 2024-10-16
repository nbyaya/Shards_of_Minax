using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using System.Collections;

namespace Server.ACC.CSS.Systems.TrackingMagic
{
    public class PredatorsPounce : TrackingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Predators Pounce", "In Volo",
            21004,
            9300
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 30; } }

        public PredatorsPounce(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private PredatorsPounce m_Owner;

            public InternalTarget(PredatorsPounce owner) : base(12, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile target && m_Owner.CheckHSequence(target))
                {
                    SpellHelper.Turn(m_Owner.Caster, target);

                    // Create a visual effect and sound at the caster's location before the leap
                    Effects.SendLocationParticles(
                        EffectItem.Create(m_Owner.Caster.Location, m_Owner.Caster.Map, EffectItem.DefaultDuration),
                        0x3728,  // Effect ID for a puff of smoke
                        10,      // Speed
                        20,      // Duration
                        5052,    // Hue
                        0        // Render Mode
                    );
                    Effects.PlaySound(m_Owner.Caster.Location, m_Owner.Caster.Map, 0x1FB); // Leap sound effect

                    // Move the caster to the target's location to simulate a leap
                    m_Owner.Caster.Location = target.Location;

                    // Create a visual effect at the target's location to show impact
                    Effects.SendLocationParticles(
                        EffectItem.Create(target.Location, target.Map, EffectItem.DefaultDuration),
                        0x36BD,  // Effect ID for an explosion
                        20,      // Speed
                        10,      // Duration
                        5044,    // Hue
                        0        // Render Mode
                    );
                    Effects.PlaySound(target.Location, target.Map, 0x2E3); // Impact sound effect

                    // Deal damage to the target
                    int damage = Utility.RandomMinMax(20, 30); // Random damage between 20 and 30
                    AOS.Damage(target, from, damage, 100, 0, 0, 0, 0);

                    // Apply a brief stun to the target
                    target.Paralyze(TimeSpan.FromSeconds(2.0)); // 2-second stun

                    // Notify the caster and target
                    from.SendMessage("You pounce on your target with the ferocity of a predator!");
                    target.SendMessage("You are struck by a sudden, stunning attack!");

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
