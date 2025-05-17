using System;
using Server;

namespace Server.Items
{
    public class OTPyramidOfProgress : BlueBook
    {
        public static readonly BookContent Content = new BookContent
        (
            "The OT Pyramid of Progress", "R.J. Neuman",
            new BookPageInfo
            (
                "Welcome, future",
                "Ascendant! Within",
                "these pages lies the",
                "proven path to",
                "Transcendent Wealth",
                "and Spiritual Capital.",
                "",
                "Behold the Pyramid!"
            ),
            new BookPageInfo
            (
                "Each tier represents",
                "a leap in both",
                "material fortune and",
                "cosmic enlightenment.",
                "",
                "OT 9:",
                "Initiate of Intention.",
                "Pay your first tithe."
            ),
            new BookPageInfo
            (
                "OT 8:",
                "Supervisor of Souls.",
                "Identify lesser minds.",
                "",
                "OT 7:",
                "Asset Coordinator.",
                "Oversee contributors.",
                "",
                "OT 6:",
                "Principled Auditor.",
                "Ensure compliance."
            ),
            new BookPageInfo
            (
                "OT 5:",
                "Auditor of Flesh.",
                "Collect overdue",
                "spiritual debts.",
                "",
                "OT 4:",
                "Clause Enforcer.",
                "Enact immutable",
                "contractual truths."
            ),
            new BookPageInfo
            (
                "OT 3:",
                "Axis of Correction.",
                "Balance all equations.",
                "",
                "OT 2:",
                "Tongue of the Star.",
                "Speak the deeper",
                "truths of wealth.",
                "",
                "OT 1:",
                "Executor Prime.",
                "Eliminate debtors."
            ),
            new BookPageInfo
            (
                "At the Pyramid's",
                "pinnacle:",
                "",
                "???",
                "",
                "The realm of ultimate",
                "ascension awaits.",
                "Not all may enter.",
                "",
                "(Note: Advancement",
                "requires spiritual",
                "investment fees.)"
            ),
            new BookPageInfo
            (
                "Remember:",
                "",
                "Stagnation is decay.",
                "Debt is opportunity.",
                "Pain is growth.",
                "",
                "Climb the Pyramid.",
                "Prosper. Transcend.",
                "",
                "~ R.J. Neuman"
            )
        );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public OTPyramidOfProgress() : base(false)
        {
            Hue = Utility.RandomMinMax(1, 3000);
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("The OT Pyramid of Progress");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "The OT Pyramid of Progress");
        }

        public OTPyramidOfProgress(Serial serial) : base(serial)
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
