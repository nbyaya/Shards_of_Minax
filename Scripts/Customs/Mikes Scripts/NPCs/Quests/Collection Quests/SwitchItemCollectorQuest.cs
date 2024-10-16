using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class SwitchItemCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Switch Item Hoarder"; } }

        public override object Description
        {
            get
            {
                return "Greetings, daring adventurer! I am Glaren the Eccentric, and I have a rather peculiar task for you. " +
                       "I need you to collect 50 Switchs for me. They are scattered far and wide, and gathering them is no easy feat. " +
                       "However, your efforts will not go unrewarded. In exchange for your assistance, you shall receive gold, a rare Maxxia Scroll, " +
                       "and an outfit that will surely make you the talk of the realm. Do you accept this challenge?"; 
            }
        }

        public override object Refuse { get { return "Very well. If you change your mind, return to me with the SwitchItems."; } }

        public override object Uncomplete { get { return "I still require 50 SwitchItems. Bring them to me if you wish to earn your reward!"; } }

        public override object Complete { get { return "Fantastic! You have brought me the 50 SwitchItems I requested. Your determination is commendable. " +
                       "Accept these rewards as a token of my gratitude, and may your adventures be ever thrilling!"; } }

        public SwitchItemCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(SwitchItem), "SwitchItems", 50, 0x2F5F)); // Assuming SwitchItem item ID is 0x1E0
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(TechnomancersHoodie), 1, "Glaren's Eccentric Outfit")); // Assuming Glaren's Eccentric Outfit is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Switch Item Hoarder quest!");
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

    public class GlarenTheEccentric : MondainQuester
    {
        [Constructable]
        public GlarenTheEccentric()
            : base("The Switch Item Hoarder", "Glaren")
        {
        }

        public GlarenTheEccentric(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x190; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x2041; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new FancyShirt { Hue = Utility.Random(1, 3000), Name = "Glaren's Colorful Vest" });
            AddItem(new ShortPants { Hue = Utility.Random(1, 3000) });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new JesterHat { Hue = Utility.Random(1, 3000), Name = "Glaren's Whimsical Hat" });
            AddItem(new LeatherGloves { Hue = Utility.Random(1, 3000), Name = "Glaren's Gloved Hands" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Glaren's Quirky Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(SwitchItemCollectorQuest)
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
