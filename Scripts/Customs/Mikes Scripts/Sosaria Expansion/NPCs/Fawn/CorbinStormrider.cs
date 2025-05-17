using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class DarknessUnveiledQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Darkness Unveiled"; } }

        public override object Description
        {
            get
            {
                return
                    "Before you stands *Corbin Stormrider*, the renowned falconer of Fawn, clad in windswept leathers and bearing a crescent-shaped lure of gleaming moonstone.\n\n" +
                    "His eyes are sharp, yet clouded by concern.\n\n" +
                    "“There’s a shadow on the cliffs. My hawks—bold as they are—won’t fly near it. **Nullshade**, they whisper in old tales, a creature that drinks the sky and silences the land.”\n\n" +
                    "“I’ve crafted lures from moonstone, hoping to draw it out, but it sees through tricks meant for birds. I need someone strong—one who can face the dark and not flinch.”\n\n" +
                    "**Find the Nullshade** where the cliffs touch the clouds, and end its blight before it pulls more light from our skies.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "I can’t fault your caution. But the cliffs grow darker by the day, and Fawn may lose more than just its birds.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still it flies? My lures hang empty, and the winds grow hollow. We must act, before nightfall never ends.";
            }
        }

        public override object Complete
        {
            get
            {
                return "The skies brighten, and my hawks return. You’ve banished the shade and saved more than just wings.\n\n" +
                       "**Take this: Wizardspike.** Forged in moonlit times, it pierces the veils between light and dark. Wield it wisely.";
            }
        }

        public DarknessUnveiledQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(Nullshade), "Nullshade", 1));
            AddReward(new BaseReward(typeof(Wizardspike), 1, "Wizardspike"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Darkness Unveiled'!");
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

    public class CorbinStormrider : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(DarknessUnveiledQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBCarpenter());
        }

        [Constructable]
        public CorbinStormrider()
            : base("the Falconer", "Corbin Stormrider")
        {
        }

        public CorbinStormrider(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(80, 85, 70);

            Female = false;
            Body = 0x190; // Male Human
            Race = Race.Human;

            Hue = 1002; // Light tan skin
            HairItemID = 0x2048; // Short hair
            HairHue = 1153; // Wind-swept grey
            FacialHairItemID = 0x203B; // Trimmed beard
            FacialHairHue = 1153;
        }

        public override void InitOutfit()
        {
            AddItem(new StuddedChest() { Hue = 2419, Name = "Skybound Jerkin" }); // Sky-blue tinged leather
            AddItem(new StuddedLegs() { Hue = 2101, Name = "Cliffwalker Greaves" }); // Earthy-brown
            AddItem(new LeatherGloves() { Hue = 2101, Name = "Falconer’s Grip" });
            AddItem(new FeatheredHat() { Hue = 1153, Name = "Stormrider’s Hat" }); // Grey feathered
            AddItem(new Boots() { Hue = 2101, Name = "Tracker’s Boots" });
            AddItem(new Cloak() { Hue = 2419, Name = "Moonstone Cloak" }); // Moonlit shimmer

            AddItem(new ShepherdsCrook() { Hue = 2500, Name = "Lurebearer’s Crook" }); // Used for falcon lures

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Falconer’s Pack";
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
