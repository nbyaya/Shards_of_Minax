using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;

namespace Server.ACC.CSS.Systems.ArmsLoreMagic
{
    public class WeaponEnhancement : ArmsLoreSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Weapon Enhancement", // Name of the spell
            "Temporarily adds bonus strength.", // Description
            // Spell circle, effect graphics, and other properties
            21004, 
            9300 
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Sixth; }
        }

        public override double CastDelay => 0.1;
        public override double RequiredSkill => 20.0;
        public override int RequiredMana => 25;

        public WeaponEnhancement(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                // Display visual effects
                Effects.PlaySound(Caster.Location, Caster.Map, 0x1F2); // Play a magical sound effect
                Effects.SendLocationEffect(Caster.Location, Caster.Map, 0x376A, 10, 4); // Play a magical visual effect

                // Apply the strength bonus
                Caster.SendMessage("You feel a surge of strength!");
                Caster.Str += 10; // Example bonus, adjust as needed

                // Set up a timer to remove the bonus after a certain duration
                Timer timer = new InternalTimer(Caster, TimeSpan.FromSeconds(30.0));
                timer.Start();
            }

            FinishSequence();
        }

        private class InternalTimer : Timer
        {
            private Mobile m_Caster;

            public InternalTimer(Mobile caster, TimeSpan duration) : base(duration)
            {
                m_Caster = caster;
            }

            protected override void OnTick()
            {
                if (m_Caster != null && !m_Caster.Deleted)
                {
                    m_Caster.SendMessage("The enhancement on your strength fades away.");
                    m_Caster.Str -= 10; // Remove the bonus
                }
            }
        }
    }
}
