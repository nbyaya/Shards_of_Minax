using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class SirHalrickQuest : BaseQuest
    {
        public override object Title { get { return "Ashes to Honor"; } }

        public override object Description
        {
            get
            {
                return 
                    "*Sir Halrick stands tall, though a shadow lingers in his eyes.*\n\n" +
                    "Once, I was a knight of the Ashen Order—sworn to protect, to honor, to serve. I failed. The fortress where I stood my last stand still holds my sword, Ashen Vow. I must reclaim it, not for glory, but for redemption. But the dead of my Order do not rest... they will not suffer me to return alone. Will you walk with me through the ashes of my past?";
            }
        }

        public override object Refuse { get { return "*Halrick’s voice drops.* Then I remain unworthy, bound by failure."; } }
        public override object Uncomplete { get { return "*The wind stirs his cloak.* The dead grow restless. We must go on."; } }

        public SirHalrickQuest() : base()
        {
            AddObjective(new EscortObjective("an Old Fortress"));
            AddReward(new BaseReward(typeof(VialWarden), "VialWarden – Reduces potion cooldowns and improves potion effects."));
        }

        public override void GiveRewards()
        {
            base.GiveRewards();
            Owner.SendMessage("*Halrick kneels, clutching his reclaimed sword.* You have my eternal gratitude. Take this—crafted for those who survive their trials. May it serve you as you have served me.", null, 0x59B);
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

    public class SirHalrickEscort : BaseEscort
    {
        public override Type[] Quests { get { return new Type[] { typeof(SirHalrickQuest) }; } }
        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => false;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBKeeperOfChivalry());
        }

        private DateTime m_NextTalkTime;

        [Constructable]
        public SirHalrickEscort() : base()
        {
            Name = "Sir Halrick";
            Title = "of the Ashen Order";
            NameHue = 1153; // Subtle gray-blue
        }

		public SirHalrickEscort(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(80, 80, 70);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = 1023; // Weathered complexion
            HairItemID = 0x2047; // Short hair
            HairHue = 1108; // Ash grey
            FacialHairItemID = 0x203F; // Short beard
            FacialHairHue = 1108;
        }

        public override void InitOutfit()
        {
            AddItem(new PlateChest() { Hue = 1150, Name = "Ashen Order Chestplate" }); // Dark steel
            AddItem(new PlateArms() { Hue = 1150, Name = "Ashen Order Vambraces" }); // Dark steel
            AddItem(new PlateGloves() { Hue = 1150, Name = "Gauntlets of Remorse" }); // Dark steel
            AddItem(new PlateLegs() { Hue = 1150, Name = "Ashen Order Greaves" }); // Dark steel
            AddItem(new CloseHelm() { Hue = 1150, Name = "Helm of the Fallen Vow" }); // Dark steel
            AddItem(new Cloak() { Hue = 1153, Name = "Cloak of Ash and Embers" }); // Ash-gray

            AddItem(new Broadsword() { Hue = 1109, Name = "Knight's Replica" }); // Placeholder for Ashen Vow

            Backpack backpack = new Backpack();
            backpack.Hue = 1109;
            backpack.Name = "Worn Knight's Pack";
            AddItem(backpack);
        }

        public override void OnThink()
        {
            base.OnThink();

            if (DateTime.Now >= m_NextTalkTime && this.Controlled)
            {
                if (Utility.RandomDouble() < 0.15)
                {
                    string[] lines = new string[]
                    {
                        "*Halrick glances ahead.* The fortress lies yonder... I can almost feel the weight of my sword.",
                        "*His hand tightens around his hilt.* The Order’s dead do not rest. Nor shall I, until this is done.",
                        "*Halrick speaks low.* Ashen Vow waits for me, as do my sins.",
                        "*He surveys the road.* Once, I walked these paths in honor. Now, in shame.",
                        "*Halrick sighs.* When the fortress fell, so did my oath. I must reclaim it.",
                        "*A cold wind stirs.* The voices... they do not forgive.",
                        "*Halrick murmurs.* Stay close. They may not take kindly to the living, nor to me."
                    };

                    Say(lines[Utility.Random(lines.Length)]);
                    m_NextTalkTime = DateTime.Now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 35));
                }
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
            writer.Write(m_NextTalkTime);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_NextTalkTime = reader.ReadDateTime();
        }
    }
}
