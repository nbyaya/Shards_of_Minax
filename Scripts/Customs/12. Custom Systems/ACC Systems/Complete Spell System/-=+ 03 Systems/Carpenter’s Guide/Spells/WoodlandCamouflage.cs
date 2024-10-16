using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.CarpentryMagic
{
    public class WoodlandCamouflage : CarpentrySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Woodland Camouflage", "A Log Xen",
            266,
            9040,
            false,
            Reagent.Ginseng,
            Reagent.SpidersSilk
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Second; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 30.0; } }
        public override int RequiredMana { get { return 20; } }

        public WoodlandCamouflage(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                // Check if the caster is already hidden
                if (Caster.Hidden)
                {
                    Caster.SendLocalizedMessage(502641); // You are already hidden!
                }
                else
                {
                    // Play visual and sound effects for casting
                    Effects.PlaySound(Caster.Location, Caster.Map, 0x64B); // Forest ambiance sound
                    Effects.SendLocationParticles(
                        EffectItem.Create(Caster.Location, Caster.Map, EffectItem.DefaultDuration),
                        0x376A, 9, 32, 1109, 0, 5022, 0
                    ); // Leaves falling effect around the caster

                    // Hide the caster
                    Caster.Hidden = true;
                    Caster.SendMessage("You blend into the forest, becoming nearly invisible.");

                    // Apply a timed effect to automatically reveal the caster after a duration
                    Timer.DelayCall(TimeSpan.FromSeconds(30.0), new TimerStateCallback(RevealCallback), Caster);
                }
            }

            FinishSequence();
        }

        private void RevealCallback(object state)
        {
            Mobile caster = (Mobile)state;

            if (caster.Hidden)
            {
                caster.Hidden = false;
                caster.SendMessage("You are no longer camouflaged.");
                Effects.PlaySound(caster.Location, caster.Map, 0x1F4); // Sound effect for reappearing
                caster.FixedParticles(0x373A, 10, 15, 5018, EffectLayer.Waist); // Visual effect for reappearing
            }
        }
    }
}
