using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class ScourgeCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Scourge Collector"; } }

        public override object Description
        {
            get
            {
                return "Ah, a brave adventurer! I am Kael the Defiant, the guardian of the ancient Scourge. " +
                       "The Scourge are malevolent relics imbued with dark energy, and I need your help to gather 50 of them. " +
                       "These relics are vital for an ancient ritual I am preparing. " +
                       "In return for your courageous efforts, I shall reward you with gold, a rare Maxxia Scroll, and the legendary " +
                       "Scourge Guardian's Attire, known for its enigmatic beauty and mystical properties.";
            }
        }

        public override object Refuse { get { return "I understand. If you change your mind, return to me with the Scourge."; } }

        public override object Uncomplete { get { return "I still require 50 Scourge. Your assistance is crucial for the ritual!"; } }

        public override object Complete { get { return "Wonderful! You have collected the 50 Scourge I needed. Your bravery and dedication are commendable. " +
                       "Please accept these rewards as a token of my appreciation. May you be guided by strength and wisdom!"; } }

        public ScourgeCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(Scourge), "Scourge", 50, 0x3185)); // Assuming Scourge item ID is 0xF6A
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(ForestersGauntlets), 1, "Scourge Guardian's Attire")); // Assuming Scourge Guardian's Attire is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Scourge Collector quest!");
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

    public class ScourgeGuardianKael : MondainQuester
    {
        [Constructable]
        public ScourgeGuardianKael()
            : base("The Scourge Guardian", "Kael")
        {
        }

        public ScourgeGuardianKael(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x190; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x2042; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new PlateChest { Hue = Utility.Random(1, 3000), Name = "Kael's Scourge Robe" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new FeatheredHat { Hue = Utility.Random(1, 3000), Name = "Kael's Enigmatic Hat" });
            AddItem(new LeatherGloves { Hue = Utility.Random(1, 3000), Name = "Kael's Arcane Gloves" });
            AddItem(new PlateLegs { Hue = Utility.Random(1, 3000), Name = "Kael's Scourge Plate" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Kael's Mystical Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(ScourgeCollectorQuest)
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
