using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class TaintCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Enigmatic Taint Collector"; } }

        public override object Description
        {
            get
            {
                return "Greetings, brave soul! I am Nyx, the Keeper of Shadows. I require your aid to collect 50 Taints, " +
                       "an eerie substance of great significance to my dark studies. These Taints are vital for unraveling the " +
                       "mysteries of the shadow realm. In exchange for your efforts, I shall reward you with gold, a rare Maxxia Scroll, " +
                       "and a unique set of Shadow Keeper's Garments that will mark you as an ally of the shadows.";
            }
        }

        public override object Refuse { get { return "Very well. Should you reconsider, return to me with the Taints."; } }

        public override object Uncomplete { get { return "I still require 50 Taints. Bring them to me so I can proceed with my research!"; } }

        public override object Complete { get { return "Excellent! You have gathered the 50 Taints I needed. Your assistance is greatly appreciated. " +
                       "Please accept these rewards as a token of my gratitude. May the shadows guide your path!"; } }

        public TaintCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(Taint), "Taints", 50, 0x3187)); // Assuming Taint item ID is 0x1F5
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(MistletoeMuffler), 1, "Shadow Keeper's Garments")); // Assuming Shadow Keeper's Garments is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Enigmatic Taint Collector quest!");
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

    public class ShadowKeeperNyx : MondainQuester
    {
        [Constructable]
        public ShadowKeeperNyx()
            : base("The Keeper of Shadows", "Nyx")
        {
        }

        public ShadowKeeperNyx(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x191; // Female Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x2041; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new LongPants { Hue = Utility.Random(1, 3000), Name = "Nyx's Shadow Pants" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new FeatheredHat { Hue = Utility.Random(1, 3000), Name = "Nyx's Shadow Hat" });
            AddItem(new LeatherGloves { Hue = Utility.Random(1, 3000) });
            AddItem(new Kilt { Hue = Utility.Random(1, 3000) });
            AddItem(new GoldRing { Hue = Utility.Random(1, 3000), Name = "Nyx's Enchanted Ring" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Nyx's Shadow Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(TaintCollectorQuest)
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
