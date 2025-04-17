using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Server;
using Server.Commands;
using Server.Gumps;
using Server.Mobiles;
using Server.Spells.Third;
using VitaNex.Items;
using VitaNex.SuperGumps;

public class StealingSkillTree : SuperGump
{
    private SkillTree skillTree;
    private Dictionary<SkillNode, Point2D> nodePositions;
    private Dictionary<int, SkillNode> buttonNodeMap;
    private Dictionary<SkillNode, int> edgeThickness;
    private const int buttonSize = 15;
    private int ySpacing = 50, xSpacing = 60;
    private int rootX = 300, rootY = 100;
	private SkillNode selectedNode;

    public StealingSkillTree(PlayerMobile user) : base(user, null, 100, 100)
    {
        skillTree = new SkillTree(user);
        nodePositions = new Dictionary<SkillNode, Point2D>();
        buttonNodeMap = new Dictionary<int, SkillNode>();
        edgeThickness = new Dictionary<SkillNode, int>();

        CalculateNodePositions(skillTree.Root, rootX, rootY, 0);
        InitializeEdgeThickness();

        User.SendGump(this);
    }
    private void CalculateNodePositions(SkillNode root, int x, int y, int depth)
    {
        if (root == null) return;

        // Dictionary to store node positions by depth level
        Dictionary<int, List<SkillNode>> levelNodes = new Dictionary<int, List<SkillNode>>();
        Dictionary<SkillNode, List<int>> nodeParentsX = new Dictionary<SkillNode, List<int>>();
        HashSet<SkillNode> visited = new HashSet<SkillNode>();

        Queue<(SkillNode node, int depth, int parentX)> queue = new Queue<(SkillNode, int, int)>();

        // Initialize with root
        queue.Enqueue((root, 0, rootX));

        while (queue.Count > 0)
        {
            var (node, level, parentX) = queue.Dequeue();

            if (!levelNodes.ContainsKey(level))
                levelNodes[level] = new List<SkillNode>();

            levelNodes[level].Add(node);

            // Store parent positions for nodes with multiple parents
            if (!nodeParentsX.ContainsKey(node))
                nodeParentsX[node] = new List<int>();

            nodeParentsX[node].Add(parentX);

            // Avoid duplicate processing
            if (visited.Contains(node)) continue;
            visited.Add(node);

            // Process children
            foreach (var child in node.Children)
            {
                queue.Enqueue((child, level + 1, parentX));  // Pass parent's X position
            }
        }

        // Assign X positions
        foreach (var kvp in levelNodes)
        {
            int level = kvp.Key;
            List<SkillNode> nodes = kvp.Value;

            int totalWidth = (nodes.Count - 1) * xSpacing;
            int startX = rootX - (totalWidth / 2);

            for (int i = 0; i < nodes.Count; i++)
            {
                SkillNode node = nodes[i];

                // If a node has multiple parents, take the average of parent X positions
                int nodeX;
                if (nodeParentsX[node].Count > 1)
                {
                    nodeX = (int)nodeParentsX[node].Average();
                }
                else
                {
                    nodeX = startX + (i * xSpacing);
                }

                int nodeY = rootY + (level * ySpacing);
                nodePositions[node] = new Point2D(nodeX, nodeY);
            }
        }

    }

