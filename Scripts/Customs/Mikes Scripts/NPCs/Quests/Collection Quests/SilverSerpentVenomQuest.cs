using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class SilverSerpentVenomQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Venomous Task"; } }

        public override object Description
        {
            get
            {
                return "Hail, brave adventurer! I am Corvian, the Enigmatic Alchemist. I have come across a dire need for Silver Serpent Venoms. " +
                       "These vile substances are essential for a powerful antidote I'm concocting. Bring me 50 of these venoms, and you will be handsomely rewarded. " +
                       "In return for your valiant efforts, I shall grant you gold, a rare Maxxia Scroll, and a specially crafted outfit to aid you in your adventures.";
            }
        }

        public override object Refuse { get { return "I understand. If you change your mind, return with the Silver Serpent Venoms."; } }

        public override object Uncomplete { get { return "I still need 50 Silver Serpent Venoms. Please return to me with the required amount."; } }

        public override object Complete { get { return "You've done it! The 50 Silver Serpent Venoms are just what I needed. Your bravery and dedication are commendable. " +
                       "Accept these rewards as a token of my gratitude. May your path be ever illuminated by fortune and glory!"; } }

        public SilverSerpentVenomQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(SilverSerpentVenom), "Silver Serpent Venoms", 50, 0x5722)); // Assuming Silver Serpent Venom item ID is 0x1F7
            AddReward(new BaseReward(typeof(Gold), 6000, "6000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(GroovyBellBottomPants), 1, "Corvian's Alchemical Attire")); // Assuming Corvian's Alchemical Attire is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Venomous Task quest!");
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

    public class CorvianTheAlchemist : MondainQuester
    {
        [Constructable]
        public CorvianTheAlchemist()
            : base("The Enigmatic Alchemist", "Corvian")
        {
        }

        public CorvianTheAlchemist(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(120, 120, 40);

            Body = 0x190; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x203B; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new Robe { Hue = Utility.Random(1, 3000), Name = "Corvian's Alchemical Robe" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new WizardsHat { Hue = Utility.Random(1, 3000), Name = "Corvian's Mystical Hat" });
            AddItem(new LeatherGloves { Hue = Utility.Random(1, 3000), Name = "Corvian's Arcane Gloves" });
            AddItem(new Spellbook { Hue = Utility.Random(1, 3000), Name = "Corvian's Tome of Secrets" }); // Assuming this is a custom item
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Corvian's Potion Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(SilverSerpentVenomQuest)
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
