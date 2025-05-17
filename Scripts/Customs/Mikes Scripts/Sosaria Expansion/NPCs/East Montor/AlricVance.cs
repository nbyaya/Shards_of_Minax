using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;

namespace Server.Engines.Quests
{
    public class AlricVance : MondainQuester
    {
        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBSpearForkWeapon()); 
        }        
		
		[Constructable]
        public AlricVance()
            : base("Relic Broker", "Alric Vance")
        {
        }

        public AlricVance(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(80, 80, 60);
            Body = 400;
            Hue = 33770; // Slightly cursed-looking pallor
            HairItemID = 0x203B;
            HairHue = 1150;
        }

        public override void InitOutfit()
        {
            AddItem(new FancyShirt(1109));
            AddItem(new LongPants(0x455));
            AddItem(new Boots(1109));
            AddItem(new Cloak(1152)); // Hinting at his cursed affliction
            AddItem(new Spellbook { Name = "Ledger of the Exodus", Hue = 1152 });

            Backpack pack = new Backpack();
            pack.Name = "Relic Satchel";
            AddItem(pack);
        }

        public override Type[] Quests => new Type[]
        {
            typeof(DustOfExodusQuest)
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
