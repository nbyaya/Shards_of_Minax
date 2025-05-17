using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class LiraSeabrightQuest : BaseQuest
    {
        public override object Title { get { return "Song Before the Storm"; } }

        public override object Description
        {
            get
            {
                return 
                    "*Lira’s sea-glass eyes glimmer with worry as she smooths her storm-blue cloak.*\n\n" +
                    "The tides listen, stranger, and they grow restless. I am Lira Seabright, a voice for the sea, and I must return to Renika Inn before the storm claims the coast. My song holds the waves at bay, but without it, the harbor will drown. Will you escort me there? The winds already howl, and I fear what stirs beneath.";
            }
        }

        public override object Refuse { get { return "*Lira's voice trembles like a rising tide.* Then may the sea show mercy... if it remembers how."; } }
        public override object Uncomplete { get { return "*Salt-laden winds pick up.* The storm quickens—we must make haste!"; } }

        public LiraSeabrightQuest() : base()
        {
            AddObjective(new EscortObjective("Renika Inn"));
            AddReward(new BaseReward(typeof(FrostwardensWoodenShield), "FrostwardensWoodenShield – A sturdy shield offering frost resistance and calm under pressure."));
        }

        public override void GiveRewards()
        {
            base.GiveRewards();
            Owner.SendMessage("*Lira places a hand over her heart, singing softly.* You’ve calmed more than the tides this day. Take this, from the Frostwardens of old—it will steady your hand when the chill bites deep.", null, 0x59B);
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

    public class LiraSeabrightEscort : BaseEscort
    {
        public override Type[] Quests { get { return new Type[] { typeof(LiraSeabrightQuest) }; } }
        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => false;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBInnKeeper());
        }

        private DateTime m_NextTalkTime;

        [Constructable]
        public LiraSeabrightEscort() : base()
        {
            Name = "Lira Seabright";
            Title = "the Tide-Singer";
            NameHue = 0x480;
        }

		public LiraSeabrightEscort(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 60, 30);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = 1050; // Fair with a sea-salt tone
            HairItemID = 0x203B; // Wavy Long Hair
            HairHue = 1153; // Deep ocean blue
        }

        public override void InitOutfit()
        {
            AddItem(new FancyDress() { Hue = 1266, Name = "Seabright's Flowing Gown" }); // Seafoam green
            AddItem(new Cloak() { Hue = 1172, Name = "Stormcaller's Cloak" }); // Deep storm-blue
            AddItem(new Sandals() { Hue = 1109, Name = "Wave-Worn Sandals" }); // Driftwood grey
            AddItem(new BodySash() { Hue = 1154, Name = "Tidewoven Sash" }); // Silver-blue shimmer
            AddItem(new FlowerGarland() { Hue = 1160, Name = "Coral Circlet" }); // Coral pink, worn like a crown
            AddItem(new SilverRing() { Hue = 1165, Name = "Pearl Ring of Tides" }); // Pearl ring that glows faintly

            Backpack backpack = new Backpack();
            backpack.Hue = 1102;
            backpack.Name = "Singer's Satchel";
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
                        "*Lira hums a haunting melody that seems to calm the winds.*",
                        "*She glances at the horizon.* I feel the sea pulling... it is eager for my voice.",
                        "*Her fingers trail through the air.* The storm listens, but it must hear the song.",
                        "*You catch her whispering.* The tide rises for those who cannot sing...",
                        "*Lira grips her sash tightly.* Each wave brings a memory, each wind a warning.",
                        "*A distant roll of thunder echoes.* The sea is not cruel—just forgotten.",
                        "*Lira sings softly.* O tide, be still, thy singer near, through storm and dark, let harbor clear."
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
