using System;
using Server.Items;
using Server.Network;

namespace Server.Items
{
    public class BootsofHaste : Boots
    {
        public override bool IsArtifact => true;

        // Internal field to manage speed boost
        private bool m_SpeedBoost = true;

        [Constructable]
        public BootsofHaste()
        {
            Hue = 0x455;
            Name = "Boots of Speed";
        }

        public BootsofHaste(Serial serial) : base(serial)
        {
        }

        public override int InitMinHits => 150;
        public override int InitMaxHits => 150;
        public override bool CanFortify => false;

        // Public property to get the SpeedBoost value
        public bool SpeedBoost
        {
            get { return m_SpeedBoost; }
            set { m_SpeedBoost = value; }
        }

        public override bool OnEquip(Mobile from)
        {
            if (!base.OnEquip(from))
                return false;

            // Apply speed control if SpeedBoost is active
            if (m_SpeedBoost)
            {
                from.SendMessage("You feel yourself moving faster!");
                from.SendSpeedControl(SpeedControlType.MountSpeed); // Apply increased speed
            }

            return true; // Successfully equipped
        }

        public override void OnRemoved(object parent)
        {
            if (parent is Mobile from)
            {
                // Revert speed control when boots are removed
                if (m_SpeedBoost)
                {
                    from.SendMessage("Your speed returns to normal.");
                    from.SendSpeedControl(SpeedControlType.Disable); // Reset to normal running speed
                }
            }

            base.OnRemoved(parent);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);  // Version number

            // Save the SpeedBoost state
            writer.Write(m_SpeedBoost);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Load the SpeedBoost state
            m_SpeedBoost = reader.ReadBool();
        }
    }
}