using System;
using Server;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    // ---------------------------------------------------
    // Black Dragoon Pirate Materia
    // This materia, when socketed, gives a unique effect
    // ---------------------------------------------------

    public class BlackDragoonPirateMateria : BaseSocketAugmentation
    {
        [Constructable]
        public BlackDragoonPirateMateria() : base(0x2809) // You can change the item ID to match your materia graphic
        {
            Name = "Black Dragoon Pirate Materia";
            Hue = Utility.RandomMinMax(1, 3000); // Assign a random hue between 1 and 3000
        }

        public override int SocketsRequired { get { return 2; } } // Number of sockets required for this materia

        public override int Icon { get { return 0x2809; } } // Icon to use in the socketing menu

        public override int IconXOffset { get { return 8; } } // Icon offset for the X axis in the socketing menu

        public override int IconYOffset { get { return 20; } } // Icon offset for the Y axis in the socketing menu

        public BlackDragoonPirateMateria(Serial serial) : base(serial)
        {
        }

        public override string OnIdentify(Mobile from)
        {
            // This description will appear when the materia is identified
            return "Grants a 99% chance on hit to summon a Black Dragoon Pirate.";
        }

        public override bool OnAugment(Mobile from, object target)
        {
            if (target is BaseWeapon)
            {
                // Attach your custom effect to the weapon here
                // Replace this with your desired effect logic
                // Example: XmlAttach.AttachTo(target, new XmlSummonStrike("YourEffectName", 100, 5.0));
				XmlAttach.AttachTo(target, new XmlSummonStrike("BlackDragoonPirate", 99, 5.0));
                
                // Make sure to follow RunUO and C# 2.0/3.0 compatible syntax
                return true;
            }
            return false;
        }

        public override bool CanAugment(Mobile from, object target)
        {
            // Can only augment weapons
            return target is BaseWeapon;
        }

        public override bool CanRecover(Mobile from, object target, int version)
        {
            // Can recover from weapons
            return target is BaseWeapon;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version 0
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
