using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class PeachCobblerCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Peach Cobbler Collection"; } }

        public override object Description
        {
            get
            {
                return "Greetings, brave adventurer! I am Seraphina, the Culinary Enchantress. " +
                       "I have an unusual request for you: I need 50 Peach Cobblers. " +
                       "These delectable treats are essential for a grand feast I am preparing to celebrate the renewal of the Moonlit Garden. " +
                       "In return for your valuable assistance, I will reward you with gold, a rare Maxxia Scroll, and a unique outfit adorned with the hues of the harvest.";
            }
        }

        public override object Refuse { get { return "Very well. If you change your mind, return to me with the Peach Cobblers."; } }

        public override object Uncomplete { get { return "I still need 50 Peach Cobblers to complete the feast preparations. Please bring them to me as soon as you can!"; } }

        public override object Complete { get { return "Marvelous! You have collected the 50 Peach Cobblers I needed. Your efforts will ensure a grand feast and a prosperous celebration. " +
                       "Please accept these rewards as a token of my gratitude. May your adventures be as fulfilling as this feast!"; } }

        public PeachCobblerCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(PeachCobbler), "Peach Cobblers", 50, 0x1041)); // Assuming Peach Cobbler item ID is 0x1F8
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(VampiresMidnightCloak), 1, "Culinary Enchantress' Cloak")); // Assuming Culinary Enchantress' Robe is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Peach Cobbler Collection quest!");
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

    public class CulinaryEnchantressSeraphina : MondainQuester
    {
        [Constructable]
        public CulinaryEnchantressSeraphina()
            : base("The Culinary Enchantress", "Seraphina")
        {
        }

        public CulinaryEnchantressSeraphina(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x191; // Female Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x203C; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new FancyDress { Hue = Utility.Random(1, 3000), Name = "Seraphina's Enchanted Dress" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new JesterHat { Hue = Utility.Random(1, 3000), Name = "Seraphina's Festival Hat" });
            AddItem(new LeatherGloves { Hue = Utility.Random(1, 3000), Name = "Seraphina's Culinary Gloves" });
            AddItem(new Kilt { Hue = Utility.Random(1, 3000) });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Seraphina's Magical Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(PeachCobblerCollectorQuest)
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
