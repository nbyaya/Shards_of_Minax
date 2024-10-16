using System;
using Server;

namespace Server.Items
{
    public class OgreHaikus : BlueBook
    {
        public static readonly BookContent Content = new BookContent
        (
            "Ogre Haikus", "Gronk the Poet",
            new BookPageInfo
            (
                "Clumsy giant's tread,",
                "Mountains shake beneath his weight,",
                "Echoes in the mist."
            ),
            new BookPageInfo
            (
                "Mossy teeth grin wide,",
                "Club swings, a dance of raw might,",
                "Forest whispers fall."
            ),
            new BookPageInfo
            (
                "Stars gaze silently,",
                "Ogre dreams of gentle night,",
                "Crickets sing to moon."
            ),
            new BookPageInfo
            (
                "Sunrise glints on club,",
                "A new day for smashing things,",
                "Simple joy abounds."
            ),
            new BookPageInfo
            (
                "Tiny flowers crushed,",
                "Beneath oblivious feet,",
                "Ogres know not soft."
            ),
            new BookPageInfo
            (
                "Muddy riverbank,",
                "Ogre's reflection grins back,",
                "Fish dart in the deep."
            ),
            new BookPageInfo
            (
                "Roaring bonfire's glow,",
                "Stories told in grumbled tones,",
                "Kinship in dark woods."
            ),
            new BookPageInfo
            (
                "Hunger never ends,",
                "Hunting through the rustling leaves,",
                "Feast on deer or sheep."
            ),
            new BookPageInfo
            (
                "Mighty ogre sleeps,",
                "Dreaming of vast, quiet lands,",
                "Peace in thunder's snore."
            ),
            new BookPageInfo
            (
                "Battle's fury spent,",
                "Alone with the stars above,",
                "Heartbeat matches night."
            ),
            // Additional pages can be left blank or added with more haikus if needed.
            new BookPageInfo
            (
            ),
            new BookPageInfo
            (
            ),
            new BookPageInfo
            (
                "Gronk the Poet",
                DateTime.Now.ToString("t"),
                DateTime.Now.ToString("d"),
                "Roar becomes soft hum."
            ),
			// ... Previous code above

			new BookPageInfo
			(
				"Wind howls through the trees,",
				"Ogre stands against the storm,",
				"Unmoved, strong and proud."
			),
			new BookPageInfo
			(
				"Starry sky above,",
				"An ogre's eyes reflect light,",
				"Wonders in the dark."
			),
			new BookPageInfo
			(
				"In the quiet glade,",
				"An ogre contemplates life,",
				"Nature's pupil, wide."
			),
			new BookPageInfo
			(
				"Child of the earth roars,",
				"Mountains bow to his fierce cry,",
				"Rivers pause and listen."
			),
			new BookPageInfo
			(
				"Feet stomp, the ground quakes,",
				"Heart beats in tune with the earth,",
				"Ogre's dance of life."
			),
			new BookPageInfo
			(
				"Through the ogre's eyes,",
				"A world unspoiled and honest,",
				"Truth in every stone."
			),
			new BookPageInfo
			(
				"Laughter rumbles deep,",
				"Echoing through the high peaks,",
				"Joy of simple things."
			),
			new BookPageInfo
			(
				"Ogre's mighty yawn,",
				"Sunset in his gaping maw,",
				"Day ends with a grunt."
			),
			new BookPageInfo
			(
				"Lost in thick fog banks,",
				"An ogre finds his way home,",
				"Nature's child at peace."
			),
			new BookPageInfo
			(
				"Under full moon's light,",
				"Shadows dance at ogre's feet,",
				"Night's silent partner."
			),
			new BookPageInfo
			(
				"Ogre's tears fall hard,",
				"Mourning trees felled by men's greed,",
				"Each drop a lost friend."
			),
			new BookPageInfo
			(
				"Cave's mouth opens wide,",
				"Ogre's home, his sacred space,",
				"Sanctuary, safe."
			),
			new BookPageInfo
			(
				"Old ogre sits still,",
				"Wisdom in his silent gaze,",
				"Stories untold, vast."
			),
			new BookPageInfo
			(
				"Lone flower stands tall,",
				"Ogre watches, ponders life,",
				"Delicate and strong."
			),
			new BookPageInfo
			(
				"Thunderous applause,",
				"Ogre claps, the valley shakes,",
				"Nature's ovation."
			),
			new BookPageInfo
			(
				"In the ogre's palm,",
				"Butterfly rests without fear,",
				"Trust in massive hands."
			),
			new BookPageInfo
			(
				"Frost nips at huge toes,",
				"Winter's kiss on ogre's skin,",
				"Snowflakes stick to brows."
			),
			new BookPageInfo
			(
				"With the dawn, hope springs,",
				"Ogre greets the sun's warm face,",
				"New day, new chances."
			),
			new BookPageInfo
			(
				"Quiet ogre thinks,",
				"Puzzles out the stars' bright dance,",
				"Cosmic wonder, held."
			)
			// ... Continue with serialization and deserialization methods as before.

        );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public OgreHaikus() : base(false)
        {
            // Set the hue to a random color between 1 and 3000
            Hue = Utility.RandomMinMax(1, 3000);
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("Ogre Haikus");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "Ogre Haikus");
        }

        public OgreHaikus(Serial serial) : base(serial)
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
