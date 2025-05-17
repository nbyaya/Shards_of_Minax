using System;
using Server;
using Server.Items;
using Server.Multis;
using Server.Multis.Deeds;

namespace Server.Multis
{
    public class NewTwoStoryBrickHouse : BaseHouse
    {
        // Same area you used in the original code
        public static Rectangle2D[] AreaArray = new Rectangle2D[]
        {
            new Rectangle2D(-5, -5, 11, 12)
        };

        // Constructor in the "new style"
        public NewTwoStoryBrickHouse(Mobile owner)
            : base(0x4E, owner, 1370, 10) // <--- Adjust the lockdowns/secures as you wish
        {
            uint keyValue = CreateKeys(owner);

            // If you want *double* south-facing doors at (1,5,5):
            AddSouthDoors(1, 5, 5, keyValue);

            // Place the house sign near (-4,5,0). You can adjust Z if needed:
            SetSign(-4, 5, 0);
        }

        // Deserialization constructor
        public NewTwoStoryBrickHouse(Serial serial)
            : base(serial)
        {
        }

        // Override the house area
        public override Rectangle2D[] Area
        {
            get { return AreaArray; }
        }

        // Where players are sent if they are banned/ejected
        public override Point3D BaseBanLocation
        {
            get { return new Point3D(-4, 5, 0); }
        }

        // The “classic” price of the building if you want one
        public override int DefaultPrice
        {
            get { return 192400; } // Adjust to whatever price you like
        }

        // If you’re allowing conversion to custom foundations in House Customization
        public override HousePlacementEntry ConvertEntry
        {
            get { return HousePlacementEntry.ThreeStoryFoundations[37]; }
        }

        // If the foundation shifts slightly in the Y direction
        public override int ConvertOffsetY
        {
            get { return -1; }
        }

        // Return our custom deed for redeeding
        public override HouseDeed GetDeed()
        {
            return new NewTwoStoryBrickHouseDeed();
        }

        // Serialization
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        // Deserialization
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
