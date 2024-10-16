using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;

namespace Server.Engines.Quests
{
    public class UnholyBoneCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Unholy Bone Collector"; } }

        public override object Description
        {
            get
            {
                return "Greetings, brave adventurer! I am Maros the Necromancer, keeper of forbidden lore. I seek your aid in collecting 50 Unholy Bones. " +
                       "These bones are vital for my dark rituals and the reawakening of ancient powers. In exchange for your efforts, I shall grant you " +
                       "gold, a rare Maxxia Scroll, and a garb of eldritch design that reflects the dark arts I practice.";
            }
        }

        public override object Refuse { get { return "Very well. If you change your mind, return to me with the Unholy Bones."; } }

        public override object Uncomplete { get { return "I still require 50 Unholy Bones. Bring them to me so I can continue my dark work!"; } }

        public override object Complete { get { return "Ah, excellent! You have brought me the 50 Unholy Bones I requested. Your contribution is invaluable. " +
                       "Accept these rewards as a token of my appreciation. May the shadows guide you on your path!"; } }

        public UnholyBoneCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(UnholyBone), "Unholy Bones", 50, 0x497)); // Assuming UnholyBone item ID is 0x1F4
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(BardOfErinsMuffler), 1, "Dark Sorcerer's Muffler")); // Assuming Dark Sorcerer's Robe is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Unholy Bone Collector quest!");
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

    public class DarkSorcererMaros : MondainQuester
    {
        [Constructable]
        public DarkSorcererMaros()
            : base("The Dark Sorcerer", "Maros")
        {
        }

        public DarkSorcererMaros(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x190; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x2043; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new Robe { Hue = Utility.Random(1, 3000), Name = "Maros' Dark Robe" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new WizardsHat { Hue = Utility.Random(1, 3000), Name = "Maros' Mystical Hat" });
            AddItem(new PlateGloves { Hue = Utility.Random(1, 3000) });
            AddItem(new BlackStaff { Hue = Utility.Random(1, 3000), Name = "Maros' Dark Staff" }); // Assuming this is a custom item
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Maros' Enchanted Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(UnholyBoneCollectorQuest)
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
