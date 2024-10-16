using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class AgapiteOreCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Agapite Quest"; } }

        public override object Description
        {
            get
            {
                return "Greetings, valiant hero! I am Thorwin, Keeper of the Agapite Shrine. For centuries, the Agapite Ore has " +
                       "been used to forge the finest armor and weapons known to man. Alas, our supplies are dwindling, and " +
                       "I am in dire need of 50 Agapite Ore to continue our great work. In exchange for your courage and effort, " +
                       "I shall reward you with gold, a rare Maxxia Scroll, and a unique set of Agapite Armor that will make you " +
                       "stand out in any battle. Will you aid us in this critical task?";
            }
        }

        public override object Refuse { get { return "Very well, if you change your mind, return to me with the Agapite Ore."; } }

        public override object Uncomplete { get { return "I still require 50 Agapite Ore. Please bring them to me so that we may continue our work."; } }

        public override object Complete { get { return "Excellent work! You have brought me the 50 Agapite Ore we needed. Your bravery and assistance are truly appreciated. " +
                       "Please accept these rewards as a token of our gratitude. May your adventures be prosperous and your battles victorious!"; } }

        public AgapiteOreCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(AgapiteOre), "Agapite Ore", 50, 0x19B9)); // Assuming Agapite Ore item ID is 0x1F4
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(GrandmastersArmguards), 1, "Agapite Armguards")); // Assuming Agapite Armor is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Agapite Ore Collector quest!");
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

    public class Thorwin : MondainQuester
    {
        [Constructable]
        public Thorwin()
            : base("The Keeper of the Agapite Shrine", "Thorwin")
        {
        }

        public Thorwin(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x190; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x2040; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new PlateChest { Hue = Utility.Random(1, 3000), Name = "Thorwin's Agapite Plate" });
            AddItem(new PlateLegs { Hue = Utility.Random(1, 3000) });
            AddItem(new PlateHelm { Hue = Utility.Random(1, 3000), Name = "Thorwin's Helm of Agapite" });
            AddItem(new PlateGloves { Hue = Utility.Random(1, 3000) });
            AddItem(new PlateArms { Hue = Utility.Random(1, 3000) });
            AddItem(new Cloak { Hue = Utility.Random(1, 3000), Name = "Thorwin's Mystical Cloak" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Thorwin's Adventurer's Pack";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(AgapiteOreCollectorQuest)
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
