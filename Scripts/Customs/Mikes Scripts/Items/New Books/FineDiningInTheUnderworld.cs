using System;
using Server;

namespace Server.Items
{
    public class FineDiningInTheUnderworld : BlueBook
    {
        public static readonly BookContent Content = new BookContent
        (
            "Fine Dining in the Underworld", "Chef Bordon the Bold",
            new BookPageInfo
            (
                "When one thinks of the",
                "Underworld, fine dining",
                "is seldom the first",
                "thought to arise. Yet,",
                "within the depths of",
                "shadow and flame,",
                "there lies cuisine",
                "worth the peril."
            ),
            new BookPageInfo
            (
                "This tome is a collection",
                "of my culinary journeys",
                "and the exotic, sometimes",
                "dangerous recipes I've",
                "gathered. From the",
                "volcanic kitchens of",
                "fire giants to the",
                "banquets of wraiths."
            ),
            new BookPageInfo
            (
                "Dining with demons",
                "demands a certain",
                "boldness, but oh, the",
                "rewards! The Searing",
                "Souffle, a dish that",
                "could roast a boar",
                "from the inside, yet",
                "delights the palate."
            ),
            new BookPageInfo
            (
                "Or the Ethereal Pudding,",
                "a dessert that phases",
                "through the very essence",
                "of one's being, leaving",
                "a taste of pure joy",
                "and a touch of",
                "melancholy for the",
                "mortal coil."
            ),
            new BookPageInfo
            (
                "Let us not forget the",
                "Spectral Stew, brewed",
                "from the essence of",
                "the Underworld itself;",
                "a dish to invigorate",
                "even the weariest",
                "of souls. Yet, be",
                "wary of the portion."
            ),
            new BookPageInfo
            (
                "One too many a spoonful,",
                "and you may find yourself",
                "part of the spectral",
                "realm indefinitely.",
                "This guide shall lead",
                "you through the",
                "preparation of such",
                "delicacies."
            ),
            new BookPageInfo
            (
                "To dine in the",
                "Underworld is to",
                "challenge death itself,",
                "for the ingredients",
                "are oft as perilous",
                "to collect as they are",
                "wondrous to taste.",
                "But fear not,"
            ),
            new BookPageInfo
            (
                "for I, Bordon the Bold,",
                "have braved these",
                "hazards to bring you",
                "these recipes. May your",
                "spoon stir true, and",
                "your tastes be ever",
                "bold. To the brave",
                "go the feast!"
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
                "Chef Bordon the Bold",
                DateTime.Now.ToString("t"),
                DateTime.Now.ToString("d"),
                "May your meals be",
                "as adventurous as",
                "your travels."
            ),
			            new BookPageInfo
            (
                "Amongst the damned and",
                "the cursed, one may",
                "find the Midnight",
                "Mushroom. Plucked from",
                "the eternal dark, it",
                "shimmers with a ghostly",
                "light and tastes of",
                "forgotten dreams."
            ),
            new BookPageInfo
            (
                "Preparation is a risky",
                "affair, for one must",
                "avoid the attention of",
                "jealous specters. Cooked",
                "in the silent flames of",
                "a banshee's wail, this",
                "fungus can grant visions",
                "or eternal sleep."
            ),
            new BookPageInfo
            (
                "For those with a",
                "fondness for heat,",
                "the Phoenix Feather",
                "Chili is a test of",
                "mettle and spirit.",
                "A single bite can",
                "ignite a fire within",
                "that burns away all woes."
            ),
            new BookPageInfo
            (
                "But beware, this chili",
                "must be consumed in",
                "the presence of a cleric,",
                "for its resurrection",
                "properties are not",
                "limited to the mythical",
                "bird from which",
                "it gets its zest."
            ),
            new BookPageInfo
            (
                "Dessert shall not be",
                "neglected, for the",
                "Underworld's sweets",
                "are decadent. The",
                "Nectar of the Styx",
                "is a brew of haunting",
                "beauty, said to",
                "soothe even Cerberus."
            ),
            new BookPageInfo
            (
                "Partake with caution,",
                "for the river from",
                "which it flows offers",
                "forgetfulness, and",
                "overeager indulgence",
                "may lead to a loss",
                "of past memories and",
                "future desires."
            ),
            new BookPageInfo
            (
                "We must speak of the",
                "Abyssal Trencher, a meal",
                "served on a platter",
                "carved from obsidian.",
                "Featuring the rarest",
                "of meats from beasts",
                "long extinct in the",
                "world above."
            ),
            new BookPageInfo
            (
                "Accompanied by the",
                "sighs of the condemned,",
                "each mouthful is a",
                "sojourn into history,",
                "a taste of power",
                "once great, now",
                "humbled and served",
                "for your repast."
            ),
            new BookPageInfo
            (
                "As a digestif, let",
                "us not forget the",
                "Elixir of Lethargy,",
                "a concoction so potent,",
                "time seems to halt",
                "with each sip. A",
                "favorite among liches",
                "who tire of eternity."
            ),
            new BookPageInfo
            (
                "And for those brave",
                "souls who reach the",
                "end of their underworld",
                "feast, the Final",
                "Fortune Cookie awaits,",
                "its paper whispering",
                "secrets of what's to",
                "come, in this life or"
            ),
            new BookPageInfo
            (
                "the next. Each",
                "fortune is a prophecy,",
                "a riddle, a truth",
                "unfathomable, penned",
                "by fate herself.",
                "",
                "Thus concludes our",
                "guide to the underworld's"
            ),
            new BookPageInfo
            (
                "most exquisite banquets.",
                "May your journeys be",
                "safe, your courage",
                "staunch, and your",
                "appetite insatiable.",
                "Till our paths cross",
                "at a table beyond,",
                "bon appétit."
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
                "Chef Bordon the Bold",
                DateTime.Now.ToString("t"),
                DateTime.Now.ToString("d"),
                "May your meals be",
                "as adventurous as",
                "your travels."
            )

        );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public FineDiningInTheUnderworld() : base(false)
        {
            // Set the hue to a random color between 1 and 3000
            Hue = Utility.RandomMinMax(1, 3000);
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("Fine Dining in the Underworld");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "Fine Dining in the Underworld");
        }

        public FineDiningInTheUnderworld(Serial serial) : base(serial)
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
