using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;

namespace Server.ACC.CSS.Systems.FencingMagic
{
    public class Ensnare : FencingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Ensnare", "Snarus Bearus",
            21005, // Animation ID for a quick thrust
            9301 // Sound effect ID for a sharp, swift sound
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; } // Adjust spell circle based on balance
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 20.0; } } // Example skill requirement
        public override int RequiredMana { get { return 20; } }

        public Ensnare(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private void Target(Mobile target)
        {
            if (!Caster.CanSee(target))
            {
                Caster.SendLocalizedMessage(500237); // Target cannot be seen.
            }
            else if (CheckHSequence(target)) // Check spell sequence and target validity
            {
                if (this.Scroll != null)
                    Scroll.Consume();
                
                SpellHelper.Turn(Caster, target);

                // Visual and Sound Effect
                Effects.SendLocationEffect(target.Location, target.Map, 0x3728, 30, 10, 0x13B2, 0); // Visual effect
                Effects.PlaySound(target.Location, target.Map, 0x1F2); // Sound effect

                // Temporary Dexterity Reduction
                TimeSpan duration = TimeSpan.FromSeconds(10.0); // Duration of debuff
                int dexReduction = (int)(target.Dex * 0.2);
                target.Dex -= dexReduction; // Reduce Dexterity by 20%

                // Timer to restore Dexterity
                Timer.DelayCall(duration, () =>
                {
                    if (target != null && !target.Deleted)
                    {
                        target.Dex += dexReduction;
                        target.SendMessage("Your dexterity returns to normal."); // Notify target
                    }
                });

                // Additional Effects
                target.FixedParticles(0x376A, 10, 15, 5013, EffectLayer.Waist); // Extra visual flair
                target.PlaySound(0x208); // Extra sound effect for impact
            }

            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private Ensnare m_Owner;

            public InternalTarget(Ensnare owner) : base(10, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Mobile target && target != from)
                    m_Owner.Target(target);
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
