using System;
using System.Collections;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;  // Make sure to include this for BuffInfo and related classes

namespace Server.ACC.CSS.Systems.TrackingMagic
{
    public class DisorientingShout : TrackingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Disorienting Shout", "Disori Shout",
            21004,
            9300
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fifth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 35; } }

        public DisorientingShout(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.SendMessage("You unleash a disorienting shout!");

                Effects.PlaySound(Caster.Location, Caster.Map, 0x2F4); // Sound effect for shout
                Effects.SendLocationParticles(Caster, 0x376A, 1, 32, 0, 0, 5022, 0); // Visual effect for shout

                ArrayList targets = new ArrayList();

                // Get mobiles in range
                foreach (Mobile m in Caster.GetMobilesInRange(5))
                {
                    if (Caster.CanBeHarmful(m, false) && Caster != m)
                        targets.Add(m);
                }

                for (int i = 0; i < targets.Count; ++i)
                {
                    Mobile m = (Mobile)targets[i];

                    Caster.DoHarmful(m);
                    m.SendMessage("You feel disoriented by the shout!");

                    m.Freeze(TimeSpan.FromSeconds(1.0)); // Short stun effect

                    m.PlaySound(0x1F9); // Sound effect for disoriented effect
                    m.FixedEffect(0x3779, 10, 16); // Visual effect on disoriented enemies

                    // Apply a negative hit chance effect (Disorient debuff)
                    BuffInfo.AddBuff(m, new BuffInfo(BuffIcon.Bless, 1075792, 1075793, TimeSpan.FromSeconds(10), m, true)); // Use a valid BuffIcon and set the boolean argument to true
                    m.Hits -= Utility.Random(5, 10); // Small health damage for added effect
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
