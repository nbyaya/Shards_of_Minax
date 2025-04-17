using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Server;
using Server.Commands;
using Server.Gumps;
using Server.Mobiles;
using VitaNex.Items;
using VitaNex.SuperGumps;

namespace Server.ACC.CSS.Systems.LumberjackingMagic
{
    // Revised Lumberjacking Skill Tree Gump using AncientKnowledge as the cost resource.
    public class LumberjackingSkillTree : SuperGump
    {
        private LumberjackingTree tree;
        private Dictionary<SkillNode, Point2D> nodePositions;
        private Dictionary<int, SkillNode> buttonNodeMap;
        private Dictionary<SkillNode, int> edgeThickness;
        private const int buttonSize = 15;
        private int ySpacing = 50, xSpacing = 60;
        private int rootX = 300, rootY = 150;
        private SkillNode selectedNode;

        public LumberjackingSkillTree(PlayerMobile user)
            : base(user, null, 100, 100)
        {
            tree = new LumberjackingTree(user);
            nodePositions = new Dictionary<SkillNode, Point2D>();
            buttonNodeMap = new Dictionary<int, SkillNode>();
            edgeThickness = new Dictionary<SkillNode, int>();

            CalculateNodePositions(tree.Root, rootX, rootY, 0);
            InitializeEdgeThickness();

            User.SendGump(this);
        }

        private void CalculateNodePositions(SkillNode root, int x, int y, int depth)
        {
            if (root == null)
                return;

            var levelNodes = new Dictionary<int, List<SkillNode>>();
            var queue = new Queue<(SkillNode node, int level)>();

            // This HashSet will ensure each node is only placed once.
            var visited = new HashSet<SkillNode>();

            queue.Enqueue((root, 0));

            while (queue.Count > 0)
            {
                var (node, level) = queue.Dequeue();

                // If we've already visited this node, skip it.
                if (!visited.Add(node))
                    continue;

                if (!levelNodes.ContainsKey(level))
                    levelNodes[level] = new List<SkillNode>();

                levelNodes[level].Add(node);

                foreach (var child in node.Children)
                {
                    queue.Enqueue((child, level + 1));
                }
            }

            // Now position each level's nodes centered around rootX
            foreach (var kvp in levelNodes)
            {
                int level = kvp.Key;
                var nodes = kvp.Value;

                // Spread them out horizontally based on how many nodes are in this level
                int totalWidth = (nodes.Count - 1) * xSpacing;
                int startX = rootX - (totalWidth / 2);

                for (int i = 0; i < nodes.Count; i++)
                {
                    int nodeX = startX + (i * xSpacing);
                    int nodeY = rootY + (level * ySpacing);

                    nodePositions[nodes[i]] = new Point2D(nodeX, nodeY);
                }
            }
        }

        private void InitializeEdgeThickness()
        {
            foreach (var node in nodePositions.Keys)
                edgeThickness[node] = 2;
        }

        protected override void CompileLayout(SuperGumpLayout layout)
        {
            layout.Add("background", () => { AddImage(0, 0, 30236); });
            layout.Add("title", () => { AddLabel(100, 20, 1153, "Lumberjacking Skill Tree"); });

            layout.Add("selectedNodeText", () =>
            {
                if (selectedNode != null)
                {
                    PlayerMobile player = User as PlayerMobile;
                    string text;

                    if (selectedNode.IsActivated(player))
                        text = $"<BASEFONT COLOR=#FFFFFF>{selectedNode.Name}</BASEFONT>";
                    else if (selectedNode.CanBeActivated(player))
                        text = $"<BASEFONT COLOR=#FFFF00>Click to unlock {selectedNode.Name} (Cost: {selectedNode.Cost} Maxxia Points)</BASEFONT>";
                    else
                        text = $"<BASEFONT COLOR=#FF0000>{selectedNode.Name} Locked! Unlock the previous node first.</BASEFONT>";

                    AddHtml(100, 50, 300, 40, text, false, false);
                }
            });

            // New layout element to display the node's description.
            layout.Add("selectedNodeDescription", () =>
            {
                if (selectedNode != null)
                {
                    string descriptionText = $"<BASEFONT COLOR=#FF0000>{selectedNode.Description}</BASEFONT>";
                    // Adjust y coordinate as needed to display below the node name.
                    AddHtml(100, 90, 300, 40, descriptionText, false, false);
                }
            });

            layout.Add("edges", () =>
            {
                foreach (var node in nodePositions.Keys)
                {
                    foreach (var child in node.Children)
                    {
                        var edgeColor = node.IsActivated(User as PlayerMobile) ? Color.Green : Color.Gray;
                        int thickness = edgeThickness[node];
                        AddLine(nodePositions[node], nodePositions[child], edgeColor, thickness);
                    }
                }
            });

            layout.Add("nodes", () =>
            {
                foreach (var node in nodePositions.Keys)
                {
                    PlayerMobile player = User as PlayerMobile;
                    if (player == null)
                        continue;

                    bool activated = node.IsActivated(player);
                    int buttonID = node.BitFlag;
                    int buttonGumpID = activated ? 1652 : 1653;
                    var pos = nodePositions[node];

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
                    selectedNode = node;
                    Refresh(true);
                }
                else
                {
                    if (node.Activate(User as PlayerMobile))
                        edgeThickness[node] = 5;
                    Refresh(true);
                }
            }
        }
    }

