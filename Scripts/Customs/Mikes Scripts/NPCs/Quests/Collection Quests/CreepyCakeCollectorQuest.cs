using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class CreepyCakeCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Creepy Cake Collector"; } }

        public override object Description
        {
            get
            {
                return "Greetings, brave adventurer! I am Grimelda, the Mistress of Gloom. My lair is in need of a peculiar ingredient: 50 Creepy Cakes. " +
                       "These eerie confections are vital for the dark rituals that maintain the balance of shadows and light. Your assistance in gathering " +
                       "them will be rewarded handsomely. In return, you will receive gold, a rare Maxxia Scroll, and a magnificent ensemble that reflects the " +
                       "mystical essence of the shadows.";
            }
        }

        public override object Refuse { get { return "Very well. Should you change your mind, return to me with the Creepy Cakes."; } }

        public override object Uncomplete { get { return "I still require 50 Creepy Cakes. Gather them and bring them to me so I may complete my dark rituals!"; } }

        public override object Complete { get { return "Marvelous! You have gathered the 50 Creepy Cakes I required. Your aid has ensured the balance of shadows. " +
                       "Please accept these rewards as a token of my gratitude. May the shadows guide you!"; } }

        public CreepyCakeCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(CreepyCake), "Creepy Cakes", 50, 0x9e9)); // Assuming Creepy Cake item ID is 0x1F6
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(PinUpHalterDress), 1, "Grimelda's Shadow Ensemble")); // Assuming Shadow Ensemble is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Creepy Cake Collector quest!");
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

    public class Grimelda : MondainQuester
    {
        [Constructable]
        public Grimelda()
            : base("The Mistress of Gloom", "Grimelda")
        {
        }

        public Grimelda(Serial serial)
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
            AddItem(new Robe { Hue = Utility.Random(1, 3000), Name = "Grimelda's Gloomy Robe" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new FeatheredHat { Hue = Utility.Random(1, 3000), Name = "Grimelda's Shadow Hat" });
            AddItem(new SkullCap { Hue = Utility.Random(1, 3000), Name = "Grimelda's Enigmatic Skull Cap" });
            AddItem(new Cloak { Hue = Utility.Random(1, 3000), Name = "Grimelda's Mystical Cloak" });
            AddItem(new LeatherGloves { Hue = Utility.Random(1, 3000) });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Grimelda's Shadow Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(CreepyCakeCollectorQuest)
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
