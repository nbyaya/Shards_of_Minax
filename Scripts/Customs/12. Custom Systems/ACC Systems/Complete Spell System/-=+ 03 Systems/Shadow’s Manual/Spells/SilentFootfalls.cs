using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;
using Server.Misc;
using Server.Items;
using System.Collections.Generic;

namespace Server.ACC.CSS.Systems.HidingMagic
{
    public class SilentFootfalls : HidingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Silent Footfalls", "Rupus Verto",
            //SpellCircle.First,
            21003,
            9202,
            false
        );

        public override SpellCircle Circle { get { return SpellCircle.First; } }
        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 20.0; } } // Example skill requirement
        public override int RequiredMana { get { return 15; } }

        public SilentFootfalls(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.SendMessage("You feel your steps becoming lighter and quieter...");
                Caster.FixedParticles(0x376A, 9, 32, 5008, EffectLayer.Waist); // Some magical effect
                Caster.PlaySound(0x1FD); // Sound effect for stealth

                // Apply the stealth effect
                Caster.Hidden = true;
                Caster.BeginAction(typeof(SilentFootfalls));

                // Timer for the duration of the stealth
                Timer.DelayCall(TimeSpan.FromSeconds(10.0), new TimerStateCallback(EndSilentFootfalls), Caster);
            }

            FinishSequence();
        }

        private void EndSilentFootfalls(object state)
        {
            Mobile caster = (Mobile)state;

            if (caster == null || !caster.Alive)
                return;

            caster.Hidden = false;
            caster.EndAction(typeof(SilentFootfalls));
            caster.SendMessage("You can no longer move silently.");
            caster.PlaySound(0x1F8); // Sound effect for ending stealth

            Effects.SendLocationParticles(EffectItem.Create(caster.Location, caster.Map, EffectItem.DefaultDuration), 0x3728, 10, 10, 5023);
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(2.5);
        }
    }
}
