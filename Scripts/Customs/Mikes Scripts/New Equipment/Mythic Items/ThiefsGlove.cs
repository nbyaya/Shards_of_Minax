using System;
using Server;
using Server.Items;
using Server.Targeting;

namespace Server.Items
{
    public class ThiefsGlove : Item
    {
        public override int LabelNumber { get { return 1070921; } } // Thief's Gloves

        [Constructable]
        public ThiefsGlove()
            : base(0x13C6)
        {
            Hue = 0x455;
			Weight = 1.0;
			Name = "Master Thiefs Glove";
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from.Skills[SkillName.Stealing].Value < 50)
            {
                from.SendMessage("You need at least 50 stealing skill to use this.");
                return;
            }

            from.Target = new StealTarget(from, this);
        }

        private class StealTarget : Target
        {
            private Mobile m_Thief;
            private ThiefsGlove m_Glove;

            public StealTarget(Mobile thief, ThiefsGlove glove)
                : base(1, false, TargetFlags.None)
            {
                m_Thief = thief;
                m_Glove = glove;
            }

            protected override void OnTarget(Mobile from, object target)
            {
                if (target is Item)
                {
                    Item item = (Item)target;

                    if (!item.Movable)
                    {
                        from.SendMessage("You cannot steal that.");
                        return;
                    }

                    Container pack = from.Backpack;

                    if (pack != null && pack.TryDropItem(from, item, false))
                    {
                        from.SendMessage("You successfully steal the item.");
                    }
                    else
                    {
                        from.SendMessage("You fail to steal the item.");
                    }
                }
                else
                {
                    from.SendMessage("You can't steal that!");
                }
            }
        }

        public ThiefsGlove(Serial serial)
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
