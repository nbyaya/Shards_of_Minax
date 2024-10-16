using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using System.Collections;

namespace Server.ACC.CSS.Systems.TrackingMagic
{
    public class NaturesVeil : TrackingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Natures Veil", "Natura Umbra",
                                                        21004,
                                                        9300
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; } // Customize circle if necessary
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 35; } }

        public NaturesVeil(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.PlaySound(0x1FB); // Ethereal sound effect
                Caster.FixedParticles(0x373A, 10, 15, 5036, EffectLayer.Waist); // Sparkle effect

                BuffMobile(Caster);
                Caster.SendMessage("You are shrouded in a protective veil, hidden from enemy tracking!");

                Timer.DelayCall(TimeSpan.FromSeconds(10.0), delegate { EndEffect(Caster); });
            }

            FinishSequence();
        }

        private void BuffMobile(Mobile m)
        {
            m.Hidden = true; // Make the player hidden

            // Add a buff icon for visual feedback
            // Ensure this method exists in your code to add a visual representation of the buff
            // This is an example; you need to adapt it to fit your server's buff system
            m.SendMessage("You are now under the effect of Nature's Veil.");
            // Add your custom buff handling here
        }

        private void EndEffect(Mobile m)
        {
            m.Hidden = false; // Remove hidden status
            m.SendMessage("The protective veil fades away...");
        }
    }
}
