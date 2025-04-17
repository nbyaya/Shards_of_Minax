using System;
using System.Collections.Generic;
using Server.ContextMenus;
using Server.Engines.Craft;
using Server.Gumps;
using Server.Mobiles;
using Server.Multis;
using Server.Network;

namespace Server.Items
{
    public class CampingMap : Item, ISecurable //, ICraftable
    {

        private List<CampsiteEntry> m_Entries;
        private string m_Description;
        private Mobile m_Crafter;

        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile Crafter
        {
            get
            {
                return m_Crafter;
            }
            set
            {
                m_Crafter = value;
                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public SecureLevel Level { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public string Description
        {
            get
            {
                return m_Description;
            }
            set
            {
                m_Description = value;
                InvalidateProperties();
            }
        }

        public virtual int MaxEntries { get { return 9; } }

        [Constructable]
        public CampingMap()
            : base(0x14EC) //0xA1E4
        {
	    Name = "camping map";
            Weight = 1.0;
	    Hue = 1801;
            m_Entries = new List<CampsiteEntry>();
        }

        public List<CampsiteEntry> Entries { get { return m_Entries; } }

	public void AddEntry(PlayerMobile pm)
	{
	    if(m_Entries == null)
	    {
                m_Entries = new List<CampsiteEntry>();	
		pm.SendMessage("New list added.");
	    }
	    else
	    {	
		CampsiteEntry entry = new CampsiteEntry(pm.Location, pm.Map, "a new campsite");
		m_Entries.Add(entry);
		pm.SendMessage(entry.Description + " added to list");
		pm.SendMessage(m_Entries.Count + " entries total");
	    }
	}

	public override void OnDoubleClick(Mobile from)
	{
	    if(from is PlayerMobile)
	    {
		PlayerMobile pm = (PlayerMobile)from;
            	pm.SendGump(new CampingMapGump(pm, this));
		pm.SendMessage("You open your camping map");
	    }
	}

        public CampingMap(Serial serial)
            : base(serial)
        {
        }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);
            SetSecureLevelEntry.AddTo(from, this, list);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version

            writer.Write(m_Entries.Count);

            for (int i = 0; i < m_Entries.Count; ++i)
                m_Entries[i].Serialize(writer);

            writer.Write(m_Description);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            int count = reader.ReadInt();
            m_Entries = new List<CampsiteEntry>(count);

            for (int i = 0; i < count; ++i)
		m_Entries.Add(new CampsiteEntry(reader));

	    m_Description = reader.ReadString();
        }

	public override void GetProperties(ObjectPropertyList list)
	{
            base.GetProperties(list);

            if (m_Description != null && m_Description.Length > 0)
                list.Add(m_Description);

	}
    }

    public class CampsiteEntry
    {
	private Map m_TargetMap;
        private string m_Description;

	[CommandProperty(AccessLevel.Counselor, AccessLevel.GameMaster)]
        public Point3D Target { get; set; }

        [CommandProperty(AccessLevel.Counselor, AccessLevel.GameMaster)]
        public Map TargetMap
        {
            get { return m_TargetMap; }
            set
            {
                if (m_TargetMap != value)
                {
                    m_TargetMap = value;
                    //InvalidateProperties();
                }
            }
        }

        public string Description
        {
            get { return m_Description; }
	    set { m_Description = value; }
        }

        public BaseGalleon Galleon { get; }

	public CampsiteEntry(Point3D loc, Map map, string desc)
	{
            Target = loc;
            m_TargetMap = map;
            m_Description = desc;
	}

        public CampsiteEntry(GenericReader reader)
        {
            int version = reader.ReadByte();

            //int version = reader.ReadInt();
	    Target = reader.ReadPoint3D();
	    m_TargetMap = reader.ReadMap();
	    m_Description = reader.ReadString();
        }

        public void Serialize(GenericWriter writer)
        {
            writer.Write((byte)3);

	    writer.Write(Target);
	    writer.Write(m_TargetMap);
	    writer.Write(m_Description);
        }
    }
}