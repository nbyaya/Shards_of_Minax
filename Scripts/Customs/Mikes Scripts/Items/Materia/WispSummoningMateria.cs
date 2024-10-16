using System;
using Server;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class WispSummoningMateria : BaseSocketAugmentation
    {
        [Constructable]
        public WispSummoningMateria() : base(0x2809) // Use the desired graphic ID for your materia
        {
            Name = "Summoning Wisp Materia";
            Hue = Utility.RandomMinMax(1, 3000); // Generate a random hue between 1 and 3000
        }
        
        public override int SocketsRequired { get { return 3; } }

        public override int Icon { get { return 0x2809; } }

        public override int IconXOffset { get { return 8; } }

        public override int IconYOffset { get { return 20; } }

        public WispSummoningMateria(Serial serial) : base(serial)
        {
        }

        public override string OnIdentify(Mobile from)
        {
            return "Grants a 99% chance on hit to summon a Wisp."; // Adjust the description as needed
        }

        public override bool OnAugment(Mobile from, object target)
        {
            if (target is BaseWeapon)
            {
                XmlAttach.AttachTo(target, new XmlSummonStrike("Wisp", 99, 5.0)); // Use "Wisp" as the summon type
                return true;
            }
            return false;
        }

        public override bool CanAugment(Mobile from, object target)
        {
            return target is BaseWeapon;
        }
        
        public override bool CanRecover(Mobile from, object target, int version)
        {
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
