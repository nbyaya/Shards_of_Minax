using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class ParasiticPlantCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Parasitic Plant Quest"; } }

        public override object Description
        {
            get
            {
                return "Greetings, brave adventurer! I am Thorne, the Botanist of the Wilds. I require your assistance to collect 50 Parasitic Plants. " +
                       "These plants are vital for my research into the dark magic that festers in the wilderness. " +
                       "In return for your aid, you will receive gold, a rare Maxxia Scroll, and the esteemed Botanist's Garb, " +
                       "a mystical attire imbued with the essence of the wilds.";
            }
        }

        public override object Refuse { get { return "Very well. If you change your mind, return to me with the Parasitic Plants."; } }

        public override object Uncomplete { get { return "I still require 50 Parasitic Plants. Return to me with them so I can continue my studies."; } }

        public override object Complete { get { return "Marvelous! You have brought me the 50 Parasitic Plants I needed. Your contribution to my research is invaluable. " +
                       "Please accept these rewards as a token of my gratitude. May the wilds favor you on your journey!"; } }

        public ParasiticPlantCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(ParasiticPlant), "Parasitic Plants", 50, 0x3190)); // Assuming Parasitic Plant item ID is 0x1F5
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(FootpadsArms), 1, "Botanist's Arms")); // Assuming Botanist's Garb is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Parasitic Plant Quest!");
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

    public class BotanistThorne : MondainQuester
    {
        [Constructable]
        public BotanistThorne()
            : base("The Botanist", "Thorne")
        {
        }

        public BotanistThorne(Serial serial)
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
            AddItem(new Shirt { Hue = Utility.Random(1, 3000), Name = "Thorne's Wildshirt" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new FeatheredHat { Hue = Utility.Random(1, 3000), Name = "Thorne's Botanist Hat" });
            AddItem(new LeatherGloves { Hue = Utility.Random(1, 3000) });
            AddItem(new Kilt { Hue = Utility.Random(1, 3000) });
            AddItem(new QuarterStaff { Hue = Utility.Random(1, 3000), Name = "Thorne's Enchanted Staff" }); // Assuming BotanicalStaff is a custom item
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Thorne's Field Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(ParasiticPlantCollectorQuest)
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
