using System;
using Server;

namespace Server.Items
{
    public class EmeticPotion : Item
    {
        [Constructable]
        public EmeticPotion() : this(1)
        {
        }
		
		[Constructable]
        public EmeticPotion(int amount) : base(0x0F07) // Graphic ID
        {
            Stackable = true;
			Amount = amount;
			Weight = 1.0;
            Hue = 2254; // Hue
            Name = "Emetic Potion";
			
        }

        public EmeticPotion(Serial serial)
            : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
                return;
            }

            if (from.Hunger <= 0)
            {
                from.SendMessage("You don't feel the need to empty your stomach further.");
                return;
            }

            from.Hunger = Math.Max(0, from.Hunger - 5); // Reduce hunger by 5 points, minimum 0
            from.SendMessage("You feel your stomach emptying, allowing you to eat more.");

            // Optionally play a sound or add a visual effect
            from.PlaySound(0x30); // Example sound for potion use

            this.Consume(); // Remove the item after use
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
