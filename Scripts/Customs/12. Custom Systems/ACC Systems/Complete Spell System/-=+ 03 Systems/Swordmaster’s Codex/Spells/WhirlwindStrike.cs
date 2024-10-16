using System;
using System.Collections.Generic;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.SwordsMagic
{
    public class WhirlwindStrike : SwordsSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Whirlwind Strike", "Whirl Max!",
            21003,
            9402
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Third; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 60.0; } }
        public override int RequiredMana { get { return 30; } }

        public WhirlwindStrike(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.PlaySound(0x1F2); // Sound effect for the Whirlwind Strike
                Caster.FixedParticles(0x3728, 10, 15, 9950, EffectLayer.Waist); // Visual effect

                List<Mobile> targets = new List<Mobile>();

                foreach (Mobile m in Caster.GetMobilesInRange(2)) // Range of 2 tiles for AoE effect
                {
                    if (Caster.CanBeHarmful(m, false) && m != Caster)
                    {
                        targets.Add(m);
                    }
                }

                foreach (Mobile m in targets)
                {
                    Caster.DoHarmful(m);
                    m.Damage(Utility.RandomMinMax(15, 30), Caster); // Random damage between 15 and 30
                    m.FixedParticles(0x374A, 10, 15, 5030, EffectLayer.Head); // Individual target visual effect
                    m.PlaySound(0x1F2); // Sound effect on each target
                }
            }

            FinishSequence();
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(1.5); // Delay before the skill can be cast again
        }
    }
}
