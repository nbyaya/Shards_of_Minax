using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Items;
using System.Collections;

namespace Server.ACC.CSS.Systems.FletchingMagic
{
    public class EagleEye : FletchingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Eagle Eye", "Accuro!",
            // SpellCircle.Second,
            21005,
            9400
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Second; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 30.0; } }
        public override int RequiredMana { get { return 10; } }

        public EagleEye(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.SendMessage("Your eyes sharpen, and you focus intensely on your targets.");
                Caster.FixedParticles(0x373A, 1, 15, 9909, EffectLayer.Head);
                Caster.PlaySound(0x2E6);

                // Apply the effect to the caster
                Caster.BeginAction(typeof(EagleEye));
                Timer.DelayCall(TimeSpan.FromSeconds(20.0), new TimerStateCallback(EndEffect), Caster);
                Caster.AddStatMod(new StatMod(StatType.Dex, "EagleEyeDex", 10, TimeSpan.FromSeconds(20.0)));

                // Buff the archery skill temporarily
                SkillMod mod = new DefaultSkillMod(SkillName.Archery, true, 20.0);
                Caster.AddSkillMod(mod);

                Timer.DelayCall(TimeSpan.FromSeconds(20.0), () =>
                {
                    Caster.RemoveSkillMod(mod);
                    Caster.SendMessage("Your keen focus fades.");
                    Caster.FixedParticles(0x3735, 1, 15, 9502, EffectLayer.Waist);
                    Caster.PlaySound(0x1F8);
                });

                Caster.FixedEffect(0x376A, 10, 16);
            }

            FinishSequence();
        }

        private static void EndEffect(object state)
        {
            Mobile caster = (Mobile)state;
            caster.EndAction(typeof(EagleEye));
        }
    }
}
