using System;
using Server;

namespace Server.Items
{
    public class OrcishTinkeringBook : BlueBook
    {
        public static readonly BookContent Content = new BookContent
            (
                "Orcish Tinkering", "Goruk",
                new BookPageInfo
                (
                    "This manuscript aims",
                    "to outline the finer",
                    "points of Orcish",
                    "tinkering, an often",
                    "underestimated skill.",
                    "",
                    "            -Goruk"
                ),
                new BookPageInfo
                (
                    "Chapter 1: Materials",
                    "Orcs prefer crude yet",
                    "sturdy materials like",
                    "iron, bone, and leather.",
                    "Quality matters less",
                    "than durability in",
                    "Orcish craftsmanship."
                ),
                new BookPageInfo
                (
                    "Chapter 2: Tools",
                    "Orcs use a variety of",
                    "tools ranging from",
                    "primitive hammers to",
                    "makeshift wrenches.",
                    "Innovation is key."
                ),
                // ... more pages can be added
				new BookPageInfo
				(
					"Chapter 3: Techniques",
					"Orcs may not have the",
					"finest methods, but",
					"they make use of brute",
					"force and ingenuity.",
					"Tinkering is a mix of",
					"trial and error."
				),
				new BookPageInfo
				(
					"Chapter 4: Traps",
					"Orcish traps are",
					"notorious for being",
					"deadly and crude.",
					"Spikes, tripwires, and",
					"explosives are common",
					"elements."
				),
				new BookPageInfo
				(
					"Chapter 5: Weapons",
					"Orcish tinkering often",
					"results in highly",
					"efficient weapons.",
					"The Orcish crossbow is",
					"a prime example, being",
					"both rugged and deadly."
				),
				new BookPageInfo
				(
					"Chapter 6: Armor",
					"Orcish armor tends to",
					"be heavy and limiting,",
					"but it offers excellent",
					"protection. It's often",
					"adorned with spikes",
					"and fearsome motifs."
				),
				new BookPageInfo
				(
					"Chapter 7: Trinkets",
					"Beyond weaponry, Orcs",
					"also tinker with",
					"various trinkets that",
					"can aid them in battle.",
					"These include crude",
					"healing potions and"
				),
				new BookPageInfo
				(
					"magical talismans that",
					"are reinforced with",
					"shamanic spells. Don't",
					"underestimate the power",
					"of an Orcish trinket."
				),
				new BookPageInfo
				(
					"Chapter 8: Conclusion",
					"Orcish tinkering may",
					"not be elegant, but it",
					"is effective. With the",
					"right materials and",
					"a strong will, Orcs can",
					"create formidable items."
				),
				new BookPageInfo
				(
					"Goruk, 4am.",
					"18.3.1487",
					"The end of my notes."
				)

            );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public OrcishTinkeringBook() : base(false)
        {
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("Orcish Tinkering");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "Orcish Tinkering");
        }

        public OrcishTinkeringBook(Serial serial) : base(serial)
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
