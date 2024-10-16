using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class SandCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Sands of Time"; } }

        public override object Description
        {
            get
            {
                return "Ah, greetings brave one! I am Ilyana, the Keeper of the Sands. My research into the ancient deserts has led me to a " +
                       "discovery that requires a vast quantity of sand. I need 500 Sand to complete a ritual that could unlock secrets of the past. " +
                       "In return for your invaluable help, I shall reward you with gold, a rare Maxxia Scroll, and a unique sash that " +
                       "embody the very essence of the desert itself.";
            }
        }

        public override object Refuse { get { return "Very well. Should you decide to assist me, return with the sand, and you shall be rewarded."; } }

        public override object Uncomplete { get { return "I still require 500 Sand. Please bring it to me so that I may proceed with my research."; } }

        public override object Complete { get { return "Splendid! You have gathered the 500 Sand I needed. Your help is invaluable in my quest for ancient knowledge. " +
                       "As a token of my gratitude, please accept these rewards. May the sands of time guide your path!"; } }

        public SandCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(Sand), "Sand", 500, 0x11EA)); // Assuming Sand item ID is 0x0E4
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(ClericsSacredSash), 1, "Desert Sash")); // Assuming Desert Robes is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Sands of Time quest!");
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

    public class DesertKeeperIlyana : MondainQuester
    {
        [Constructable]
        public DesertKeeperIlyana()
            : base("The Keeper of the Sands", "Ilyana")
        {
        }

        public DesertKeeperIlyana(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x190; // Female Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x2045; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new Robe { Hue = Utility.Random(1, 3000), Name = "Ilyana's Desert Robes" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new WideBrimHat { Hue = Utility.Random(1, 3000), Name = "Ilyana's Desert Hat" });
            AddItem(new LeatherGloves { Hue = Utility.Random(1, 3000), Name = "Ilyana's Quill of Wisdom" });
            AddItem(new GoldBracelet { Hue = Utility.Random(1, 3000), Name = "Ilyana's Desert Bracelet" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Ilyana's Travel Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(SandCollectorQuest)
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
