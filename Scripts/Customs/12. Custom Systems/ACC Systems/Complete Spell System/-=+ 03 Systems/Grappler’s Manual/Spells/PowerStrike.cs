using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using System.Collections;

namespace Server.ACC.CSS.Systems.WrestlingMagic
{
    public class PowerStrike : WrestlingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Power Strike", "Ferox Maximus",
            21004,
            9300,
            false,
            Reagent.MandrakeRoot
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Third; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 15; } }

        public PowerStrike(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private PowerStrike m_Owner;

            public InternalTarget(PowerStrike owner) : base(1, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Mobile)
                {
                    Mobile target = (Mobile)targeted;

                    if (from.CanBeHarmful(target) && m_Owner.CheckSequence())
                    {
                        from.DoHarmful(target);

                        // Calculate damage and stun chance
                        int damage = Utility.RandomMinMax(20, 40); // Enhanced damage range
                        double stunChance = 0.3; // 30% chance to stun

                        // Apply damage
                        AOS.Damage(target, from, damage, 100, 0, 0, 0, 0);

                        // Visual and sound effects
                        Effects.SendLocationEffect(target.Location, target.Map, 0x37B9, 20, 10, 0xB72, 0); // Explosion effect
                        Effects.PlaySound(target.Location, target.Map, 0x309); // Sound of a powerful strike

                        // Check for stun
                        if (Utility.RandomDouble() < stunChance)
                        {
                            target.Freeze(TimeSpan.FromSeconds(2.0)); // Stun for 2 seconds
                            from.SendMessage("You have stunned your opponent!");
                            target.SendMessage("You have been stunned by a powerful strike!");
                        }

                        // Flashy effect on caster
                        from.FixedParticles(0x3779, 1, 30, 9964, 2, 3, EffectLayer.Waist);
                        from.PlaySound(0x5C3); // Sound effect for the caster

                    }
                }

                m_Owner.FinishSequence();
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(2.5);
        }
    }
}
