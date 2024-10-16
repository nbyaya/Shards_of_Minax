using System;
using Server.Mobiles;

namespace Server.Mobiles
{
    public class GoldenDragon : BaseCreature
    {
        public GoldenDragon() 
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Golden Dragon";
            Body = 12; // Use the dragon's body ID
            Hue = 0x8A5; // Gold hue

            // You can adjust stats, skills, etc. here
        }

        public GoldenDragon(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
