using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class NightsThornQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Night's Thorn"; } }

        public override object Description
        {
            get
            {
                return
                    "You find **Orin Starwhisper**, a contemplative astronomer of Fawn, hunched over a cracked telescope.\n\n" +
                    "His robes shimmer faintly under the moonlight, eyes shadowed by sleepless nights.\n\n" +
                    "“The stars—no, the very sky itself—weeps under its scream. This creature, the **Nachtbram**, it shrieks and the heavens fracture. I’ve charted its howls across the phases of the moon. Always near... always closer.”\n\n" +
                    "“My lenses splintered as I sought its form. I saw only shadow and thorn, a wound in the night sky. It must be stopped, before it unravels the constellations themselves.”\n\n" +
                    "**Banish the Nachtbram**, and let the stars shine true once more.";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then the sky shall bleed, and we shall lose the light by which we guide our lives.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still it howls? The nights grow longer. My instruments fail. The sky twists in ways I cannot chart.";
            }
        }

        public override object Complete
        {
            get
            {
                return "The scream is gone. The stars... they breathe again.\n\n" +
                       "You’ve saved not just my work, but the celestial harmony we depend upon.\n\n" +
                       "Take this: **BrassFountain**, a relic shaped from stellar brass, imbued with the calm of a silent night. Let it bring clarity, as you have brought peace.";
            }
        }

        public NightsThornQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(Nachtbram), "Nachtbram", 1));
            AddReward(new BaseReward(typeof(BrassFountain), 1, "BrassFountain"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Night's Thorn'!");
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

    public class OrinStarwhisper : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(NightsThornQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => false;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBHairStylist()); // Astronomer -> Cartography
        }

        [Constructable]
        public OrinStarwhisper()
            : base("the Astronomer", "Orin Starwhisper")
        {
        }

        public OrinStarwhisper(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(70, 85, 60);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = 1150; // Midnight blue
            FacialHairItemID = Race.RandomFacialHair(this);
            FacialHairHue = 1150;
        }

        public override void InitOutfit()
        {
            AddItem(new FancyShirt() { Hue = 1281, Name = "Starwoven Tunic" }); // Deep indigo
            AddItem(new LongPants() { Hue = 1270, Name = "Night-Sky Trousers" }); // Shadow grey
            AddItem(new WizardsHat() { Hue = 1153, Name = "Moon-Phase Hat" }); // Pale silver
            AddItem(new Cloak() { Hue = 1277, Name = "Celestial Veil" }); // Starry blue
            AddItem(new Sandals() { Hue = 1151, Name = "Skywatcher's Sandals" }); // Dark blue

            Backpack backpack = new Backpack();
            backpack.Hue = 1175;
            backpack.Name = "Star-Chart Pack";
            AddItem(backpack);

            AddItem(new GnarledStaff() { Hue = 1154, Name = "Astrologer's Cane" }); // Light cosmic blue
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
