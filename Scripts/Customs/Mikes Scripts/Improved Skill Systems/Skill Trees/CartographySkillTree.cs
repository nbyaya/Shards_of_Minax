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

namespace Server.ACC.CSS.Systems.CartographyMagic
{
    // The Cartography skill tree gump.
    public class CartographySkillTree : SuperGump
    {
        private CartographyTree tree;
        private Dictionary<SkillNode, Point2D> nodePositions;
        private Dictionary<int, SkillNode> buttonNodeMap;
        private Dictionary<SkillNode, int> edgeThickness;
        private const int buttonSize = 15;
        private int ySpacing = 50, xSpacing = 60;
        private int rootX = 300, rootY = 150;
        private SkillNode selectedNode;

        public CartographySkillTree(PlayerMobile user)
            : base(user, null, 100, 100)
        {
            tree = new CartographyTree(user);
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

            // Ensure each node is only placed once.
            var visited = new HashSet<SkillNode>();
            queue.Enqueue((root, 0));

            while (queue.Count > 0)
            {
                var (node, level) = queue.Dequeue();
                if (!visited.Add(node))
                    continue;

                if (!levelNodes.ContainsKey(level))
                    levelNodes[level] = new List<SkillNode>();

                levelNodes[level].Add(node);

                foreach (var child in node.Children)
                    queue.Enqueue((child, level + 1));
            }

            // Center each level’s nodes around the rootX.
            foreach (var kvp in levelNodes)
            {
                int level = kvp.Key;
                var nodes = kvp.Value;
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
            layout.Add("title", () => { AddLabel(100, 20, 1153, "Cartography Skill Tree"); });

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

            // Display node description.
            layout.Add("selectedNodeDescription", () =>
            {
                if (selectedNode != null)
                {
                    string descriptionText = $"<BASEFONT COLOR=#FF0000>{selectedNode.Description}</BASEFONT>";
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

    // The generic SkillNode class.
    public class SkillNode
    {
        public int BitFlag { get; }
        public string Name { get; }
        public int Cost { get; }
        public string Description { get; }
        public List<SkillNode> Children { get; }
        public SkillNode Parent { get; private set; }
        private readonly Action<PlayerMobile> onActivate;

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
            if (!profile.Talents.ContainsKey(TalentID.CartographyNodes))
                profile.Talents[TalentID.CartographyNodes] = new Talent(TalentID.CartographyNodes) { Points = 0 };

            return (profile.Talents[TalentID.CartographyNodes].Points & BitFlag) != 0;
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
                player.SendMessage("You have no Maxxia Points available.");
                return false;
            }

            if (ancientKnowledge.Points < Cost)
            {
                player.SendMessage($"You need {Cost} Maxxia Points to unlock {Name}.");
                return false;
            }

            ancientKnowledge.Points -= Cost;
            profile.Talents[TalentID.CartographyNodes].Points |= BitFlag;

            player.SendMessage($"{Name} activated!");
            onActivate?.Invoke(player);
            return true;
        }
    }

    // The Cartography tree structure with eight layers (plus ultimate node).
    public class CartographyTree
    {
        public SkillNode Root { get; }

        public CartographyTree(PlayerMobile player)
        {
            var profile = player.AcquireTalents();
            int nodeIndex = 0x01;

            // Layer 0: Root Node – Unlocks basic cartography spells.
            Root = new SkillNode(nodeIndex, "Call of the Map", 5, "Unlocks basic cartography spells", (p) =>
            {
                // Set bit 0x01 to unlock basic atlas spells.
                profile.Talents[TalentID.CartographySpells].Points |= 0x01;
            });

            // Layer 1: Basic bonuses / spell unlocks.
            nodeIndex <<= 1;
            // Changed from a passive bonus to a spell unlock: 0x02.
            var compassSense = new SkillNode(nodeIndex, "Compass Sense", 6, "Unlocks a spell", (p) =>
            {
                profile.Talents[TalentID.CartographySpells].Points |= 0x02;
            });

            nodeIndex <<= 1;
            var mapReading = new SkillNode(nodeIndex, "Map Reading", 6, "Enhances map detail perception", (p) =>
            {
                profile.Talents[TalentID.CartographySpells].Points |= 0x04;
            });

            nodeIndex <<= 1;
            var chartingMastery = new SkillNode(nodeIndex, "Charting Mastery", 6, "Unlocks bonus atlas spells", (p) =>
            {
                profile.Talents[TalentID.CartographySpells].Points |= 0x08;
            });

            nodeIndex <<= 1;
            var terrainAwareness = new SkillNode(nodeIndex, "Terrain Awareness", 6, "Increases map detail range", (p) =>
            {
                profile.Talents[TalentID.CartographyMapping].Points += 1;
            });

            Root.AddChild(compassSense);
            Root.AddChild(mapReading);
            Root.AddChild(chartingMastery);
            Root.AddChild(terrainAwareness);

            // Layer 2: Advanced magical and practical bonuses.
            nodeIndex <<= 1;
            var hiddenRoutes = new SkillNode(nodeIndex, "Hidden Routes", 7, "Unlocks additional atlas spells", (p) =>
            {
                profile.Talents[TalentID.CartographySpells].Points |= 0x10;
            });

            nodeIndex <<= 1;
            var windWhisper = new SkillNode(nodeIndex, "Wind Whisper", 7, "Improves spell casting speed", (p) =>
            {
                profile.Talents[TalentID.CartographyEfficiency].Points += 1;
            });

            nodeIndex <<= 1;
            var mysticCoordinates = new SkillNode(nodeIndex, "Mystic Coordinates", 7, "Unlocks advanced atlas spells", (p) =>
            {
                profile.Talents[TalentID.CartographySpells].Points |= 0x20;
            });

            nodeIndex <<= 1;
            var pathfinder = new SkillNode(nodeIndex, "Pathfinder", 7, "Enhances discovery of hidden landmarks", (p) =>
            {
                profile.Talents[TalentID.CartographyMapping].Points += 1;
            });

            compassSense.AddChild(hiddenRoutes);
            mapReading.AddChild(windWhisper);
            chartingMastery.AddChild(mysticCoordinates);
            terrainAwareness.AddChild(pathfinder);

            // Layer 3: Further detail and passive bonuses.
            nodeIndex <<= 1;
            var cartographicVision = new SkillNode(nodeIndex, "Cartographic Vision", 8, "Enhances atlas clarity", (p) =>
            {
                profile.Talents[TalentID.CartographyAccuracy].Points += 1;
            });

            nodeIndex <<= 1;
            var arcaneTopography = new SkillNode(nodeIndex, "Arcane Topography", 8, "Unlocks bonus mapping spells", (p) =>
            {
                profile.Talents[TalentID.CartographySpells].Points |= 0x40;
            });

            nodeIndex <<= 1;
            var legendaryCartography = new SkillNode(nodeIndex, "Legendary Cartography", 8, "Boosts spell potency", (p) =>
            {
                profile.Talents[TalentID.CartographyEfficiency].Points += 1;
            });

            nodeIndex <<= 1;
            var unchartedTerritories = new SkillNode(nodeIndex, "Uncharted Territories", 8, "Expands mapping range", (p) =>
            {
                profile.Talents[TalentID.CartographyMapping].Points += 1;
            });

            hiddenRoutes.AddChild(cartographicVision);
            windWhisper.AddChild(arcaneTopography);
            mysticCoordinates.AddChild(legendaryCartography);
            pathfinder.AddChild(unchartedTerritories);

            // Layer 4: More advanced magical enhancements.
            nodeIndex <<= 1;
            var atlasBlessing = new SkillNode(nodeIndex, "Atlas Blessing", 9, "Enhances overall atlas spells", (p) =>
            {
                profile.Talents[TalentID.CartographySpells].Points |= 0x80;
            });

            nodeIndex <<= 1;
            var guidesGrace = new SkillNode(nodeIndex, "Guide's Grace", 9, "Improves map reading efficiency", (p) =>
            {
                profile.Talents[TalentID.CartographyAccuracy].Points += 1;
            });

            nodeIndex <<= 1;
            var explorersIntuition = new SkillNode(nodeIndex, "Explorer's Intuition", 9, "Increases discovery of landmarks", (p) =>
            {
                profile.Talents[TalentID.CartographyMapping].Points += 1;
            });

            nodeIndex <<= 1;
            var celestialNavigation = new SkillNode(nodeIndex, "Celestial Navigation", 9, "Unlocks celestial atlas spells", (p) =>
            {
                profile.Talents[TalentID.CartographySpells].Points |= 0x100;
            });

            cartographicVision.AddChild(atlasBlessing);
            arcaneTopography.AddChild(guidesGrace);
            legendaryCartography.AddChild(explorersIntuition);
            unchartedTerritories.AddChild(celestialNavigation);

            // Layer 5: Expert-level nodes.
            nodeIndex <<= 1;
            var primeExplorer = new SkillNode(nodeIndex, "Prime Explorer", 10, "Boosts mapping efficiency", (p) =>
            {
                profile.Talents[TalentID.CartographyEfficiency].Points += 1;
            });

            nodeIndex <<= 1;
            var masterCharting = new SkillNode(nodeIndex, "Master Charting", 10, "Enhances atlas spell power", (p) =>
            {
                profile.Talents[TalentID.CartographySpells].Points |= 0x200;
            });

            nodeIndex <<= 1;
            var secretLandmarks = new SkillNode(nodeIndex, "Secret Landmarks", 10, "Reveals hidden map details", (p) =>
            {
                profile.Talents[TalentID.CartographyMapping].Points += 1;
            });

            nodeIndex <<= 1;
            var routeOptimization = new SkillNode(nodeIndex, "Route Optimization", 10, "Improves travel efficiency", (p) =>
            {
                profile.Talents[TalentID.CartographyEfficiency].Points += 1;
            });

            atlasBlessing.AddChild(primeExplorer);
            guidesGrace.AddChild(masterCharting);
            explorersIntuition.AddChild(secretLandmarks);
            celestialNavigation.AddChild(routeOptimization);

            // Layer 6: Mastery nodes.
            nodeIndex <<= 1;
            var expandedHorizons = new SkillNode(nodeIndex, "Expanded Horizons", 11, "Expands visible map range", (p) =>
            {
                profile.Talents[TalentID.CartographyAccuracy].Points += 1;
            });

            nodeIndex <<= 1;
            var mysticMap = new SkillNode(nodeIndex, "Mystic Map", 11, "Boosts atlas spell effects", (p) =>
            {
                profile.Talents[TalentID.CartographySpells].Points |= 0x400;
            });

            nodeIndex <<= 1;
            var ancientCartographer = new SkillNode(nodeIndex, "Ancient Cartographer", 11, "Enhances mapping bonuses", (p) =>
            {
                profile.Talents[TalentID.CartographyMapping].Points += 1;
            });

            nodeIndex <<= 1;
            var terrainTransformation = new SkillNode(nodeIndex, "Terrain Transformation", 11, "Improves travel speed", (p) =>
            {
                profile.Talents[TalentID.CartographyEfficiency].Points += 1;
            });

            primeExplorer.AddChild(expandedHorizons);
            masterCharting.AddChild(mysticMap);
            secretLandmarks.AddChild(ancientCartographer);
            routeOptimization.AddChild(terrainTransformation);

            // Layer 7: Final, pinnacle bonuses.
            nodeIndex <<= 1;
            var barrierOfTheMap = new SkillNode(nodeIndex, "Barrier of the Map", 12, "Provides protective map ward", (p) =>
            {
                profile.Talents[TalentID.CartographySpells].Points |= 0x800;
            });

            nodeIndex <<= 1;
            // Changed from a passive bonus to a spell unlock: 0x4000.
            var cartographersEndowment = new SkillNode(nodeIndex, "Cartographer's Endowment", 12, "Unlocks a spell", (p) =>
            {
                profile.Talents[TalentID.CartographySpells].Points |= 0x4000;
            });

            nodeIndex <<= 1;
            // Changed from a passive bonus to a spell unlock: 0x8000.
            var cartographersFury = new SkillNode(nodeIndex, "Cartographer's Fury", 12, "Unlocks a spell", (p) =>
            {
                profile.Talents[TalentID.CartographySpells].Points |= 0x8000;
            });

            nodeIndex <<= 1;
            var echoesOfTheMap = new SkillNode(nodeIndex, "Echoes of the Map", 12, "Enhances travel range", (p) =>
            {
                profile.Talents[TalentID.CartographyAccuracy].Points += 1;
            });

            expandedHorizons.AddChild(barrierOfTheMap);
            mysticMap.AddChild(cartographersEndowment);
            ancientCartographer.AddChild(cartographersFury);
            terrainTransformation.AddChild(echoesOfTheMap);

            // Layer 8: Ultimate node.
            nodeIndex <<= 1;
            var ultimateCartographer = new SkillNode(nodeIndex, "Ultimate Cartographer", 13, "Ultimate bonus: boosts all cartography skills", (p) =>
            {
                // Unlocks two spell bits: 0x1000 and 0x2000.
                profile.Talents[TalentID.CartographySpells].Points |= 0x1000 | 0x2000;
                profile.Talents[TalentID.CartographyAccuracy].Points += 1;
                profile.Talents[TalentID.CartographyEfficiency].Points += 1;
                profile.Talents[TalentID.CartographyMapping].Points += 1;
            });

            // Attach the ultimate node to all layer 7 nodes.
            foreach (var node in new[] { barrierOfTheMap, echoesOfTheMap, cartographersFury, cartographersEndowment })
            {
                node.AddChild(ultimateCartographer);
            }
        }
    }
}
