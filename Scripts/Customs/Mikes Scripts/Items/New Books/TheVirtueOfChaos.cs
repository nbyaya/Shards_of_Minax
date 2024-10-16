using System;
using Server;

namespace Server.Items
{
    public class TheVirtueOfChaos : BlueBook
    {
        public static readonly BookContent Content = new BookContent
        (
            "The Virtue of Chaos", "Lord Blackthorn",
            new BookPageInfo
            (
                "This manuscript delves",
                "into the intricate",
                "concepts that define",
                "chaos, not as an",
                "entity of destruction,",
                "but as a force for",
                "transformation and",
                "renewal."
            ),
            new BookPageInfo
            (
                "Chaos doesn’t abide by",
                "the set rules and thus,",
                "gives life to",
                "innovation. It pushes",
                "us to explore, adapt,",
                "and finally, evolve."
            ),
            new BookPageInfo
            (
                "Do not fear chaos.",
                "Embrace it, for within",
                "its complex labyrinth",
                "are the keys to new",
                "horizons, untraveled",
                "paths, and undiscovered",
                "truths."
            ),
            new BookPageInfo
            (
                "Chaos is the virtuous",
                "element that forces",
                "the stagnant waters",
                "of society to move,",
                "to change, and to",
                "refresh."
            ),
            new BookPageInfo
            (
                "Like the flames that",
                "forge the finest",
                "weapons, chaos",
                "strengthens the soul,",
                "sharpens the mind, and",
                "enlightens the spirit."
            ),
            new BookPageInfo
            (
                "To the wise, chaos is",
                "not a pit, but a",
                "ladder; not an end,",
                "but a beginning; not a",
                "foe, but a friend."
            ),
			new BookPageInfo
			(
				"The virtue of chaos",
				"also encourages",
				"unconventional",
				"thinking. Where order",
				"seeks to place",
				"boundaries, chaos",
				"tears them down."
			),
			new BookPageInfo
			(
				"Yet, it is essential",
				"to distinguish chaos",
				"from mere anarchy.",
				"Anarchy is the absence",
				"of rules, whereas",
				"chaos operates on",
				"rules yet to be",
				"understood."
			),
			new BookPageInfo
			(
				"Chaos fuels",
				"imagination, inspires",
				"creativity, and",
				"engenders innovation.",
				"It is the primal",
				"soup of possibilities."
			),
			new BookPageInfo
			(
				"It opens the door to",
				"a multitude of",
				"solutions, rather than",
				"restricting us to the",
				"tunnel-vision of a",
				"single answer."
			),
			new BookPageInfo
			(
				"In the realm of",
				"adventuring, chaos",
				"tests our mettle. A",
				"battle seldom goes",
				"as planned; the",
				"elements of chaos",
				"keep warriors on their"
			),
			new BookPageInfo
			(
				"toes, force mages to",
				"improvise, and",
				"challenge rogues to",
				"adapt. In embracing",
				"the chaos, heroes",
				"are born."
			),
			new BookPageInfo
			(
				"The virtue of chaos",
				"is a path of courage,",
				"flexibility, and",
				"unyielding optimism.",
				"It is a belief that",
				"every problem is"
			),
			new BookPageInfo
			(
				"an opportunity in",
				"disguise, and that",
				"every failure is but",
				"a stepping stone on",
				"the road to success."
			),
			new BookPageInfo
			(
				"So, kind reader, do",
				"not shy away from",
				"chaos. Learn its",
				"rhythms, dance to its",
				"tunes, and uncover",
				"the virtues that lie"
			),
			new BookPageInfo
			(
				"within its intricate",
				"maze. For, in chaos,",
				"we find not only",
				"uncertainty and",
				"disorder but also",
				"limitless potential."
			)

        );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public TheVirtueOfChaos() : base(false)
        {
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("The Virtue of Chaos");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "The Virtue of Chaos");
        }

        public TheVirtueOfChaos(Serial serial) : base(serial)
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