    // Revised SkillNode that uses AncientKnowledge for costs.
    public class SkillNode
    {
        public int BitFlag { get; }
        public string Name { get; }
        public int Cost { get; }
        public string Description { get; } // <-- New property for description.
        public List<SkillNode> Children { get; }
        public SkillNode Parent { get; private set; }
        private readonly Action<PlayerMobile> onActivate;

        // Modified constructor to accept a description.
        public SkillNode(int bitFlag, string name, int cost, string description = "", Action<PlayerMobile> onActivate = null)
        {
            BitFlag = bitFlag;
            Name = name;
            Cost = cost;
            Description = description;
            Children = new List<SkillNode>();
            this.onActivate = onActivate;
        }

        public void AddChild(SkillNode child)
        {
            child.Parent = this;
            Children.Add(child);
        }

        public bool IsActivated(PlayerMobile player)
        {
            var profile = player.AcquireTalents();
            if (!profile.Talents.ContainsKey(TalentID.LumberjackingNodes))
                profile.Talents[TalentID.LumberjackingNodes] = new Talent(TalentID.LumberjackingNodes) { Points = 0 };

            return (profile.Talents[TalentID.LumberjackingNodes].Points & BitFlag) != 0;
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

            // Use AncientKnowledge points for unlocking.
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
            profile.Talents[TalentID.LumberjackingNodes].Points |= BitFlag;

            player.SendMessage($"{Name} activated!");
            onActivate?.Invoke(player);
            return true;
        }
    }

    // Full Lumberjacking tree structure with multiple layers and over 30 nodes.
    public class LumberjackingTree
    {
        public SkillNode Root { get; }

