using System;
using Server;

namespace Server.Items
{
    public class AnOrcishCookbook : BlueBook
    {
        public static readonly BookContent Content = new BookContent
            (
                "An Orcish Cookbook", "Grog the Chef",
                new BookPageInfo
                (
                    "Welcome to Grog's",
                    "cookbook of delicious",
                    "orcish recipes!",
                    "Inside, you'll find",
                    "dishes that will make",
                    "your tusks water!",
                    "",
                    "          -Grog"
                ),
                new BookPageInfo
                (
                    "Roasted Boar:",
                    "1) Hunt a boar.",
                    "2) Skin and gut it.",
                    "3) Roast on open fire.",
                    "4) Season with herbs.",
                    "5) Feast!",
                    "",
                    ""
                ),
                new BookPageInfo
                (
                    "Goblin Stew:",
                    "1) Capture a goblin.",
                    "2) Do NOT skin it.",
                    "3) Boil in a large pot.",
                    "4) Add rocks for flavor.",
                    "5) Serve hot!",
                    "",
                    ""
                ),
                // Add more pages as needed
				new BookPageInfo
				(
					"Bat Wing Soup:",
					"1) Collect bat wings.",
					"2) Boil in water.",
					"3) Add eye of newt.",
					"4) Stir until thick.",
					"5) Serve in skull bowl.",
					"",
					""
				),
				new BookPageInfo
				(
					"Slime Jelly:",
					"1) Gather green slime.",
					"2) Heat until liquid.",
					"3) Add troll blood.",
					"4) Stir and let cool.",
					"5) Eat with spoon.",
					"",
					""
				),
				new BookPageInfo
				(
					"Orcish Ale:",
					"1) Brew hops and malt.",
					"2) Add a dash of blood.",
					"3) Ferment in barrel.",
					"4) Strain through teeth.",
					"5) Guzzle down!",
					"",
					""
				),
				new BookPageInfo
				(
					"Minotaur Steak:",
					"1) Slay a minotaur.",
					"2) Cut tender steak.",
					"3) Cook over hot coals.",
					"4) Season with fear.",
					"5) Best served rare!",
					"",
					""
				),
				new BookPageInfo
				(
					"Rat Tail Noodles:",
					"1) Skin several rats.",
					"2) Cut tails into strips.",
					"3) Boil until tender.",
					"4) Add to any soup.",
					"5) Delicious and chewy!",
					"",
					""
				),
				new BookPageInfo
				(
					"Beetle Crunch:",
					"1) Gather live beetles.",
					"2) Dry in the sun.",
					"3) Crush into paste.",
					"4) Shape and fry.",
					"5) A crunchy snack!",
					"",
					""
				),
				new BookPageInfo
				(
					"Wyrm Tongue Stew:",
					"1) Harvest dragon tongue.",
					"2) Slice and sautee.",
					"3) Add to boiling pot.",
					"4) Season with gems.",
					"5) A royal treat!",
					"",
					""
				)

            );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public AnOrcishCookbook() : base(false)
        {
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("An Orcish Cookbook");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "An Orcish Cookbook");
        }

        public AnOrcishCookbook(Serial serial) : base(serial)
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
