using System;
using Server;

namespace Server.Items
{
    public class RulesOfAcquisition : BlueBook
    {
        public static readonly BookContent Content = new BookContent
        (
            "The Rules of Acquisition", "Quark",
            new BookPageInfo
            (
                "The Ferengi Rules of",
                "Acquisition are a set",
                "of guidelines for",
                "ensuring the most",
                "profit.",
                "",
                "",
                "            -Quark"
            ),
            new BookPageInfo
            (
                "Rule 1:",
                "Once you have their",
                "money, you never give",
                "it back."
            ),
            new BookPageInfo
            (
                "Rule 2:",
                "The best deal is the",
                "one that brings the",
                "most profit."
            ),
            new BookPageInfo
            (
                "Rule 3:",
                "Never spend more for",
                "an acquisition than",
                "you have to."
            ),
            new BookPageInfo
            (
                "Rule 4:",
                "A woman wearing",
                "clothes is like a man",
                "in the kitchen."
            ),
            new BookPageInfo
            (
                "Rule 5:",
                "If you can't break a",
                "contract, bend it."
            ),
			new BookPageInfo
			(
				"Rule 6:",
				"Never allow family",
				"to stand in the way",
				"of opportunity."
			),
			new BookPageInfo
			(
				"Rule 7:",
				"Keep your ears open."
			),
			new BookPageInfo
			(
				"Rule 8:",
				"Small print leads to",
				"large risk."
			),
			new BookPageInfo
			(
				"Rule 9:",
				"Opportunity plus",
				"instinct equals profit."
			),
			new BookPageInfo
			(
				"Rule 10:",
				"Greed is eternal."
			),
			new BookPageInfo
			(
				"Rule 11:",
				"Even if it's free,",
				"you can always buy it",
				"cheaper."
			),
			new BookPageInfo
			(
				"Rule 12:",
				"Anything worth doing",
				"is worth doing for money."
			),
			new BookPageInfo
			(
				"Rule 13:",
				"Anything worth doing",
				"is worth doing twice."
			),
			new BookPageInfo
			(
				"Rule 14:",
				"Keep your family",
				"close; keep your",
				"latinum closer."
			),
			new BookPageInfo
			(
				"Rule 15:",
				"Acting stupid is often",
				"smart."
			),
			new BookPageInfo
			(
				"Rule 16:",
				"A deal is a deal",
				"until a better one",
				"comes along."
			),
			new BookPageInfo
			(
				"Rule 17:",
				"A contract is a",
				"contract is a contract",
				"but only between Ferengi."
			),
			new BookPageInfo
			(
				"Rule 18:",
				"A Ferengi without",
				"profit is no Ferengi",
				"at all."
			),
			new BookPageInfo
			(
				"Rule 19:",
				"Satisfaction is not",
				"guaranteed."
			),
			new BookPageInfo
			(
				"Rule 20:",
				"He who dives under",
				"the table today, lives",
				"to profit tomorrow."
			)


        );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public RulesOfAcquisition() : base(false)
        {
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("The Rules of Acquisition");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "The Rules of Acquisition");
        }

        public RulesOfAcquisition(Serial serial) : base(serial)
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
