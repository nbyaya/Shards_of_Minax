using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;
using Server.Items;
using System.Collections.Generic;

namespace Server.ACC.CSS.Systems.TailoringMagic
{
    public class SilkStep : TailoringSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Silk Step", "Mov Fux",
            // SpellCircle.First,
            21014,
            9300,
            false
        );

        public override SpellCircle Circle { get { return SpellCircle.First; } }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 20.0; } }
        public override int RequiredMana { get { return 20; } }

        public SilkStep(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.SendMessage("You begin to move with the grace of a spider, becoming nearly invisible.");
                Caster.Hidden = true;
                Caster.Blessed = true;

                // Apply visual and sound effects
                Effects.SendLocationParticles(EffectItem.Create(Caster.Location, Caster.Map, EffectItem.DefaultDuration), 0x376A, 1, 29, 1150, 7, 9502, 0);
                Caster.PlaySound(0x1FE);

                // Timer to end the effect after a short duration
                Timer.DelayCall(TimeSpan.FromSeconds(10), EndSilkStep);
            }

            FinishSequence();
        }

        private void EndSilkStep()
        {
            Caster.Hidden = false;
            Caster.Blessed = false;
            Caster.SendMessage("The effects of Silk Step wear off, and you return to normal.");
            Effects.SendLocationParticles(EffectItem.Create(Caster.Location, Caster.Map, EffectItem.DefaultDuration), 0x376A, 1, 29, 1150, 7, 9502, 0);
            Caster.PlaySound(0x1FE);
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(2.0);
        }
    }
}
