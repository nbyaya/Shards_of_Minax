/*************************************
* WaypointPortals.cs
*
* A minimal waypoint/portal system with a side–menu and multi–page gump.
* – World portal entries are defined by category.
* – Each player has a WaypointProfile storing which portals have been discovered.
* – When a player physically uses a portal item, that destination is “discovered.”
* – The gump displays a left–side menu of categories and a paged list of portal entries.
* – If a portal is locked (not discovered) the gump shows “??? (Locked)” and clicking it does nothing.
* – The [worldomnigen command creates a portal item for every entry.
*
* (C) 2025 Rutibex
*************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Server;
using Server.Commands;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.PortalWaypoints
{
   #region WaypointEntry & Profile Classes

   /// <summary>
   /// A simple container for a portal destination.
   /// </summary>
   public class WaypointEntry
   {
      public string Name { get; set; }
      public Point3D Destination { get; set; }
      public Map Map { get; set; }

      public WaypointEntry(string name, Point3D destination, Map map)
      {
         Name = name;
         Destination = destination;
         Map = map;
      }
   }

   /// <summary>
   /// Per–player data tracking discovered portals.
   /// </summary>
   public class WaypointProfile
   {
      public PlayerMobile Owner { get; private set; }
      public HashSet<string> UnlockedPortals { get; private set; } = new HashSet<string>();

      public WaypointProfile(PlayerMobile owner)
      {
         Owner = owner;
         // By default, only the "Throne Room" is unlocked.
         UnlockedPortals.Add("Throne Room");
      }

      public void Serialize(GenericWriter writer)
      {
         writer.Write(0); // version
         writer.Write(Owner);
         writer.Write(UnlockedPortals.Count);
         foreach (string portal in UnlockedPortals)
         {
            writer.Write(portal);
         }
      }

      public void Deserialize(GenericReader reader)
      {
         reader.ReadInt(); // version
         Owner = reader.ReadMobile<PlayerMobile>();
         int count = reader.ReadInt();
         UnlockedPortals = new HashSet<string>();
         for (int i = 0; i < count; i++)
            UnlockedPortals.Add(reader.ReadString());
      }
   }

   public static class WaypointProfiles
   {
      public static Dictionary<PlayerMobile, WaypointProfile> Profiles = new Dictionary<PlayerMobile, WaypointProfile>();

      public static WaypointProfile AcquireProfile(this PlayerMobile player)
      {
         if (!Profiles.ContainsKey(player))
            Profiles[player] = new WaypointProfile(player);
         return Profiles[player];
      }

      public static void Save(GenericWriter writer)
      {
         writer.Write(Profiles.Count);
         foreach (var profile in Profiles.Values)
         {
            profile.Serialize(writer);
         }
      }

      public static void Load(GenericReader reader)
      {
         int count = reader.ReadInt();
         Profiles = new Dictionary<PlayerMobile, WaypointProfile>();
         for (int i = 0; i < count; i++)
         {
            WaypointProfile profile = new WaypointProfile(null);
            profile.Deserialize(reader);
            if (profile.Owner != null)
               Profiles[profile.Owner] = profile;
         }
      }
   }

   #endregion

   #region WaypointPortal Item

   /// <summary>
   /// A non–movable portal item. When double–clicked it opens the waypoint gump.
   /// </summary>
   public class WaypointPortal : Item
   {
      // Define your portal entries organized by category.
      // You can add more categories if desired.
      public static Dictionary<string, WaypointEntry[]> Categories = new Dictionary<string, WaypointEntry[]>();

      // Make the entries accessible from the gump.
      static WaypointPortal()
      {
         // For example, one category "Custom"
         Categories["Trammel"] = new WaypointEntry[]
         {
			new WaypointEntry("Britain T", new Point3D(1434, 1699, 2), Map.Trammel ),
			new WaypointEntry("Bucs Den T", new Point3D(2705, 2162, 0), Map.Trammel ),
			new WaypointEntry("Cove T", new Point3D(2237, 1214, 0), Map.Trammel ),
			new WaypointEntry("Delucia T", new Point3D(5274, 3991, 37), Map.Trammel ),
			new WaypointEntry("New Haven T", new Point3D(3493, 2577, 14), Map.Trammel ),
			new WaypointEntry("Jhelom T", new Point3D(1417, 3821, 0), Map.Trammel ),
			new WaypointEntry("Magincia T", new Point3D(3728, 2164, 20), Map.Trammel ),
			new WaypointEntry("Minoc T", new Point3D(2525, 582, 0), Map.Trammel ),
			new WaypointEntry("Moonglow T", new Point3D(4471, 1177, 0), Map.Trammel ),
			new WaypointEntry("Nujel'm T", new Point3D(3770, 1308, 0), Map.Trammel ),
			new WaypointEntry("Papua T", new Point3D(5729, 3208, -6), Map.Trammel ),
			new WaypointEntry("Serpents Hold T", new Point3D(2895, 3479, 15), Map.Trammel ),
			new WaypointEntry("Skara Brae T", new Point3D(596, 2138, 0), Map.Trammel ),
			new WaypointEntry("Trinsic T", new Point3D(1823, 2821, 0), Map.Trammel ),
			new WaypointEntry("Vesper T", new Point3D(2899, 676, 0), Map.Trammel ),
			new WaypointEntry("Wind T", new Point3D(1361, 895, 0), Map.Trammel ),
			new WaypointEntry("Yew T", new Point3D(542, 985, 0), Map.Trammel )
         };
		 		 
         Categories["Trammel-Dun"] = new WaypointEntry[]
         {
		    new WaypointEntry("Blighted Grove T", new Point3D(586, 1643, -5), Map.Trammel ),
			new WaypointEntry("Covetous T", new Point3D(2498, 921, 0), Map.Trammel ),
			new WaypointEntry("Daemon Temple T", new Point3D(4591, 3647, 80), Map.Trammel ),
			new WaypointEntry("Deceit T", new Point3D(4111, 434, 5), Map.Trammel ),
			new WaypointEntry("Despise T", new Point3D(1301, 1080, 0), Map.Trammel ),
			new WaypointEntry("Destard T", new Point3D(1176, 2640, 2), Map.Trammel ),
			new WaypointEntry("Fire T", new Point3D(2923, 3409, 8), Map.Trammel ),
			new WaypointEntry("Hythloth T", new Point3D(4721, 3824, 0), Map.Trammel ),
			new WaypointEntry("Ice T", new Point3D(1999, 81, 4), Map.Trammel ),
			new WaypointEntry("Ophidian Temple T", new Point3D(5766, 2634, 43), Map.Trammel ),
			new WaypointEntry("Orc Caves T", new Point3D(1017, 1429, 0), Map.Trammel ),
			new WaypointEntry("Painted Caves T", new Point3D(1716, 2993, 0), Map.Trammel ),
			new WaypointEntry("Paroxysmus T", new Point3D(5569, 3019, 31), Map.Trammel ),
			new WaypointEntry("Prism of Light T", new Point3D(3789, 1095, 20), Map.Trammel ),
			new WaypointEntry("Sanctuary T", new Point3D(759, 1642, 0), Map.Trammel ),
			new WaypointEntry("Shame T", new Point3D(511, 1565, 0), Map.Trammel ),
			new WaypointEntry("Solen Hive T", new Point3D(2607, 763, 0), Map.Trammel ),
			new WaypointEntry("Terathan Keep T", new Point3D(5451, 3143, -60), Map.Trammel ),
			new WaypointEntry("Wrong T", new Point3D(2043, 238, 10), Map.Trammel )
         };

         Categories["Felucca"] = new WaypointEntry[]
         {
			new WaypointEntry("Britain F", new Point3D(1434, 1699, 2), Map.Felucca ),
			new WaypointEntry("Bucs Den F", new Point3D(2705, 2162, 0), Map.Felucca ),
			new WaypointEntry("Cove F", new Point3D(2237, 1214, 0), Map.Felucca ),
			new WaypointEntry("Delucia F", new Point3D(5274, 3991, 37), Map.Felucca ),
			new WaypointEntry("Jhelom F", new Point3D(1417, 3821, 0), Map.Felucca ),
			new WaypointEntry("Magincia F", new Point3D(3728, 2164, 20), Map.Felucca ),
			new WaypointEntry("Minoc F", new Point3D(2525, 582, 0), Map.Felucca ),
			new WaypointEntry("Moonglow F", new Point3D(4471, 1177, 0), Map.Felucca ),
			new WaypointEntry("Nujel'm F", new Point3D(3770, 1308, 0), Map.Felucca ),
			new WaypointEntry("Ocllo F", new Point3D(3626, 2611, 0), Map.Felucca ),
			new WaypointEntry("Papua F", new Point3D(5729, 3208, -6), Map.Felucca ),
			new WaypointEntry("Serpents Hold F", new Point3D(2895, 3479, 15), Map.Felucca ),
			new WaypointEntry("Skara Brae F", new Point3D(596, 2138, 0), Map.Felucca ),
			new WaypointEntry("Trinsic F", new Point3D(1823, 2821, 0), Map.Felucca ),
			new WaypointEntry("Vesper F", new Point3D(2899, 676, 0), Map.Felucca ),
			new WaypointEntry("Wind F", new Point3D(1361, 895, 0), Map.Felucca ),
			new WaypointEntry("Yew F", new Point3D(542, 985, 0), Map.Felucca )
         };

         Categories["Felucca Dungeons"] = new WaypointEntry[]
         {
		    new WaypointEntry("Blighted Grove F", new Point3D(586, 1643, -5), Map.Felucca ),
			new WaypointEntry("Covetous F", new Point3D(2498, 921, 0), Map.Felucca ),
			new WaypointEntry("Daemon Temple F", new Point3D(4591, 3647, 80), Map.Felucca ),
			new WaypointEntry("Deceit F", new Point3D(4111, 434, 5), Map.Felucca ),
			new WaypointEntry("Despise F", new Point3D(1301, 1080, 0), Map.Felucca ),
			new WaypointEntry("Destard F", new Point3D(1176, 2640, 2), Map.Felucca ),
			new WaypointEntry("Fire F", new Point3D(2923, 3409, 8), Map.Felucca ),
			new WaypointEntry("Hythloth F", new Point3D(4721, 3824, 0), Map.Felucca ),
			new WaypointEntry("Ice F", new Point3D(1999, 81, 4), Map.Felucca ),
			new WaypointEntry("Ophidian Temple F", new Point3D(5766, 2634, 43), Map.Felucca ),
			new WaypointEntry("Orc Caves F", new Point3D(1017, 1429, 0), Map.Felucca ),
			new WaypointEntry("Painted Caves F", new Point3D(1716, 2993, 0), Map.Felucca ),
			new WaypointEntry("Paroxysmus F", new Point3D(5569, 3019, 31), Map.Felucca ),
			new WaypointEntry("Prism of Light F", new Point3D(3789, 1095, 20), Map.Felucca ),
			new WaypointEntry("Sanctuary F", new Point3D(759, 1642, 0), Map.Felucca ),
			new WaypointEntry("Shame F", new Point3D(511, 1565, 0), Map.Felucca ),
			new WaypointEntry("Solen Hive F", new Point3D(2607, 763, 0), Map.Felucca ),
			new WaypointEntry("Terathan Keep F", new Point3D(5451, 3143, -60), Map.Felucca ),
			new WaypointEntry("Wrong F", new Point3D(2043, 238, 10), Map.Felucca )
         };
		 
         Categories["Public Moongates"] = new WaypointEntry[]
         {
			new WaypointEntry("Britain TMG", new Point3D(1336, 1997, 5), Map.Trammel ),
			new WaypointEntry("New Haven TMG", new Point3D(3450, 2677, 25), Map.Trammel ),
			new WaypointEntry("Jhelom TMG", new Point3D(1499, 3771, 5), Map.Trammel ),
			new WaypointEntry("Magincia TMG", new Point3D(3563, 2139, 34), Map.Trammel ),
			new WaypointEntry("Minoc TMG", new Point3D(2701, 692, 5), Map.Trammel ),
			new WaypointEntry("Moonglow TMG", new Point3D(4467, 1283, 5), Map.Trammel ),
			new WaypointEntry("Skara Brae TMG", new Point3D(643, 2067, 5), Map.Trammel ),
			new WaypointEntry("Trinsic TMG", new Point3D(1828, 2948, -20), Map.Trammel ),
			new WaypointEntry("Yew TMG", new Point3D(771, 752, 5), Map.Trammel ),
			new WaypointEntry("Britain FMG", new Point3D(1336, 1997, 5), Map.Felucca ),
			new WaypointEntry("Buccaneer's Den FMG", new Point3D(2711, 2234, 0), Map.Felucca ),
			new WaypointEntry("Jhelom FMG", new Point3D(1499, 3771, 5), Map.Felucca ),
			new WaypointEntry("Magincia FMG", new Point3D(3563, 2139, 34), Map.Felucca ),
			new WaypointEntry("Minoc FMG", new Point3D(2701, 692, 5), Map.Felucca ),
			new WaypointEntry("Moonglow FMG", new Point3D(4467, 1283, 5), Map.Felucca ),
			new WaypointEntry("Skara Brae FMG", new Point3D(643, 2067, 5), Map.Felucca ),
			new WaypointEntry("Trinsic FMG", new Point3D(1828, 2948, -20), Map.Felucca ),
			new WaypointEntry("Yew FMG", new Point3D(771, 752, 5), Map.Felucca )
         };

         Categories["Ilshenar"] = new WaypointEntry[]
         {
			new WaypointEntry("Ankh Dungeon", new Point3D(576, 1150, -100), Map.Ilshenar ),
			new WaypointEntry("Blood Dungeon", new Point3D(1747, 1171, -2), Map.Ilshenar ),
			new WaypointEntry("Exodus Dungeon", new Point3D(854, 778, -80), Map.Ilshenar ),
			new WaypointEntry("Lakeshire", new Point3D(1203, 1124, -25), Map.Ilshenar ),
			new WaypointEntry("Mistas", new Point3D(819, 1130, -29), Map.Ilshenar ),
			new WaypointEntry("Montor", new Point3D(1706, 205, 104), Map.Ilshenar ),
			new WaypointEntry("Rock Dungeon", new Point3D(1787, 572, 69), Map.Ilshenar ),
			new WaypointEntry("Savage Camp", new Point3D(1151, 659, -80), Map.Ilshenar ),
			new WaypointEntry("Sorceror's Dungeon", new Point3D(548, 462, -53), Map.Ilshenar ),
			new WaypointEntry("Spectre Dungeon", new Point3D(1363, 1033, -8), Map.Ilshenar ),
			new WaypointEntry("Spider Cave", new Point3D(1420, 913, -16), Map.Ilshenar ),
			new WaypointEntry("Wisp Dungeon", new Point3D(651, 1302, -58), Map.Ilshenar )
         };

         Categories["Ilshenar Shrines"] = new WaypointEntry[]
         {
			new WaypointEntry("Compassion", new Point3D(1215, 467, -13), Map.Ilshenar ),
			new WaypointEntry("Honesty", new Point3D(722, 1366, -60), Map.Ilshenar ),
			new WaypointEntry("Honor", new Point3D(744, 724, -28), Map.Ilshenar ),
			new WaypointEntry("Humility", new Point3D(281, 1016, 0), Map.Ilshenar ),
			new WaypointEntry("Justice", new Point3D(987, 1011, -32), Map.Ilshenar ),
			new WaypointEntry("Sacrifice", new Point3D(1174, 1286, -30), Map.Ilshenar ),
			new WaypointEntry("Spirituality", new Point3D(1532, 1340, -3), Map.Ilshenar ),
			new WaypointEntry("Valor", new Point3D(528, 216, -45), Map.Ilshenar ),
			new WaypointEntry("Choas", new Point3D(1721, 218, 96), Map.Ilshenar )
         };

         Categories["Malas"] = new WaypointEntry[]
         {
			new WaypointEntry("Doom", new Point3D(2368, 1267, -85), Map.Malas ),
			new WaypointEntry("Labyrinth", new Point3D(1730, 981, -80), Map.Malas ),
			new WaypointEntry("Luna", new Point3D(1015, 527, -65), Map.Malas ),
			new WaypointEntry("Orc Fort 1", new Point3D(912, 215, -90), Map.Malas ),
			new WaypointEntry("Orc Fort 2", new Point3D(1678, 374, -50), Map.Malas ),
			new WaypointEntry("Orc Fort 3", new Point3D(1375, 621, -86), Map.Malas ),
			new WaypointEntry("Orc Fort 4", new Point3D(1184, 715, -89), Map.Malas ),
			new WaypointEntry("Orc Fort 5", new Point3D(1279, 1324, -90), Map.Malas ),
			new WaypointEntry("Orc Fort 6", new Point3D(1598, 1834, -107), Map.Malas ),
			new WaypointEntry("Ruined Temple", new Point3D(1598, 1762, -110), Map.Malas ),
			new WaypointEntry("Umbra", new Point3D(1997, 1386, -85), Map.Malas )
         };

         Categories["Tokuno"] = new WaypointEntry[]
         {
			new WaypointEntry("Bushido Dojo", new Point3D(322, 430, 32), Map.Tokuno ),
			new WaypointEntry("Crane Marsh", new Point3D(203, 985, 18), Map.Tokuno ),
			new WaypointEntry("Fan Dancer's Dojo", new Point3D(970, 222, 23), Map.Tokuno ),
			new WaypointEntry("Isamu-Jima", new Point3D(1169, 998, 41), Map.Tokuno ),
			new WaypointEntry("Makoto-Jima", new Point3D(802, 1204, 25), Map.Tokuno ),
			new WaypointEntry("Homare-Jima", new Point3D(270, 628, 15), Map.Tokuno ),
			new WaypointEntry("Makoto Desert", new Point3D(724, 1050, 33), Map.Tokuno ),
			new WaypointEntry("Makoto Zento", new Point3D(741, 1261, 30), Map.Tokuno ),
			new WaypointEntry("Mt. Sho Castle", new Point3D(1234, 772, 3), Map.Tokuno ),
			new WaypointEntry("Valor Shrine", new Point3D(1044, 523, 15), Map.Tokuno ),
			new WaypointEntry("Yomotsu Mine", new Point3D(257, 786, 63), Map.Tokuno )
         };

         Categories["TerMur"] = new WaypointEntry[]
         {
			new WaypointEntry("Royal City", new Point3D(852, 3526, -43), Map.TerMur),
			new WaypointEntry("Holy City", new Point3D(926, 3989, -36), Map.TerMur),
			new WaypointEntry("Fisherman's Reach", new Point3D(612, 3038, 35), Map.TerMur),
			new WaypointEntry("Tomb of Kings", new Point3D(997, 3843, -41), Map.TerMur),
			new WaypointEntry("Valley of Eodon", new Point3D(719, 1863, 40), Map.TerMur),
			new WaypointEntry("Underworld", new Point3D(4194, 3268, 0), Map.Trammel)
         };
		 		 
         Categories["Special"] = new WaypointEntry[]
         {
            new WaypointEntry("Throne Room", new Point3D(1322, 1624, 55), Map.Trammel),
            new WaypointEntry("Taming Forest", new Point3D(902, 912, 0), Map.Trammel),
            new WaypointEntry("Beast Hunters Guild", new Point3D(2954, 3352, 15), Map.Trammel),
            new WaypointEntry("Ultimate Masters", new Point3D(4311, 993, 15), Map.Trammel)
         };

         // You can add more categories here…
      }

      // When generating, we pass an entry so that the portal item is placed in the world.
      public WaypointPortal(WaypointEntry entry) : base(0xF6C) // using a moongate graphic
      {
         Movable = false;
         Hue = 2448;
         Name = "Waypoint Portal";
         // Place this portal in the world at the destination.
         MoveToWorld(entry.Destination, entry.Map);
      }

      [Constructable]
      public WaypointPortal() : this(new WaypointEntry("Throne Room", new Point3D(1322, 1624, 55), Map.Trammel))
      {
      }

		public override void OnDoubleClick(Mobile from)
		{
			if (!from.Player)
				return;

			PlayerMobile player = from as PlayerMobile;
			if (player == null)
				return;

			// Get the waypoint profile of the player
			WaypointProfile profile = player.AcquireProfile();

			// Find the portal entry corresponding to this portal
			foreach (var category in WaypointPortal.Categories)
			{
				foreach (var entry in category.Value)
				{
					if (entry.Destination == this.Location && entry.Map == this.Map)
					{
						// Check if the portal is already unlocked
						if (!profile.UnlockedPortals.Contains(entry.Name))
						{
							// Unlock the portal
							profile.UnlockedPortals.Add(entry.Name);
							player.SendMessage($"You have discovered {entry.Name}!");
						}
						break;
					}
				}
			}

			from.CloseGump(typeof(WaypointGump));
			from.SendGump(new WaypointGump(from));
		}


      public WaypointPortal(Serial serial) : base(serial) { }

      public override void Serialize(GenericWriter writer)
      {
         base.Serialize(writer);
         writer.Write(0); // version
      }

      public override void Deserialize(GenericReader reader)
      {
         base.Deserialize(reader);
         int version = reader.ReadInt();
      }
   }

   #endregion

   #region WaypointGump with Side Menu & Pagination

   /// <summary>
   /// A gump that shows a left–side category menu and a paged list of portals in the selected category.
   /// </summary>
	public class WaypointGump : Gump
	{
		private Mobile m_From;
		private string m_SelectedCategory;
		private int m_Page; // current page of portal entries for the selected category
		private const int PageSize = 10; // Show 10 entries per page instead of 5

		public WaypointGump(Mobile from) : this(from, GetFirstCategory(), 0)
		{
		}

		public WaypointGump(Mobile from, string selectedCategory, int page) : base(50, 50)
		{
			m_From = from;
			m_SelectedCategory = selectedCategory;
			m_Page = page;
			Closable = true;
			Dragable = true;
			BuildGump();
		}

		private static string GetFirstCategory()
		{
			if (WaypointPortal.Categories.Count > 0)
				return new List<string>(WaypointPortal.Categories.Keys)[0];
			return "Custom";
		}

		private void BuildGump()
		{
			AddPage(0);
			AddBackground(0, 0, 500, 500, 5054); // Increase height to 500 to fit more entries

			// Title
			AddHtml(150, 10, 200, 25, "<center>Waypoint Portals</center>", false, false);

			// Left–side menu: list categories
			int catY = 40;
			int catIndex = 0;
			foreach (string cat in WaypointPortal.Categories.Keys)
			{
				string label = cat == m_SelectedCategory ? $"<basefont color=#00FF00>{cat}</basefont>" : cat;
				AddButton(10, catY, 4005, 4007, catIndex + 1, GumpButtonType.Reply, 0);
				AddHtml(50, catY, 100, 25, label, false, false);
				catY += 30;
				catIndex++;
			}

			// Main panel: list portal entries for the selected category (paged)
			if (WaypointPortal.Categories.ContainsKey(m_SelectedCategory))
			{
				WaypointEntry[] entries = WaypointPortal.Categories[m_SelectedCategory];
				WaypointProfile profile = ((PlayerMobile)m_From).AcquireProfile();

				int start = m_Page * PageSize;
				int end = Math.Min(start + PageSize, entries.Length);
				int entryY = 40; // Y position of first entry

				for (int i = start; i < end; i++)
				{
					WaypointEntry entry = entries[i];
					bool unlocked = profile.UnlockedPortals.Contains(entry.Name);
					string text = unlocked ? entry.Name : "<basefont color=#FE0000>??? (Locked)</basefont>";
					AddButton(200, entryY, 4005, 4007, 1000 + (i - start), GumpButtonType.Reply, 0);
					AddHtml(240, entryY, 200, 25, text, false, false);
					entryY += 30;
				}

				// Adjusted Pagination controls
				int paginationY = 400; // Move pagination buttons lower

				if (m_Page > 0)
				{
					AddButton(200, paginationY, 4014, 4016, 2000, GumpButtonType.Reply, 0); // Previous page
					AddHtml(240, paginationY, 100, 25, "<center>Previous</center>", false, false);
				}
				if (end < entries.Length)
				{
					AddButton(320, paginationY, 4005, 4007, 3000, GumpButtonType.Reply, 0); // Next page
					AddHtml(360, paginationY, 100, 25, "<center>Next</center>", false, false);
				}
			}
		}

		public override void OnResponse(NetState state, RelayInfo info)
		{
			Mobile from = state.Mobile;

			if (info.ButtonID > 0 && info.ButtonID < 1000)
			{
				int catIndex = info.ButtonID - 1;
				List<string> keys = new List<string>(WaypointPortal.Categories.Keys);
				if (catIndex >= 0 && catIndex < keys.Count)
				{
					from.SendGump(new WaypointGump(from, keys[catIndex], 0));
					return;
				}
			}
			else if (info.ButtonID == 2000) // Previous page
			{
				int newPage = m_Page - 1;
				if (newPage < 0) newPage = 0;
				from.SendGump(new WaypointGump(from, m_SelectedCategory, newPage));
				return;
			}
			else if (info.ButtonID == 3000) // Next page
			{
				from.SendGump(new WaypointGump(from, m_SelectedCategory, m_Page + 1));
				return;
			}
			else if (info.ButtonID >= 1000)
			{
				int indexInPage = info.ButtonID - 1000;
				int index = m_Page * PageSize + indexInPage;
				if (!WaypointPortal.Categories.ContainsKey(m_SelectedCategory))
				{
					from.SendMessage("No portals in this category.");
					return;
				}
				WaypointEntry[] entries = WaypointPortal.Categories[m_SelectedCategory];
				if (index < 0 || index >= entries.Length)
				{
					from.SendMessage("Invalid selection.");
					return;
				}
				WaypointEntry entry = entries[index];
				WaypointProfile profile = ((PlayerMobile)from).AcquireProfile();
				if (!profile.UnlockedPortals.Contains(entry.Name))
				{
					from.SendMessage("That location is locked. You must discover it first.");
					return;
				}
				from.MoveToWorld(entry.Destination, entry.Map);
			}
			else
			{
				from.SendMessage("Cancelled.");
			}
		}
	}


   #endregion

   #region World Waypoint Generation Command

   /// <summary>
   /// The [worldomnigen command creates a WaypointPortal item for every defined entry.
   /// </summary>
   public class WorldWaypointGenCommand
   {
      public static void Initialize()
      {
         CommandSystem.Register("worldomnigen", AccessLevel.Administrator, new CommandEventHandler(WorldWaypointGen_OnCommand));
         // Hook save/load for profiles.
         EventSink.WorldSave += e => Persistence.Serialize(@"Saves\WaypointProfiles.bin", WaypointProfiles.Save);
         EventSink.WorldLoad += () => Persistence.Deserialize(@"Saves\WaypointProfiles.bin", WaypointProfiles.Load);
      }

      [Usage("worldomnigen")]
      [Description("Generates waypoint portals for each defined portal entry.")]
      public static void WorldWaypointGen_OnCommand(CommandEventArgs e)
      {
         int count = 0;
         foreach (var kvp in WaypointPortal.Categories)
         {
            WaypointEntry[] entries = kvp.Value;
            if (entries != null)
            {
               foreach (WaypointEntry entry in entries)
               {
                  new WaypointPortal(entry);
                  count++;
               }
            }
         }
         e.Mobile.SendMessage("Generated {0} waypoint portals.", count);
      }
   }

   #endregion
}
