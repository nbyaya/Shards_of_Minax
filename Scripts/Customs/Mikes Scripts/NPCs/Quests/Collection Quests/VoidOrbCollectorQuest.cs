using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class VoidOrbCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Void Orb Conundrum"; } }

        public override object Description
        {
            get
            {
                return "Greetings, valiant adventurer! I am Xelorian, the Void Sage. The Void Orbs hold the key to unraveling ancient cosmic mysteries, " +
                       "but their essence has become dangerously fragmented. I need you to collect 50 Void Orbs for me. Your bravery and diligence will be rewarded " +
                       "with a generous sum of gold, a rare Maxxia Scroll, and a mystical Void Sage's Pants that reflects the very essence of the void.";
            }
        }

        public override object Refuse { get { return "I understand. Should you choose to accept this challenge later, return to me with the Void Orbs."; } }

        public override object Uncomplete { get { return "I still require 50 Void Orbs. Bring them to me so we can delve deeper into the mysteries of the void!"; } }

        public override object Complete { get { return "Wonderful! You have gathered the 50 Void Orbs I requested. The power of the void now pulses stronger, thanks to you. " +
                       "Accept these rewards as a mark of your achievement. May the void guide your path in future endeavors!"; } }

        public VoidOrbCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(VoidOrb), "Void Orbs", 50, 0x573E)); // Assuming Void Orb item ID is 0x1F3
            AddReward(new BaseReward(typeof(Gold), 10000, "10000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(BaggyHipHopPants), 1, "Void Sage's Pants")); // Assuming Void Sage's Robe is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Void Orb Conundrum quest!");
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

    public class VoidSageXelorian : MondainQuester
    {
        [Constructable]
        public VoidSageXelorian()
            : base("The Void Sage", "Xelorian")
        {
        }

        public VoidSageXelorian(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x190; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x2041; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new ChainChest { Hue = Utility.Random(1, 3000), Name = "Xelorian's Void Robe" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new WizardsHat { Hue = Utility.Random(1, 3000), Name = "Xelorian's Arcane Hat" });
            AddItem(new PlateGloves { Hue = Utility.Random(1, 3000), Name = "Xelorian's Void Gloves" });
            AddItem(new Cloak { Hue = Utility.Random(1, 3000), Name = "Xelorian's Mystical Cloak" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Xelorian's Enigmatic Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(VoidOrbCollectorQuest)
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
