using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class WasabiCCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Wasabi's Wisdom"; } }

        public override object Description
        {
            get
            {
                return "Greetings, brave traveler! I am Satori, the Sage of Spices. I seek your aid in a task most peculiar but crucial. " +
                       "I require 50 Wasabi Clumps to complete my ancient recipe, said to bestow extraordinary insights. " +
                       "In return for your effort, you shall be rewarded with gold, a rare Maxxia Scroll, and a mystic garment of great power.";
            }
        }

        public override object Refuse { get { return "Very well. If you change your mind, return with the Wasabi Clumps."; } }

        public override object Uncomplete { get { return "I still need 50 Wasabi Clumps. Gather them for me to aid in my mystical endeavor!"; } }

        public override object Complete { get { return "Marvelous! You have gathered the 50 Wasabi Clumps I needed. Your help is invaluable. " +
                       "Accept these rewards as a token of my gratitude, and may you always find the spice of adventure in your travels!"; } }

        public WasabiCCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(WasabiClumps), "Wasabi Clumps", 50, 0x24EB)); // Assuming Wasabi Clump item ID is 0x12F2
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(PsychedelicTieDyeShirt), 1, "Satori's Mystic Shirt")); // Assuming Satori's Mystic Robe is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Wasabi's Wisdom quest!");
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

    public class SageSatori : MondainQuester
    {
        [Constructable]
        public SageSatori()
            : base("The Sage of Spices", "Satori")
        {
        }

        public SageSatori(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x190; // Female Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x203C; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new Robe { Hue = Utility.Random(1, 3000), Name = "Satori's Mystic Robe" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new WizardsHat { Hue = Utility.Random(1, 3000), Name = "Satori's Enchanted Hat" });
            AddItem(new GoldRing { Hue = Utility.Random(1, 3000), Name = "Satori's Wisdom Ring" });
            AddItem(new LeatherGloves { Hue = Utility.Random(1, 3000), Name = "Satori's Mystic Gloves" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Satori's Magical Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(WasabiCCollectorQuest)
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
