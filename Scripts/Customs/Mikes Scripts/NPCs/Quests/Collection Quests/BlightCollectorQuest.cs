using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class BlightCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Blight of Shadows"; } }

        public override object Description
        {
            get
            {
                return "Ah, a brave soul! I am Morven, the Keeper of Shadows. The Blight that taints our land is a dangerous and " +
                       "corrupting force that I need to understand. I require 50 Blight items to further my research into this dark " +
                       "mystery. In return for your aid, you will be richly rewarded with gold, a rare Maxxia Scroll, and a " +
                       "distinguished garment that bears the mark of my shadowy domain.";
            }
        }

        public override object Refuse { get { return "Very well. Should you reconsider, bring me the Blight items when you are ready."; } }

        public override object Uncomplete { get { return "The Blight items are still missing! Please gather 50 of them for me."; } }

        public override object Complete { get { return "You have succeeded in gathering the 50 Blight items. Your dedication is commendable. " +
                       "Accept these rewards as a token of my appreciation. May the shadows guide your path."; } }

        public BlightCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(Blight), "Blight", 50, 0x3183)); // Assuming Blight item ID is 0x1F4
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(UrbanitesSneakers), 1, "Shadow Keeper's Shoes")); // Assuming Shadow Keeper's Garment is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Blight of Shadows quest!");
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

    public class ShadowKeeperMorven : MondainQuester
    {
        [Constructable]
        public ShadowKeeperMorven()
            : base("The Keeper of Shadows", "Morven")
        {
        }

        public ShadowKeeperMorven(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x190; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x2046; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new Robe { Hue = Utility.Random(1, 3000), Name = "Morven's Shadow Robe" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new Cloak { Hue = Utility.Random(1, 3000), Name = "Morven's Enigmatic Cloak" });
            AddItem(new SkullCap { Hue = Utility.Random(1, 3000), Name = "Morven's Mystical Cap" });
            AddItem(new GoldRing { Hue = Utility.Random(1, 3000), Name = "Morven's Shadow Ring" });
            AddItem(new LeatherGloves { Hue = Utility.Random(1, 3000), Name = "Morven's Shadow Gloves" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Morven's Arcane Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(BlightCollectorQuest)
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
