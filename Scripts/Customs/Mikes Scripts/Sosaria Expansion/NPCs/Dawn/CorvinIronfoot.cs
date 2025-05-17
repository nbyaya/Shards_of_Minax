using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class HowlOfTheNightQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Howl of the Night"; } }

        public override object Description
        {
            get
            {
                return
                    "You meet *Corvin Ironfoot*, the stout blacksmith of Dawn. His broad frame is soot-streaked, and a thick leather apron clings to his chest.\n\n" +
                    "He adjusts his smith's gauntlet, his eyes clouded by worry.\n\n" +
                    "\"Travelers speak of a **Black Wolf**, with eyes like embers, that haunts the road to Doom dungeon. I've seen it myself—it struck just as the sun dipped low, tearing through my iron-laden wagon. My work, my livelihood, lost to shadows and snarls.\"\n\n" +
                    "\"I need someone strong, someone swift, to **slay this beast**. Our merchants won't risk the road while it prowls, and I won't see another shipment ruined.\"\n\n" +
                    "**Track the Black Wolf and end its curse. Bring peace to the road, and you'll earn more than coin—you'll earn my craft.**";
            }
        }

        public override object Refuse
        {
            get
            {
                return "\"Aye, I can't force you. But mark my words, that wolf won't wait for dusk to strike again.\"";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "\"Still alive, that cursed wolf? I hear the howls now even at dawn. Merchants are talking of leaving. I can't hold this town together with broken blades and empty forges.\"";
            }
        }

        public override object Complete
        {
            get
            {
                return "\"You did it. I knew from the moment you stepped in, you had the steel. The roads are safe, the howls gone, and my forge will burn bright once more.\n\n" +
                       "Take this, the *Sea Serpent’s Scarf*. Woven from the threads of shipwrecked sails, they say it shields against both wind and fire. Wear it well, friend.\"";
            }
        }

        public HowlOfTheNightQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(BlackWolf), "Black Wolf", 1));
            AddReward(new BaseReward(typeof(SeaSerpentsScarf), 1, "Sea Serpent's Scarf"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Howl of the Night'!");
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

    public class CorvinIronfoot : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(HowlOfTheNightQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBBlacksmith());
        }

        [Constructable]
        public CorvinIronfoot()
            : base("the Blacksmith", "Corvin Ironfoot")
        {
        }

        public CorvinIronfoot(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(90, 80, 50);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = 1108; // Weathered, tanned skin
            HairItemID = 0x2049; // Long Hair
            HairHue = 1107; // Ash-black
            FacialHairItemID = 0x204B; // Full beard
            FacialHairHue = 1107;
        }

        public override void InitOutfit()
        {
            AddItem(new LeatherDo() { Hue = 1819, Name = "Forge-Singed Tunic" }); // Dark ember brown
            AddItem(new StuddedLegs() { Hue = 2101, Name = "Ironstrider Greaves" }); // Soot-gray
            AddItem(new LeatherGloves() { Hue = 2208, Name = "Anvil-Hardened Gloves" }); // Burnished bronze
            AddItem(new FullApron() { Hue = 1844, Name = "Coal-Stained Apron" }); // Deep charcoal
            AddItem(new Boots() { Hue = 1812, Name = "Forge-Treader Boots" }); // Black leather
            AddItem(new SmithSmasher() { Hue = 2424, Name = "Corvin’s Hammer" }); // Dull steel, work-worn

            Backpack backpack = new Backpack();
            backpack.Hue = 1153;
            backpack.Name = "Ironworker's Pack";
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
