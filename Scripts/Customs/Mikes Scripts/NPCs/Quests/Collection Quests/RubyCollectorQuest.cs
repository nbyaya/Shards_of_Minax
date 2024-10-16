using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class RubyCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Ruby Conundrum"; } }

        public override object Description
        {
            get
            {
                return "Greetings, brave adventurer! I am Aric, the Mystic of the Eternal Flame. In the ancient lore of my order, " +
                       "there is a prophecy about a powerful artifact that can only be forged with the blood of dragons and the " +
                       "spark of 50 Rubies. I seek your aid to gather these precious gems. In return for your valiant efforts, " +
                       "I will reward you with gold, a rare Maxxia Scroll, and the illustrious Hat of the Mystic Flame, imbued " +
                       "with magic from the very essence of the Eternal Flame.";
            }
        }

        public override object Refuse { get { return "Very well. Should you reconsider, return to me with the Rubies."; } }

        public override object Uncomplete { get { return "I still require 50 Rubies. Please bring them to me to complete the quest!"; } }

        public override object Complete { get { return "Wonderful! You have brought me the 50 Rubies I needed. Your assistance is greatly appreciated. " +
                       "Please accept these rewards as a token of my gratitude. May your path be ever lit by the Eternal Flame!"; } }

        public RubyCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(Ruby), "Rubies", 50, 0xF13)); // Assuming Ruby item ID is 0x1F5
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(RangersHatNightSight), 1, "Hat of the Mystic Flame")); // Assuming Mystic Flame Robe is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Ruby Conundrum quest!");
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

    public class MysticAric : MondainQuester
    {
        [Constructable]
        public MysticAric()
            : base("The Mystic of the Eternal Flame", "Aric")
        {
        }

        public MysticAric(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x191; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x2043; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new Robe { Hue = Utility.Random(1, 3000), Name = "Aric's Mystic Robe" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new WizardsHat { Hue = Utility.Random(1, 3000), Name = "Aric's Flame Hat" });
            AddItem(new LeatherGloves { Hue = Utility.Random(1, 3000) });
            AddItem(new Cloak { Hue = Utility.Random(1, 3000), Name = "Aric's Flame Bracelet" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Aric's Enchanted Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(RubyCollectorQuest)
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
