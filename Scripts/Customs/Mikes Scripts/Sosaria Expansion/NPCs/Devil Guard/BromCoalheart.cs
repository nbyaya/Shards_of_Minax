using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class SilenceTheBronzeQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Silence the Bronze"; } }

        public override object Description
        {
            get
            {
                return
                    "You find *Brom Coalheart*, a weathered prospector with soot-streaked skin and eyes that burn like embers.\n\n" +
                    "His hammer rests by his side, glowing faintly with heat. The ground beneath him vibrates slightly.\n\n" +
                    "“The earth’s singing again—but not like before. It’s the BronzeOfTheDeep, stirs from the molten veins below.”\n\n" +
                    "“Years back, my mentor went chasing that sound. Never returned. Now it’s louder, angrier. The lower shafts quake. If we don’t still it, the whole mine’s gonna drown in bronze.”\n\n" +
                    "**Slay the BronzeOfTheDeep** before its molten core floods Devil Guard’s lifeblood.”\n\n";
            }
        }

        public override object Refuse
        {
            get
            {
                return "You’ll let it sing then? Fine. But when the mines melt, don’t say Brom Coalheart didn’t warn you.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still hear it... that thrumming, deep down. The bronze calls, louder than ever.";
            }
        }

        public override object Complete
        {
            get
            {
                return "It’s done? You’ve quieted the Bronze?\n\n" +
                       "I thought I’d die to that song... but you broke it. Broke its hold.\n\n" +
                       "Here—*TongueOfTheGods*. It’s all I got of worth, a relic from the deep. Let it serve you better than the mines served me.";
            }
        }

        public SilenceTheBronzeQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(BronzeOfTheDeep), "BronzeOfTheDeep", 1));
            AddReward(new BaseReward(typeof(TongueOfTheGods), 1, "TongueOfTheGods"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Silence the Bronze'!");
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

    public class BromCoalheart : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(SilenceTheBronzeQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBMiner());
        }

        [Constructable]
        public BromCoalheart()
            : base("the Prospector", "Brom Coalheart")
        {
        }

        public BromCoalheart(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(100, 85, 45);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = 1109; // Dark, soot-stained skin tone
            HairItemID = Race.RandomHair(this);
            HairHue = 1150; // Charcoal black
            FacialHairItemID = Race.RandomFacialHair(this);
            FacialHairHue = 1150;
        }

        public override void InitOutfit()
        {
            AddItem(new StuddedChest() { Hue = 1823, Name = "Coalheart's Vest" }); // Dark bronze hue
            AddItem(new StuddedLegs() { Hue = 2410, Name = "Smelter's Trousers" });
            AddItem(new LeatherGloves() { Hue = 2305, Name = "Charred Gloves" });
            AddItem(new PlateHelm() { Hue = 2424, Name = "Molten Visor" });
            AddItem(new HalfApron() { Hue = 2422, Name = "Ore-Splitter's Apron" });
            AddItem(new Boots() { Hue = 1813, Name = "Deepwalker Boots" });

            AddItem(new Maul() { Hue = 2208, Name = "Forgeheart Hammer" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Prospector's Pack";
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
