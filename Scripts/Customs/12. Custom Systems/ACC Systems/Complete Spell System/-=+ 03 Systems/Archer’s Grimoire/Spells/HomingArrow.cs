using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.ArcheryMagic
{
    public class HomingArrow : ArcherySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Homing Arrow", "Seek Foe",
            21005,
            9301
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Second; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 25; } }

        public HomingArrow(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private HomingArrow m_Owner;

            public InternalTarget(HomingArrow owner) : base(12, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile)
                {
                    Mobile target = (Mobile)o;

                    if (!from.CanBeHarmful(target))
                    {
                        from.SendLocalizedMessage(100402); // You can't harm that target.
                        return;
                    }

                    if (m_Owner.CheckSequence())
                    {
                        SpellHelper.Turn(from, target);

                        // Play flashy effects and sounds
                        Effects.SendMovingEffect(from, target, 0x1BFE, 7, 0, false, false, 0x480, 0);
                        from.PlaySound(0x145);

                        Timer.DelayCall(TimeSpan.FromSeconds(0.5), () =>
                        {
                            if (from.CanBeHarmful(target))
                            {
                                from.DoHarmful(target);
                                AOS.Damage(target, from, Utility.RandomMinMax(30, 40), 0, 100, 0, 0, 0); // Deals 30-40 physical damage
                                Effects.SendLocationParticles(EffectItem.Create(target.Location, target.Map, EffectItem.DefaultDuration), 0x373A, 10, 15, 5023);
                                target.PlaySound(0x208); // Hit sound
                            }
                        });
                    }
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(2.0);
        }
    }
}
