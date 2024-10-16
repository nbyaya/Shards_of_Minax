using System;
using Server;

namespace Server.Items
{
    public class LycanthropicLegends : BlueBook
    {
        public static readonly BookContent Content = new BookContent
        (
            "Lycanthropic Legends", "Luna the Learned",
            new BookPageInfo
            (
                "In the still of night,",
                "under the glow of the",
                "full moon, the legends",
                "stir. This tome is a",
                "collection of tales and",
                "knowledge about the",
                "enigmatic beings",
                "known as Lycanthropes."
            ),
            new BookPageInfo
            (
                "These creatures, also",
                "known as werewolves,",
                "bear the gift and curse",
                "of transformation. It is",
                "a phenomenon that has",
                "sparked both fear and",
                "fascination throughout",
                "the ages."
            ),
            new BookPageInfo
            (
                "The first known case",
                "dates back to ancient",
                "myths where men",
                "could don the skins",
                "of wolves to gain the",
                "strength of the beast.",
                "Such power came with",
                "a primal cost."
            ),
            new BookPageInfo
            (
                "Through the medieval",
                "times, many believed",
                "that lycanthropy was",
                "a punishment, a curse",
                "laid by the wrathful",
                "or by deals with",
                "shadowy figures."
            ),
            new BookPageInfo
            (
                "Others, however,",
                "sought this gift,",
                "using arcane rituals",
                "and elixirs to invoke",
                "the beast within.",
                "For some, the call of",
                "the wild was too",
                "seductive to deny."
            ),
            new BookPageInfo
            (
                "Tales of heroic deeds",
                "and vicious rampages",
                "alike have been",
                "ascribed to these",
                "beings. Lycanthropes",
                "straddle the line",
                "between human and",
                "beast, often caught"
            ),
            new BookPageInfo
            (
                "in a tumultuous battle",
                "for control. They are",
                "both feared and",
                "revered, embodiments",
                "of nature's untamed",
                "spirit."
            ),
            new BookPageInfo
            (
                // These pages left intentionally blank for notes and illustrations
            ),
            new BookPageInfo
            (
            ),
            new BookPageInfo
            (
            ),
            new BookPageInfo
            (
                "Lycanthropic Legends",
                DateTime.Now.ToString("t"),
                DateTime.Now.ToString("d"),
                "Beware the full moon",
                "and embrace the wilds",
                "within."
            ),
			// Continuing from the previous BookPageInfo...
			new BookPageInfo
			(
				"The most profound",
				"transformation is not",
				"of the flesh, but of the",
				"mind. The beast's",
				"instincts meld with",
				"human thought,",
				"creating a tapestry of",
				"primitive impulse and"
			),
			new BookPageInfo
			(
				"intellectual reason.",
				"In the eyes of a",
				"lycanthrope, the world",
				"becomes a vivid",
				"landscape of scents",
				"and sounds, each",
				"more intense than",
				"what human senses"
			),
			new BookPageInfo
			(
				"can perceive. But",
				"with these gifts,",
				"comes a hunger",
				"untamed by civilized",
				"mores. The urge to",
				"roam, to hunt, to howl",
				"at the moon—it claws",
				"from within, seeking"
			),
			new BookPageInfo
			(
				"release.",
				"",
				"Legends speak of",
				"various ways one might",
				"become a lycanthrope.",
				"Some say it is a",
				"heritage, passed down",
				"through ancient blood."
			),
			new BookPageInfo
			(
				"Others whisper of",
				"bites under a full moon's",
				"light, curses cast by",
				"spurned lovers, or the",
				"wrath of the gods for",
				"mortal hubris. Each",
				"tale differs, yet the",
				"outcome remains the"
			),
			new BookPageInfo
			(
				"same—a life altered",
				"forever by the beast",
				"within.",
				"",
				"The gift of lycanthropy",
				"is often seen as a",
				"double-edged sword.",
				"For some, it brings"
			),
			new BookPageInfo
			(
				"freedom and strength.",
				"To others, it is a",
				"curse of eternal",
				"struggle, a rift between",
				"their human nature",
				"and the wild spirit",
				"struggling for dominion",
				"over their shared"
			),
			new BookPageInfo
			(
				"existence. But whether",
				"blessing or bane, the",
				"lycanthropic experience",
				"is one of profound",
				"change.",
				"",
				"Throughout history,",
				"lycanthropes have been"
			),
			new BookPageInfo
			(
				"feared and hunted.",
				"Towns and villages",
				"would sometimes call",
				"upon hunters—brave",
				"or foolhardy souls",
				"willing to tread into",
				"the heart of darkness",
				"to protect the innocent."
			),
			new BookPageInfo
			(
				"Yet, some lycanthropes",
				"sought control over",
				"their affliction, using",
				"various means to lock",
				"away their inner beast",
				"during the times of",
				"vulnerability. Potions,",
				"talismans, and rituals"
			),
			new BookPageInfo
			(
				"were but a few of the",
				"methods employed to",
				"retain their humanity.",
				"",
				"Others embraced their",
				"nature, forming packs",
				"and living apart from",
				"the world of men. In"
			),
			new BookPageInfo
			(
				"these enclaves, they",
				"could be true to their",
				"nature, free from the",
				"judgment and torches",
				"of civilization. In the",
				"wilderness, they found",
				"a different kind of",
				"society, ruled not by"
			),
			new BookPageInfo
			(
				"laws, but by the moon",
				"and fang.",
				"",
				"To this day, the howl",
				"of a lycanthrope under",
				"the full moon sends a",
				"chilling message to all",
				"who hear it. It speaks"
			),
			new BookPageInfo
			(
				"of a world alongside",
				"ours, one that watches",
				"and waits with ancient",
				"eyes. Whether you fear",
				"or revere these",
				"creatures, one truth",
				"remains—lycanthropes",
				"are as complex and"
			),
			new BookPageInfo
			(
				"varied as the stars",
				"above. And their",
				"legends, as old as",
				"time, continue to",
				"unfold in the shadows",
				"of the world."
			),
			// Additional blank pages for player notes or future content
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
				"Lycanthropic Legends",
				DateTime.Now.ToString("t"),
				DateTime.Now.ToString("d"),
				"In the presence of the",
				"beast, may you find the",
				"humanity within."
			)

        );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public LycanthropicLegends() : base(false)
        {
            // Set the hue to a mysterious color, perhaps something dark and earthy
            Hue = Utility.RandomMinMax(1, 3000);
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("Lycanthropic Legends");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "Lycanthropic Legends");
        }

        public LycanthropicLegends(Serial serial) : base(serial)
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
