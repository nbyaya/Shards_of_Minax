using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using Server.Misc;
using System.Collections.Generic;

namespace Server.ACC.CSS.Systems.NecromancyMagic
{
    public class DarkVision : NecromancySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Dark Vision", "Vas An Lor",
            21004,
            9300,
            false
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Third; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 20; } }

        public DarkVision(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                // Apply Dark Vision effect to the caster
                Caster.SendMessage("You feel your vision expand, allowing you to see in the darkness.");
                Caster.FixedParticles(0x374A, 10, 15, 5036, EffectLayer.Head); // Visual effect on caster
                Caster.PlaySound(0x1FD); // Play sound effect

                // Add a light source around the caster
                Caster.LightLevel = 25;

                // Reveal hidden or invisible creatures within a certain range
                List<Mobile> hiddenCreatures = new List<Mobile>();

                foreach (Mobile m in Caster.GetMobilesInRange(10))
                {
                    if (m.Hidden || m.AccessLevel > AccessLevel.Player)
                    {
                        m.RevealingAction();
                        hiddenCreatures.Add(m);
                    }
                }

                if (hiddenCreatures.Count > 0)
                {
                    Caster.SendMessage("You sense the presence of hidden creatures around you!");
                    foreach (Mobile m in hiddenCreatures)
                    {
                        m.FixedParticles(0x375A, 9, 32, 5008, EffectLayer.Waist); // Flashy effect on detected creatures
                        m.PlaySound(0x213); // Sound effect on detection
                    }
                }
                else
                {
                    Caster.SendMessage("There are no hidden creatures in your vicinity.");
                }

                // Set a timer to remove the light source effect after a duration
                Timer.DelayCall(TimeSpan.FromSeconds(30), () =>
                {
                    if (Caster != null && !Caster.Deleted)
                    {
                        Caster.LightLevel = 0;
                        Caster.SendMessage("Your dark vision fades.");
                    }
                });
            }

            FinishSequence();
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(5.0);
        }
    }
}
