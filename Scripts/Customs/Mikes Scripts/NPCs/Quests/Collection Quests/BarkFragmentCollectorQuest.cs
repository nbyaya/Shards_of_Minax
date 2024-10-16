using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class BarkFragmentCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Bark Fragment Collector"; } }

        public override object Description
        {
            get
            {
                return "Greetings, brave adventurer! I am Grathor the Wild, keeper of the ancient woods. " +
                       "The forest whispers of a powerful ancient tree whose bark holds the essence of the forest's magic. " +
                       "I need you to gather 50 Bark Fragments from the depths of the wild. These fragments are vital to restoring the balance of nature. " +
                       "In return for your noble efforts, I will reward you with gold, a rare Maxxia Scroll, and a unique Forest Guardian's Garb.";
            }
        }

        public override object Refuse { get { return "Very well. If you change your mind, return with the Bark Fragments."; } }

        public override object Uncomplete { get { return "I still need 50 Bark Fragments. Return to me once you have gathered them to aid in my quest."; } }

        public override object Complete { get { return "Splendid work! You have brought me the 50 Bark Fragments I sought. The forest’s balance will be restored thanks to you. " +
                       "Please accept these rewards as a token of my gratitude. May the forest’s blessings follow you in your travels!"; } }

        public BarkFragmentCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(BarkFragment), "Bark Fragments", 50, 0x318F)); // Assuming Bark Fragment item ID is 0x1F0
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(PopStarsGlitteringCap
), 1, "Forest Guardian's Cap")); // Assuming Forest Guardian's Garb is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Bark Fragment Collector quest!");
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

    public class GrathorTheWild : MondainQuester
    {
        [Constructable]
        public GrathorTheWild()
            : base("The Forest Keeper", "Grathor the Wild")
        {
        }

        public GrathorTheWild(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x190; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x2046; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new Robe { Hue = Utility.Random(1, 3000), Name = "Grathor's Forest Robe" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new FeatheredHat { Hue = Utility.Random(1, 3000), Name = "Grathor's Woodland Hat" });
            AddItem(new LeatherGloves { Hue = Utility.Random(1, 3000) });
            AddItem(new Kilt { Hue = Utility.Random(1, 3000) });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Grathor's Forest Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(BarkFragmentCollectorQuest)
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
