using System;
using Server;

namespace Server.Items
{
    public class RatmenRiddles : BlueBook
    {
        public static readonly BookContent Content = new BookContent
        (
            "Ratmen Riddles: A Cryptic Collection", "Riddlemaster Whiskersnout",
            new BookPageInfo
            (
                "In shadows' keep and",
                "alleyways deep,",
                "Riddlemaster Whiskersnout",
                "spins riddles round",
                "for those to keep.",
                "A cryptic collection",
                "herein lies,",
                "of Ratmen lore and"
            ),
            new BookPageInfo
            (
                "their clever guise.",
                "Seek ye answers,",
                "brave and bold?",
                "Then ponder these riddles",
                "that in whispers are told.",
                "",
                "First to tease your weary brain,"
            ),
            new BookPageInfo
            (
                "A trinket lost, a treasure gained.",
                "It shines not gold nor silver sheen,",
                "But in the dark, it's clearly seen.",
                "What am I?",
                "",
                "Turn the page for more,"
            ),
            new BookPageInfo
            (
                "riddles to explore.",
                "",
                "I can run but never walk,",
                "I have a mouth but never talk.",
                "I have a head but never weep,",
                "I have a bed but never sleep.",
                "What am I?"
            ),
            new BookPageInfo
            (
                "Solve the riddles if you dare,",
                "and learn the secrets Ratmen share.",
                "Twist and turn through mazes blind,",
                "for 'tis the Ratmen's way you'll find.",
                "",
                "Another riddle for your quest,"
            ),
            new BookPageInfo
            (
                "This one is sure to test your zest.",
                "I'm not alive, but I can grow.",
                "I don't have lungs, but I need air.",
                "I don't have a mouth, but water kills me.",
                "What am I?"
            ),
            new BookPageInfo
            (
                "Mull over these cryptic lines,",
                "the answer's there between the vines.",
                "The riddles speak of ancient lore,",
                "unlocking secrets, myths, and more.",
                "",
                "The final riddle to complete,"
            ),
            new BookPageInfo
            (
                "A task I'm sure you'll find quite neat.",
                "I start with an 'e', end with an 'e',",
                "but typically contain just one letter.",
                "What am I?",
                "",
                "So ends our Ratmen riddles' fun,"
            ),
            new BookPageInfo
            (
                "Hope your brain's not overdone!",
                "Ponder well and think it through,",
                "Riddlemaster Whiskersnout bids adieu!"
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
                "Riddlemaster Whiskersnout",
                DateTime.Now.ToString("t"),
                DateTime.Now.ToString("d"),
                "May your wits stay sharp,",
                "your answers smart."
            ),
			// Additional pages added to the book content.
			new BookPageInfo
			(
				"A guardian silent in the hall,",
				"Never moving, standing tall.",
				"In shining armor, it is clad,",
				"Yet this warrior’s not alive, be glad.",
				"What am I?"
			),
			new BookPageInfo
			(
				"Turn not away from metal kin,",
				"For their riddles lie within.",
				"",
				"From what can one never quite escape,",
				"That shows neither hatred nor love,",
				"Is always measured but has no weight?",
				"What am I?"
			),
			new BookPageInfo
			(
				"This challenge, I hope, won’t make you weep,",
				"Its answer’s not far, it's not that deep.",
				"",
				"Alive without breath,",
				"As cold as death;",
				"Never thirsty, ever drinking,",
				"All in mail, never clinking.",
				"What am I?"
			),
			new BookPageInfo
			(
				"Ponder deep, ponder true,",
				"Or the Ratmen will outwit you.",
				"",
				"No doors there are to this stronghold,",
				"Yet thieves break in to steal its gold.",
				"What am I?"
			),
			new BookPageInfo
			(
				"Ratmen's wealth and cunning's proof,",
				"Are found within these riddles' roof.",
				"",
				"The more you take, the more you leave behind.",
				"What am I?"
			),
			new BookPageInfo
			(
				"Take care, dear reader, as you tread",
				"Through Ratmen's riddles, as I've said.",
				"",
				"I speak without a mouth and hear without ears.",
				"I have no body, but I come alive with the wind.",
				"What am I?"
			),
			new BookPageInfo
			(
				"Riddles old and riddles new,",
				"A test of wit I bring to you.",
				"",
				"I have cities, but no houses.",
				"I have mountains, but no trees.",
				"I have water, but no fish.",
				"What am I?"
			),
			new BookPageInfo
			(
				"If answers you've found, then you've done well,",
				"But Ratmen's wit is tough to quell.",
				"",
				"I am not alive, but I grow;",
				"I don't have lungs, but I need air;",
				"I don't have a mouth, but water kills me.",
				"What am I?"
			),
			new BookPageInfo
			(
				"Within these pages lie the key",
				"To Ratmen's secret treasury.",
				"",
				"Forward I am heavy, but backward I am not.",
				"What am I?"
			),
			new BookPageInfo
			(
				"Riddles of night, riddles of dread,",
				"With these, Ratmen's tales are spread.",
				"",
				"You will always find me in the past.",
				"I can be created in the present,",
				"But the future can never taint me.",
				"What am I?"
			),
			new BookPageInfo
			(
				"In winding warrens, rats convene,",
				"To puzzle out what these lines mean.",
				"",
				"I am the beginning of the end,",
				"And the end of time and space.",
				"I am essential to creation,",
				"And surround every place.",
				"What am I?"
			),
			new BookPageInfo
			(
				"The last of riddles, the end of tales,",
				"In your success, the Ratmen hail.",
				"",
				"This thing all things devours:",
				"Birds, beasts, trees, flowers;",
				"Gnaws iron, bites steel;",
				"Grinds hard stones to meal;",
				"Slays kings, ruins towns;",
				"And beats high mountains down.",
				"What am I?"
			),
			new BookPageInfo
			(
				// These pages left intentionally blank for future expansions or player notes.
			),
			new BookPageInfo
			(
			),
			new BookPageInfo
			(
			),
			new BookPageInfo
			(
				"Riddlemaster Whiskersnout",
				DateTime.Now.ToString("t"),
				DateTime.Now.ToString("d"),
				"In shadows deep, where Ratmen creep,",
				"May these riddles your mind keenly keep."
			)
			// End of the additional content.

        );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public RatmenRiddles() : base(false)
        {
            // Set the hue to a random color between 1 and 3000
            Hue = Utility.RandomMinMax(1, 3000);
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("Ratmen Riddles: A Cryptic Collection");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "Ratmen Riddles: A Cryptic Collection");
        }

        public RatmenRiddles(Serial serial) : base(serial)
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
