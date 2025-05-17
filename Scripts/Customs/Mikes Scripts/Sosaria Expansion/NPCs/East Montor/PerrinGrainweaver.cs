using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class CaprineCalamityQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Caprine Calamity"; } }

        public override object Description
        {
            get
            {
                return
                    "Perrin Grainweaver, the miller’s apprentice, stands amid torn sacks and scattered grain.\n\n" +
                    "His hands are dusted with flour, yet tremble with frustration.\n\n" +
                    "“I can’t make a living like this! Every dusk, that cursed **DrakonGoat** comes down from the caves, gnawing through everything. Its horns, they glow—like it's born of fire and mischief.”\n\n" +
                    "“We’ve tried traps, we’ve tried noise. Nothing works. It's not just a goat—it’s something else.”\n\n" +
                    "“You’ll help? Truly? **Hunt down that DrakonGoat** and give me a chance to start fresh. Without it, maybe East Montor can trust me with the mill again.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "I understand... but with every sack it ruins, I lose more than just grain. I lose hope.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still it prowls? My grain’s near gone. Please, don’t let it come again.";
            }
        }

        public override object Complete
        {
            get
            {
                return "You’ve done it? The DrakonGoat’s dead?\n\n" +
                       "Thank you, truly! Here, take this—**SilkRoadTreasuresChest**. It’s all I can offer, but it's more than fair for what you’ve done.\n\n" +
                       "With peace restored, maybe the mill can turn again.";
            }
        }

        public CaprineCalamityQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(DrakonGoat), "DrakonGoat", 1));
            AddReward(new BaseReward(typeof(SilkRoadTreasuresChest), 1, "SilkRoadTreasuresChest"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Caprine Calamity'!");
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

    public class PerrinGrainweaver : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(CaprineCalamityQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => false;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBMiller());
        }

        [Constructable]
        public PerrinGrainweaver()
            : base("the Mill's Apprentice", "Perrin Grainweaver")
        {
        }

        public PerrinGrainweaver(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(60, 70, 40);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = 1130; // Wheat-blonde
        }

        public override void InitOutfit()
        {
            AddItem(new Shirt() { Hue = 2425, Name = "Mill-Dusted Shirt" }); // Off-white
            AddItem(new ShortPants() { Hue = 1810, Name = "Grainworker's Breeches" }); // Earth-brown
            AddItem(new HalfApron() { Hue = 2412, Name = "Flour-Streaked Apron" }); // Light tan
            AddItem(new Sandals() { Hue = 2306, Name = "Softgrain Sandals" }); // Pale leather

            AddItem(new Pitchfork() { Hue = 0, Name = "Worn Grain Fork" }); // Default hue
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
