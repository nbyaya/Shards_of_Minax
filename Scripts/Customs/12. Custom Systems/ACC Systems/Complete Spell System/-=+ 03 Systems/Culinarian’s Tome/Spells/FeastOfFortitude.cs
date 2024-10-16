using System;
using Server.Mobiles;
using Server.Network;
using Server.Items;
using Server.Spells;
using Server.Targeting;

namespace Server.ACC.CSS.Systems.CookingMagic
{
    public class FeastOfFortitude : CookingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Feast of Fortitude", "Ego Fortis",
            21004,
            9300
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; } // Adjust based on your system's requirements
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 30; } }

        public FeastOfFortitude(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.FixedParticles(0x373A, 10, 15, 5018, EffectLayer.Waist); // Visual effect: swirling energy around the waist
                Caster.PlaySound(0x1F7); // Sound effect: feast or celebration

                BuffTimer buffTimer = new BuffTimer(Caster);
                buffTimer.Start();
                
                Caster.SendMessage("You feel a surge of strength as you feast!");
            }

            FinishSequence();
        }

        private class BuffTimer : Timer
        {
            private Mobile m_Caster;

            public BuffTimer(Mobile caster) : base(TimeSpan.FromSeconds(0), TimeSpan.FromMinutes(30)) // Duration of 30 minutes
            {
                m_Caster = caster;
                Priority = TimerPriority.OneSecond;

                // Apply initial strength boost
                m_Caster.Str += 10; // Temporary strength boost
                m_Caster.SendMessage("Your strength has increased!");
            }

            protected override void OnTick()
            {
                Stop();
                m_Caster.Str -= 10; // Remove the temporary strength boost
                m_Caster.SendMessage("The effects of the feast have worn off.");
            }
        }
    }
}
