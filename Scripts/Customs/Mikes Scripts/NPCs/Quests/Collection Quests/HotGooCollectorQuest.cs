using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class HotGooCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Goo of Legends"; } }

        public override object Description
        {
            get
            {
                return "Greetings, brave traveler! I am Quixara, the Enigmatic Alchemist. My research into the ancient arts of alchemy " +
                       "has led me to the discovery of a rare substance known as HotGoo. This mysterious goo is essential for my latest " +
                       "potions, and I require 50 of these elusive items to complete my work. In exchange for your invaluable assistance, " +
                       "I shall reward you with a handsome sum of gold, a rare Maxxia Scroll, and the unique and vibrant Alchemist's Ensemble.";
            }
        }

        public override object Refuse { get { return "Very well. Should you change your mind, return with the HotGoo."; } }

        public override object Uncomplete { get { return "I still need 50 HotGoo to proceed with my alchemical research. Please bring them to me!"; } }

        public override object Complete { get { return "Fantastic! You have brought me the 50 HotGoo I needed. Your contribution is immensely valuable. " +
                       "Accept these rewards as a token of my gratitude, and may your own journeys be filled with wonder and discovery!"; } }

        public HotGooCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(GoblinBlood), "HotGoo", 50, 0x572C)); // Assuming HotGoo item ID is 0xF7E
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(SantasEnchantedRobe), 1, "Alchemist's Ensemble")); // Assuming Alchemist's Ensemble is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Goo of Legends quest!");
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

    public class AlchemistQuixara : MondainQuester
    {
        [Constructable]
        public AlchemistQuixara()
            : base("The Enigmatic Alchemist", "Quixara")
        {
        }

        public AlchemistQuixara(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x190; // Female Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x203C; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new FemaleLeatherChest { Hue = Utility.Random(1, 3000), Name = "Quixara's Alchemist Chest" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new FeatheredHat { Hue = Utility.Random(1, 3000), Name = "Quixara's Enigmatic Hat" });
            AddItem(new LeatherGloves { Hue = Utility.Random(1, 3000), Name = "Quixara's Enigmatic Gloves" });
            AddItem(new Kilt { Hue = Utility.Random(1, 3000) });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Quixara's Alchemist Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(HotGooCollectorQuest)
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
