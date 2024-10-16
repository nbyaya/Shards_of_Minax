using System;
using Server;

namespace Server.Items
{
    public class TheCodeOfHonesty : BlueBook
    {
        public static readonly BookContent Content = new BookContent
        (
            "The Code of Honesty", "Sage Truthbearer",
            new BookPageInfo
            (
                "In an age where shadows",
                "thrive and deception",
                "runs rampant, the Code",
                "of Honesty stands as",
                "a beacon for the just",
                "and the true. Written",
                "by Sage Truthbearer,",
                "this tome imparts the"
            ),
            new BookPageInfo
            (
                "virtues of truth and",
                "integrity. Within these",
                "pages lie the principles",
                "for a forthright life,",
                "guiding the reader to",
                "a path of honor that",
                "is oft neglected in",
                "lesser tomes and by"
            ),
            new BookPageInfo
            (
                "lesser men. Honesty",
                "is not merely a virtue;",
                "it is the bedrock upon",
                "which trust is built,",
                "societies flourish, and",
                "individuals find inner",
                "peace. To live without",
                "deceit is to be free of"
            ),
            new BookPageInfo
            (
                "the burdens that tie",
                "the tongues of lesser",
                "beings. This book is",
                "a humble attempt to",
                "resurrect the ancient",
                "ways of living in truth,",
                "where one's word is",
                "their bond, and truth"
            ),
            new BookPageInfo
            (
                "is their guiding star.",
                "",
                "Chapter One speaks of",
                "Honesty's Shield:",
                "against the slings and",
                "arrows of outrageous",
                "fortune, honesty grants",
                "a defense no armor could."
            ),
            new BookPageInfo
            (
                "Chapter Two details",
                "Honesty's Path: a",
                "journey that is clear",
                "and unobstructed by",
                "the webs of deceit",
                "that entangle the feet",
                "of liars, leading only",
                "to their downfall."
            ),
            new BookPageInfo
            (
                "The following chapters",
                "continue to lay down",
                "the stones on which",
                "one may walk a life of",
                "sincerity and openness,",
                "never needing to glance",
                "over one's shoulder",
                "for fear of mistruths"
            ),
            new BookPageInfo
            (
                "told. Let it be known",
                "that to follow the Code",
                "of Honesty is to lead",
                "a life of harmony with",
                "the world, with others,",
                "and most importantly,",
                "with oneself.",
                ""
            ),
            new BookPageInfo
            (
                "Let this book serve",
                "as your compass to",
                "navigate the often",
                "turbulent waters of",
                "doubt and guile. Let",
                "it guide you towards",
                "calm shores where",
                "one's spirit may rest."
            ),
            new BookPageInfo
            (
                "For the honest heart",
                "knows no turmoil,",
                "and the truthful soul",
                "knows no fear. In",
                "truth, there is power;",
                "in honesty, there is",
                "freedom. May you",
                "embrace these words."
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
                "Sage Truthbearer",
                DateTime.Now.ToString("t"),
                DateTime.Now.ToString("d"),
                "Live in truth, and",
                "truth shall live in you."
            ),
            new BookPageInfo
            (
                "Chapter Three unfolds",
                "Honesty's Light: that",
                "which reveals the truth",
                "of all things, banishing",
                "the darkness of deceit",
                "and guiding the lost.",
                "",
                "Chapter Four reveals"
            ),
            new BookPageInfo
            (
                "Honesty's Heart: the",
                "inner courage that is",
                "fostered by living a",
                "life without lies, a",
                "strength that upholds",
                "the just and emboldens",
                "the spirit.",
                ""
            ),
            new BookPageInfo
            (
                "Chapter Five describes",
                "Honesty's Echo: the",
                "legacy that one leaves",
                "behind, the untarnished",
                "reputation that echoes",
                "through time long after",
                "one's final departure.",
                ""
            ),
            new BookPageInfo
            (
                "The subsequent chapters",
                "illuminate the principles",
                "by which an honest life",
                "can be achieved, a life",
                "unshackled by the common",
                "fetters of guile and",
                "misdirection.",
                ""
            ),
            new BookPageInfo
            (
                "In embracing these tenets,",
                "we find not just solace",
                "but strength. The truth",
                "is a shield, not of steel,",
                "but of incorruptible",
                "spirit that no physical",
                "force can dent or rust.",
                ""
            ),
            new BookPageInfo
            (
                "In these trying times,",
                "let the Code be a",
                "compass, a lighthouse",
                "amidst the fog of",
                "falsehoods. Let it be",
                "a declaration of our",
                "highest selves, a",
                "commitment to live"
            ),
            new BookPageInfo
            (
                "out the truth in our",
                "hearts, to speak it with",
                "our lips, to demonstrate",
                "it through our deeds.",
                "May the Code of Honesty",
                "guide you to a life",
                "worthy of the legacy",
                "you wish to bestow."
            ),
            new BookPageInfo
            (
                "The final chapters of",
                "this book are dedicated",
                "to the application of",
                "honesty in daily life;",
                "how to be honest with",
                "oneself, with others, and",
                "with the world at large.",
                ""
            ),
            new BookPageInfo
            (
                "For honesty is not",
                "merely the avoidance of",
                "falsehood, but the active",
                "pursuit of truth in all",
                "things: in thought, in",
                "speech, and in action.",
                "Thus concludes the",
                "Code of Honesty."
            ),
            new BookPageInfo
            (
                "Embrace these teachings",
                "and let them transform",
                "you. Walk the path of",
                "integrity, and leave",
                "behind footsteps that",
                "others may follow with",
                "confidence and hope.",
                "This is the true quest."
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
                "Sage Truthbearer",
                DateTime.Now.ToString("t"),
                DateTime.Now.ToString("d"),
                "Live in truth, and",
                "truth shall live in you."
            )
        );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public TheCodeOfHonesty() : base(false)
        {
            // Set the hue to a random color between 1 and 3000
            Hue = Utility.RandomMinMax(1, 3000);
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("The Code of Honesty");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "The Code of Honesty");
        }

        public TheCodeOfHonesty(Serial serial) : base(serial)
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
