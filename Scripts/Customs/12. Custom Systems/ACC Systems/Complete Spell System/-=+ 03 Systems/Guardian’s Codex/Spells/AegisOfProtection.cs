using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Items;
using System.Collections;

namespace Server.ACC.CSS.Systems.ParryMagic
{
    public class AegisOfProtection : ParrySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Aegis of Protection", "Aegis Maximus",
                                                        21005,
                                                        9301
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Third; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 25; } }

        public AegisOfProtection(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                // Consume mana
                Caster.Mana -= RequiredMana;

                // Apply the healing effect over time
                Caster.SendMessage("You feel a protective aegis surrounding you, restoring your health!");
                Effects.PlaySound(Caster.Location, Caster.Map, 0x1F7); // Play a protective sound
                Caster.FixedParticles(0x373A, 10, 15, 5018, EffectLayer.Waist); // Visual effect around the caster

                // Apply healing over time
                new HealTimer(Caster).Start();
            }

            FinishSequence();
        }

        private class HealTimer : Timer
        {
            private Mobile m_Caster;
            private int m_HealCount = 5; // Number of times to heal
            private int m_HealAmount = 10; // Amount of health regained per tick

            public HealTimer(Mobile caster) : base(TimeSpan.FromSeconds(1.0), TimeSpan.FromSeconds(1.0)) // Heals every second
            {
                m_Caster = caster;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Caster == null || m_Caster.Deleted || !m_Caster.Alive)
                {
                    Stop();
                    return;
                }

                if (m_HealCount > 0)
                {
                    m_Caster.Hits += m_HealAmount;
                    m_Caster.FixedEffect(0x376A, 1, 16, 1153, 0); // Green sparkles indicating healing

                    m_HealCount--;

                    if (m_HealCount == 0)
                    {
                        m_Caster.SendMessage("The aegis of protection fades away.");
                        Stop();
                    }
                }
                else
                {
                    Stop();
                }
            }
        }
    }
}
