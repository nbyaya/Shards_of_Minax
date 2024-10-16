using System;
using Server;

namespace Server.Items
{
    public class OdesToTheObsidianOgres : BlueBook
    {
        public static readonly BookContent Content = new BookContent
        (
            "Odes to the Obsidian Ogres", "Grondar the Bard",
            new BookPageInfo
            (
                "In shadowed hills and",
                "twilight's deep gloam,",
                "Obsidian Ogres find",
                "their heart's home.",
                "With skin like night,",
                "and eyes like coals,",
                "They roam the wilds,",
                "guarding ancient shoals."
            ),
            new BookPageInfo
            (
                "No simple brute, nor",
                "mindless beast;",
                "Their wisdom old, a",
                "sage's feast.",
                "With every step, the",
                "earth it quakes,",
                "And in their wake,",
                "silence breaks."
            ),
            new BookPageInfo
            (
                "Yet, sing I will of",
                "their hidden grace,",
                "The gentle giants of",
                "a fierce-faced race.",
                "An ode to their might,",
                "their unyielding will,",
                "The mountains bow,",
                "the rivers still."
            ),
            new BookPageInfo
            (
                "By moon's soft glow or",
                "sun's harsh light,",
                "They craft from obsidian,",
                "their armor bright.",
                "No steel could cleave,",
                "nor magic blight,",
                "The Ogre's garb, in",
                "battle's night."
            ),
            new BookPageInfo
            (
                "Their laughter booms,",
                "across fields wide,",
                "A sound to charm,",
                "to warm the inside.",
                "Yet cross not one, or",
                "you shall find,",
                "The fury of their",
                "kindled mind."
            ),
            new BookPageInfo
            (
                "In cavern's deep, where",
                "crystals gleam,",
                "Ogres ponder, dream",
                "their silent dream.",
                "Of worlds that were,",
                "and yet might be,",
                "Their songs a balm,",
                "a memory."
            ),
            new BookPageInfo
            (
                "So here's my ode to",
                "Obsidian kin,",
                "Who walk 'neath stars,",
                "in night's soft hymn.",
                "May we learn much from",
                "their stoic ways,",
                "And sing their praise,",
                "till end of days."
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
                "Grondar the Bard",
                DateTime.Now.ToString("t"),
                DateTime.Now.ToString("d"),
                "In awe of the night's",
                "silent guardians."
            ),
// ...previous code...
            new BookPageInfo
            (
                "Tales are told 'neath",
                "the tavern's glow,",
                "Of ogres' deeds and",
                "the debts we owe.",
                "For many a battle they",
                "have turned the tide,",
                "And side by side with",
                "men, they've died."
            ),
            new BookPageInfo
            (
                "With hearts as vast as",
                "their homeland's skies,",
                "Within their chests, no",
                "cowardice lies.",
                "And though they love not",
                "the sword's sharp kiss,",
                "In war, they grant foes",
                "no bliss."
            ),
            new BookPageInfo
            (
                "Yet peace they cherish,",
                "like the earth's deep gems,",
                "A tranquility that never",
                "condemns.",
                "In fields, they toil,",
                "In forests, they roam,",
                "Under the mountain's",
                "brow, their home."
            ),
            new BookPageInfo
            (
                "They speak in rumbles,",
                "like the storm's own voice,",
                "In the language of the",
                "wild, the ogre's choice.",
                "And when they dance,",
                "the world does yield,",
                "To the thumping rhythm",
                "of the unconquered shield."
            ),
            new BookPageInfo
            (
                "But ogres dream not",
                "just of war's loud ring;",
                "Of simpler things does",
                "their heart sing.",
                "Of love's sweet clasp,",
                "and offspring's laughter,",
                "Of the life's warmth",
                "they're always after."
            ),
            new BookPageInfo
            (
                "Their hands can crush",
                "stone, bend iron bars,",
                "Yet gently cradle",
                "the night's soft stars.",
                "In those great palms",
                "lies untold power,",
                "Like the silent strength",
                "of the midnight hour."
            ),
            new BookPageInfo
            (
                "So listen well when",
                "the ogres speak,",
                "For wisdom lies in the",
                "truth they seek.",
                "And if you chance to",
                "meet one's gaze,",
                "You'll find a depth as",
                "enduring as days."
            ),
            new BookPageInfo
            (
                "Remember, traveler,",
                "in your wide roam,",
                "The obsidian ogres walk",
                "where secrets foam.",
                "If kinship you offer,",
                "true and sincere,",
                "An ogre's friendship",
                "you will endear."
            ),
            new BookPageInfo
            (
                "So ends my ode to",
                "these gentle kin,",
                "Whose tales are woven",
                "into the world's skin.",
                "Next time the mountain's",
                "shadow you cross,",
                "Recall the ogres, and",
                "their almighty force."
            ),
            new BookPageInfo
            (
                "May this book find you",
                "in good health,",
                "And bring you some joy,",
                "or a measure of wealth.",
                "For in these pages lies",
                "the ogres' song;",
                "A reminder that even",
                "the mighty can be kind."
            ),
            new BookPageInfo
            (
                // These pages left intentionally blank for owner's notes or drawings.
            ),
            new BookPageInfo
            (
            ),
            new BookPageInfo
            (
            ),
            new BookPageInfo
            (
                "Grondar the Bard",
                DateTime.Now.ToString("t"),
                DateTime.Now.ToString("d"),
                "Humbly, for the Obsidian Ogres."
            )

        );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public OdesToTheObsidianOgres() : base(false)
        {
            // Set the hue to a random obsidian-like color
            Hue = 1175;
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("Odes to the Obsidian Ogres");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "Odes to the Obsidian Ogres");
        }

        public OdesToTheObsidianOgres(Serial serial) : base(serial)
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
