using System;
using Server;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    // ---------------------------------------------------
    // Saw Line Materia
    // This materia, when socketed, allows the bearer to unleash a saw line attack
    // ---------------------------------------------------

    public class SawLineMateria : BaseSocketAugmentation
    {
        [Constructable]
        public SawLineMateria() : base(0x2809) // Use the item ID for your materia graphic
        {
            Name = "Saw Line Materia";
            Hue = Utility.RandomMinMax(1, 3000); // Generate a random hue between 1 and 3000
        }
        
        public override int SocketsRequired { get { return 1; } } // Adjust number of sockets required as needed
        
        public override int Icon { get { return 0x2809; } } // Icon for the socketing menu

        public override int IconXOffset { get { return 8; } } // X axis icon offset in the socketing menu

        public override int IconYOffset { get { return 20; } } // Y axis icon offset in the socketing menu

        public SawLineMateria(Serial serial) : base(serial)
        {
        }

        public override string OnIdentify(Mobile from)
        {
            return "Unleashes a powerful saw line attack on your foes.";
        }

        public override bool OnAugment(Mobile from, object target)
        {
            if (target is BaseWeapon || target is BaseArmor) // Allows for weapon or armor augmentation
            {
                // Attach the Saw Line effect with specified parameters (example: refractory 5 seconds, damage 20, range 8)
                // Adjust these parameters based on the desired effect
                XmlAttach.AttachTo(target, new XmlSawLine(5.0, 20, 8));
                return true;
            }
            return false;
        }

        public override bool CanAugment(Mobile from, object target)
        {
            // Can augment both weapons and armor
            return target is BaseWeapon || target is BaseArmor;
        }

        public override bool CanRecover(Mobile from, object target, int version)
        {
            // Can recover from both weapons and armor
            return target is BaseWeapon || target is BaseArmor;
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
