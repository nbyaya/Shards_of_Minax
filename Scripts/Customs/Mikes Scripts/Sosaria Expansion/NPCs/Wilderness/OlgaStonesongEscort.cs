using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class OlgaStonesongQuest : BaseQuest
    {
        public override object Title { get { return "The Song of Stone and Sky"; } }

        public override object Description
        {
            get
            {
                return 
                    "*Olga’s eyes gleam like polished granite, her voice steady and resonant.*\n\n" +
                    "I am Olga Stonesong, bard of the high crags. Within me stirs a melody older than mountains—a song that awakens the guardians of stone. But only in my home, atop Mountain Crest, can the full song be sung. The road is perilous, and foes stir that would see the guardians sleep forever. Will you see me home, and hear the mountains rise in song?";
            }
        }

        public override object Refuse { get { return "*Olga’s voice softens, a somber note lingering.* Then may the stones forget my name, and silence reign."; } }
        public override object Uncomplete { get { return "*Olga hums softly, the song unfinished.* The guardians wait, we cannot tarry."; } }

        public OlgaStonesongQuest() : base()
        {
            AddObjective(new EscortObjective("the Mountain Crest Village"));
            AddReward(new BaseReward(typeof(CherubsBlade), "CherubsBlade – A light, holy weapon with a chance to heal its wielder."));
        }

        public override void GiveRewards()
        {
            base.GiveRewards();
            Owner.SendMessage("*As the last notes of Olga’s song echo across the peaks, a warmth fills your hand.* Take this, forged in song and stone. May it guard you, as you have guarded me.", null, 0x59B);
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

    public class OlgaStonesongEscort : BaseEscort
    {
        public override Type[] Quests { get { return new Type[] { typeof(OlgaStonesongQuest) }; } }
        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBBard());
        }

        private DateTime m_NextTalkTime;

        [Constructable]
        public OlgaStonesongEscort() : base()
        {
            Name = "Olga Stonesong";
            Title = "the Mountain Bard";
            NameHue = 0x59C; // Stone grey with a hint of warmth
        }

		public OlgaStonesongEscort(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(80, 60, 40);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = 1055; // Slightly ruddy mountain skin tone
            HairItemID = 0x203B; // Braided hair
            HairHue = 1149; // Earthy auburn
        }

        public override void InitOutfit()
        {
            AddItem(new FancyShirt() { Hue = 2405, Name = "Skyweave Blouse" }); // Misty blue
            AddItem(new FancyKilt() { Hue = 2425, Name = "Stonefold Kilt" }); // Granite grey
            AddItem(new Cloak() { Hue = 2212, Name = "Echoing Cloak" }); // Deep mountain green
            AddItem(new Boots() { Hue = 2101, Name = "Treader's Boots" }); // Weathered leather
            AddItem(new FeatheredHat() { Hue = 1141, Name = "Bard's Crest" }); // Steel blue with a grey feather
            AddItem(new BodySash() { Hue = 1154, Name = "Harmonic Sash" }); // Indigo

            AddItem(new ResonantHarp() { Hue = 2411, Name = "Stonesong Harp" }); // Polished stone tones

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Ballad Satchel";
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
                        "*Olga hums a low, powerful tune.* Do you hear it? The earth stirs at the song’s touch.",
                        "*She touches the harp gently.* Each note carries a memory, each chord a promise.",
                        "*Olga gazes to the mountains.* They sleep now, but soon they will rise.",
                        "*Her voice lifts.* Stone and sky, awaken with me!",
                        "*Olga’s eyes close briefly.* The guardians dream of ancient days... I must sing them awake.",
                        "*She smiles.* The path is hard, but I’ve walked with harder songs in my heart.",
                        "*The harp resonates softly.* This melody... it will shape the stone once more."
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
