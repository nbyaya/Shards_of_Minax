using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class KaelOfTheMistQuest : BaseQuest
    {
        public override object Title { get { return "Through the Mists"; } }

        public override object Description
        {
            get
            {
                return
                    "*Kael’s gaze is distant, like he’s listening to a world you cannot hear.*\n\n" +
                    "I am Kael... of the Mist. My time here is not meant to last. There is a portal, hidden in folds of fog, that opens only to those who know its song. I must return before the stars shift again. Will you guide me? The mists confuse my path, but with you, I might yet find the way home.";
            }
        }

        public override object Refuse { get { return "*Kael’s form shimmers faintly.* Then the mists take me... and the stars will mourn."; } }
        public override object Uncomplete { get { return "*Kael closes his eyes.* No... the portal drifts... we must not stop."; } }

        public KaelOfTheMistQuest() : base()
        {
            AddObjective(new EscortObjective("Unknown Land"));
            AddReward(new BaseReward(typeof(WizardsHat), "StarlightWizardHat – Amplifies magic during nighttime or near star-aligned places."));
        }

        public override void GiveRewards()
        {
            base.GiveRewards();
            Owner.SendMessage("*Kael bows, his form beginning to fade.* Thank you, guide. Take this, woven from the light of distant stars. May it guide you when shadows fall.", null, 0x59B);
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

    public class KaelOfTheMistEscort : BaseEscort
    {
        public override Type[] Quests { get { return new Type[] { typeof(KaelOfTheMistQuest) }; } }
        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => false;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBMystic());
        }

        private DateTime m_NextTalkTime;

        [Constructable]
        public KaelOfTheMistEscort() : base()
        {
            Name = "Kael of the Mist";
            Title = "the Wayward Mystic";
            NameHue = 1153; // Subtle ethereal hue
        }

		public KaelOfTheMistEscort(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(70, 60, 50);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = 1150; // Pale, moonlit tone
            HairItemID = 0x2049; // Medium long
            HairHue = 1151; // Silvery-blue
            FacialHairItemID = 0x203F; // Van Dyke
            FacialHairHue = 1151;
        }

        public override void InitOutfit()
        {
            AddItem(new HoodedShroudOfShadows() { Hue = 1154, Name = "Mistborne Shroud" }); // Soft misty blue
            AddItem(new Sandals() { Hue = 1153, Name = "Wanderer's Steps" }); // Ethereal pale gray
            AddItem(new BodySash() { Hue = 1175, Name = "Sash of Lost Stars" }); // Starry navy
            AddItem(new WizardsHat() { Hue = 1153, Name = "Starlit Hood" }); // Matches shroud
            AddItem(new GnarledStaff() { Hue = 1150, Name = "Staff of Veiled Paths" }); // Pale, weathered wood

            Backpack backpack = new Backpack();
            backpack.Hue = 1151;
            backpack.Name = "Misty Satchel";
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
                        "*Kael’s voice echoes softly.* The mists are alive... they test us.",
                        "*He glances upward.* The stars... they shift, but still remember me.",
                        "*A distant hum surrounds him.* Do you hear it? The song of the veil...",
                        "*Kael pauses.* Not all paths are walked with feet... some with memory.",
                        "*His eyes grow distant.* Beyond the veil, the skies are different... clearer.",
                        "*Kael hums a foreign melody.* This song... it opens doors unseen.",
                        "*He whispers.* They call me... but the portal will not wait forever."
                    };

                    Say(lines[Utility.Random(lines.Length)]);
                    m_NextTalkTime = DateTime.Now + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 40));
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
