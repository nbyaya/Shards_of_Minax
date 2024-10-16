using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using System.Collections;

namespace Server.ACC.CSS.Systems.HidingMagic
{
    public class Camouflage : HidingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Camouflage", "Blendu Wendus",
                                                        21006, 9205
                                                       );

        public override SpellCircle Circle { get { return SpellCircle.Third; } }
		public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 30.0; } } // Adjust skill requirements as needed
        public override int RequiredMana { get { return 20; } }

        public Camouflage(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (Caster.Hidden || !CheckSequence())
            {
                Caster.SendMessage("You are already hidden or the spell failed.");
                return;
            }

            Caster.Hidden = true;
            Caster.SendMessage("You blend into your surroundings, becoming harder to spot.");
            Effects.PlaySound(Caster.Location, Caster.Map, 0x651); // Play sound effect for activation
            Caster.FixedParticles(0x376A, 10, 15, 5030, EffectLayer.Waist); // Visual effect around the caster

            // Start a timer to keep the caster hidden as long as they remain stationary
            new CamouflageTimer(Caster).Start();

            FinishSequence();
        }

        private class CamouflageTimer : Timer
        {
            private Mobile m_Caster;
            private Point3D m_LastLocation;

            public CamouflageTimer(Mobile caster) : base(TimeSpan.FromSeconds(1.0), TimeSpan.FromSeconds(1.0))
            {
                m_Caster = caster;
                m_LastLocation = caster.Location;
            }

            protected override void OnTick()
            {
                // If the caster has moved, cancel the camouflage effect
                if (m_Caster.Deleted || !m_Caster.Hidden || m_Caster.Location != m_LastLocation)
                {
                    m_Caster.Hidden = false;
                    m_Caster.SendMessage("You are no longer camouflaged.");
                    Effects.PlaySound(m_Caster.Location, m_Caster.Map, 0x653); // Play sound effect when camouflage ends
                    Stop();
                }
            }
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(3.0); // Adjust cast delay as needed
        }
    }
}
