using System;
using Server;
using Server.Items;
using Server.Targeting;

public class MagicMasterKey : Item
{
    [Constructable]
    public MagicMasterKey() : base(0x100E)
    {
        Name = "Master Key";
        Weight = 1.0;
    }

    public MagicMasterKey(Serial serial) : base(serial)
    {
    }

    public override void OnDoubleClick(Mobile from)
    {
        if (from.Skills[SkillName.Lockpicking].Value < 125)
        {
            from.SendMessage("You must have a lockpicking skill of 125 to use this.");
            return;
        }

        from.SendMessage("Select a container to unlock.");
        from.Target = new UnlockTarget(this);
    }

    private class UnlockTarget : Target
    {
        private MagicMasterKey m_Key;

        public UnlockTarget(MagicMasterKey key) : base(-1, false, TargetFlags.None)
        {
            m_Key = key;
        }

        protected override void OnTarget(Mobile from, object targeted)
        {
            if (targeted is LockableContainer)
            {
                LockableContainer container = (LockableContainer)targeted;

                if (container.Locked)
                {
                    container.Locked = false;
                    from.SendMessage("You unlock the container.");
                }
                else
                {
                    from.SendMessage("That is already unlocked.");
                }
            }
            else
            {
                from.SendMessage("That is not a lockable container.");
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

