using System;
using Server;

namespace Server.Items
{
    public class HistoryOfTheDaemons : BlueBook
    {
        public static readonly BookContent Content = new BookContent
            (
                "History of the Daemons", "Archmage Zorlon",
                new BookPageInfo
                (
                    "The story of Daemons",
                    "is one filled with",
                    "mystery and peril.",
                    "For centuries, these",
                    "fiery beings have",
                    "haunted the lands",
                    "",
                    "           -Zorlon"
                ),
                new BookPageInfo
                (
                    "It is said that",
                    "Daemons originate from",
                    "the abyss, a place of",
                    "eternal darkness. They",
                    "are often summoned",
                    "by warlocks seeking",
                    "to gain forbidden",
                    "powers."
                ),
				new BookPageInfo
				(
					"Daemons possess",
					"unimaginable strength",
					"and magical prowess,",
					"but are bound by the",
					"laws of their realm,",
					"unable to leave unless",
					"summoned by a mortal.",
					"Their true names are"
				),
				new BookPageInfo
				(
					"known to very few, as",
					"the speaking of a",
					"Daemon's true name",
					"grants the speaker",
					"control over the",
					"creature. Many",
					"warlocks have sought",
					"these names in vain."
				),
				new BookPageInfo
				(
					"There are various",
					"types of Daemons,",
					"each with their own",
					"distinct powers and",
					"weaknesses. Fire",
					"Daemons, for example,",
					"are resistant to fire",
					"but vulnerable to ice."
				),
				new BookPageInfo
				(
					"It is said that the",
					"first Daemon was",
					"created by the gods as",
					"a guardian for the",
					"Underworld. However,",
					"it became too powerful",
					"and was banished to",
					"the abyss."
				),
				new BookPageInfo
				(
					"Daemons have been",
					"involved in many",
					"historical events,",
					"often as instruments",
					"of destruction. Their",
					"role in the fall of",
					"the Kingdom of",
					"Valoria is infamous."
				),
				new BookPageInfo
				(
					"Despite their fearsome",
					"nature, some argue",
					"that Daemons are not",
					"inherently evil, but",
					"simply follow a",
					"different set of moral",
					"guidelines that we do",
					"not understand."
				),
				new BookPageInfo
				(
					"The summoning of",
					"Daemons is outlawed",
					"in many kingdoms due",
					"to the risks involved.",
					"An inexperienced",
					"summoner may lose",
					"control, unleashing",
					"untold havoc."
				),
				new BookPageInfo
				(
					"It remains a topic of",
					"debate among scholars",
					"whether Daemons have",
					"free will or are",
					"simply bound by the",
					"laws of their nature.",
					"What is clear is that",
					"they are not to be"
				),
				new BookPageInfo
				(
					"trifled with. The",
					"unprepared may find",
					"themselves consumed",
					"by the very power",
					"they sought to",
					"control.",
					"",
					"May this book serve"
				),
				new BookPageInfo
				(
					"as a warning and a",
					"guide for those who",
					"seek to understand",
					"these enigmatic",
					"beings. Tread",
					"carefully, for the path",
					"to knowledge is fraught",
					"with danger."
				)

            );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public HistoryOfTheDaemons() : base(false)
        {
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("History of the Daemons");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "History of the Daemons");
        }

        public HistoryOfTheDaemons(Serial serial) : base(serial)
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
