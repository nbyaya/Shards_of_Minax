using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class ReflectiveWolfEyeQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Reflective Wolf Eye Hunt"; } }

        public override object Description
        {
            get
            {
                return "Greetings, brave adventurer! I am Kaelith, the Enigmatic Seer. I am in dire need of 50 Reflective Wolf Eyes. " +
                       "These mystical orbs are crucial for the ancient rites I perform to protect our realm from dark forces. Your aid in " +
                       "this matter will not go unnoticed. In return, you shall be rewarded with gold, a rare Maxxia Scroll, and an exquisitely " +
                       "enchanted Seer's Attire that will surely mark you as a true protector of the realm.";
            }
        }

        public override object Refuse { get { return "Very well. If you change your mind, return to me with the Reflective Wolf Eyes."; } }

        public override object Uncomplete { get { return "I still require 50 Reflective Wolf Eyes. Please bring them to me so I can continue my rites."; } }

        public override object Complete { get { return "Marvelous! You have brought me the 50 Reflective Wolf Eyes I required. Your bravery and dedication are truly commendable. " +
                       "Please accept these rewards as a token of my gratitude. May you continue to safeguard our world with valor!"; } }

        public ReflectiveWolfEyeQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(ReflectiveWolfEye), "Reflective Wolf Eyes", 50, 0x5749)); // Assuming Reflective Wolf Eye item ID is 0x1F0
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(BanditsLegs), 1, "Seer's Attire")); // Assuming Seer's Attire is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Reflective Wolf Eye Hunt quest!");
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

    public class KaelithTheEnigmatic : MondainQuester
    {
        [Constructable]
        public KaelithTheEnigmatic()
            : base("The Enigmatic Seer", "Kaelith")
        {
        }

        public KaelithTheEnigmatic(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x191; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x2047; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new Shirt { Hue = Utility.Random(1, 3000), Name = "Kaelith's Mystical Robe" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new WizardsHat { Hue = Utility.Random(1, 3000), Name = "Kaelith's Enigmatic Hat" });
            AddItem(new LeatherGloves { Hue = Utility.Random(1, 3000), Name = "Kaelith's Seer's Gloves" });
            AddItem(new LongPants { Hue = Utility.Random(1, 3000) });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Kaelith's Arcane Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(ReflectiveWolfEyeQuest)
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
