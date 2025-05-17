using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class InfernalSavageQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Stop the InfernalSavage"; } }

        public override object Description
        {
            get
            {
                return
                    "You meet *Lissa Brookwood*, a fisherwoman hardened by riverside storms and tempered by recent terror.\n\n" +
                    "She grips a net, its cords scorched and frayed, eyes fierce yet weary.\n\n" +
                    "\"The river’s no longer ours. That *thing*—the **InfernalSavage**—it haunts the hidden fords, tearing nets, ruining camps. My nets are in tatters, my friends too afraid to fish.\"\n\n" +
                    "\"It comes from the Gate of Hell, they say. Born of fire, rage, and hunger. But I’ve seen it with my own eyes—its claws glowing like molten iron.\"\n\n" +
                    "\"Help us. Hunt it down, where the river meets flame. Before we lose not just our catch, but our lives.\"\n\n" +
                    "**Track and slay the InfernalSavage** lurking near the Gate of Hell’s riverside fords.";
            }
        }

        public override object Refuse
        {
            get
            {
                return "If not you, then who? The rivers won't wait, and neither will the beast.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still it lives? Then the river runs red, and no net can hold it.";
            }
        }

        public override object Complete
        {
            get
            {
                return "You’ve done it. The river breathes again, and so do we.\n\n" +
                       "Take this—**the GalacticExplorer’sTrove**. Found it once in a storm-caught wreck, never knew what to make of it till now. It’s yours, for saving us from the flames.";
            }
        }

        public InfernalSavageQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(InfernalSavage), "InfernalSavage", 1));
            AddReward(new BaseReward(typeof(GalacticExplorersTrove), 1, "GalacticExplorer’sTrove"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Stop the InfernalSavage'!");
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

    public class LissaBrookwood : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(InfernalSavageQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBFisherman());
        }

        [Constructable]
        public LissaBrookwood()
            : base("the Fisherwoman", "Lissa Brookwood")
        {
        }

        public LissaBrookwood(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 80, 30);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = 1153; // Sun-faded blonde
        }

        public override void InitOutfit()
        {
            AddItem(new LeatherSkirt() { Hue = 2407, Name = "River-Tide Skirt" }); // Water-blue
            AddItem(new FancyShirt() { Hue = 2115, Name = "Stormwoven Blouse" }); // Storm-gray
            AddItem(new BodySash() { Hue = 1358, Name = "Ember-Singed Sash" }); // Singed red
            AddItem(new Sandals() { Hue = 1175, Name = "Ford-Walker Sandals" }); // Muddy-brown
            AddItem(new StrawHat() { Hue = 1109, Name = "Weathered Fisher’s Hat" }); // Weathered gray

            AddItem(new FishermansTrident() { Hue = 2413, Name = "Brookwood's Piercer" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1170;
            backpack.Name = "Netmender’s Pack";
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

