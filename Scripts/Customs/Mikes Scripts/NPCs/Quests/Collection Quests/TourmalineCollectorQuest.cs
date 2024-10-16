using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class TourmalineCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Tourmaline Treasure Hunt"; } }

        public override object Description
        {
            get
            {
                return "Ah, brave adventurer! I am Gemara, the Crystal Sage. I seek your aid in gathering 50 Tourmaline, " +
                       "precious stones of rare beauty and magical properties. These Tourmaline will aid in crafting a potent enchantment " +
                       "that could turn the tide in our ongoing battle against the forces of darkness. For your noble effort, " +
                       "I will reward you with a handsome sum of gold, a rare Maxxia Scroll, and the illustrious Crystal Sage's Boots, " +
                       "which bears the radiant hues of the gem's essence.";
            }
        }

        public override object Refuse { get { return "Very well. If you change your mind, return with the Tourmaline."; } }

        public override object Uncomplete { get { return "I still need 50 Tourmaline to complete my enchantment. Please bring them to me when you can."; } }

        public override object Complete { get { return "Fantastic! You've brought me the 50 Tourmaline I required. Your contribution is invaluable. " +
                       "As a token of my gratitude, please accept these rewards. May the light of the Tourmaline guide your path!"; } }

        public TourmalineCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(Tourmaline), "Tourmaline", 50, 0xF2D)); // Assuming Tourmaline item ID is 0x1F3
            AddReward(new BaseReward(typeof(Gold), 6000, "6000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(BootsOfCommand), 1, "Crystal Sage's Boots")); // Assuming Crystal Sage's Robe is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Tourmaline Treasure Hunt quest!");
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

    public class CrystalSageGemara : MondainQuester
    {
        [Constructable]
        public CrystalSageGemara()
            : base("The Crystal Sage", "Gemara")
        {
        }

        public CrystalSageGemara(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(120, 120, 40);

            Body = 0x190; // Female Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x203C; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new PlainDress { Hue = Utility.Random(1, 3000), Name = "Gemara's Crystal Robe" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new FeatheredHat { Hue = Utility.Random(1, 3000), Name = "Gemara's Radiant Hat" });
            AddItem(new GoldRing { Hue = Utility.Random(1, 3000), Name = "Gemara's Enchanted Ring" });
            AddItem(new Cloak { Hue = Utility.Random(1, 3000), Name = "Gemara's Mystical Cloak" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Gemara's Magical Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(TourmalineCollectorQuest)
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
