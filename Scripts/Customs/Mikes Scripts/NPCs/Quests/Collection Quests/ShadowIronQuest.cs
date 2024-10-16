using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;

namespace Server.Engines.Quests
{
    public class ShadowIronQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Shadow Iron Discovery"; } }

        public override object Description
        {
            get
            {
                return "Ah, brave adventurer! I am Valthor, the Enigmatic Blacksmith. For centuries, " +
                       "I have studied the mysterious Shadow Iron, a rare and powerful metal with ancient " +
                       "properties. I need your help to gather 50 pieces of Shadow Iron Ore. " +
                       "In return for your aid, I shall reward you with gold, a rare Maxxia Scroll, " +
                       "and a unique Tunic imbued with the essence of the Shadow Iron!";
            }
        }

        public override object Refuse { get { return "I understand. Should you change your mind, return to me with the Shadow Iron Ore."; } }

        public override object Uncomplete { get { return "I am still waiting for the 50 pieces of Shadow Iron Ore. Please bring them to me at your earliest convenience!"; } }

        public override object Complete { get { return "You have done it! You have gathered all 50 pieces of Shadow Iron Ore. Your assistance is invaluable. " +
                       "Please accept these rewards as a token of my gratitude. May the Shadow Iron serve you well!"; } }

        public ShadowIronQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(ShadowIronOre), "Shadow Iron Ore", 50, 0x19B7)); // Assuming Shadow Iron Ore item ID is 0x1F2
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(DeepSeaTunic), 1, "Shadow Iron Tunic")); // Assuming Shadow Iron Armor is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Shadow Iron Discovery quest!");
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

    public class EnigmaticBlacksmithValthor : MondainQuester
    {
        [Constructable]
        public EnigmaticBlacksmithValthor()
            : base("The Enigmatic Blacksmith", "Valthor")
        {
        }

        public EnigmaticBlacksmithValthor(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x190; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x2040; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new PlateChest { Hue = Utility.Random(1, 3000), Name = "Valthor's Shadow Chestplate" });
            AddItem(new PlateLegs { Hue = Utility.Random(1, 3000) });
            AddItem(new PlateArms { Hue = Utility.Random(1, 3000) });
            AddItem(new PlateGloves { Hue = Utility.Random(1, 3000) });
            AddItem(new Cloak { Hue = Utility.Random(1, 3000) });
            AddItem(new SmithHammer { Hue = Utility.Random(1, 3000), Name = "Valthor's Forging Hammer" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Valthor's Crafting Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(ShadowIronQuest)
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
