using System;
using Server.Network;
using Server.Mobiles;
using Server.Items;
using Server.Gumps;
using Server.Targeting;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class BlacksmithingCatalyst : LevelUpScroll
    {
        [Constructable]
        public BlacksmithingCatalyst(int value) : base(value)
        {
            Name = "Blacksmithing Catalyst";
            Hue = 0x497; // Optional unique hue
			ItemID = 0x0F91;
        }

		[Constructable]
		public BlacksmithingCatalyst() : this(1) 
		{
			// defaults to a +1 level scroll; change “1” to whatever you like
		}


        public BlacksmithingCatalyst(Serial serial) : base(serial) { }

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
                return;
            }

            if (BlacksmithValidated || from.Skills[SkillName.Blacksmith].Value >= LevelItems.BlacksmithSkillRequired)
            {
                from.SendMessage("Which weapon or armor would you like to level up?");
                from.Target = new BlacksmithLevelItemTarget(this);
            }
            else
            {
                from.SendMessage("Please target another player with a base Blacksmith skill of " + LevelItems.BlacksmithSkillRequired + " or higher.");
                from.Target = new BlacksmithTarget(this);
            }
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("a Blacksmithing Catalyst (+{0} level)", Value);
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "a blacksmithing scroll of Leveling (+{0} level)", Value);
        }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);

            string status = BlacksmithValidated
                ? "(Blacksmith Validated)"
                : $"(Must be validated by player with {LevelItems.BlacksmithSkillRequired}+ Blacksmithy skill)";

            list.Add(1060847, "Weapons and Armor Only\t{0}", status);
        }

        private class BlacksmithLevelItemTarget : Target
        {
            private readonly BlacksmithingCatalyst m_Scroll;

            public BlacksmithLevelItemTarget(BlacksmithingCatalyst scroll) : base(-1, false, TargetFlags.None)
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

                    if (!(item is BaseWeapon || item is BaseArmor))
                    {
                        from.SendMessage("This scroll only works on weapons and armor!");
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
                    from.SendMessage("That is not a valid item.");
                }
            }
        }

        private class BlacksmithTarget : Target
        {
            private readonly BlacksmithingCatalyst m_Scroll;

            public BlacksmithTarget(BlacksmithingCatalyst scroll) : base(-1, false, TargetFlags.None)
            {
                m_Scroll = scroll;
            }

            protected override void OnTarget(Mobile from, object target)
            {
                if (target is PlayerMobile player)
                {
                    if (player.Skills[SkillName.Blacksmith].Value < LevelItems.BlacksmithSkillRequired)
                    {
                        from.SendMessage("This player's blacksmith skill is not high enough.");
                    }
                    else
                    {
                        from.SendMessage("This player is a valid blacksmith.");
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
