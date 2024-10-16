using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Items;
using Server.Spells;

namespace Server.ACC.CSS.Systems.TasteIDMagic
{
    public class FlavorInfusion : TasteIDSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Flavor Infusion", "Savore Pretti",
            21018, // Animation ID
            9314   // Animation Frame
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 0.0; } }
        public override int RequiredMana { get { return 15; } }

        public FlavorInfusion(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private FlavorInfusion m_Spell;

            public InternalTarget(FlavorInfusion spell) : base(1, false, TargetFlags.None)
            {
                m_Spell = spell;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Item item)
                {
                    if (!from.CanSee(targeted))
                    {
                        from.SendLocalizedMessage(500237); // Target can not be seen.
                    }
                    else if (m_Spell.CheckSequence())
                    {
                        // Rename item with "gourmet" prefix
                        string originalName = item.Name ?? item.GetType().Name;
                        item.Name = "gourmet " + originalName.ToLower();

                        // Visual and sound effects
                        from.PlaySound(0x3E8); // Play a magical sound effect
                        Effects.SendLocationParticles(EffectItem.Create(from.Location, from.Map, EffectItem.DefaultDuration), 0x375A, 1, 30, 1153, 4, 0, 0); // Blue magical particle effect

                        from.SendMessage("You have infused the item with a gourmet touch!");
                    }
                }
                else
                {
                    from.SendMessage("That is not a valid target.");
                }

                m_Spell.FinishSequence();
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Spell.FinishSequence();
            }
        }
    }
}
