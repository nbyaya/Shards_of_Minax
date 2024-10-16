using System;
using Server;

namespace Server.Items
{
    public class ElvenCarpentry : BlueBook
    {
        public static readonly BookContent Content = new BookContent
            (
                "Elven Carpentry", "Elmior Woodhand",
                new BookPageInfo
                (
                    "This book aims to",
                    "share the secrets",
                    "and techniques of",
                    "Elven carpentry",
                    "honed through the",
                    "ages.",
                    "",
                    "        -Elmior"
                ),
                new BookPageInfo
                (
                    "Elves have long",
                    "been recognized",
                    "for their superior",
                    "woodworking skills.",
                    "Our fine bows,",
                    "arrows, and other",
                    "wooden artifacts",
                    "stand as proof."
                ),
                new BookPageInfo
                (
                    "Essential tools",
                    "include elven",
                    "wood blades and",
                    "the moon-enchanted",
                    "mallets. These",
                    "tools, combined",
                    "with the proper",
                    "techniques, create"
                ),
                new BookPageInfo
                (
                    "masterpieces.",
                    "",
                    "Remember, the wood",
                    "is not just a",
                    "material; it's a",
                    "living entity that",
                    "breathes and grows.",
                    "Treat it with the",
                    "respect it deserves."
                ),
                new BookPageInfo
                (
                    "In conclusion, the",
                    "art of Elven",
                    "carpentry is a",
                    "marvelous and",
                    "rewarding skill.",
                    "I hope this guide",
                    "serves you well in",
                    "your future endeavors."
                ),
				new BookPageInfo
				(
					"Wood Selection",
					"Choosing the right",
					"wood is crucial.",
					"Elm and oak are",
					"generally strong,",
					"while willow and",
					"pine are more",
					"flexible."
				),
				new BookPageInfo
				(
					"Wood Treatment",
					"Before carving,",
					"treat the wood with",
					"moonwater elixir.",
					"This not only",
					"softens the wood,",
					"but also imbues it",
					"with arcane energy."
				),
				new BookPageInfo
				(
					"Safety Measures",
					"Always remember to",
					"wear protective",
					"garments. Even elven",
					"craftsmen are not",
					"immune to the",
					"occasional slip of",
					"the chisel."
				),
				new BookPageInfo
				(
					"Advanced Techniques",
					"Master craftsmen",
					"often employ glyphs",
					"and runes to",
					"enhance their work.",
					"These can add",
					"various magical",
					"effects."
				),
				new BookPageInfo
				(
					"Bow Crafting",
					"The crafting of",
					"elven bows is an",
					"art form. The pull,",
					"the flexibility, and",
					"the aerodynamics",
					"are all key aspects",
					"to consider."
				),
				new BookPageInfo
				(
					"Heritage and Lore",
					"The elven affinity",
					"for carpentry goes",
					"back millennia. It",
					"is said that Corellon",
					"Larethian himself",
					"was a master of the",
					"craft."
				),
				new BookPageInfo
				(
					"Inspirations",
					"Take inspiration",
					"from nature and the",
					"world around you.",
					"The more harmonious",
					"your work is with",
					"nature, the better",
					"the outcome."
				),
				new BookPageInfo
				(
					"Final Remarks",
					"With the right",
					"skills and dedication,",
					"you too can become",
					"a master in elven",
					"carpentry. May your",
					"hands always be steady",
					"and your blade sharp."
				)

            );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public ElvenCarpentry() : base(false)
        {
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("Elven Carpentry");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "Elven Carpentry");
        }

        public ElvenCarpentry(Serial serial) : base(serial)
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
