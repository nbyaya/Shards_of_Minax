using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Items;
using Server.Misc;

namespace Server.ACC.CSS.Systems.HidingMagic
{
    public class DisappearingAct : HidingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Disappearing Act", "Invisibilis Tempus",
            // SpellCircle.Second,
            21004,
            9203,
            false
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Second; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 25; } }

        public DisappearingAct(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.SendMessage("You feel the shadows envelop you...");
                
                // Play sound effect for invisibility
                Effects.PlaySound(Caster.Location, Caster.Map, 0x1F0);

                // Create a particle effect to show a visual transition to invisibility
                Caster.FixedParticles(0x376A, 10, 15, 5037, EffectLayer.Head);

                // Apply the invisibility effect
                Caster.Hidden = true;

                // Start a timer to make the player visible again after 10 seconds
                Timer.DelayCall(TimeSpan.FromSeconds(10.0), () =>
                {
                    if (Caster != null && !Caster.Deleted)
                    {
                        Caster.Hidden = false;
                        Caster.SendMessage("You are visible again!");

                        // Play sound effect when becoming visible again
                        Effects.PlaySound(Caster.Location, Caster.Map, 0x1FE);
                    }
                });
            }

            FinishSequence();
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(2.0);
        }
    }
}
