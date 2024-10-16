using System;
using Server;

namespace Server.Items
{
    public class HowToHaggleWithAHag : BlueBook
    {
        public static readonly BookContent Content = new BookContent
        (
            "How to Haggle with a Hag", "Bargain Ben",
            new BookPageInfo
            (
                "In my travels wide",
                "and dealings vast,",
                "I've haggled with",
                "creatures from the",
                "humble shopkeep to",
                "the mightiest of",
                "dragons. Yet none",
                "so tricky as a Hag."
            ),
            new BookPageInfo
            (
                "To bargain with a Hag,",
                "you must understand,",
                "their love for deals",
                "runs deep as a",
                "canyon's span. They",
                "crave the sport,",
                "the banter, the",
                "thrill of the haggle."
            ),
            new BookPageInfo
            (
                "A Hag will cackle,",
                "her eyes a-twinkle,",
                "as she sizes you up,",
                "her intent not so",
                "simple. She'll lure you",
                "with trinkets,",
                "gold, and more,",
                "but beware the cost,"
            ),
            new BookPageInfo
            (
                "it's steeped in lore.",
                "Never show your need,",
                "nor your purse too full,",
                "or she'll twist the deal",
                "till you feel the pull.",
                "Start with a trifle,",
                "an offer quite low,",
                "and be ready to walk,",
                "letting interest show."
            ),
            new BookPageInfo
            (
                "The Hag will huff,",
                "perhaps even scowl,",
                "claim you insult",
                "her with a bid so foul.",
                "Yet stand your ground,",
                "with a grin or jest,",
                "for in this dance",
                "patience is best."
            ),
            new BookPageInfo
            (
                "She'll call you names,",
                "maybe curse your kin,",
                "but it's all part of",
                "the bargaining din.",
                "Throw in a charm,",
                "a sweetener, so to speak,",
                "and watch the Hag's",
                "resistance grow weak."
            ),
            new BookPageInfo
            (
                "Bargain with wit,",
                "and bargain with glee,",
                "for a Hag respects",
                "those who haggle free.",
                "If she laughs and agrees,",
                "then the deal is struck,",
                "you've haggled with a Hag",
                "and with some luck."
            ),
            new BookPageInfo
            (
                "So take these words",
                "as you venture forth,",
                "to the murky swamp",
                "or the icy north.",
                "With Hags a-plenty",
                "waiting with glee,",
                "may your words be shrewd,",
                "and your spirit free."
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
                "Bargain Ben",
                DateTime.Now.ToString("t"),
                DateTime.Now.ToString("d"),
                "May your pockets jingle",
                "and your treasures shine."
            ),
			new BookPageInfo
			(
				"Remember, a Hag's goods",
				"may come with more than",
				"a price; a curse, a hex,",
				"or a potion not nice.",
				"Ask for a tale of each",
				"item's past,",
				"and listen well, for",
				"Hags speak truths vast."
			),
			new BookPageInfo
			(
				"And should you find",
				"an item cursed,",
				"fear not to bargain,",
				"to quench your thirst.",
				"For Hags enjoy a",
				"clever ploy,",
				"turn the curse 'round,",
				"and it might bring joy."
			),
			new BookPageInfo
			(
				"But what of love potions,",
				"or dreams of wealth?",
				"Be wary of promises",
				"for your heart's health.",
				"A Hag's brew can turn",
				"as sour as old wine,",
				"and leave you with naught",
				"but a swine."
			),
			new BookPageInfo
			(
				"If the haggle goes sour,",
				"and the Hag's eyes flare,",
				"back away with care,",
				"avoid the snare.",
				"For a Hag scorned",
				"is a fearsome foe,",
				"best leave her be,",
				"and away you go."
			),
			new BookPageInfo
			(
				"Yet, strike a deal",
				"fair and true,",
				"and a Hag might reveal",
				"secrets, to few.",
				"She might whisper of",
				"hidden treasures of yore,",
				"or paths to take,",
				"that you must explore."
			),
			new BookPageInfo
			(
				"With each successful trade,",
				"a friendship may bloom,",
				"beyond the cauldron's",
				"bubbling fume.",
				"A Hag as a friend",
				"is loyal till end,",
				"and the deals you'll make,",
				"oh, how they'll ascend!"
			),
			new BookPageInfo
			(
				"So this is my guide,",
				"tried and true,",
				"on how to haggle with",
				"a Hag - not to rue.",
				"May your words be smooth,",
				"your courage stout,",
				"and may your coin pouch",
				"never give out."
			),
			new BookPageInfo
			(
				"For in the world's bazaar",
				"of magic so wild,",
				"the Hag's crafty ways",
				"can charm beast or child.",
				"Trade with respect,",
				"and earn her sly smile,",
				"and you'll have stories",
				"worthwhile."
			),
			new BookPageInfo
			(
				"May your travels be safe,",
				"and your bargains be grand,",
				"and may you never find",
				"yourself at the wrong end",
				"of a Hag's gnarled hand.",
				"",
				"Yours in shrewd barter,",
				"Bargain Ben"
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
				"Bargain Ben",
				DateTime.Now.ToString("t"),
				DateTime.Now.ToString("d"),
				"And remember - in the",
				"realm of haggle and witches,",
				"the richest treasure",
				"is the wisdom of the ages."
			)

        );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public HowToHaggleWithAHag() : base(false)
        {
            // Set the hue to a random color between 1 and 3000
            Hue = Utility.RandomMinMax(1, 3000);
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("How to Haggle with a Hag");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "How to Haggle with a Hag");
        }

        public HowToHaggleWithAHag(Serial serial) : base(serial)
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
