using System;
using Server.Network;
using Server.Mobiles;
using Server.Items;
using Server.Gumps;
using Server.Targeting;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class TinkeringCatalyst : LevelUpScroll
    {
        [Constructable]
        public TinkeringCatalyst(int value) : base(value)
        {
            Name = "Tinkering Catalyst";
            Hue = 0x8A5; // Unique color for Tinkering
			ItemID = 0x0F91;
        }

		[Constructable]
		public TinkeringCatalyst() : this(1) 
		{
			// defaults to a +1 level scroll; change “1” to whatever you like
		}


        public TinkeringCatalyst(Serial serial) : base(serial) { }

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
                return;
            }

            if (BlacksmithValidated || (from.Skills[SkillName.Tinkering].Value >= LevelItems.BlacksmithSkillRequired))
            {
                from.SendMessage("Which piece of jewelry would you like to level up?");
                from.Target = new TinkeringLevelItemTarget(this);
            }
            else
            {
                from.SendMessage("Please target another player with a base Tinkering skill of " + LevelItems.BlacksmithSkillRequired + " or higher.");
                from.Target = new TinkeringSkillTarget(this);
            }
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("a Tinkering Catalyst (+{0} level)", Value);
        }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            string message = BlacksmithValidated
                ? "(Tinkering Validated)"
                : $"(Must be validated by player with {LevelItems.BlacksmithSkillRequired}+ Tinkering skill)";
            list.Add(1060847, "Jewelry Only\t{0}", message);
        }

        private class TinkeringLevelItemTarget : Target
        {
            private readonly TinkeringCatalyst m_Scroll;

            public TinkeringLevelItemTarget(TinkeringCatalyst scroll) : base(-1, false, TargetFlags.None)
            {
                m_Scroll = scroll;
            }

            protected override void OnTarget(Mobile from, object target)
            {
                if (target is BaseJewel jewel)
                {
                    if (jewel.RootParent != from || !jewel.IsChildOf(from.Backpack))
                    {
                        from.SendMessage("The item must be in your pack to level it up.");
                        return;
                    }

                    XmlLevelItem levitem = XmlAttach.FindAttachment(jewel, typeof(XmlLevelItem)) as XmlLevelItem;

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
                            from.SendMessage($"Your item has leveled up by {m_Scroll.Value} levels.");
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
                    from.SendMessage("This scroll only works on jewelry items.");
                }
            }
        }

        private class TinkeringSkillTarget : Target
        {
            private readonly TinkeringCatalyst m_Scroll;

            public TinkeringSkillTarget(TinkeringCatalyst scroll) : base(-1, false, TargetFlags.None)
            {
                m_Scroll = scroll;
            }

            protected override void OnTarget(Mobile from, object target)
            {
                if (target is PlayerMobile player)
                {
                    if (player.Skills[SkillName.Tinkering].Value < LevelItems.BlacksmithSkillRequired)
                    {
                        from.SendMessage("This player's tinkering skill is not high enough.");
                    }
                    else
                    {
                        from.SendMessage("This player is a valid tinkerer.");
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
