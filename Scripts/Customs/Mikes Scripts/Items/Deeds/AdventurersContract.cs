using System;
using Server.Targeting;
using Server.Mobiles;

namespace Server.Items
{
    public class AdventurersContract : Item
    {
        [Constructable]
        public AdventurersContract() : base(0x14F0)  // Base item ID for a deed/scroll
        {
            Name = "Adventurer's Contract";
            Hue = 0x58B;  // You can adjust this value for a different color
        }

        public AdventurersContract(Serial serial) : base(serial) { }

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

        public override void OnDoubleClick(Mobile from)
        {
            if (from.Followers + 1 > from.FollowersMax)
            {
                from.SendMessage("You have too many followers to use this contract.");
            }
            else
            {
                from.BeginTarget(-1, false, TargetFlags.None, new TargetCallback(OnTarget));
                from.SendMessage("Select a human NPC to recruit as your follower.");
            }
        }

        public void OnTarget(Mobile from, object targeted)
        {
            if (targeted is BaseCreature && ((BaseCreature)targeted).Body.IsHuman)
            {
                BaseCreature target = (BaseCreature)targeted;
                
                if (!target.Controlled)
                {
                    target.Controlled = true;
                    target.ControlMaster = from;
                    target.ControlTarget = from;
                    target.ControlOrder = OrderType.Follow;
                    target.BondingBegin = DateTime.Now;
                    target.OwnerAbandonTime = DateTime.Now + TimeSpan.FromDays(1.0); // They can be abandoned in a day

                    from.SendMessage("You have successfully recruited the adventurer.");
                    this.Delete();  // Delete the contract after successful use
                }
                else
                {
                    from.SendMessage("That human is already controlled.");
                }
            }
            else
            {
                from.SendMessage("You can only recruit human NPCs with this contract.");
            }
        }
    }
}
