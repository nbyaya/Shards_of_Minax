using System;
using Server;

namespace Server.Items
{
    public class AncestorsAndTheOrcs : BlueBook
    {
        public static readonly BookContent Content = new BookContent
        (
            "Ancestors and the Orcs", "Grok the Wise",
            new BookPageInfo
            (
                "This manuscript aims",
                "to provide a deeper",
                "understanding of Orc",
                "history and the role",
                "ancestors play in our",
                "society.",
                "",
                "           -Grok"
            ),
            new BookPageInfo
            (
                "The notion that Orcs",
                "are merely savages",
                "is a false portrayal",
                "shaped by years of",
                "misunderstanding.",
                "This text hopes to",
                "correct that.",
                "   -Grok"
            ),
            new BookPageInfo
            (
                "Ancestors guide us",
                "through spiritual",
                "rituals. Their wisdom",
                "is bestowed upon the",
                "chieftains, who lead",
                "our tribes and clans.",
                "",
                "    -Grok"
            ),
            new BookPageInfo
            (
                "While Orcs may not",
                "be scholars like",
                "humans or elves, we",
                "have a rich oral",
                "tradition that has",
                "been passed down",
                "through generations."
            ),
            new BookPageInfo
            (
                "In conclusion, our",
                "ancestors provide us",
                "with the strength",
                "and wisdom to",
                "survive. Ignoring",
                "this spiritual aspect",
                "of Orc culture would",
                "be a great mistake."
            ),
			new BookPageInfo
			(
				"The Sacred Caves",
				"It is in the hidden",
				"caves that we often",
				"commune with our",
				"ancestors. These",
				"caves are sanctified",
				"grounds and are",
				"protected by shamans."
			),
			new BookPageInfo
			(
				"The Rituals",
				"Rituals vary from tribe",
				"to tribe but generally",
				"involve the burning",
				"of sacred herbs, and",
				"the recitation of",
				"ancient chants."
			),
			new BookPageInfo
			(
				"Orcish War Tactics",
				"Orcish war tactics are",
				"often misunderstood.",
				"They are devised by",
				"the chieftains but",
				"influenced by ancestral",
				"warriors through",
				"dreams and visions."
			),
			new BookPageInfo
			(
				"The Hunt",
				"The act of hunting is",
				"not just for sustenance",
				"but also a rite of",
				"passage. The success",
				"of the hunt often",
				"depends on ancestral",
				"guidance."
			),
			new BookPageInfo
			(
				"Ancestral Spirits",
				"Sometimes the spirits",
				"of our ancestors",
				"manifest visibly,",
				"especially when the",
				"tribe faces existential",
				"threats. They offer",
				"guidance and protection."
			),
			new BookPageInfo
			(
				"Conclusion",
				"It is crucial for the",
				"future generations to",
				"remember and honor",
				"our ancestors. They",
				"hold the keys to our",
				"past, present, and",
				"future."
			)

        );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public AncestorsAndTheOrcs() : base(false)
        {
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("Ancestors and the Orcs");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "Ancestors and the Orcs");
        }

        public AncestorsAndTheOrcs(Serial serial) : base(serial)
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
