using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.ArcheryMagic
{
    public class EagleEye : ArcherySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Eagle Eye", "Oculi Aquilae",
            21005,
            9301,
            false
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Second; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 30.0; } }
        public override int RequiredMana { get { return 15; } }

        public EagleEye(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.FixedParticles(0x373A, 10, 15, 5018, EffectLayer.Head); // Visual effect around the caster's head
                Caster.PlaySound(0x1E2); // Sound effect for casting

                double duration = 10.0 + (Caster.Skills[CastSkill].Value / 5.0); // Duration of the effect based on skill
                double dexBonus = 10.0 + (Caster.Skills[CastSkill].Value / 10.0); // Dexterity bonus

                Caster.SendMessage("Your vision sharpens, and you feel more accurate!");
                Caster.AddStatMod(new StatMod(StatType.Dex, "EagleEyeDex", (int)dexBonus, TimeSpan.FromSeconds(duration)));

                Timer.DelayCall(TimeSpan.FromSeconds(duration), () =>
                {
                    Caster.SendMessage("The effect of Eagle Eye wears off.");
                    Caster.PlaySound(0x1E3); // Sound effect when the effect wears off
                });
            }

            FinishSequence();
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(5.0);
        }
    }
}