    private void InitializeEdgeThickness()
    {
        foreach (var node in nodePositions.Keys)
        {
            edgeThickness[node] = 2; // Default thickness
        }
    }
	protected override void CompileLayout(SuperGumpLayout layout)
	{
		// Background image
		layout.Add("background", () => { AddImage(0, 0, 30236); });

		// Title at the top
		layout.Add("title", () => { AddLabel(100, 20, 1153, "Stealing Skill Tree"); });

		// Fixed text under the title for the selected node
		layout.Add("selectedNodeText", () =>
		{
			if (selectedNode != null)
			{
				PlayerMobile player = User as PlayerMobile;
				string text;

				if (selectedNode.IsActivated(player))
				{
					text = $"<BASEFONT COLOR=#FFFFFF>{selectedNode.Name}</BASEFONT>";
				}
				else if (selectedNode.CanBeActivated(player))
				{
					text = $"<BASEFONT COLOR=#FFFF00>Click to unlock {selectedNode.Name} (Cost: {selectedNode.Cost} Maxxia points)</BASEFONT>";
				}
				else
				{
					text = $"<BASEFONT COLOR=#FF0000>{selectedNode.Name} Locked! Unlock the previous node first.</BASEFONT>";
				}

				AddHtml(100, 50, 300, 40, text, false, false);
			}
		});

		// Edges connecting the nodes
		layout.Add("edges", () =>
		{
			foreach (var node in nodePositions.Keys)
			{
				foreach (var child in node.Children)
				{
					Color edgeColor = node.IsActivated(User as PlayerMobile) ? Color.Red : Color.Gray;
					int thickness = edgeThickness[node];
					AddLine(nodePositions[node], nodePositions[child], edgeColor, thickness);
				}
			}
		});

		// Node buttons without text
		layout.Add("nodes", () =>
		{
			foreach (var node in nodePositions.Keys)
			{
				PlayerMobile player = User as PlayerMobile;
				if (player == null) continue;

				bool isActivated = node.IsActivated(player);
				int buttonID = node.BitFlag;
				int buttonGumpID = isActivated ? 1652 : 1653;
				Point2D pos = nodePositions[node];

				AddButton(pos.X - buttonSize / 2, pos.Y - buttonSize / 2, buttonGumpID, buttonGumpID, buttonID, GumpButtonType.Reply, 0);
				buttonNodeMap[buttonID] = node;
			}
		});
	}
	public override void HandleButtonClick(GumpButton button)
	{
		if (buttonNodeMap.TryGetValue(button.ButtonID, out SkillNode node))
		{
			if (selectedNode != node)
			{
				// First click: select the node and show its text
				selectedNode = node;
				Refresh(true);
			}
			else
			{
				// Second click on the same node: attempt activation
				if (node.Activate(User as PlayerMobile))
				{
					// Activation successful, update edge thickness
					edgeThickness[node] = 5; // Sets thickness for edges from this node to its children
				}
				Refresh(true); // Update text and edges
			}
		}
	}
}

public class SkillNode
{
    public int BitFlag { get; }  // Each node represents a unique bit (0x01, 0x02, 0x04, ...)
    public string Name { get; }
    public int Cost { get; }
    public List<SkillNode> Children { get; }
    public SkillNode Parent { get; private set; }
    private Action<PlayerMobile> OnActivate;

    public SkillNode(int bitFlag, string name, int cost, Action<PlayerMobile> onActivate = null)
    {
        BitFlag = bitFlag;
        Name = name;
        Cost = cost;
        Children = new List<SkillNode>();
        OnActivate = onActivate;
    }

    public void AddChild(SkillNode child)
    {
        child.Parent = this;
        Children.Add(child);
    }

    public bool IsActivated(PlayerMobile player)
    {
        var profile = player.AcquireTalents();

        // ✅ Ensure SkillNodes1 exists before accessing it
        if (!profile.Talents.ContainsKey(TalentID.StealingNodes))
        {
            profile.Talents[TalentID.StealingNodes] = new Talent(TalentID.StealingNodes) { Points = 0 };
        }

        return (profile.Talents[TalentID.StealingNodes].Points & BitFlag) != 0;
    }


    public bool CanBeActivated(PlayerMobile player)
    {
        return Parent == null || Parent.IsActivated(player);
    }

    public bool Activate(PlayerMobile player)
    {
        if (IsActivated(player))
        {
            player.SendMessage($"{Name} is already activated!");
            return false;
        }

        if (!CanBeActivated(player))
        {
            player.SendMessage($"{Name} is locked! Unlock the previous node first.");
            return false;
        }

        var profile = player.AcquireTalents();
        if (!profile.Talents.TryGetValue(TalentID.AncientKnowledge, out var ancientKnowledge))
        {
            player.SendMessage("You have no Maxxia Points points available.");
            return false;
        }

        if (ancientKnowledge.Points < Cost)
        {
            player.SendMessage($"You need {Cost} Maxxia Points points to unlock {Name}.");
            return false;
        }

        ancientKnowledge.Points -= Cost;

        // Use bitwise OR to store the unlocked node
        profile.Talents[TalentID.StealingNodes].Points |= BitFlag;

        player.SendMessage($"{Name} activated!");
        OnActivate?.Invoke(player);
        return true;
    }
}

public class SkillTree
{
    public SkillNode Root { get; }

