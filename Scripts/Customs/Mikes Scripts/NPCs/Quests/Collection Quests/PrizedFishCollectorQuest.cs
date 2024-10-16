using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class PrizedFishCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Prized Fish Collection"; } }

        public override object Description
        {
            get
            {
                return "Ah, greetings, noble adventurer! I am Captain Meloria, once a revered sea captain now dedicated to the study of rare aquatic specimens. " +
                       "I seek your aid in collecting 50 Prized Fish, magnificent creatures that were once the pride of the ocean’s bounty. " +
                       "In return for your effort, I shall bestow upon you gold, a rare Maxxia Scroll, and a unique Captain's Garb adorned with treasures of the deep.";
            }
        }

        public override object Refuse { get { return "Very well. Should you change your mind, return to me with the Prized Fish."; } }

        public override object Uncomplete { get { return "I am still in need of 50 Prized Fish. Please bring them to me as soon as possible!"; } }

        public override object Complete { get { return "Splendid work! You have managed to gather all 50 Prized Fish. Your efforts have not gone unnoticed. " +
                       "Please accept these rewards as a token of my gratitude. May your adventures be as vast and rewarding as the ocean!"; } }

        public PrizedFishCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(PrizedFish), "Prized Fish", 50, 0xDD6)); // Assuming Prized Fish item ID is 0x1F0
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(FishmongersKilt), 1, "Captain's Kilt")); // Assuming Captain's Garb is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Prized Fish Collection quest!");
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

    public class CaptainMeloria : MondainQuester
    {
        [Constructable]
        public CaptainMeloria()
            : base("Captain Meloria", "The Sea's Guardian")
        {
        }

        public CaptainMeloria(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x190; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x204A; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new FancyShirt { Hue = Utility.Random(1, 3000), Name = "Captain's Silk Shirt" });
            AddItem(new LongPants { Hue = Utility.Random(1, 3000) });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new TricorneHat { Hue = Utility.Random(1, 3000), Name = "Captain's Tricorne Hat" });
            AddItem(new GoldBracelet { Hue = Utility.Random(1, 3000), Name = "Captain's Gold Bracelet" });
            AddItem(new GoldRing { Hue = Utility.Random(1, 3000), Name = "Captain's Golden Ring" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Captain's Treasure Chest";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(PrizedFishCollectorQuest)
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
