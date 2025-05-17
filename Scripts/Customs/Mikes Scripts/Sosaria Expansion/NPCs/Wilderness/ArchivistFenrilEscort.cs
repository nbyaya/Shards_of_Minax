using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class FenrilScrollQuest : BaseQuest
    {
        public override object Title { get { return "The Cursed Scroll"; } }

        public override object Description
        {
            get
            {
                return 
                    "Please, traveler, I am Archivist Fenril, once of Britain’s great library. I left something... no, *the* thing, in the Old Ruins. A scroll, ancient and frayed, that speaks of a curse powerful enough to end this world. I must retrieve it before it falls into the wrong hands—or worse, awakens! Will you escort me there, through danger and shadow?";
            }
        }

        public override object Refuse { get { return "You don’t understand! If that scroll is read aloud, the curse may begin! I must return, even if alone..."; } }
        public override object Uncomplete { get { return "The ruins call to me still, friend. That scroll must be found... before it’s too late."; } }

        public FenrilScrollQuest() : base()
        {
            AddObjective(new EscortObjective("some Old Ruins"));
            AddReward(new BaseReward(typeof(PineResin), "PineResin"));
        }

        public override void GiveRewards()
        {
            base.GiveRewards();
            Owner.SendMessage("Thank you, brave soul. The scroll is safe once more, and Sosaria breathes easier... for now. May this PineResin serve you well in your craft, as a token of my undying gratitude.", null, 0x59B);
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

    public class ArchivistFenrilEscort : BaseEscort
    {
        public override Type[] Quests { get { return new Type[] { typeof(FenrilScrollQuest) }; } }
        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => false;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBScribe(this));
        }

        private DateTime m_NextTalkTime;

        [Constructable]
        public ArchivistFenrilEscort() : base()
        {
            Name = "Fenril";
            Title = "the Archivist";
            NameHue = 0x5A3;
        }

		public ArchivistFenrilEscort(Serial serial) : base(serial) { }		

        public override void InitBody()
        {
            InitStats(40, 45, 25);
            Female = false;
            CantWalk = false;
            Race = Race.Human;
            Hue = 0x83F8;
            HairItemID = 0x2049;
            HairHue = 0x455;
            FacialHairItemID = 0x203B;
            FacialHairHue = 0x455;
        }

        public override void InitOutfit()
        {
            AddItem(new Robe() { Hue = 0x482 }); // Deep blue scholar robe
            AddItem(new Sandals() { Hue = 0x47F }); // Midnight sandals
            AddItem(new WizardsHat() { Hue = 0x47E }); // Matching wizard hat
            AddItem(new BodySash() { Hue = 0x59B }); // Sash of the Archivist
            AddItem(new Spellbook() { Hue = 0x488 }); // Bound with notes
        }

        public override void OnThink()
        {
            base.OnThink();

            if (DateTime.Now >= m_NextTalkTime && this.Controlled)
            {
                if (Utility.RandomDouble() < 0.1)
                {
                    string[] lines = new string[]
                    {
                        "*Fenril clutches his satchel tightly* 'The scroll... I can almost hear it calling.'",
                        "'I have studied curses all my life, but none like this. It is older than Sosaria itself.'",
                        "'If we fail, friend, know that I am grateful for your courage.'",
                        "*He peers ahead anxiously* 'The ruins are near... I hope we are not too late.'",
                        "'Beware the glyphs on the stones... they stir when the scroll is close.'",
                        "'No matter what happens, do not read from the scroll. Promise me!'"
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
