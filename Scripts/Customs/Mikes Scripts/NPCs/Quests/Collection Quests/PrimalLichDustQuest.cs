using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class PrimalLichDustQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Primal Lich's Secret"; } }

        public override object Description
        {
            get
            {
                return "Greetings, brave soul! I am Zephyrus, the Eccentric Alchemist. I am in desperate need of 50 PrimalLichDust. " +
                       "These ancient remnants hold the key to unlocking the secrets of a long-lost alchemical formula. " +
                       "Help me, and I will reward you with gold, a rare Maxxia Scroll, and a unique Alchemist's Ensemble that will make you the envy of all adventurers.";
            }
        }

        public override object Refuse { get { return "Very well. If you change your mind, return to me with the PrimalLichDust."; } }

        public override object Uncomplete { get { return "I still require 50 PrimalLichDust. Please bring them to me so I can continue my research!"; } }

        public override object Complete { get { return "Fantastic! You have collected the 50 PrimalLichDust I needed. Your contribution is invaluable. " +
                       "Please accept these rewards as a token of my gratitude. May your alchemical endeavors be as prosperous as our quest!"; } }

        public PrimalLichDustQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(PrimalLichDust), "Primal Lich Dust", 50, 0x2DB5)); // Assuming Primal Lich Dust item ID is 0x1F4
            AddReward(new BaseReward(typeof(Gold), 6000, "6000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(WhisperingWindSash), 1, "Alchemist's Ensemble")); // Assuming Alchemist's Ensemble is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Primal Lich's Secret quest!");
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

    public class ZephyrusTheAlchemist : MondainQuester
    {
        [Constructable]
        public ZephyrusTheAlchemist()
            : base("The Eccentric Alchemist", "Zephyrus")
        {
        }

        public ZephyrusTheAlchemist(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x191; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x2046; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new FancyShirt { Hue = Utility.Random(1, 3000), Name = "Zephyrus's Alchemist Robe" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new JesterHat { Hue = Utility.Random(1, 3000), Name = "Zephyrus's Alchemical Hat" });
            AddItem(new LeatherGloves { Hue = Utility.Random(1, 3000), Name = "Zephyrus's Mystical Gloves" });
            AddItem(new Kilt { Hue = Utility.Random(1, 3000), Name = "Zephyrus's Alchemist Kilt" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Zephyrus's Arcane Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(PrimalLichDustQuest)
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
