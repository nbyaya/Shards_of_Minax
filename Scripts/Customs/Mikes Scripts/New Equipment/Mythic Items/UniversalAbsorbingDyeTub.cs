using System;
using Server.Targeting;
using Server.Mobiles;
using Server.Items;
using Server;

namespace Server.Items
{
    public class UniversalAbsorbingDyeTub : Item
    {
        private int m_AbsorbedHue;

        [Constructable]
        public UniversalAbsorbingDyeTub() : base(0xFAB)
        {
            m_AbsorbedHue = 0;
			Name = "Universal Absorbing Dye Tub";  // Set the name of the item
        }

        public UniversalAbsorbingDyeTub(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
            writer.Write((int)m_AbsorbedHue);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
            m_AbsorbedHue = reader.ReadInt();
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (m_AbsorbedHue == 0)
            {
                from.SendMessage("Target the item or mobile to absorb its hue.");
                from.Target = new AbsorbHueTarget(this);
            }
            else
            {
                from.SendMessage("Target the item or mobile to dye.");
                from.Target = new DyeTarget(this);
            }
        }

// ... (Previous code remains unchanged)

		private class AbsorbHueTarget : Target
		{
			private UniversalAbsorbingDyeTub m_Tub;

			public AbsorbHueTarget(UniversalAbsorbingDyeTub tub) : base(1, false, TargetFlags.None)
			{
				m_Tub = tub;
			}

			protected override void OnTarget(Mobile from, object targeted)
			{
				if (targeted is Item)
				{
					Item item = (Item)targeted;
					m_Tub.m_AbsorbedHue = item.Hue;
					m_Tub.Hue = item.Hue;  // Set the Dye Tub hue to match the item's hue
				}
				else if (targeted is Mobile)
				{
					Mobile mob = (Mobile)targeted;
					m_Tub.m_AbsorbedHue = mob.Hue;
					m_Tub.Hue = mob.Hue;  // Set the Dye Tub hue to match the mobile's hue
				}
				from.SendMessage("Hue absorbed.");
			}
		}

// ... (Previous code remains unchanged)


// ... (Previous code remains unchanged)

		private class DyeTarget : Target
		{
			private UniversalAbsorbingDyeTub m_Tub;

			public DyeTarget(UniversalAbsorbingDyeTub tub) : base(1, false, TargetFlags.None)
			{
				m_Tub = tub;
			}

			protected override void OnTarget(Mobile from, object targeted)
			{
				if (targeted is Item)
				{
					Item item = (Item)targeted;
					item.Hue = m_Tub.m_AbsorbedHue;
				}
				else if (targeted is Mobile)
				{
					Mobile mob = (Mobile)targeted;
					mob.Hue = m_Tub.m_AbsorbedHue;
				}

				m_Tub.m_AbsorbedHue = 0; // Reset absorbed hue to initial state
				m_Tub.Hue = 0;  // Reset the Dye Tub hue to neutral
				from.SendMessage("Item dyed.");
			}
		}

// ... (Previous code remains unchanged)

    }
}
