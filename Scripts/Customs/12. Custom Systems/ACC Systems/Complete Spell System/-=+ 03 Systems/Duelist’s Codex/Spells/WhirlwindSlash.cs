using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;
using Server.Items;
using System.Collections;

namespace Server.ACC.CSS.Systems.FencingMagic
{
    public class WhirlwindSlash : FencingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Whirlwind Slash", "Spin Slash!",
                                                        //SpellCircle.Fifth,
                                                        21005,
                                                        9301
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fifth; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 60.0; } }
        public override int RequiredMana { get { return 20; } }

        public WhirlwindSlash(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.PlaySound(0x208); // Spinning slash sound
                Caster.FixedParticles(0x3779, 1, 15, 9902, 0, 0, EffectLayer.Waist); // Visual effect

                ArrayList targets = new ArrayList();

                foreach (Mobile m in Caster.GetMobilesInRange(2))
                {
                    if (m != Caster && SpellHelper.ValidIndirectTarget(Caster, m) && Caster.CanBeHarmful(m, false))
                    {
                        targets.Add(m);
                    }
                }

                for (int i = 0; i < targets.Count; ++i)
                {
                    Mobile m = (Mobile)targets[i];

                    Caster.DoHarmful(m);
                    SpellHelper.Damage(this, m, Utility.RandomMinMax(30, 50)); // Heavy damage

                    m.PlaySound(0x1FB); // Slash impact sound
                    m.FixedParticles(0x36BD, 20, 10, 5044, EffectLayer.Head); // Blood splash effect
                }
            }

            FinishSequence();
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(1.0); // Quick casting time to match the fast-paced nature of the skill
        }
    }
}
