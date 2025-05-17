using System;
using Server;
using Server.Items;
using Server.Targeting;

namespace Server.Items
{
    public class FeluccaPortalPrism : Item
    {
        [Constructable]
        public FeluccaPortalPrism() : base(0x1F13) // You can change the graphic as desired
        {
            Name = "Felucca Portal Prism";
            Hue = 1281; // Optional: gives a glowing prism hue
            Weight = 1.0;
        }

        public FeluccaPortalPrism(Serial serial) : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack to use it.
                return;
            }

            from.SendMessage("Select the magic map you wish to rebind to Felucca.");
            from.Target = new PrismTarget(this);
        }

        private class PrismTarget : Target
        {
            private readonly FeluccaPortalPrism _prism;

            public PrismTarget(FeluccaPortalPrism prism) : base(12, false, TargetFlags.None)
            {
                _prism = prism;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (_prism.Deleted)
                    return;

                if (targeted is MagicMapBase map)
                {
                    if (!map.IsChildOf(from.Backpack))
                    {
                        from.SendMessage("That map must be in your backpack to modify it.");
                        return;
                    }

                    // Check if it's already set
                    if (map.DestinationFacet == Map.Felucca)
                    {
                        from.SendMessage("This map already leads to Felucca.");
                        return;
                    }

                    // Change logic here — make it override if necessary
                    Type mapType = map.GetType();
                    var destFacetField = mapType.GetProperty("DestinationFacet");

                    if (destFacetField != null && destFacetField.CanWrite)
                    {
                        destFacetField.SetValue(map, Map.Felucca, null);
                        from.SendMessage("The portal destination has been attuned to Felucca.");
                    }
                    else
                    {
                        from.SendMessage("This map type cannot have its destination changed.");
                        return;
                    }

                    _prism.Delete();
                }
                else
                {
                    from.SendMessage("You can only use this on a magical map.");
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                if (!_prism.Deleted)
                    from.SendMessage("You decide not to attune any maps.");
            }
        }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add("Shifts the location through the facets.");
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
