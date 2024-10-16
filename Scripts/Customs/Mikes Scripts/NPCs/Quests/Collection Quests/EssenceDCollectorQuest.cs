using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class EssenceDCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Essence of Diligence"; } }

        public override object Description
        {
            get
            {
                return "Greetings, brave adventurer! I am Elyndra, the Seer. I require your help to gather 50 Essence of Diligence. " +
                       "These essences are crucial for a ritual that will enhance our defenses. Your reward will include gold, a rare Maxxia Scroll, " +
                       "and a unique set of Seer's Attire that will reflect your dedication and skill.";
            }
        }

        public override object Refuse { get { return "I understand. If you change your mind, return to me with the essences."; } }

        public override object Uncomplete { get { return "I still need 50 Essence of Diligence. Please bring them to me as soon as you can!"; } }

        public override object Complete { get { return "Fantastic! You have collected the 50 Essence of Diligence. Your help is invaluable. " +
                       "Accept these rewards as a token of my gratitude. May your journey be blessed!"; } }

        public EssenceDCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(EssenceDiligence), "Essence of Diligence", 50, 0x571C)); // Assuming EssenceDiligence item ID is 0x1F1
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(ThiefsSilentShoes), 1, "Seer's Attire")); // Assuming Seer's Attire is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Essence of Diligence quest!");
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

    public class ElyndraTheSeer : MondainQuester
    {
        [Constructable]
        public ElyndraTheSeer()
            : base("The Seer", "Elyndra")
        {
        }

        public ElyndraTheSeer(Serial serial)
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
            AddItem(new Robe { Hue = Utility.Random(1, 3000), Name = "Elyndra's Seer Robe" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new SkullCap { Hue = Utility.Random(1, 3000), Name = "Elyndra's Mystical Cap" });
            AddItem(new GoldRing { Hue = Utility.Random(1, 3000), Name = "Elyndra's Enchanted Necklace" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Elyndra's Satchel of Mysteries";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(EssenceDCollectorQuest)
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
