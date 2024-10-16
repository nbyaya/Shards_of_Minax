using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class RaptorTeethCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Raptor Teeth Collection"; } }

        public override object Description
        {
            get
            {
                return "Greetings, valiant adventurer! I am Varok, the Raptor Hunter. A dire threat looms over our lands as " +
                       "a monstrous raptor pack has been terrorizing our village. I need you to collect 50 Raptor Teeth from " +
                       "these fearsome beasts. They are key to crafting a powerful talisman to protect us. Your bravery will " +
                       "not go unrewarded; you will receive gold, a rare Maxxia Scroll, and a one-of-a-kind Raptor Hunter's Belt " +
                       "as a token of my gratitude.";
            }
        }

        public override object Refuse { get { return "Very well. Should you change your mind, return to me with the Raptor Teeth."; } }

        public override object Uncomplete { get { return "I still need 50 Raptor Teeth to craft the talisman. Please bring them to me as soon as you can!"; } }

        public override object Complete { get { return "Marvelous work! You have brought me the 50 Raptor Teeth I required. Your courage is exemplary. " +
                       "Please accept these rewards as a token of my appreciation. May your future adventures be just as successful!"; } }

        public RaptorTeethCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(RaptorTeeth), "Raptor Teeth", 50, 0x5747)); // Assuming Raptor Teeth item ID is 0x1C2
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(ChampionsChampionshipBelt), 1, "Raptor Hunter's Belt")); // Assuming Raptor Hunter's Cloak is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Raptor Teeth Collection quest!");
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

    public class RaptorHunterVarok : MondainQuester
    {
        [Constructable]
        public RaptorHunterVarok()
            : base("The Raptor Hunter", "Varok")
        {
        }

        public RaptorHunterVarok(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x190; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x2040; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new ChainChest { Hue = Utility.Random(1, 3000), Name = "Varok's Battle Armor" });
            AddItem(new LeatherLegs { Hue = Utility.Random(1, 3000) });
            AddItem(new NorseHelm { Hue = Utility.Random(1, 3000), Name = "Varok's Raptor Helm" });
            AddItem(new PlateGloves { Hue = Utility.Random(1, 3000), Name = "Varok's Gauntlets" });
            AddItem(new Boots { Hue = Utility.Random(1, 3000) });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Varok's Adventurer's Pack";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(RaptorTeethCollectorQuest)
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
