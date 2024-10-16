using System;
using Server.Mobiles;
using Server.Network;
using Server.Items;
using Server.Spells;
using System.Collections;

namespace Server.ACC.CSS.Systems.AlchemyMagic
{
    public class HealingDraught : AlchemySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Healing Draught", "In Vas Mani",
            21005,
            9301
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; } // Adjust as necessary
        }

        public override double CastDelay { get { return 0.2; } } // Casting delay in seconds
        public override double RequiredSkill { get { return 20.0; } } // Skill requirement
        public override int RequiredMana { get { return 15; } } // Mana cost

        public HealingDraught(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Mobile target = Caster; // Healing self for this spell
                target.SendLocalizedMessage(1008124); // "You drink a healing draught."

                // Healing over time
                int healAmount = Utility.RandomMinMax(10, 20); // Initial heal amount
                int totalHeal = Utility.RandomMinMax(30, 50); // Total healing over time

                target.Hits += healAmount;
                target.FixedParticles(0x376A, 1, 62, 9922, 1153, 3, EffectLayer.Waist);
                target.PlaySound(0x1F2); // Healing sound

                Timer healTimer = new HealTimer(target, healAmount, totalHeal);
                healTimer.Start();
            }

            FinishSequence();
        }

        private class HealTimer : Timer
        {
            private Mobile m_Target;
            private int m_HealAmount;
            private int m_TotalHeal;
            private int m_HealCount;

            public HealTimer(Mobile target, int healAmount, int totalHeal) : base(TimeSpan.FromSeconds(1.0), TimeSpan.FromSeconds(1.0))
            {
                m_Target = target;
                m_HealAmount = healAmount;
                m_TotalHeal = totalHeal;
                m_HealCount = 0;
            }

            protected override void OnTick()
            {
                if (m_Target == null || m_Target.Deleted || !m_Target.Alive)
                {
                    Stop();
                    return;
                }

                m_HealCount += m_HealAmount;
                m_Target.Hits += m_HealAmount;
                m_Target.FixedParticles(0x376A, 1, 62, 9922, 1153, 3, EffectLayer.Waist);

                if (m_HealCount >= m_TotalHeal)
                {
                    Stop();
                }
            }
        }
    }
}
