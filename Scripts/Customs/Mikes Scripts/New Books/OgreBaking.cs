using System;
using Server;

namespace Server.Items
{
    public class OgreBaking : BlueBook
    {
        public static readonly BookContent Content = new BookContent
            (
                "Ogre Baking", "Chef Grumnak",
                new BookPageInfo
                (
                    "Welcome to the",
                    "world of ogre",
                    "baking! This book",
                    "is a collection of",
                    "ancient ogre",
                    "recipes passed",
                    "down through",
                    "generations."
                ),
                new BookPageInfo
                (
                    "Recipe 1: Mud Pie",
                    "",
                    "Ingredients:",
                    "- Mud",
                    "- Rock",
                    "- Stick",
                    "",
                    "Bake under the sun."
                ),
                new BookPageInfo
                (
                    "Recipe 2: Stone",
                    "Cake",
                    "",
                    "Ingredients:",
                    "- Large Stone",
                    "- Moss",
                    "",
                    "Smash together."
                ),
                new BookPageInfo
                (
                    "Ogre baking has",
                    "always been a",
                    "simple, yet hearty",
                    "affair. Do not",
                    "expect fancy",
                    "flavors but do",
                    "expect full",
                    "stomachs."
                ),
                new BookPageInfo
                (
                    "Remember, the",
                    "key to good ogre",
                    "baking is to have",
                    "fun and to always",
                    "eat your creations",
                    "with a loud",
                    "belch. It's a sign",
                    "of appreciation."
                ),
				new BookPageInfo
				(
					"Recipe 3: Bug",
					"Stew",
					"",
					"Ingredients:",
					"- 10 Bugs",
					"- Water",
					"- Mud",
					"",
					"Boil and enjoy!"
				),
				new BookPageInfo
				(
					"Recipe 4: Fish",
					"Slap",
					"",
					"Ingredients:",
					"- 1 Fish",
					"- 1 Large Leaf",
					"- Stick",
					"",
					"Wrap and slap!"
				),
				new BookPageInfo
				(
					"Recipe 5: Grass",
					"Bread",
					"",
					"Ingredients:",
					"- Handful Grass",
					"- Mud",
					"",
					"Knead and bake!"
				),
				new BookPageInfo
				(
					"Cooking Tips:",
					"- Always yell while",
					"  cooking, it adds",
					"  flavor.",
					"- Never measure",
					"  ingredients.",
					"- Smash food for",
					"  better texture."
				),
				new BookPageInfo
				(
					"Common Mistakes:",
					"- Cooking too long,",
					"  ogres like it raw.",
					"- Using metal",
					"  tools, sticks are",
					"  best.",
					"- Not enough mud,",
					"  more mud is always",
					"  better."
				),
				new BookPageInfo
				(
					"Substitutes:",
					"- No bugs? Use",
					"  rocks.",
					"- No fish? Use",
					"  slime.",
					"- No grass? Use",
					"  leaves."
				)

            );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public OgreBaking() : base(false)
        {
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("Ogre Baking");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "Ogre Baking");
        }

        public OgreBaking(Serial serial) : base(serial)
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
