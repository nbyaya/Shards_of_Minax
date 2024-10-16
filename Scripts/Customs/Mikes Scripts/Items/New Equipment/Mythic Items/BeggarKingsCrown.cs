using Server;
using Server.Items;
using Server.Targeting;
using Server.Mobiles;

public class BeggarKingsCrown : Item
{
    [Constructable]
    public BeggarKingsCrown() : base(0x9AC)
    {
        Name = "Beggar King's Crown";
    }

    public BeggarKingsCrown(Serial serial) : base(serial)
    {
    }

    public override void OnDoubleClick(Mobile from)
    {
        if (from.Skills.Begging.Value < 50)
        {
            from.SendMessage("You need at least 50 points in begging to use this.");
            return;
        }

        from.SendMessage("Select an NPC to make your pet.");
        from.Target = new InternalTarget(this);
    }

    private class InternalTarget : Target
    {
        private BeggarKingsCrown m_Crown;

        public InternalTarget(BeggarKingsCrown crown) : base(10, false, TargetFlags.None)
        {
            m_Crown = crown;
        }

        protected override void OnTarget(Mobile from, object targeted)
        {
            if (targeted is BaseCreature)
            {
                BaseCreature creature = (BaseCreature)targeted;

                if (from.Followers + creature.ControlSlots <= from.FollowersMax)
                {
                    creature.Controlled = true;
                    creature.ControlMaster = from;
                    creature.IsBonded = true; // or false, depending on your server's rules
                    from.SendMessage("You have made the NPC your follower.");
                }
                else
                {
                    from.SendMessage("You have too many followers to control this NPC.");
                }
            }
            else
            {
                from.SendMessage("This is not a valid target.");
            }
        }
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
