using System;
using Server;

namespace Server.Items
{
    public class OdesToTheFallen : BlueBook
    {
        public static readonly BookContent Content = new BookContent
        (
            "Odes to the Fallen", "A Bard Unknown",
            new BookPageInfo
            (
                "In whispered tunes,",
                "our hearts lament",
                "the brave souls lost,",
                "their lights now spent.",
                "They marched to war,",
                "with courage's breath,",
                "Now lay in silence,",
                "embraced by death."
            ),
            new BookPageInfo
            (
                "Their valor shines,",
                "in stories old,",
                "In hushed reverence,",
                "their tales are told.",
                "A mother's tears,",
                "in moonlight glow,",
                "For the child she held,",
                "now lying low."
            ),
            new BookPageInfo
            (
                "The warriors' cries,",
                "once loud and clear,",
                "Now but echoes",
                "that we fear.",
                "Their swords are still,",
                "shields gather dust,",
                "Yet in their silence,",
                "lies trust."
            ),
            new BookPageInfo
            (
                "For every end,",
                "marks anew,",
                "In the cycle of life,",
                "we find what's true.",
                "The fallen give us",
                "strength to stand,",
                "To hold the morrow",
                "in our hand."
            ),
            new BookPageInfo
            (
                "So sing we now,",
                "an ode so grave,",
                "To the fallen heroes,",
                "the noble and brave.",
                "May their journey be",
                "soft and kind,",
                "In the halls of stars,",
                "peace to find."
            ),
            new BookPageInfo
            (
                "As seasons turn,",
                "and years will pass,",
                "Grass will grow",
                "o'er fields of glass.",
                "Their memories shall",
                "in us reside,",
                "A guiding force,",
                "ever by our side."
            ),
            new BookPageInfo
            (
                "Thus, we part not",
                "in sorrow's shade,",
                "But celebrate the",
                "sacrifice they made.",
                "With every note",
                "that we now sing,",
                "We honor the fallen",
                "and life they bring."
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
                "A Bard Unknown",
                DateTime.Now.ToString("t"),
                DateTime.Now.ToString("d"),
                "In remembrance of",
                "the souls that were",
                "and the stories",
                "that shall forever",
                "whisper through",
                "the ancient ethers."
            ),
			// ...previous code...
			new BookPageInfo
			(
				"In battles fought,",
				"upon the seas,",
				"Sailors rest 'neath",
				"waves with ease.",
				"The ocean's depth",
				"now their fortress bed,",
				"Guarded by the silence",
				"of the hallowed dead."
			),
			new BookPageInfo
			(
				"In the crypts and tombs,",
				"beneath the earth,",
				"Lies warriors of old,",
				"of noble birth.",
				"With sword in hand,",
				"still clad in mail,",
				"Their spirit whispers",
				"through every gale."
			),
			new BookPageInfo
			(
				"Upon the windswept hills,",
				"under the sky's expanse,",
				"The fallen lie, eyes closed,",
				"as if in trance.",
				"The grasses sway above",
				"with gentle heft,",
				"Singing lullabies to those",
				"from us left."
			),
			new BookPageInfo
			(
				"In the city's heart,",
				"where statues stand,",
				"Honoring the fallen",
				"of the land.",
				"Their stone-cold gaze",
				"casts forth a plea,",
				"To remember them,",
				"in the land of the free."
			),
			new BookPageInfo
			(
				"Let not our hearts",
				"be drowned in woe,",
				"For in each death,",
				"life's seeds we sow.",
				"In their eternal slumber,",
				"heroes guide our way,",
				"In every choice we make,",
				"every night and day."
			),
			new BookPageInfo
			(
				"An ode to the mage,",
				"whose spells did cast,",
				"A light so bright,",
				"it could not last.",
				"In magic's weave, they",
				"found their fate,",
				"A sacrifice made,",
				"to seal hate's gate."
			),
			new BookPageInfo
			(
				"The minstrels play,",
				"their tunes doth rise,",
				"To meet the heroes",
				"in the skies.",
				"Each note a step,",
				"on the ethereal stair,",
				"Leading to the heavens,",
				"in the air."
			),
			new BookPageInfo
			(
				"Hark! The children's choir",
				"sings a song of peace,",
				"For uncles and aunts",
				"whose battles cease.",
				"Innocent voices carry",
				"far and wide,",
				"A prayer for rest",
				"at eventide."
			),
			new BookPageInfo
			(
				"With flags unfurled",
				"upon the breeze,",
				"We mark the memory",
				"of the cease.",
				"Colors bright upon",
				"the mourning field,",
				"Their symbols of life",
				"to death now yield."
			),
			new BookPageInfo
			(
				"The final ode is one",
				"of silence pure,",
				"No words suffice for lives",
				"so prematurely demure.",
				"We stand, we reflect,",
				"on the cost of strife,",
				"And treasure the delicate",
				"balance of life."
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
				"A Bard Unknown",
				DateTime.Now.ToString("t"),
				DateTime.Now.ToString("d"),
				"Lest we forget those",
				"who have fallen,",
				"Let us carry their dreams",
				"and aspirations—",
				"That they may live on",
				"within our actions",
				"and our songs."
			)
			// ...following code...

        );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public OdesToTheFallen() : base(false)
        {
            // Set the hue to a somber color to reflect the book's tone
            Hue = Utility.RandomNeutralHue();
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("Odes to the Fallen");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "Odes to the Fallen");
        }

        public OdesToTheFallen(Serial serial) : base(serial)
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
