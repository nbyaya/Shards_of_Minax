using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class WoodOfWhispersQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Wood of Whispers"; } }

        public override object Description
        {
            get
            {
                return
                    "You find *Bram Honeyhand*, Dawn’s beloved beekeeper, his eyes full of worry as he gently wipes sap from a ruined hive.\n\n" +
                    "“Something unnatural stirs in the woods. My bees... they won’t hum, won’t dance. The hives, they’re strangled by roots—*the wood itself moves*, bleeds sap that kills anything small enough to breathe near it.”\n\n" +
                    "“I followed the trail—it leads near the Doom Dungeon. The others think me touched, but I saw it: a twisted tree, alive, feeding on light and sound. If I lose the bees, the harvest falls, and Dawn will feel the weight.”\n\n" +
                    "**Slay the Doomwood**, that cursed animate root, before it silences every hive and chokes the land with its poison.";
            }
        }

        public override object Refuse
        {
            get
            {
                return "I understand... but each day my bees grow quieter. If you change your mind, I’ll still be here, hoping for the hum to return.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "The Doomwood still stands? Then I’ll tend what hives I can... but know this: every moment we delay, the sap spreads.";
            }
        }

        public override object Complete
        {
            get
            {
                return "You’ve done it? The sap has ceased, and already the bees stir anew! You’ve not just saved my hives—you’ve saved Dawn’s heart.\n\n" +
                       "Take this, *Rootsong*. It’s made from the very wood that once bound us in silence, now tuned to life’s melody.";
            }
        }

        public WoodOfWhispersQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(Doomwood), "Doomwood", 1));
            AddReward(new BaseReward(typeof(Rootsong), 1, "Rootsong"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Wood of Whispers'!");
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

    public class BramHoneyhand : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(WoodOfWhispersQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBBeekeeper());
        }

        [Constructable]
        public BramHoneyhand()
            : base("the Beekeeper", "Bram Honeyhand")
        {
        }

        public BramHoneyhand(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(80, 70, 40);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = 0x838; // Warm tan
            HairItemID = 0x2049; // Short
            HairHue = 1153; // Honey-blonde
            FacialHairItemID = 0x204D; // Full beard
            FacialHairHue = 1153;
        }

        public override void InitOutfit()
        {
            AddItem(new FancyShirt() { Hue = 2129, Name = "Honeycomb Tunic" }); // Golden-yellow
            AddItem(new ShortPants() { Hue = 2301, Name = "Apiarist’s Breeches" }); // Earth-brown
            AddItem(new HalfApron() { Hue = 1175, Name = "Beeswax-Stained Apron" }); // Wax-yellow
            AddItem(new StrawHat() { Hue = 2101, Name = "Hive-Keeper's Hat" }); // Straw color
            AddItem(new Sandals() { Hue = 1170, Name = "Pollen-Dusted Sandals" }); // Light tan

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Honeyhand's Pack";
            AddItem(backpack);

            AddItem(new GnarledStaff() { Hue = 2120, Name = "Smoker's Cane" }); // Smoky gray
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
