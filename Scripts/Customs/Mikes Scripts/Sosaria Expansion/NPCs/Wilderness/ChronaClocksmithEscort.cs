using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class ChronaClocksmithQuest : BaseQuest
    {
        public override object Title { get { return "The Tides of Time"; } }

        public override object Description
        {
            get
            {
                return 
                    "*Chrona’s eyes flicker with fleeting colors, her voice echoing with strange resonance.*\n\n" +
                    "\"I am Chrona, once a humble clocksmith... now a prisoner of fractured moments. My body lingers here, but my soul drifts. If I do not return to the Time Lord’s Chamber soon, I shall fade from all timelines. Will you help me? The way is not safe, but the currents of time grow wild... and I cannot walk them alone.\"";
            }
        }

        public override object Refuse { get { return "*Chrona’s form flickers.* \"Then I will remain here... until I no longer am.\""; } }
        public override object Uncomplete { get { return "*She clutches her head.* \"The pull... I feel myself slipping! We must hurry!\""; } }

        public ChronaClocksmithQuest() : base()
        {
            AddObjective(new EscortObjective("the Time Lord's Chamber"));
            AddReward(new BaseReward(typeof(WizardKey), "WizardKey – Unlocks arcane portals and chests bound by temporal magic."));
        }

        public override void GiveRewards()
        {
            base.GiveRewards();
            Owner.SendMessage("*Chrona smiles faintly, her form stabilizing.* \"Thank you... you have given me back my place in time. Take this key, forged from the very strands of what once was and may yet be. May it open paths unseen.\"", null, 0x488);
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

    public class ChronaClocksmithEscort : BaseEscort
    {
        public override Type[] Quests { get { return new Type[] { typeof(ChronaClocksmithQuest) }; } }
        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => false;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBTinker(this));
        }

        private DateTime m_NextTalkTime;

        [Constructable]
        public ChronaClocksmithEscort() : base()
        {
            Name = "Chrona";
            Title = "the Clocksmith";
            NameHue = 0x5A6;
        }

		public ChronaClocksmithEscort(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(70, 60, 50);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = 0x47E; // Pale, almost luminescent skin tone
            HairItemID = 0x203B; // Short hair
            HairHue = 1153; // Silvery-blue hair
        }

        public override void InitOutfit()
        {
            AddItem(new FancyShirt() { Hue = 1171, Name = "Chrona's Timewoven Tunic" }); // Iridescent silver
            AddItem(new LongPants() { Hue = 1170, Name = "Chrono-Threaded Leggings" }); // Deep midnight blue
            AddItem(new ThighBoots() { Hue = 1157, Name = "Boots of Fleeting Steps" }); // Pale grey
            AddItem(new BodySash() { Hue = 1154, Name = "Temporal Sash" }); // Light blue, shifting hues
            AddItem(new SilverBracelet() { Hue = 1152, Name = "Chrona's Mechanist Band" }); // Custom clockwork item, golden hue
            AddItem(new Cloak() { Hue = 1173, Name = "Cloak of Lost Hours" }); // Shimmers with a violet sheen
            AddItem(new FeatheredHat() { Hue = 1175, Name = "Hat of Minute Whispers" }); // A fine hat with silver accents

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Temporal Toolkit";
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
                        "*Chrona glances at her hands, watching them shimmer.* Time... it's slipping again.",
                        "*She frowns.* Do you hear it? The ticking? It's out of step.",
                        "*Chrona murmurs.* If we fail... I may never have existed.",
                        "*Her voice wavers.* They warned me... never tamper with the gears of destiny.",
                        "*She looks ahead, determined.* I won't fade, not yet. Let's move quickly.",
                        "*Chrona breathes heavily.* The closer we get, the stronger I feel.",
                        "*She adjusts her sash.* This cloak has seen centuries... I must not let it see my end."
                    };

                    Say(lines[Utility.Random(lines.Length)]);
                    m_NextTalkTime = DateTime.Now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
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
