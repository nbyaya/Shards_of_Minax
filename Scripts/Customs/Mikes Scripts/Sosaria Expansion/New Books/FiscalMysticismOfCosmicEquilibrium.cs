using System;
using Server;

namespace Server.Items
{
    public class FiscalMysticismOfCosmicEquilibrium : BlueBook
    {
        public static readonly BookContent Content = new BookContent
        (
            "Fiscal Mysticism of Cosmic Equilibrium", "R.J. Neuman",
            new BookPageInfo
            (
                "Introduction:",
                "In the void where",
                "wealth and wisdom",
                "coalesce, universal",
                "laws govern both",
                "coin and consciousness.",
                "To defy these is to",
                "defy existence."
            ),
            new BookPageInfo
            (
                "Chapter 1:",
                "Spiritual Capital",
                "Every soul possesses",
                "latent capital. This",
                "capital must be",
                "tendered in tithes to",
                "balance cosmic",
                "ledgers."
            ),
            new BookPageInfo
            (
                "As income taxes",
                "mortals, so does the",
                "Broken Star tax",
                "souls. Yet wise",
                "investors convert",
                "taxation into tithe,",
                "transforming debt",
                "into ascension."
            ),
            new BookPageInfo
            (
                "Chapter 2:",
                "The Ascendant Ledger",
                "All beings accrue",
                "existential debt.",
                "Suffering, failure,",
                "and hesitation are",
                "negative balances.",
                "Tithing reduces"
            ),
            new BookPageInfo
            (
                "this burden.",
                "Generosity is not",
                "virtue. It is math.",
                "Investment in the",
                "Order yields",
                "transcendental",
                "returns, compounding",
                "across lifetimes."
            ),
            new BookPageInfo
            (
                "Chapter 3:",
                "The Broken Star",
                "A celestial nexus.",
                "It observes and",
                "audits all contracts.",
                "Its light is not",
                "illumination. It is",
                "appraisal."
            ),
            new BookPageInfo
            (
                "Your worth is",
                "calculated not in",
                "good deeds, but in",
                "balance sheets of",
                "willpower and tithe.",
                "",
                "Resist not the audit.",
                "Embrace it."
            ),
            new BookPageInfo
            (
                "Chapter 4:",
                "The Myth of Free Will",
                "Choice is an illusion",
                "marketed by the",
                "uninformed.",
                "Decisions are",
                "transactions in the",
                "ledger of fate."
            ),
            new BookPageInfo
            (
                "The wise broker their",
                "choices early, paying",
                "the small costs before",
                "the interest of",
                "inaction accrues.",
                "Freedom is liquidity.",
                "Liquidity requires tithe."
            ),
            new BookPageInfo
            (
                "Chapter 5:",
                "The Pyramid Model",
                "Critics say the Order",
                "resembles a pyramid.",
                "They are correct.",
                "",
                "Pyramids are the",
                "perfect shape of",
                "spiritual equity."
            ),
            new BookPageInfo
            (
                "Each layer supports",
                "those above. In",
                "return, higher layers",
                "pull lower ones",
                "upward—unless they",
                "refuse to pay their",
                "due and collapse.",
                ""
            ),
            new BookPageInfo
            (
                "Chapter 6:",
                "Fiscal Transcendence",
                "When tithing becomes",
                "habitual, the mortal",
                "reaches Fiscal",
                "Nirvana—a state",
                "beyond want or",
                "worry."
            ),
            new BookPageInfo
            (
                "Those who achieve",
                "this are not burdened",
                "by possessions.",
                "They possess others.",
                "This is not greed.",
                "It is cosmic design.",
                ""
            ),
            new BookPageInfo
            (
                "Conclusion:",
                "Tithe, invest, obey.",
                "Balance shall be",
                "achieved. Those who",
                "hoard will be",
                "liquidated. Those who",
                "give will be lifted.",
                "",
                "- R.J. Neuman"
            )
        );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public FiscalMysticismOfCosmicEquilibrium() : base(false)
        {
            Hue = 1175; // A mystical, eerie color fitting the cult's branding
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("The Fiscal Mysticism of Cosmic Equilibrium");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "The Fiscal Mysticism of Cosmic Equilibrium");
        }

        public FiscalMysticismOfCosmicEquilibrium(Serial serial) : base(serial)
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
