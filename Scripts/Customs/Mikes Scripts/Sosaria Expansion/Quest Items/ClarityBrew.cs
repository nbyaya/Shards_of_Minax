using System;
using Server;
using Server.Mobiles;

namespace Server.Items
{
    public class ClarityBrew : Item
    {
        [Constructable]
        public ClarityBrew() : base(0x100F) // Bottle
        {
            Hue = 1153; // Ethereal blue
            Name = "Clarity Brew";
            Weight = 1.0;
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendMessage("That must be in your pack to use it.");
                return;
            }

            from.SendMessage(0x59, "You feel a strange vision taking hold...");

            // Insert vision effects or dialogue pop-up logic here
            from.PlaySound(0x1F7);
            from.FixedParticles(0x373A, 10, 15, 5016, 92, 2, EffectLayer.Head);

            Delete(); // One-time use
        }

        public ClarityBrew(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt();
        }
    }
}
