using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Items;
using System.Collections.Generic;

namespace Server.ACC.CSS.Systems.ChivalryMagic
{
    public class BlessedShield : ChivalrySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Blessed Shield", "Defensio Sacra",
            21002,
            9300
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 25; } }

        public BlessedShield(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                // Play sound and visual effects
                Caster.PlaySound(0x5C);
                Caster.FixedParticles(0x373A, 10, 15, 5018, EffectLayer.Waist);

                // Apply Blessed Shield effect
                Caster.SendMessage("You are blessed with a protective shield!");

                int duration = (int)(Caster.Skills[SkillName.Chivalry].Value / 10.0) + 10; // Duration based on skill level, minimum 10 seconds
                int armorBonus = 10 + (int)(Caster.Skills[SkillName.Chivalry].Value / 20.0); // Armor bonus based on skill level

                // Apply armor bonus
                Caster.AddStatMod(new StatMod(StatType.Str, "BlessedShield", armorBonus, TimeSpan.FromSeconds(duration)));

                Timer timer = new BlessedShieldTimer(Caster, duration, armorBonus);
                timer.Start();

                FinishSequence();
            }
        }

        private class BlessedShieldTimer : Timer
        {
            private Mobile m_Caster;
            private DateTime m_End;
            private int m_ArmorBonus;

            public BlessedShieldTimer(Mobile caster, int duration, int armorBonus) : base(TimeSpan.FromSeconds(duration))
            {
                m_Caster = caster;
                m_End = DateTime.Now + TimeSpan.FromSeconds(duration);
                m_ArmorBonus = armorBonus;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (DateTime.Now >= m_End)
                {
                    m_Caster.SendMessage("The blessed shield has worn off.");
                    m_Caster.RemoveStatMod("BlessedShield");
                    Stop();
                }
                else
                {
                    if (Utility.RandomDouble() < 0.25) // 25% chance to block all damage
                    {
                        m_Caster.SendMessage("Your blessed shield blocks all incoming damage!");
                        m_Caster.FixedParticles(0x376A, 1, 15, 9909, EffectLayer.Waist);
                        m_Caster.PlaySound(0x1F2);
                    }
                }
            }
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(5.0);
        }
    }
}