        public LumberjackingTree(PlayerMobile player)
        {
            var profile = player.AcquireTalents();
            int nodeIndex = 0x01;

            // Layer 0: Root Node – Unlocks basic lumberjacking spells.
            Root = new SkillNode(nodeIndex, "Call of the Forest", 5, "Unlocks basic lumberjacking spells", (p) =>
            {
                // Unlock basic spells.
                profile.Talents[TalentID.LumberjackingSpells].Points |= 0x01;
            });

            // Layer 1: Basic bonuses.
            nodeIndex <<= 1;
            var woodSense = new SkillNode(nodeIndex, "Wood Sense", 6, "Increases harvest range", (p) =>
            {
                // Increase harvest range.
                profile.Talents[TalentID.LumberjackingRange].Points += 1;
            });

            nodeIndex <<= 1;
            // Modified: now unlocks spell bit 0x02 instead of adding efficiency bonus.
            var efficientChopping = new SkillNode(nodeIndex, "Efficient Chopping", 6, "Unlocks Efficient Chopping Spell", (p) =>
            {
                profile.Talents[TalentID.LumberjackingSpells].Points |= 0x02;
            });

            nodeIndex <<= 1;
            var boardMastery = new SkillNode(nodeIndex, "Board Conversion Mastery", 6, "Unlocks bonus board conversion spell", (p) =>
            {
                // Unlock bonus board conversion spell.
                profile.Talents[TalentID.LumberjackingSpells].Points |= 0x04;
            });

            nodeIndex <<= 1;
            var naturesBounty = new SkillNode(nodeIndex, "Nature's Bounty", 6, "Increases harvest yield", (p) =>
            {
                // Increase harvest yield.
                profile.Talents[TalentID.LumberjackingYield].Points += 1;
            });

            // Attach Layer 1 nodes to Root.
            Root.AddChild(woodSense);
            Root.AddChild(efficientChopping);
            Root.AddChild(boardMastery);
            Root.AddChild(naturesBounty);

            // Layer 2: Advanced magical and practical bonuses.
            nodeIndex <<= 1;
            var forestWhisper = new SkillNode(nodeIndex, "Forest Whisper", 7, "Unlocks additional spells", (p) =>
            {
                profile.Talents[TalentID.LumberjackingSpells].Points |= 0x08;
            });

            nodeIndex <<= 1;
            var timberFlow = new SkillNode(nodeIndex, "Timber Flow", 7, "Improves chopping efficiency further", (p) =>
            {
                profile.Talents[TalentID.LumberjackingEfficiency].Points += 1;
            });

            nodeIndex <<= 1;
            var arcaneTimber = new SkillNode(nodeIndex, "Arcane Timber", 7, "Unlocks advanced lumberjacking spells", (p) =>
            {
                profile.Talents[TalentID.LumberjackingSpells].Points |= 0x10;
            });

            nodeIndex <<= 1;
            var deepRoots = new SkillNode(nodeIndex, "Deep Roots", 7, "Increases harvest range further", (p) =>
            {
                profile.Talents[TalentID.LumberjackingRange].Points += 1;
            });

            // Attach Layer 2 nodes.
            woodSense.AddChild(forestWhisper);
            efficientChopping.AddChild(timberFlow);
            boardMastery.AddChild(arcaneTimber);
            naturesBounty.AddChild(deepRoots);

            // Layer 3: Further yield and efficiency improvements.
            nodeIndex <<= 1;
            var bountifulGrove = new SkillNode(nodeIndex, "Bountiful Grove", 8, "Enhances wood yield", (p) =>
            {
                profile.Talents[TalentID.LumberjackingYield].Points += 1;
            });

            nodeIndex <<= 1;
            var razorSap = new SkillNode(nodeIndex, "Razor Sap", 8, "Further improves chopping efficiency", (p) =>
            {
                profile.Talents[TalentID.LumberjackingEfficiency].Points += 1;
            });

            nodeIndex <<= 1;
            var woodenFortitude = new SkillNode(nodeIndex, "Wooden Fortitude", 8, "Unlocks a fortitude bonus", (p) =>
            {
                profile.Talents[TalentID.LumberjackingSpells].Points |= 0x20;
            });

            nodeIndex <<= 1;
            var naturalReflexes = new SkillNode(nodeIndex, "Natural Reflexes", 8, "Improves reaction speed", (p) =>
            {
                profile.Talents[TalentID.LumberjackingEfficiency].Points += 1;
            });

            // Attach Layer 3 nodes.
            forestWhisper.AddChild(bountifulGrove);
            timberFlow.AddChild(razorSap);
            arcaneTimber.AddChild(woodenFortitude);
            deepRoots.AddChild(naturalReflexes);

            // Layer 4: More advanced magical enhancements.
            nodeIndex <<= 1;
            var treesBlessing = new SkillNode(nodeIndex, "Tree's Blessing", 9, "Enhances wood yield further", (p) =>
            {
                profile.Talents[TalentID.LumberjackingYield].Points += 1;
            });

            nodeIndex <<= 1;
            var faesGrace = new SkillNode(nodeIndex, "Fae's Grace", 9, "Unlocks fae-related bonus spells", (p) =>
            {
                profile.Talents[TalentID.LumberjackingSpells].Points |= 0x40;
            });

            nodeIndex <<= 1;
            var elderTimber = new SkillNode(nodeIndex, "Elder Timber", 9, "Unlocks elder wood spells", (p) =>
            {
                profile.Talents[TalentID.LumberjackingSpells].Points |= 0x80;
            });

            nodeIndex <<= 1;
            var sylvanSurge = new SkillNode(nodeIndex, "Sylvan Surge", 9, "Boosts lumberjacking range", (p) =>
            {
                profile.Talents[TalentID.LumberjackingRange].Points += 1;
            });

            // Attach Layer 4 nodes.
            bountifulGrove.AddChild(treesBlessing);
            razorSap.AddChild(faesGrace);
            woodenFortitude.AddChild(elderTimber);
            naturalReflexes.AddChild(sylvanSurge);

            // Layer 5: Expert-level nodes.
            nodeIndex <<= 1;
            var primevalEfficiency = new SkillNode(nodeIndex, "Primeval Efficiency", 10, "Boosts overall efficiency", (p) =>
            {
                profile.Talents[TalentID.LumberjackingEfficiency].Points += 1;
            });

            nodeIndex <<= 1;
            // Modified: now unlocks spell bit 0x2000 instead of boosting yield.
            var bountifulHarvest = new SkillNode(nodeIndex, "Bountiful Harvest", 10, "Unlocks Bountiful Harvest Spell", (p) =>
            {
                profile.Talents[TalentID.LumberjackingSpells].Points |= 0x2000;
            });

            nodeIndex <<= 1;
            var logMastery = new SkillNode(nodeIndex, "Log Mastery", 10, "Unlocks mastery level spells", (p) =>
            {
                profile.Talents[TalentID.LumberjackingSpells].Points |= 0x100;
            });

            nodeIndex <<= 1;
            var chopChopMomentum = new SkillNode(nodeIndex, "Chop-Chop Momentum", 10, "Increases chopping momentum", (p) =>
            {
                profile.Talents[TalentID.LumberjackingEfficiency].Points += 1;
            });

            // Attach Layer 5 nodes.
            treesBlessing.AddChild(primevalEfficiency);
            faesGrace.AddChild(bountifulHarvest);
            elderTimber.AddChild(logMastery);
            sylvanSurge.AddChild(chopChopMomentum);

            // Layer 6: Mastery nodes.
            nodeIndex <<= 1;
            var expandedPerception = new SkillNode(nodeIndex, "Expanded Perception", 11, "Enhances spatial awareness", (p) =>
            {
                profile.Talents[TalentID.LumberjackingRange].Points += 1;
            });

            nodeIndex <<= 1;
            // Modified: now unlocks spell bit 0x4000 instead of boosting yield.
            var mysticSapling = new SkillNode(nodeIndex, "Mystic Sapling", 11, "Unlocks Mystic Sapling Spell", (p) =>
            {
                profile.Talents[TalentID.LumberjackingSpells].Points |= 0x4000;
            });

            nodeIndex <<= 1;
            var ancientArborist = new SkillNode(nodeIndex, "Ancient Arborist", 11, "Unlocks ancient lumberjacking spells", (p) =>
            {
                profile.Talents[TalentID.LumberjackingSpells].Points |= 0x200;
            });

            nodeIndex <<= 1;
            var timberTransformation = new SkillNode(nodeIndex, "Timber Transformation", 11, "Increases efficiency with magic", (p) =>
            {
                profile.Talents[TalentID.LumberjackingEfficiency].Points += 1;
            });

            // Attach Layer 6 nodes.
            primevalEfficiency.AddChild(expandedPerception);
            bountifulHarvest.AddChild(mysticSapling);
            logMastery.AddChild(ancientArborist);
            chopChopMomentum.AddChild(timberTransformation);

            // Layer 7: Final, pinnacle bonuses.
            nodeIndex <<= 1;
            var barkBarrier = new SkillNode(nodeIndex, "Bark Barrier", 12, "Provides a protective barrier", (p) =>
            {
                profile.Talents[TalentID.LumberjackingSpells].Points |= 0x400;
            });

            nodeIndex <<= 1;
            var naturesEndowment = new SkillNode(nodeIndex, "Nature's Endowment", 12, "Further increases wood yield", (p) =>
            {
                profile.Talents[TalentID.LumberjackingYield].Points += 1;
            });

            nodeIndex <<= 1;
            // Modified: now unlocks spell bit 0x8000 instead of boosting efficiency.
            var forestsFury = new SkillNode(nodeIndex, "Forest's Fury", 12, "Unlocks Forest's Fury Spell", (p) =>
            {
                profile.Talents[TalentID.LumberjackingSpells].Points |= 0x8000;
            });

            nodeIndex <<= 1;
            var echoesOfTheWild = new SkillNode(nodeIndex, "Echoes of the Wild", 12, "Enhances range with wild power", (p) =>
            {
                profile.Talents[TalentID.LumberjackingRange].Points += 1;
            });

            // Attach Layer 7 nodes.
            expandedPerception.AddChild(barkBarrier);
            mysticSapling.AddChild(naturesEndowment);
            ancientArborist.AddChild(forestsFury);
            timberTransformation.AddChild(echoesOfTheWild);

            // Layer 8: Ultimate node.
            nodeIndex <<= 1;
            var ultimateArborist = new SkillNode(nodeIndex, "Ultimate Arborist", 13, "Ultimate bonus: boosts all lumberjacking skills", (p) =>
            {
                // Grants a final bonus to spells and all bonuses.
                profile.Talents[TalentID.LumberjackingSpells].Points |= 0x800 | 0x1000;
                profile.Talents[TalentID.LumberjackingRange].Points += 1;
                profile.Talents[TalentID.LumberjackingEfficiency].Points += 1;
                profile.Talents[TalentID.LumberjackingYield].Points += 1;
            });

            // Attach the ultimate node to all Layer 7 nodes.
            foreach (var node in new[] { barkBarrier, echoesOfTheWild, forestsFury, naturesEndowment })
            {
                node.AddChild(ultimateArborist);
            }
        }
    }

    // Command to open the Lumberjacking Skill Tree.
    public class LumberjackingSkillTreeCommand
    {
        public static void Initialize()
        {
            CommandSystem.Register("LumberTree", AccessLevel.Player, cmd => ShowTree(cmd.Mobile));
        }

        private static void ShowTree(Mobile m)
        {
            if (m is PlayerMobile pm)
            {
                pm.SendMessage("Opening Lumberjacking Skill Tree...");
                pm.SendGump(new LumberjackingSkillTree(pm));
            }
            else
            {
                m.SendMessage("You must be a player to use this command.");
            }
        }
    }
}
