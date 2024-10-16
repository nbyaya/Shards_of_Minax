using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.ArcheryMagic
{
    public class PinningShot : ArcherySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Pinning Shot", "Stay still!",
                                                        //SpellCircle.Second,
                                                        21005,
                                                        9301
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Second; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 30; } }

        public PinningShot(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private PinningShot m_Owner;

            public InternalTarget(PinningShot owner) : base(10, false, TargetFlags.Harmful)
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

                        // Visual and Sound Effects
                        Effects.SendTargetParticles(target, 0x36BD, 20, 10, 5044, EffectLayer.Head);
                        target.PlaySound(0x204);

                        // Immobilize effect
                        target.Freeze(TimeSpan.FromSeconds(5.0));

                        from.SendMessage("You have pinned your target to the ground!");
                        target.SendMessage("You have been pinned to the ground and cannot move!");
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
            return TimeSpan.FromSeconds(1.5);
        }
    }
}
