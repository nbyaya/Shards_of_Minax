using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class AvengersFallQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Avengers Fall"; } }

        public override object Description
        {
            get
            {
                return
                    "Before you stands *Yara Veinseer*, the keen-eyed Vein Scout of Devil Guard.\n\n" +
                    "Her gaze is sharp, calculating—her cloak dusted with the stone and sweat of the mines. She nods, her voice low but urgent:\n\n" +
                    "“I've watched these tunnels shift, heard them sing warnings before collapse. But this... this is no natural danger.”\n\n" +
                    "“The **CaveAvenger**—a beast twisted by dark ore and vengeance. It’s hunting us, picking off scouts one by one. We can't hold the southern vein while it roams. We can't breathe down there.”\n\n" +
                    "“You look capable. Take this map, find the monster. End it. Let the mines be ours again.”\n\n" +
                    "**Slay the CaveAvenger** and return with proof. Let the vein run clean with no more blood spilled.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "The mines won't wait. Neither will the Avenger. Pray you don't hear its cry in the night.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still alive down there? Or have you fled? The vein won’t stay sealed. It’s only a matter of time before it feeds again.";
            }
        }

        public override object Complete
        {
            get
            {
                return "You did it. The Avenger is gone, and we can breathe again. The southern vein’s song has quieted.\n\n" +
                       "**This is for you:** *BarkbindLoop*—crafted from the roots that hold these mountains steady. Wear it well. You’ve earned it.";
            }
        }

        public AvengersFallQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(CaveAvenger), "CaveAvenger", 1));
            AddReward(new BaseReward(typeof(BarkbindLoop), 1, "BarkbindLoop"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Avengers Fall'!");
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

    public class YaraVeinseer : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(AvengersFallQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBMiner()); // Closest to vein scouting and mining lore
        }

        [Constructable]
        public YaraVeinseer()
            : base("the Vein Scout", "Yara Veinseer")
        {
        }

        public YaraVeinseer(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(90, 80, 50);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = 1154; // Dark iron
        }

        public override void InitOutfit()
        {
            AddItem(new LeatherNinjaHood() { Hue = 1820, Name = "Vein-Seeker Hood" }); // Deep slate grey
            AddItem(new LeatherDo() { Hue = 1819, Name = "Miner’s Mantle" }); // Shadow brown
            AddItem(new StuddedLegs() { Hue = 2305, Name = "Stonebound Greaves" }); // Earthy tone
            AddItem(new LeatherGloves() { Hue = 1824, Name = "Ore-Touched Gloves" });
            AddItem(new Boots() { Hue = 2101, Name = "Tunnel-Striders" });

            AddItem(new Pickaxe() { Hue = 2507, Name = "Vein-Singer Pick" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1151;
            backpack.Name = "Scout’s Satchel";
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
