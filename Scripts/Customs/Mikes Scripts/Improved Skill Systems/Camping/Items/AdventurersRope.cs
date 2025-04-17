using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Targeting; // Necessary for Targeting
using Server.Mobiles;

namespace Server.Items
{
    public class AdventurersRope : Item
    {
        [Constructable]
        public AdventurersRope() : base(0x14F8) // Use the appropriate item ID
        {
            Name = "Adventurer's Rope";
            Weight = 1.0;
            LootType = LootType.Regular;
        }

        public AdventurersRope(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int) 0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
		
		public override void OnDoubleClick(Mobile from)
		{
			if (from is PlayerMobile)
			{
				PlayerMobile pm = (PlayerMobile)from;

				if (pm.Skills[SkillName.Camping].Value < 50) // Properly checks the Camping skill
				{
					pm.SendMessage("You must have at least 50 points in camping to use this.");
				}
				else
				{
					pm.SendMessage("Where would you like to teleport?");
					pm.BeginTarget(12, true, TargetFlags.None, new TargetCallback(OnTarget)); // Correct invocation of BeginTarget
				}
			}
		}


        private void OnTarget(Mobile from, object targeted)
        {
            IPoint3D p = targeted as IPoint3D;
            
            if (p != null && from.Map != null)
            {
                Point3D loc = new Point3D(p);

                // Assuming direct teleport without considering spell restrictions for simplicity
                from.MoveToWorld(loc, from.Map);
                from.SendMessage("You use the rope and find yourself in a new location!");

                this.Delete(); // Deletes the item after use
            }
        }
    }
}
