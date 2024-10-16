using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class WhitePearlCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The White Pearl Collector"; } }

        public override object Description
        {
            get
            {
                return "Ah, greetings brave soul! I am Oceania, the Seafaring Sage. My studies into the mysteries of the deep sea " +
                       "have led me to require 50 White Pearls. These pearls are not just beautiful but hold secrets of ancient magic " +
                       "that can reveal the hidden wonders of the ocean. Your efforts will be rewarded handsomely with gold, a rare Maxxia Scroll, " +
                       "and a Chest that channels the very essence of the sea.";
            }
        }

        public override object Refuse { get { return "Very well. If you change your mind, return to me with the White Pearls."; } }

        public override object Uncomplete { get { return "I am still in need of 50 White Pearls. Please gather them so that I may continue my research!"; } }

        public override object Complete { get { return "Marvelous! You have collected the 50 White Pearls I needed. Your contribution is invaluable. " +
                       "Accept these rewards as a token of my gratitude. May the mysteries of the sea guide you on your adventures!"; } }

        public WhitePearlCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(WhitePearl), "White Pearls", 50, 0x3196)); // Assuming White Pearl item ID is 0x1F4
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(SpectralGuardiansChest), 1, "Sea Sage's Chest")); // Assuming Sea Sage's Robe is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the White Pearl Collector quest!");
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

    public class SeaSageOceania : MondainQuester
    {
        [Constructable]
        public SeaSageOceania()
            : base("The Seafaring Sage", "Oceania")
        {
        }

        public SeaSageOceania(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x191; // Female Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x2047; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new FemaleLeatherChest { Hue = Utility.Random(1, 3000), Name = "Oceania's Sea Sage Robe" });
            AddItem(new ThighBoots { Hue = Utility.Random(1, 3000) });
            AddItem(new WideBrimHat { Hue = Utility.Random(1, 3000), Name = "Oceania's Enchanted Hat" });
            AddItem(new LeatherGloves { Hue = Utility.Random(1, 3000) });
            AddItem(new Kilt { Hue = Utility.Random(1, 3000) });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Oceania's Mystic Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(WhitePearlCollectorQuest)
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
