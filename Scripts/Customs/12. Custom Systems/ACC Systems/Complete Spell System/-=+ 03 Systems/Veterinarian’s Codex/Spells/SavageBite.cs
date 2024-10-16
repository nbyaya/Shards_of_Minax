using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;

namespace Server.ACC.CSS.Systems.VeterinaryMagic
{
    public class SavageBite : VeterinarySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Savage Bite", "In Mordax",
                                                        //SpellCircle.Second,
                                                        21005,
                                                        9301,
                                                        false,
                                                        Reagent.Bloodmoss,
                                                        Reagent.BatWing
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Second; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 30.0; } }
        public override int RequiredMana { get { return 20; } }

        public SavageBite(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private SavageBite m_Owner;

            public InternalTarget(SavageBite owner) : base(10, false, TargetFlags.Harmful)
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

                        // Visual and Sound Effects
                        Effects.SendMovingEffect(from, target, 0x36D4, 10, 0, false, false, 1167, 0); // Red projectile
                        Effects.PlaySound(target.Location, target.Map, 0x1F1); // Bite sound

                        int damage = Utility.RandomMinMax(15, 25);
                        target.Damage(damage, from);

                        // Additional visual effect on impact
                        target.FixedParticles(0x374A, 10, 15, 5032, EffectLayer.Waist);

                        // Stun the target briefly
                        target.Freeze(TimeSpan.FromSeconds(1.0));
                    }
                    else
                    {
                        from.SendMessage("You cannot harm that target.");
                    }
                }
                else
                {
                    from.SendMessage("That is not a valid target.");
                }

                m_Owner.FinishSequence();
            }
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(5.0);
        }
    }
}
