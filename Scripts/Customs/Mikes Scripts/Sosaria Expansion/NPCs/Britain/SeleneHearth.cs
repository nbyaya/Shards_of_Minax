using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class CarnageCuisineQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Carnage Cuisine"; } }

        public override object Description
        {
            get
            {
                return
                    "You find *Selene Hearth*, head chef of Castle British, meticulously scrubbing down a row of spice jars—each labeled with flowing script and stained with a strange, violet ichor.\n\n" +
                    "She doesn’t look up as she speaks.\n\n" +
                    "“You’ve not known ruin until you’ve tasted it in thyme.”\n\n" +
                    "“Last night—midnight, as I prepped for a royal luncheon—I heard the vault’s cold vents moan. Then came the smell... spoiled marrow and ozone. *It* came through. Slithered in through the meat lockers. Splattered my saffron stash. Called itself the *Meatformer.*”\n\n" +
                    "“I’ve sealed the spice racks, but if it reaches the main pantry, Castle British’s larders will be tainted for a generation.”\n\n" +
                    "**Slay the Meatformer** before it corrupts the Preservation Vault’s stores forever.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then may your appetite never cross my kitchen door again.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still breathing, is it? Then I keep scrubbing. My rosemary reeks of rot.";
            }
        }

        public override object Complete
        {
            get
            {
                return "Bless the flame—you’ve carved that thing like a roast.\n\n" +
                       "The spice racks breathe easy again. Take this: *GaleplumeCrest.* I was saving it for a royal pie, but you've earned a taste of wind-kissed legend.";
            }
        }

        public CarnageCuisineQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(Meatformer), "Meatformer", 1));
            AddReward(new BaseReward(typeof(GaleplumeCrest), 1, "GaleplumeCrest"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Carnage Cuisine'!");
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

    public class SeleneHearth : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(CarnageCuisineQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBCook());
        }

        [Constructable]
        public SeleneHearth()
            : base("the Castle Chef", "Selene Hearth")
        {
        }

        public SeleneHearth(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 80, 30);
            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = 1102; // Smoky black
        }

        public override void InitOutfit()
        {
            AddItem(new FullApron() { Hue = 1153, Name = "Ichor-Stained Apron" }); // Violet ichor
            AddItem(new FancyShirt() { Hue = 2424, Name = "Starched Chef's Shirt" }); // Pure white
            AddItem(new LongPants() { Hue = 1150, Name = "Basil-Tone Slacks" }); // Cool green
            AddItem(new Sandals() { Hue = 1175, Name = "Smoky Kitchen Soles" }); // Sooty grey
            AddItem(new ChefsToque() { Hue = 2301, Name = "Selene’s Spice-Crowned Toque" }); // Deep saffron

            AddItem(new GourmandsFork() { Name = "Spice Piercer" });

            Backpack backpack = new Backpack();
            backpack.Hue = 0x48E;
            backpack.Name = "Sampler Kit";
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
