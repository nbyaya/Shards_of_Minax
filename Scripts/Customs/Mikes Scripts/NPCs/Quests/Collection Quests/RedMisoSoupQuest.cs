using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class RedMisoSoupQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Quest for Red Miso Soup"; } }

        public override object Description
        {
            get
            {
                return "Ah, brave adventurer! I am Roshin, the Culinary Sage. I have a humble request that only someone with a heart of gold " +
                       "can fulfill. My prized Red Miso Soup recipe has been missing for ages, and the only way to recreate it is by collecting " +
                       "50 bowls of RedMisoSoup. These bowls will help me perfect my recipe and restore the ancient flavors of our land. " +
                       "For your efforts, I will reward you with gold, a rare Maxxia Scroll, and a uniquely enchanted Chef's Attire that " +
                       "will surely make you the talk of the town.";
            }
        }

        public override object Refuse { get { return "I understand. If you change your mind, bring the RedMisoSoup to me and we will speak again."; } }

        public override object Uncomplete { get { return "I still need 50 bowls of RedMisoSoup. Please, bring them to me so I can complete my recipe!"; } }

        public override object Complete { get { return "You've done it! 50 bowls of RedMisoSoup are more than enough to help me perfect my recipe. Your dedication " +
                       "to the culinary arts is truly commendable. Accept these rewards as a token of my gratitude and may your adventures be " +
                       "as fulfilling as a hearty bowl of soup!"; } }

        public RedMisoSoupQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(RedMisoSoup), "Red Miso Soup", 50, 0x284F)); // Assuming RedMisoSoup item ID is 0x1F4
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(PaupersPlateGorget), 1, "Chef's Attire")); // Assuming Chef's Attire is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Quest for Red Miso Soup!");
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

    public class CulinarySageRoshin : MondainQuester
    {
        [Constructable]
        public CulinarySageRoshin()
            : base("The Culinary Sage", "Roshin")
        {
        }

        public CulinarySageRoshin(Serial serial)
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
            AddItem(new FancyShirt { Hue = Utility.Random(1, 3000), Name = "Roshin's Chef Shirt" });
            AddItem(new LongPants { Hue = Utility.Random(1, 3000) });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new JesterHat { Hue = Utility.Random(1, 3000), Name = "Roshin's Culinary Hat" });
            AddItem(new GoldBracelet { Hue = Utility.Random(1, 3000), Name = "Roshin's Golden Bracelet" });
            AddItem(new HalfApron { Hue = Utility.Random(1, 3000), Name = "Roshin's Apron" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Roshin's Culinary Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(RedMisoSoupQuest)
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
