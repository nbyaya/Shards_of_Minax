using System;
using Server;

namespace Server.Items
{
    public class EnigmaOfElementalEquilibrium : BlueBook
    {
        public static readonly BookContent Content = new BookContent
        (
            "Enigma of Elemental Equilibrium", "Aelius the Elementalist",
            new BookPageInfo
            (
                "In the realm of",
                "mystical energies and",
                "elemental forces,",
                "there exists a profound",
                "enigma, a puzzle that",
                "has confounded even the",
                "wisest of mages.",
                ""
            ),
            new BookPageInfo
            (
                "The quest for",
                "Elemental Equilibrium.",
                "It is a pursuit that",
                "consumes the hearts and",
                "minds of those who dare",
                "to harness the primal",
                "powers of the elements.",
                ""
            ),
            new BookPageInfo
            (
                "To achieve this balance",
                "is to unlock the true",
                "potential of elemental",
                "magic. Yet, it remains",
                "a mystery that has",
                "eluded many, leading to",
                "catastrophic consequences",
                "and untold chaos."
            ),
            new BookPageInfo
            (
                "The ancient scrolls",
                "speak of legendary",
                "Elementalists who",
                "navigated this enigma",
                "with grace and",
                "precision. They could",
                "call upon the fury of",
                "fire, the serenity of"
            ),
            new BookPageInfo
            (
                "water, the steadfastness",
                "of earth, and the",
                "freedom of air in",
                "perfect harmony. Their",
                "spells were a symphony",
                "of elements, each note",
                "played with flawless",
                "execution."
            ),
            new BookPageInfo
            (
                "But for every",
                "Elementalist who",
                "succeeded, there were",
                "dozens who faltered,",
                "unleashing destructive",
                "unbalances that shook",
                "the very foundations of",
                "the world."
            ),
            new BookPageInfo
            (
                "The pursuit of",
                "Elemental Equilibrium",
                "requires not only",
                "knowledge but also",
                "wisdom. It demands",
                "discipline, humility, and",
                "respect for the forces",
                "that govern our world."
            ),
            new BookPageInfo
            (
                "To those who seek to",
                "understand this",
                "enigma, I offer this",
                "advice: tread carefully,",
                "for the elements are",
                "unforgiving. The path",
                "to balance is fraught",
                "with trials and tests."
            ),
            new BookPageInfo
            (
                "May your journey",
                "through the mysteries",
                "of elemental magic lead",
                "you to a harmonious",
                "conclusion. And may",
                "you unlock the secrets",
                "of the Enigma of",
                "Elemental Equilibrium."
            ),
            new BookPageInfo
            (
                // These pages left intentionally blank.
            ),
            new BookPageInfo
            (
            ),
            new BookPageInfo
            (
            ),
            new BookPageInfo
            (
                "Aelius the Elementalist",
                DateTime.Now.ToString("t"),
                DateTime.Now.ToString("d"),
                "May your elemental",
                "journey be a balanced",
                "and enlightened one."
            ),
			            new BookPageInfo
            (
                "The Elements and",
                "Their Essence",
                "",
                "To understand the",
                "Enigma of Elemental",
                "Equilibrium, one must",
                "first grasp the nature",
                "of the four primary"
            ),
            new BookPageInfo
            (
                "elements: Fire, Water,",
                "Earth, and Air. Each",
                "element possesses its",
                "own unique essence and",
                "qualities.",
                "",
                "Fire, the element of",
                "passion and destruction,"
            ),
            new BookPageInfo
            (
                "burns with an",
                "unquenchable fervor. Its",
                "flames symbolize both",
                "creation and",
                "destruction, and its",
                "power lies in",
                "transformation.",
                ""
            ),
            new BookPageInfo
            (
                "Water, the element of",
                "emotions and healing,",
                "flows with a tranquil",
                "serenity. It cleanses",
                "and purifies, yet it",
                "can also unleash",
                "tsunamis of great",
                "power."
            ),
            new BookPageInfo
            (
                "Earth, the element of",
                "stability and",
                "protection, is",
                "unyielding and",
                "resilient. It is the",
                "foundation upon which",
                "all life rests, and its",
                "strength is unmatched."
            ),
            new BookPageInfo
            (
                "Air, the element of",
                "freedom and",
                "movement, dances",
                "through the skies with",
                "grace. It carries",
                "whispers of secrets and",
                "is the breath of life",
                "itself."
            ),
            new BookPageInfo
            (
                "The Dance of",
                "Harmony",
                "",
                "Elemental Equilibrium",
                "is the delicate dance",
                "of balancing these",
                "forces. It is a",
                "seamless fusion of the"
            ),
            new BookPageInfo
            (
                "elements, a symphony",
                "that resonates with the",
                "rhythms of creation and",
                "destruction. Achieving",
                "harmony among them",
                "requires a deep",
                "understanding of their",
                "interplay."
            ),
            new BookPageInfo
            (
                "The trials of",
                "Elementalists are",
                "legendary. They",
                "commune with the",
                "elemental spirits,",
                "embarking on quests to",
                "prove their worthiness.",
                "Only then can they"
            ),
            new BookPageInfo
            (
                "unravel the secrets of",
                "Elemental Equilibrium",
                "and harness the full",
                "potential of elemental",
                "magic. The rewards are",
                "great, but so are the",
                "risks.",
                ""
            ),
            new BookPageInfo
            (
                "The Art of Elemental",
                "Spells",
                "",
                "Elemental spells are",
                "the key to achieving",
                "equilibrium. By",
                "weaving the essences of",
                "fire, water, earth, and"
            ),
            new BookPageInfo
            (
                "air into intricate",
                "patterns, mages can",
                "manipulate the elements",
                "to their will. But",
                "missteps can lead to",
                "catastrophe, as the",
                "elements rebel against",
                "unbalance."
            ),
            new BookPageInfo
            (
                "Each elemental spell",
                "must be cast with",
                "precision, guided by",
                "intuition and",
                "knowledge. A true",
                "Elementalist becomes",
                "one with the elements,",
                "a conduit of their"
            ),
            new BookPageInfo
            (
                "power.",
                "",
                "In the following pages,",
                "we shall delve deeper",
                "into the essence of",
                "each element, the",
                "mysteries of their",
                "balance, and the"
            ),
            new BookPageInfo
            (
                "trials that await those",
                "who seek to master the",
                "Enigma of Elemental",
                "Equilibrium. May your",
                "journey be enlightening",
                "and your spells true.",
                "",
                ""
            )

        );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public EnigmaOfElementalEquilibrium() : base(false)
        {
            // Set the hue to a random color between 1 and 3000
			Hue = Utility.RandomMinMax(1, 3000);
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("Enigma of Elemental Equilibrium");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "Enigma of Elemental Equilibrium");
        }

        public EnigmaOfElementalEquilibrium(Serial serial) : base(serial)
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