    public SkillTree(PlayerMobile pm)
    {
        var profile = pm.AcquireTalents();
        int nodeIndex = 0x01;

        Root = new SkillNode(nodeIndex, "Call Rogues", 5, (player) =>
        {
            profile.Talents[TalentID.StealingSpells].Points |= 0x01; 
        });

        nodeIndex <<= 1;
        var node1 = new SkillNode(nodeIndex, "Shadow Strike", 5, (player) =>
        {
            profile.Talents[TalentID.StealingSpells].Points |= 0x02;
        });

        nodeIndex <<= 1;
        var node2 = new SkillNode(nodeIndex, "Disorienting Thrust", 5, (player) =>
        {
            profile.Talents[TalentID.StealingSpells].Points |= 0x04;
        });

        nodeIndex <<= 1;
        var node3 = new SkillNode(nodeIndex, "Smoke Bomb", 5, (player) =>
        {
            profile.Talents[TalentID.StealingSpells].Points |= 0x08;
        });

        nodeIndex <<= 1;
        var node4 = new SkillNode(nodeIndex, "Thieving Swipe", 5, (player) =>
        {
            profile.Talents[TalentID.StealingSpells].Points |= 0x10;
        });

        nodeIndex <<= 1;
        var node5 = new SkillNode(nodeIndex, "Backstab", 5, (player) =>
        {
            profile.Talents[TalentID.StealingSpells].Points |= 0x20;
        });

        nodeIndex <<= 1;
        var node6 = new SkillNode(nodeIndex, "Dagger Dance", 5, (player) =>
        {
            profile.Talents[TalentID.StealingSpells].Points |= 0x40;
        });

        nodeIndex <<= 1;
        var node7 = new SkillNode(nodeIndex, "Panic Trap", 5, (player) =>
        {
            profile.Talents[TalentID.StealingSpells].Points |= 0x80;
        });

        nodeIndex <<= 1;
        var node8 = new SkillNode(nodeIndex, "Invisibility Cloak", 5, (player) =>
        {
            profile.Talents[TalentID.StealingSpells].Points |= 0x100;
        });

        nodeIndex <<= 1;
        var node9 = new SkillNode(nodeIndex, "Pickpocket", 5, (player) =>
        {
            profile.Talents[TalentID.StealingSpells].Points |= 0x200;
        });

        nodeIndex <<= 1;
        var node10 = new SkillNode(nodeIndex, "Shadow Meld", 5, (player) =>
        {
            profile.Talents[TalentID.StealingSpells].Points |= 0x400;
        });

        nodeIndex <<= 1;
        var node11 = new SkillNode(nodeIndex, "Disguise Self", 5, (player) =>
        {
            profile.Talents[TalentID.StealingSpells].Points |= 0x800;
        });

        nodeIndex <<= 1;
        var node12 = new SkillNode(nodeIndex, "Escape Artist", 5, (player) =>
        {
            profile.Talents[TalentID.StealingSpells].Points |= 0x1000;
        });

        nodeIndex <<= 1;
        var node13 = new SkillNode(nodeIndex, "Trap Mastery", 5, (player) =>
        {
            profile.Talents[TalentID.StealingSpells].Points |= 0x2000;
        });

        nodeIndex <<= 1;
        var node14 = new SkillNode(nodeIndex, "Evasion Boost", 5, (player) =>
        {
            profile.Talents[TalentID.StealingSpells].Points |= 0x4000;
        });

        nodeIndex <<= 1;
        var node15 = new SkillNode(nodeIndex, "Information Gatherer", 5, (player) =>
        {
            profile.Talents[TalentID.StealingSpells].Points |= 0x8000;
        });

        nodeIndex <<= 1;
        var node16 = new SkillNode(nodeIndex, "Stealing Distance +1", 5, (player) =>
        {
            profile.Talents[TalentID.StealingDistance].Points++;
        });

        nodeIndex <<= 1;
        var node17 = new SkillNode(nodeIndex, "Stealing Weight +10", 5, (player) =>
        {
            profile.Talents[TalentID.StealingWeight].Points++;
        });

        nodeIndex <<= 1;
        var node18 = new SkillNode(nodeIndex, "Stealing Weight +10", 5, (player) =>
        {
            profile.Talents[TalentID.StealingWeight].Points++;
        });

        nodeIndex <<= 1;
        var node19 = new SkillNode(nodeIndex, "Stealing Distance +1", 5, (player) =>
        {
            profile.Talents[TalentID.StealingDistance].Points++;
        });

        nodeIndex <<= 1;
        var node20 = new SkillNode(nodeIndex, "Stealing Distance +1", 5, (player) =>
        {
            profile.Talents[TalentID.StealingDistance].Points++;
        });

        nodeIndex <<= 1;
        var node21 = new SkillNode(nodeIndex, "Stealing Weight +10", 5, (player) =>
        {
            profile.Talents[TalentID.StealingWeight].Points++;
        });

        nodeIndex <<= 1;
        var node22 = new SkillNode(nodeIndex, "Stealing Distance +1", 5, (player) =>
        {
            profile.Talents[TalentID.StealingDistance].Points++;
        });

        nodeIndex <<= 1;
        var node23 = new SkillNode(nodeIndex, "Stealing Distance +1", 5, (player) =>
        {
            profile.Talents[TalentID.StealingDistance].Points++;
        });

        nodeIndex <<= 1;
        var node24 = new SkillNode(nodeIndex, "Stealing Distance +1", 5, (player) =>
        {
            profile.Talents[TalentID.StealingDistance].Points++;
        });

        nodeIndex <<= 1;
        var node25 = new SkillNode(nodeIndex, "Stealing Distance +1", 5, (player) =>
        {
            profile.Talents[TalentID.StealingDistance].Points++;
        });

        nodeIndex <<= 1;
        var node26 = new SkillNode(nodeIndex, "Allows Stealing of equipped items", 5, (player) =>
        {
            profile.Talents[TalentID.StealingEquipped].Points++;
        });

        nodeIndex <<= 1;
        var node27 = new SkillNode(nodeIndex, "Stealing Distance +1", 5, (player) =>
        {
            profile.Talents[TalentID.StealingDistance].Points++;
        });

        nodeIndex <<= 1;
        var node28 = new SkillNode(nodeIndex, "Stealing Distance +1", 5, (player) =>
        {
            profile.Talents[TalentID.StealingDistance].Points++;
        });

        nodeIndex <<= 1;
        var node29 = new SkillNode(nodeIndex, "Stealing Distance +1", 5, (player) =>
        {
            profile.Talents[TalentID.StealingDistance].Points++;
        });

        nodeIndex <<= 1;
        var node30 = new SkillNode(nodeIndex, "Trying to steal immovable items will have a chance to create a copy of the item", 5, (player) =>
        {
            profile.Talents[TalentID.StealingImmovable].Points++;
        });

        nodeIndex <<= 1;
        var node31 = new SkillNode(nodeIndex, "Stealing now can break Line of Sight", 5, (player) =>
        {
            profile.Talents[TalentID.StealingLoS].Points++;
        });

        // Root Layer
        Root.AddChild(node1);
        Root.AddChild(node2);
        Root.AddChild(node3);

        // Layer 2
        node2.AddChild(node4);
        node2.AddChild(node5);

        node3.AddChild(node6);
        node3.AddChild(node7);

        // Layer 3
        node4.AddChild(node8);
        node4.AddChild(node9);

        node6.AddChild(node10);

        node7.AddChild(node11);

        // Layer 4
        node9.AddChild(node12);

        node10.AddChild(node13);

        node11.AddChild(node14);
        node11.AddChild(node15);

        // Layer 5
        node12.AddChild(node16);
        node13.AddChild(node16);
        node14.AddChild(node16);
        node15.AddChild(node16);

        // Layer 6
        node16.AddChild(node17);
        node16.AddChild(node18);
        node16.AddChild(node19);

        // Layer 7
        node17.AddChild(node20);
        node17.AddChild(node21);

        node18.AddChild(node21);
        node18.AddChild(node22);
        node18.AddChild(node23);
        node18.AddChild(node24);

        // Layer 8
        node20.AddChild(node25);

        node21.AddChild(node26);

        node22.AddChild(node27);

        // Layer 9
        node25.AddChild(node28);
        node25.AddChild(node29);
        node25.AddChild(node30);
        node25.AddChild(node31);
    }
}

public class StealingSkillTreeCommand
{
    public static void Initialize()
    {
        CommandSystem.Register("SkillTree", AccessLevel.Player, cmd => ShowTestGump(cmd.Mobile));
    }

    private static void ShowTestGump(Mobile m)
    {
        if (m is PlayerMobile pm)
        {
            pm.SendMessage("Opening StealingSkillTree..."); // Consider changing this message, or removing it completely
            pm.SendGump(new StealingSkillTree(pm));
        }
        else
        {
            m.SendMessage("You must be a player to use this command.");
        }
    }
}