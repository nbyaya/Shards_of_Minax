using System;
using Server.Network;
using Server.Mobiles;
using Server.Items;
using Server.Gumps;
using Server.Targeting;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class TailoringCatalyst : LevelUpScroll
    {
        [Constructable]
        public TailoringCatalyst(int value) : base(value)
        {
            Name = "Tailoring Catalyst";
            Hue = 0x8A4; // Optional: Unique hue for tailoring
			ItemID = 0x0F91;
        }

		[Constructable]
		public TailoringCatalyst() : this(1) 
		{
			// defaults to a +1 level scroll; change “1” to whatever you like
		}

        public TailoringCatalyst(Serial serial) : base(serial) { }

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
                return;
            }

            if (BlacksmithValidated || (from.Skills[SkillName.Tailoring].Value >= LevelItems.BlacksmithSkillRequired))
            {
                from.SendMessage("Which clothing item would you like to level up?");
                from.Target = new TailoringLevelItemTarget(this);
            }
            else
            {
                from.SendMessage("Please target another player with a base Tailoring skill of " + LevelItems.BlacksmithSkillRequired + " or higher.");
                from.Target = new TailorTarget(this);
            }
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("a Tailoring Catalyst (+{0} level)", Value);
        }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            string message = BlacksmithValidated
                ? "(Tailor Validated)"
                : $"(Must be validated by player with {LevelItems.BlacksmithSkillRequired}+ Tailoring skill)";
            list.Add(1060847, "Clothing Only\t{0}", message);
        }

        private class TailoringLevelItemTarget : Target
        {
            private readonly TailoringCatalyst m_Scroll;

            public TailoringLevelItemTarget(TailoringCatalyst scroll) : base(-1, false, TargetFlags.None)
            {
                m_Scroll = scroll;
            }

            protected override void OnTarget(Mobile from, object target)
            {
                if (target is Item item)
                {
                    if (item.RootParent != from || !item.IsChildOf(from.Backpack))
                    {
                        from.SendMessage("The item must be in your pack to level it up.");
                        return;
                    }

                    if (!(item is BaseClothing))
                    {
                        from.SendMessage("This scroll only works on clothing items.");
                        return;
                    }

                    XmlLevelItem levitem = XmlAttach.FindAttachment(item, typeof(XmlLevelItem)) as XmlLevelItem;

                    if (levitem != null)
                    {
                        if (levitem.Level + m_Scroll.Value > 100)
                        {
                            from.SendMessage("The level on this item is already too high to use this scroll!");
                        }
                        else
                        {
                            levitem.Level += m_Scroll.Value;
                            levitem.Points += m_Scroll.Value * 4;
                            from.SendMessage($"Your clothing item has leveled up by {m_Scroll.Value} levels.");
                            m_Scroll.Delete();
                        }
                    }
                    else
                    {
                        from.SendMessage("This item is not levelable.");
                    }
                }
                else
                {
                    from.SendMessage("That is not a valid item.");
                }
            }
        }

        private class TailorTarget : Target
        {
            private readonly TailoringCatalyst m_Scroll;

            public TailorTarget(TailoringCatalyst scroll) : base(-1, false, TargetFlags.None)
            {
                m_Scroll = scroll;
            }

            protected override void OnTarget(Mobile from, object target)
            {
                if (target is PlayerMobile player)
                {
                    if (player.Skills[SkillName.Tailoring].Value < LevelItems.BlacksmithSkillRequired)
                    {
                        from.SendMessage("This player's tailoring skill is not high enough.");
                    }
                    else
                    {
                        from.SendMessage("This player is a valid tailor.");
                        from.SendGump(new AwaitingSmithApprovalGump(m_Scroll, from)); // Reuse Gumps for approval
                        player.SendGump(new LevelUpAcceptGump(m_Scroll, from));
                    }
                }
                else
                {
                    from.SendMessage("Target another player.");
                }
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            // No new data needed
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
        }
    }
}
