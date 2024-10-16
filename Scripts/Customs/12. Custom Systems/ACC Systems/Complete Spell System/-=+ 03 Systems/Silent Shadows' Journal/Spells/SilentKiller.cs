using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using System.Collections;
using Server.Items;

namespace Server.ACC.CSS.Systems.StealthMagic
{
    public class SilentKiller : StealthSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Silent Killer", "Silent Takedown",
            21014,
            9213
        );

        public override double CastDelay => 0.1;
        public override double RequiredSkill => 75.0;
        public override int RequiredMana => 30;

        // Assuming `Cooldown` is not a member of `StealthSpell`, you might need to remove this if not needed
        // public override TimeSpan Cooldown => TimeSpan.FromMinutes(5);

        // Circle should match the expected type of SpellCircle
        public override SpellCircle Circle => SpellCircle.Fourth; // Assuming SpellCircle.Fourth corresponds to circle 4

        public SilentKiller(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private SilentKiller m_Owner;

            public InternalTarget(SilentKiller owner) : base(1, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Mobile target && m_Owner.Caster != target)
                {
                    if (target.Hidden || target.Warmode || target.Skills[SkillName.Hiding].Value < 50)
                    {
                        from.SendMessage("Your target is not unaware of your presence.");
                        return;
                    }

                    if (target.Hits < (target.HitsMax * 0.25))
                    {
                        m_Owner.PerformSilentKill(target);
                    }
                    else
                    {
                        from.SendMessage("The target is not weak enough to perform a silent takedown.");
                    }
                }
                else
                {
                    from.SendMessage("You can only target living creatures.");
                }

                m_Owner.FinishSequence();
            }
        }

        private void PerformSilentKill(Mobile target)
        {
            // Check if target is aware
            if (target.Combatant == null)
            {
                Effects.SendLocationParticles(
                    EffectItem.Create(target.Location, target.Map, EffectItem.DefaultDuration),
                    0x376A, 10, 15, 1153, 0, 5023, 0
                ); // Flashy blood effect

                Effects.PlaySound(target.Location, target.Map, 0x1FB); // Death scream sound

                target.Kill();

                Caster.SendMessage("You have silently eliminated your target.");
            }
            else
            {
                Caster.SendMessage("Your target is aware of your presence and cannot be silently taken down.");
            }
        }
    }
}
