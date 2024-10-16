using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;
using System.Collections;

namespace Server.ACC.CSS.Systems.TasteIDMagic
{
    public class SpicyRetaliation : TasteIDSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Spicy Retaliation", "Spico Vex",
                                                        //SpellCircle.Fourth,
                                                        21007,
                                                        9303
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 60.0; } }
        public override int RequiredMana { get { return 12; } }

        public SpicyRetaliation(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.Waist);
                Caster.PlaySound(0x208);

                ArrayList targets = new ArrayList();

                foreach (Mobile m in Caster.GetMobilesInRange(3))
                {
                    if (Caster.CanBeHarmful(m, false) && Caster != m)
                    {
                        targets.Add(m);
                    }
                }

                for (int i = 0; i < targets.Count; ++i)
                {
                    Mobile m = (Mobile)targets[i];

                    double damage = Utility.RandomMinMax(10, 20); // Random damage between 10 and 20
                    Caster.DoHarmful(m);
                    m.Damage((int)damage, Caster);

                    m.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.Waist);
                    m.PlaySound(0x208);
                }
            }

            FinishSequence();
        }
    }
}
