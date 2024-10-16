using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.ArcheryMagic
{
    public class StealthShot : ArcherySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Stealth Shot", "In Omus",
            // Use a unique icon and sound
            21005,
            9301,
            false
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 30; } }

        public StealthShot(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private StealthShot m_Owner;

            public InternalTarget(StealthShot owner) : base(10, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Mobile target)
                {
                    if (from.CanBeHarmful(target) && m_Owner.CheckSequence())
                    {
                        from.RevealingAction();
                        from.DoHarmful(target);

                        // Calculate increased damage if the caster is hiding
                        double damage = Utility.RandomMinMax(15, 25); // Base damage
                        if (from.Hidden)
                        {
                            damage *= 1.5; // 50% increased damage
                        }

                        // Apply damage
                        AOS.Damage(target, from, (int)damage, 100, 0, 0, 0, 0);

                        // Play visual effects
                        Effects.SendTargetParticles(target, 0x3728, 1, 13, 0x480, EffectLayer.Waist);
                        Effects.SendTargetParticles(target, 0x372A, 10, 15, 5013, EffectLayer.Head);

                        // Play sound effects
                        from.PlaySound(0x145); // Sound for firing an arrow
                        target.PlaySound(0x5C2); // Impact sound

                        // Chance to remain hidden
                        if (Utility.RandomDouble() < 0.5)
                        {
                            from.Hidden = true;
                            from.SendMessage("You remain hidden after the shot!");
                        }
                        else
                        {
                            from.RevealingAction();
                        }
                    }
                }
                else
                {
                    from.SendMessage("You must target a mobile.");
                }

                m_Owner.FinishSequence();
            }
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(2.0); // Quick casting for an archer skill
        }
    }
}
