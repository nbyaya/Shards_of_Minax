using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class TatteredClothQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Tattered Cloth Dilemma"; } }

        public override object Description
        {
            get
            {
                return "Ah, brave adventurer! I am Elandor, the Enigmatic Weaver. For centuries, I have safeguarded the secrets of " +
                       "ancient textile arts. However, a terrible mishap has led to a flood of UncutCloth that must be reprocessed. " +
                       "These scraps hold the essence of long-forgotten weaves and patterns. If you bring me 500 UncutCloth, I will " +
                       "reward you with gold, a rare Maxxia Scroll, and an exquisite outfit of my own design. Your assistance is " +
                       "crucial to restoring the ancient art. Will you take on this task?";
            }
        }

        public override object Refuse { get { return "Very well. If you change your mind, return to me with the UncutCloth."; } }

        public override object Uncomplete { get { return "I still require 500 UncutCloth. Please gather them and return to me!"; } }

        public override object Complete { get { return "Wonderful! You have brought me the 500 UncutCloth I needed. Your efforts have helped preserve the " +
                       "ancient art of weaving. Accept these rewards as a token of my appreciation. May your journey be filled with " +
                       "many more grand adventures!"; } }

        public TatteredClothQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(UncutCloth), "UncutCloth", 500, 0x1767)); // Assuming UncutCloth item ID is 0x1F5
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(SmithsProtectiveTunic), 1, "Elandor's Weaver Outfit")); // Assuming Elandor's Weaver Outfit is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Tattered Cloth Dilemma quest!");
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

    public class EnigmaticWeaverElandor : MondainQuester
    {
        [Constructable]
        public EnigmaticWeaverElandor()
            : base("The Enigmatic Weaver", "Elandor")
        {
        }

        public EnigmaticWeaverElandor(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x190; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x203C; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new Robe { Hue = Utility.Random(1, 3000), Name = "Elandor's Mystical Robe" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new FeatheredHat { Hue = Utility.Random(1, 3000), Name = "Elandor's Woven Hat" });
            AddItem(new LeatherGloves { Hue = Utility.Random(1, 3000) });
            AddItem(new Kilt { Hue = Utility.Random(1, 3000) });
            AddItem(new Cloak { Hue = Utility.Random(1, 3000), Name = "Elandor's Enchanted Cloak" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Elandor's Crafting Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(TatteredClothQuest)
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
