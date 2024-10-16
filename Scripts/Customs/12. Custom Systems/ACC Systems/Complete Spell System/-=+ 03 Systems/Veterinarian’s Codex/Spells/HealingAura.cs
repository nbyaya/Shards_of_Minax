using System;
using System.Collections;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;
using Server.Items;

namespace Server.ACC.CSS.Systems.VeterinaryMagic
{
    public class HealingAura : VeterinarySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Healing Aura", "Sanare Aura",
            21005,
            9301,
            false,
            Reagent.Ginseng,
            Reagent.Garlic,
            Reagent.SpidersSilk
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Third; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 20; } }

        public HealingAura(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                // Apply the healing aura effect
                Caster.FixedParticles(0x375A, 1, 15, 5021, 1153, 2, EffectLayer.Waist);
                Caster.PlaySound(0x1F2);

                AuraEffect effect = new AuraEffect(Caster);
                effect.Start();
            }

            FinishSequence();
        }

        private class AuraEffect
        {
            private Mobile m_Caster;
            private Timer m_Timer;

            public AuraEffect(Mobile caster)
            {
                m_Caster = caster;
            }

            public void Start()
            {
                m_Timer = new AuraTimer(m_Caster);
                m_Timer.Start();
            }

            private class AuraTimer : Timer
            {
                private Mobile m_Caster;
                private DateTime m_EndTime;

                public AuraTimer(Mobile caster) : base(TimeSpan.Zero, TimeSpan.FromSeconds(2.0))
                {
                    m_Caster = caster;
                    m_EndTime = DateTime.Now + TimeSpan.FromSeconds(30.0); // Duration of the healing aura
                }

                protected override void OnTick()
                {
                    if (m_Caster == null || m_Caster.Deleted || DateTime.Now >= m_EndTime)
                    {
                        Stop();
                        return;
                    }

                    // Healing logic
                    foreach (Mobile m in m_Caster.GetMobilesInRange(5)) // Range of the aura
                    {
                        if (m is BaseCreature && ((BaseCreature)m).Controlled && m.Alive && m != m_Caster)
                        {
                            m.FixedParticles(0x376A, 1, 15, 9909, 1153, 2, EffectLayer.Waist);
                            m.PlaySound(0x202);
                            m.Hits += 5 + (int)(m_Caster.Skills[SkillName.Veterinary].Value / 10); // Heal amount
                        }
                    }

                    // Heal the caster as well
                    m_Caster.Hits += 5 + (int)(m_Caster.Skills[SkillName.Veterinary].Value / 10);
                }
            }
        }
    }
}
