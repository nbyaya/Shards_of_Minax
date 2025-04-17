using Server.Gumps;
using Server.Mobiles;
using Server.Network;
using System;
using Server.Items;
using Server.Multis;
using Server.Targeting;

namespace Server.Gumps
{
        public class OutpostGump : Gump
        {
            private readonly Mobile m_From;
	    private readonly OutpostCamp m_Camp;
	    private bool HasTent;

            public OutpostGump(Mobile from, OutpostCamp camp)
                : base(100, 0)
            {
                m_From = from;
		m_Camp = camp;

                //AddBackground(0, 0, 380, 380, 0xA28);
            	AddBackground(0, 0, 380, 380, 0x6DB);

		AddHtml(140, 20, 300, 35, FormatColor("#FFFFFF", "Secure Outpost"), false, false);
	        AddItem(80, 15, 0xFAC);
	        AddItem(80, 20, 0xDE3);


		AddHtml(50, 55, 300, 140, FormatColor("#FFFFFF", "Outposts are upgradable encampments that can be outfitted with various utilities that benefit all passing adventurers.<br><br>Any adventurer with enough skill in Camping can establish or upgrade these outposts."), false, false);

                AddButton(75, 208, 0xFA5, 0xFA7, 1, GumpButtonType.Reply, 0);
                AddHtml(110, 210, 110, 70, FormatColor("#FFFFFF", "MARK LOCATION"), false, false);

                AddButton(75, 233, 0xFA5, 0xFA7, 2, GumpButtonType.Reply, 0);
                AddHtml(110, 235, 110, 70, FormatColor("#FFFFFF", "BUILD TENT"), false, false);

                AddButton(75, 258, 0xFA5, 0xFA7, 3, GumpButtonType.Reply, 0);
                AddHtml(110, 260, 110, 70, FormatColor("#FFFFFF", "BUILD SHRINE"), false, false);

                AddButton(75, 283, 0xFA5, 0xFA7, 4, GumpButtonType.Reply, 0);
                AddHtml(110, 285, 110, 70, FormatColor("#FFFFFF", "BUILD STASH"), false, false);

                AddButton(75, 308, 0xFA5, 0xFA7, 5, GumpButtonType.Reply, 0);
                AddHtml(110, 310, 110, 70, FormatColor("#FFFFFF", "BUILD ANVIL & FORGE"), false, false);
            }

            public override void OnResponse(NetState sender, RelayInfo info)
            {

                PlayerMobile pm = m_From as PlayerMobile;
		OutpostCamp camp = m_Camp;
		int button = info.ButtonID; 
		double camping = pm.Skills[SkillName.Camping].Value;
		double mining = pm.Skills[SkillName.Mining].Value;

                if(camp.m_Tent != null)
		    HasTent = true;
		else
		    HasTent = false;


		if(button == 1 && m_Camp.Active)
		{
              	    //CampersMap map = FindMap(pm);
		    if(pm.Skills[SkillName.Camping].Value < 50.0)
		    {
			pm.SendMessage("You do not have sufficient skill in camping to do that.");
			return;
		    }
		    else
		    {
			/*
		    	map.Target = pm.Location;
		    	map.TargetMap = pm.Map;
		    	map.Marked = true;
			pm.PlaySound(0x249);
		    	pm.SendMessage("Your camper's map has been updated.");
			*/

		    	m_From.Target = new InternalTarget();
		    	m_From.SendMessage("Select a camping map.");

		    }

		    pm.SendGump(new OutpostGump(pm, m_Camp));
		}

		if(button == 2 && m_Camp.Active)
		{
		    if(m_Camp.m_Tent == null && (pm.Skills[SkillName.Camping].Value >= 70))
		    {
			m_Camp.AddTent();
			pm.SendMessage("You upgrade the outpost with a tent.");
		        pm.PlaySound(0x23D);
		    }
		    else if(m_Camp.m_Tent != null)
			pm.SendMessage("The outpost already has that upgrade.");
		    else
			pm.SendMessage("Doing that would require greater skill in Camping.");

		    pm.SendGump(new OutpostGump(pm, m_Camp));
		}

		if(button == 3 && m_Camp.Active)
		{
		    if(m_Camp.m_Ankh == null && (pm.Skills[SkillName.Camping].Value >= 90))
		    {
			m_Camp.AddAnkh();
			pm.SendMessage("You upgrade the outpost with a shrine.");
		        pm.PlaySound(0x1E7);
		    }
		    else if(m_Camp.m_Ankh != null)
			pm.SendMessage("The outpost already has that upgrade.");
		    else
			pm.SendMessage("Doing that would require greater skill in Camping.");

		   pm.SendGump(new OutpostGump(pm, m_Camp));
		}

		if(button == 4 && m_Camp.Active)
		{
		    if(m_Camp.m_Stash == null && (pm.Skills[SkillName.Camping].Value >= 100))
		    {
			m_Camp.AddBank();
			pm.SendMessage("You upgrade the outpost with a stash.");
		        pm.PlaySound(0x2A); //or 0x3BA
		    }
		    else if(m_Camp.m_Stash != null)
			pm.SendMessage("The outpost already has that upgrade.");
		    else
			pm.SendMessage("Doing that would require greater skill in Camping.");

		   pm.SendGump(new OutpostGump(pm, m_Camp));
		}

		if(button == 5 && m_Camp.Active)
		{
		    if(m_Camp.m_Anvil == null && (pm.Skills[SkillName.Camping].Value >= 70))
		    {
			m_Camp.AddAnvilAndForge();
			pm.SendMessage("You upgrade the outpost with an anvil and forge.");
		        pm.PlaySound(0x3BA);
		        pm.PlaySound(0x2B);
		    }
		    else if(m_Camp.m_Anvil != null)
			pm.SendMessage("The outpost already has that upgrade.");
		    else
			pm.SendMessage("Doing that would require greater skill in Camping.");

		   pm.SendGump(new OutpostGump(pm, m_Camp));
		}

            }

	    private string FormatColor(string color, string str)
	    {
		return String.Format("<BASEFONT COLOR={0}>{1}", color, str);
	    }

	    /*
            private static CampersMap FindMap(Mobile from)
            {
            	if (from == null || from.Backpack == null)
            	{
                    return null;
            	}

            	if (from.Holding is CampersMap)
            	{
                    return (CampersMap)from.Holding;
            	}

            	return from.Backpack.FindItemByType<CampersMap>();
            }
	    */

            private void CloseGump()
            {
                //Campfire.RemoveEntry(m_Entry);
                m_From.CloseGump(typeof(OutpostGump));
            }

	    private class InternalTarget : Target
	    {
		public InternalTarget()
		    :base(12, false, TargetFlags.None)
		{
		}

		protected override void OnTarget(Mobile from, object targeted)
		{
		    if(targeted is CampingMap)
		    {
			if(from is PlayerMobile)
			{
			    PlayerMobile pm = (PlayerMobile)from;
			    CampingMap map = (CampingMap)targeted;

			    if(map.Entries.Count < map.MaxEntries)
			    {
				map.AddEntry(pm);
				pm.PlaySound(0x249);
			    }
			    else
				pm.SendMessage("That map has too many entries. Location cannot be added.");
			}
		    }
		    else
			from.SendMessage("That is not a camping map!");
		}
	    }
        }
}