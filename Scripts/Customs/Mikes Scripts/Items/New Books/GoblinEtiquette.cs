using System;
using Server;

namespace Server.Items
{
    public class GoblinEtiquette : BlueBook
    {
        public static readonly BookContent Content = new BookContent
        (
            "Goblin Etiquette", "Grot Nosebiter",
            new BookPageInfo
            (
                "As a refined goblin,",
                "I, Grot Nosebiter,",
                "have penned this",
                "guide. For too long,",
                "my kin are seen as",
                "uncouth! Herein lies",
                "the art of goblin",
                "politesse."
            ),
            new BookPageInfo
            (
                "First, introductions",
                "are a must: snarl",
                "your name, show your",
                "sharpest tooth. This",
                "displays vigor and",
                "strength! Weak teeth",
                "earn no respect in",
                "goblin society."
            ),
            new BookPageInfo
            (
                "In the dining pit,",
                "never grab the",
                "largest rat before",
                "the host does. It's",
                "terribly rude! And",
                "remember, belching",
                "is high praise to",
                "a chef's skills."
            ),
            new BookPageInfo
            (
                "Should you ever",
                "need to apologize,",
                "a gift of shiny",
                "objects or a tasty",
                "bug shows sincerity.",
                "Never offer your",
                "weapon; that's",
                "an insult."
            ),
            new BookPageInfo
            (
                "At a duel, it is",
                "customary to hiss",
                "and spit before",
                "bashing. Do not",
                "strike before this,",
                "lest you be branded",
                "a cheater and",
                "shunned."
            ),
            new BookPageInfo
            (
                "For courtship, a",
                "gentle goblin offers",
                "the choicest trash",
                "to his beloved. If",
                "she throws it at",
                "him, she is smitten!",
                "Should she eat it,",
                "wedding plans start."
            ),
            new BookPageInfo
            (
                "In matters of",
                "fashion, the latest",
                "trend is mud",
                "splatters on the",
                "left side. Right side",
                "splatters are a",
                "faux pas and last",
                "season's scandal!"
            ),
            new BookPageInfo
            (
                "Lastly, honor the",
                "elderly. Oldest",
                "goblins tell the",
                "best raiding stories",
                "and give the worst",
                "bites. Their wisdom",
                "and toothlessness",
                "are equally revered."
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
                "Heed these rules",
                "well, and ascend the",
                "ranks of goblin",
                "society with finesse.",
                "Ignore them at your",
                "peril, and may you be",
                "chased by the",
                "angry mob!"
            ),
            new BookPageInfo
            (
                "Grot Nosebiter",
                DateTime.Now.ToString("t"),
                DateTime.Now.ToString("d"),
                "Live long, eat bugs,",
                "and may your enemies'",
                "toes rot!"
            ),
			            new BookPageInfo
            (
                "Esteemed reader,",
                "this tome shall",
                "illuminate the refined",
                "complexities of goblin",
                "society, which I dare",
                "say, rival the courts",
                "of kings in",
                "sophistication."
            ),
            new BookPageInfo
            (
                "Alas, to the untrained",
                "eye, goblin culture",
                "may seem chaotic",
                "and brutish, but pay",
                "mind, there is order",
                "in the madness, and",
                "madness in the order."
            ),
            new BookPageInfo
            (
                "When you meet a",
                "goblin, the correct",
                "greeting is a grunt,",
                "followed by a display",
                "of your most recent",
                "scars. This exchange",
                "promotes both history",
                "and hierarchy."
            ),
            new BookPageInfo
            (
                "During feasts, always",
                "eat with your hands -",
                "utensils are frowned",
                "upon as they imply",
                "the food is not worth",
                "the fight to consume",
                "it with gusto and zeal."
            ),
            // Insert additional pages after previous content
            new BookPageInfo
            (
                "Goblin attire, while",
                "often dismissed as",
                "mere rags, is a",
                "delicate matter of",
                "status. The more",
                "patches one has, the",
                "braver they're deemed,",
                "for they survived"
            ),
            new BookPageInfo
            (
                "enough tussles to",
                "wear their history.",
                "Do note, however,",
                "new rags are a",
                "boast of recent",
                "plunders, and should",
                "be regarded with",
                "envy and suspicion."
            ),
            new BookPageInfo
            (
                "In matters of love,",
                "never underestimate",
                "the allure of a well-placed",
                "mud pie. A splat",
                "beneath the right eye",
                "is considered a",
                "proposal. Two splats,",
                "and you're engaged!"
            ),
            new BookPageInfo
            (
                "Trade and barter",
                "follow simple rules:",
                "He who snarls the",
                "loudest, or has the",
                "sharpest stick, often",
                "gets the best deal.",
                "Politeness is mistaken",
                "for weakness."
            ),
            new BookPageInfo
            (
                "On the subject of",
                "decorum, remember:",
                "when invited to a",
                "goblin's lair, it is",
                "customary to bring",
                "a gift. A fresh-caught",
                "rat or a shiny rock",
                "will suffice."
            ),
            new BookPageInfo
            (
                "Hospitality in goblin",
                "culture is paramount.",
                "A guest will be",
                "offered the finest",
                "seat of honor: a",
                "stump covered in",
                "the fur of something",
                "they likely helped kill."
            ),
            new BookPageInfo
            (
                "In conclusion, my dear",
                "reader, to navigate",
                "goblin society is to",
                "walk a razor's edge",
                "between being",
                "considered a friend",
                "and becoming",
                "dinner."
            ),
            new BookPageInfo
            (
                "Yet with this guide",
                "in hand, you shall",
                "stand tall among",
                "peers, equipped with",
                "the savoir-faire to",
                "engage with goblins",
                "without losing your",
                "fingers - or worse."
            ),
            // Add final words or summaries
            new BookPageInfo
            (
                "Thus armed with",
                "knowledge, may your",
                "dealings with goblins",
                "be prosperous, or at",
                "least, may you leave",
                "with more than you",
                "came with - and",
                "certainly with your life!"
            ),
            new BookPageInfo
            (
                // These pages left intentionally blank for owner's notes or expansion.
            ),
            new BookPageInfo
            (
            ),
            new BookPageInfo
            (
                "Penned by the",
                "honorable Grot",
                "Nosebiter, on this",
                "day, the evidence of",
                "our civility and",
                "refinement is laid",
                "bare for all to marvel."
            ),
            new BookPageInfo
            (
                "Grot Nosebiter",
                DateTime.Now.ToString("t"),
                DateTime.Now.ToString("d"),
                "Live fiercely, laugh",
                "heartily, and may your",
                "enemies never hear",
                "you coming."
            )
        );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public GoblinEtiquette() : base(false)
        {
            // Set the hue to a random greenish color, appropriate for goblin theme
            Hue = Utility.RandomMinMax(61, 71);
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("Goblin Etiquette");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "Goblin Etiquette");
        }

        public GoblinEtiquette(Serial serial) : base(serial)
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
