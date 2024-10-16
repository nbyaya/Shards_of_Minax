using System;
using Server;

namespace Server.Items
{
    public class HistoryOfImps : BlueBook
    {
        public static readonly BookContent Content = new BookContent
            (
                "History of Imps", "Mordred",
                new BookPageInfo
                (
                    "Imps are small,",
                    "mischievous beings",
                    "often associated with",
                    "magic. This history",
                    "aims to shed light",
                    "on these misunderstood",
                    "creatures.",
                    "          -Mordred"
                ),
                new BookPageInfo
                (
                    "Imps have been",
                    "known to exist in",
                    "various realms,",
                    "always lurking in",
                    "shadows or busy",
                    "causing mischief.",
                    "They are not evil",
                    "per se, but rather"
                ),
                new BookPageInfo
                (
                    "opportunistic. Imps",
                    "are commonly used",
                    "by mages for",
                    "various tasks,",
                    "thanks to their",
                    "affinity for magical",
                    "arts. However, be",
                    "warned that they can"
                ),
                new BookPageInfo
                (
                    "be quite tricky",
                    "and deceitful.",
                    "In some cultures,",
                    "imps are considered",
                    "harbingers of luck,",
                    "good or bad, while",
                    "in others they are",
                    "feared as omens"
                ),
                new BookPageInfo
                (
                    "of bad fortune.",
                    "Their existence has",
                    "been documented for",
                    "centuries, and they",
                    "appear in various",
                    "forms across different",
                    "mythologies.",
                    ""
                ),
                new BookPageInfo
                (
                    "It's worth noting",
                    "that imps are highly",
                    "intelligent and can",
                    "speak various languages.",
                    "However, they often",
                    "choose to communicate",
                    "in riddles or puzzles,",
                    "complicating tasks"
                ),
                new BookPageInfo
                (
                    "assigned to them.",
                    "So, if you ever",
                    "encounter an imp, be",
                    "wary but also",
                    "respectful. Who knows,",
                    "you might gain a",
                    "powerful ally or",
                    "suffer a cunning foe."
                ),
				new BookPageInfo
				(
					"Imps in Literature",
					"Imps often feature",
					"in stories, folklore,",
					"and myths. They serve",
					"as antagonists, comic",
					"relief, or even heroes.",
					"Their roles vary, but",
					"they always add an"
				),
				new BookPageInfo
				(
					"element of unpredictability",
					"to the tales they",
					"inhabit. In medieval",
					"lore, they were",
					"commonly written as",
					"the familiars of witches,",
					"sorcerers, and other",
					"magic users."
				),
				new BookPageInfo
				(
					"Imps & Magic",
					"Imps are inherently",
					"magical beings, and",
					"they naturally possess",
					"a wide array of magical",
					"abilities. These can",
					"range from simple",
					"spells to complex"
				),
				new BookPageInfo
				(
					"rituals. It's believed",
					"that the blood of imps",
					"has alchemical properties",
					"and can be used in",
					"various magical potions.",
					"However, capturing an",
					"imp for this purpose",
					"is often more trouble"
				),
				new BookPageInfo
				(
					"than it's worth.",
					"Imps & Human",
					"Relations",
					"Although they are",
					"mischievous, imps can",
					"form bonds with humans.",
					"Such bonds are formed",
					"over time and often"
				),
				new BookPageInfo
				(
					"require some form",
					"of mutual benefit.",
					"An imp might serve",
					"a human in exchange",
					"for protection or",
					"magical enhancement.",
					"But remember, imps",
					"are not to be"
				),
				new BookPageInfo
				(
					"trusted blindly.",
					"Breaking a deal with",
					"an imp can lead to",
					"dire consequences,",
					"as they are known for",
					"their vengeful nature.",
					"An angered imp can",
					"become a relentless"
				),
				new BookPageInfo
				(
					"enemy, so weigh your",
					"actions carefully.",
					"Conclusion",
					"Imps are complex",
					"beings, not simply",
					"the mischievous",
					"creatures of lore."
				),
				new BookPageInfo
				(
					"Understanding their",
					"nature can lead to",
					"fruitful partnerships",
					"or prevent unfortunate",
					"mishaps. As with any",
					"creature, approach",
					"imps with caution,",
					"respect, and an open"
				),
				new BookPageInfo
				(
					"mind, and you may",
					"find that they are",
					"more than just the",
					"sum of their tales."
				)

            );

        public override BookContent DefaultContent{ get{ return Content; } }

        [Constructable]
        public HistoryOfImps() : base( false )
        {
        }

        public override void AddNameProperty( ObjectPropertyList list )
        {
            list.Add( "History of Imps" );
        }

        public override void OnSingleClick( Mobile from )
        {
            LabelTo( from, "History of Imps" );
        }

        public HistoryOfImps( Serial serial ) : base( serial )
        {
        }

        public override void Serialize( GenericWriter writer )
        {
            base.Serialize( writer );

            writer.WriteEncodedInt( (int)0 ); // version
        }

        public override void Deserialize( GenericReader reader )
        {
            base.Deserialize( reader );

            int version = reader.ReadEncodedInt();
        }
    }
}
