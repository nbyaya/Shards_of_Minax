using System;
using Server;

namespace Server.Items
{
    public class OTLoyaltyOathScroll : BrownBook
    {
        public static readonly BookContent Content = new BookContent
        (
            "The OT Loyalty Oath Scroll", "R.J. Neuman",
            new BookPageInfo
            (
                "ORDER OF",
                "TRANSCENDENCE (OT)",
                "",
                "LOYALTY OATH SCROLL",
                "",
                "I, the undersigned,",
                "hereby affirm my",
                "unwavering fidelity"
            ),
            new BookPageInfo
            (
                "to the principles,",
                "promises, and profit",
                "potentials of the",
                "Order of",
                "Transcendence.",
                "",
                "I acknowledge that:",
                ""
            ),
            new BookPageInfo
            (
                "1. My soul is a",
                "ledger.",
                "",
                "2. My wealth,",
                "both tangible and",
                "intangible, may be",
                "requisitioned to",
                "balance cosmic"
            ),
            new BookPageInfo
            (
                "accounts as deemed",
                "necessary by the",
                "Founder and/or",
                "designated",
                "executive proxies.",
                "",
                "3. Resistance or"
            ),
            new BookPageInfo
            (
                "hesitation shall be",
                "interpreted as an",
                "unspoken request for",
                "corrective measures.",
                "",
                "4. Ascension is not",
                "a right but an"
            ),
            new BookPageInfo
            (
                "opportunity subject",
                "to approval, merit,",
                "and timely tithe",
                "compliance.",
                "",
                "5. Disputes may be",
                "resolved via the"
            ),
            new BookPageInfo
            (
                "Doctrine of",
                "Balance and",
                "Correction (Rev.",
                "Edition 3.2b), at the",
                "sole discretion of",
                "OT arbitration",
                "councils or their"
            ),
            new BookPageInfo
            (
                "appointed surrogates.",
                "",
                "6. Voluntary",
                "participation",
                "precludes withdrawal",
                "or refund claims,",
                "except where"
            ),
            new BookPageInfo
            (
                "preauthorized by",
                "the Founder in",
                "writing, notarized,",
                "and co-signed by no",
                "fewer than two",
                "celestial witnesses.",
                ""
            ),
            new BookPageInfo
            (
                "I further",
                "acknowledge that",
                "interpretation of",
                "this oath may",
                "evolve in alignment",
                "with higher OT",
                "principles, as",
                "revealed by the"
            ),
            new BookPageInfo
            (
                "Founder or cosmic",
                "entities expressing",
                "authorized will.",
                "",
                "By inscribing my",
                "name, I accept these",
                "terms into my flesh,",
                "fortune, and fate."
            ),
            new BookPageInfo
            (
                "",
                "__________________",
                " (Initiate’s Mark)",
                "",
                "__________________",
                " (Witnessed by R.J.",
                "Neuman, Founder)",
                ""
            )
        );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public OTLoyaltyOathScroll() : base(false)
        {
            Hue = 1109; // Dark blue/void hue
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("The OT Loyalty Oath Scroll");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "The OT Loyalty Oath Scroll");
        }

        public OTLoyaltyOathScroll(Serial serial) : base(serial)
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
