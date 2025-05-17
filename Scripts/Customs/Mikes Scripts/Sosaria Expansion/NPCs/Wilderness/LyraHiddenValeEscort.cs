using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class LyraHiddenValeQuest : BaseQuest
    {
        public override object Title { get { return "Return to Ambrosia"; } }

        public override object Description
        {
            get
            {
                return
                    "*Lyra’s eyes are vivid green, her voice soft yet urgent.*\n\n" +
                    "\"I am Lyra, once of Ambrosia, now a shadow fleeing those who would steal my memories. They fear what I know... of healing, of the Void, of what sleeps beneath the waves. I must return to Ambrosia before it is lost forever. Will you walk with me, through danger and doubt, to see me home?\"";
            }
        }

        public override object Refuse { get { return "*Lyra lowers her gaze.* \"Then may fate yet guide me alone, though I fear I will not reach the shore.\""; } }
        public override object Uncomplete { get { return "*Lyra breathes heavily.* \"They are close... we must not falter now.\""; } }

        public LyraHiddenValeQuest() : base()
        {
            AddObjective(new EscortObjective("Ambrosia"));
            AddReward(new BaseReward(typeof(GreenDragonCrescentBlade), "GreenDragonCrescentBlade – A powerful blade blessed with draconic energy."));
        }

        public override void GiveRewards()
        {
            base.GiveRewards();
            Owner.SendMessage("*Lyra smiles faintly, a glow about her hands as she blesses your weapon.* \"You have saved more than me today. Take this, forged by ancient wyrms. Let it protect you, as you have protected me.\"", null, 0x59B);
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

    public class LyraHiddenValeEscort : BaseEscort
    {
        public override Type[] Quests { get { return new Type[] { typeof(LyraHiddenValeQuest) }; } }
        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => false;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBHealer());
        }

        private DateTime m_NextTalkTime;

        [Constructable]
        public LyraHiddenValeEscort() : base()
        {
            Name = "Lyra";
            Title = "of the Hidden Vale";
            NameHue = 0x48D;
        }

		public LyraHiddenValeEscort(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(50, 60, 70);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = 1153; // Pale, moonlit tone
            HairItemID = 0x203B; // Long hair
            HairHue = 1157; // Light, silver-blue
        }

        public override void InitOutfit()
        {
            AddItem(new FancyDress() { Hue = 1266, Name = "Mistgown of Ambrosia" }); // Seafoam green
            AddItem(new Cloak() { Hue = 1272, Name = "Veil of the Hidden Vale" }); // Iridescent teal
            AddItem(new Sandals() { Hue = 1270, Name = "Pathwalker’s Sandals" }); // Ocean blue
            AddItem(new BodySash() { Hue = 1260, Name = "Sash of Healing Tides" }); // Soft pearl white
            AddItem(new Circlet() { Hue = 1150, Name = "Memorybinder Circlet" }); // Dark silver

            Backpack backpack = new Backpack();
            backpack.Hue = 1268; // Emerald green
            backpack.Name = "Herbalist’s Pack";
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
                        "*Lyra clutches her circlet.* They chase the truth I carry... we must hurry.",
                        "*Her eyes scan the horizon.* The sea calls me home... Ambrosia waits.",
                        "*She whispers softly.* My memories... they tried to steal them. But Ambrosia will restore what’s lost.",
                        "*Lyra hums a melody.* This song was taught to me by the waters. It keeps me calm... it keeps me whole.",
                        "*Her hand glows faintly.* I can mend wounds... but not if I fall to them.",
                        "*Lyra looks to you.* Thank you for walking with me. I was never meant to walk alone.",
                        "*A shadow passes over her face.* If I forget who I am... remind me of Ambrosia, and why I returned."
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
