using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Items;
using System.Collections.Generic;

namespace Server.ACC.CSS.Systems.FencingMagic
{
    public class Dodge : FencingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Dodge", "Evade",
                                                        //SpellCircle.Second,
                                                        21005,
                                                        9301,
                                                        false,
                                                        Reagent.BlackPearl
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Second; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 30.0; } }
        public override int RequiredMana { get { return 20; } }

        public Dodge(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.SendMessage("You feel lighter on your feet!");
                Caster.PlaySound(0x51D); // Play dodge sound effect
                Caster.FixedParticles(0x3779, 1, 15, 9949, 92, 3, EffectLayer.Head); // Visual effect



                // Temporary increase in physical resistance (armor)

                Timer.DelayCall(TimeSpan.FromSeconds(10.0), () => // Timer to revert the effect after 10 seconds
                {
                    EndEffect(Caster);
                });
            }

            FinishSequence();
        }

        private void EndEffect(Mobile caster)
        {
            if (caster == null || caster.Deleted)
                return;

            caster.SendMessage("Your armor boost fades away.");

        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(1.5);
        }
    }
}
