using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;

namespace Server.Engines.Quests
{
    public class BravaCoil : MondainQuester
    {
        [Constructable]
        public BravaCoil()
            : base("Brava Coil", "Mine Forewoman")
        {
        }

        public BravaCoil(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 25);

            Female = true;
            Race = Race.Human;

            Hue = Utility.RandomSkinHue();
            HairItemID = 0x2049;
            HairHue = Utility.RandomHairHue();
        }

        public override void InitOutfit()
        {
            AddItem(new Pickaxe());
            AddItem(new LeatherGloves());
            AddItem(new LeatherChest());
            AddItem(new LeatherLegs());
            AddItem(new Boots());
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(ClearingTheTunnelsQuest)
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
