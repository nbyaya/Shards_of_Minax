using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class WhispersFromTheCryptQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Whispers from the Crypt"; } }

        public override object Description
        {
            get
            {
                return
                    "Father Aldwyn, gravekeeper of Grey, clutches his lantern tightly, his eyes sunken with sleepless nights.\n\n" +
                    "“They speak to me, traveler… the dead. I walk among their graves each night, and still they rise not from the soil, yet their voices... they chill the marrow.”\n\n" +
                    "“A presence—no, a *plague*—has crept from the crypts near Exodus. These GraveWhispers poison the silence. They urge the dead to rise, and soon they might listen.”\n\n" +
                    "“I beg you—find these phantoms in the depths of Exodus Dungeon and silence them. Only then might the dead rest… and I with them.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then I am left to face the night alone, with only whispers for company. May the graves stay closed a while longer.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "You haven’t faced them yet? The voices grow bolder… last night they spoke my name.";
            }
        }

        public override object Complete
        {
            get
            {
                return "It is done? The crypt is silent? Then take this—it's called the *GermanUnificationChest*. A relic of another time, but it may serve you better than me.\n\n" +
                       "I might sleep tonight, thanks to you.";
            }
        }

        public WhispersFromTheCryptQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(GraveWhispers), "GraveWhispers", 3));
            AddReward(new BaseReward(typeof(GermanUnificationChest), 1, "GermanUnificationChest"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Whispers from the Crypt'!");
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

    public class FatherAldwyn : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(WhispersFromTheCryptQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBMonk()); // Gravekeeper's meditative, solemn role.
        }

        [Constructable]
        public FatherAldwyn()
            : base("the Gravekeeper", "Father Aldwyn")
        {
        }

        public FatherAldwyn(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 60, 50);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = 1001; // Pale, ghostly hue.
            HairItemID = 0x203B; // Long Hair
            HairHue = 1150; // Midnight Black
            FacialHairItemID = 0x2041; // Full Beard
            FacialHairHue = 1150; // Midnight Black
        }

        public override void InitOutfit()
        {
            AddItem(new MonkRobe() { Hue = 2406, Name = "Gravekeeper's Shroud" }); // Dark Grey
            AddItem(new Sandals() { Hue = 2309, Name = "Silent Steps" });
            AddItem(new Cloak() { Hue = 1109, Name = "Shadowveil Cloak" }); // Dusty Black
            AddItem(new SkullCap() { Hue = 1102, Name = "Mortuary Cap" }); // Faded Charcoal

            AddItem(new CampingLanturn() { Hue = 1161, Name = "Lantern of Restless Nights" }); // Pale Blue Glow

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Gravekeeper's Satchel";
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
