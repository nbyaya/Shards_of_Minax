using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class ShatteredFacadeQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Shattered Facade"; } }

        public override object Description
        {
            get
            {
                return
                    "Before you stands *Sorin Icevein*, a Lapidary Artisan known for crafting the finest crystal in Mountain Crest.\n\n" +
                    "His hands, dusted with frost-like powder, tremble as he examines a splintered gem.\n\n" +
                    "\"My work, my legacy, is unraveling... The merchants I trusted were not what they seemed. **Changelings**, crystalline mimics, have infiltrated our trade. One, in particular, has stolen shipments bound for the nobles.\"\n\n" +
                    "\"I've traced the illusion’s hum back to the Ice Cavern—where the Crystal Changeling hides. It mimics not just faces, but the trust of those it deceives.\"\n\n" +
                    "\"Please, adventurer. Find it. Slay it. Before the façade it weaves shatters all that I’ve built.\"";
            }
        }

        public override object Refuse
        {
            get
            {
                return "I understand. But remember—appearances can lie, and truth is a fragile gem.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "The hum still echoes. That thing continues to wear our faces, ruin our names.";
            }
        }

        public override object Complete
        {
            get
            {
                return "So the impostor is no more? Then perhaps trust can shine again.\n\n" +
                       "**Take this Celestial Scimitar**—crafted in a time when truth and beauty were one. Let its light guide you through falsehoods yet to come.";
            }
        }

        public ShatteredFacadeQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(CrystalChangeling), "Crystal Changeling", 1));
            AddReward(new BaseReward(typeof(CelestialScimitar), 1, "Celestial Scimitar"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Shattered Facade'!");
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

    public class SorinIcevein : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(ShatteredFacadeQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBShipwright(this));
        }

        [Constructable]
        public SorinIcevein()
            : base("the Lapidary Artisan", "Sorin Icevein")
        {
        }

        public SorinIcevein(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 80, 35);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = 0x840; // Pale, icy hue
            HairItemID = 0x203C; // Short Hair
            HairHue = 1153; // Frost-white hair
            FacialHairItemID = 0x203F; // Medium Beard
            FacialHairHue = 1153;
        }

        public override void InitOutfit()
        {
            AddItem(new ElvenShirt() { Hue = 1150, Name = "Frostlace Tunic" }); // Ice-blue shimmer
            AddItem(new ElvenPants() { Hue = 1160, Name = "Gemcutter's Leggings" }); // Pale silver
            AddItem(new FurBoots() { Hue = 1109, Name = "Winterhide Boots" }); // Greyish fur
            AddItem(new Cloak() { Hue = 1152, Name = "Mistwoven Cloak" }); // Soft shimmering white
            AddItem(new LeatherGloves() { Hue = 1150, Name = "Facet-Grip Gloves" }); // Ice blue
            AddItem(new Circlet() { Hue = 1153, Name = "Veinwatch Circlet" }); // Frost-white circlet of lapidary
            AddItem(new QuarterStaff() { Hue = 1150, Name = "Lapidary’s Focus" }); // Custom staff for artisan look

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Crystal Satchel";
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
