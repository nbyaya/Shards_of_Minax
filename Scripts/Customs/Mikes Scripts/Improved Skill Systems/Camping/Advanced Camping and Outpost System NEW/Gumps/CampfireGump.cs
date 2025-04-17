using Server.Gumps;
using Server.Mobiles;
using Server.Network;
using System;
using Server.Items;
using Server.Multis;
using Server.Targeting;

namespace Server.Gumps
{
        public class CampfireGump : Gump
        {
            //private readonly Timer m_CloseTimer;
            private readonly CampfireEntry m_Entry;
	    private readonly Campfire m_Campfire;

            public CampfireGump(CampfireEntry entry, Campfire fire)
                : base(100, 0)
            {
                m_Entry = entry;
		m_Campfire = fire;

                //AddBackground(0, 0, 380, 200, 0xA28); //Old background
            	AddBackground(0, 0, 380, 200, 0x6DB);

		AddHtml(120, 20, 200, 35, FormatColor("#FFFFFF", "Secure Campfire"), false, false);
	        AddItem(80, 20, 0xDE3);


		AddHtml(50, 45, 300, 140, FormatColor("#FFFFFF", "At a secure campsite, you can: <br>-Mark this location on your Camping Map.<br>-Upgrade your campsite to provide yourself and all nearby allies with a boon."), false, false);

                AddButton(75, 133, 0xFA5, 0xFA7, 1, GumpButtonType.Reply, 0);
		//string mark = Color("FFFFFF", "MARK LOCATION");
                AddHtml(110, 135, 110, 70, FormatColor("#FFFFFF", "MARK LOCATION"), false, false);

                AddButton(75, 158, 0xFA5, 0xFA7, 2, GumpButtonType.Reply, 0);
                AddHtml(110, 160, 110, 70, FormatColor("#FFFFFF", "UPGRADE CAMP"), false, false);

            }

            public override void OnResponse(NetState sender, RelayInfo info)
            {
                PlayerMobile pm = m_Entry.Player;
		Campfire fire = m_Campfire;
		int button = info.ButtonID;
            	BaseBoat boat = BaseBoat.FindBoatAt(pm.Location, pm.Map);

		Mobile m = fire.Owner;

                if (Campfire.GetEntry(pm) != m_Entry)
                    return;

		if(button == 1)
		{
		    
		    /*
              	    CampersMap map = FindMap(pm);
		    if(map == null || map.Deleted)
		    {
			pm.SendMessage("A camper'smap must be in your backpack.");
			return;
		    }
		    else if(pm.Skills[SkillName.Camping].Value < 60.0)
		    {
			pm.SendMessage("You do not have sufficient skill in camping to do that.");
			return;
		    }
            	    else if (boat != null && !(boat is BaseGalleon))
            	    {
                	pm.LocalOverheadMessage(MessageType.Regular, 0x3B2, 501800); // You cannot mark an object at that location.
            	    }
		    else
		    {
		    	map.Target = fire.Location;
		    	map.TargetMap = fire.Map;
		    	map.Marked = true;
			pm.PlaySound(0x249);
		    	pm.SendMessage("Your camper's map has been updated.");
		    }
		    */

		    if(fire.Owner != null) // this uses a modified kindling and campfire file
		    {
		    	m.Target = new InternalTarget();
		    	m.SendMessage("Select a camping map.");
		    	m.SendGump(new CampfireGump(m_Entry, fire));

		    }
		    else
			pm.SendMessage("campfire owner not found");
		
		}

		if(button == 2)
		{
		    fire.IsUpgraded = true;
		    pm.PlaySound(0x208);
		    pm.SendGump(new CampfireGump(m_Entry, fire));
		}

                //Campfire.RemoveEntry(m_Entry);

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
                Campfire.RemoveEntry(m_Entry);
                m_Entry.Player.CloseGump(typeof(CampfireGump));
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