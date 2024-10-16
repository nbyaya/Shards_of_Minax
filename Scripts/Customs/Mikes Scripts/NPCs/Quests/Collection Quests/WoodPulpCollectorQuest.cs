using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class WoodPulpCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The WoodPulp Collector"; } }

        public override object Description
        {
            get
            {
                return "Greetings, brave wanderer! I am Thorne, Keeper of the Verdant Grove. I require your assistance in gathering " +
                       "50 pieces of WoodPulp. These will be used in my sacred rituals to rejuvenate the forest and maintain its mystical energy. " +
                       "In return for your efforts, I will reward you with gold, a rare Maxxia Scroll, and a uniquely enchanted Forest Guardian's Leggings.";
            }
        }

        public override object Refuse { get { return "Very well. Should you change your mind, return to me with the WoodPulp."; } }

        public override object Uncomplete { get { return "I still require 50 pieces of WoodPulp. Please bring them to me so I can continue my rituals!"; } }

        public override object Complete { get { return "Splendid work! You have collected the 50 pieces of WoodPulp I needed. Your help is invaluable to the forest's wellbeing. " +
                       "Please accept these rewards as a token of my gratitude. May the forest watch over you!"; } }

        public WoodPulpCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(WoodPulp), "WoodPulp", 50, 0x103D)); // Assuming WoodPulp item ID is 0x1F5
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(BeggarsBondLeggings), 1, "Forest Guardian's Leggings")); // Assuming Forest Guardian's Cloak is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the WoodPulp Collector quest!");
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

    public class ForestGuardianThorne : MondainQuester
    {
        [Constructable]
        public ForestGuardianThorne()
            : base("The Keeper of the Verdant Grove", "Thorne")
        {
        }

        public ForestGuardianThorne(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x191; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x2040; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new Robe { Hue = Utility.Random(1, 3000), Name = "Thorne's Forest Robe" });
            AddItem(new Boots { Hue = Utility.Random(1, 3000) });
            AddItem(new WideBrimHat { Hue = Utility.Random(1, 3000), Name = "Thorne's Woodland Hat" });
            AddItem(new PlateGloves { Hue = Utility.Random(1, 3000), Name = "Thorne's Forest Gloves" });
            AddItem(new Bow { Hue = Utility.Random(1, 3000), Name = "Thorne's Enchanted Bow" }); // Assuming ForestBow is a custom item
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Thorne's Nature Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(WoodPulpCollectorQuest)
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
