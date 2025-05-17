using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Engines.Quests
{
    public class JothanReeve : MondainQuester
    {
        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBFisherman()); 
        }        
		
		
		[Constructable]
        public JothanReeve() : base("Jothan Reeve", "Relic Hoarder")
        {
        }

        public JothanReeve(Serial serial) : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(75, 60, 90);
            Female = false;
            Race = Race.Human;

            Hue = 0x83EA;
            HairItemID = 0x203B;
            HairHue = 1150;
            FacialHairItemID = 0x204D;
            FacialHairHue = HairHue;
        }

        public override void InitOutfit()
        {
            AddItem(new HoodedShroudOfShadows());
            AddItem(new Sandals());
            AddItem(new Robe(1109));
            AddItem(new Pouch());
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(ShatterTheAnointedQuest)
                };
            }
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
