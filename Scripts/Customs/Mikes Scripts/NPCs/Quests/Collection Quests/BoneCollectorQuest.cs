using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class BoneCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Bone Collector's Request"; } }

        public override object Description
        {
            get
            {
                return "Greetings, adventurer! I am Morgath, a renowned necromancer with a peculiar request. " +
                       "I require 50 bones to complete my dark ritual. The ritual is crucial for my research into ancient magics. " +
                       "If you assist me by gathering these bones, I will reward you handsomely with gold, a rare Maxxia Scroll, " +
                       "and a unique Necromancer's Kabuto that will mark you as a true ally of the arcane arts.";
            }
        }

        public override object Refuse { get { return "Very well. If you change your mind, come back and see me. The ritual awaits!"; } }

        public override object Uncomplete { get { return "I still need 50 bones to complete my ritual. Please bring them to me as soon as possible!"; } }

        public override object Complete { get { return "Excellent work! You have gathered all the bones I needed. The ritual will proceed, thanks to your help. " +
                       "Please accept these rewards as a token of my gratitude. Thank you for your assistance!"; } }

        public BoneCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(Bone), "Bone", 50, 0xf7e)); // Assuming Bone item ID is 0x1F2
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(RoguesKabuto), 1, "Necromancer's Kabuto")); // Assuming Necromancer's Robe is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Bone Collector's Request quest!");
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

    public class BoneCollectorMorgath : MondainQuester
    {
        [Constructable]
        public BoneCollectorMorgath()
            : base("The Necromancer", "Bone Collector Morgath")
        {
        }

        public BoneCollectorMorgath(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 25);

            Body = 0x190; // Male Body

            Hue = Utility.RandomSkinHue();
            HairItemID = 0x203D; // Long hair style
            HairHue = 1161; // Hair hue (dark brown)
        }

        public override void InitOutfit()
        {
            AddItem(new Robe(0x2A3)); // Dark robe
            AddItem(new Sandals(0x1E2)); // Dark sandals
            AddItem(new SkullCap { Name = "Morgath's Skull Cap", Hue = 1150 }); // Custom Skull Cap
            AddItem(new Cloak { Name = "Morgath's Cloak", Hue = 1150 }); // Custom Cloak
            AddItem(new GnarledStaff { Name = "Morgath's Ritual Staff", Hue = 1150 }); // Custom Ritual Staff
            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Bag of Necromantic Components";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(BoneCollectorQuest)
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
