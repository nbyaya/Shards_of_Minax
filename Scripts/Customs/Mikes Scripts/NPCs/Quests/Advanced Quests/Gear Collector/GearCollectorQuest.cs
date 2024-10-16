using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class ExoticGearCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Exotic Gear Hunt"; } }

        public override object Description
        {
            get
            {
                return "Greetings, adventurer! I am Felix, the master clockmaker of our town. I am in dire need of 30 Exotic Gears to complete my latest masterpiece—a grand clock that will stand as a testament to our craftsmanship for generations. These Exotic Gears are not easily found, but I have heard whispers of an eccentric gear maker who possesses them. Bring me these gears, and I shall reward you with gold, a finely crafted Timekeeper's Ring, and the prestigious Clockmaker's Robe!";
            }
        }

        public override object Refuse { get { return "I see. If you change your mind, seek out the Exotic Gears I require."; } }

        public override object Uncomplete { get { return "I still require 30 Exotic Gears to complete my grand clock. Please bring them to me!"; } }

        public override object Complete { get { return "Fantastic! You've gathered the 30 Exotic Gears I needed. Your contribution is invaluable. Please accept these rewards as a token of my gratitude, and may your timepieces always be precise!"; } }

        public ExoticGearCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(ExoticGear), "Exotic Gears", 30, 0x1F4)); // Replace 0x1F4 with the actual item ID
            AddReward(new BaseReward(typeof(Gold), 3000, "3000 Gold"));
            AddReward(new BaseReward(typeof(TimekeepersRing), 1, "Timekeeper's Ring")); // Define TimekeepersRing similarly to PizzaLoversOutfit
            AddReward(new BaseReward(typeof(ClockmakersRobe), 1, "Clockmaker's Robe")); // Define ClockmakersRobe similarly
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Exotic Gear Hunt Quest!");
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

    // Define the reward items similarly to PizzaLoversOutfit
    public class TimekeepersRing : Item
    {
        [Constructable]
        public TimekeepersRing() : base(0x108A) // Replace with desired item ID
        {
            Name = "Timekeeper's Ring";
            Hue = Utility.Random(1, 3000);
        }

        public TimekeepersRing(Serial serial) : base(serial)
        {
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

    public class ClockmakersRobe : Item
    {
        [Constructable]
        public ClockmakersRobe() : base(0x1C1) // Replace with desired item ID
        {
            Name = "Clockmaker's Robe";
            Hue = Utility.Random(1, 3000);
        }

        public ClockmakersRobe(Serial serial) : base(serial)
        {
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
