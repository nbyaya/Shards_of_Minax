using System;
using Server;

namespace Server.Items
{
    public class ScrollsOfTheSubliminal : BlueBook
    {
        public static readonly BookContent Content = new BookContent
        (
            "Scrolls of the Subliminal", "Lysandra the Enigmatic",
            new BookPageInfo
            (
                "In the realm of",
                "arcane mysteries and",
                "subtle enchantments,",
                "I, Lysandra, weave a",
                "tale of hidden powers",
                "and elusive secrets.",
                "",
                "These scrolls bear"
            ),
            new BookPageInfo
            (
                "witness to my",
                "journey into the",
                "realms of the",
                "subliminal. A realm",
                "where magic whispers",
                "in the shadows, and",
                "the subconscious",
                "holds the key."
            ),
            new BookPageInfo
            (
                "With ink imbued in",
                "dreams, I transcribe",
                "the silent incantations",
                "that shape thoughts",
                "and desires. My",
                "quest to master the",
                "subliminal arts began",
                "with a single question."
            ),
            new BookPageInfo
            (
                "How do thoughts",
                "become reality?",
                "The answer lay",
                "beyond the realm of",
                "common spells and",
                "incantations, hidden",
                "within the depths of",
                "the subconscious."
            ),
            new BookPageInfo
            (
                "Through these",
                "pages, I shall share",
                "the methods and",
                "mysteries of the",
                "subliminal arts. From",
                "influencing the mind",
                "without words to",
                "weaving illusions in"
            ),
            new BookPageInfo
            (
                "the thoughts of",
                "others, each chapter",
                "will unveil the secrets",
                "of this esoteric path.",
                "",
                "May you, dear reader,",
                "tread cautiously as you",
                "embark on this journey"
            ),
            new BookPageInfo
            (
                "into the subliminal.",
                "For the power it",
                "holds is profound,",
                "and with great",
                "knowledge comes",
                "great responsibility.",
                "",
                "Lysandra the Enigmatic"
            ),
            new BookPageInfo
            (
                DateTime.Now.ToString("t"),
                DateTime.Now.ToString("d"),
                "Unlock the",
                "subliminal world,",
                "and may your",
                "intentions be guided",
                "by wisdom and",
                "compassion."
            ),
			            new BookPageInfo
            (
                "Chapter 1: The",
                "Whispers of the Mind",
                "",
                "The subliminal arts",
                "begin with a subtle",
                "dance of thoughts and",
                "desires. It is the",
                "realm of whispers,"
            ),
            new BookPageInfo
            (
                "where intentions take",
                "shape before words are",
                "spoken. To tap into",
                "this realm, one must",
                "quiet the mind and",
                "listen to the",
                "whispers within.",
                ""
            ),
            new BookPageInfo
            (
                "Chapter 2: The",
                "Subconscious Canvas",
                "",
                "In the canvas of the",
                "subconscious, thoughts",
                "paint vivid pictures",
                "and dreams become",
                "reality. Learn to",
                "shape this canvas with"
            ),
            new BookPageInfo
            (
                "precision, for every",
                "brushstroke carries",
                "meaning. Explore the",
                "archetypes and symbols",
                "that reside within,",
                "and you shall unlock",
                "the power to",
                "influence minds."
            ),
            new BookPageInfo
            (
                "Chapter 3: Illusions",
                "of the Mind",
                "",
                "The mind is a realm",
                "ripe for illusions. By",
                "weaving subtle",
                "phantoms within",
                "thoughts, one can",
                "create perceptions that",
                "deceive and enchant.",
                ""
            ),
            new BookPageInfo
            (
                "Chapter 4: The",
                "Language of Emotions",
                "",
                "Emotions are the",
                "language of the",
                "subconscious. Learn",
                "to speak this language",
                "fluently, for it is the",
                "key to shaping hearts",
                "and minds alike.",
                ""
            ),
            new BookPageInfo
            (
                "Chapter 5: The",
                "Art of Suggestion",
                "",
                "Suggestion is a",
                "subliminal tool of",
                "great power. Master",
                "the art of planting",
                "suggestions within",
                "the minds of others,",
                "and you will guide",
                "their thoughts and"
            ),
            new BookPageInfo
            (
                "actions to your",
                "will. But always use",
                "this power with",
                "caution and",
                "compassion, for the",
                "subliminal arts can",
                "both heal and harm.",
                ""
            ),
            new BookPageInfo
            (
                "Chapter 6: The",
                "Subliminal Ethos",
                "",
                "As you journey deeper",
                "into the subliminal",
                "arts, remember the",
                "ethos that governs",
                "them. Seek not to",
                "control, but to",
                "understand. Seek not",
                "to manipulate, but to"
            ),
            new BookPageInfo
            (
                "enlighten. The true",
                "power of the",
                "subliminal lies in",
                "unlocking the",
                "potential within",
                "ourselves and others.",
                "May your path be one",
                "of wisdom and"
            ),
            new BookPageInfo
            (
                "illumination, guided by",
                "the Scrolls of the",
                "Subliminal.",
                "",
                "Lysandra the",
                "Enigmatic",
                "",
                DateTime.Now.ToString("t"),
                DateTime.Now.ToString("d")
            )

        );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public ScrollsOfTheSubliminal() : base(false)
        {
            // Set the hue to a random color between 1 and 3000
			Hue = Utility.RandomMinMax(1, 3000);
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("Scrolls of the Subliminal");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "Scrolls of the Subliminal");
        }

        public ScrollsOfTheSubliminal(Serial serial) : base(serial)
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
