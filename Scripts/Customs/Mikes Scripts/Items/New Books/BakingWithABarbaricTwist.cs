using System;
using Server;

namespace Server.Items
{
    public class BakingWithABarbaricTwist : BlueBook
    {
        public static readonly BookContent Content = new BookContent
        (
            "Baking with a Barbaric Twist", "Gronk the Baker",
            new BookPageInfo
            (
                "Baking, an art as",
                "ancient as time, has",
                "its roots in the",
                "warmth of hearth",
                "and home. But here,",
                "in the hands of a",
                "barbarian, it becomes",
                "a wild culinary ride."
            ),
            new BookPageInfo
            (
                "I am Gronk, the once",
                "mighty warrior who",
                "swapped sword for",
                "spatula, battle cry",
                "for baking. This tome",
                "is my legacy, blending",
                "barbaric robustness",
                "with delicate pastries."
            ),
            new BookPageInfo
            (
                "Commence with the",
                "Mammoth Meat Pie:",
                "a feast for a clan,",
                "or a challenge for",
                "one. The crust, thick",
                "and sturdy, encases",
                "the rich filling of",
                "beast and root."
            ),
            new BookPageInfo
            (
                "Then venture to the",
                "Berzerker Berry Tart:",
                "its tartness will jolt",
                "you awake better than",
                "any war horn's call.",
                "Only the bravest dare",
                "pair it with the fiery",
                "Frost Dragon Cream."
            ),
            new BookPageInfo
            (
                "For a true test of",
                "mettle, the Volcano",
                "Cake awaits—layers",
                "of spice cake with a",
                "molten chocolate",
                "core. It erupts with",
                "each slice, a tribute",
                "to battles past."
            ),
            new BookPageInfo
            (
                "And let us not forget",
                "the shield-sized",
                "Cookies of Conquest,",
                "a hefty treat to",
                "uphold the spirit of",
                "the warrior within.",
                "Each bite a crunch",
                "like the clashing of"
            ),
            new BookPageInfo
            (
                "swords. But beware,",
                "for they are as",
                "addictive as the",
                "siren's song, leading",
                "many a strong will",
                "to sweet surrender.",
                "",
                "Whether you dine in"
            ),
            new BookPageInfo
            (
                "a mead hall or a",
                "modest kitchen, let",
                "these recipes imbue",
                "your meals with a",
                "savage grace. Embrace",
                "the barbaric twist,",
                "for even the wildest",
                "heart can find solace"
            ),
            new BookPageInfo
            (
                "in the ritual of",
                "baking. May your",
                "ovens be as fiery as",
                "a dragon's breath",
                "and your feasts as",
                "legendary as the",
                "tales of old."
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
                "Gronk the Baker",
                DateTime.Now.ToString("t"),
                DateTime.Now.ToString("d"),
                "May your crusts be",
                "crisp and your bread",
                "rise high!"
            ),
			// Continuation from the previous code snippet
			new BookPageInfo
			(
				"in the quietude of",
				"the kitchen. Let the",
				"rolling pin be your",
				"staff, and the mixing",
				"bowl your cauldron",
				"of might."
			),
			new BookPageInfo
			(
				"Fear not the complexity",
				"of the Enchanted",
				"Elven Bread, with",
				"its myriad of grains",
				"and seeds. It’s a",
				"bread so light, it",
				"might lift the",
				"soul itself."
			),
			new BookPageInfo
			(
				"Dare to pair your",
				"feasts with the",
				"Goblin Grog Icing,",
				"a concoction so potent,",
				"it could wake the",
				"dead—or at least",
				"liven any festivity."
			),
			new BookPageInfo
			(
				"For those of hearty",
				"appetite, the Giant’s",
				"Feast Loaf, a behemoth",
				"of meats and cheese",
				"encased in bread, will",
				"test the might of",
				"your oven and your",
				"stomach."
			),
			new BookPageInfo
			(
				"In the quiet winter",
				"months, the Solstice",
				"Stew Bun can warm",
				"even the coldest",
				"heart, a bread bowl",
				"worthy of the longest",
				"night's feast."
			),
			new BookPageInfo
			(
				"Summon your courage",
				"for the Siren's Sweet",
				"Bun, a twisted delight",
				"of cinnamon and",
				"sugar, that ensnares",
				"all who dare its",
				"swirling depths."
			),
			new BookPageInfo
			(
				"Let not the kitchen’s",
				"heat dismay; the",
				"Forge-Baked Flatbread",
				"requires a flame as",
				"fierce as the forges",
				"of the dwarven halls."
			),
			new BookPageInfo
			(
				"No feast is complete",
				"without the Lament",
				"of the Banshee",
				"Pie, a dessert so",
				"sorrowfully sweet,",
				"it could bring tears",
				"to a troll's eye."
			),
			new BookPageInfo
			(
				"Finally, the pièce de",
				"résistance, the Dragon’s",
				"Breath Chili Bread:",
				"a fiery challenge that",
				"bakes in the heat of",
				"a dragon’s ire and the",
				"tenderness of a maiden’s",
				"love."
			),
			new BookPageInfo
			(
				"Baking with a",
				"barbaric twist is not",
				"just about food. It is",
				"a journey, a quest of",
				"flavors, a battle of",
				"textures. Each recipe",
				"is a saga to be told."
			),
			new BookPageInfo
			(
				"So wield your whisk",
				"with honor, and may",
				"your kneading be as",
				"steady as your heart",
				"in battle. For in the",
				"end, the greatest",
				"victory is the triumph",
				"of taste."
			),
			new BookPageInfo
			(
				"May your meals be",
				"mighty, your desserts",
				"dauntless, and your",
				"snacks without",
				"surrender. Onward,",
				"baker-warriors, to",
				"glory in the kitchen!"
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
				"Gronk the Baker",
				DateTime.Now.ToString("t"),
				DateTime.Now.ToString("d"),
				"Eat well, laugh often,",
				"bake bravely."
			)
			// Continuation of the existing code

        );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public BakingWithABarbaricTwist() : base(false)
        {
            // Set the hue to a random color between 1 and 3000
            Hue = Utility.RandomMinMax(1, 3000);
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("Baking with a Barbaric Twist");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "Baking with a Barbaric Twist");
        }

        public BakingWithABarbaricTwist(Serial serial) : base(serial)
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
