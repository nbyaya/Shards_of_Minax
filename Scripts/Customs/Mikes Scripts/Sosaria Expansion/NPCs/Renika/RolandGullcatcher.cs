using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class SongOfTheShatteredCarvingQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Song of the Shattered Carving"; } }

        public override object Description
        {
            get
            {
                return
                    "You meet *Roland Gullcatcher*, a fisherman whose hands bear the marks of both sea toil and delicate brushstrokes.\n\n" +
                    "He stands by a battered easel, half-finished seascape shimmering with hues of dawn.\n\n" +
                    "“The nets, friend... they’re no good anymore. This blasted bird—*CarvedBird*, they call it—has been cawing like doom itself, shredding my nets before they touch water.”\n\n" +
                    "“I’ve tried everything. Old rope, new rope, even silver-threaded lines... nothing holds. I need something stronger, something old. If I had a rune-etched feather from that cursed bird, I could weave it into my nets, bind them with its own spell.”\n\n" +
                    "“It haunts the Mountain Stronghold, nesting in the high arches. Bring me that feather, and I’ll finally cast without fear—and maybe paint without nightmares.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then may the winds spare your sails, and the bird keep its scream. I’ll mend with what I have.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still no feather? The bird's cry worsens, tearing not just nets—but peace.";
            }
        }

        public override object Complete
        {
            get
            {
                return "You’ve done it... the feather, it hums with strange power.\n\n" +
                       "Now I can weave nets that hold, and maybe, just maybe, capture a peaceful night's sleep.\n\n" +
                       "Here, take this: *AnniversaryPainting*. One of my best works—painted with hope, not fear.";
            }
        }

        public SongOfTheShatteredCarvingQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(CarvedBird), "CarvedBird", 1));
            AddReward(new BaseReward(typeof(AnniversaryPainting), 1, "AnniversaryPainting"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Song of the Shattered Carving'!");
            Owner.PlaySound(CompleteSound);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class RolandGullcatcher : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(SongOfTheShatteredCarvingQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBFisherman()); // Closest vendor type
        }

        [Constructable]
        public RolandGullcatcher()
            : base("the Gullcatcher", "Roland Gullcatcher")
        {
        }

        public RolandGullcatcher(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 60, 40);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = 1052; // Sun-kissed skin tone
            HairItemID = 0x203B; // Long hair
            HairHue = 1153; // Ocean blue tint
            FacialHairItemID = 0x2041; // Medium beard
            FacialHairHue = 1153; // Matching ocean blue
        }

        public override void InitOutfit()
        {
            AddItem(new FancyShirt() { Hue = 1324, Name = "Wave-Worn Shirt" }); // Deep sea green
            AddItem(new LongPants() { Hue = 1801, Name = "Salt-Stained Trousers" }); // Sandy beige
            AddItem(new BodySash() { Hue = 2101, Name = "Gullfeather Sash" }); // Light feather gray
            AddItem(new Sandals() { Hue = 1175, Name = "Beachcomber’s Sandals" }); // Sea foam blue
            AddItem(new StrawHat() { Hue = 2106, Name = "Tidewatcher’s Hat" }); // Driftwood brown

            Backpack backpack = new Backpack();
            backpack.Hue = 2101;
            backpack.Name = "Fishing Satchel";
            AddItem(backpack);

            AddItem(new FishingPole() { Hue = 2401, Name = "Rune-Bound Rod" });
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
