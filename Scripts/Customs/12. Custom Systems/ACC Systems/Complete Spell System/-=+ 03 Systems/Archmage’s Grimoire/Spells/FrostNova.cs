using System;
using System.Collections;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;

namespace Server.ACC.CSS.Systems.MageryMagic
{
    public class FrostNova : MagerySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Frost Nova", "Kal Vas Frost",
            //SpellCircle.Sixth,
            21004,
            9300,
            false,
            Reagent.BlackPearl,
            Reagent.SpidersSilk,
            Reagent.Nightshade
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Sixth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 70.0; } }
        public override int RequiredMana { get { return 25; } }

        public FrostNova(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.FixedParticles(0x3818, 10, 30, 5052, EffectLayer.Waist);
                Caster.PlaySound(0x64F);

                Map map = Caster.Map;
                if (map == null)
                    return;

                int range = 3; // Radius of effect
                ArrayList targets = new ArrayList();

                foreach (Mobile m in Caster.GetMobilesInRange(range))
                {
                    if (Caster != m && Caster.CanBeHarmful(m, false))
                    {
                        targets.Add(m);
                    }
                }

                for (int i = 0; i < targets.Count; ++i)
                {
                    Mobile m = (Mobile)targets[i];

                    double damage = Utility.RandomMinMax(20, 30);
                    Caster.DoHarmful(m);

                    m.FixedParticles(0x3818, 10, 30, 5052, EffectLayer.Waist);
                    m.PlaySound(0x64F);

                    SpellHelper.Damage(this, m, damage, 0, 0, 100, 0, 0); // Pure cold damage
                    m.SendMessage("You are frozen by the Frost Nova!");

                    // Apply freezing effect: reduce movement speed temporarily
                    m.Paralyze(TimeSpan.FromSeconds(2.0 + Utility.RandomDouble() * 3.0));
                }
            }

            FinishSequence();
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(2.5);
        }
    }
}
