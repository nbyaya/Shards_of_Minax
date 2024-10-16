using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using System.Collections;

namespace Server.ACC.CSS.Systems.FletchingMagic
{
    public class Camouflage : FletchingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Camouflage", "In Visibilis",
            21005, 9400
        );

        public override SpellCircle Circle { get { return SpellCircle.Second; } }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 60.0; } }
        public override int RequiredMana { get { return 15; } }

        public Camouflage(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (Caster.Hidden)
            {
                Caster.SendMessage("You are already camouflaged.");
                return;
            }

            if (CheckSequence())
            {
                // Hide the caster
                Caster.Hidden = true;
                Caster.SendMessage("You blend into your surroundings, becoming harder to detect.");

                // Add a visual effect to the caster
                Effects.SendLocationParticles(
                    EffectItem.Create(Caster.Location, Caster.Map, EffectItem.DefaultDuration),
                    0x376A, 1, 14, 1153, 3, 9502, 0
                );

                // Play a sound effect
                Caster.PlaySound(0x1FD);

                // Start a timer to end the effect after a certain duration
                Timer.DelayCall(TimeSpan.FromSeconds(30), () => EndEffect(Caster));
            }

            FinishSequence();
        }

        private void EndEffect(Mobile caster)
        {
            if (caster == null || caster.Deleted)
                return;

            if (caster.Hidden)
            {
                caster.Hidden = false;
                caster.SendMessage("Your camouflage fades away.");
                // Remove any additional effects or bonuses you may have applied
            }
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(2.0);
        }
    }
}
