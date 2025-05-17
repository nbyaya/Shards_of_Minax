using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class BoneAndClayQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Bone and Clay"; } }

        public override object Description
        {
            get
            {
                return
                    "You approach *Bronn Sandshaper*, Artisan of Moon's Reflecting Pool.\n\n" +
                    "“Do you see these statues? My life’s work—each grain of sand, each curve of marble, shaped by my hands.\n\n" +
                    "Yet out there, near *The Pyramid*, a grotesque mockery sculpts abominations. The *Mummified Beast* twists sand and bone into nightmares.\n\n" +
                    "I cannot abide its existence.\n\n" +
                    "**Destroy the Mummified Beast** and let true artistry prevail.”";
            }
        }

        public override object Refuse { get { return "Then the sands will remain tainted, and art will suffer. I had hoped for better."; } }

        public override object Uncomplete { get { return "The Beast still molds terror from sand? End its reign before my own creations crumble from despair."; } }

        public override object Complete { get { return "You’ve done it. The sands are freed. Accept this—a token from a fellow creator, born of passion and wrath."; } }

        public BoneAndClayQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(MummifiedBeast), "Mummified Beast", 1));

            AddReward(new BaseReward(typeof(WaylonsLastLaugh), 1, "WaylonsLastLaugh"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Bone and Clay'!");
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

    public class BronnSandshaper : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(BoneAndClayQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBBeekeeper()); 
        }

        [Constructable]
        public BronnSandshaper()
            : base("the Sculptor of Sands", "Bronn Sandshaper")
        {
        }

        public BronnSandshaper(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(90, 70, 35);

            Female = false;
            Body = 0x190; // Male Body
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = 0x203C; // Long Hair
            HairHue = 1150; // Pale silver
            FacialHairItemID = 0x2041; // Short Beard
            FacialHairHue = 1150;
        }

        public override void InitOutfit()
        {
            AddItem(new FancyShirt() { Hue = 2413, Name = "Sandshaper's Tunic" }); // Warm sandstone hue
            AddItem(new FancyKilt() { Hue = 2101, Name = "Dune-Walker’s Kilt" }); // Deep gold
            AddItem(new BodySash() { Hue = 2418, Name = "Sash of the Sculptor" }); // Coppery-brown
            AddItem(new Sandals() { Hue = 2111, Name = "Marble-Dusted Sandals" }); // Dusty grey
            AddItem(new Cloak() { Hue = 2411, Name = "Cloak of Shifting Sands" }); // Faint ochre
            AddItem(new Scythe() { Hue = 2405, Name = "Chisel of Wrath" }); // Styled like an artisan’s massive sculpting tool

            Backpack backpack = new Backpack();
            backpack.Hue = 44;
            backpack.Name = "Artisan's Pack";
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
