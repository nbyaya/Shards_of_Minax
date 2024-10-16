using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;

namespace Server.Engines.Quests
{
    public class BlackScaleCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Black Scale Hunt"; } }

        public override object Description
        {
            get
            {
                return "Greetings, brave wanderer! I am Drakos, the Wyrm Sage. Long ago, my clan was cursed by a dark sorcerer, " +
                       "and only the rare BlackScales can lift this malevolent enchantment. These scales are found only on the most " +
                       "treacherous of beasts. I need 50 of them to break the curse. For your efforts, I shall reward you with gold, " +
                       "a Maxxia Scroll of great power, and a unique set of armor imbued with the essence of dragonkind.";
            }
        }

        public override object Refuse { get { return "Very well. If you change your mind, return to me with the BlackScales."; } }

        public override object Uncomplete { get { return "I still need 50 BlackScales to remove the curse. Return to me when you have gathered them."; } }

        public override object Complete { get { return "You have done it! With these 50 BlackScales, I can finally lift the curse from my clan. Accept these rewards as " +
                       "a token of my gratitude. May your adventures be legendary!"; } }

        public BlackScaleCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(BlackScales), "BlackScales", 50, 0x26B4)); // Assuming BlackScale item ID is 0xF6F
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(MasterFencersGorget), 1, "Dragon Scale Gorget")); // Assuming Dragon Scale Armor is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Black Scale Hunt quest!");
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

    public class WyrmSageDrakos : MondainQuester
    {
        [Constructable]
        public WyrmSageDrakos()
            : base("The Wyrm Sage", "Drakos")
        {
        }

        public WyrmSageDrakos(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(120, 120, 50);

            Body = 0x190; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x203E; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new Robe { Hue = Utility.Random(1, 3000), Name = "Drakos' Dragon Robe" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new WideBrimHat { Hue = Utility.Random(1, 3000), Name = "Drakos' Wyrm Hat" });
            AddItem(new PlateArms { Hue = Utility.Random(1, 3000), Name = "Drakos' Dragon Arms" });
            AddItem(new PlateLegs { Hue = Utility.Random(1, 3000), Name = "Drakos' Dragon Legs" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Drakos' Mystical Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(BlackScaleCollectorQuest)
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
