using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class PassionEssenceCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Essence of Passion's Request"; } }

        public override object Description
        {
            get
            {
                return "Greetings, adventurer! I am Valtor, the Keeper of Passion. I need your help to collect 50 Essences of Passion, " +
                       "which are crucial for my mystical rituals. Your dedication to gathering these essences will be rewarded with gold, " +
                       "a rare Maxxia Scroll, and an exquisite Bandana that symbolizes your commitment to this quest.";
            }
        }

        public override object Refuse { get { return "I understand. If you change your mind, return with the essences."; } }

        public override object Uncomplete { get { return "I am still in need of 50 Essences of Passion. Please bring them to me so that I may continue with my rituals!"; } }

        public override object Complete { get { return "Fantastic! You have gathered the 50 Essences of Passion I required. Your efforts are truly appreciated. " +
                       "Please accept these rewards as a token of my gratitude. May your journey be filled with wonder!"; } }

        public PassionEssenceCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(EssencePassion), "Essence of Passion", 50, 0x571C)); // Assuming EssencePassion item ID is 0x1F1
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(BeggarsLuckyBandana), 1, "Valtor's Mystical Bandana")); // Assuming Valtor's Mystical Robe is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Essence of Passion's Request quest!");
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

    public class ValtorTheKeeper : MondainQuester
    {
        [Constructable]
        public ValtorTheKeeper()
            : base("The Keeper of Passion", "Valtor")
        {
        }

        public ValtorTheKeeper(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(120, 120, 30);

            Body = 0x190; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x203F; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new Robe { Hue = Utility.Random(1, 3000), Name = "Valtor's Mystical Robe" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new WizardsHat { Hue = Utility.Random(1, 3000), Name = "Valtor's Enchanted Hat" });
            AddItem(new GoldRing { Hue = Utility.Random(1, 3000), Name = "Ring of Passion" });
            AddItem(new Necklace { Hue = Utility.Random(1, 3000), Name = "Amulet of Wisdom" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Valtor's Spellcaster's Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(PassionEssenceCollectorQuest)
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
