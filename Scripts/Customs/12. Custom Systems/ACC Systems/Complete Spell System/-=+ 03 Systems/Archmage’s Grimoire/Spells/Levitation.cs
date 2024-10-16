using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using System.Collections;

namespace Server.ACC.CSS.Systems.MageryMagic
{
    public class Levitation : MagerySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Levitation", "Uus Por Ort",
            //SpellCircle.Sixth,
            21004,
            9300,
            false,
            Reagent.BlackPearl,
            Reagent.MandrakeRoot
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Sixth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 75.0; } }
        public override int RequiredMana { get { return 25; } }

        public Levitation(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.SendMessage("You feel light as a feather!");
                Caster.FixedParticles(0x373A, 10, 15, 5018, EffectLayer.Waist);
                Caster.PlaySound(0x213);

                Caster.BeginAction(typeof(Levitation));

                Timer.DelayCall(TimeSpan.FromSeconds(10.0), () => RemoveLevitationEffect(Caster));

                Caster.Hidden = true; // Optional: Makes the caster invisible during levitation
                Caster.Blessed = true; // Grants 100% physical immunity
            }

            FinishSequence();
        }

        private void RemoveLevitationEffect(Mobile caster)
        {
            caster.EndAction(typeof(Levitation));

            caster.Blessed = false;
            caster.Hidden = false; // Make the caster visible again if invisible

            caster.SendMessage("You feel your weight return.");
            caster.FixedParticles(0x373A, 10, 15, 5018, EffectLayer.Waist);
            caster.PlaySound(0x213);
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(5.0);
        }
    }
}
