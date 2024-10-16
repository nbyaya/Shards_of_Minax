using System;
using Server;

namespace Server.Items
{
    public class OrcishStandUpComedy : BlueBook
    {
        public static readonly BookContent Content = new BookContent
        (
            "Orcish Stand-Up Comedy", "Grubnar the Witty",
            new BookPageInfo
            (
                "This tome contains the",
                "greatest collection of",
                "Orcish humor, jests,",
                "and gags known",
                "throughout the lands.",
                "I am Grubnar, a unique",
                "sight - an orc who",
                "wields laughter like"
            ),
            new BookPageInfo
            (
                "a sword! Prepare to",
                "grin, chuckle, or even",
                "guffaw! But be",
                "warned, some jokes",
                "may cause your sides",
                "to split as if struck by",
                "a mighty orcish axe!",
                ""
            ),
            new BookPageInfo
            (
                "Why did the human",
                "cross the road? To",
                "flee from our war",
                "drums! Ha! But",
                "seriously, humans",
                "are always running.",
                "Why not face us",
                "in glorious battle?"
            ),
            new BookPageInfo
            (
                "How many elves does",
                "it take to sharpen",
                "a sword? Just one,",
                "but it takes a whole",
                "saga of singing,",
                "weeping, and",
                "poetry recital!",
                "Elves... So dramatic!"
            ),
            new BookPageInfo
            (
                "What do you call",
                "a goblin who has",
                "lost his way? A",
                "better goblin!",
                "Everyone knows the",
                "best goblins are the",
                "ones you can't find!",
                ""
            ),
            new BookPageInfo
            (
                "Knock knock!",
                "Who's there?",
                "Orc.",
                "Orc who?",
                "Orc gonna smash",
                "your door if you",
                "don't let us in!",
                "Ha! Classic!"
            ),
            new BookPageInfo
            (
                "Why don't orcs play",
                "cards in the jungle?",
                "Too many cheetahs!",
                "But truly, it is",
                "because the trees",
                "are always peeking",
                "at our hands!",
                ""
            ),
            new BookPageInfo
            (
                "An orc and a troll",
                "are roasting boar.",
                "Troll says, 'Meat's",
                "burnt.' Orc says,",
                "'Good, now it's",
                "just like my love",
                "life!' Who knew",
                "orcs had romance?"
            ),
            new BookPageInfo
            (
                "Orcish cooking show",
                "would be short. 'Take",
                "meat. Put on fire.",
                "Eat. Show over.'",
                "Why complicate",
                "perfection? Meat,",
                "fire, done!",
                ""
            ),
            new BookPageInfo
            (
                "And finally... Why",
                "do we, orcs, always",
                "carry a weapon?",
                "Because you can't",
                "kill a dragon with",
                "laughter! Yet...",
                "Hah! But we can try!",
                ""
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
                "Behold, more belly-laughs",
                "from Grubnar's collection",
                "of orcish humor. Let no",
                "one say an orc can't tickle",
                "your funny bone - right",
                "before he breaks it!",
                "Ready? Then let's crack on!",
                ""
            ),
            new BookPageInfo
            (
                "What's an orc's favorite",
                "instrument? The dinner",
                "bell! But after that,",
                "it's the drum. Nothing",
                "beats a good retreat,",
                "after all... unless we",
                "win, then everything",
                "does!"
            ),
            new BookPageInfo
            (
                "If you ever hear an orc",
                "with a joke about",
                "pebbles, don't listen.",
                "It's probably just",
                "some form of rock",
                "music. Yes, we're",
                "modern like that!",
                ""
            ),
            new BookPageInfo
            (
                "Why did the orc stop",
                "writing poetry? Because",
                "rhymes are less fun when",
                "you can't end every",
                "line with 'smash'!",
                "Orcish literature –",
                "short but impactful!",
                ""
            ),
            new BookPageInfo
            (
                "I asked a shaman how",
                "many spells he knew.",
                "He said, 'A lot'. But",
                "he just sat there,",
                "so I guess 'a lot'",
                "means zero in shaman!",
                "Wise creatures, shamans...",
                "sometimes too wise."
            ),
            new BookPageInfo
            (
                "Ever seen a goblin",
                "try to ride a warg?",
                "It's like a flea trying",
                "to steer a dog! But",
                "less biting and more",
                "screaming. Oh, the",
                "screaming is music!",
                ""
            ),
            new BookPageInfo
            (
                "There was an orc who",
                "swore he'd eat his",
                "hat if he lost a bet.",
                "Good thing orcs don't",
                "wear hats. Instead, he",
                "just had a nice bowl of",
                "grub stew! Problem solved!",
                ""
            ),
            new BookPageInfo
            (
                "They say time flies like",
                "an arrow, but fruit flies",
                "like a banana. Well, in",
                "orc camps, everything",
                "flies like a thrown rock!",
                "It's our answer to",
                "aerodynamics.",
                ""
            ),
            new BookPageInfo
            (
                "Once an orc tried to",
                "count to ten. It was",
                "going well until we",
                "realized he was counting",
                "his fingers. Never got",
                "past nine after that",
                "forge accident...",
                ""
            ),
            new BookPageInfo
            (
                "If you give an orc a fish,",
                "he'll eat for a day. If",
                "you teach an orc to fish,",
                "he'll hit the fish with",
                "a stick. If you show an",
                "orc a fishing rod, he'll",
                "probably eat that too!",
                ""
            ),
            new BookPageInfo
            (
                "We once captured a bard.",
                "He offered to play for",
                "his freedom. After his",
                "first song, we freed him",
                "just to make it stop.",
                "Worst. Drummer. Ever.",
                "But a good singer!",
                ""
            ),
            new BookPageInfo
            (
                "How do you know if a",
                "goblin is lying? His",
                "lips are moving! And",
                "also, he's probably",
                "stealing your boots",
                "while he does it.",
                "Crafty little things!",
                ""
            ),
            new BookPageInfo
            (
                "My friend said I had",
                "no understanding of",
                "irony. Which was ironic",
                "because we were at",
                "the blacksmith's at the",
                "time. No wait, that's",
                "just coincidence...",
                "Irony's the other one!"
            ),
            new BookPageInfo
            (
                "What's the best thing",
                "about orcish cuisine?",
                "If you don't like it,",
                "you can always just eat",
                "the cook. Just kidding!",
                "We respect our chefs.",
                "They fight back.",
                ""
            ),
            new BookPageInfo
            (
                "An elf once told me",
                "patience is a virtue.",
                "So I patiently waited",
                "before challenging him",
                "to a duel. He then told",
                "me discretion is the",
                "better part of valor.",
                "He was full of advice!"
            ),
            new BookPageInfo
            (
                "They say you can't get",
                "blood from a stone.",
                "But give an orc a big",
                "enough hammer and",
                "he'll get blood from",
                "anything!",
                "Persistence – our",
                "secret ingredient."
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
                "Grubnar the Witty",
                DateTime.Now.ToString("t"),
                DateTime.Now.ToString("d"),
                "Remember, if you can't",
                "laugh at yourself, an orc",
                "will do it for you...",
                "right before the duel!"
            ),
            new BookPageInfo
            (
                "Grubnar the Witty",
                DateTime.Now.ToString("t"),
                DateTime.Now.ToString("d"),
                "May your battles be",
                "fierce and your",
                "laughter be hearty."
            )
        );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public OrcishStandUpComedy() : base(false)
        {
            // Set the hue to a random color between 1 and 3000
            Hue = Utility.RandomMinMax(1, 3000);
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("Orcish Stand-Up Comedy");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "Orcish Stand-Up Comedy");
        }

        public OrcishStandUpComedy(Serial serial) : base(serial)
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
