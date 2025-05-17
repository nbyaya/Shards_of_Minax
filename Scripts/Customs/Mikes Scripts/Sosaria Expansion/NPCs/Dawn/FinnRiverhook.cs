using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class SilentArchivesQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Silent Archives"; } }

        public override object Description
        {
            get
            {
                return
                    "You find *Finn Riverhook*, the weathered fisher of Dawn, perched on the edge of the riverbank. His net is torn, his eyes restless, darting from the water to the soaked pages spread before him.\n\n" +
                    "“The river carried them back… books from the library, but defiled.” He gestures at the ruined manuscripts. Fish scales and black ink cling like leeches to the parchment.\n\n" +
                    "“There’s something in Doom—**a Librarian**, they say. Or was. Now it haunts the catacombs, pulling words from pages, people from land... and baiting them to drown.”\n\n" +
                    "“One of ours followed it. Gone. And these came floating back.”\n\n" +
                    "“You’ve got to go. **Find the Cult Librarian**, and end it before Dawn’s stories are lost. Bring back what's left, if you can.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then let the river carry the rest of our past away. But I fear what more it might bring.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still no sign of the Librarian’s end? The river churns more violently now… and I swear I hear reading in the night.";
            }
        }

        public override object Complete
        {
            get
            {
                return "You’ve done it? The Librarian’s silence echoes louder than its voice ever did.\n\n" +
                       "These waters… they feel clearer. The fish stir again.\n\n" +
                       "**Take Cogfang.** It’s old, but it’s sharp, and faithful like the tides. Let it serve you as well as you’ve served Dawn.";
            }
        }

        public SilentArchivesQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(CultLibrarian), "Cult Librarian", 1));
            AddReward(new BaseReward(typeof(Cogfang), 1, "Cogfang"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Silent Archives'!");
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

    public class FinnRiverhook : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(SilentArchivesQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBFisherman());
        }

        [Constructable]
        public FinnRiverhook()
            : base("the Weathered Fisher", "Finn Riverhook")
        {
        }

        public FinnRiverhook(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(80, 75, 60);

            Female = false;
            Body = 0x190; // Male Human
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = 0x203C; // Long Hair
            HairHue = 1107; // Sea-grey
            FacialHairItemID = 0x2041; // Short Beard
            FacialHairHue = 1107;
        }

        public override void InitOutfit()
        {
            AddItem(new FancyShirt() { Hue = 1108, Name = "Storm-Washed Shirt" }); // Pale Blue
            AddItem(new LongPants() { Hue = 2401, Name = "Riverbank Trousers" }); // Moss-Green
            AddItem(new HalfApron() { Hue = 2301, Name = "Tide-Stained Apron" }); // Faded Blue
            AddItem(new ThighBoots() { Hue = 1810, Name = "Waterlogged Boots" }); // Muddy Brown
            AddItem(new WideBrimHat() { Hue = 1109, Name = "Fisherman's Shade" }); // Salt-Grey

            AddItem(new FishingPole() { Hue = 2503, Name = "Riverhook’s Rod" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Tideworn Satchel";
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
