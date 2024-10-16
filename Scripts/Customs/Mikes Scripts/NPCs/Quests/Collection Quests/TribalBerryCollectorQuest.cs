using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class TribalBerryCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Tribal Berry Hunt"; } }

        public override object Description
        {
            get
            {
                return "Greetings, brave adventurer! I am Xantor, the Shaman of the Sacred Tribe. Our people have long revered the Tribal Berry for its mystical properties. " +
                       "But recently, the berries have become scarce, and I need your help to gather 50 of them. In exchange for your assistance, " +
                       "I will reward you with gold, a rare Maxxia Scroll, and a ceremonial garb adorned with the colors of our ancestors.";
            }
        }

        public override object Refuse { get { return "Very well. If you change your mind, return to me with the Tribal Berries."; } }

        public override object Uncomplete { get { return "I still need 50 Tribal Berries. Please bring them to me so that we can restore balance to our tribe!"; } }

        public override object Complete { get { return "You've done it! You've brought me the 50 Tribal Berries I needed. Your dedication is a beacon of hope for our people. " +
                       "Please accept these rewards as a symbol of our gratitude. May your path be blessed with fortune and protection!"; } }

        public TribalBerryCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(TribalBerry), "Tribal Berries", 50, 0x9D0)); // Assuming Tribal Berry item ID is 0x1F0
            AddReward(new BaseReward(typeof(Gold), 6000, "6000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(HarmonysLeggings), 1, "Tribal Ceremonial Garb")); // Assuming Tribal Ceremonial Garb is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Tribal Berry Hunt quest!");
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

    public class ShamanXantor : MondainQuester
    {
        [Constructable]
        public ShamanXantor()
            : base("The Shaman", "Xantor")
        {
        }

        public ShamanXantor(Serial serial)
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
            AddItem(new Shirt { Hue = Utility.Random(1, 3000), Name = "Xantor's Sacred Robe" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new TribalMask { Hue = Utility.Random(1, 3000), Name = "Xantor's Tribal Headdress" });
            AddItem(new LeatherGloves { Hue = Utility.Random(1, 3000) });
            AddItem(new Kilt { Hue = Utility.Random(1, 3000) });
            AddItem(new Necklace { Hue = Utility.Random(1, 3000), Name = "Xantor's Spirit Necklace" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Xantor's Mystical Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(TribalBerryCollectorQuest)
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
