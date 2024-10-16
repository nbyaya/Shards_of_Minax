using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;

namespace Server.Engines.Quests
{
    public class GraniteCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Granite Conundrum"; } }

        public override object Description
        {
            get
            {
                return "Hail, brave adventurer! I am Thalon, the Stone Sage. The ancient wards protecting my realm have been weakened, " +
                       "and only the sacred Granite can restore their strength. I need you to gather 50 pieces of Granite for me. " +
                       "In return, I shall reward you with gold, a rare Maxxia Scroll, and a special Stone Sage's Cap that holds mystical powers.";
            }
        }

        public override object Refuse { get { return "Very well. If you change your mind, return to me with the Granite."; } }

        public override object Uncomplete { get { return "The Granite is still missing! Please bring me 50 pieces of Granite to aid in restoring the wards."; } }

        public override object Complete { get { return "Ah, marvelous! You have brought me the 50 pieces of Granite. Your efforts are most commendable. " +
                       "Accept these rewards as a token of my gratitude. May the wards remain strong and protect you on your journey!"; } }

        public GraniteCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(Granite), "Granite", 50, 0x1779)); // Assuming Granite item ID is 0x0F7A
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(SherlocksSleuthingCap), 1, "Stone Sage's Cap")); // Assuming Stone Sage's Cloak is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Granite Conundrum quest!");
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

    public class StoneSageThalon : MondainQuester
    {
        [Constructable]
        public StoneSageThalon()
            : base("The Stone Sage", "Thalon")
        {
        }

        public StoneSageThalon(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x190; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x204F; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new Robe { Hue = Utility.Random(1, 3000), Name = "Thalon's Stone Robe" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new SkullCap { Hue = Utility.Random(1, 3000), Name = "Thalon's Mystical Skullcap" });
            AddItem(new LeatherGloves { Hue = Utility.Random(1, 3000), Name = "Thalon's Enchanted Gloves" });
            AddItem(new Kilt { Hue = Utility.Random(1, 3000) });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Thalon's Ancient Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(GraniteCollectorQuest)
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
