using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class SlithEyeCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The SlithEye Collection"; } }

        public override object Description
        {
            get
            {
                return "Hail, valiant hero! I am Serethis, the Warden of the Hidden Glade. A dark menace lurks in the shadows, " +
                       "and I need your help to gather 50 SlithEye. These sinister eyes are crucial to crafting a protective charm " +
                       "that will keep our realm safe from an encroaching darkness. In return for your brave efforts, you shall be " +
                       "rewarded with gold, a rare Maxxia Scroll, and the magnificent Warden's Regalia—a symbol of your valor.";
            }
        }

        public override object Refuse { get { return "Very well. If you change your mind, return to me with the SlithEye."; } }

        public override object Uncomplete { get { return "I still require 50 SlithEye. Bring them to me to aid in the defense of our realm!"; } }

        public override object Complete { get { return "Splendid! You have gathered the 50 SlithEye I required. Your bravery shines brightly. " +
                       "Accept these rewards as a testament to your courage. May the light guide you in your future endeavors!"; } }

        public SlithEyeCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(SlithEye), "SlithEye", 50, 0x5749)); // Assuming SlithEye item ID is 0x1F3
            AddReward(new BaseReward(typeof(Gold), 6000, "6000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(ElvenSnowBoots), 1, "Warden's Regalia")); // Assuming Warden's Regalia is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the SlithEye Collection quest!");
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

    public class WardenSerethis : MondainQuester
    {
        [Constructable]
        public WardenSerethis()
            : base("The Warden of the Hidden Glade", "Serethis")
        {
        }

        public WardenSerethis(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(120, 100, 40);

            Body = 0x190; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x203B; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new Robe { Hue = Utility.Random(1, 3000), Name = "Serethis's Warden Robe" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new FeatheredHat { Hue = Utility.Random(1, 3000), Name = "Serethis's Enchanted Hat" });
            AddItem(new GoldBracelet { Hue = Utility.Random(1, 3000), Name = "Serethis's Mystic Bracelet" });
            AddItem(new PlateGloves { Hue = Utility.Random(1, 3000), Name = "Serethis's Guardian Gloves" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Serethis's Magic Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(SlithEyeCollectorQuest)
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
