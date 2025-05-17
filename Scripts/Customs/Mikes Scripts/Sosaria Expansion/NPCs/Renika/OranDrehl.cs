using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;

namespace Server.Engines.Quests
{
    public class OranDrehl : MondainQuester
    {
        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBKnifeWeapon()); 
        }        
		
		[Constructable]
        public OranDrehl() : base("Historian of Britain", "Oran Drehl")
        {
        }

        public OranDrehl(Serial serial) : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(70, 70, 90);
            Body = 0x190;
            Hue = 33773;
            HairItemID = 0x2049;
            HairHue = 1150;
        }

        public override void InitOutfit()
        {
            AddItem(new Robe(0x497));
            AddItem(new Sandals(0x1BB));
            AddItem(new WizardsHat(1152));
            AddItem(new Spellbook { Name = "Tome of Forgotten Scripts", Hue = 1152 });
        }

        public override Type[] Quests => new Type[]
        {
            typeof(RunesOfTheArchonsQuest)
        };

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
