using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class FinalActQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Final Act"; } }

        public override object Description
        {
            get
            {
                return
                    "Elric Playfrock, draped in flamboyant silks and adorned with gilded stage makeup, greets you with a flourish and a bow.\n\n" +
                    "“A *villain* walks the halls of Vault 44, donning faces not its own!”\n\n" +
                    "“This wretched *Cosplayer* mimics the vault’s staff, sows chaos in their labs, and disrupts the delicate harmony of their craft. But I—yes, I!—noticed the flaws in their performance. The way their *shadow lagged*, the twitch in their eye when the bell struck noon. It is not a perfect act, and I refuse to let it stand!”\n\n" +
                    "“Go, brave soul, to the Preservation Vault and bring the curtain down on this charlatan. Slay the Cosplayer and retrieve its costume scraps as proof. Only then shall the *Final Act* begin anew, with truth restored to the stage!”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "A pity! The audience will remain confused, their hearts tugged by falsehood. But remember—*the show must go on*.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still it mimics? I hear whispers of confusion in the labs, and I feel the taint of bad acting clinging to the air!";
            }
        }

        public override object Complete
        {
            get
            {
                return "*Bravo!* The fiend is no more, its threads torn asunder! You’ve played your part to perfection.\n\n" +
                       "Take this: the *Crown of the Jungle King*. A gift befitting a hero who knows that every show must end, and that truth, in the end, is the finest performance of all.";
            }
        }

        public FinalActQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(Cosplayer), "Cosplayer", 1));
            AddReward(new BaseReward(typeof(CrownOfTheJungleKing), 1, "Crown of the Jungle King"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Final Act'!");
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

    public class ElricPlayfrock : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(FinalActQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBBard()); // Closest vendor type to a Court Entertainer
        }

        [Constructable]
        public ElricPlayfrock()
            : base("the Court Entertainer", "Elric Playfrock")
        {
        }

        public ElricPlayfrock(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(80, 85, 40);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = 1002; // Pale theatrical skin tone
            HairItemID = 0x203C; // Long Hair
            HairHue = 1153; // Deep Purple
            FacialHairItemID = 0x2041; // Goatee
            FacialHairHue = 1153;
        }

        public override void InitOutfit()
        {
            AddItem(new FancyShirt() { Hue = 2118, Name = "Velvet Performer’s Shirt" }); // Royal Purple
            AddItem(new LongPants() { Hue = 2105, Name = "Crimson Troupe Breeches" });
            AddItem(new Cloak() { Hue = 1157, Name = "Dramatis Cloak" }); // Midnight Blue
            AddItem(new JesterShoes() { Hue = 1175, Name = "Gilded Footfalls" }); // Gold-Trimmed
            AddItem(new FeatheredHat() { Hue = 2117, Name = "Plume of the Stage" }); // Deep Red Hat with Feather

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Playfrock's Prop Satchel";
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
