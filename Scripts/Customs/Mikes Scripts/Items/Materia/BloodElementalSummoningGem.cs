using System;
using Server;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    // ---------------------------------------------------
    // Blood Elemental Summoning Gem
    // This gem, when socketed, gives a 99% chance to summon a Blood Elemental on hit
    // ---------------------------------------------------

    public class BloodElementalSummoningGem : BaseSocketAugmentation
    {

        [Constructable]
        public BloodElementalSummoningGem() : base(0x2809) // You can change the item ID to match your gem graphic
        {
            Name = "Blood Elemental Summoning Gem";
            Hue = 1358; // Change the hue to match the color you want for your gem
        }
        
        public override int SocketsRequired { get { return 3; } } // Number of sockets required for this gem
        
        public override int Icon { get { return 0x2809; } } // Icon to use in the socketing menu

        public override int IconXOffset { get { return 8; } } // Icon offset for the X axis in the socketing menu

        public override int IconYOffset { get { return 20; } } // Icon offset for the Y axis in the socketing menu

        public BloodElementalSummoningGem( Serial serial ) : base( serial )
        {
        }

        public override string OnIdentify(Mobile from)
        {
            // This description will appear when the gem is identified
            return "Grants a 99% chance on hit to summon a Blood Elemental.";
        }

        public override bool OnAugment(Mobile from, object target)
        {
            if (target is BaseWeapon)
            {
                // Attach the summoning effect to the weapon
                XmlAttach.AttachTo(target, new XmlSummonStrike("BloodElemental", 99, 5.0));
                return true;
            }
            return false;
        }

        public override bool CanAugment(Mobile from, object target)
        {
            // Can only augment weapons
            return target is BaseWeapon;
        }
        
        public override bool OnRecover(Mobile from, object target, int version)
        {
            if (target is BaseWeapon)
            {
                // Remove the summoning effect from the weapon
                return true;
            }
            return false;
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
