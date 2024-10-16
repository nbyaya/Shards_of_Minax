using System;
using Server;

namespace Server.Items
{
    public class TheAgeOfShadows : BlueBook
    {
        public static readonly BookContent Content = new BookContent
            (
                "The Age of Shadows", "Aleric",
                new BookPageInfo
                (
                    "In ancient times,",
                    "Sosaria was a land",
                    "shrouded in mystery",
                    "and magic, a place",
                    "where dragons roamed",
                    "and heroes were",
                    "forged.",
                    "          -Aleric"
                ),
                new BookPageInfo
                (
                    "Before the rise of",
                    "kingdoms, an age",
                    "called the Age of",
                    "Shadows existed.",
                    "Darkness and light",
                    "danced in a delicate",
                    "balance, giving birth",
                    "to both good and evil."
                ),
                new BookPageInfo
                (
                    "It was an era when",
                    "the first daemons",
                    "were summoned and",
                    "when the Virtues were",
                    "not yet defined.",
                    "Archmages and",
                    "warriors were still",
                    "learning the bounds"
                ),
                new BookPageInfo
                (
                    "of their powers, and",
                    "civilizations were",
                    "young. Hidden away",
                    "were artifacts of",
                    "immense power, and",
                    "creatures of legend",
                    "still roamed freely.",
                    "Adventurers sought"
                ),
                new BookPageInfo
                (
                    "treasures and fame,",
                    "often stumbling upon",
                    "horrors that defied",
                    "imagination. Yet,",
                    "these tales formed",
                    "the core myths and",
                    "legends that would",
                    "guide future"
                ),
                new BookPageInfo
                (
                    "generations.",
                    "",
                    "And so, as you tread",
                    "upon the soils of",
                    "Sosaria, remember,",
                    "you are walking on",
                    "lands rich in history",
                    "and steeped in the"
                ),
                new BookPageInfo
                (
                    "legends of the Age of",
                    "Shadows. Its echoes",
                    "still reverberate",
                    "through dungeons",
                    "deep and forests",
                    "dark, awaiting those",
                    "bold enough to",
                    "uncover its secrets."
                ),
				new BookPageInfo
				(
					"In the earliest days,",
					"many different races",
					"shared Sosaria, each",
					"with their own lore.",
					"The Elves were first",
					"to master the arts of",
					"magic, while Dwarves",
					"excelled in craftsmanship."
				),
				new BookPageInfo
				(
					"The Age of Shadows",
					"wasn't just a time",
					"of darkness, but",
					"also one of great",
					"enlightenment. Magic",
					"was studied not as a",
					"tool, but as an art,",
					"a pure expression of"
				),
				new BookPageInfo
				(
					"one's soul. And yet,",
					"magic also led to",
					"corruption. The first",
					"Necromancers rose,",
					"distorting the balance",
					"and bringing forth",
					"creatures from",
					"nightmarish realms."
				),
				new BookPageInfo
				(
					"During this era, the",
					"first guilds and",
					"orders were formed.",
					"Mages of virtue",
					"banded together to",
					"stop the corrupting",
					"spread of dark magic.",
					"Thus, the Mage's Guild"
				),
				new BookPageInfo
				(
					"was founded, a beacon",
					"of light in an age",
					"swirling with shadows.",
					"Warriors also began",
					"to organize, creating",
					"the first Paladins,",
					"knights sworn to",
					"defend the realms."
				),
				new BookPageInfo
				(
					"The end of the Age",
					"came not in a blaze",
					"of glory, but in",
					"whispers and tales.",
					"It is said a great",
					"cataclysm was averted",
					"by unnamed heroes,",
					"forever lost to history."
				),
				new BookPageInfo
				(
					"The Age of Shadows",
					"left an indelible mark",
					"on Sosaria, its echoes",
					"felt in every spell",
					"cast, each sword",
					"swung, and in the",
					"hearts of all who",
					"dare to dream."
				)

            );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public TheAgeOfShadows() : base(false)
        {
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("The Age of Shadows");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "The Age of Shadows");
        }

        public TheAgeOfShadows(Serial serial) : base(serial)
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
