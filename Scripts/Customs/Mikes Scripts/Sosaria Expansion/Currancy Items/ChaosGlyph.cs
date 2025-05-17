using System;
using Server;
using Server.Items;
using Server.Targeting;

namespace Server.Items
{
    /// <summary>
    /// Chaos Glyph ― “Shuffles the magical forces within the map.”
    /// </summary>
    public class ChaosGlyph : Item
    {
        /* -------------------------------------------------------------
         *  ‣ Visuals / Basics
         * -----------------------------------------------------------*/
        private const int DefaultItemID = 0x1F14; // pick any graphic you like
        private const int DefaultHue    = 1173;   // purple-ish chaos hue

        [Constructable]
        public ChaosGlyph() : base(DefaultItemID)
        {
            Name   = "Chaos Glyph";
            Hue    = DefaultHue;
            Weight = 1.0;
        }

        public ChaosGlyph(Serial serial) : base(serial)
        {
        }

        /* -------------------------------------------------------------
         *  ‣ Property List (tooltip)
         * -----------------------------------------------------------*/
        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add("Shuffles the magical forces within a map.");
        }

        /* -------------------------------------------------------------
         *  ‣ Activation
         * -----------------------------------------------------------*/
        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001); // must be in backpack
                return;
            }

            from.SendMessage("Select the magic map you wish to scramble.");
            from.Target = new GlyphTarget(this);
        }

        private class GlyphTarget : Target
        {
            private readonly ChaosGlyph _glyph;

            public GlyphTarget(ChaosGlyph glyph) : base(12, false, TargetFlags.None)
            {
                _glyph = glyph;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (_glyph.Deleted)
                    return;

                if (targeted is MagicMapBase map)
                {
                    if (!map.IsChildOf(from.Backpack))
                    {
                        from.SendMessage("That map must be in your backpack to modify it.");
                        return;
                    }

                    /* -- Apply effect ------------------------------------------------ */
                    map.RollModifiers();
                    from.SendMessage("Chaotic energy courses through the parchment—its modifiers have been reshuffled!");

                    /* -- Consume the glyph ------------------------------------------- */
                    _glyph.Delete();
                }
                else
                {
                    from.SendMessage("The Chaos Glyph only works on a magic map.");
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                if (!_glyph.Deleted)
                    from.SendMessage("You decide not to alter any maps for now.");
            }
        }

        /* -------------------------------------------------------------
         *  ‣ Serialization
         * -----------------------------------------------------------*/
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
