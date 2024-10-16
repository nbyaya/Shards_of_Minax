using System;
using Server;

namespace Server.Items
{
    public class HistoryOfMongbats : BlueBook
    {
        public static readonly BookContent Content = new BookContent
            (
                "History of Mongbats", "A Scholar",
                new BookPageInfo
                (
                    "This tome is dedicated",
                    "to the enigmatic",
                    "creatures known as",
                    "Mongbats. They are",
                    "often dismissed as",
                    "mere pests, but they",
                    "have a rich history",
                    "worth exploring."
                ),
                new BookPageInfo
                (
                    "Mongbats are winged",
                    "mammals that inhabit",
                    "various forests and",
                    "caves. Though they",
                    "may appear menacing",
                    "at first glance, they",
                    "are generally more",
                    "afraid of you."
                ),
                new BookPageInfo
                (
                    "Their social structure",
                    "is fascinating. They",
                    "live in groups known",
                    "as 'clans', each led",
                    "by an elder Mongbat.",
                    "Communication is",
                    "done through a",
                    "series of chirps."
                ),
                new BookPageInfo
                (
                    "Contrary to popular",
                    "belief, Mongbats are",
                    "not inherently evil.",
                    "They are protective",
                    "of their territory",
                    "and will usually",
                    "avoid human contact",
                    "if possible."
                ),
                new BookPageInfo
                (
                    "However, the encroach-",
                    "ment of civilization",
                    "into their habitats",
                    "has led to increased",
                    "conflicts with humans.",
                    "It is a sad but",
                    "inescapable truth."
                ),
                new BookPageInfo
                (
                    "So next time you find",
                    "yourself face to face",
                    "with a Mongbat, pause",
                    "and consider its rich",
                    "and storied history",
                    "before drawing your",
                    "weapon."
                ),
                new BookPageInfo
                (
                    "Mongbats are not only",
                    "known for their agility",
                    "in flight but also for",
                    "their keen senses.",
                    "They have a sharp",
                    "sense of smell and",
                    "acute hearing, which",
                    "are vital for survival."
                ),
                new BookPageInfo
                (
                    "Many adventurers have",
                    "often wondered about",
                    "the diet of a Mongbat.",
                    "They primarily feed",
                    "on small rodents and",
                    "insects but have been",
                    "known to eat fruits",
                    "and plants."
                ),
                new BookPageInfo
                (
                    "The mating season of",
                    "the Mongbats is a",
                    "unique spectacle.",
                    "The males perform",
                    "elaborate dances in",
                    "the air to attract",
                    "females. After the",
                    "mating, eggs are laid."
                ),
                new BookPageInfo
                (
                    "Mongbat eggs are",
                    "coveted by many for",
                    "their supposed",
                    "magical properties.",
                    "However, this has",
                    "led to overhunting",
                    "and decline in some",
                    "Mongbat populations."
                ),
                new BookPageInfo
                (
                    "The Mongbat language",
                    "is not well understood",
                    "but researchers are",
                    "making progress. It is",
                    "thought that different",
                    "chirps and gestures",
                    "can signify danger,",
                    "food, or mating."
                ),
                new BookPageInfo
                (
                    "In folklore, Mongbats",
                    "have often been",
                    "associated with dark",
                    "magic. While there is",
                    "no conclusive evidence",
                    "of this, the myth",
                    "persists in many",
                    "communities."
                ),
                new BookPageInfo
                (
                    "Lastly, it's worth",
                    "noting that Mongbats",
                    "have often been used",
                    "in alchemy. Their",
                    "wings and fur are",
                    "said to have various",
                    "properties, although",
                    "such claims are dubious."
                ),
                new BookPageInfo
                (
                    "So ends our exploration",
                    "into the life and habits",
                    "of Mongbats. May this",
                    "book serve as a guide",
                    "to understanding these",
                    "misunderstood creatures."
                )

            );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public HistoryOfMongbats() : base(false)
        {
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("History of Mongbats");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "History of Mongbats");
        }

        public HistoryOfMongbats(Serial serial) : base(serial)
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
