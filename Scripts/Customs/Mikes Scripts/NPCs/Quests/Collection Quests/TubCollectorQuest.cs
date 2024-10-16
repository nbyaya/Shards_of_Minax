using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class TubCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Arcane Tub Collector"; } }

        public override object Description
        {
            get
            {
                return "Greetings, brave wanderer! I am Garendel, the Arcane Keeper. My research requires the collection of 50 Empty Wooden Tubs. " +
                       "These ancient vessels are vital for my arcane experiments and magical concoctions. Your efforts will be richly rewarded with gold, " +
                       "a rare Maxxia Scroll, and a specially enchanted Arcane Keeper's Attire. Will you assist me in this critical task?";
            }
        }

        public override object Refuse { get { return "Very well. If you change your mind, return to me with the Empty Wooden Tubs."; } }

        public override object Uncomplete { get { return "I still require 50 Empty Wooden Tubs. Please bring them to me so I can continue my research!"; } }

        public override object Complete { get { return "Fantastic! You have brought me the 50 Empty Wooden Tubs I needed. Your assistance has proven invaluable. " +
                       "Accept these rewards as a token of my gratitude. May your future endeavors be as successful as this quest!"; } }

        public TubCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(EmptyWoodenTub), "Empty Wooden Tubs", 50, 0x1605)); // Assuming Empty Wooden Tub item ID is 0x1F4
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(WhisperingSondals), 1, "Arcane Keeper's Attire")); // Assuming Arcane Keeper's Attire is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Arcane Tub Collector quest!");
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

    public class ArcaneKeeperGarendel : MondainQuester
    {
        [Constructable]
        public ArcaneKeeperGarendel()
            : base("The Arcane Keeper", "Garendel")
        {
        }

        public ArcaneKeeperGarendel(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x190; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x203D; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new Robe { Hue = Utility.Random(1, 3000), Name = "Garendel's Arcane Robe" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new WizardsHat { Hue = Utility.Random(1, 3000), Name = "Garendel's Mystical Hat" });
            AddItem(new LeatherGloves { Hue = Utility.Random(1, 3000) });
            AddItem(new GoldBracelet { Hue = Utility.Random(1, 3000), Name = "Garendel's Enchanted Bracelet" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Garendel's Arcane Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(TubCollectorQuest)
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
