using System;
using Server;

namespace Server.Items
{
    public class SpeculationsOnTheOriginOfMinax : BlueBook
    {
        public static readonly BookContent Content = new BookContent
            (
                "Speculations on the Origin of Minax", "Saradin",
                new BookPageInfo
                (
                    "This book attempts to",
                    "uncover the enigmatic",
                    "origins of the dark",
                    "sorceress Minax.",
                    "It is only through",
                    "speculation and",
                    "fragments of history",
                    "that we can begin"
                ),
                new BookPageInfo
                (
                    "to understand her",
                    "malevolent powers.",
                    "It is widely known",
                    "that she was a student",
                    "of the evil wizard",
                    "Mondain, yet what",
                    "drove her to a life",
                    "of darkness?"
                ),
                new BookPageInfo
                (
                    "Some say she was",
                    "born under the",
                    "Twin Moons, and",
                    "her soul is forever",
                    "bound to their dark",
                    "cycle. Others claim",
                    "she found an artifact",
                    "that corrupted her."
                ),
                new BookPageInfo
                (
                    "We may never know",
                    "the true origin of",
                    "Minax, but be wary,",
                    "adventurer. The tale",
                    "of Minax serves as a",
                    "warning to us all",
                    "of the corrupting",
                    "nature of power."
                ),
				new BookPageInfo
				(
					"The rumors of her",
					"origin vary from",
					"region to region,",
					"cult to cult. Some",
					"believe she was the",
					"lost child of an",
					"ancient, forgotten",
					"deity."
				),
				new BookPageInfo
				(
					"Others whisper she",
					"came from a parallel",
					"realm, where dark",
					"arts and malevolent",
					"beings reign",
					"supreme. Yet, these",
					"are but whispers,",
					"unverified claims."
				),
				new BookPageInfo
				(
					"Historians have tried",
					"to trace her roots",
					"through scattered",
					"texts and artifacts.",
					"Some suggest that she",
					"might be related to",
					"a once-powerful, but",
					"now fallen, kingdom."
				),
				new BookPageInfo
				(
					"There are even accounts",
					"that mention Minax as",
					"being part of a secret",
					"society, dedicated to",
					"unearthly experiments",
					"and the accumulation",
					"of dark knowledge."
				),
				new BookPageInfo
				(
					"The only factual",
					"information we have",
					"is that her magical",
					"abilities far exceed",
					"those of her peers.",
					"Her capacity to bend",
					"the realms of time",
					"and space is"
				),
				new BookPageInfo
				(
					"unprecedented. She",
					"has been seen in",
					"various timelines,",
					"meddling with the",
					"events to further her",
					"own mysterious goals.",
					"What could be the",
					"source of such power?"
				),
				new BookPageInfo
				(
					"Until the truth",
					"is revealed, it's",
					"critical to consider",
					"these speculations",
					"as possible leads",
					"for future research.",
					"But let us not forget",
					"the dark influence"
				),
				new BookPageInfo
				(
					"she has left in her",
					"wake, the cities",
					"ruined, and lives",
					"shattered. One",
					"thing is certain,",
					"Minax remains a",
					"threat we cannot",
					"yet fully comprehend."
				)

            );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public SpeculationsOnTheOriginOfMinax() : base(false)
        {
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("Speculations on the Origin of Minax");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "Speculations on the Origin of Minax");
        }

        public SpeculationsOnTheOriginOfMinax(Serial serial) : base(serial)
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
