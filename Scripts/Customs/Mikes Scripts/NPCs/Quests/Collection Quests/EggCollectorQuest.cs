using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class EggCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Great Egg Hunt"; } }

        public override object Description
        {
            get
            {
                return "Ah, a new face! I am Thalric, the Keeper of the Lost Eggs. Long ago, magical eggs were scattered across the land, " +
                       "each holding a fragment of an ancient spell. I need you to help me collect 50 of these eggs. They are said to be hidden " +
                       "in various places and guarded by creatures of lore. In return for your tireless efforts, you will receive gold, a rare " +
                       "Maxxia Scroll, and an exquisite set of enchanted attire that will mark you as a true seeker of the arcane.";
            }
        }

        public override object Refuse { get { return "I understand. If you change your mind, return to me with the eggs."; } }

        public override object Uncomplete { get { return "I still need 50 Eggs. Gather them and bring them back to me!"; } }

        public override object Complete { get { return "Fantastic! You have gathered all 50 Eggs. Your bravery and resourcefulness are commendable. " +
                       "Please accept these rewards as a token of my gratitude. May your journeys be ever fruitful!"; } }

        public EggCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(Eggs), "Eggs", 50, 0x9B5)); // Assuming Egg item ID is 0x1F4
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(BardsMythicalTunic), 1, "Enchanted Attire")); // Assuming Enchanted Attire is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Great Egg Hunt quest!");
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

    public class ThalricTheKeeper : MondainQuester
    {
        [Constructable]
        public ThalricTheKeeper()
            : base("The Keeper of the Lost Eggs", "Thalric")
        {
        }

        public ThalricTheKeeper(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x190; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x2049; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new FancyShirt { Hue = Utility.Random(1, 3000), Name = "Thalric's Mystical Shirt" });
            AddItem(new LongPants { Hue = Utility.Random(1, 3000) });
            AddItem(new Shoes { Hue = Utility.Random(1, 3000) });
            AddItem(new FeatheredHat { Hue = Utility.Random(1, 3000), Name = "Thalric's Enchanted Hat" });
            AddItem(new GoldRing { Hue = Utility.Random(1, 3000), Name = "Thalric's Ancient Ring" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Thalric's Magical Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(EggCollectorQuest)
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
