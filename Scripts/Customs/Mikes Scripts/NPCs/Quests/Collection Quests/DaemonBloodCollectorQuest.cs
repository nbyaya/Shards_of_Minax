using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;

namespace Server.Engines.Quests
{
    public class DaemonBloodCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Infernal Collector's Challenge"; } }

        public override object Description
        {
            get
            {
                return "Greetings, adventurer! I am Pyros, the Infernal Collector. I require your assistance to gather 50 DaemonBloods. " +
                       "These foul substances are crucial for my dark experiments and rituals. In exchange for your help, I will reward you " +
                       "with gold, a rare Maxxia Scroll, and a magical Infernal Robe that will ignite your enemies with its fiery aura.";
            }
        }

        public override object Refuse { get { return "I understand. Should you change your mind, return to me with the DaemonBloods."; } }

        public override object Uncomplete { get { return "I still need 50 DaemonBloods. Bring them to me to aid in my experiments!"; } }

        public override object Complete { get { return "Fantastic work! You've gathered the 50 DaemonBloods I required. As a token of my gratitude, please accept these rewards. " +
                       "May the flames of the inferno guide you on your path!"; } }

        public DaemonBloodCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(DaemonBlood), "DaemonBlood", 50, 0xF7D)); // Assuming DaemonBlood item ID is 0x1F0
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(MummysWrappings), 1, "Magical Infernal Wrappings")); // Assuming Infernal Robe is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Infernal Collector's Challenge quest!");
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

    public class PyrosTheInfernalCollector : MondainQuester
    {
        [Constructable]
        public PyrosTheInfernalCollector()
            : base("The Infernal Collector", "Pyros")
        {
        }

        public PyrosTheInfernalCollector(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 25);

            Body = 0x190; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x203D; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new Robe { Hue = Utility.Random(1, 3000), Name = "Pyros' Infernal Robe" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new WizardsHat { Hue = Utility.Random(1, 3000), Name = "Pyros' Fiery Hat" });
            AddItem(new GnarledStaff { Hue = Utility.Random(1, 3000), Name = "Pyros' Infernal Staff" }); // Assuming Infernal Staff is a custom item
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Pyros' Dark Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(DaemonBloodCollectorQuest)
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
