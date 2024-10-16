using System;
using Server.Mobiles;
using Server.Items;
using Server.Network;
using Server.Spells;
using System.Collections.Generic;

namespace Server.ACC.CSS.Systems.CookingMagic
{
    public class MysticMince : CookingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Mystic Mince", "Ex Minuto Cibus",
            21004,
            9300
        );

        public override SpellCircle Circle { get { return SpellCircle.Second; } }
        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 25.0; } }
        public override int RequiredMana { get { return 30; } }

        public MysticMince(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                // Play a magical sound and effect at the caster's location
                Caster.PlaySound(0x3A);
                Caster.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.Waist);

                // Create the Mystic Mince item
                MysticMinceItem mince = new MysticMinceItem();
                mince.MoveToWorld(Caster.Location, Caster.Map);

                // Notify the player
                Caster.SendMessage("You summon a Mystic Mince!");

                // Additional flashy effects
                Effects.SendLocationEffect(Caster.Location, Caster.Map, 0x376A, 20, 10, 1153, 0);
                Effects.PlaySound(Caster.Location, Caster.Map, 0x1F5);
            }

            FinishSequence();
        }
    }

    public class MysticMinceItem : Item
    {
        [Constructable]
        public MysticMinceItem() : base(0x15F9) // Food item graphic
        {
            Name = "Mystic Mince";
            Hue = 1153; // A magical blue hue
            Stackable = false;
            Weight = 1.0;
        }

        public MysticMinceItem(Serial serial) : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from == null || !from.Alive)
                return;

            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1060640); // You must have the object in your backpack to use it.
                return;
            }

            // Play eating sound and visual effect
            from.PlaySound(0x3A);
            from.FixedEffect(0x373A, 10, 15);

            // Grant a temporary bonus to Magery skill
            from.SendMessage("You feel a surge of magical knowledge!");
            from.Skills[SkillName.Magery].Base += 20;

            Timer.DelayCall(TimeSpan.FromSeconds(120), () =>
            {
                from.SendMessage("The effect of the Mystic Mince fades.");
                from.Skills[SkillName.Magery].Base -= 20;
            });

            // Remove the item from inventory after use
            this.Delete();
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
