using System;
using Server;

namespace Server.Items
{
    public class BalladsOfTheBattleborne : BlueBook
    {
        public static readonly BookContent Content = new BookContent
        (
            "Ballads of the Battleborne", "Valiant T. Warbourne",
            new BookPageInfo
            (
                "In an age of strife,",
                "under the blood-red",
                "sky, stood warriors",
                "brave, born to die.",
                "Champions of might,",
                "in armor so bright,",
                "singing songs of the",
                "battleborne's fight."
            ),
            new BookPageInfo
            (
                "With steel in hand,",
                "and hearts ablaze,",
                "they charged through",
                "the field, unphased.",
                "The clash of swords,",
                "a thunderous sound,",
                "as foes fell to earth,",
                "to honor bound."
            ),
            new BookPageInfo
            (
                "The bards shall sing",
                "of the siege at Dawn's",
                "light, where the battleborne",
                "fought through the night.",
                "Against odds grim,",
                "they stood firm and true,",
                "as the ramparts they scaled,",
                "and the enemy slew."
            ),
            new BookPageInfo
            (
                "In the icy north,",
                "where the chill winds wail,",
                "the war-horns bellowed",
                "through the gale.",
                "There did the frost giants",
                "meet their fate,",
                "at the hands of the bold,",
                "at Everfrost Gate."
            ),
            new BookPageInfo
            (
                "When dragons roared",
                "above castle stone,",
                "the battleborne answered",
                "with fire of their own.",
                "Through smoke and ash,",
                "the sky they claimed,",
                "until the beast fell",
                "and peace was named."
            ),
            new BookPageInfo
            (
                "No darkened depth,",
                "nor demon's stare,",
                "could quell the courage",
                "found in their prayer.",
                "In the abyss, the fiends",
                "gathered their mass,",
                "but not against the light",
                "of valor shall they pass."
            ),
            new BookPageInfo
            (
                "When the last stand came,",
                "by the old willow tree,",
                "against the tide of shadow,",
                "stood the valiant and free.",
                "With hope as their banner,",
                "and will as their blade,",
                "the battleborne sang,",
                "unafraid."
            ),
            new BookPageInfo
            (
                "This tome is their legacy,",
                "inked in blood and tears,",
                "a testament to the brave,",
                "across the years.",
                "Read these ballads,",
                "sing the refrains,",
                "of the battleborne's glory,",
                "and their unyielding pains."
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
                "Valiant T. Warbourne",
                DateTime.Now.ToString("t"),
                DateTime.Now.ToString("d"),
                "In memory of the fallen,",
                "and the spirits they have",
                "lent to the wind."
            ),
			new BookPageInfo
			(
				"Through somber woods,",
				"and whispered pines,",
				"the battleborne marched",
				"in disciplined lines.",
				"With every step, a silent vow,",
				"to stand as guardians",
				"of the now."
			),
			new BookPageInfo
			(
				"Upon the seas, stormy and vast,",
				"their ships set sail, their fates cast.",
				"Krakens rose from the abyss' bed,",
				"to meet the battleborne's steel,",
				"unfettered and dread."
			),
			new BookPageInfo
			(
				"Under the desert sun, scorching and cruel,",
				"the battleborne's resolve was",
				"the oasis, the jewel.",
				"Sand demons whirled, a harsh, grinding dance,",
				"yet not a step wavered, nor did",
				"a glance."
			),
			new BookPageInfo
			(
				"Atop the highest peak, where the air",
				"is thin, and the cliffs are steep,",
				"they found the dragons' kin.",
				"The battle raged at the edge",
				"of the world, where wings of war",
				"were unfurled."
			),
			new BookPageInfo
			(
				"In caverns deep, where light",
				"dare not tread,",
				"the echoes of battle became",
				"the voice of dread.",
				"Yet in that darkness, so stark and blind,",
				"the battleborne's courage,",
				"the monsters did find."
			),
			new BookPageInfo
			(
				"The fae folk whispered, from shadow to leaf,",
				"of the battleborne's quest, of their",
				"unyielding belief.",
				"Through the enchanted mists, they did",
				"stride, where magic weaves thick",
				"and the ancient ones hide."
			),
			new BookPageInfo
			(
				"In the land of the dead, where silence reigns,",
				"the battleborne marched, breaking",
				"ethereal chains.",
				"Against specters of yore and",
				"wraiths of spite,",
				"they brought back the day,",
				"dispelling the night."
			),
			new BookPageInfo
			(
				"In the halls of the king, a great feast was held,",
				"to honor the battleborne, unexcelled.",
				"Their tales filled the air, songs rose",
				"in waves,",
				"celebrating the living, remembering the graves."
			),
			new BookPageInfo
			(
				"But ballads not only of victory sing,",
				"but also of loss, and the pain it can bring.",
				"For the battleborne, too, knew the",
				"sting of defeat,",
				"Yet never did they allow it to",
				"turn to retreat."
			),
			new BookPageInfo
			(
				"So heed these ballads, learn",
				"what they tell,",
				"of courage, of strength, and of",
				"the will to excel.",
				"For the spirit of the battleborne",
				"never dies,",
				"It lives on in the heart where",
				"true valor lies."
			),
			new BookPageInfo
			(
				// These pages left intentionally blank for notes or future ballads.
			),
			new BookPageInfo
			(
			),
			new BookPageInfo
			(
			),
			new BookPageInfo
			(
				"Valiant T. Warbourne",
				DateTime.Now.ToString("t"),
				DateTime.Now.ToString("d"),
				"Let these ballads echo",
				"in halls and in heart,",
				"For as long as the brave",
				"shall play their part."
			)

        );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public BalladsOfTheBattleborne() : base(false)
        {
            // Set the hue to a random color that represents the theme of battle and valor.
            Hue = Utility.RandomMinMax(1, 3000);
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("Ballads of the Battleborne");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "Ballads of the Battleborne");
        }

        public BalladsOfTheBattleborne(Serial serial) : base(serial)
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
