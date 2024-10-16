using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.FletchingMagic
{
    public class CripplingShot : FletchingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Crippling Shot", "In Bal Vel",
            21005,
            9400,
            false
        );

        public override SpellCircle Circle => SpellCircle.Fourth;

        public override double CastDelay => 0.1;
        public override double RequiredSkill => 60.0;
        public override int RequiredMana => 12;

        public CripplingShot(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private CripplingShot m_Owner;

            public InternalTarget(CripplingShot owner) : base(12, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Mobile target)
                {
                    if (from.CanBeHarmful(target) && m_Owner.CheckSequence())
                    {
                        from.DoHarmful(target);

                        // Play shot effect
                        Effects.SendMovingEffect(from, target, 0xF42, 10, 0, false, false, 0, 0);
                        from.PlaySound(0x145); // Sound for shooting

                        // Delayed effect to simulate arrow travel
                        Timer.DelayCall(TimeSpan.FromSeconds(0.5), () => ApplyCripplingEffect(from, target));

                        m_Owner.FinishSequence();
                    }
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }

            private void ApplyCripplingEffect(Mobile caster, Mobile target)
            {
                if (target != null && !target.Deleted && target.Alive)
                {
                    // Apply slowing effect
                    target.SendMessage("You feel your movement and attack speed slow down!");
                    target.PlaySound(0x1FB); // Sound effect for slowing down
                    Effects.SendLocationEffect(target.Location, target.Map, 0x376A, 10, 1, 1153, 0); // Visual effect

                    // Save the original Dexterity
                    int originalDex = target.Dex;
                    int slowedDex = (int)(originalDex * 0.5); // Slow down by 50%
                    
                    target.Dex = slowedDex; // Adjust Dexterity

                    // Ensure Dexterity is restored after 10 seconds
                    Timer.DelayCall(TimeSpan.FromSeconds(10.0), () => RemoveCripplingEffect(target, originalDex));
                }
            }

            private void RemoveCripplingEffect(Mobile target, int originalDex)
            {
                if (target != null && !target.Deleted && target.Alive)
                {
                    target.Dex = originalDex; // Restore original Dexterity
                    target.SendMessage("You regain your normal speed.");
                }
            }
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(1.5);
        }
    }
}
