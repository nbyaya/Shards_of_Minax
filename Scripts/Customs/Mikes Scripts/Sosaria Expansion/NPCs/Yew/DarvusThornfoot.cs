using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class SilkAndTerrorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Silk and Terror"; } }

        public override object Description
        {
            get
            {
                return
                    "You meet *Darvus Thornfoot*, the wiry, fast-talking runner for Yew’s bustling market. His clothes are spattered with forest dust, and he nervously fidgets with a rolled-up map stained with ink and fear.\n\n" +
                    "“It’s bad—real bad. The *Catastrophe* tunnels? Used to be a shortcut for my caravans. Now? A cursed spider's den. The **ColossalBlackWidow** spun webs so thick, they choke the very light! I’ve got wagons trapped in there, goods rotting... and people missing.”\n\n" +
                    "“I can’t afford more losses. *Yew* can’t afford it. This trade route is our lifeline.”\n\n" +
                    "“You look like the sort who can handle themselves in the dark. Get in there. Burn the webs. Crush that monster. **Slay the Widow** so we can breathe again.”\n\n" +
                    "“Do it for Yew, for trade... do it for *Eren*, my best runner, who never came back.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then may the silk close tighter. But know this: every day we wait, more lives, more wagons, more hope is tangled in her web.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still she weaves? I can hear the screams now, like threads snapping in the wind. Please, you have to stop her.";
            }
        }

        public override object Complete
        {
            get
            {
                return "You’ve done it? You’ve *cut the cords*? Yew owes you more than coin—we owe you our roads, our trade, our future.\n\n" +
                       "Take this: *NightstepThreads*. Woven by our best, in memory of those we lost to silk and terror. Walk swift, friend, and never be caught again.";
            }
        }

        public SilkAndTerrorQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(ColossalBlackWidow), "Colossal Black Widow", 1));
            AddReward(new BaseReward(typeof(NightstepThreads), 1, "NightstepThreads"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Silk and Terror'!");
            Owner.PlaySound(CompleteSound);
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

    public class DarvusThornfoot : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(SilkAndTerrorQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBProvisioner()); 
        }


        [Constructable]
        public DarvusThornfoot()
            : base("the Market Runner", "Darvus Thornfoot")
        {
        }

        public DarvusThornfoot(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 85, 35);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = 1811; // Sandy brown
            FacialHairItemID = 0x203F; // Goatee
            FacialHairHue = HairHue;
        }

        public override void InitOutfit()
        {
            AddItem(new Doublet() { Hue = 2213, Name = "Runner’s Jerkin" }); // Moss green
            AddItem(new ShortPants() { Hue = 2210, Name = "Thornfoot Breeches" }); // Earthy brown
            AddItem(new HalfApron() { Hue = 1109, Name = "Trade-Worn Apron" }); // Dusty grey
            AddItem(new Sandals() { Hue = 2101, Name = "Silent Step Sandals" }); // Deep black
            AddItem(new Bandana() { Hue = 2118, Name = "Forest Shade Bandana" }); // Dark green

            AddItem(new SkinningKnife() { Hue = 1107, Name = "Web-Cutter" }); // Steel blue

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Runner's Satchel";
            AddItem(backpack);
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
}
