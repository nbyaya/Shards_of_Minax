using System;
using Server;
using Server.Items;
using Server.Engines.Quests;

namespace Server.Mobiles
{
    public class EiraSnowmantle : MondainQuester
    {
        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBProvisioner()); 
        }        
		
		[Constructable]
        public EiraSnowmantle() : base("Crystal Sculptor", "Eira Snowmantle") { }

        public override void InitBody()
        {
            InitStats(70, 80, 90);
            Body = 401;
            Hue = 1150; // Pale frost-toned skin
            HairItemID = 0x203B;
            HairHue = 0x47F;
        }

        public override void InitOutfit()
        {
            AddItem(new Robe { Hue = 0x480, Name = "Ice-Threaded Robe" });
            AddItem(new Sandals { Hue = 0x47F });
            AddItem(new HalfApron { Hue = 0x46A, Name = "Crystalbelt" });

            Backpack pack = new Backpack();
            pack.Name = "Eira's Tools";
            AddItem(pack);
        }

        public override Type[] Quests => new Type[]
        {
            typeof(HeartOfTheGlacierQuest)
        };

        public EiraSnowmantle(Serial serial)
            : base(serial)
        {
        }

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
