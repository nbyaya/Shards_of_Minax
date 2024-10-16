using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class BarrelPartsQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Gadgeteer's Request"; } }

        public override object Description
        {
            get
            {
                return "Greetings, adventurer! I am Gortus, an inventor with a flair for the eccentric. " +
                       "My latest invention requires a large number of BarrelParts, and I am in dire need of 50 of them. " +
                       "These parts are essential for completing my newest gadget. Will you assist me in collecting them? " +
                       "In return, I will reward you with a generous amount of gold, a rare Maxxia Scroll, and a unique " +
                       "Gadgeteer's Cloak that you won't find anywhere else.";
            }
        }

        public override object Refuse { get { return "I understand. If you change your mind and wish to help, come back and see me. My invention awaits!"; } }

        public override object Uncomplete { get { return "I still need 50 BarrelParts to finish my invention. Please bring them to me as soon as possible!"; } }

        public override object Complete { get { return "Excellent work! You've gathered all 50 BarrelParts I need. My invention will be completed thanks to you. " +
                       "Please accept these rewards as a token of my gratitude. Thank you for your assistance!"; } }

        public BarrelPartsQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(BarrelLid), "Barrel Lid", 50, 0x1DB8)); // Assuming BarrelPart item ID is 0x1F0
			AddObjective(new ObtainObjective(typeof(BarrelStaves), "Barrel Staves", 50, 0x1EB1)); // Assuming BarrelPart item ID is 0x1F0
			AddObjective(new ObtainObjective(typeof(BarrelHoops), "Barrel Hoops", 50, 0x1DB7)); // Assuming BarrelPart item ID is 0x1F0
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(NavigatorsProtectiveCap), 1, "Gadgeteer's Cap")); // Assuming Gadgeteer's Cloak is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Gadgeteer's Request quest!");
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

    public class GadgeteerCloak : Cloak
    {
        [Constructable]
        public GadgeteerCloak() : base()
        {
            Name = "Gadgeteer's Cloak";
            Hue = 1150; // Unique hue
            // Add custom attributes or features here if desired
        }

        public GadgeteerCloak(Serial serial) : base(serial)
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

    public class GortusTheGadgeteer : MondainQuester
    {
        [Constructable]
        public GortusTheGadgeteer()
            : base("The Gadgeteer", "Gortus the Gadgeteer")
        {
        }

        public GortusTheGadgeteer(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 25);

            Body = 0x190; // Male Body

            Hue = Utility.RandomSkinHue();
            HairItemID = 0x203C; // Inventor's hair style
            HairHue = 1150; // Hair hue (bright color)
        }

        public override void InitOutfit()
        {
            AddItem(new FancyShirt(1150)); // Fancy shirt
            AddItem(new LongPants(1150)); // Long pants
            AddItem(new Shoes(1150)); // Matching shoes
            AddItem(new Cloak { Name = "Gortus' Cloak", Hue = 1150 }); // Custom Cloak
            AddItem(new TricorneHat { Name = "Gortus' Tricorne Hat", Hue = 1150 }); // Custom Hat
            AddItem(new Spellbook { Name = "Gortus' Spellbook", Hue = 1150 }); // Custom Spellbook
            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Gortus' Invention Kit";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(BarrelPartsQuest)
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
