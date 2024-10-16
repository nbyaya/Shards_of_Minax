using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using System.Collections.Generic;

namespace Server.ACC.CSS.Systems.ChivalryMagic
{
    public class HolyLight : ChivalrySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Holy Light", "Lux Divina",
            21003,
            9300
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 30; } }

        public HolyLight(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.SendMessage("You unleash a burst of holy light!");

                Effects.PlaySound(Caster.Location, Caster.Map, 0x1E0); // Holy light sound
                Effects.SendLocationParticles(Caster, 0x376A, 9, 32, 1153, 7, 9909, 0); // Holy light visual

                int range = 5; // Radius of effect

                List<Mobile> targets = new List<Mobile>();

                foreach (Mobile m in Caster.GetMobilesInRange(range))
                {
                    if (Caster.CanBeHarmful(m, false))
                    {
                        // Include all BaseCreatures and players in the target list
                        if (m is BaseCreature || m.Player)
                        {
                            targets.Add(m);
                        }
                    }
                }

                foreach (Mobile m in targets)
                {
                    if (m is BaseCreature)
                    {
                        // Apply damage to creatures
                        Caster.DoHarmful(m);
                        m.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.CenterFeet);
                        m.PlaySound(0x208);
                        m.Damage(Utility.RandomMinMax(20, 40), Caster); // Damage to creatures
                    }
                    else if (m.Player && m.Karma >= 0 && m.Alive)
                    {
                        // Heal players
                        m.FixedParticles(0x376A, 9, 32, 5005, EffectLayer.Waist);
                        m.PlaySound(0x1F2);
                        m.Hits += Utility.RandomMinMax(10, 20); // Heal players
                    }
                }
            }

            FinishSequence();
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(5.0);
        }
    }
}
