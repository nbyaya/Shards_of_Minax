using System;
using Server;

namespace Server.Items
{
    public class ABCsForBarbarians : BlueBook
    {
        public static readonly BookContent Content = new BookContent
            (
                "ABCs for Barbarians", "Throg",
                new BookPageInfo
                (
                    "A is for Axe, the tool",
                    "of our trade.",
                    "B is for Battle, where",
                    "legends are made.",
                    "C is for Clan, a",
                    "family so grand.",
                    "",
                    "       -Throg"
                ),
                new BookPageInfo
                (
                    "D is for Drum, we play",
                    "at the fire.",
                    "E is for Enemy, earns",
                    "our ire.",
                    "F is for Fight, 'tis",
                    "what we adore.",
                    "G is for Grunt, a sound",
                    "you can't ignore."
                ),
                new BookPageInfo
                (
                    "H is for Hunt, our way",
                    "to survive.",
                    "I is for Iron, makes",
                    "weapons alive.",
                    "J is for Jump, across",
                    "the stream.",
                    "K is for Kill, the",
                    "end of a dream."
                ),
				new BookPageInfo
				(
					"L is for Lute, a bard's",
					"best friend.",
					"M is for Mead, brings",
					"joy without end.",
					"N is for Night, when",
					"wolves are about.",
					"O is for Ogre, we'll",
					"knock him out."
				),
				new BookPageInfo
				(
					"P is for Potion, for",
					"quick healing.",
					"Q is for Quest, a life",
					"with meaning.",
					"R is for Raid, where",
					"we take our due.",
					"S is for Sword, sharp",
					"and true."
				),
				new BookPageInfo
				(
					"T is for Tribe, where",
					"we all belong.",
					"U is for Uproar, when",
					"chants grow strong.",
					"V is for Victory, a",
					"warrior's delight.",
					"W is for Warcry, we",
					"shout in the fight."
				),
				new BookPageInfo
				(
					"X is for Xebec, a ship",
					"on the sea.",
					"Y is for Yell, loud as",
					"can be.",
					"Z is for Zeal, in",
					"every swing.",
					"And that, dear friends,",
					"is the end of this thing."
				),
                new BookPageInfo
                (
                    "And so it continues,",
                    "through the rest",
                    "of the letters.",
                    "Learning these will",
                    "make you all betters.",
                    "Go forth, my clan,",
                    "live large and wild.",
                    "And remember to always",
                    "nurture your inner child."
                )
            );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public ABCsForBarbarians() : base(false)
        {
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("ABCs for Barbarians");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "ABCs for Barbarians");
        }

        public ABCsForBarbarians(Serial serial) : base(serial)
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
