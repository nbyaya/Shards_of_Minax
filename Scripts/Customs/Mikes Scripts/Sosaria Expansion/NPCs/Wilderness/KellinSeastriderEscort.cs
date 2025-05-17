using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class KellinSeastriderQuest : BaseQuest
    {
        public override object Title { get { return "The Tides That Bind"; } }

        public override object Description
        {
            get
            {
                return
                    "*Kellin’s voice is like the whisper of waves on a midnight shore.*\n\n" +
                    "The sea... she cursed me, aye. Till I reach the warmth of the Fisherman’s Inn before the tides turn again, I cannot rest. Each dawn, I rise anew, soaked in the salt of dreams, my bones weary of walking. Will you guide me, stranger? Help me find peace before the tide pulls me under once more?";
            }
        }

        public override object Refuse { get { return "*Kellin’s eyes dim like a fading star.* Then the tide claims me again... until tomorrow."; } }
        public override object Uncomplete { get { return "*The sea breeze sharpens.* The tide turns soon... we must hurry!"; } }

        public KellinSeastriderQuest() : base()
        {
            AddObjective(new EscortObjective("Fisherman’s Inn"));
            AddReward(new BaseReward(typeof(GeomancersStaff), "Geomancer’s Staff – Enhances earth magic and tremor abilities."));
        }

        public override void GiveRewards()
        {
            base.GiveRewards();
            Owner.SendMessage("*Kellin breathes deeply, the scent of salt lifting from his clothes.* The tide is kind today. I am free, at last. Take this, earth-bound friend. May it ground you when the world shifts beneath your feet.", null, 0x59B);
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

    public class KellinSeastriderEscort : BaseEscort
    {
        public override Type[] Quests { get { return new Type[] { typeof(KellinSeastriderQuest) }; } }
        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => false;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBFisherman());
        }

        private DateTime m_NextTalkTime;

        [Constructable]
        public KellinSeastriderEscort() : base()
        {
            Name = "Kellin Seastrider";
            Title = "the Cursed Sailor";
            NameHue = 0x480;
        }

		public KellinSeastriderEscort(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(85, 60, 40);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = 1001; // Weathered tan
            HairItemID = 0x2044; // Medium long hair
            HairHue = 1150; // Sea-salt silver
            FacialHairItemID = 0x204B; // Full beard
            FacialHairHue = 1150;
        }

        public override void InitOutfit()
        {
            AddItem(new FancyShirt() { Hue = 1153, Name = "Tideworn Shirt" }); // Deep sea blue
            AddItem(new LongPants() { Hue = 1109, Name = "Salt-Stained Trousers" }); // Grey-brown
            AddItem(new Boots() { Hue = 1175, Name = "Wave-Treaders" }); // Washed out brown
            AddItem(new Cloak() { Hue = 1266, Name = "Cloak of the Drowned" }); // Pale blue
            AddItem(new TricorneHat() { Hue = 1102, Name = "Seastrider’s Tricorne" }); // Faded navy
            AddItem(new BodySash() { Hue = 1154, Name = "Sash of Storms" }); // Storm grey

            AddItem(new FishermansTrident() { Hue = 2101, Name = "Kellin’s Driftwood Trident" }); // Weathered wood

            Backpack backpack = new Backpack();
            backpack.Hue = 1109;
            backpack.Name = "Barnacle-Crusted Pack";
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
                        "*Kellin glances at the horizon.* She’s out there, waiting... the sea never forgets.",
                        "*He tightens his grip on the trident.* The tide turns soon... I feel it in my bones.",
                        "*Kellin hums a sailor’s dirge.* We sang this when the storm took them... now it sings to me.",
                        "*A shadow passes over his face.* Do you hear it? The waves, they whisper curses.",
                        "*He wipes salt from his brow.* If I fail today, I wake again... always wet, always cold.",
                        "*Kellin speaks softly.* They said no man can outrun the sea... but I must try."
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
