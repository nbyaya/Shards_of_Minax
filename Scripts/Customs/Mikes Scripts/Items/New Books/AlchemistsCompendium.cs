using System;
using Server;

namespace Server.Items
{
    public class AlchemistsCompendium : BlueBook
    {
        public static readonly BookContent Content = new BookContent
            (
                "The Alchemist's Compendium", "Alchemius",
                new BookPageInfo
                (
                    "Greetings, aspiring",
                    "alchemist! This",
                    "compendium aims to",
                    "educate you on the",
                    "art of alchemy.",
                    "Whether you're a",
                    "beginner or an",
                    "expert, read on!"
                ),
                new BookPageInfo
                (
                    "Essential Ingredients:",
                    "Reagents are vital",
                    "for any alchemical",
                    "experiment. Common",
                    "reagents include",
                    "Garlic, Ginseng,",
                    "and Mandrake root."
                ),
                new BookPageInfo
                (
                    "Potions:",
                    "Health potions",
                    "require Ginseng and",
                    "Garlic. Mana potions",
                    "need Black Pearl and",
                    "Mandrake root."
                ),
                new BookPageInfo
                (
                    "Tools of the Trade:",
                    "You will need a",
                    "mortar and pestle,",
                    "as well as vials and",
                    "an alchemy table."
                ),
                new BookPageInfo
                (
                    "Safety Measures:",
                    "Always wear",
                    "protective gear and",
                    "work in a well-",
                    "ventilated area."
                ),
                new BookPageInfo
                (
                    "Health Potion Recipe:",
                    "1 Ginseng, 1 Garlic.",
                    "Grind them together",
                    "and add to a vial of",
                    "water. Heat slowly."
                ),
                new BookPageInfo
                (
                    "Mana Potion Recipe:",
                    "1 Black Pearl, 1",
                    "Mandrake Root.",
                    "Combine and mix",
                    "into a vial of water.",
                    "Chill before use."
                ),
                new BookPageInfo
                (
                    "Cure Potion Recipe:",
                    "Garlic and Ginseng",
                    "ground together and",
                    "mixed with water.",
                    "Drink immediately."
                ),
                new BookPageInfo
                (
                    "Advanced Alchemy:",
                    "Enhanced potions",
                    "are possible but",
                    "require more skill",
                    "and rare reagents.",
                    "Be cautious!"
                ),
                new BookPageInfo
                (
                    "Conclusion:",
                    "Alchemy is an art",
                    "that rewards those",
                    "who experiment",
                    "and practice.",
                    "May your vials",
                    "never shatter!"
                )
            );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public AlchemistsCompendium() : base(false)
        {
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("The Alchemist's Compendium");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "The Alchemist's Compendium");
        }

        public AlchemistsCompendium(Serial serial) : base(serial)
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
