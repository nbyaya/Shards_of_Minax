using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class VarinTheTestSubjectQuest : BaseQuest
    {
        public override object Title { get { return "Echoes of Code"; } }

        public override object Description
        {
            get
            {
                return 
                    "*Varin’s eyes flicker, as if searching beyond your face.*\n\n" +
                    "\"Do you ever feel like... we are lines of code? Loops within loops? I was somewhere else. A dungeon, but not like this world—crafted by hands beyond ours. I was meant to be tested, to fight, to respawn, to learn. Then I woke here. I need to go back. Not to escape, but to understand. Will you take me to the place where code bleeds into stone?\"";
            }
        }

        public override object Refuse { get { return "*Varin blinks, lost in thought.* \"I see... perhaps I will find my path another way.\""; } }
        public override object Uncomplete { get { return "*Varin paces nervously.* \"This isn’t the right loop... we must find the pattern again.\""; } }

        public VarinTheTestSubjectQuest() : base()
        {
            AddObjective(new EscortObjective("a Player's Dungeon"));
            AddReward(new BaseReward(typeof(DrapedBlanket), "DrapedBlanket – A cozy item for resting, boosts HP regeneration out of combat."));
        }

        public override void GiveRewards()
        {
            base.GiveRewards();
            Owner.SendMessage("*Varin smiles faintly.* \"Thank you. I think... I am closer now. Rest well, friend, and don’t forget to save your progress.\"", null, 0x59B);
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

    public class VarinTheTestSubjectEscort : BaseEscort
    {
        public override Type[] Quests { get { return new Type[] { typeof(VarinTheTestSubjectQuest) }; } }
        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => false;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBMystic());
        }

        private DateTime m_NextTalkTime;

        [Constructable]
        public VarinTheTestSubjectEscort() : base()
        {
            Name = "Varin";
            Title = "the Test Subject";
            NameHue = 0x22; // Electric blue
        }

		public VarinTheTestSubjectEscort(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(80, 60, 40);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = 1153; // Slightly pale blue-ish skin tone
            HairItemID = 0x203B; // Messy hair
            HairHue = 1157; // Unnatural pale blue
            FacialHairItemID = 0x2041; // Short beard
            FacialHairHue = 1157;
        }

        public override void InitOutfit()
        {
            AddItem(new ElvenShirt() { Hue = 1171, Name = "Digit-Woven Tunic" }); // Iridescent blue
            AddItem(new ElvenPants() { Hue = 1170, Name = "Glitched Trousers" }); // Flickering purple-blue
            AddItem(new Sandals() { Hue = 1150, Name = "Rooted Path Sandals" }); // Dark grey
            AddItem(new Cloak() { Hue = 2101, Name = "Cloak of Recursion" }); // Shimmering silver
            AddItem(new LeatherGloves() { Hue = 1165, Name = "Debug Gauntlets" }); // Light violet
            AddItem(new WizardsHat() { Hue = 1175, Name = "Echo-Crown" }); // Deep starry black

            AddItem(new MysticStaff() { Hue = 1160, Name = "Code-Wrapped Staff" }); // Subtle glowing blue

            Backpack backpack = new Backpack();
            backpack.Hue = 1159;
            backpack.Name = "Inventory Fragment";
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
                        "*Varin mutters.* Does this feel... scripted to you?",
                        "*He looks around.* Someone’s watching. Always watching.",
                        "*Varin taps his staff.* This isn’t just wood... it's a pointer, guiding me.",
                        "*A distant look in his eyes.* I remember respawning... again and again.",
                        "*Varin laughs softly.* They made me to fight. But now, I seek the 'why'.",
                        "*He hums a strange tune.* It’s from... somewhere. Not this world.",
                        "*Varin points ahead.* This path... it's familiar. Like déjà vu coded in."
                    };

                    Say(lines[Utility.Random(lines.Length)]);
                    m_NextTalkTime = DateTime.Now + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 30));
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
