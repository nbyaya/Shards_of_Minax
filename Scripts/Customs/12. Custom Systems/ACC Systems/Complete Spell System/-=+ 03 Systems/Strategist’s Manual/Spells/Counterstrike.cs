using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.TacticsMagic
{
    public class Counterstrike : TacticsSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Counterstrike", "Per Fiducia!",
            21004, // GFX
            9300,  // Sound
            false, // No targeting is required for this spell
            Reagent.BlackPearl, 
            Reagent.MandrakeRoot
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; }
        }

        public override double CastDelay { get { return 0.5; } } // Quick cast
        public override double RequiredSkill { get { return 50.0; } } // Moderate skill level required
        public override int RequiredMana { get { return 15; } } // Mana cost as per description

        public Counterstrike(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.SendMessage("You prepare to counter the next incoming attack...");

                // Apply the Counterstrike effect to the caster for a short duration
                CounterstrikeEffect effect = new CounterstrikeEffect(Caster);
                effect.Start();

                FinishSequence();
            }
        }

        // Custom effect class to handle the counterstrike
        private class CounterstrikeEffect
        {
            private Mobile m_Caster;
            private Timer m_Timer;
            private DateTime m_End;
            private bool m_Active;

            public CounterstrikeEffect(Mobile caster)
            {
                m_Caster = caster;
                m_End = DateTime.Now + TimeSpan.FromSeconds(5.0); // Effect lasts for 5 seconds
                m_Active = true;

                m_Timer = new InternalTimer(this);
                m_Timer.Start();
            }

            public void Start()
            {
                m_Caster.FixedParticles(0x376A, 1, 14, 9904, 97, 3, EffectLayer.Waist); // Visual effect
                m_Caster.PlaySound(0x213); // Sound effect
            }

            public void OnDamageTaken(Mobile attacker, int damage)
            {
                if (!m_Active || DateTime.Now > m_End)
                {
                    End();
                    return;
                }

                if (m_Caster == null || attacker == null || !attacker.Alive || !m_Caster.Alive)
                    return;

                // Counterattack logic
                double counterDamage = Utility.RandomMinMax(10, 20); // Damage range for the counterstrike

                // Directly use the Damage method to apply damage
                if (attacker is BaseCreature baseCreature)
                {
                    baseCreature.Damage((int)counterDamage, m_Caster);
                }
                else
                {
                    attacker.Damage((int)counterDamage, m_Caster);
                }

                attacker.FixedParticles(0x36BD, 20, 10, 5044, EffectLayer.Head); // Visual effect on attacker
                attacker.PlaySound(0x28E); // Sound effect on attacker

                m_Caster.SendMessage("You counterstrike your attacker with a precise blow!");
                attacker.SendMessage("You have been countered with a precise strike!");

                End();
            }

            public void End()
            {
                m_Active = false;
                m_Timer.Stop();
            }

            private class InternalTimer : Timer
            {
                private CounterstrikeEffect m_Effect;

                public InternalTimer(CounterstrikeEffect effect) : base(TimeSpan.FromSeconds(0.1), TimeSpan.FromSeconds(0.1))
                {
                    m_Effect = effect;
                    Priority = TimerPriority.TenMS;
                }

                protected override void OnTick()
                {
                    if (DateTime.Now > m_Effect.m_End)
                        m_Effect.End();
                }
            }
        }
    }
}
