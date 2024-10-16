using System;
using Server.Mobiles;
using Server.Items;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using System.Collections.Generic;

namespace Server.ACC.CSS.Systems.StealthMagic
{
    public class TrapDetection : StealthSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Trap Detection", "Trappo Revilo!",
            21007, // Sound ID
            9206   // Effect ID
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fifth; } // Example value; adjust as needed
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 50.0; } } // Adjust as needed
        public override int RequiredMana { get { return 30; } } // Adjust as needed

        public TrapDetection(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.SendMessage("You begin to sense the presence of traps in the vicinity...");

                // Play sound and effect at the caster's location
                Caster.PlaySound(21007);
                Caster.FixedParticles(0x374A, 10, 30, 5054, EffectLayer.Waist);

                // Disable traps within a radius of 5 tiles for 10 seconds
                List<Item> traps = new List<Item>();

                foreach (Item item in Caster.GetItemsInRange(5))
                {
                    if (item is TrapableContainer || item is BaseTrap)
                    {
                        traps.Add(item);
                        item.Visible = false; // Make the trap invisible (representing it being disabled)
                        Caster.SendMessage("A trap is disabled."); // Send message to caster instead of item
                    }
                }

                // Start a timer to reveal the traps again after 10 seconds
                Timer.DelayCall(TimeSpan.FromSeconds(10.0), () =>
                {
                    foreach (Item trap in traps)
                    {
                        if (!trap.Deleted)
                        {
                            trap.Visible = true; // Reveal traps after the duration
                            Caster.SendMessage("The trap is rearmed."); // Send message to caster instead of item
                        }
                    }
                });

                // Set a cooldown of 1 minute
                StartCooldown(Caster, TimeSpan.FromMinutes(1.0));
            }

            FinishSequence();
        }

        private void StartCooldown(Mobile caster, TimeSpan duration)
        {
            Timer.DelayCall(duration, () =>
            {
                caster.SendMessage("You feel ready to use Trap Detection again.");
            });
        }

    }
}
