using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.ArcheryMagic
{
    public class TrackingArrow : ArcherySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Tracking Arrow", "Be marked!",
            //SpellCircle.Third,
            21005,
            9301
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Third; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 60.0; } }
        public override int RequiredMana { get { return 20; } }

        public TrackingArrow(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private void Target(Mobile m)
        {
            if (Caster.CanBeHarmful(m) && CheckSequence())
            {
                Caster.DoHarmful(m);
                SpellHelper.Turn(Caster, m);

                // Apply the visual and sound effects
                Effects.SendLocationParticles(EffectItem.Create(m.Location, m.Map, EffectItem.DefaultDuration), 0x3779, 10, 30, 1153, 4, 3, 0);
                Effects.PlaySound(m.Location, m.Map, 0x208);

                // Reduce the target's strength by 90%
                int strReduction = (int)(m.RawStr * 0.9);
                m.RawStr -= strReduction;

                // Display the message to the target
                m.SendMessage("You have been hit by a tracking arrow! Your strength is greatly reduced!");

                // Timer to restore the target's strength after 30 seconds
                Timer.DelayCall(TimeSpan.FromSeconds(30), () =>
                {
                    if (m != null && !m.Deleted)
                    {
                        m.RawStr += strReduction;
                        m.SendMessage("The effect of the tracking arrow has worn off, and your strength is restored.");
                    }
                });
            }

            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private TrackingArrow m_Owner;

            public InternalTarget(TrackingArrow owner) : base(12, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Mobile)
                {
                    m_Owner.Target((Mobile)targeted);
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
