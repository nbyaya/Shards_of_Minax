using System;
using System.Collections;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;

namespace Server.ACC.CSS.Systems.ProvocationMagic
{
    public class ConfoundingAura : ProvocationSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Confounding Aura", "Ignis Confusus",
            21005,
            9400
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 20; } }

        public ConfoundingAura(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.PlaySound(0x1E3); // Aura activation sound
                Caster.FixedParticles(0x375A, 1, 30, 9944, 3, 3, EffectLayer.Waist); // Aura visual effect

                new ConfoundingAuraEffect(Caster).Start(); // Start the effect

                FinishSequence();
            }
        }

        private class ConfoundingAuraEffect : Timer
        {
            private Mobile m_Caster;

            public ConfoundingAuraEffect(Mobile caster) : base(TimeSpan.Zero, TimeSpan.FromSeconds(1.0))
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

                ArrayList targets = new ArrayList();

                foreach (Mobile m in m_Caster.GetMobilesInRange(5)) // Affects enemies in a 5-tile radius
                {
                    if (m != m_Caster && m.Alive && m.AccessLevel == AccessLevel.Player && m.Criminal && m.CanBeHarmful(m_Caster))
                    {
                        targets.Add(m);
                    }
                }

                foreach (Mobile m in targets)
                {
                    m.CantWalk = true; // Simulate confusion by making enemies temporarily unable to move
                    m.SendMessage("You feel disoriented and confused!");

                    m.PlaySound(0x213); // Confusion sound effect
                    m.FixedParticles(0x374A, 10, 30, 5024, 3, 3, EffectLayer.Head); // Confusion visual effect

                    // Reduce accuracy and evasiveness by affecting skills
                    m.Skills[SkillName.Wrestling].Base -= 10; // Example for reducing wrestling skill (used as a proxy for accuracy)
                    m.Skills[SkillName.Parry].Base -= 10; // Example for reducing parry skill (used as a proxy for evasion)

                    Timer.DelayCall(TimeSpan.FromSeconds(5), delegate // Revert effects after 5 seconds
                    {
                        if (m != null && !m.Deleted && m.Alive)
                        {
                            m.CantWalk = false;
                            m.Skills[SkillName.Wrestling].Base += 10; // Revert wrestling skill
                            m.Skills[SkillName.Parry].Base += 10; // Revert parry skill
                            m.SendMessage("You regain your senses.");
                        }
                    });
                }
            }
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(2.0);
        }
    }
}
