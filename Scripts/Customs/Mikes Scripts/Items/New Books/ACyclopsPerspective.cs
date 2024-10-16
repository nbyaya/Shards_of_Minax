using System;
using Server;

namespace Server.Items
{
    public class ACyclopsPerspective : BlueBook
    {
        public static readonly BookContent Content = new BookContent
        (
            "A Cyclops' Perspective", "Gronk the One-Eyed",
            new BookPageInfo
            (
                "I sit here with a large",
                "quill, borrowed from",
                "a giant bird, to scribe",
                "my thoughts. They call",
                "me Gronk the One-Eyed,",
                "a cyclops of simple taste,",
                "seeking solace in a world",
                "designed for the two-eyed."
            ),
            new BookPageInfo
            (
                "This tome is my attempt",
                "to offer a glimpse into",
                "my solitary life, to help",
                "the smaller folk understand",
                "that not all giants desire",
                "chaos and destruction.",
                "",
                "In the shadow of mountains,"
            ),
            new BookPageInfo
            (
                "I live, away from the bustling",
                "cities and the scurrying feet",
                "of humans and their kin.",
                "My home is the quiet earth,",
                "my companions the rocks,",
                "and my pastime the crafting",
                "of stone sculptures that",
                "stand as silent as I."
            ),
            new BookPageInfo
            (
                "Many fear me on sight,",
                "believing tales of my kin",
                "eating goats—or worse,",
                "men. Yet I dine on the",
                "bounty of the forest and",
                "take only what I need to",
                "sustain my hefty frame.",
                "Nature provides plenty."
            ),
            new BookPageInfo
            (
                "The loneliness of a cyclops",
                "is rarely spoken of. We are",
                "feared, shunned, avoided",
                "like a blight. Children",
                "are told to behave or",
                "be fed to us, as if we",
                "crave such small, bony meals.",
                "A gross misconception."
            ),
            new BookPageInfo
            (
                "Sometimes, I watch from",
                "afar as the smaller ones",
                "gather and laugh, wondering",
                "how it feels to be amidst",
                "their kind without fear",
                "or the need to hide.",
                "Would they laugh with me,",
                "or run in terror?"
            ),
            new BookPageInfo
            (
                "Yet, not all are cruel.",
                "Once, a child wandered",
                "near my domain, lost and",
                "weeping. I approached,",
                "cautiously, and led the",
                "young one to the edge",
                "of my land, pointing the",
                "way home. A rare touch"
            ),
            new BookPageInfo
            (
                "of warmth in my often",
                "cold existence. I ponder",
                "if the child remembers",
                "the gentle giant that",
                "one misty eve turned",
                "myth into protector.",
                "",
                "Such moments are precious,"
            ),
            new BookPageInfo
            (
                "fleeting and few. I commit",
                "them to memory, and now",
                "to paper, hoping they might",
                "endure longer than even",
                "stone. For even a cyclops,",
                "with his single eye, dreams",
                "of a day when the world",
                "will look back without fear."
            ),
            new BookPageInfo
            (
                "And so, I end my tale here,",
                "with the hope that it may",
                "find its way into the hands",
                "of one who seeks truth over",
                "fable. May it change a heart,",
                "or at least, entertain a mind",
                "willing to see through the",
                "eye of Gronk the One-Eyed."
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
                "Gronk the One-Eyed",
                DateTime.Now.ToString("t"),
                DateTime.Now.ToString("d"),
                "May your journeys be",
                "safe, and your hearts",
                "open to the tales of",
                "the misunderstood."
            ),
// ... [Previous BookPageInfo entries]
            new BookPageInfo
            (
                "In my solitude, I've",
                "befriended the mountain",
                "goats, whose clambering",
                "I watch with great amusement.",
                "They seem to pay me no mind,",
                "perhaps seeing in me a kinship,",
                "as we both tread paths that",
                "others cannot fathom."
            ),
            new BookPageInfo
            (
                "When the moon is full and",
                "bathes the world in its pale",
                "glow, I venture closer to",
                "civilization. Hidden by the",
                "cloak of night, I observe the",
                "lives I've long been excluded",
                "from, a silent giant amidst",
                "the dance of shadows."
            ),
            new BookPageInfo
            (
                "I've come to realize that",
                "the small folk carry their",
                "own burdens, fight their own",
                "giants. In their struggles,",
                "I see a reflection of my",
                "own. Is not all life a quest",
                "to find one's place within",
                "the tapestry of existence?"
            ),
            new BookPageInfo
            (
                "Once, during a fierce storm,",
                "a bolt of lightning struck a",
                "tree near my dwelling, setting",
                "it aflame. I watched in awe,",
                "knowing the same fire that",
                "burns can also cleanse. Perhaps",
                "such is the way of the world,",
                "even for a lone cyclops."
            ),
            new BookPageInfo
            (
                "Yet the fire drew near to",
                "my home, and I was forced",
                "to act. With great clods of",
                "earth and stone, I battled",
                "the blaze. By dawn's light,",
                "the fire was quelled, and",
                "I wondered if my actions",
                "mirrored those of heroes."
            ),
            new BookPageInfo
            (
                "Rumors began to spread of",
                "a 'gentle giant' amongst",
                "the hills. A myth to some,",
                "a ghost story to others, and",
                "yet to a few, perhaps a sign",
                "that not all is as it seems,",
                "that giants may have hearts",
                "as wide as their footprints."
            ),
            new BookPageInfo
            (
                "Artists came, driven by",
                "curiosity, to capture my",
                "form with brush and canvas.",
                "I sat still, as if a mountain",
                "itself, allowing them to",
                "steal my shadow. When they",
                "left, they seemed smaller,",
                "overwhelmed by the truth."
            ),
            new BookPageInfo
            (
                "In the deepest of nights,",
                "when I dare to dream, I",
                "envision a world where my",
                "voice is heard, not as a",
                "roar, but as a whisper,",
                "carrying tales of old,",
                "wisdom long forgotten by",
                "those who walk on two legs."
            ),
            new BookPageInfo
            (
                "Perhaps, when the stars are",
                "right, I will find another",
                "of my kind. Together, we",
                "could share the silence,",
                "understand the whispers of",
                "the earth, and no longer",
                "wander this world alone,",
                "but as two halves of a whole."
            ),
            new BookPageInfo
            (
                "Until such time, I hold to",
                "my routine, guarding the",
                "land that has become my",
                "charge. For though I did",
                "not choose this life of",
                "solitude, I will live it with",
                "the dignity befitting a",
                "creature of my stature."
            ),
            new BookPageInfo
            (
                "This book, my legacy, I leave",
                "in a hidden nook of the world,",
                "a treasure for an intrepid soul",
                "to find. Perhaps, in reading,",
                "you will see not a monster,",
                "but a being with a story, a",
                "life, a perspective uniquely",
                "my own—Gronk's truth."
            ),
            new BookPageInfo
            (
                "And if by chance we meet,",
                "on some star-crossed night,",
                "fear not. Extend your hand,",
                "though it may tremble, and",
                "look not into my eye, but my",
                "heart. Therein lies the kinship",
                "of all creatures, great and small."
            ),
            new BookPageInfo
            (
                "So ends my tale for now,",
                "etched into the pages of",
                "this tome. Remember me",
                "not as a myth, nor a beast,",
                "but as Gronk, the cyclops",
                "who saw the world through",
                "a lens of wonder, and hoped",
                "for a glimpse of acceptance."
            ),
            new BookPageInfo
            (
                "Gronk, the One-Eyed",
                DateTime.Now.ToString("t"),
                DateTime.Now.ToString("d"),
                "May your view be ever",
                "wide, your steps firm,",
                "and your heart open."
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
            )

        );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public ACyclopsPerspective() : base(false)
        {
            // Set the hue to a random color between 3001 and 6000
            Hue = Utility.RandomMinMax(3001, 6000);
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("A Cyclops' Perspective");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "A Cyclops' Perspective");
        }

        public ACyclopsPerspective(Serial serial) : base(serial)
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
