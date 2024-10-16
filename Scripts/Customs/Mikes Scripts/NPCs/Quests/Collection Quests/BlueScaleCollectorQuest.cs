using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class BlueScaleCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Blue Scale Endeavor"; } }

        public override object Description
        {
            get
            {
                return "Greetings, brave explorer! I am Kaelith, the Dragon's Guardian. I am in dire need of 50 Blue Scales, " +
                       "harvested from the fiercest of creatures. These scales are essential for a potent enchantment I am preparing. " +
                       "In return for your heroic efforts, you will be rewarded with a substantial amount of gold, a coveted Maxxia Scroll, " +
                       "and the illustrious Dragon Guardian's Attire, which is imbued with the essence of dragons.";
            }
        }

        public override object Refuse { get { return "Very well. Should you change your mind, return with the Blue Scales and I shall assist you."; } }

        public override object Uncomplete { get { return "I still require 50 Blue Scales to proceed with my enchantment. Please bring them to me!"; } }

        public override object Complete { get { return "Fantastic work! You have retrieved the 50 Blue Scales I sought. Your bravery is truly commendable. " +
                       "Please accept these rewards as a token of my gratitude. May your future endeavors be just as successful!"; } }

        public BlueScaleCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(BlueScales), "Blue Scales", 50, 0x26B4)); // Assuming Blue Scale item ID is 0xF7F
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(NewWaveNeonShades), 1, "Dragon Guardian's Shades")); // Assuming Dragon Guardian's Attire is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Blue Scale Endeavor quest!");
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

    public class DragonGuardianKaelith : MondainQuester
    {
        [Constructable]
        public DragonGuardianKaelith()
            : base("The Dragon's Guardian", "Kaelith")
        {
        }

        public DragonGuardianKaelith(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(120, 120, 40);

            Body = 0x190; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x2044; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new Robe { Hue = Utility.Random(1, 3000), Name = "Kaelith's Dragon Robe" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new DragonHelm { Hue = Utility.Random(1, 3000), Name = "Kaelith's Dragon Helm" });
            AddItem(new PlateGloves { Hue = Utility.Random(1, 3000), Name = "Kaelith's Dragon Gauntlets" });
            AddItem(new DragonLegs { Hue = Utility.Random(1, 3000), Name = "Kaelith's Dragon Leggings" });
            AddItem(new DragonArms { Hue = Utility.Random(1, 3000), Name = "Kaelith's Dragon Bracers" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Kaelith's Dragon Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(BlueScaleCollectorQuest)
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
