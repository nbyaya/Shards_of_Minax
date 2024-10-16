using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Items;
using System.Collections;

namespace Server.ACC.CSS.Systems.ParryMagic
{
    public class ReflectiveShield : ParrySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Reflective Shield", "In Sanct Lor",
            21005, // Gump ID for the spell icon
            9301 // Icon ID
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Sixth; }
        }

        public override int RequiredMana { get { return 25; } }
        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 60.0; } }

        public ReflectiveShield(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.FixedParticles(0x375A, 1, 15, 9909, EffectLayer.Waist); // Visual effect around the caster
                Caster.PlaySound(0x28E); // Sound effect

                Timer timer = new ReflectiveShieldTimer(Caster);
                timer.Start();
            }

            FinishSequence();
        }

        private class ReflectiveShieldTimer : Timer
        {
            private Mobile m_Caster;
            private int m_Ticks = 20; // Number of ticks (20 seconds)

            public ReflectiveShieldTimer(Mobile caster) : base(TimeSpan.FromSeconds(1.0), TimeSpan.FromSeconds(1.0))
            {
                m_Caster = caster;
            }

            protected override void OnTick()
            {
                if (m_Ticks-- > 0 && !m_Caster.Deleted && m_Caster.Alive)
                {
                    ArrayList targets = new ArrayList();

                    // Get all mobiles within 1 tile of the caster
                    foreach (Mobile m in m_Caster.GetMobilesInRange(1))
                    {
                        if (m != m_Caster && m is BaseCreature && m.Alive && m_Caster.CanBeHarmful(m, false))
                        {
                            targets.Add(m);
                        }
                    }

                    for (int i = 0; i < targets.Count; ++i)
                    {
                        Mobile m = (Mobile)targets[i];

                        // Deal medium damage
                        int damage = Utility.RandomMinMax(10, 20);
                        m_Caster.DoHarmful(m);
                        AOS.Damage(m, m_Caster, damage, 100, 0, 0, 0, 0);

                        // Visual effect and sound on target
                        m.FixedParticles(0x374A, 10, 15, 5038, EffectLayer.Waist);
                        m.PlaySound(0x208);
                    }
                }
                else
                {
                    Stop();
                }
            }
        }
    }
}
