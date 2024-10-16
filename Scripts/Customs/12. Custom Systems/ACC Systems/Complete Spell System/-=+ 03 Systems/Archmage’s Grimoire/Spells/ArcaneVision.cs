using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.MageryMagic
{
    public class ArcaneVision : MagerySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Arcane Vision", "Revelatio Creaturae",
            21004,
            9300,
            false,
            Reagent.Nightshade,
            Reagent.SpidersSilk,
            Reagent.SulfurousAsh
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fifth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 60.0; } }
        public override int RequiredMana { get { return 15; } }

        public ArcaneVision(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.FixedParticles(0x374A, 10, 30, 5054, EffectLayer.Head); // Particle effect around the caster
                Caster.PlaySound(0x1E9); // Sound effect

                Map map = Caster.Map;

                if (map != null)
                {
                    double skill = Caster.Skills[CastSkill].Value;
                    int range = (int)(10 + skill / 10); // The range increases with the caster's skill

                    foreach (Mobile m in Caster.GetMobilesInRange(range))
                    {
                        if (m != null && m.Hidden && m != Caster && Caster.CanSee(m))
                        {
                            m.RevealingAction();
                            m.FixedParticles(0x375A, 9, 20, 5049, EffectLayer.Waist); // Particle effect on revealed creature
                            m.PlaySound(0x1F2); // Sound effect on revealed creature
                            Caster.SendMessage(0x44, "You reveal a hidden creature!"); // Message to caster
                        }
                    }

                    Caster.SendMessage(0x44, "Your Arcane Vision reveals all hidden creatures in the area!"); // Message to caster
                }
            }

            FinishSequence();
        }
    }
}
