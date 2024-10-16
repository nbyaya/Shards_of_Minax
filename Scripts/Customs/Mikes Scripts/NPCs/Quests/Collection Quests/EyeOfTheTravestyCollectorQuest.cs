using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class EyeOfTheTravestyCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Eye of the Travesty Collector"; } }

        public override object Description
        {
            get
            {
                return "Greetings, brave adventurer! I am Mystara, the Keeper of Secrets. My studies into the arcane arts have led me to " +
                       "seek out 50 EyeOfTheTravesty. These eyes are crucial for a grand ritual that will reveal hidden truths about our world. " +
                       "In exchange for your aid, I will reward you with gold, a rare Maxxia Scroll, and a magnificent and enchanted Mystara's Robe.";
            }
        }

        public override object Refuse { get { return "I understand. Should you decide to assist me later, return with the Eyes of the Travesty."; } }

        public override object Uncomplete { get { return "I still require 50 EyeOfTheTravesty to complete my ritual. Please bring them to me as soon as you can!"; } }

        public override object Complete { get { return "Fantastic work! You have gathered the 50 EyeOfTheTravesty. Your contribution is invaluable. " +
                       "As a token of my appreciation, please accept these rewards. May the arcane forces favor you in your future endeavors!"; } }

        public EyeOfTheTravestyCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(EyeOfTheTravesty), "EyeOfTheTravesty", 50, 0x318D)); // Assuming EyeOfTheTravesty item ID is 0x2B04
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(PopStarsFingerlessGloves), 1, "Mystara's Robe")); // Assuming Mystara's Robe is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Eye of the Travesty Collector quest!");
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

    public class Mystara : MondainQuester
    {
        [Constructable]
        public Mystara()
            : base("The Keeper of Secrets", "Mystara")
        {
        }

        public Mystara(Serial serial)
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
            AddItem(new FemaleLeatherChest { Hue = Utility.Random(1, 3000), Name = "Mystara's Enchanted Robe" });
            AddItem(new ThighBoots { Hue = Utility.Random(1, 3000) });
            AddItem(new FeatheredHat { Hue = Utility.Random(1, 3000), Name = "Mystara's Mystical Hat" });
            AddItem(new Kilt { Hue = Utility.Random(1, 3000), Name = "Mystara's Arcane Bracelet" });
            AddItem(new GoldRing { Hue = Utility.Random(1, 3000), Name = "Mystara's Magical Ring" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Mystara's Arcane Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(EyeOfTheTravestyCollectorQuest)
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
