using System;
using Server;

namespace Server.Items
{
    public class BrokenStarPendantBlueprint : BlueBook
    {
        public static readonly BookContent Content = new BookContent
        (
            "The Broken Star Pendant Blueprint", "R.J. Neuman",
            new BookPageInfo
            (
                "Concept Draft #1",
                "Pendant shape:",
                "Five-point star with",
                "a deliberate crack",
                "through center.",
                "",
                "Material: Gold-",
                "plated tin alloy."
            ),
            new BookPageInfo
            (
                "Engraving idea:",
                "\"Wealth is",
                "Alignment.\"",
                "",
                "Note to jeweler:",
                "Make it look heavy,",
                "but keep cost low.",
                "Use colored glass"
            ),
            new BookPageInfo
            (
                "instead of gems.",
                "",
                "Markup target:",
                "Initiate tier price:",
                "500 gold minimum.",
                "",
                "Claim they're",
                "\"attuned to cosmic"
            ),
            new BookPageInfo
            (
                "wealth resonance.\"",
                "",
                "Concept Draft #2",
                "Optional: small",
                "compartment in",
                "back for 'ritual salt.'",
                "Actually just sand.",
                "",
                "Extra fee."
            ),
            new BookPageInfo
            (
                "Concept Draft #3",
                "Add chain links",
                "shaped like tiny",
                "pyramids. Symbolic.",
                "",
                "Production notes:",
                "- Maximum profit.",
                "- Zero real value.",
                "- Impressive shine."
            ),
            new BookPageInfo
            (
                "Marketing pitch:",
                "\"Each pendant",
                "aligns the wearer",
                "with the fiscal",
                "currents of the",
                "Broken Star.",
                "Investment is not",
                "optional—it's a",
                "cosmic obligation.\""
            ),
            new BookPageInfo
            (
                "Personal notes:",
                "First shipment sold",
                "out instantly.",
                "Cultists believe",
                "they amplify wealth",
                "and immunity to debt.",
                "",
                "Ritual effects:",
                "- Psychological only."
            ),
            new BookPageInfo
            (
                "Advanced tier:",
                "Add 'Founder's Seal'",
                "to backside.",
                "Charge double.",
                "",
                "Final thought:",
                "\"If they think it",
                "works, it works.",
                "That's synergy.\""
            ),
            new BookPageInfo
            (
                " -- End Blueprint --",
                "",
                "Property of",
                "R.J. Neuman.",
                "Unauthorized copying",
                "or duplication is a",
                "violation of cosmic",
                "contractual law."
            )
        );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public BrokenStarPendantBlueprint() : base(false)
        {
            Hue = 1175;
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("The Broken Star Pendant Blueprint");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "The Broken Star Pendant Blueprint");
        }

        public BrokenStarPendantBlueprint(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.WriteEncodedInt((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadEncodedInt();
        }
    }
}
