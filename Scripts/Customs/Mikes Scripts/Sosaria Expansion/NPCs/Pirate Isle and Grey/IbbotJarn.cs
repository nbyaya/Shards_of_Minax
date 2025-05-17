using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Engines.Quests
{
    public class IbbotJarn : MondainQuester
    {
        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBCarpenter()); 
        }
		
		[Constructable]
        public IbbotJarn() : base("Mushroom Brewer", "Ibbot 'Muckbeard' Jarn")
        {
        }

        public override void InitBody()
        {
            InitStats(50, 60, 80);
            Body = 0x190; // Male
            Hue = 33789;
            HairItemID = 0x2049;
            HairHue = 2101;
            FacialHairItemID = 0x203C;
            FacialHairHue = 2101;
        }

        public override void InitOutfit()
        {
            AddItem(new Robe(2213)); // Muted orange-brown hue
            AddItem(new Sandals(2309));
            AddItem(new WizardsHat(0x845));
            AddItem(new Backpack());

            var bottle = new Bottle { Name = "Muckbeard’s Brew", Hue = 0x488 };
            AddItem(bottle);
        }

        public override Type[] Quests => new Type[]
        {
            typeof(FungusAmongUsQuest)
        };

        public IbbotJarn(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt();
        }
    }
}
