using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.ForensicsMagic
{
    public class ChemicalAnalysis : ForensicsSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Chemical Analysis", "Analyze Item!",
                                                        21004, // Icon ID
                                                        9300  // Cast sound
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 0.0; } } // No skill required
        public override int RequiredMana { get { return 25; } }

        public ChemicalAnalysis(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private ChemicalAnalysis m_Spell;

            public InternalTarget(ChemicalAnalysis spell) : base(12, false, TargetFlags.None)
            {
                m_Spell = spell;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Item item)
                {
                    if (m_Spell.CheckSequence())
                    {
                        from.SendMessage($"Item ID: {item.ItemID}, Hue: {item.Hue}");
                        
                        // Play a flashy visual effect on the item and a sound at the caster's location
                        Effects.SendLocationParticles(EffectItem.Create(item.Location, item.Map, EffectItem.DefaultDuration), 0x376A, 9, 32, 5008);
                        Effects.PlaySound(from.Location, from.Map, 0x1F2);

                        // Highlight the item temporarily
                        item.PublicOverheadMessage(MessageType.Regular, 0x3B2, false, "Analyzed!");

                        // Apply a light blue hue temporarily to make it look analyzed
                        int originalHue = item.Hue; // Save the original hue
                        item.Hue = 1154; // Apply temporary hue

                        Timer.DelayCall(TimeSpan.FromSeconds(5), () =>
                        {
                            item.Hue = originalHue; // Reset the hue to original
                        });
                    }

                    m_Spell.FinishSequence();
                }
                else
                {
                    from.SendMessage("That is not a valid item.");
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Spell.FinishSequence();
            }
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(2.0);
        }
    }
}
