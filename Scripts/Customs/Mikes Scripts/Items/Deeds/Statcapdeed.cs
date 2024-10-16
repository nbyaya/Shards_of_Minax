using System;
using Server;
using Server.Network;
using Server.Prompts;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Items
{
    public class StatCapTarget : Target
    {
        private StatCapDeed m_Deed;

        public StatCapTarget(StatCapDeed deed) : base(1, false, TargetFlags.None)
        {
            m_Deed = deed;
        }

        protected override void OnTarget(Mobile from, object target)
        {
            if (!(target is PlayerMobile))
            {
                from.SendMessage("You can only use this on a player.");
                return;
            }

            PlayerMobile player = (PlayerMobile)target;
            
            if (m_Deed.Deleted || m_Deed.RootParent != from)
            {
                from.SendMessage("An error occurred.");
                return;
            }

            player.StatCap += 50;
            from.SendMessage("You've successfully increased the stat cap by 50!");
            m_Deed.Delete();
        }
    }

    public class StatCapDeed : Item
    {
        [Constructable]
        public StatCapDeed() : base(0x14F0)
        {
            Name = "a +50 stat cap deed";
            Hue = 1152;
            Weight = 1.0;
        }

        public StatCapDeed(Serial serial) : base(serial)
        {
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

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendMessage("The deed must be in your backpack to use.");
                return;
            }
            
            from.SendMessage("Whose stat cap would you like to increase?");
            from.Target = new StatCapTarget(this);
        }
    }
}
