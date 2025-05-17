using System;
using Server.Network;
using Server.Mobiles;
using Server.Items;
using Server.Gumps;
using Server.Targeting;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class FletchingCatalyst : LevelUpScroll
    {
        [Constructable]
        public FletchingCatalyst(int value) : base(value)
        {
            Name = "Fletching Catalyst";
            Hue = 0x59B; // Different hue to distinguish
			ItemID = 0x0F91;
        }

		[Constructable]
		public FletchingCatalyst() : this(1) 
		{
			// defaults to a +1 level scroll; change “1” to whatever you like
		}

        public FletchingCatalyst(Serial serial) : base(serial) { }

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
                return;
            }

            if (BlacksmithValidated || from.Skills[SkillName.Fletching].Value >= LevelItems.BlacksmithSkillRequired)
            {
                from.SendMessage("Which quiver would you like to level up?");
                from.Target = new FletchingLevelItemTarget(this);
            }
            else
            {
                from.SendMessage("Please target another player with a base Fletching skill of " + LevelItems.BlacksmithSkillRequired + " or higher.");
                from.Target = new FletchingSkillTarget(this);
            }
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("a Fletching Catalyst (+{0} level)", Value);
        }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            string message = BlacksmithValidated
                ? "(Tailor Validated)"
                : $"(Must be validated by player with {LevelItems.BlacksmithSkillRequired}+ Fletching skill)";
            list.Add(1060847, "Quivers Only\t{0}", message);
        }

        private class FletchingLevelItemTarget : Target
        {
            private readonly FletchingCatalyst m_Scroll;

            public FletchingLevelItemTarget(FletchingCatalyst scroll) : base(-1, false, TargetFlags.None)
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

                    if (!(item is BaseQuiver))
                    {
                        from.SendMessage("This scroll only works on quivers!");
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
                            from.SendMessage($"Your quiver has leveled up by {m_Scroll.Value} levels.");
                            m_Scroll.Delete();
                        }
                    }
                    else
                    {
                        from.SendMessage("This quiver is not levelable.");
                    }
                }
                else
                {
                    from.SendMessage("That is not a valid item.");
                }
            }
        }

        private class FletchingSkillTarget : Target
        {
            private readonly FletchingCatalyst m_Scroll;

            public FletchingSkillTarget(FletchingCatalyst scroll) : base(-1, false, TargetFlags.None)
            {
                m_Scroll = scroll;
            }

            protected override void OnTarget(Mobile from, object target)
            {
                if (target is PlayerMobile player)
                {
                    if (player.Skills[SkillName.Fletching].Value < LevelItems.BlacksmithSkillRequired)
                    {
                        from.SendMessage("This player's fletching skill is not high enough.");
                    }
                    else
                    {
                        from.SendMessage("This player is a valid fletcher.");
                        from.SendGump(new AwaitingSmithApprovalGump(m_Scroll, from));
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
            // No custom fields needed beyond base
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
        }		
    }
}
