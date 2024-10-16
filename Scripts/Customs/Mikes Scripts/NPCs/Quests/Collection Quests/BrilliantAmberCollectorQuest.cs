using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class BrilliantAmberCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Amber Enthusiast's Quest"; } }

        public override object Description
        {
            get
            {
                return "Hail, noble traveler! I am Galdric, the Amber Enthusiast. My research into ancient magic has revealed that 50 pieces " +
                       "of Brilliant Amber are needed to complete a powerful enchantment. These gems hold the essence of the sun's light and " +
                       "are vital to my work. As a token of my gratitude for your assistance, I shall reward you with gold, a rare Maxxia Scroll, " +
                       "and a uniquely enchanted Amber Enthusiast's Attire.";
            }
        }

        public override object Refuse { get { return "I understand. If you change your mind, return to me with the Brilliant Amber."; } }

        public override object Uncomplete { get { return "I still require 50 Brilliant Amber. Bring them to me to aid in my magical research!"; } }

        public override object Complete { get { return "Fantastic! You have collected the 50 Brilliant Amber I sought. Your help is invaluable. " +
                       "Please accept these rewards as a gesture of my appreciation. May your journey be filled with light and success!"; } }

        public BrilliantAmberCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(BrilliantAmber), "Brilliant Amber", 50, 0x3199)); // Assuming Brilliant Amber item ID is 0xF6D
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(BeastmiastersTunic), 1, "Amber Enthusiast's Attire")); // Assuming Amber Enthusiast's Attire is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Brilliant Amber Collector quest!");
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

    public class AmberEnthusiastGaldric : MondainQuester
    {
        [Constructable]
        public AmberEnthusiastGaldric()
            : base("The Amber Enthusiast", "Galdric")
        {
        }

        public AmberEnthusiastGaldric(Serial serial)
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
            AddItem(new Robe { Hue = Utility.Random(1, 3000), Name = "Galdric's Amber Robe" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new Cap { Hue = Utility.Random(1, 3000), Name = "Galdric's Sunlit Hat" });
            AddItem(new LeatherGloves { Hue = Utility.Random(1, 3000), Name = "Galdric's Amber Gloves" });
            AddItem(new Tunic { Hue = Utility.Random(1, 3000) });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Galdric's Enchanted Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(BrilliantAmberCollectorQuest)
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
