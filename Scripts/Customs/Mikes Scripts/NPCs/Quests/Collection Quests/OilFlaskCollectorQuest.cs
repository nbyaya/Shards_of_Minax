using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class OilFlaskCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Mysterious Alchemist's Request"; } }

        public override object Description
        {
            get
            {
                return "Greetings, daring explorer! I am Althoran, a seeker of rare alchemical ingredients. My current experiment requires " +
                       "500 Oil Flasks, which are crucial for distilling a potent elixir. These flasks will allow me to unlock the secrets of an " +
                       "ancient recipe lost to time. In exchange for your assistance, I will grant you gold, a rare Maxxia Scroll, and a unique " +
                       "Alchemist's Attire that will mark you as a true ally of the arcane arts.";
            }
        }

        public override object Refuse { get { return "Very well. Should you decide to help, return to me with the Oil Flasks."; } }

        public override object Uncomplete { get { return "I still need 500 Oil Flasks. Please bring them to me to assist with my alchemical research!"; } }

        public override object Complete { get { return "Marvelous! You have brought me the 500 Oil Flasks I required. Your help is invaluable. " +
                       "As a token of my gratitude, please accept these rewards. May your future endeavors be as successful as this one!"; } }

        public OilFlaskCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(OilFlask), "Oil Flasks", 500, 0x1C18)); // Assuming Oil Flask item ID is 0xF1F
            AddReward(new BaseReward(typeof(Gold), 10000, "10000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(ElementalistsGauntlets), 1, "Alchemist's Attire")); // Assuming Alchemist's Attire is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Oil Flask Collector quest!");
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

    public class AlchemistAlthoran : MondainQuester
    {
        [Constructable]
        public AlchemistAlthoran()
            : base("The Mysterious Alchemist", "Althoran")
        {
        }

        public AlchemistAlthoran(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x190; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x2044; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new LongPants { Hue = Utility.Random(1, 3000), Name = "Althoran's Alchemist Robe" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new WizardsHat { Hue = Utility.Random(1, 3000), Name = "Althoran's Enchanted Hat" });
            AddItem(new LeatherGloves { Hue = Utility.Random(1, 3000), Name = "Althoran's Mystic Gloves" });
            AddItem(new FemaleLeatherChest { Hue = Utility.Random(1, 3000), Name = "Althoran's Shrouded Chest" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Althoran's Alchemical Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(OilFlaskCollectorQuest)
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
