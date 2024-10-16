using System;
using Server;

namespace Server.Items
{
    public class XenodochialXorns : BlueBook
    {
        public static readonly BookContent Content = new BookContent
        (
            "Xenodochial Xorns", "Travels of Tymon",
            new BookPageInfo
            (
                "In my journeys through",
                "the cavernous depths,",
                "I chanced upon a sight",
                "most bizarre - Xorns,",
                "creatures of the earth,",
                "offering a hand in",
                "guidance rather than",
                "claws in defiance."
            ),
            new BookPageInfo
            (
                "This account serves",
                "as a record of these",
                "peculiar beings - their",
                "ways, their culture,",
                "and the warm welcome",
                "they bestowed upon",
                "a wayward traveler",
                "such as myself."
            ),
            // ... Additional pages would continue the narrative ...
            new BookPageInfo
            (
                // The last written page by Tymon
                "Thus, I write with",
                "a quill gifted by",
                "a Xorn, dipped in",
                "ink as dark as the",
                "soil they cherish.",
                "May this tome serve",
                "as a bridge between",
                "our worlds."
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
                "Travels of Tymon",
                DateTime.Now.ToString("t"),
                DateTime.Now.ToString("d"),
                "May hospitality",
                "prevail in all realms."
            ),
			// ... Previous pages ...
			new BookPageInfo
			(
				"The depths yawned wide,",
				"veins of ore like stars",
				"in the stony night.",
				"My lantern, a lone sun,",
				"cast long shadows -",
				"and there, the Xorns",
				"stood as if molded",
				"from the earth itself."
			),
			new BookPageInfo
			(
				"Their eyes, aglow with",
				"a gentle luminescence,",
				"watched me not with",
				"malice, but curiosity.",
				"The leader, a Xorn of",
				"imposing size, stepped",
				"forward, its maw",
				"grinding stone."
			),
			new BookPageInfo
			(
				"Yet, it spoke not in",
				"growls, but through",
				"a form of earthy",
				"telepathy. 'Weary",
				"traveler,' it rumbled,",
				"'rest here in safety,",
				"for the earth's embrace",
				"is kind.'"
			),
			new BookPageInfo
			(
				"Astounded by their",
				"graciousness, I",
				"accepted. They guided",
				"me through tunnels",
				"aglow with mineral",
				"light, to a chamber",
				"adorned with crystal",
				"and geode."
			),
			new BookPageInfo
			(
				"In this subterranean",
				"sanctuary, I was fed",
				"minerals I dared not",
				"eat - yet they proved",
				"nourishing to my",
				"surprise. The Xorns",
				"feasted alongside me,",
				"ingesting precious ores."
			),
			new BookPageInfo
			(
				"With each shared meal,",
				"the Xorns divulged",
				"their tales. Stories of",
				"the deep earth, of",
				"magic pulsing through",
				"stone, and of caverns",
				"singing with the breath",
				"of the world."
			),
			new BookPageInfo
			(
				"They spoke of their",
				"kind's loneliness,",
				"misunderstood by those",
				"above. How they yearned",
				"for companionship, for",
				"those who could see",
				"past their rough-hewn",
				"visages."
			),
			new BookPageInfo
			(
				"In return, I offered",
				"tales of the surface,",
				"of seas vast and",
				"forests green. Eyes",
				"wide, they listened,",
				"their hearts heavy",
				"with a longing for",
				"sights unseen."
			),
			new BookPageInfo
			(
				"Days turned to nights,",
				"and my fear ebbed away,",
				"replaced by a profound",
				"peace. The earth's",
				"heartbeat was a lullaby,",
				"the Xorns' presence",
				"a protective circle.",
				"I was home, yet not."
			),
			new BookPageInfo
			(
				"When it was time to",
				"part, a solemn mood",
				"fell. 'Friend of earth,'",
				"the leader murmured,",
				"'carry our story above.",
				"Let them know we wait",
				"in friendship, not fury.'"
			),
			new BookPageInfo
			(
				"I ascended, the weight",
				"of their trust heavy",
				"as the rocks they bore.",
				"Now, I pen their tale,",
				"a testament to the",
				"xenodochial Xorns,",
				"keepers of earth's",
				"hidden kindness."
			),
			new BookPageInfo
			(
				"Should you wander",
				"into deep places,",
				"fear not the earth's",
				"stir. For within its",
				"dark embrace, you may",
				"find unexpected friends,",
				"the Xenodochial Xorns."
			),
			// ... Additional pages if needed ...
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
				"Travels of Tymon",
				DateTime.Now.ToString("t"),
				DateTime.Now.ToString("d"),
				"In friendship may all",
				"creatures of the world",
				"meet, and in understanding,",
				"grow."
			)

			// ... Continue with the class definition and methods ...

        );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public XenodochialXorns() : base(false)
        {
            // Set the hue to a random earthy color to represent the Xorns' affinity with earth
            Hue = Utility.RandomMinMax(2400, 2599); // Adjust the color range as needed
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("Xenodochial Xorns");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "Xenodochial Xorns");
        }

        public XenodochialXorns(Serial serial) : base(serial)
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
