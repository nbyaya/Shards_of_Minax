using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;

namespace Server.ACC.CSS.Systems.HealingMagic
{
    public class HealingAura : HealingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Healing Aura", "Salus Sanctus",
            21004, 9300,
            Reagent.Garlic, Reagent.Ginseng, Reagent.SpidersSilk
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 60.0; } }
        public override int RequiredMana { get { return 30; } }

        private const int HealIntervalSeconds = 5; // Heal every 5 seconds
        private const int HealRange = 5; // Range of 5 tiles
        private const int HealAmount = 10; // Heal 10 hit points each tick
        private Timer m_Timer;

        public HealingAura(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.SendMessage("You invoke a healing aura.");
                Effects.PlaySound(Caster.Location, Caster.Map, 0x1F2);
                Caster.FixedParticles(0x375A, 10, 15, 5010, EffectLayer.Waist);

                // Start the healing aura
                m_Timer = new HealTimer(Caster, HealIntervalSeconds, HealRange, HealAmount);
                m_Timer.Start();
            }

            FinishSequence();
        }

        private class HealTimer : Timer
        {
            private Mobile m_Caster;
            private int m_Range;
            private int m_HealAmount;

            public HealTimer(Mobile caster, int intervalSeconds, int range, int healAmount)
                : base(TimeSpan.FromSeconds(intervalSeconds), TimeSpan.FromSeconds(intervalSeconds))
            {
                m_Caster = caster;
                m_Range = range;
                m_HealAmount = healAmount;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Caster == null || m_Caster.Deleted || !m_Caster.Alive)
                {
                    Stop();
                    return;
                }

                List<Mobile> alliesInRange = new List<Mobile>();

                foreach (Mobile m in m_Caster.GetMobilesInRange(m_Range))
                {
                    if (m is PlayerMobile && m.Alive && m.Karma >= 0 && m != m_Caster && m.Criminal == false)
                    {
                        alliesInRange.Add(m);
                    }
                }

                foreach (Mobile ally in alliesInRange)
                {
                    int toHeal = Math.Min(ally.HitsMax - ally.Hits, m_HealAmount);
                    if (toHeal > 0)
                    {
                        ally.Hits += toHeal;
                        ally.SendMessage("You are healed by the aura!");
                        ally.FixedParticles(0x376A, 9, 32, 5008, EffectLayer.Waist);
                        Effects.PlaySound(ally.Location, ally.Map, 0x1F2);
                    }
                }
            }
        }

        public override void OnDisturb(DisturbType type, bool message)
        {
            if (m_Timer != null)
            {
                m_Timer.Stop();
                m_Timer = null;
            }

            base.OnDisturb(type, message);
        }
    }
}
