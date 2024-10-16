using System;
using Server;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    // ---------------------------------------------------
    // Black Solen Infiltrator Queen Summoning Materia
    // This materia, when socketed, gives a 99% chance to summon a Black Solen Infiltrator Queen on hit
    // ---------------------------------------------------

    public class BlackSolenInfiltratorQueenSummoningMateria : BaseSocketAugmentation
    {

        [Constructable]
        public BlackSolenInfiltratorQueenSummoningMateria() : base(0x2809) // You can change the item ID to match your materia graphic
        {
            Name = "Summoning Black Solen Infiltrator Queen Materia";
            Hue = Utility.RandomMinMax(1, 3000); // Change the hue to a random value between 1 and 3000
        }

        public override int SocketsRequired { get { return 6; } } // Number of sockets required for this materia

        public override int Icon { get { return 0x2809; } } // Icon to use in the socketing menu

        public override int IconXOffset { get { return 8; } } // Icon offset for the X axis in the socketing menu

        public override int IconYOffset { get { return 20; } } // Icon offset for the Y axis in the socketing menu

        public BlackSolenInfiltratorQueenSummoningMateria(Serial serial) : base(serial)
        {
        }

        public override string OnIdentify(Mobile from)
        {
            // This description will appear when the materia is identified
            return "Grants a 99% chance on hit to summon a Black Solen Infiltrator Queen.";
        }

        public override bool OnAugment(Mobile from, object target)
        {
            if (target is BaseWeapon)
            {
                // Attach the summoning effect to the weapon
                XmlAttach.AttachTo(target, new XmlSummonStrike("BlackSolenInfiltratorQueen", 99, 5.0));
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
