using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Items;
using Server.Misc;
using System.Collections;

namespace Server.ACC.CSS.Systems.ParryMagic
{
    public class EnhancedGuard : ParrySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Enhanced Guard", "Guardia Vitalis",
            21005, // Icon ID
            9301 // Sound ID
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Sixth; }
        }

        public override double CastDelay { get { return 0.2; } } // Cast delay in seconds
        public override double RequiredSkill { get { return 50.0; } } // Required skill level
        public override int RequiredMana { get { return 20; } } // Mana cost

        public EnhancedGuard(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.SendMessage("You feel a surge of vitality as you prepare to defend yourself!");

                // Visual effect on caster
                Caster.FixedParticles(0x376A, 9, 32, 5008, EffectLayer.Waist);
                Caster.PlaySound(0x211); // Play casting sound

                // Apply stamina regeneration buff
                BuffTimer buffTimer = new BuffTimer(Caster);
                buffTimer.Start();
            }

            FinishSequence();
        }

        private class BuffTimer : Timer
        {
            private Mobile m_Caster;
            private DateTime m_End;
            private int m_StaminaPerTick = 5; // Amount of stamina restored per tick

            public BuffTimer(Mobile caster) : base(TimeSpan.FromSeconds(1.0), TimeSpan.FromSeconds(1.0))
            {
                m_Caster = caster;
                m_End = DateTime.Now + TimeSpan.FromSeconds(30.0); // Buff duration
            }

            protected override void OnTick()
            {
                if (m_Caster == null || m_Caster.Deleted || !m_Caster.Alive)
                {
                    Stop();
                    return;
                }

                if (DateTime.Now >= m_End)
                {
                    m_Caster.SendMessage("The effects of Enhanced Guard wear off.");
                    Stop();
                }
                else
                {
                    m_Caster.Stam += m_StaminaPerTick; // Restore stamina
                    m_Caster.FixedEffect(0x376A, 10, 16); // Visual effect each tick
                }
            }
        }
    }
}
