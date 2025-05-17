using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class GoblinsGambitQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Goblin’s Gambit"; } }

        public override object Description
        {
            get
            {
                return
                    "*Rikkit Twobrains*, the ever-fidgeting tinker, waves a soot-covered hand, wild eyes darting from you to the crude blueprint in his grasp.\n\n" +
                    "“That *RotfangGoblin* stole my trap designs! Twisted ‘em into wicked things down in the **Exodus Dungeon**. Now it’s snaring wanderers like flies in a flask!”\n\n" +
                    "“You want the reward? Good. Smash that goblin’s face in, tear down every trap you see, and bring me back whatever schematics you can salvage.”\n\n" +
                    "“Oh, and don’t die. I’m still testing my ‘Life Recaller’ device, and trust me—you don’t want to be prototype #3.”\n\n" +
                    "**Slay the RotfangGoblin and dismantle its cursed traps.**";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Fine, fine! Just don’t come crying to me when you get snared in a spring-loaded spine crusher!";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still ticking, that goblin? My traps’ll never live it down... Go on, finish the job!";
            }
        }

        public override object Complete
        {
            get
            {
                return "Ha! That’ll teach the little snatcher! *Rotfang* is rot-gone, and my traps... well, they’ll need some tinkering.\n\n" +
                       "Here—take this, the **HelmetOfTheOreWhisperer**. It'll help you hear the *truth* in the stones. Or the lies, whichever speaks first.";
            }
        }

        public GoblinsGambitQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(RotfangGoblin), "RotfangGoblin", 1));
            AddReward(new BaseReward(typeof(HelmetOfTheOreWhisperer), 1, "HelmetOfTheOreWhisperer"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Goblin’s Gambit'!");
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

    public class RikkitTwobrains : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(GoblinsGambitQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBTinker(this));
        }

        [Constructable]
        public RikkitTwobrains()
            : base("the Eccentric Tinker", "Rikkit Twobrains")
        {
        }

        public RikkitTwobrains(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(70, 80, 40);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = 1824; // Soot-black
            FacialHairItemID = 0x2041; // Crazy beard
            FacialHairHue = 1824;
        }

        public override void InitOutfit()
        {
            AddItem(new FancyShirt() { Hue = 1157, Name = "Ink-Splotched Tinker’s Shirt" }); // Dark blue
            AddItem(new LeatherNinjaPants() { Hue = 1109, Name = "Soot-Streaked Trousers" }); // Dust-grey
            AddItem(new HalfApron() { Hue = 1359, Name = "Grease-Stained Apron" }); // Copper tone
            AddItem(new LeatherGloves() { Hue = 1108, Name = "Burnt-Fingered Gloves" }); // Ash-grey
            AddItem(new Sandals() { Hue = 2413, Name = "Tinker’s Treads" }); // Soft tan
            AddItem(new SkullCap() { Hue = 1150, Name = "Cogspinner's Cap" }); // Steel-blue
            AddItem(new TinkerTools() { Hue = 0, Name = "Multi-Gadget Wrench" });

            Backpack backpack = new Backpack();
            backpack.Hue = 2101;
            backpack.Name = "Schematics Satchel";
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
