using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using System.Collections;
using Server.Items;

namespace Server.ACC.CSS.Systems.StealthMagic
{
    public class SilentMovement : StealthSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Silent Movement", "Silens Motus",
                                                        21002,
                                                        9201,
                                                        false
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 0.0; } }
        public override int RequiredMana { get { return 10; } }

        public SilentMovement(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Mobile caster = Caster;

                caster.SendMessage("You begin to move silently.");
                caster.FixedParticles(0x376A, 9, 32, 5008, EffectLayer.Waist); // Visual effect around the waist
                caster.PlaySound(0x24A); // Sound effect for stealth activation

                caster.Hidden = true;
                caster.Blessed = true; // Temporarily make the caster invulnerable to detection
                Timer.DelayCall(TimeSpan.FromSeconds(60), new TimerStateCallback(EndSilentMovement), caster);

                Effects.SendLocationParticles(
                    EffectItem.Create(caster.Location, caster.Map, EffectItem.DefaultDuration), 
                    0x376A, 9, 32, 5008);
            }

            FinishSequence();
        }

        private static void EndSilentMovement(object state)
        {
            Mobile caster = (Mobile)state;

            if (caster == null || caster.Deleted)
                return;

            caster.SendMessage("Your silent movement effect fades.");
            caster.Hidden = false;
            caster.Blessed = false; // End invulnerability

            caster.PlaySound(0x1F8); // Sound effect when the ability ends
        }
    }
}
