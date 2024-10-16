using System;
using System.Collections;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server;

namespace Server.ACC.CSS.Systems.StealthMagic
{
    public class CloakOfShadows : StealthSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Cloak of Shadows", "Umbra Obscura",
                                                        21003,
                                                        9202
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 30; } }

        public CloakOfShadows(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                // Play a shadowy sound effect at the caster's location
                Caster.PlaySound(0x58D);
                
                Caster.FixedParticles(0x374A, 10, 15, 5038, EffectLayer.Waist); // Dark swirl effect around caster

                // Apply the darkness effect
                Effects.SendLocationEffect(Caster.Location, Caster.Map, 0x376A, 20, 10, 1109, 0); // Create a field of darkness

                // Set a timer for the duration of the effect (30 seconds)
                new DarknessTimer(Caster).Start();
            }

            FinishSequence();
        }

        private class DarknessTimer : Timer
        {
            private Mobile m_Caster;

            public DarknessTimer(Mobile caster) : base(TimeSpan.FromSeconds(30.0))
            {
                m_Caster = caster;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Caster == null || m_Caster.Deleted)
                    return;

                // Check for enemies within a radius of 5 tiles
                ArrayList enemies = new ArrayList();
                foreach (Mobile m in m_Caster.GetMobilesInRange(5))
                {
                    if (m != m_Caster && m.Alive && m.CanSee(m_Caster) && !m.Hidden && m.Player)
                    {
                        enemies.Add(m);
                    }
                }

                // Apply a visual effect and/or message
                foreach (Mobile enemy in enemies)
                {
                    // Example of applying a visual effect
                    enemy.PlaySound(0x24D); // Play a sound to signify the effect

                    // Inform the enemy
                    enemy.SendMessage("Your vision is clouded by shadows!");
                }
            }
        }
    }
}
