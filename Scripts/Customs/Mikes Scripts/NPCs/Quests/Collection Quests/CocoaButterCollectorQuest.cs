using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class CocoaButterCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Essence of Cocoa Butter"; } }

        public override object Description
        {
            get
            {
                return "Greetings, brave traveler! I am Zoranna, the Alchemist of the Enchanted Forest. " +
                       "My magical concoctions require a rare and potent ingredient: Cocoa Butter. " +
                       "I need you to collect 50 units of this precious substance to complete my latest elixir. " +
                       "In exchange for your efforts, I will reward you with gold, a rare Maxxia Scroll, " +
                       "and an exquisite Alchemist's Robe, enchanted with mystical properties.";
            }
        }

        public override object Refuse { get { return "I understand. If you change your mind, return with the Cocoa Butter."; } }

        public override object Uncomplete { get { return "I still require 50 units of Cocoa Butter. Please gather them so I can continue my work!"; } }

        public override object Complete { get { return "Wonderful! You have collected the 50 Cocoa Butter I needed. Your assistance is invaluable. " +
                       "Please accept these rewards as a token of my gratitude. May your adventures be filled with success!"; } }

        public CocoaButterCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(CocoaButter), "Cocoa Butter", 50, 0x1044)); // Assuming Cocoa Butter item ID is 0x1F2
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(DarkLordsRobe), 1, "Alchemist's Robe")); // Assuming Alchemist's Robe is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Cocoa Butter Collector quest!");
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

    public class ZorannaTheAlchemist : MondainQuester
    {
        [Constructable]
        public ZorannaTheAlchemist()
            : base("The Alchemist", "Zoranna")
        {
        }

        public ZorannaTheAlchemist(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x191; // Female Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x203B; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new Robe { Hue = Utility.Random(1, 3000), Name = "Zoranna's Alchemist Robe" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new FeatheredHat { Hue = Utility.Random(1, 3000), Name = "Zoranna's Alchemist Hat" });
            AddItem(new LeatherGloves { Hue = Utility.Random(1, 3000) });
            AddItem(new Kilt { Hue = Utility.Random(1, 3000) });
            AddItem(new Spellbook { Hue = Utility.Random(1, 3000), Name = "Zoranna's Tome of Potions" }); // Assuming this is a custom item
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Zoranna's Magical Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(CocoaButterCollectorQuest)
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
