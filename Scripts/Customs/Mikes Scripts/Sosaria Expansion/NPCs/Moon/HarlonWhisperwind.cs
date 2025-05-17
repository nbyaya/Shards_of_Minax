using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class ScarabaicHymnQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Scarabaic Hymn"; } }

        public override object Description
        {
            get
            {
                return
                    "You find *Harlon Whisperwind*, the bard of Moon, perched atop a low wall, strumming a lute with frayed, barely intact strings. His voice, soft and weary, rises above the hum of the scarabs skittering below.\n\n" +
                    "“They come with the dusk, drawn to my songs… chewing the strings, burrowing into the wood. Each night, their queen sends them, weaving bridges of beetle-flesh to scale the walls.”\n\n" +
                    "“Music is life here, traveler. And life is fading. I cannot play with their mandibles gnawing at the heart of my craft.”\n\n" +
                    "“The Queen of the Scarabs nests beneath the sands, but her brood swarms our southern walls. You must end her hymn of decay.”\n\n" +
                    "**Slay the Queen of the Scarabs**, and let Moon hear song again under the stars.";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then the silence will spread, note by note, until nothing remains but the drone of wings in the dark.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "She still sings, doesn't she? I can hear her chittered rhythm beneath the soil.";
            }
        }

        public override object Complete
        {
            get
            {
                return "The walls are still, the strings are safe... and the stars can once again dance to music, not madness.\n\n" +
                       "Take this, friend: *FlamewhiskerHat*. A gift forged in firelight and melody—a token of the night you silenced the Queen.";
            }
        }

        public ScarabaicHymnQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(QueenOfTheScarabs), "Queen of the Scarabs", 1));
            AddReward(new BaseReward(typeof(FlamewhiskerHat), 1, "FlamewhiskerHat"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Scarabaic Hymn'!");
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

    public class HarlonWhisperwind : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(ScarabaicHymnQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBBowyer()); 
        }

        [Constructable]
        public HarlonWhisperwind()
            : base("the Bard of Moon", "Harlon Whisperwind")
        {
        }

        public HarlonWhisperwind(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 60, 70);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = 33770; // Pale desert-touched hue
            HairItemID = Race.RandomHair(this);
            HairHue = 1153; // Silvery white
            FacialHairItemID = 0;
        }

        public override void InitOutfit()
        {
            AddItem(new FancyShirt() { Hue = 2115, Name = "Whispering Tunic" }); // Moonlit blue
            AddItem(new LongPants() { Hue = 2208, Name = "Twilight Weave Trousers" }); // Soft twilight purple
            AddItem(new Cloak() { Hue = 2419, Name = "Starlight Cloak" }); // Ethereal white
            AddItem(new Boots() { Hue = 1109, Name = "Duststep Boots" }); // Ash-gray
            AddItem(new FeatheredHat() { Hue = 2422, Name = "Minstrel’s Plume" }); // Deep scarlet with a black feather

            AddItem(new ResonantHarp() { Hue = 2117, Name = "Stringless Lute" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Songkeeper’s Pack";
            AddItem(backpack);
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
