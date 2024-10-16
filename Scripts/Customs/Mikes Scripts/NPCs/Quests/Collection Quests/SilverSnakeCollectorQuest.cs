using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class SilverSnakeCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Silver Serpent's Bounty"; } }

        public override object Description
        {
            get
            {
                return "Greetings, brave adventurer! I am Seraphius, the Serpent Sage. A great calamity has befallen my sacred grove—" +
                       "silver serpents, once a symbol of prosperity, have turned hostile and now endanger our realm. I need your help " +
                       "to gather 50 Silver Snake Skins from these creatures. Your courage will be handsomely rewarded with gold, " +
                       "a rare Maxxia Scroll, and a specially enchanted Serpent Sage's Vestment that will signify your valor.";
            }
        }

        public override object Refuse { get { return "Very well. If you change your mind, return to me with the Silver Snake Skins."; } }

        public override object Uncomplete { get { return "I still require 50 Silver Snake Skins. Please bring them to me so that we may restore peace to our grove!"; } }

        public override object Complete { get { return "Marvelous! You have collected the 50 Silver Snake Skins I needed. Your bravery and dedication have saved the grove. " +
                       "Please accept these rewards as a token of my deepest gratitude. May the spirits of the serpents watch over you!"; } }

        public SilverSnakeCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(SilverSnakeSkin), "Silver Snake Skins", 50, 0x5744)); // Assuming Silver Snake Skin item ID is 0x0F7
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(FilmNoirDetectivesTrenchCoat), 1, "Serpent Sage's Vestment")); // Assuming Serpent Sage's Vestment is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Silver Serpent's Bounty quest!");
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

    public class SerpentSageSeraphius : MondainQuester
    {
        [Constructable]
        public SerpentSageSeraphius()
            : base("The Serpent Sage", "Seraphius")
        {
        }

        public SerpentSageSeraphius(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x191; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x203F; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new Robe { Hue = Utility.Random(1, 3000), Name = "Seraphius's Enchanted Robe" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new SkullCap { Hue = Utility.Random(1, 3000), Name = "Seraphius's Mystical Cap" });
            AddItem(new GoldBracelet { Hue = Utility.Random(1, 3000), Name = "Seraphius's Serpent Bracelet" });
            AddItem(new LeatherGloves { Hue = Utility.Random(1, 3000), Name = "Seraphius's Serpent Gloves" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Seraphius's Magical Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(SilverSnakeCollectorQuest)
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
