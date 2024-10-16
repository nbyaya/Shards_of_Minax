using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    // ---------------------------------------------------
    // Time Line Materia
    // This materia, when socketed, grants the ability to manipulate time lines
    // ---------------------------------------------------

    public class TimeLineMateria : BaseSocketAugmentation
    {
        [Constructable]
        public TimeLineMateria() : base(0x2809) // Adjust item ID for your materia graphic
        {
            Name = "Time Line Materia";
            Hue = Utility.RandomMinMax(1, 3000); // Generate a random hue
        }

        public override int SocketsRequired { get { return 1; } } // Number of sockets required

        public override int Icon { get { return 0x2809; } } // Icon for the socketing menu

        public override int IconXOffset { get { return 8; } } // X axis icon offset

        public override int IconYOffset { get { return 20; } } // Y axis icon offset

        public TimeLineMateria(Serial serial) : base(serial)
        {
        }

        public override string OnIdentify(Mobile from)
        {
            return "Manipulates the fabric of time itself.";
        }

        public override bool OnAugment(Mobile from, object target)
        {
            if (target is BaseWeapon || target is BaseArmor) // Works with both weapons and armor
            {
                // Attach the TimeLine effect. Parameters (e.g., refractory, damage, range) to be adjusted as needed.
                // Example values: refractory 10 seconds, damage 15, range 10
                XmlAttach.AttachTo(target, new XmlTimeLine(10.0, 15, 10));
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
