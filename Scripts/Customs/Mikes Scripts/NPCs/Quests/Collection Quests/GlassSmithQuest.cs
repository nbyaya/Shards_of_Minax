using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;

namespace Server.Engines.Quests
{
    public class GlassSmithQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Glass Smith's Request"; } }

        public override object Description
        {
            get
            {
                return "Greetings, brave adventurer! I am Glaros, the Glass Smith. My workshop is in dire need of 50 WorkableGlass, " +
                       "which are crucial for the enchanting glasswork I am crafting. The glass will help me complete a magnificent " +
                       "project that will bring both beauty and utility to our realm. In return for your invaluable assistance, " +
                       "I will reward you with gold, a rare Maxxia Scroll, and a specially crafted Glass Smith's Pants.";
            }
        }

        public override object Refuse { get { return "Very well. If you change your mind, return to me with the WorkableGlass."; } }

        public override object Uncomplete { get { return "I still require 50 WorkableGlass. Please bring them to me so I can finish my work!"; } }

        public override object Complete { get { return "Fantastic! You've gathered the 50 WorkableGlass I needed. Your help has ensured the success of my project. " +
                       "Please accept these rewards as a token of my gratitude. May your path be illuminated with fortune!"; } }

        public GlassSmithQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(WorkableGlass), "WorkableGlass", 50, 0x1F5)); // Assuming WorkableGlass item ID is 0x1F5
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(StreetArtistsBaggyPants), 1, "Glass Smith's Pants")); // Assuming Glass Smith's Cloak is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Glass Smith's Request quest!");
            Owner.PlaySound(CompleteSound);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class GlassSmithGlaros : MondainQuester
    {
        [Constructable]
        public GlassSmithGlaros()
            : base("The Glass Smith", "Glaros")
        {
        }

        public GlassSmithGlaros(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x190; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x203C; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new Robe { Hue = Utility.Random(1, 3000), Name = "Glaros's Glass Robe" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new WideBrimHat { Hue = Utility.Random(1, 3000), Name = "Glaros's Crafting Hat" });
            AddItem(new LeatherGloves { Hue = Utility.Random(1, 3000), Name = "Glaros's Crafting Gloves" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Glaros's Glass Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(GlassSmithQuest)
                };
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
