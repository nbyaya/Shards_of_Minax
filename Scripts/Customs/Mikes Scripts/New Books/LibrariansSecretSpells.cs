using System;
using Server;

namespace Server.Items
{
    public class LibrariansSecretSpells : BlueBook
    {
        public static readonly BookContent Content = new BookContent
        (
            "Librarian's Secret: Spells to Sort and Shelve", "Syllabria the Organized",
            new BookPageInfo
            (
                "In the quiet halls of",
                "the grand library of",
                "Eldoria, amid the",
                "scent of aged parchment,",
                "there lies a secret",
                "known to the master",
                "librarians. A trove of",
                "enchanted knowledge."
            ),
            new BookPageInfo
            (
                "This book, dear reader,",
                "reveals the arcane",
                "practices that keep",
                "our vast collection",
                "immaculately organized,",
                "allowing tomes to be",
                "found with a mere",
                "whisper of desire."
            ),
            new BookPageInfo
            (
                "The 'Sortilius' spell,",
                "for starters, is a basic",
                "yet powerful charm.",
                "With a flick of the wrist,",
                "and a murmur of words,",
                "books fly to their rightful",
                "places, as if guided by",
                "an invisible hand."
            ),
            new BookPageInfo
            (
                "But be wary of its",
                "misuse, for an incorrect",
                "incantation may leave",
                "your library in chaos,",
                "books fluttering like",
                "a flock of startled",
                "birds, seeking refuge",
                "in the oddest of nooks."
            ),
            new BookPageInfo
            (
                "'Dustius Removus' is",
                "a spell of cleaning,",
                "a silent, swirling wind",
                "that caresses each spine,",
                "leaving no trace of",
                "dust or time's wear.",
                "A must-have for any",
                "keeper of ancient lore."
            ),
            new BookPageInfo
            (
                "Among the most",
                "coveted of spells within",
                "these walls is the",
                "'Libra Locatum'. It",
                "allows the caster to",
                "find any volume in the",
                "library's labyrinthine",
                "stacks with unerring"
            ),
            new BookPageInfo
            (
                "precision. To the",
                "untrained, it appears",
                "as mere intuition,",
                "but to the librarian,",
                "it is an art as refined",
                "as any mastered by",
                "the most dexterous",
                "of wizards."
            ),
            new BookPageInfo
            (
                "These spells and more",
                "are detailed within,",
                "from the 'Silencio'",
                "spell that keeps our",
                "reading rooms quiet,",
                "to the 'Lumos Libris',",
                "which bathes a needed",
                "text in gentle light."
            ),
            new BookPageInfo
            (
                "So read on, brave",
                "soul, whether you be",
                "a librarian in search",
                "of order, a mage in",
                "need of organization,",
                "or a mere lover of",
                "books. These spells",
                "are your secret now."
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
                "Syllabria the Organized",
                DateTime.Now.ToString("t"),
                DateTime.Now.ToString("d"),
                "May the shelves always",
                "align, and your books",
                "be ever in reach."
            ),
			// Continuing from the previous content
			new BookPageInfo
			(
				"Furthermore, the 'Indexus'",
				"spell is a marvel for",
				"cataloging. With a simple",
				"gesture, each book whispers",
				"its title and content,",
				"allowing the librarian to",
				"catalog without ever",
				"opening a cover."
			),
			new BookPageInfo
			(
				"However, the use of",
				"'Indexus' requires a",
				"disciplined mind, for the",
				"cacophony of countless",
				"tomes speaking at once",
				"can be overwhelming to",
				"even the most seasoned",
				"spellcaster."
			),
			new BookPageInfo
			(
				"'Pristinus Repairus'",
				"must be handled with",
				"extreme care. This delicate",
				"spell weaves together the",
				"fabric of torn pages,",
				"renews faded ink, and",
				"strengthens brittle bindings,",
				"restoring books to their"
			),
			new BookPageInfo
			(
				"original splendor. A",
				"librarian must use this",
				"spell judiciously, lest the",
				"magic alters historical",
				"texts, erasing their",
				"provenance and worth",
				"with well-intentioned",
				"overzealousness."
			),
			new BookPageInfo
			(
				"The 'Silentium Perpetuum'",
				"enchantment is one that",
				"lingers in the library's",
				"atmosphere. It absorbs",
				"sound, allowing patrons to",
				"study in undisturbed",
				"quietude. Only the turning",
				"of pages softly punctuates"
			),
			new BookPageInfo
			(
				"the silence. This perpetual",
				"quiet is occasionally",
				"broken by the 'Resonare'",
				"spell, which amplifies the",
				"voice of the reader during",
				"public readings or when",
				"a spell of storytelling",
				"is cast."
			),
			new BookPageInfo
			(
				"Woe to the librarian who",
				"casts 'Ex Libris' without",
				"preparation. This advanced",
				"spell ejects books in a",
				"spectacular but chaotic",
				"fashion from the shelves,",
				"intended for swift",
				"retrieval or impromptu"
			),
			new BookPageInfo
			(
				"barricades in dire",
				"circumstances. It has been",
				"said that during the Great",
				"Siege, the librarians of",
				"old cast 'Ex Libris',",
				"turning their library into",
				"a fortress lined with",
				"literary bulwarks."
			),
			new BookPageInfo
			(
				"'Chronologus Sync', a",
				"masterful temporal spell,",
				"is rarely used and only",
				"under strict regulation.",
				"It synchronizes the flow",
				"of time within the library,",
				"ensuring the preservation",
				"of its contents against"
			),
			new BookPageInfo
			(
				"the ravages of time,",
				"an invaluable asset in the",
				"protection of ephemeral",
				"manuscripts. Yet, its",
				"casting is a guarded",
				"secret, as the potential",
				"for misuse is as great as",
				"its benefits."
			),
			new BookPageInfo
			(
				"Lastly, 'Nocte Libri',",
				"a spell of serenity,",
				"casts a peaceful aura",
				"over the library each",
				"night. It ensures that",
				"the books rest as well,",
				"their knowledge safe",
				"within the dreaming shelves,"
			),
			new BookPageInfo
			(
				"waiting to awaken with",
				"the morning light. This",
				"spell, a librarian's lullaby,",
				"is the gentle reminder of",
				"the guardianship we hold",
				"over the world's collected",
				"wisdom and tales."
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
				"Syllabria the Organized",
				DateTime.Now.ToString("t"),
				DateTime.Now.ToString("d"),
				"May the corridors of",
				"knowledge echo with",
				"the silent footsteps of",
				"curiosity and wonder."
			)
			// End of book content expansion

        );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public LibrariansSecretSpells() : base(false)
        {
            // Set the hue to a random color between 1 and 3000
            Hue = Utility.RandomMinMax(1, 3000);
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("Librarian's Secret: Spells to Sort and Shelve");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "Librarian's Secret: Spells to Sort and Shelve");
        }

        public LibrariansSecretSpells(Serial serial) : base(serial)
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
