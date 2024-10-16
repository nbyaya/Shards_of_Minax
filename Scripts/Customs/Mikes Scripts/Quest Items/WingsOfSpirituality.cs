using System;
using Server;
using Server.Mobiles;

namespace Server.Items
{
    public class WingsOfSpirituality : Item
    {
        [Constructable]
        public WingsOfSpirituality() : base(0x1515) // You can change the item ID to any appropriate wing graphic you have.
        {
            Name = "Wings of Spirituality";
            Hue = 1150; // Adjust color as needed.
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from is PlayerMobile)
            {
                if (!IsChildOf(from.Backpack)) 
                {
                    from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
                    return;
                }

                // Check if the player already has the effect.
                if (from.FindItemOnLayer(Layer.Cloak) is WingsOfSpiritualityEffect)
                {
                    from.SendMessage("You are already under the influence of the Wings of Spirituality!");
                    return;
                }

                from.SendMessage("You feel light as a feather!");
                
                WingsOfSpiritualityEffect effect = new WingsOfSpiritualityEffect();
                from.AddItem(effect); // Apply the effect to the player.

                Timer.DelayCall(TimeSpan.FromSeconds(30), delegate // The effect will last 30 seconds.
                {
                    effect.Delete(); // Remove the effect after 30 seconds.
                    from.SendMessage("The influence of the Wings of Spirituality fades away.");
                });
            }
        }

        public WingsOfSpirituality(Serial serial) : base(serial)
        {
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

    public class WingsOfSpiritualityEffect : Item
    {
        public WingsOfSpiritualityEffect() : base(0x1515)
        {
            Name = "Wings of Spirituality Effect";
            Movable = false;
            Layer = Layer.Cloak; // This ensures the effect is layered on top of other items the player might wear.
        }

        public WingsOfSpiritualityEffect(Serial serial) : base(serial)
        {
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
