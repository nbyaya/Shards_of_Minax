using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Items;
using System.Collections;

namespace Server.ACC.CSS.Systems.TrackingMagic
{
    public class SilentFootsteps : TrackingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Silent Footsteps", "Quietus Tread",
                                                        21004,
                                                        9300
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 30.0; } }
        public override int RequiredMana { get { return 20; } }

        public SilentFootsteps(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.SendMessage("You invoke the Silent Footsteps ability.");
                
                // Apply visual effects
                Effects.SendLocationParticles(EffectItem.Create(Caster.Location, Caster.Map, EffectItem.DefaultDuration), 0x373A, 1, 15, 1153, 0, 5029, 0);
                Effects.PlaySound(Caster.Location, Caster.Map, 0x1F2);

                // Apply the stealth effect
                Caster.Hidden = true;
                Timer.DelayCall(TimeSpan.FromSeconds(5.0), EndEffect, Caster);
            }

            FinishSequence();
        }

        private void EndEffect(object state)
        {
            Mobile caster = (Mobile)state;

            if (caster != null && !caster.Deleted)
            {
                caster.Hidden = false;
                caster.SendMessage("The effect of Silent Footsteps has worn off.");
                
                // Play sound and visual effect upon ending stealth
                Effects.SendLocationParticles(EffectItem.Create(caster.Location, caster.Map, EffectItem.DefaultDuration), 0x376A, 1, 14, 1102, 0, 5029, 0);
                Effects.PlaySound(caster.Location, caster.Map, 0x1F8);
            }
        }
    }
}
