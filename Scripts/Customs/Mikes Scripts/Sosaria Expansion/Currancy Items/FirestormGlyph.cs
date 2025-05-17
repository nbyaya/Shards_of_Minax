using System;
using Server;
using Server.Items;
using Server.Targeting;

namespace Server.Items
{
    /// <summary>
    /// Glyph of Firestorm ― “Imbues the map with a ring of fire elementals.”
    /// </summary>
    public class FirestormGlyph : Item
    {
        private const int DefaultItemID = 0x1F15; // graphic of your choice
        private const int DefaultHue    = 0x489;  // fiery hue

        [Constructable]
        public FirestormGlyph() : base(DefaultItemID)
        {
            Name   = "Glyph of Firestorm";
            Hue    = DefaultHue;
            Weight = 1.0;
        }

        public FirestormGlyph(Serial serial) : base(serial) { }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add("Adds the Firestorm modifier to a magic map.");
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001); // must be in backpack
                return;
            }

            from.SendMessage("Select the magic map to empower with Firestorm.");
            from.Target = new GlyphTarget(this);
        }

        private class GlyphTarget : Target
        {
            private readonly FirestormGlyph _glyph;

            public GlyphTarget(FirestormGlyph glyph) : base(12, false, TargetFlags.None)
            {
                _glyph = glyph;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (_glyph.Deleted)
                    return;

                if (targeted is MagicMapBase map && map.IsChildOf(from.Backpack))
                {
                    // Add the Firestorm modifier if not already present
                    map.AddModifier("Firestorm");
                    from.SendMessage("Flames surge across the parchment—Firestorm has been added!");
                    _glyph.Delete();
                }
                else
                {
                    from.SendMessage("That is not a magic map in your pack.");
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                if (!_glyph.Deleted)
                    from.SendMessage("You decide against inscribing the glyph.");
            }
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
