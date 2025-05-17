using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class AlphasEndQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Alpha’s End"; } }

        public override object Description
        {
            get
            {
                return
                    "You stand before *Riven Stormcloak*, renowned Beast Tamer of Moon’s wild outskirts.\n\n" +
                    "“I’ve spent my life taming creatures born of the sands, but now the *Dune Wolves* threaten everything. Their pack leader mimics the sacred hymns of the Moon—it drives the beasts I train into a frenzy.\n\n" +
                    "That Alpha must fall.\n\n" +
                    "**Hunt the Dune Wolf Alpha** and end its reign over the borderlands.";
            }
        }

        public override object Refuse { get { return "Then stay clear of the wilds, friend. The wolves are merciless, and the Moon’s pull grows ever stronger."; } }

        public override object Uncomplete { get { return "Still it lives? My beasts suffer while that monster howls. Put an end to it."; } }

        public override object Complete { get { return "It is done? The dunes will rest easy tonight... and so will my creatures. Take this—crafted to shield you from storm and sand alike."; } }

        public AlphasEndQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(DuneWolf), "Dune Wolf", 1));
            AddReward(new BaseReward(typeof(RainfoldOvercoat), 1, "RainfoldOvercoat"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Alpha’s End'!");
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

    public class RivenStormcloak : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(AlphasEndQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBAnimalTrainer()); 
        }

        [Constructable]
        public RivenStormcloak()
            : base("the Beast Tamer", "Riven Stormcloak")
        {
        }

        public RivenStormcloak(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(90, 85, 30);

            Female = false;
            Body = 0x190; // Male Body
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = 0x2049; // Short Hair
            HairHue = 1109; // Ash Grey
            FacialHairItemID = 0x203B; // Short Beard
            FacialHairHue = 1109;
        }

        public override void InitOutfit()
        {
            // Unique Outfit for Riven Stormcloak
            AddItem(new StuddedChest() { Hue = 1818, Name = "Stormcloak's Hide Vest" }); // Sandy leather tone
            AddItem(new LeatherLegs() { Hue = 1818, Name = "Dune-Walker Trousers" });
            AddItem(new Cloak() { Hue = 2419, Name = "Mantle of the Sand Warden" }); // Desert gold hue
            AddItem(new LeatherGloves() { Hue = 1820, Name = "Tamer’s Grip" });
            AddItem(new Sandals() { Hue = 2418, Name = "Windswept Sandals" });
            AddItem(new BearMask() { Hue = 1846, Name = "Wolf’s Gaze Mask" }); // Faded bone-white
            AddItem(new ShepherdsCrook() { Hue = 1109, Name = "Crook of the Beastmaster" });

            Backpack backpack = new Backpack();
            backpack.Hue = 44;
            backpack.Name = "Tamer's Pack";
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
