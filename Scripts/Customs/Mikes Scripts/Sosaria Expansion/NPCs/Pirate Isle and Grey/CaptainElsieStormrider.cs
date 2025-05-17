using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class ChargingSpecterQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Charging Specter"; } }

        public override object Description
        {
            get
            {
                return
                    "*Captain Elsie Stormrider*, the corsair of Pirate Isle, adjusts her tricorn hat against the sea wind, her eyes narrowed like thunderclouds.\n\n" +
                    "“The docks are no longer safe. That **SpectralCharger**—it roams, hunts, drives my best mounts to madness.”\n\n" +
                    "“It struck during a moonless patrol. Lanterns flickered, hooves struck sand, and then... silence. My men returned shaken, their minds near broken.”\n\n" +
                    "“This isn’t just some ghost tale to scare sailors. It rides near the **Exodus Dungeon**, preying on those who stray near the cliffs.”\n\n" +
                    "“I’ve marked its haunt with flickering lanterns. Follow them, find the beast, and end its charge.”\n\n" +
                    "**Slay the SpectralCharger** before it drives more of my fleet to ruin.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Aye, but know this—each night you hesitate, the Charger grows bolder.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still it rides? The lanterns flicker again. My mounts refuse to leave the stables. Act fast.";
            }
        }

        public override object Complete
        {
            get
            {
                return "So, you've unhorsed the ghost? Ha! The sea blesses you.\n\n" +
                       "Take this **SpecialChivalryChest**—a prize fit for those who stand firm against the shadows.";
            }
        }

        public ChargingSpecterQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(SpectralCharger), "SpectralCharger", 1));
            AddReward(new BaseReward(typeof(SpecialChivalryChest), 1, "SpecialChivalryChest"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Charging Specter'!");
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

    public class CaptainElsieStormrider : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(ChargingSpecterQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBShipwright(this));
        }

        [Constructable]
        public CaptainElsieStormrider()
            : base("the Corsair", "Captain Elsie Stormrider")
        {
        }

        public CaptainElsieStormrider(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(90, 95, 60);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = 0x2048; // Long wavy hair
            HairHue = 1157; // Storm-silver
        }

        public override void InitOutfit()
        {
            AddItem(new StuddedDo() { Hue = 1109, Name = "Storm-Leather Corsair's Tunic" }); // Dark sea-gray
            AddItem(new LeatherLegs() { Hue = 1108, Name = "Wave-Rider's Breeches" });
            AddItem(new LeatherGloves() { Hue = 1150, Name = "Wind-Worn Gloves" });
            AddItem(new TricorneHat() { Hue = 1175, Name = "Captain's Crest Hat" });
            AddItem(new Cloak() { Hue = 1175, Name = "Seafoam Cloak" });
            AddItem(new ThighBoots() { Hue = 1102, Name = "Deckwalker's Boots" });

            AddItem(new Cutlass() { Hue = 2500, Name = "Stormrider's Edge" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1153;
            backpack.Name = "Corsair's Pack";
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
