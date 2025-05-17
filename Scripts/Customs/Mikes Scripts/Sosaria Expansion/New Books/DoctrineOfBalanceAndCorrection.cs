using System;
using Server;

namespace Server.Items
{
    public class DoctrineOfBalanceAndCorrection : BlueBook
    {
        public static readonly BookContent Content = new BookContent
        (
            "Doctrine of Balance & Correction", "R.J. Neuman",
            new BookPageInfo
            (
                "The following is the",
                "required doctrine",
                "for OT 3 Overseers",
                "and above. It is",
                "the guiding truth",
                "for sustaining the",
                "Order’s hierarchical",
                "and spiritual balance."
            ),
            new BookPageInfo
            (
                "Rebellion is not a",
                "sin. It is a symptom.",
                "It signals imbalance.",
                "",
                "Correction is not",
                "punishment. It is",
                "an act of mercy.",
                "",
                "Anomalies must be"
            ),
            new BookPageInfo
            (
                "corrected swiftly,",
                "or imbalance will",
                "cascade downward,",
                "undoing the shared",
                "ascent of all.",
                "",
                "Thus, correction is",
                "a sacred duty."
            ),
            new BookPageInfo
            (
                "Section I:",
                "Anomalous Behavior",
                "",
                "Failure to tithe,",
                "questioning authority,",
                "unauthorized rituals,",
                "withholding assets,",
                "or expressing doubt."
            ),
            new BookPageInfo
            (
                "Section II:",
                "Standard Corrections",
                "",
                "1. Isolation.",
                "2. Financial audit.",
                "3. Physical rebalancing",
                "(approved overseers",
                "only).",
                "",
                "Persistent anomalies"
            ),
            new BookPageInfo
            (
                "require escalation",
                "to the Clause of",
                "Finality (see OT 5",
                "manual).",
                "",
                "Remember:",
                "Doubt is contagious.",
                "Correction contains it."
            ),
            new BookPageInfo
            (
                "Section III:",
                "The Mathematics of",
                "Correction",
                "",
                "Every soul is a",
                "variable. Every debt,",
                "a coefficient. Every",
                "correction, a necessary",
                "balancing operation."
            ),
            new BookPageInfo
            (
                "This is not belief.",
                "This is arithmetic.",
                "",
                "The equation must",
                "resolve.",
                "",
                "Imbalance must be",
                "reduced to zero.",
                "Resistance must be",
                "divided by will."
            ),
            new BookPageInfo
            (
                "Section IV:",
                "Justification",
                "",
                "Balance is not cruelty.",
                "Balance is the",
                "universal principle",
                "ensuring our ascent.",
                "",
                "Correction is not",
                "judgment. It is the"
            ),
            new BookPageInfo
            (
                "inevitable result of",
                "unsustainable variables.",
                "",
                "Anomalies resist,",
                "but they cannot",
                "defy mathematics.",
                "",
                "To refuse correction",
                "is to invite total",
                "annihilation."
            ),
            new BookPageInfo
            (
                "Closing Affirmation:",
                "",
                "\"Correction is mercy.",
                "Balance is ascension.",
                "The Broken Star is",
                "not a god. It is a",
                "calculation beyond",
                "the minds of mortals.\""
            ),
            new BookPageInfo
            (
                "— R.J. Neuman",
                "Founder & Principal",
                "Architect of Ascension",
                "",
                "(This document is",
                "property of the",
                "Order of Transcendence.",
                "Reproduction without",
                "approval is an anomaly.)"
            )
        );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public DoctrineOfBalanceAndCorrection() : base(false)
        {
            Hue = Utility.RandomMinMax(1100, 1175);
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("Doctrine of Balance & Correction");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "Doctrine of Balance & Correction");
        }

        public DoctrineOfBalanceAndCorrection(Serial serial) : base(serial)
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
