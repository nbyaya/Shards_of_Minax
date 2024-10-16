using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class SlithTongueCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Slith Tongue Collection"; } }

        public override object Description
        {
            get
            {
                return "Greetings, brave adventurer! I am Aeloria, the Enigmatic Seer. " +
                       "A dark prophecy foretells that 50 SlithTongues are needed to break a curse upon this land. " +
                       "These vile tongues hold a sinister power, and their collection is vital to our cause. " +
                       "Your efforts will not go unnoticed. I shall reward you with gold, a rare Maxxia Scroll, and a Tunic imbued with ancient magic.";
            }
        }

        public override object Refuse { get { return "Very well. Should you change your mind, return to me with the SlithTongues."; } }

        public override object Uncomplete { get { return "I still require 50 SlithTongues. Bring them to me so we can lift this dark curse!"; } }

        public override object Complete { get { return "Outstanding! You have retrieved the 50 SlithTongues needed to break the curse. " +
                       "Your bravery is commendable. Accept these rewards as a token of my gratitude. May you continue to shine brightly in your adventures!"; } }

        public SlithTongueCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(SlithTongue), "SlithTongues", 50, 0x5746)); // Assuming SlithTongue item ID is 0x1F2
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(ArtisansTunic), 1, "Aeloria's Enchanted Tunic")); // Assuming Enchanted Robe is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Slith Tongue Collection quest!");
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

    public class EnigmaticSeerAeloria : MondainQuester
    {
        [Constructable]
        public EnigmaticSeerAeloria()
            : base("The Enigmatic Seer", "Aeloria")
        {
        }

        public EnigmaticSeerAeloria(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x191; // Female Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x2045; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new FemalePlateChest { Hue = Utility.Random(1, 3000), Name = "Aeloria's Enchanted Robe" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new WizardsHat { Hue = Utility.Random(1, 3000), Name = "Aeloria's Mystic Hat" });
            AddItem(new LeatherGloves { Hue = Utility.Random(1, 3000), Name = "Aeloria's Sorcerous Gloves" });
            AddItem(new Kilt { Hue = Utility.Random(1, 3000) });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Aeloria's Arcane Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(SlithTongueCollectorQuest)
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
