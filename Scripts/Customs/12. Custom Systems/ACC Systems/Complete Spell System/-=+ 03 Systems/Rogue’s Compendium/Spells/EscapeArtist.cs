using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;

namespace Server.ACC.CSS.Systems.StealingMagic
{
    public class EscapeArtist : StealingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Escape Artist", "Freedom and Swiftness",
                                                        //SpellCircle.First,
                                                        21004,
                                                        9300
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

        public override double CastDelay { get { return 0.5; } }
        public override double RequiredSkill { get { return 20.0; } } // Low requirement as it's a quick escape ability
        public override int RequiredMana { get { return 10; } }

        public EscapeArtist(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.PlaySound(0x1F7); // A sound effect for escape or disappearing
                Caster.FixedParticles(0x373A, 10, 15, 5018, EffectLayer.Waist); // Visual effect of magical burst

                if (Caster.Paralyzed || Caster.Frozen)
                {
                    Caster.Paralyzed = false;
                    Caster.Frozen = false;
                }

                // Applying a burst of speed
                Caster.SendLocalizedMessage(1060185); // "You feel yourself moving faster!"
                Caster.MovingEffect(Caster, 0x3779, 10, 0, false, false); // Speed boost visual effect
                Caster.Hits += 5; // Minor healing effect as a reward for escape
                Caster.Stam += 20; // Restore some stamina

                // Boosting the speed temporarily
                Effects.SendTargetEffect(Caster, 0x3779, 10, 30);
                new SpeedBoostTimer(Caster).Start();

                Caster.SendMessage("You quickly free yourself from binds and traps, gaining a burst of speed!");
            }

            FinishSequence();
        }

        private class SpeedBoostTimer : Timer
        {
            private Mobile m_Mobile;

            public SpeedBoostTimer(Mobile mobile) : base(TimeSpan.FromSeconds(10.0))
            {
                m_Mobile = mobile;
                Priority = TimerPriority.TwoFiftyMS;
                m_Mobile.SendMessage("You feel a surge of speed!");
                m_Mobile.Delta(MobileDelta.Hits);
            }

            protected override void OnTick()
            {
                if (m_Mobile != null && !m_Mobile.Deleted)
                {
                    m_Mobile.SendMessage("The burst of speed fades.");
                    m_Mobile.Delta(MobileDelta.Hits);
                }
            }
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(2.0); // Quick cast time
        }
    }
}
