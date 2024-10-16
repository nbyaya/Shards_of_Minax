using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;

namespace Server.Engines.Quests
{
    public class PowderedIronCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Powdered Iron Plight"; } }

        public override object Description
        {
            get
            {
                return "Greetings, brave adventurer! I am Voltran, the Master Artisan. " +
                       "I have an urgent request for you. The land is in dire need of enchanted artifacts, and to forge them, " +
                       "I require 50 units of Powdered Iron. This rare substance is crucial for the magical bonds in our creations. " +
                       "In return for your valuable help, I will reward you with gold, a rare Maxxia Scroll, and a splendidly unique Master Artisan's Hat.";
            }
        }

        public override object Refuse { get { return "Very well. Should you change your mind, return to me with the Powdered Iron."; } }

        public override object Uncomplete { get { return "I still need 50 Powdered Iron. Please bring them to me so I can continue my work!"; } }

        public override object Complete { get { return "Fantastic! You have collected the 50 Powdered Iron I needed. Your dedication is truly commendable. " +
                       "As a token of my gratitude, please accept these rewards. May your adventures be filled with fortune and wonder!"; } }

        public PowderedIronCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(PowderedIron), "Powdered Iron", 50, 0x573D)); // Assuming Powdered Iron item ID is 0x1F2
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(RangersHat), 1, "Master Artisan's Hat")); // Assuming Master Artisan's Tunic is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Powdered Iron Plight quest!");
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

    public class MasterArtisanVoltran : MondainQuester
    {
        [Constructable]
        public MasterArtisanVoltran()
            : base("The Master Artisan", "Voltran")
        {
        }

        public MasterArtisanVoltran(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x190; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x203F; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new Robe { Hue = Utility.Random(1, 3000), Name = "Voltran's Master Artisan Robe" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new WizardsHat { Hue = Utility.Random(1, 3000), Name = "Voltran's Enchanted Hat" });
            AddItem(new LeatherGloves { Hue = Utility.Random(1, 3000), Name = "Voltran's Artisan Gloves" });
            AddItem(new Kilt { Hue = Utility.Random(1, 3000) });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Voltran's Crafting Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(PowderedIronCollectorQuest)
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
