using System;
using Server;

namespace Server.Items
{
    public class AntecedantsOfElvenLaw : BlueBook
    {
        public static readonly BookContent Content = new BookContent
            (
                "Antecedants of Elven Law", "Eolande",
                new BookPageInfo
                (
                    "This manuscript",
                    "details the ancient",
                    "laws that have",
                    "shaped Elven",
                    "civilization for",
                    "millennia.",
                    "",
                    "          -Eolande"
                ),
                new BookPageInfo
                (
                    "The basis of Elven",
                    "law stems from the",
                    "connection to",
                    "nature, a core",
                    "aspect of Elven",
                    "life. Nature's",
                    "harmony influences",
                    "the governance."
                ),
                new BookPageInfo
                (
                    "Historically, Elven",
                    "law consisted of",
                    "three main",
                    "principles:",
                    "Wisdom, Empathy,",
                    "and Justice.",
                    "",
                    "",
                    ""
                ),
                new BookPageInfo
                (
                    "Wisdom is the",
                    "ability to apply",
                    "knowledge and",
                    "experience with",
                    "common sense.",
                    "",
                    "Empathy deals",
                    "with understanding"
                ),
                new BookPageInfo
                (
                    "and sharing the",
                    "feelings of",
                    "another.",
                    "",
                    "Justice is the",
                    "fair and equitable",
                    "treatment of all",
                    "beings."
                ),
                new BookPageInfo
                (
                    "While times have",
                    "changed, these",
                    "principles have",
                    "remained the",
                    "foundation for",
                    "Elven Law."
                ),
				new BookPageInfo
				(
					"The Threefold Law",
					"is a crucial",
					"concept that",
					"governs Elven",
					"judiciary",
					"decisions.",
					"",
					""
				),
				new BookPageInfo
				(
					"This law states",
					"that any action,",
					"positive or",
					"negative, will",
					"return to the",
					"doer threefold.",
					"",
					""
				),
				new BookPageInfo
				(
					"In cases of",
					"treason, ancient",
					"Elven law",
					"prescribes a",
					"ritual of",
					"banishment,",
					"forever separating",
					"the individual"
				),
				new BookPageInfo
				(
					"from Elven lands.",
					"This severe",
					"punishment is",
					"only enacted",
					"after a council",
					"of elders has",
					"deliberated and",
					"given consent."
				),
				new BookPageInfo
				(
					"Land disputes are",
					"settled through",
					"the Rite of",
					"the Green, a",
					"ceremonial duel",
					"held in sacred",
					"groves. The",
					"combatants are"
				),
				new BookPageInfo
				(
					"chosen by the",
					"involved parties,",
					"and they must",
					"fight until one",
					"yields or is",
					"incapacitated.",
					"",
					""
				),
				new BookPageInfo
				(
					"The Council of",
					"the Wise, a",
					"collective of",
					"Elven sages,",
					"often act as",
					"advisers in",
					"difficult cases.",
					""
				)

            );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public AntecedantsOfElvenLaw() : base(false)
        {
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("Antecedants of Elven Law");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "Antecedants of Elven Law");
        }

        public AntecedantsOfElvenLaw(Serial serial) : base(serial)
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
