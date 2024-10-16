using System;
using Server;

namespace Server.Items
{
    public class OrcishCarpentryBook : BlueBook
    {
        public static readonly BookContent Content = new BookContent
            (
                "Orcish Carpentry", "Gruknar",
                new BookPageInfo
                (
                    "Dis book teach yu",
                    "da way of Orcish",
                    "carpentry.",
                    "Smash, nail, an",
                    "bind wood togetha.",
                    "",
                    "",
                    "           -Gruknar"
                ),
                new BookPageInfo
                (
                    "Step 1: Findin'",
                    "Wood",
                    "Go to forest, chop",
                    "down biggest tree",
                    "yu find. Big wood",
                    "good wood.",
                    "",
                    ""
                ),
                new BookPageInfo
                (
                    "Step 2: Cuttin'",
                    "Wood",
                    "Use axe or big",
                    "sword. Cut into",
                    "pieces. No need for",
                    "exact, jus' cut.",
                    "",
                    ""
                ),
                // ... More pages here
				new BookPageInfo
				(
					"Step 3: Nails an'",
					"Bindings",
					"Use rusty nails an'",
					"rope. Orcish way no",
					"need fancy tools.",
					"Hit nail hard wit'",
					"hammer till it stick.",
					""
				),
				new BookPageInfo
				(
					"Step 4: Smashin'",
					"Togatha",
					"Put pieces close,",
					"den smash togetha",
					"wit' big hammer.",
					"If it no fit, smash",
					"harda.",
					""
				),
				new BookPageInfo
				(
					"Step 5: Mor'",
					"Smashin'",
					"If it still no fit,",
					"smash again! Orc",
					"way is repeat till",
					"it work. No give up.",
					"",
					""
				),
				new BookPageInfo
				(
					"Step 6: Testin'",
					"Strength",
					"Stand on it, jump",
					"on it, hit it! If it",
					"break, go back an'",
					"smash again.",
					"",
					""
				),
				new BookPageInfo
				(
					"Step 7: Paintin'",
					"an' Decor",
					"Paint wit' color of",
					"tribe or use enemy",
					"blood for best look.",
					"Add teef or bones",
					"for style.",
					""
				),
				new BookPageInfo
				(
					"Common Mistakes",
					"",
					"1. No smash hard",
					"enuf.",
					"2. Use weak wood.",
					"3. No check if it",
					"strong enuf.",
					"4. Forget to add",
					"tribe mark."
				),
				new BookPageInfo
				(
					"Final Note",
					"",
					"Orcish carpentry no",
					"jus' bout buildin'.",
					"It 'bout honor,",
					"strength, an' tribe.",
					"Build proud, build",
					"Orc strong!",
					"          -Gruknar"
				),
                new BookPageInfo
                (
                    "Final Words",
                    "",
                    "Remember, Orc",
                    "carpentry strong!",
                    "It no look pretty,",
                    "but it survive many",
                    "battles!",
                    "          -Gruknar"
                )
            );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public OrcishCarpentryBook() : base(false)
        {
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("Orcish Carpentry Book");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "Orcish Carpentry Book");
        }

        public OrcishCarpentryBook(Serial serial) : base(serial)
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
