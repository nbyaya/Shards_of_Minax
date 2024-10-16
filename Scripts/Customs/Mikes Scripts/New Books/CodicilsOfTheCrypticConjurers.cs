using System;
using Server;

namespace Server.Items
{
    public class CodicilsOfTheCrypticConjurers : BlueBook
    {
        public static readonly BookContent Content = new BookContent
        (
            "Codicils of the Cryptic Conjurers", "Archmage Selene",
            new BookPageInfo
            (
                "In the annals of",
                "arcane knowledge,",
                "there exists a secret",
                "order known as the",
                "Cryptic Conjurers.",
                "Hidden from the eyes",
                "of the world, this",
                "enigmatic brotherhood"
            ),
            new BookPageInfo
            (
                "of mages dedicates",
                "itself to the",
                "mysteries of conjuration",
                "and the art of",
                "summoning. Their",
                "Codicils, a collection",
                "of profound wisdom",
                "passed down through"
            ),
            new BookPageInfo
            (
                "generations, offer",
                "insights into the",
                "esoteric arts they",
                "practice. As a",
                "student of magic,",
                "I have sought to",
                "reveal some of the",
                "mysteries contained"
            ),
            new BookPageInfo
            (
                "within these pages.",
                "The Codicils of the",
                "Cryptic Conjurers",
                "are divided into",
                "three chapters, each",
                "dealing with different",
                "aspects of conjuration.",
                ""
            ),
            new BookPageInfo
            (
                "Chapter I: The Art",
                "of Summoning",
                "",
                "Learn the",
                "fundamentals of",
                "summoning creatures",
                "from other planes.",
                "Discover the secrets"
            ),
            new BookPageInfo
            (
                "of binding and",
                "controlling summoned",
                "entities to do your",
                "bidding. Beware the",
                "dangers of summoning",
                "beyond your",
                "capabilities.",
                ""
            ),
            new BookPageInfo
            (
                "Chapter II: The",
                "Conjurer's Circle",
                "",
                "Explore the intricate",
                "patterns of magic",
                "circles used by the",
                "Cryptic Conjurers to",
                "enhance their",
                "summoning rituals."
            ),
            new BookPageInfo
            (
                "Learn how to create",
                "and activate these",
                "circles for different",
                "purposes, from",
                "protection to",
                "amplification of",
                "summoned creatures' powers.",
                ""
            ),
            new BookPageInfo
            (
                "Chapter III: Beyond",
                "the Veil",
                "",
                "Delve into the",
                "mysterious realms",
                "beyond our reality",
                "that the Cryptic",
                "Conjurers access.",
                "Discover the secrets"
            ),
            new BookPageInfo
            (
                "of interdimensional",
                "travel and learn",
                "how to navigate the",
                "unfathomable landscapes",
                "of the multiverse.",
                "But be warned, for",
                "the boundaries between",
                "worlds are fragile."
            ),
            new BookPageInfo
            (
                "These Codicils are",
                "not for the faint of",
                "heart, and their",
                "practice requires",
                "great caution and",
                "respect for the",
                "mysteries of the",
                "cosmos."
            ),
            new BookPageInfo
            (
                "As I continue my",
                "studies, I hope to",
                "uncover more of the",
                "Cryptic Conjurers'",
                "secrets. May these",
                "Codicils serve as a",
                "guide to aspiring",
                "conjurers and"
            ),
            new BookPageInfo
            (
                "seekers of arcane",
                "knowledge alike.",
                "",
                "Archmage Selene",
                DateTime.Now.ToString("t"),
                DateTime.Now.ToString("d"),
                "May the planes be",
                "your playground."
            ),
			new BookPageInfo
			(
				"Chapter IV: The",
				"Summoner's Arsenal",
				"",
				"Discover the",
				"extensive list of",
				"magical tools and",
				"reagents used by",
				"Cryptic Conjurers to",
				"enhance their summoning."
			),
			new BookPageInfo
			(
				"Learn the secrets of",
				"conjurer's crystals,",
				"sigils of binding, and",
				"how to infuse",
				"summoned beings with",
				"enchantments. Uncover",
				"the rituals to",
				"strengthen your",
				"connection to summoned",
				"creatures."
			),
			new BookPageInfo
			(
				"Chapter V: The",
				"Guardians of Secrets",
				"",
				"Explore the role of",
				"the Cryptic Conjurers",
				"in guarding ancient",
				"lore and forbidden",
				"knowledge. Discover",
				"how they protect",
				"powerful artifacts and",
				"arcane relics."
			),
			new BookPageInfo
			(
				"Learn about the",
				"challenges faced by",
				"those who seek to",
				"trespass into the",
				"Conjurers' sanctums",
				"and the magical traps",
				"set to deter",
				"unwanted intruders.",
				""
			),
			new BookPageInfo
			(
				"Chapter VI: The",
				"Unseen Forces",
				"",
				"Delve into the",
				"mysterious forces that",
				"guide the actions of",
				"Cryptic Conjurers. Is",
				"there a higher",
				"purpose to their",
				"summoning?"
			),
			new BookPageInfo
			(
				"Unravel the",
				"enigmatic connection",
				"between the Cryptic",
				"Conjurers and the",
				"cosmic entities they",
				"call upon. Explore",
				"the balance between",
				"order and chaos in",
				"the multiverse.",
				""
			),
			new BookPageInfo
			(
				"Chapter VII: The",
				"Legacy Continues",
				"",
				"Learn about the",
				"training and",
				"initiation of new",
				"members into the",
				"ranks of the Cryptic",
				"Conjurers. Discover",
				"the secrets of their",
				"hidden academies.",
				""
			),
			new BookPageInfo
			(
				"Understand the",
				"oath of secrecy",
				"taken by all",
				"Cryptic Conjurers and",
				"the consequences of",
				"betrayal. Explore the",
				"paths available to",
				"those who aspire to",
				"join this enigmatic",
				"order."
			),
			new BookPageInfo
			(
				"These additional",
				"chapters offer further",
				"insight into the",
				"mysteries of the",
				"Cryptic Conjurers.",
				"May your study of",
				"this book lead you",
				"to a deeper",
				"understanding of the",
				"arcane arts.",
				""
			),
			new BookPageInfo
			(
				"Archmage Selene",
				DateTime.Now.ToString("t"),
				DateTime.Now.ToString("d"),
				"May your conjurations",
				"be both profound and",
				"enlightening."
			)

        );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public CodicilsOfTheCrypticConjurers() : base(false)
        {
            // Set the hue to a random color between 1 and 3000
            Hue = Utility.RandomMinMax(1, 3000);
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("Codicils of the Cryptic Conjurers");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "Codicils of the Cryptic Conjurers");
        }

        public CodicilsOfTheCrypticConjurers(Serial serial) : base(serial)
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
