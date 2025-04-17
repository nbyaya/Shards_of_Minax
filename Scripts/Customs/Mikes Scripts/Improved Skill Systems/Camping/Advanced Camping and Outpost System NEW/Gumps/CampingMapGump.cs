using System;
using Server;
using Server.Gumps;
using Server.Items;
using Server.SkillHandlers;
using System.Collections.Generic;
using System.Linq;
using Server.Misc;
using Server.Commands;
using Server.Network;
using Server.Prompts;

namespace Server.Mobiles
{
    public class CampingMapGump: Gump
    {
	private CampingMap m_Map;
	private PlayerMobile m_Player;
	private List<CampsiteEntry> m_Entries;

        public CampingMapGump(PlayerMobile pm, CampingMap map)
            : base(250, 50)
        {

	    m_Map = map;
	    m_Player = pm;

	    if(map.Entries != null)
	    {
		pm.SendMessage("This map has " + map.Entries.Count + " entries.");
		AddEntries(map.Entries);
	    }

	    if(map.Description == null)
		map.Description = "Camping Map";

	    AddGumpLayout();

	}

        public void AddGumpLayout()
        {
            AddPage(0);
	    AddImage(0, 0, 0x4DF);

	    if(m_Map.Entries != null)
		AddEntries(m_Map.Entries);

            AddButton(60, 25, 0x9A9, 0x9AA, 1, GumpButtonType.Reply, 0); // rename map

	    if(m_Map != null && m_Map.Description != null)
	    	AddHtml(100, 30, 200, 40,  m_Map.Description, false, false); // map name
	    else
	    	AddHtml(100, 30, 200, 40,  "no description", false, false); // map name

	    //headers
	    AddHtml(100, 60, 125, 36, "Name", false, false);
	    AddHtml(250, 60, 125, 36, "Travel", false, false);
	    AddHtml(295, 60, 125, 36, "Remove", false, false);
	
	}

	public void AddEntries(List<CampsiteEntry> entries)
	{

 	    int button = 2;
	    for( int i = 0; i < entries.Count; i++)
	    {
		AddButton(60, 80 + (i * 20),  0x15E1, 0x15E5, button, GumpButtonType.Reply, 0); // rename
		AddHtml(100, 80 + (i * 20), 125, 36,  entries[i].Description, false, false); //description
		AddButton(255, 80 + (i * 20), 0x4B9, 0x4BA, button + 1, GumpButtonType.Reply, 0); // travel
		AddButton(300, 80 + (i * 20), 0x657, 0x657, i + 20, GumpButtonType.Reply, 0); //remove
		button += 2;
	    }

	    m_Entries = entries;
	}

	public override void OnResponse(NetState sender, RelayInfo info)
	{
	    int button = info.ButtonID; 
	    if(button == 1 && m_Map != null)
	    {
		/*
		if(m_Map.Entries.Count < m_Map.MaxEntries)
		{
		    m_Map.AddEntry(m_Player);
		    AddEntries(m_Map.Entries);
		    m_Player.SendGump( new CampingMapGump(m_Player, m_Map));
		}
		else
		    m_Player.SendMessage("That map has too many entries. Location cannot be added.");
		*/

		if (!m_Map.IsLockedDown || m_Player.AccessLevel >= AccessLevel.GameMaster)
		    m_Player.Prompt = new RenameMapPrompt(m_Map);
		else
		    m_Player.SendMessage("You cannot do that while this item is locked down.");
	    }
	    else if(m_Map == null)
		m_Player.SendMessage("Cannot get map");

	    int index = info.ButtonID - 2;

	    if( index >= 0 && index < (m_Entries.Count*2))
	    {
		if( index == 0 || index % 2 == 0)
		{
		   if(index == 0)
			m_Player.Prompt = new RenamePrompt(m_Entries[index]);
		    else
		    	m_Player.Prompt = new RenamePrompt(m_Entries[index/2]);
		}
		else
		{
		    m_Player.Say("*You begin your journey*");

		    Timer.DelayCall(TimeSpan.FromSeconds(3), () => 
		    {
		    	m_Player.SendMessage("You find your way back to your campsite.");
		    	m_Player.Location = m_Entries[index/2].Target;
		    	m_Player.Map = m_Entries[index/2].TargetMap;

		    });
		}
	    }

	    if(	index >= 18 && index <= (m_Entries.Count) + 18)
	    {
		if (!m_Map.IsLockedDown || m_Player.AccessLevel >= AccessLevel.GameMaster)
		{
		    m_Player.SendMessage("This will delete the entry.");
		    m_Entries.RemoveAt(index-18);
		}
		else
		    m_Player.SendMessage("You cannot do that while this item is locked down.");
	    }
	}

        private class RenamePrompt : Prompt
        {
            public override int MessageCliloc { get { return 501804; } }
            private readonly CampsiteEntry m_Entry;

            public RenamePrompt(CampsiteEntry entry)
            {
                m_Entry = entry;
            }

            public override void OnResponse(Mobile from, string text)
            {
		m_Entry.Description = text;
		from.SendMessage("The name of the campsite has been changed.");
            }
        }

        private class RenameMapPrompt : Prompt
        {
            public override int MessageCliloc { get { return 501804; } }
            private readonly CampingMap m_Map;

            public RenameMapPrompt(CampingMap map)
            {
                m_Map = map;
            }

            public override void OnResponse(Mobile from, string text)
            {
		m_Map.Description = text;
		from.SendMessage("The name of your map has been changed.");
            }
        }
    }
}