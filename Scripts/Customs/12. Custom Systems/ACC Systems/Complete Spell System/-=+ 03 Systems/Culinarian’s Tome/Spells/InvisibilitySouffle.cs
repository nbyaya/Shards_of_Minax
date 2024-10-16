using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Items;
using Server.Spells;
using System.Collections;

namespace Server.ACC.CSS.Systems.CookingMagic
{
    public class InvisibilitySouffle : CookingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Invisibility Soufflé", "Apertus Culinarius",
            21004, 9300 // Graphics ID and Sound ID
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 20.0; } }
        public override int RequiredMana { get { return 15; } }

        public InvisibilitySouffle(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                SouffleItem souffle = new SouffleItem();
                souffle.MoveToWorld(Caster.Location, Caster.Map);
                Effects.SendLocationParticles(EffectItem.Create(souffle.Location, souffle.Map, EffectItem.DefaultDuration), 0x376A, 10, 15, 5044); // visual effect
                Effects.PlaySound(souffle.Location, souffle.Map, 0x213); // sound effect
            }

            FinishSequence();
        }

        private class SouffleItem : Item
        {
            public SouffleItem() : base(0x09B3) // Item ID for Souffle (example)
            {
                Name = "Invisibility Soufflé";
                Hue = 0x47E; // Unique color
                Movable = true;
                Weight = 1.0;
            }

            public SouffleItem(Serial serial) : base(serial)
            {
            }

            public override void OnDoubleClick(Mobile from)
            {
                if (!IsChildOf(from.Backpack))
                {
                    from.SendLocalizedMessage(1042001); // That must be in your pack to use it.
                    return;
                }

                if (from.Hidden)
                {
                    from.SendMessage("You are already hidden.");
                    return;
                }

                // Start the invisibility effect
                from.Hidden = true;
                from.SendMessage("You consume the soufflé and become invisible!");

                // Visual and sound effects
                from.FixedParticles(0x375A, 10, 15, 5037, EffectLayer.Waist);
                from.PlaySound(0x3C4);

                // Timer for invisibility duration
                Timer.DelayCall(TimeSpan.FromSeconds(10.0), () =>
                {
                    if (from.Hidden)
                    {
                        from.Hidden = false;
                        from.SendMessage("The invisibility effect has worn off.");
                        from.FixedParticles(0x375A, 10, 15, 5037, EffectLayer.Waist);
                        from.PlaySound(0x3C4);
                    }
                });

                this.Delete(); // Remove the item after use
            }

            public override void Serialize(GenericWriter writer)
            {
                base.Serialize(writer);
                writer.Write((int)0); // version
            }

            public override void Deserialize(GenericReader reader)
            {
                base.Deserialize(reader);
                int version = reader.ReadInt();
            }
        }
    }
}
