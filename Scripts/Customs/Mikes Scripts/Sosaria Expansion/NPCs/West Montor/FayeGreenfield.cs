using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class HaltTheBurningRavagerQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Halt the BurningRavager"; } }

        public override object Description
        {
            get
            {
                return
                    "You approach *Faye Greenfield*, her hands rough from ranch work, her eyes scanning the horizon with a worried gaze.\n\n" +
                    "Dressed in a weathered coat dyed the color of summer fields, she stands firm despite the trembling of her cattle behind her.\n\n" +
                    "“That creature—the *BurningRavager*—it's been shrieking across the ridges, driving my livestock mad with fear. It’s not the first time I’ve faced fire, but I won’t lose another herd.”\n\n" +
                    "“Years ago, a wildfire spirit took everything. My family's stock, our land, our hope. Now, this *thing* wants to finish the job. I can’t let it. But I can’t fight it either.”\n\n" +
                    "**Slay the BurningRavager** before it tramples through my pens and sets the fields ablaze. Do this, and you’ll have my thanks—and something sturdy to help tend your own.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then keep your distance from my ranch. I’ll protect what I can… but I won’t forget those who turned away.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "You haven’t dealt with it yet? Each night I hear its cry, and my cattle stampede in terror. I can't hold them much longer.";
            }
        }

        public override object Complete
        {
            get
            {
                return "It’s gone? Truly?\n\n" +
                       "The fields feel lighter already. The cattle are calm. You've not only saved my herd—you’ve spared me another season of grief.\n\n" +
                       "**Take this FeedingTrough**—crafted by my own hand, it’ll serve you well. A symbol of trust between ranchers and protectors.";
            }
        }

        public HaltTheBurningRavagerQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(BurningRavager), "BurningRavager", 1));
            AddReward(new BaseReward(typeof(FeedingTrough), 1, "FeedingTrough"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Halt the BurningRavager'!");
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

    public class FayeGreenfield : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(HaltTheBurningRavagerQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBRancher());
        }

        [Constructable]
        public FayeGreenfield()
            : base("the Rancher", "Faye Greenfield")
        {
        }

        public FayeGreenfield(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(70, 85, 50);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = 1002; // Sun-tanned skin
            HairItemID = 0x203B; // Long hair
            HairHue = 1153; // Wheat-blonde
        }

        public override void InitOutfit()
        {
            AddItem(new FormalShirt() { Hue = 2101, Name = "Rancher's Linen" }); // Dusty cream
            AddItem(new LeatherLegs() { Hue = 2106, Name = "Field-Worn Chaps" }); // Earth-brown
            AddItem(new HalfApron() { Hue = 2111, Name = "Greenfield Apron" }); // Grass-green
            AddItem(new StrawHat() { Hue = 2105, Name = "Sun-Bleached Hat" }); // Faded straw
            AddItem(new Boots() { Hue = 2109, Name = "Fencewalkers" }); // Dark leather boots

            AddItem(new ShepherdsCrook() { Hue = 2412, Name = "Ashwood Crook" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1107;
            backpack.Name = "Ranch Pack";
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
