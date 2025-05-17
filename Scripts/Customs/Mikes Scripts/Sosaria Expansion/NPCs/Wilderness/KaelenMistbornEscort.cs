using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class KaelenMistbornQuest : BaseQuest
    {
        public override object Title { get { return "Through the Veil"; } }

        public override object Description
        {
            get
            {
                return
                    "*Kaelen’s form flickers like a candle flame, his voice soft yet profound.*\n\n" +
                    "\"I am Kaelen Mistborn, philosopher of the unseen, wanderer of veiled paths. My time in this realm is nearing its end, yet the Etherial Plane eludes me. Without guidance, I will fade... scattered between worlds. Will you walk beside me, through shadow and mist, and help me return before my essence is lost?\"";
            }
        }

        public override object Refuse { get { return "*Kaelen’s eyes dim.* \"Then may I not burden you further, stranger.\""; } }
        public override object Uncomplete { get { return "*Kaelen’s voice echoes faintly.* \"The veil stirs… we must press on.\""; } }

        public KaelenMistbornQuest() : base()
        {
            AddObjective(new EscortObjective("Etherial Plane"));
            AddReward(new BaseReward(typeof(CompositeBow), "NebulaBow – Fires spectral arrows dealing extra damage to incorporeal foes."));
        }

        public override void GiveRewards()
        {
            base.GiveRewards();
            Owner.SendMessage("*Kaelen's form begins to shimmer and dissolve.* \"Gratitude... beyond words. The stars call, and I shall answer. May this bow aid you where shadows linger.\"", null, 0x480);
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

    public class KaelenMistbornEscort : BaseEscort
    {
        public override Type[] Quests { get { return new Type[] { typeof(KaelenMistbornQuest) }; } }
        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => false;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBMystic());
        }

        private DateTime m_NextTalkTime;

        [Constructable]
        public KaelenMistbornEscort() : base()
        {
            Name = "Kaelen Mistborn";
            Title = "the Veiled Philosopher";
            NameHue = 0x47E; // Pale, spectral blue
        }

		public KaelenMistbornEscort(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(70, 60, 40);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = 1150; // Ethereal pale
            HairItemID = 0x2049; // Short hair
            HairHue = 1153; // Moonlight silver
            FacialHairItemID = 0x2041; // Beard
            FacialHairHue = 1153;
        }

        public override void InitOutfit()
        {
            AddItem(new HoodedShroudOfShadows() { Hue = 1154, Name = "Shroud of the Boundless" }); // Misty violet hue
            AddItem(new Cloak() { Hue = 1153, Name = "Veil of the Forgotten Stars" }); // Faint silver-blue
            AddItem(new Sandals() { Hue = 1170, Name = "Steps of the Voidwalker" }); // Deep midnight hue
            AddItem(new BodySash() { Hue = 1165, Name = "Sash of Astral Threads" }); // Soft, glowing teal
            AddItem(new LeatherGloves() { Hue = 1150, Name = "Grips of the Fading" }); // Pale, semi-transparent

            Backpack backpack = new Backpack();
            backpack.Hue = 1153;
            backpack.Name = "Philosopher's Reliquary";
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
                        "*Kaelen glances skyward.* Can you feel it? The stars tug at my soul.",
                        "*His form shimmers faintly.* Each step draws me closer… or is it further?",
                        "*Kaelen speaks softly.* What is the self, if not a whisper caught between worlds?",
                        "*He hums a haunting tune.* The Etherial Plane sings… I must answer.",
                        "*A flicker passes over him.* The longer I stay, the less I remain.",
                        "*Kaelen touches the air.* The veil thins, but will it open in time?",
                        "*His voice distant.* When I return… what shall I remember of this place?"
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
