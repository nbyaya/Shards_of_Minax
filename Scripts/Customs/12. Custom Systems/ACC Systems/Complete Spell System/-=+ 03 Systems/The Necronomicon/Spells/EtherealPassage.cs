using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;
using Server.Misc;
using Server.Items;

namespace Server.ACC.CSS.Systems.NecromancyMagic
{
    public class EtherealPassage : NecromancySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Ethereal Passage", "An Lor Xon",
            // SpellCircle.Sixth,  // Uncomment if you use a circle system
            21004,
            9300,
            false,
            Reagent.Nightshade,
            Reagent.BlackPearl
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Sixth; } // Change circle if needed
        }

        public override double CastDelay { get { return 2.5; } }
        public override double RequiredSkill { get { return 70.0; } }
        public override int RequiredMana { get { return 20; } }

        public EtherealPassage(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.SendMessage("You feel your body becoming ethereal...");
                Caster.Hidden = true;
                Caster.Blessed = true; // Makes the caster unable to be harmed for a brief period

                Effects.PlaySound(Caster.Location, Caster.Map, 0x1FE); // A magical sound effect
                Caster.FixedParticles(0x373A, 10, 15, 5036, EffectLayer.Head); // Visual effect around the caster

                Timer.DelayCall(TimeSpan.FromSeconds(10.0), () => EndEffect(Caster));
            }

            FinishSequence();
        }

        private void EndEffect(Mobile caster)
        {
            if (caster == null || caster.Deleted)
                return;

            caster.Hidden = false;
            caster.Blessed = false;
            caster.SendMessage("The ethereal effect fades, and you return to your normal state.");

            Effects.PlaySound(caster.Location, caster.Map, 0x1FD); // Another sound effect when the effect ends
            caster.FixedParticles(0x373A, 10, 15, 5036, EffectLayer.Head); // Visual effect indicating the end of the ethereal state
        }
    }
}
