using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;

namespace Server.Engines.Quests
{
    public class VitriolCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Vitriol of the Forgotten Alchemist"; } }

        public override object Description
        {
            get
            {
                return "Greetings, brave adventurer! I am Alistair, the Forgotten Alchemist. My alchemical research requires " +
                       "50 Vials of Vitriol. These vials are critical for my experiments to revive ancient potions of power. " +
                       "In exchange for your efforts, I shall reward you with gold, a rare Maxxia Scroll, and a unique alchemical attire " +
                       "that will mark you as a hero of the alchemical arts.";
            }
        }

        public override object Refuse { get { return "If you change your mind, return with the Vials of Vitriol. My research depends on them."; } }

        public override object Uncomplete { get { return "I am still in need of 50 Vials of Vitriol. Please bring them to me so I may continue my work."; } }

        public override object Complete { get { return "Marvelous! You have gathered the 50 Vials of Vitriol I sought. Your contribution is invaluable. " +
                       "Please accept these rewards as a token of my appreciation. May your adventures be ever so thrilling!"; } }

        public VitriolCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(VialOfVitriol), "Vials of Vitriol", 50, 0x5722)); // Assuming VialOfVitriol item ID is 0xE2B
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(FlowerChildSundress), 1, "Alchemist's Attire")); // Assuming Alchemist's Attire is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Vitriol Collector quest!");
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

    public class ForgottenAlchemistAlistair : MondainQuester
    {
        [Constructable]
        public ForgottenAlchemistAlistair()
            : base("The Forgotten Alchemist", "Alistair")
        {
        }

        public ForgottenAlchemistAlistair(Serial serial)
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
            AddItem(new Robe { Hue = Utility.Random(1, 3000), Name = "Alchemist's Robe" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new WizardsHat { Hue = Utility.Random(1, 3000), Name = "Alchemist's Hat" });
            AddItem(new LeatherGloves { Hue = Utility.Random(1, 3000), Name = "Alchemist's Gloves" });
            AddItem(new Shoes { Hue = Utility.Random(1, 3000) });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Alchemist's Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(VitriolCollectorQuest)
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
