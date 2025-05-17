using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class MoonlitHuntQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Moonlit Hunt"; } }

        public override object Description
        {
            get
            {
                return
                    "Before you stands *Lycanin Howlcrest*, the grim-faced **Hunter Guildmaster** of Death Glutch.\n\n" +
                    "His cloak flutters in the dry wind, silvered with age and something deeper—resignation, perhaps. A scar streaks across his jaw, his eyes ever watching the waxing moon.\n\n" +
                    "“A beast roams the old Academy,” he growls, “not just any beast—a rogue from *my* bloodline. It brings shame, death, and fear. My kind live by ancient laws. When one of us falls to madness, we hunt them. Alone if we must.”\n\n" +
                    "**“But this one… this one is different. The lunar pull warps it beyond reason.”**\n\n" +
                    "“I ask not just for your sword, but your sense. Slay it, yes. But remember: silver wounds more than flesh. It sears the soul.”\n\n" +
                    "**Track and kill the rogue Werewolf** that stalks Malidor Witches Academy.";
            }
        }

        public override object Refuse
        {
            get
            {
                return "“Then let the beast run wild, and know this blood will not stay silent forever.”";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "“Still alive? Then it grows bolder with each nightfall. I can feel it.”";
            }
        }

        public override object Complete
        {
            get
            {
                return "“It is done, then? The moon no longer claws at my mind.”\n\n" +
                       "“You’ve lifted more than a blade—you’ve lifted a burden I’ve long carried.”\n\n" +
                       "Take this, *VoidsEmbrace*. A gift forged in shadow, tempered in duty.";
            }
        }

        public MoonlitHuntQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(Werewolf), "the rogue Werewolf", 1));
            AddReward(new BaseReward(typeof(VoidsEmbrace), 1, "VoidsEmbrace"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Moonlit Hunt'!");
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

    public class LycaninHowlcrest : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(MoonlitHuntQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBRanger()); // Closest profession to a hunter guildmaster.
        }

        [Constructable]
        public LycaninHowlcrest()
            : base("the Hunter Guildmaster", "Lycanin Howlcrest")
        {
        }

        public LycaninHowlcrest(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(100, 90, 90);

            Female = false;
            Body = 0x190; // Male human
            Race = Race.Human;

            Hue = 33770; // Pale, almost moonlit skin
            HairItemID = 0x2049; // Long hair
            HairHue = 1150; // Raven black
            FacialHairItemID = 0x204B; // Medium beard
            FacialHairHue = 1150;
        }

        public override void InitOutfit()
        {
            AddItem(new StuddedChest() { Hue = 1108, Name = "Moonhide Jerkin" }); // Dark grey
            AddItem(new StuddedLegs() { Hue = 1109, Name = "Hunter's Greaves" }); // Darker grey
            AddItem(new Cloak() { Hue = 1157, Name = "Cloak of the Crescent Hunt" }); // Midnight blue
            AddItem(new BearMask() { Hue = 1154, Name = "Wolf's Maw Helm" }); // Dark iron hue
            AddItem(new LeatherGloves() { Hue = 2406, Name = "Silverclaw Gloves" }); // Pale silver
            AddItem(new Boots() { Hue = 1102, Name = "Moonstalker Boots" }); // Deep black

            AddItem(new CrescentBlade() { Hue = 2401, Name = "Lunarscar Blade" }); // Silvered weapon

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Hunter's Pouch";
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
