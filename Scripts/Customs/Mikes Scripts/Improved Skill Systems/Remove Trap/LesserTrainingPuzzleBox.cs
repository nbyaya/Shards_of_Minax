using System;
using Server;
using System.Collections.Generic;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;

namespace Server.Items
{
    public class LesserTrainingPuzzleBox : Item
    {
        public List<int> Path { get; set; }

        [Constructable]
        public LesserTrainingPuzzleBox() : base(0xE80) // Wooden box item ID
        {
            Name = "Lesser Training Puzzle Box";
            Weight = 1.0;
            Path = GetRandomPath();
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from is PlayerMobile && from.InRange(GetWorldLocation(), 2) && from.InLOS(this))
            {
                from.SendGump(new LesserPuzzleBoxGump((PlayerMobile)from, this));
            }
        }

        private int[] m_Possibles = new int[]
        {
            0, 1, 2,
            3, 4, 5,
            6, 7, 8
        };

        private int[][] _Paths = new int[][]
        {
            new int[] { 0, 1, 2, 5, 8 },
            new int[] { 0, 3, 6, 7, 8 },
            new int[] { 0, 1, 4, 7, 8 },
            new int[] { 0, 3, 4, 5, 8 },
            new int[] { 0, 1, 2, 4, 6, 7, 8 },
            new int[] { 0, 3, 4, 1, 2, 5, 8 },
            new int[] { 0, 1, 4, 3, 6, 7, 8 },
            new int[] { 0, 3, 6, 7, 4, 1, 2, 5, 8 }
        };

        public List<int> GetRandomPath()
        {
            return new List<int>(_Paths[Utility.Random(_Paths.Length)]);
        }

        public void OnPuzzleCompleted(PlayerMobile pm)
        {
            pm.PrivateOverheadMessage(MessageType.Regular, 0x35, false, "You have solved the puzzle and gained a little experience in Remove Trap!", pm.NetState);
            pm.Skills[SkillName.RemoveTrap].Base += 0.1;
            Delete();
        }

        public LesserTrainingPuzzleBox(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int v = reader.ReadInt();

            Path = GetRandomPath();
        }

        public class LesserPuzzleBoxGump : Gump
        {
            public LesserTrainingPuzzleBox Box { get; set; }
            public bool ShowNext { get; set; }
            public PlayerMobile User { get; set; }

            public List<int> Path { get { return Box != null && !Box.Deleted ? Box.Path : null; } }
            public List<int> Progress { get; set; }

            public LesserPuzzleBoxGump(PlayerMobile pm, LesserTrainingPuzzleBox box)
                : base(25, 25)
            {
                Box = box;
                User = pm;

                Progress = new List<int>();
                Progress.Add(0);

                AddGumpLayout();
            }

            public void Refresh()
            {
                Entries.Clear();
                Entries.TrimExcess();
                AddGumpLayout();

                User.CloseGump(this.GetType());
                User.SendGump(this, false);
            }

            public void AddGumpLayout()
            {
                AddBackground(50, 50, 550, 440, 2600);
                AddBackground(100, 75, 450, 90, 2600);
                AddBackground(90, 175, 270, 270, 2600);
                AddBackground(100, 185, 250, 250, 5120);
                AddBackground(370, 175, 178, 200, 5120);

                AddImage(145, 95, 10451);
                AddImage(435, 95, 10451);
                AddImage(0, 50, 10400);
                AddImage(0, 200, 10401);
                AddImage(0, 360, 10402);

                AddImage(565, 50, 10410);
                AddImage(565, 200, 10411);
                AddImage(565, 360, 10412);

                AddImage(370, 175, 10452);
                AddImage(370, 360, 10452);

                AddImageTiled(125, 207, 130, 3, 5031);
                AddImageTiled(125, 287, 130, 3, 5031);

                AddImageTiled(123, 205, 3, 130, 5031);
                AddImageTiled(203, 205, 3, 130, 5031);

                AddImage(420, 250, 1417);
                AddImage(440, 270, 2642);

                AddHtml(220, 90, 210, 16, "<center>Lesser Training Puzzle Box</center>", false, false);
                AddHtml(220, 112, 210, 16, "<center>Use the Directional Controls to</center>", false, false);
                AddHtml(220, 131, 210, 16, "<center>Solve the Puzzle</center>", false, false);

                int x = 110;
                int y = 195;
                int xOffset = 0;
                int yOffset = 0;

                for (int i = 0; i < 9; i++)
                {
                    int itemID = Progress.Contains(i) ? 2152 : 9720;

                    if (i < 3)
                    {
                        xOffset = x + (40 * i);
                        yOffset = y;
                    }
                    else if (i < 6)
                    {
                        xOffset = x + (40 * (i - 3));
                        yOffset = y + 40;
                    }
                    else if (i < 9)
                    {
                        xOffset = x + (40 * (i - 6));
                        yOffset = y + 80;
                    }

                    AddImage(xOffset, yOffset, itemID);

                    if (i == Progress[Progress.Count - 1])
                        AddImage(xOffset + 8, yOffset + 8, 5032);

                    if (ShowNext && Progress.Count <= Path.Count && i == Path[Progress.Count])
                        AddImage(xOffset + 8, yOffset + 8, 2361);
                }

                ShowNext = false;

                if (User.Skills[SkillName.RemoveTrap].Base >= 30)
                {
                    AddHtml(410, 415, 150, 32, "Attempt to pick the lock", false, false);
                    AddButton(370, 415, 4005, 4005, 5, GumpButtonType.Reply, 0);
                }

                AddButton(453, 245, 10700, 10701, 1, GumpButtonType.Reply, 0); // up
                AddButton(478, 281, 10710, 10711, 2, GumpButtonType.Reply, 0); // right
                AddButton(453, 305, 10720, 10721, 3, GumpButtonType.Reply, 0); // down 
                AddButton(413, 281, 10730, 10731, 4, GumpButtonType.Reply, 0); // left
            }

            public override void OnResponse(NetState state, RelayInfo info)
            {
                if (info.ButtonID > 0 && info.ButtonID <= 5)
                    HandleButton(info.ButtonID);
            }

            public void HandleButton(int id)
            {
                if (Box == null || Box.Deleted)
                    return;

                if (id > 0 && id < 5)
                {
                    int current = Progress[Progress.Count - 1];
                    int next = 8;
                    int pick;

                    if (Progress.Count >= 0 && Progress.Count < Path.Count)
                        next = Path[Progress.Count];

                    switch (id)
                    {
                        default:
                        case 1: pick = current - 3; break;
                        case 2: pick = current + 1; break;
                        case 3: pick = current + 3; break;
                        case 4: pick = current - 1; break;
                    }

                    if (Progress.Contains(pick) || pick < 0 || pick > 8)
                    {
                        User.PlaySound(0x5B6);
                        Refresh();
                    }
                    else if ((current == 5 || current == 7) && pick == 8)
                    {
                        User.PlaySound(0x3D);
                        Box.OnPuzzleCompleted(User);
                    }
                    else if (pick == next)
                    {
                        Progress.Add(pick);
                        User.PlaySound(0x1F5);
                        Refresh();
                    }
                    else
                    {
                        User.SendLocalizedMessage(1054015); // You make a mistake and reset the mechanism.
                        ClearProgress();
                    }
                }
                else if (id == 5)
                {
                    ShowNext = true;
                    Refresh();
                }
            }

            public void ClearProgress()
            {
                Progress.Clear();
                Progress.Add(0);
            }
        }
    }
}