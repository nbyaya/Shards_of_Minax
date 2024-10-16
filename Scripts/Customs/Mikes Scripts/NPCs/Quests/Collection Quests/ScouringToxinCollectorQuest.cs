using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class ScouringToxinCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Scouring Toxin Collector"; } }

        public override object Description
        {
            get
            {
                return "Greetings, brave adventurer! I am Zorath, the Master Alchemist. My research into ancient toxins has led me to a pressing need for 50 Scouring Toxins. " +
                       "These toxins are pivotal for concocting a powerful antidote that could save countless lives. " +
                       "In exchange for your help, I shall reward you with gold, a rare Maxxia Scroll, and a special Alchemist's Garb that will mark you as a true connoisseur of the arcane arts.";
            }
        }

        public override object Refuse { get { return "Very well. Should you change your mind, return to me with the Scouring Toxins."; } }

        public override object Uncomplete { get { return "I am still in need of 50 Scouring Toxins. Bring them to me so I can continue my vital work!"; } }

        public override object Complete { get { return "Splendid work! You have gathered the 50 Scouring Toxins I required. Your assistance is invaluable. " +
                       "Please accept these rewards as a token of my gratitude. May your travels be filled with success!"; } }

        public ScouringToxinCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(ScouringToxin), "Scouring Toxins", 50, 0x1848)); // Assuming Scouring Toxin item ID is 0xF5F
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(MysticsAegis), 1, "Alchemist's Garb")); // Assuming Alchemist's Garb is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Scouring Toxin Collector quest!");
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

    public class MasterAlchemistZorath : MondainQuester
    {
        [Constructable]
        public MasterAlchemistZorath()
            : base("The Master Alchemist", "Zorath")
        {
        }

        public MasterAlchemistZorath(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x190; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x203B; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new Robe { Hue = Utility.Random(1, 3000), Name = "Zorath's Alchemist Robe" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new WideBrimHat { Hue = Utility.Random(1, 3000), Name = "Zorath's Enchanted Hat" });
            AddItem(new LeatherGloves { Hue = Utility.Random(1, 3000), Name = "Zorath's Alchemist Gloves" });
            AddItem(new Cloak { Hue = Utility.Random(1, 3000), Name = "Zorath's Mystical Cloak" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Zorath's Potion Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(ScouringToxinCollectorQuest)
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
