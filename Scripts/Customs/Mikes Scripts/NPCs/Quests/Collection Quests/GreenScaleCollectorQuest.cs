using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class GreenScaleCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Scale of Legends"; } }

        public override object Description
        {
            get
            {
                return "Greetings, brave soul! I am Drakar, the Sage of the Scaled Legacy. Long ago, the great dragons roamed " +
                       "these lands, their scales holding the essence of their ancient power. I seek 50 Green Scales from these noble " +
                       "creatures to restore a legendary artifact. In return for your valor, you shall be rewarded with gold, a rare Maxxia Scroll, " +
                       "and the unique Boots of the Scaled Sage, adorned with the colors of the dragon's might.";
            }
        }

        public override object Refuse { get { return "Very well. Should you decide to help, return to me with the Green Scales."; } }

        public override object Uncomplete { get { return "I still need 50 Green Scales. Please bring them to me so I can complete the restoration!"; } }

        public override object Complete { get { return "Marvelous work! You have brought me the 50 Green Scales. Your bravery will be remembered. " +
                       "Please accept these rewards as a mark of my gratitude. May the dragons' legacy guide your path!"; } }

        public GreenScaleCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(GreenScales), "Green Scales", 50, 0x26B4)); // Assuming Green Scale item ID is 0xF7D
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(WhisperersBoots), 1, "Boots of the Scaled Sage")); // Assuming Scaled Sage's Robe is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Scale of Legends quest!");
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

    public class ScaledSageDrakar : MondainQuester
    {
        [Constructable]
        public ScaledSageDrakar()
            : base("The Sage of the Scaled Legacy", "Drakar")
        {
        }

        public ScaledSageDrakar(Serial serial)
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
            AddItem(new Robe { Hue = Utility.Random(1, 3000), Name = "Robes of the Scaled Sage" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new FeatheredHat { Hue = Utility.Random(1, 3000), Name = "Drakar's Scaled Hat" });
            AddItem(new PlateGloves { Hue = Utility.Random(1, 3000), Name = "Drakar's Dragon-Scaled Gloves" });
            AddItem(new GoldBracelet { Hue = Utility.Random(1, 3000), Name = "Drakar's Enchanted Bracelet" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Drakar's Mystical Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(GreenScaleCollectorQuest)
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
