using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class DepthsOfDespairQuestB : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Depths of Despair"; } }

        public override object Description
        {
            get
            {
                return
                    "You find *Rorgar Tidecaller*, the weather-beaten fishmonger of Pirate Isle, his eyes sunken and face drawn tight with worry.\n\n" +
                    "\"These waters have turned foul, stranger. My nets drag up fish twisted and bristling with rot—*the Manefall's* doing. That cursed thing stalks the deep trenches near Exodus, poisoning our shoals.\"\n\n" +
                    "\"I've seen many storms, but this is no natural blight. The AbyssalManefall must die, or my family’s name will sink with it. No more trade, no more life on this isle.\"\n\n" +
                    "**Slay the AbyssalManefall**, and return peace to these waters—or leave us to starve beneath cursed waves.";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then may the sea take us, and curse these nets forever.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still it lurks? The fish twist more each tide, and my nets come up emptier. I fear we haven’t long now.";
            }
        }

        public override object Complete
        {
            get
            {
                return "You’ve done it? The sea… it smells clean again. The nets… I’ll mend them with joy this time.\n\n" +
                       "Here, take this: *AshokasTreasureChest*. My father dredged it from the depths long ago, and said to give it only to one who’d save our souls.\n\n" +
                       "**You’ve done more than that—you’ve saved our bloodline.**";
            }
        }

        public DepthsOfDespairQuestB() : base()
        {
            AddObjective(new SlayObjective(typeof(AbyssalManefall), "AbyssalManefall", 1));
            AddReward(new BaseReward(typeof(AshokasTreasureChest), 1, "AshokasTreasureChest"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Depths of Despair'!");
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

    public class RorgarTidecaller : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(DepthsOfDespairQuestB) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBFisherman()); // Closest profession to fishmonger
        }

        [Constructable]
        public RorgarTidecaller()
            : base("the Fishmonger", "Rorgar Tidecaller")
        {
        }

        public RorgarTidecaller(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(80, 80, 25);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = 1109; // Salt-weathered skin tone
            HairItemID = Race.RandomHair(this);
            HairHue = 2101; // Seaweed green tint
            FacialHairItemID = Race.RandomFacialHair(this);
            FacialHairHue = 2101;
        }

        public override void InitOutfit()
        {
            AddItem(new Tunic() { Hue = 1820, Name = "Saltstained Tunic" });
            AddItem(new ShortPants() { Hue = 1803, Name = "Brine-Soaked Breeches" });
            AddItem(new Sandals() { Hue = 2105, Name = "Coral-Worn Sandals" });
            AddItem(new Bandana() { Hue = 2106, Name = "Sea's Grasp Bandana" });
            AddItem(new BodySash() { Hue = 2208, Name = "Tidecaller's Sash" });

            AddItem(new FishermansTrident() { Hue = 2408, Name = "Rorgar's Trident" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1153;
            backpack.Name = "Fishmonger's Pack";
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
