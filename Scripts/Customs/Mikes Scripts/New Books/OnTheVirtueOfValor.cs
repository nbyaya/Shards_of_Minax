using System;
using Server;

namespace Server.Items
{
    public class OnTheVirtueOfValor : BlueBook
    {
        public static readonly BookContent Content = new BookContent
            (
                "On the Virtue of Valor", "A Humble Scribe",
                new BookPageInfo
                (
                    "In the many virtues",
                    "that one can pursue,",
                    "Valor stands as a",
                    "pillar of courage,",
                    "bravery, and action.",
                    "",
                    "        -A Humble Scribe"
                ),
                new BookPageInfo
                (
                    "Valor is not just the",
                    "absence of fear, but",
                    "the willingness to",
                    "confront it. It is",
                    "this virtue that",
                    "defines true heroes,",
                    "and the mantra that",
                    "guides it is:"
                ),
                new BookPageInfo
                (
                    "   TRZ KAL RAH",
                    "",
                    "Chanting this mantra",
                    "invokes the inner",
                    "courage needed to",
                    "face challenges, be",
                    "they physical or",
                    "spiritual."
                ),
                new BookPageInfo
                (
                    "Let us not forget,",
                    "Valor is not blind.",
                    "It is guided by",
                    "wisdom and tempered",
                    "with humility. It is",
                    "not a virtue to be",
                    "taken lightly, for",
                    "recklessness is not"
                ),
                new BookPageInfo
                (
                    "valor, and bravery",
                    "without cause is",
                    "foolishness.",
                    "",
                    "So embrace Valor,",
                    "let it guide you, but",
                    "always in concert",
                    "with the other virtues."
                ),
				new BookPageInfo
				(
					"To delve deeper into",
					"Valor, one must",
					"understand its three",
					"key aspects: courage,",
					"sacrifice, and",
					"integrity. Each aspect",
					"carves a unique facet",
					"into the gemstone of"
				),
				new BookPageInfo
				(
					"Valor, which shines",
					"brightest when all are",
					"in harmony.",
					"",
					"1) Courage",
					"",
					"Courage is the most",
					"obvious aspect of"
				),
				new BookPageInfo
				(
					"Valor. It's the",
					"readiness to confront",
					"fear, pain, or",
					"challenges head-on.",
					"Courage manifests not",
					"only in battle, but",
					"also in everyday",
					"actions and choices."
				),
				new BookPageInfo
				(
					"2) Sacrifice",
					"",
					"Sacrifice is the",
					"willingness to put",
					"oneself at risk for",
					"the greater good, or",
					"to protect another.",
					"Sacrifice deepens the",
					"meaning of Valor, as"
				),
				new BookPageInfo
				(
					"it elevates the",
					"virtue from mere",
					"boldness to",
					"selflessness.",
					"",
					"3) Integrity",
					"",
					"Integrity ensures"
				),
				new BookPageInfo
				(
					"that acts of Valor",
					"are guided by ethical",
					"principles. Valor",
					"without integrity",
					"can lead to",
					"destructive or",
					"self-serving actions,",
					"thus negating its"
				),
				new BookPageInfo
				(
					"noble purpose.",
					"",
					"When these three",
					"aspects align, true",
					"Valor is achieved.",
					"It becomes a mighty",
					"force for positive",
					"change, capable of"
				),
				new BookPageInfo
				(
					"overcoming even the",
					"darkest of evils.",
					"",
					"Therefore, strive for",
					"Valor in all you do,",
					"and let the mantra",
					"TRZ KAL RAH illuminate",
					"your path."
				)

            );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public OnTheVirtueOfValor() : base(false)
        {
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("On the Virtue of Valor");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "On the Virtue of Valor");
        }

        public OnTheVirtueOfValor(Serial serial) : base(serial)
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
