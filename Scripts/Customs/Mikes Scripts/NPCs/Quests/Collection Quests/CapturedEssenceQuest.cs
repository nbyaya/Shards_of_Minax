using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class CapturedEssenceQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Enigma of Captured Essence"; } }

        public override object Description
        {
            get
            {
                return "Ah, greetings, courageous adventurer! I am Ravathor the Enigmatic, keeper of arcane mysteries. " +
                       "I require your aid to gather 50 CapturedEssences. These essences are key to unlocking the ancient " +
                       "secrets of the realm, and their collection will greatly assist in my research. In gratitude for your help, " +
                       "you will be rewarded with a substantial amount of gold, a rare Maxxia Scroll, and an ornate ensemble " +
                       "that reflects the depth of our shared journey.";
            }
        }

        public override object Refuse { get { return "Very well. Should you change your mind, bring me the CapturedEssences."; } }

        public override object Uncomplete { get { return "I still need 50 CapturedEssences. Return to me with them so I may continue my work!"; } }

        public override object Complete { get { return "Splendid! You have gathered the 50 CapturedEssences I sought. Your dedication is invaluable. " +
                       "Accept these rewards as a symbol of our shared quest. May your path be ever enlightened!"; } }

        public CapturedEssenceQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(CapturedEssence), "CapturedEssences", 50, 0x318E)); // Assuming CapturedEssence item ID is 0x1F3
            AddReward(new BaseReward(typeof(Gold), 7000, "7000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(SommelierBodySash), 1, "Ravathor's Enigmatic Sash")); // Assuming Enigmatic Ensemble is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Enigma of Captured Essence quest!");
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

    public class RavathorTheEnigmatic : MondainQuester
    {
        [Constructable]
        public RavathorTheEnigmatic()
            : base("The Enigmatic Sage", "Ravathor")
        {
        }

        public RavathorTheEnigmatic(Serial serial)
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
            AddItem(new FemaleLeatherChest { Hue = Utility.Random(1, 3000), Name = "Ravathor's Enigmatic Robe" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new SkullCap { Hue = Utility.Random(1, 3000), Name = "Ravathor's Ancient Headpiece" });
            AddItem(new Cloak { Hue = Utility.Random(1, 3000), Name = "Ravathor's Cloak of Secrets" });
            AddItem(new GoldRing { Hue = Utility.Random(1, 3000), Name = "Ravathor's Arcane Ring" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Ravathor's Mysterious Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(CapturedEssenceQuest)
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
