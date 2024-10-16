using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Items;
using Server.Spells;

namespace Server.ACC.CSS.Systems.LockpickingMagic
{
    public class SilentPick : LockpickingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Silent Pick", "Xex Pix",
            21001,
            9301
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; } // Adjust as needed
        }

        public override double CastDelay { get { return 0.2; } } // Adjust cast delay
        public override double RequiredSkill { get { return 25.0; } } // Required skill level to cast
        public override int RequiredMana { get { return 10; } } // Mana cost

        public SilentPick(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private SilentPick m_Owner;

            public InternalTarget(SilentPick owner) : base(6, false, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is LockableContainer container && container.Locked)
                {
                    if (m_Owner.CheckSequence())
                    {
                        // Play lockpicking animation and sound
                        from.PlaySound(0x241);
                        from.Animate(9, 1, 1, true, false, 0);

                        // Flashy visual effect
                        Effects.SendLocationParticles(EffectItem.Create(container.Location, container.Map, EffectItem.DefaultDuration), 0x375A, 10, 15, 0, 0, 5020, 0);
                        Effects.PlaySound(container.Location, container.Map, 0x1F7);

                        // Successful lockpick attempt
                        if (Utility.RandomDouble() < (from.Skills[SkillName.Lockpicking].Value / 100.0))
                        {
                            container.Locked = false;
                            from.SendMessage("You silently pick the lock without making any noise.");
                        }
                        else
                        {
                            from.SendMessage("You fail to pick the lock silently.");
                        }
                    }
                }
                else
                {
                    from.SendMessage("That is not a locked container.");
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
            return TimeSpan.FromSeconds(1.5); // Adjust as needed
        }
    }
}
