using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class ClockworkCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Mechanical Marvels"; } }

        public override object Description
        {
            get
            {
                return "Greetings, adventurer! I am Professor Gearwind, an esteemed inventor and mechanical engineer. " +
                       "Currently, I am working on a grand project that requires a vast amount of Clockwork Assemblies. " +
                       "If you could bring me 50 Clockwork Assemblies, I will reward you handsomely with gold, a rare Maxxia Scroll, " +
                       "and an exclusive Mechanical Gear Hat that signifies your contribution to the advancement of technology.";
            }
        }

        public override object Refuse { get { return "Ah, I understand if you're not interested. Should you reconsider, feel free to return and assist me with my mechanical endeavors."; } }

        public override object Uncomplete { get { return "I still need 50 Clockwork Assemblies to continue my work. Please bring them to me at once!"; } }

        public override object Complete { get { return "Excellent! You've gathered all 50 Clockwork Assemblies. My project will advance thanks to your efforts. " +
                       "Here are your rewards. I am grateful for your assistance!"; } }

        public ClockworkCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(ClockworkAssembly), "Clockwork Assembly", 50, 0x1EA8)); // Assuming Clockwork Assembly item ID is 0x1F4
            AddReward(new BaseReward(typeof(Gold), 7000, "7000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(WardenOfTheWestsArms), 1, "Mechanical Gear")); // Assuming Mechanical Gear Hat is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Mechanical Marvels quest!");
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

    public class ClockworkCollectorProfessorGearwind : MondainQuester
    {
        [Constructable]
        public ClockworkCollectorProfessorGearwind()
            : base("The Inventor", "Professor Gearwind")
        {
        }

        public ClockworkCollectorProfessorGearwind(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(120, 120, 30);

            Body = 0x190; // Male Body

            Hue = Utility.RandomSkinHue();
            HairItemID = 0x203B; // Inventor's hair style
            HairHue = 1150; // Hair hue (silver)
        }

        public override void InitOutfit()
        {
            AddItem(new FancyShirt(1150)); // Fancy shirt
            AddItem(new LongPants(1150)); // Long pants
            AddItem(new Shoes(1150)); // Matching shoes
            AddItem(new Cap { Name = "Professor Gearwind's Gear Hat", Hue = 1150 }); // Custom Gear Hat
            AddItem(new Robe { Name = "Professor Gearwind's Lab Coat", Hue = 1150 }); // Custom Lab Coat
            AddItem(new GnarledStaff { Name = "Professor Gearwind's Staff", Hue = 1150 }); // Custom Staff
            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Bag of Mechanical Parts";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(ClockworkCollectorQuest)
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
