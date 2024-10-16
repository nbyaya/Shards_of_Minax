using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class BloodCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Dark Ritual"; } }

        public override object Description
        {
            get
            {
                return "Greetings, brave adventurer! I am Alaric the Blood Seeker, a mystic with a dire need for " +
                       "a rare substance to complete a dark and powerful ritual. I require 50 Blood of the Dark Father " +
                       "to summon the ancient spirits and unlock their secrets. Will you assist me in gathering this " +
                       "blood? In exchange, I will grant you gold, a rare Maxxia Scroll, and a unique and enchanted " +
                       "Ritual Robe that will mark you as an ally of the dark arts.";
            }
        }

        public override object Refuse { get { return "I see you are not interested. Should you reconsider, return to me and assist in this crucial ritual."; } }

        public override object Uncomplete { get { return "I still need 50 Blood of the Dark Father. Gather them and bring them to me for the ritual."; } }

        public override object Complete { get { return "Excellent work! With the blood you’ve gathered, the ritual can proceed. Accept these rewards as thanks for your aid."; } }

        public BloodCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(BloodOfTheDarkFather), "Blood of the Dark Father", 50, 0x9D7F)); // Assuming BloodOfTheDarkFather item ID is 0x1BF
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(MidnightRevelersBoots), 1, "Ritual Boots")); // Assuming Ritual Robe is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Dark Ritual quest!");
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

    public class BloodCollectorAlaric : MondainQuester
    {
        [Constructable]
        public BloodCollectorAlaric()
            : base("The Dark Mystic", "Alaric the Blood Seeker")
        {
        }

        public BloodCollectorAlaric(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 25);

            Body = 0x190; // Male Body

            Hue = 1150; // Dark hue for a more sinister look
            HairItemID = 0x203C; // Long, dark hair style
            HairHue = 1151; // Dark hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new Robe { Hue = 1151, Name = "Alaric's Ritual Robe" }); // Custom Ritual Robe
            AddItem(new Sandals(1151)); // Dark sandals
            AddItem(new SkullCap { Name = "Alaric's Mystic Skullcap", Hue = 1151 }); // Custom Skullcap
            AddItem(new GnarledStaff { Name = "Alaric's Ritual Staff", Hue = 1151 }); // Custom Ritual Staff
            Backpack backpack = new Backpack();
            backpack.Hue = 1151;
            backpack.Name = "Bag of Dark Components";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(BloodCollectorQuest)
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
