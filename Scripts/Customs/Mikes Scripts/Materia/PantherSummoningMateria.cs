using System;
using Server;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    // Panther Summoning Materia
    public class PantherSummoningMateria : BaseSocketAugmentation
    {
        [Constructable]
        public PantherSummoningMateria() : base(0x2809) // Use the appropriate item ID for your materia graphic
        {
            Name = "Summoning Panther Materia";
            Hue = Utility.RandomMinMax(1, 3000); // Random hue between 1 and 3000
        }

        public override int SocketsRequired { get { return 1; } }

        public override int Icon { get { return 0x2809; } }
        public override int IconXOffset { get { return 8; } }
        public override int IconYOffset { get { return 20; } }

        public PantherSummoningMateria(Serial serial) : base(serial)
        {
        }

        public override string OnIdentify(Mobile from)
        {
            return "Grants a 99% chance on hit to summon a Panther.";
        }

        public override bool OnAugment(Mobile from, object target)
        {
            if (target is BaseWeapon)
            {
                // Attach the summoning effect to the weapon
                XmlAttach.AttachTo(target, new XmlSummonStrike("Panther", 99, 5.0));
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
