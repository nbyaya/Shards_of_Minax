using System;
using Server;

namespace Server.Items
{
    public class OrcishAlchemy : BlueBook
    {
        public static readonly BookContent Content = new BookContent
            (
                "Orcish Alchemy", "Gruumsh",
                new BookPageInfo
                (
                    "This text is a",
                    "collection of ancient",
                    "orcish alchemical",
                    "recipes and potions.",
                    "",
                    "          -Gruumsh"
                ),
                new BookPageInfo
                (
                    "In orcish tribes, the",
                    "art of alchemy is a",
                    "closely guarded",
                    "secret, passed down",
                    "from one Shaman to",
                    "the next."
                ),
                new BookPageInfo
                (
                    "One common potion is",
                    "the 'Orcish Brew',",
                    "made from",
                    "Bloodmoss and",
                    "Nightshade. It gives",
                    "temporary strength",
                    "to our warriors."
                ),
                new BookPageInfo
                (
                    "Another important",
                    "potion is the",
                    "'Healer's Mix',",
                    "created from Ginseng",
                    "and Garlic. It helps",
                    "to close wounds and",
                    "revive the fallen."
                ),
                new BookPageInfo
                (
                    "Note that the misuse",
                    "of these potions can",
                    "lead to disastrous",
                    "results. They should",
                    "only be administered",
                    "by a trained Shaman."
                ),
                new BookPageInfo
                (
                    "These recipes have",
                    "been the foundation",
                    "of orcish warfare",
                    "and should be",
                    "guarded at all costs."
                ),
				new BookPageInfo
				(
					"Worg's Blood Elixir:",
					"A potent potion that",
					"grants heightened",
					"senses. It's made from",
					"Worg blood and",
					"Spider's Silk."
				),
				new BookPageInfo
				(
					"Goblin's Curse:",
					"A harmful potion",
					"used to weaken the",
					"enemy. It consists of",
					"poisonous herbs like",
					"Deadly Nightshade."
				),
				new BookPageInfo
				(
					"Earthshaker Brew:",
					"A rare potion that",
					"imbues the drinker",
					"with great power.",
					"Mandrake root and",
					"Daemon blood are key."
				),
				new BookPageInfo
				(
					"Spirit Connection:",
					"A sacred ritual that",
					"connects the spirit to",
					"the ancestors. Requires",
					"Sulfurous ash and",
					"Black Pearl."
				),
				new BookPageInfo
				(
					"Moonclaw Potion:",
					"This brew grants the",
					"ability to see in",
					"darkness. The main",
					"ingredients are Bat",
					"Wing and Eye of Newt."
				),
				new BookPageInfo
				(
					"Orcish War Paint:",
					"A mixture applied to",
					"warriors before battle.",
					"It's made from mud,",
					"berries, and animal",
					"fat."
				),
				new BookPageInfo
				(
					"The brewing methods",
					"are passed down",
					"through generations,",
					"known only to the",
					"most trusted of",
					"Shamans."
				),
				new BookPageInfo
				(
					"Let it be known that",
					"the secrets held",
					"within these pages",
					"are sacred, and any",
					"Orc found sharing",
					"these with outsiders"
				),
				new BookPageInfo
				(
					"will be deemed a",
					"traitor to the tribe",
					"and sentenced to",
					"death.",
					"",
					"May Gruumsh guide",
					"your brews."
				)

            );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public OrcishAlchemy() : base(false)
        {
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("Orcish Alchemy");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "Orcish Alchemy");
        }

        public OrcishAlchemy(Serial serial) : base(serial)
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
