using System;
using Server;

namespace Server.Items
{
    public class GazerGossip : BlueBook
    {
        public static readonly BookContent Content = new BookContent
        (
            "Gazer Gossip", "EyeSpy the Observer",
            new BookPageInfo
            (
                "Many tales I've seen,",
                "with countless eyes,",
                "Gazers are we, watching",
                "under the arcane skies.",
                "In these pages, find",
                "the whispers and wind,",
                "Gossip we've gathered,",
                "secrets we've pinned."
            ),
            new BookPageInfo
            (
                "A Gazer's life is one",
                "of silent observation,",
                "Staring into the abyss,",
                "and all creation.",
                "We've seen knights bold,",
                "mages unwise,",
                "Lovers' first meeting,",
                "and their eventual demise."
            ),
            new BookPageInfo
            (
                "Here's a tale of a rogue,",
                "who danced with shadows,",
                "He pilfered and prowled,",
                "in the gallows and meadows.",
                "His fate, oh so dark,",
                "met under moon's glow,",
                "As we watched in silence,",
                "from the world below."
            ),
            new BookPageInfo
            (
                "Turn the page, you'll find",
                "of a dragon's last flight,",
                "Her scales shimmering,",
                "in the failing light.",
                "With our unblinking gaze,",
                "we witnessed her fall,",
                "A majestic end for her,",
                "the mightiest of all."
            ),
            new BookPageInfo
            (
                "Not all is so grave,",
                "in our ledger's span,",
                "We've seen jesters juggle",
                "and bards form a clan.",
                "Their songs still echo,",
                "in our vast mind's ear,",
                "A melody for the ages,",
                "for those who hear."
            ),
            new BookPageInfo
            (
                "We gazed upon a witch,",
                "brewing potions most vile,",
                "Her cauldron's contents",
                "could the dead revile.",
                "Yet, in her heart, a softness",
                "for a love unrequited,",
                "Whispered wishes for him,",
                "left us quite delighted."
            ),
            new BookPageInfo
            (
                "So many stories we hold,",
                "in our all-seeing throng,",
                "The world's a stage for us,",
                "and life is but a song.",
                "We'll keep our vigil,",
                "keep record of the dance,",
                "In this book, 'Gazer Gossip',",
                "you'll find perchance."
            ),
            new BookPageInfo
            (
                "Remember, dear reader,",
                "under our steady stare,",
                "Your tales might end up",
                "in our care, laid bare.",
                "So live a life worth watching,",
                "worthy of a Gazer's note,",
                "For your story's end",
                "might be what we next quote."
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
                "EyeSpy the Observer",
                DateTime.Now.ToString("t"),
                DateTime.Now.ToString("d"),
                "May your secrets be safe,",
                "and your stories bold."
            ),
			new BookPageInfo
			(
				"Upon a hill, under the",
				"cloak of night, stood a",
				"tower, bathed in starry",
				"light. A sorceress gazed",
				"into realms far and wide,",
				"Seeking knowledge that",
				"worlds did hide."
			),
			new BookPageInfo
			(
				"Her eyes met ours, a",
				"staring contest ensued,",
				"A battle of wills, 'twixt",
				"the watched and viewer.",
				"Neither she nor we did",
				"yield that eve, until",
				"dawn's light began to weave."
			),
			new BookPageInfo
			(
				"In a village square,",
				"a bard told stories grand,",
				"Of heroes and villains",
				"from across the land.",
				"We listened and learned,",
				"for such tales bear",
				"truths, with careful",
				"ears discerned."
			),
			new BookPageInfo
			(
				"There's the Gossip of",
				"the gemstone bright,",
				"Stolen from a dragon's",
				"hoard in the night.",
				"A thief’s bold heart,",
				"by greed was driven;",
				"Now he's just a spy",
				"for us in prison."
			),
			new BookPageInfo
			(
				"Let's not forget the",
				"cobbler's shoes,",
				"Enchanted with steps",
				"that one can't refuse.",
				"They danced him to",
				"castles and courts aplenty,",
				"Yet, now they rest",
				"silent, soles empty."
			),
			new BookPageInfo
			(
				"In a quiet grove, where",
				"elves softly tread,",
				"A secret meeting, words",
				"of revolution were said.",
				"Their plans and hopes,",
				"like fragile glass,",
				"Reflected dreams we saw",
				"in the grass."
			),
			new BookPageInfo
			(
				"Then there was the",
				"alchemist with his",
				"potions and brews,",
				"His elixir of life",
				"was front-page news.",
				"But alas! For a Gazer’s",
				"eye did spot,",
				"His life's work was",
				"naught but a draught."
			),
			new BookPageInfo
			(
				"We saw a seer, her",
				"crystal clear,",
				"Revealing futures that",
				"drew near.",
				"Yet not even she saw",
				"within her own sphere,",
				"The Gazer’s gaze",
				"returning her leer."
			),
			new BookPageInfo
			(
				"The Gazer’s gaze,",
				"a fateful mirror,",
				"Reflecting all, the",
				"truth bearer.",
				"No secret safe, no",
				"whisper too quiet,",
				"Our sight unveils",
				"the calm and the riot."
			),
			new BookPageInfo
			(
				"So read these pages,",
				"dear seeker of lore,",
				"The Gazer's Gossip",
				"has myths and more.",
				"And should you feel",
				"a watchful presence,",
				"Know we record with",
				"meticulous essence."
			),
			new BookPageInfo
			(
				"EyeSpy the Observer",
				DateTime.Now.ToString("t"),
				DateTime.Now.ToString("d"),
				"Witness to the world’s",
				"broad tapestry unfurled."
			)
			// The closing BookPageInfo instances from the previous script go here...

        );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public GazerGossip() : base(false)
        {
            // Set the hue to a random color between 3001 and 6000 for a different range than the example book
            Hue = Utility.RandomMinMax(3001, 6000);
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("Gazer Gossip");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "Gazer Gossip");
        }

        public GazerGossip(Serial serial) : base(serial)
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
