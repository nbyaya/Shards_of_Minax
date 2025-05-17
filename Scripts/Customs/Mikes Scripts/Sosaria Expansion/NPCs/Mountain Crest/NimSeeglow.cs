using System;
using Server.Mobiles;
using Server.Items;

namespace Server.Engines.Quests
{
    public class NimSeeglow : MondainQuester
    {
        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBInnKeeper()); 
        }        
		
		[Constructable]
        public NimSeeglow() : base("Nim Seeglow", "Frostmancer Hermit")
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 100);
            Female = true;
            Race = Race.Human;
            Hue = 0x83F;

            HairItemID = 0x203C;
            HairHue = 1152;
        }

        public override void InitOutfit()
        {
            AddItem(new Robe { Hue = 1150 });
            AddItem(new Sandals { Hue = 1152 });
            AddItem(new WizardsHat { Hue = 1150 });
        }

        public override Type[] Quests => new Type[] { typeof(WailNoMoreQuest) };

        public NimSeeglow(Serial serial) : base(serial) { }

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
