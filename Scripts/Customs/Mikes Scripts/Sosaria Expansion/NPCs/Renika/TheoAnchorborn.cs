using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class MountainsWhisperQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Mountain’s Whisper"; } }

        public override object Description
        {
            get
            {
                return
                    "You find *Theo Anchorborn*, Dockmaster of Renika, standing firm on the weathered boards of the harbor.\n\n" +
                    "His coat flutters in the sea breeze, yet his eyes are fixed inland—toward the looming peaks.\n\n" +
                    "“They say the sea is wild, but mountains hold their own storms… I should know. My blood fled from one centuries ago.”\n\n" +
                    "“My family founded this port, fleeing the wrath of the **AncientYamandon**—a beast that still stirs in the caverns of Mountain Stronghold. Its voice carries on the wind, a whisper of the souls it’s bound to those walls.”\n\n" +
                    "“Lately, I’ve heard that whisper again. Sailors hear it in the rigging. The beast wakes, and with it, the curse.”\n\n" +
                    "“Go. Slay the AncientYamandon. Free the trapped souls, and silence the mountain’s cry before it echoes across Renika once more.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "I can’t force you to face the mountain, but I fear the beast’s voice will reach us all in time.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still no peace from the peaks? The harbor groans, as if it too feels the weight of those whispers.";
            }
        }

        public override object Complete
        {
            get
            {
                return "The mountain is silent? Then perhaps, at last, the souls have found their rest—and my forebears can know peace.\n\n" +
                       "Take these *RingmastersSandals*. They’ve seen many docks and roads. May they carry you forward as swiftly as you’ve delivered us from the past.";
            }
        }

        public MountainsWhisperQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(AncientYamandon), "AncientYamandon", 1));
            AddReward(new BaseReward(typeof(RingmastersSandals), 1, "RingmastersSandals"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Mountain’s Whisper'!");
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

    public class TheoAnchorborn : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(MountainsWhisperQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBVeterinarian());
        }

        [Constructable]
        public TheoAnchorborn()
            : base("the Dockmaster", "Theo Anchorborn")
        {
        }

        public TheoAnchorborn(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(90, 85, 35);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = 33770; // Weathered tan
            HairItemID = 0x203B; // Long hair
            HairHue = 1102; // Salt-and-pepper gray
            FacialHairItemID = 0x204C; // Medium beard
            FacialHairHue = 1102;
        }

        public override void InitOutfit()
        {
            AddItem(new Doublet() { Hue = 2955, Name = "Seafoam Doublet" });
            AddItem(new LongPants() { Hue = 2503, Name = "Stormline Trousers" });
            AddItem(new BodySash() { Hue = 1157, Name = "Anchor’s Embrace Sash" });
            AddItem(new TricorneHat() { Hue = 1175, Name = "Waveshadow Tricorne" });
            AddItem(new Boots() { Hue = 2101, Name = "Saltstained Boots" });
            AddItem(new Cutlass() { Hue = 2219, Name = "Dockmaster's Blade" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Harbor Pack";
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
