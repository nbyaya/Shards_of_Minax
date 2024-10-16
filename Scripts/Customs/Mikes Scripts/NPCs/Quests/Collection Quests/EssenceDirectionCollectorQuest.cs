using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class EssenceDirectionCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Mystic's Request"; } }

        public override object Description
        {
            get
            {
                return "Greetings, brave adventurer! I am Eldara the Mystic. I require your aid in collecting 50 Essence Directions. " +
                       "These essences are crucial for my magical research and will aid in the protection of our realm. Your assistance will be " +
                       "rewarded with gold, a rare Maxxia Scroll, and a unique Mystic Robe that will enhance your arcane prowess.";
            }
        }

        public override object Refuse { get { return "I understand. If you change your mind, return to me with the essence directions."; } }

        public override object Uncomplete { get { return "I still need 50 Essence Directions. Please bring them to me so I can proceed with my research!"; } }

        public override object Complete { get { return "Fantastic! You have brought me the 50 Essence Directions I needed. Your help is invaluable. " +
                       "Please accept these rewards as a token of my gratitude. May your journey be filled with magic and wonder!"; } }

        public EssenceDirectionCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(EssenceDirection), "Essence Direction", 50, 0x571C)); // Assuming Essence Direction item ID is 0x1F1
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(SorceressMidnightRobe), 1, "Mystic Robe")); // Assuming Mystic Robe is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Mystic's Request quest!");
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

    public class EldaraTheMystic : MondainQuester
    {
        [Constructable]
        public EldaraTheMystic()
            : base("The Mystic", "Eldara")
        {
        }

        public EldaraTheMystic(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 25);

            Body = 0x191; // Female Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x203F; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new Robe { Hue = Utility.Random(1, 3000), Name = "Eldara's Mystic Robe" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new WizardsHat { Hue = Utility.Random(1, 3000), Name = "Eldara's Mystic Hat" });
            AddItem(new GoldRing { Hue = Utility.Random(1, 3000), Name = "Eldara's Enchanted Necklace" });
            AddItem(new QuarterStaff { Hue = Utility.Random(1, 3000), Name = "Eldara's Magical Staff" }); // Assuming Magical Staff is a custom item
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Eldara's Magical Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(EssenceDirectionCollectorQuest)
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
