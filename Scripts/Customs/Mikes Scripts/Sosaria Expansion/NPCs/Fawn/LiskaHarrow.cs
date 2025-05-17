using System;
using Server;
using Server.Mobiles;
using Server.Engines.Quests;
using Server.Items;

namespace Server.Mobiles
{
    public class LiskaHarrow : MondainQuester
    {
        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBJewel()); 
        }        
		
		[Constructable]
        public LiskaHarrow() : base("Ghost Whisperer", "Liska Harrow")
        {
        }

        public LiskaHarrow(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(60, 75, 90);
            Body = 0x191;
            Hue = 0x47E; // Pale violet
            HairItemID = 0x2049;
            HairHue = 1153;
        }

        public override void InitOutfit()
        {
            AddItem(new Robe(1154));
            AddItem(new Sandals(0x455));
            AddItem(new Cloak(0x47E));
            AddItem(new GnarledStaff { Name = "Whispering Rod", Hue = 0x480 });
        }

        public override Type[] Quests => new Type[] { typeof(VoicesOfTheVeinQuest) };

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
