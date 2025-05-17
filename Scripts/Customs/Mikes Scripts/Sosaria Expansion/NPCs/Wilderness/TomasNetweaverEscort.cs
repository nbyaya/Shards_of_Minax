using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class TomasNetweaverQuest : BaseQuest
    {
        public override object Title { get { return "Net of Fortune"; } }

        public override object Description
        {
            get
            {
                return
                    "*Tomas clutches a frost-covered satchel, his beard encrusted with ice, yet his eyes shine with determination.*\n\n" +
                    "Name’s Tomas Netweaver, a fisherman by trade... and by fate, it seems. This storm stranded me, but I’ve got what I came for—a Frostscale Herring, rarest in these waters. My village’s hope rests in this little beast. Without it, we’ll starve or freeze. Help me get back to Iceclad Fisherman's Village, and the sea will owe you a debt!";
            }
        }

        public override object Refuse { get { return "*Tomas shakes his head, snow flurrying from his coat.* Then I’ll go alone... if I must."; } }
        public override object Uncomplete { get { return "*Tomas shivers, clutching his satchel tightly.* We’ve not far... but the storm grows cruel."; } }

        public TomasNetweaverQuest() : base()
        {
            AddObjective(new EscortObjective("the Iceclad Fisherman's Village"));
            AddReward(new BaseReward(typeof(GriswoldsEdge), "GriswoldsEdge – A legendary blade once forged in forgotten fire."));
        }

        public override void GiveRewards()
        {
            base.GiveRewards();
            Owner.SendMessage("*Tomas grins, holding up the fish.* You’ve done me and mine a great turn, friend. Take this—'GriswoldsEdge.' Forged in fire, tempered in ice, it’s seen storms you wouldn’t believe. May it serve you well where the tides grow dark.", null, 0x59B);
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

    public class TomasNetweaverEscort : BaseEscort
    {
        public override Type[] Quests { get { return new Type[] { typeof(TomasNetweaverQuest) }; } }
        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => false;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBFisherman());
        }

        private DateTime m_NextTalkTime;

        [Constructable]
        public TomasNetweaverEscort() : base()
        {
            Name = "Tomas Netweaver";
            Title = "the Frostbitten Fisherman";
            NameHue = 0x480;
        }

		public TomasNetweaverEscort(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 60, 40);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = 1020; // Weathered complexion
            HairItemID = 0x2048; // Long hair
            HairHue = 1102; // Frost grey
            FacialHairItemID = 0x204B; // Full beard
            FacialHairHue = 1102;
        }

        public override void InitOutfit()
        {
            AddItem(new Cloak() { Hue = 1150, Name = "Frostwind Cloak" }); // Deep icy blue
            AddItem(new FurSarong() { Hue = 2101, Name = "Sealhide Trousers" }); // Dark fur
            AddItem(new HalfApron() { Hue = 2413, Name = "Netweaver's Apron" }); // Sea-worn green
            AddItem(new FancyShirt() { Hue = 1109, Name = "Stormproof Tunic" }); // Slate grey
            AddItem(new FurBoots() { Hue = 1108, Name = "Icebound Boots" }); // Frosted grey
            AddItem(new FeatheredHat() { Hue = 1153, Name = "Mariner's Crest" }); // Cold sky blue

            AddItem(new FishermansTrident() { Hue = 2063, Name = "Wintertide Spear" }); // Silver-blue weapon

            Backpack backpack = new Backpack();
            backpack.Hue = 1109;
            backpack.Name = "Fisherman's Satchel";
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
                        "*Tomas tightens his cloak.* These winds... they bite deep, don't they?",
                        "*He pats his satchel.* This fish... it's more than food. It's hope.",
                        "*Tomas peers into the snow.* We’ve still a way to go, but I feel the sea calling.",
                        "*His eyes narrow.* Storms like these... they hide things better left unfound.",
                        "*He grips his spear.* Keep your eyes sharp. Frost wraiths hunt in weather like this.",
                        "*Tomas hums a sea shanty.* Oh the sea, she gives, and she takes, but never for free..."
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
