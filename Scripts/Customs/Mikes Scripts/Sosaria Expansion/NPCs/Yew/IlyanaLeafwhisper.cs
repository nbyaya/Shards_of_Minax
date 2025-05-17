using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class MazeOfRotQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Maze of Rot"; } }

        public override object Description
        {
            get
            {
                return
                    "You find yourself beneath the canopy of Yew, standing before *Ilyana Leafwhisper*, Druid of the Old Grove.\n\n" +
                    "Her robes ripple like leaves in a breeze only she can feel, and her eyes, a vibrant emerald, are shadowed with concern.\n\n" +
                    "“The forest speaks in groans now. It weeps beneath rot and corruption that spreads from **Catastrophe**.”\n\n" +
                    "“A creature—no, an abomination—called the **PutridMinotaur** has emerged. It wears a necklace of the Sylvan Pact, once a symbol of harmony, now dripping with decay.”\n\n" +
                    "“I sealed that pact long ago, binding my soul to the forest’s health. Now, I feel the rot claw at my spirit.”\n\n" +
                    "“Slay the beast. Reclaim the necklace. Let the balance be restored—or Yew itself may wither.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then the roots will continue to die, and with them, the hearts of those who call Yew home.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "The forest remains restless, and I feel the poison grow within me. The PutridMinotaur still defiles our sacred pact.";
            }
        }

        public override object Complete
        {
            get
            {
                return "The beast is slain? The necklace… yes, I can feel its pain, but also its hope.\n\n" +
                       "You have done what I could not—stood against the rot and triumphed.\n\n" +
                       "Take this: the **Necklace of the Sylvan Pact**. May it bind you now, as it once bound me, to the living heart of the forest.";
            }
        }

        public MazeOfRotQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(PutridMinotaur), "PutridMinotaur", 1));
            AddReward(new BaseReward(typeof(NecklaceOfTheSylvanPact), 1, "Necklace of the Sylvan Pact"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x59, "You have restored balance to the Grove.");
            Owner.PlaySound(CompleteSound);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class IlyanaLeafwhisper : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(MazeOfRotQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;
        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBLeatherArmor());
        }

        [Constructable]
        public IlyanaLeafwhisper()
            : base("the Druid of Yew", "Ilyana Leafwhisper")
        {
        }

        public IlyanaLeafwhisper(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(90, 100, 40);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = 1020; // Soft forest tan
            HairItemID = 0x203C; // Long Hair
            HairHue = 1153; // Deep emerald green
        }

        public override void InitOutfit()
        {
            AddItem(new FlowerGarland() { Hue = 1260, Name = "Crown of the Grove" });
            AddItem(new FancyShirt() { Hue = 1423, Name = "Whispering Bark Tunic" });
            AddItem(new Skirt() { Hue = 1260, Name = "Verdant Flow Skirt" });
            AddItem(new Cloak() { Hue = 1266, Name = "Veil of Moss" });
            AddItem(new Sandals() { Hue = 1207, Name = "Rootwoven Sandals" });
            AddItem(new BodySash() { Hue = 1270, Name = "Druidic Sash of Harmony" });

            Backpack backpack = new Backpack();
            backpack.Hue = 2001;
            backpack.Name = "Herbalist’s Satchel";
            AddItem(backpack);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
