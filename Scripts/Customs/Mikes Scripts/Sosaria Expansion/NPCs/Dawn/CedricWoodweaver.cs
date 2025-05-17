using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class FeathersOfFoulFaithQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Feathers of Foul Faith"; } }

        public override object Description
        {
            get
            {
                return
                    "Cedric Woodweaver, a sturdy carpenter with a passion for fine timber, stands amidst a scatter of ruined planks.\n\n" +
                    "He wipes sweat from his brow, clearly agitated.\n\n" +
                    "“Something’s fouling my lumber. Not rats, not termites—a damned *chicken*, but not one I’ve ever seen. It pecks with purpose, like it's... chanting. Every dawn, more wood ruined. Some say it’s a spawn from that cursed Doom dungeon.”\n\n" +
                    "“I traced its trail to the edge of town, where the soil's blackened. That bird's no natural beast—it’s *foul faith feathered*, born of dark rites. If I don’t stop it, I’ll have no shop left.”\n\n" +
                    "“Track it to Doom, end it, and I’ll see you rewarded with something worthy of your effort.”\n\n" +
                    "**Slay the Cult Chicken**, and let my saws sing once more.";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then beware the morning crow—it carries omens of splintered dreams.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "You haven’t dealt with it yet? The thing was back last night, mocking me with its screeching chant. My planks won’t last another dawn!";
            }
        }

        public override object Complete
        {
            get
            {
                return "You did it? Truly? The air feels lighter already.\n\n" +
                       "No more chants, no more ruin. You’ve saved more than my shop—you’ve saved Dawn’s peace.\n\n" +
                       "**Take this, Gravemaker.** A fitting reward, forged from the last of my untouched timber, hardened with resolve.";
            }
        }

        public FeathersOfFoulFaithQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(CultChicken), "Cult Chicken", 1));
            AddReward(new BaseReward(typeof(Gravemaker), 1, "Gravemaker"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Feathers of Foul Faith'!");
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

    public class CedricWoodweaver : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(FeathersOfFoulFaithQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBCarpenter());
        }

        [Constructable]
        public CedricWoodweaver()
            : base("the Carpenter", "Cedric Woodweaver")
        {
        }

        public CedricWoodweaver(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(90, 85, 60);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = 33770; // Weathered tan
            HairItemID = 0x2047; // Short hair
            HairHue = 1150; // Ash-brown
            FacialHairItemID = 0x203E; // Full beard
            FacialHairHue = 1150;
        }

        public override void InitOutfit()
        {
            AddItem(new FancyShirt() { Hue = 2123, Name = "Timberman's Tunic" }); // Rich oak-brown
            AddItem(new ShortPants() { Hue = 2415, Name = "Splinterguard Breeches" }); // Dark chestnut
            AddItem(new HalfApron() { Hue = 1175, Name = "Woodweaver's Apron" }); // Deep forest green
            AddItem(new Boots() { Hue = 2306, Name = "Ironroot Boots" }); // Grey-black
            AddItem(new LeatherGloves() { Hue = 2503, Name = "Grain-Touched Gloves" }); // Bark-brown
            AddItem(new Bandana() { Hue = 1170, Name = "Sawdust-Stained Bandana" }); // Faded crimson

            AddItem(new CarpentersHammer() { Hue = 2101, Name = "Faith-Splitter" }); // Weathered iron
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
