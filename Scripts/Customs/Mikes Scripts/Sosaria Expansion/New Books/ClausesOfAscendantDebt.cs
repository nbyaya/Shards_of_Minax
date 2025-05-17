using System;
using Server;

namespace Server.Items
{
    public class ClausesOfAscendantDebt : BlueBook
    {
        public static readonly BookContent Content = new BookContent
        (
            "The Clauses of Ascendant Debt", "R.J. Neuman",
            new BookPageInfo
            (
                "CONFIDENTIAL - OT 5+",
                "Clauses of Ascendant",
                "Debt (3rd Revision)",
                "By R.J. Neuman",
                "",
                "To ascend is to owe.",
                "To owe is to grow.",
                "The following clauses"
            ),
            new BookPageInfo
            (
                "are binding to all",
                "who seek higher",
                "transcendence within",
                "the Order of",
                "Transcendence.",
                "",
                "Clause One:",
                "All debts must be"
            ),
            new BookPageInfo
            (
                "honored. Failure to",
                "repay spiritual,",
                "financial, or karmic",
                "obligations shall",
                "result in corrective",
                "expenditure.",
                "",
                "Clause Two:"
            ),
            new BookPageInfo
            (
                "Resistance does not",
                "alter obligation.",
                "All protest is an",
                "admission of unpaid",
                "dues. Compliance is",
                "the only path to",
                "balance and growth.",
                ""
            ),
            new BookPageInfo
            (
                "Clause Three:",
                "The flesh is",
                "collateral. Service,",
                "sacrifice, and",
                "submission may be",
                "requested to offset",
                "spiritual deficits",
                "without prior notice."
            ),
            new BookPageInfo
            (
                "Clause Four:",
                "Termination may be",
                "enforced in cases",
                "where debt exceeds",
                "potential for",
                "redemption. This is",
                "a kindness.",
                ""
            ),
            new BookPageInfo
            (
                "Clause Five:",
                "All lower-tier",
                "members (OT 6 and",
                "below) consent to",
                "debt adjustment at",
                "the discretion of",
                "higher echelons, as",
                "expressed through"
            ),
            new BookPageInfo
            (
                "ritual, reassignment,",
                "or sacrifice.",
                "",
                "Clause Six:",
                "Ascension absolves",
                "past balances.",
                "Payment today is an",
                "investment in eternal"
            ),
            new BookPageInfo
            (
                "wealth beyond",
                "mortality.",
                "",
                "Clause Seven:",
                "Queries regarding",
                "clauses are proof of",
                "insufficient",
                "devotion and may be",
                "penalized."
            ),
            new BookPageInfo
            (
                "This document is",
                "not to be shared",
                "with Initiates or",
                "non-Ascendants.",
                "",
                "Property of the",
                "Founder. Unauthorized",
                "duplication is theft."
            ),
            new BookPageInfo
            (
                "",
                "",
                "\"The ledger is not",
                "cruel. It is merely",
                "accurate.\"",
                "",
                "- R.J. Neuman",
                ""
            )
        );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public ClausesOfAscendantDebt() : base(false)
        {
            Hue = Utility.RandomMinMax(1150, 1175); // A somber legalistic color range
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("The Clauses of Ascendant Debt");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "Clauses of Ascendant Debt");
        }

        public ClausesOfAscendantDebt(Serial serial) : base(serial)
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
