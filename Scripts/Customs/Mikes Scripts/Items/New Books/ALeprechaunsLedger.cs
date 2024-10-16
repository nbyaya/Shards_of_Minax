using System;
using Server;

namespace Server.Items
{
    public class ALeprechaunsLedger : BlueBook
    {
        public static readonly BookContent Content = new BookContent
        (
            "A Leprechaun’s Ledger", "Liam the Green",
            new BookPageInfo
            (
                "Within these pages lies",
                "the ledger of me life,",
                "a tale of gold and mischief,",
                "of pots hidden from sight.",
                "I'm Liam, that sprite",
                "in green, cap to boots,",
                "keeper of riches untold,",
                "and crafter of cunning routes."
            ),
            new BookPageInfo
            (
                "Many a man and maid",
                "have sought me wares,",
                "with greed in their hearts,",
                "and traps laid bare.",
                "But with a twinkle in",
                "me eye, and a click",
                "of me heels, I lead them",
                "on a dance, through"
            ),
            new BookPageInfo
            (
                "hills, over streams,",
                "through mystical fog,",
                "to find at the end",
                "naught but a hollow log.",
                "But worry not, for",
                "all is in jest,",
                "for those kind of heart",
                "I grant a quest."
            ),
            new BookPageInfo
            (
                "To those pure in spirit,",
                "I show me pot of gold,",
                "brimming and bright,",
                "a sight to behold.",
                "But to hold such wealth,",
                "one must pass me test,",
                "of wit and of cheer,",
                "and for generosity, zest."
            ),
            new BookPageInfo
            (
                "So remember this well,",
                "when you spy a rainbow,",
                "at its end you may find,",
                "me treasures untold.",
                "But 'tis not gold or",
                "jewels that make one rich,",
                "but a heart full of love,",
                "and life without a hitch."
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
            ),
            new BookPageInfo
            (
                "Liam the Green",
                DateTime.Now.ToString("t"),
                DateTime.Now.ToString("d"),
                "May the luck of the Irish",
                "be ever in your favor,",
                "and your heart be ever",
                "brave and true."
            ),
			//... (previous BookPageInfo objects)
			new BookPageInfo
			(
				"But let's turn the leaf,",
				"and record the new,",
				"of the day I outwitted",
				"a banshee most blue.",
				"With a wail and a screech",
				"she came for me gold,",
				"but I had a trick",
				"for this specter so bold."
			),
			new BookPageInfo
			(
				"A fiddle I played,",
				"a tune spry and light,",
				"that had the old banshee",
				"dancing in fright.",
				"Away she fled into",
				"the night's chilly mists,",
				"leaving me to me coins,",
				"and the stars to be kissed."
			),
			new BookPageInfo
			(
				"Now, a secret 'tween us,",
				"a leprechaun's word:",
				"The gold we possess",
				"is not what you've heard.",
				"It's not the clink of coins,",
				"nor the glitter of metal,",
				"but the joy in the dance,",
				"and in overcoming a nettled."
			),
			new BookPageInfo
			(
				"For each time a soul",
				"thinks they've got me beat,",
				"I show them a world",
				"where even small feet",
				"can lead a grand jig",
				"that spins the earth 'round,",
				"and in that merry chase,",
				"true joy is found."
			),
			new BookPageInfo
			(
				"But don't think me gold",
				"is a mere fairy tale.",
				"For those with true heart,",
				"they can still prevail.",
				"In a glen by the hill,",
				"where the foxgloves grow,",
				"look 'neath the stone",
				"where the fairy lights glow."
			),
			new BookPageInfo
			(
				"There you'll find a chest,",
				"from times old as sin,",
				"with a lock that will open",
				"for the kind-hearted kin.",
				"And inside, you'll see,",
				"more than gold can measure,",
				"memories, dreams, and",
				"life's little pleasures."
			),
			new BookPageInfo
			(
				"So mark well me words,",
				"and follow the clues,",
				"with a laugh and a smile,",
				"and in good sturdy shoes.",
				"For the ledger of life",
				"has entries anew,",
				"and the next page to turn",
				"might be up to you."
			),
			new BookPageInfo
			(
				"I'll end this entry here",
				"with a twinkle in me eye,",
				"for there's more to be written",
				"‘neath this emerald sky.",
				"May your paths be merry,",
				"your heart light and free,",
				"and may you find treasure",
				"wherever you be."
			),
			new BookPageInfo
			(
				"Liam the Green",
				DateTime.Now.ToString("t"),
				DateTime.Now.ToString("d"),
				"Keep your wits sharp,",
				"your spirit ever bright,",
				"and may fortune leap",
				"at your side, day and night."
			)

			// Add more pages as needed, or leave blank pages for the owner to fill in their own adventures and encounters.

			//...

        );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public ALeprechaunsLedger() : base(false)
        {
            // Set the hue to a random color that might represent a leprechaun
            Hue = Utility.RandomMinMax(1002, 1365);
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("A Leprechaun’s Ledger");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "A Leprechaun’s Ledger");
        }

        public ALeprechaunsLedger(Serial serial) : base(serial)
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
