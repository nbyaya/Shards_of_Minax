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

namespace Server.ACC.CSS.Systems.CampingMagic
{
    // Revised Camping Skill Tree Gump using AncientKnowledge (Maxxia Points) as the cost resource.
    public class CampingSkillTree : SuperGump
    {
        private CampingTree tree;
        private Dictionary<SkillNode, Point2D> nodePositions;
        private Dictionary<int, SkillNode> buttonNodeMap;
        private Dictionary<SkillNode, int> edgeThickness;
        private const int buttonSize = 15;
        private int ySpacing = 50, xSpacing = 60;
        private int rootX = 300, rootY = 150;
        private SkillNode selectedNode;

        public CampingSkillTree(PlayerMobile user)
            : base(user, null, 100, 100)
        {
            tree = new CampingTree(user);
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
                {
                    queue.Enqueue((child, level + 1));
                }
            }

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
            layout.Add("title", () => { AddLabel(100, 20, 1153, "Camping Skill Tree"); });

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

        // Nested SkillNode class used for both trees
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
                if (!profile.Talents.ContainsKey(TalentID.CampingNodes))
                    profile.Talents[TalentID.CampingNodes] = new Talent(TalentID.CampingNodes) { Points = 0 };

                return (profile.Talents[TalentID.CampingNodes].Points & BitFlag) != 0;
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
                profile.Talents[TalentID.CampingNodes].Points |= BitFlag;

                player.SendMessage($"{Name} activated!");
                onActivate?.Invoke(player);
                return true;
            }
        }

        // The CampingTree structure with 30 nodes (Layers 0 to 8)
        public class CampingTree
        {
            public SkillNode Root { get; }

            public CampingTree(PlayerMobile player)
            {
                var profile = player.AcquireTalents();
                int nodeIndex = 0x01;

                // Layer 0: Root Node – Unlocks basic camping spells.
                Root = new SkillNode(nodeIndex, "Call of the Wild", 5, "Unlocks basic camping spells", (p) =>
                {
                    profile.Talents[TalentID.CampingSpells].Points |= 0x01;
                });

                // Layer 1: Basic camping bonuses.
                nodeIndex <<= 1;
                var warmHearth = new SkillNode(nodeIndex, "Warm Hearth", 6, "Improves campfire efficiency", (p) =>
                {
                    profile.Talents[TalentID.CampingCampfireEfficiency].Points += 1;
                });

                nodeIndex <<= 1;
                // Modified: now unlocks a spell instead of reducing tent setup time.
                var quickSetup = new SkillNode(nodeIndex, "Quick Setup", 6, "Unlocks Quick Setup spell", (p) =>
                {
                    profile.Talents[TalentID.CampingSpells].Points |= 0x02;
                });

                nodeIndex <<= 1;
                var cozyShelter = new SkillNode(nodeIndex, "Cozy Shelter", 6, "Unlocks bonus shelter spell", (p) =>
                {
                    profile.Talents[TalentID.CampingSpells].Points |= 0x04;
                });

                nodeIndex <<= 1;
                var resourcefulness = new SkillNode(nodeIndex, "Resourcefulness", 6, "Increases resource yields when camping", (p) =>
                {
                    profile.Talents[TalentID.CampingRestoration].Points += 1;
                });

                Root.AddChild(warmHearth);
                Root.AddChild(quickSetup);
                Root.AddChild(cozyShelter);
                Root.AddChild(resourcefulness);

                // Layer 2: Advanced active and passive bonuses.
                nodeIndex <<= 1;
                var campfireMastery = new SkillNode(nodeIndex, "Campfire Mastery", 7, "Unlocks additional fire-based spells", (p) =>
                {
                    profile.Talents[TalentID.CampingSpells].Points |= 0x08;
                });

                nodeIndex <<= 1;
                var efficientPacking = new SkillNode(nodeIndex, "Efficient Packing", 7, "Improves travel bonus", (p) =>
                {
                    profile.Talents[TalentID.CampingTravelBonus].Points += 1;
                });

                nodeIndex <<= 1;
                var enhancedInsulation = new SkillNode(nodeIndex, "Enhanced Insulation", 7, "Further reduces setup time", (p) =>
                {
                    profile.Talents[TalentID.CampingTentDuration].Points += 1;
                });

                nodeIndex <<= 1;
                // Modified: now unlocks a spell instead of boosting restorative effects.
                var naturesBoon = new SkillNode(nodeIndex, "Nature's Boon", 7, "Unlocks Nature's Boon spell", (p) =>
                {
                    profile.Talents[TalentID.CampingSpells].Points |= 0x10;
                });

                warmHearth.AddChild(campfireMastery);
                quickSetup.AddChild(efficientPacking);
                cozyShelter.AddChild(enhancedInsulation);
                resourcefulness.AddChild(naturesBoon);

                // Layer 3: Further improvements.
                nodeIndex <<= 1;
                // Modified: now unlocks a spell instead of enhancing travel bonus.
                var starlitVigil = new SkillNode(nodeIndex, "Starlit Vigil", 8, "Unlocks Starlit Vigil spell", (p) =>
                {
                    profile.Talents[TalentID.CampingSpells].Points |= 0x80;
                });

                nodeIndex <<= 1;
                var firelightGlow = new SkillNode(nodeIndex, "Firelight Glow", 8, "Further improves campfire efficiency", (p) =>
                {
                    profile.Talents[TalentID.CampingCampfireEfficiency].Points += 1;
                });

                nodeIndex <<= 1;
                var swiftAssembly = new SkillNode(nodeIndex, "Swift Assembly", 8, "Further reduces tent setup time", (p) =>
                {
                    profile.Talents[TalentID.CampingTentDuration].Points += 1;
                });

                nodeIndex <<= 1;
                var restorativeAura = new SkillNode(nodeIndex, "Restorative Aura", 8, "Unlocks a restorative bonus spell", (p) =>
                {
                    profile.Talents[TalentID.CampingSpells].Points |= 0x20;
                });

                campfireMastery.AddChild(starlitVigil);
                efficientPacking.AddChild(firelightGlow);
                enhancedInsulation.AddChild(swiftAssembly);
                naturesBoon.AddChild(restorativeAura);

                // Layer 4: More magical and passive enhancements.
                nodeIndex <<= 1;
                var emberEchoes = new SkillNode(nodeIndex, "Ember Echoes", 9, "Increases resource yields further", (p) =>
                {
                    profile.Talents[TalentID.CampingRestoration].Points += 1;
                });

                nodeIndex <<= 1;
                var mysticCampfire = new SkillNode(nodeIndex, "Mystic Campfire", 9, "Unlocks magic campfire spell", (p) =>
                {
                    profile.Talents[TalentID.CampingSpells].Points |= 0x40;
                });

                nodeIndex <<= 1;
                // Modified: now unlocks a defensive spell instead of a passive bonus.
                var vigilantWard = new SkillNode(nodeIndex, "Vigilant Ward", 9, "Unlocks Vigilant Ward spell", (p) =>
                {
                    profile.Talents[TalentID.CampingSpells].Points |= 0x2000;
                });

                nodeIndex <<= 1;
                var travelersInstinct = new SkillNode(nodeIndex, "Traveler's Instinct", 9, "Further enhances travel bonus", (p) =>
                {
                    profile.Talents[TalentID.CampingTravelBonus].Points += 1;
                });

                starlitVigil.AddChild(emberEchoes);
                firelightGlow.AddChild(mysticCampfire);
                swiftAssembly.AddChild(vigilantWard);
                restorativeAura.AddChild(travelersInstinct);

                // Layer 5: Expert-level nodes.
                nodeIndex <<= 1;
                var primevalComfort = new SkillNode(nodeIndex, "Primeval Comfort", 10, "Boosts overall camping bonuses", (p) =>
                {
                    // Passive bonus; implement as needed.
                });

                nodeIndex <<= 1;
                // Modified: now unlocks a spell instead of increasing resource cache.
                var bountifulCache = new SkillNode(nodeIndex, "Bountiful Cache", 10, "Unlocks Bountiful Cache spell", (p) =>
                {
                    profile.Talents[TalentID.CampingSpells].Points |= 0x4000;
                });

                nodeIndex <<= 1;
                var shelterMastery = new SkillNode(nodeIndex, "Shelter Mastery", 10, "Unlocks advanced shelter spells", (p) =>
                {
                    profile.Talents[TalentID.CampingSpells].Points |= 0x100;
                });

                nodeIndex <<= 1;
                var rapidDeployment = new SkillNode(nodeIndex, "Rapid Deployment", 10, "Further reduces setup time", (p) =>
                {
                    profile.Talents[TalentID.CampingTentDuration].Points += 1;
                });

                emberEchoes.AddChild(primevalComfort);
                mysticCampfire.AddChild(bountifulCache);
                vigilantWard.AddChild(shelterMastery);
                travelersInstinct.AddChild(rapidDeployment);

                // Layer 6: Mastery nodes.
                nodeIndex <<= 1;
                var expandedHorizons = new SkillNode(nodeIndex, "Expanded Horizons", 11, "Further enhances travel bonus", (p) =>
                {
                    profile.Talents[TalentID.CampingTravelBonus].Points += 1;
                });

                nodeIndex <<= 1;
                var luminousEmbers = new SkillNode(nodeIndex, "Luminous Embers", 11, "Enhances campfire efficiency", (p) =>
                {
                    profile.Talents[TalentID.CampingCampfireEfficiency].Points += 1;
                });

                nodeIndex <<= 1;
                var ancientOutdoors = new SkillNode(nodeIndex, "Ancient Outdoors", 11, "Unlocks ancient camping spells", (p) =>
                {
                    profile.Talents[TalentID.CampingSpells].Points |= 0x200;
                });

                nodeIndex <<= 1;
                // Modified: now unlocks a spell instead of further improving setup speed.
                var agileAssembly = new SkillNode(nodeIndex, "Agile Assembly", 11, "Unlocks Agile Assembly spell", (p) =>
                {
                    profile.Talents[TalentID.CampingSpells].Points |= 0x8000;
                });

                primevalComfort.AddChild(expandedHorizons);
                bountifulCache.AddChild(luminousEmbers);
                shelterMastery.AddChild(ancientOutdoors);
                rapidDeployment.AddChild(agileAssembly);

                // Layer 7: Final, pinnacle bonuses.
                nodeIndex <<= 1;
                var barrierOfWarmth = new SkillNode(nodeIndex, "Barrier of Warmth", 12, "Provides a protective barrier", (p) =>
                {
                    profile.Talents[TalentID.CampingSpells].Points |= 0x400;
                });

                nodeIndex <<= 1;
                var naturesEndowment = new SkillNode(nodeIndex, "Nature's Endowment", 12, "Further increases restorative power", (p) =>
                {
                    profile.Talents[TalentID.CampingRestoration].Points += 1;
                });

                nodeIndex <<= 1;
                var wildsFury = new SkillNode(nodeIndex, "Wilds Fury", 12, "Boosts travel bonus", (p) =>
                {
                    profile.Talents[TalentID.CampingTravelBonus].Points += 1;
                });

                nodeIndex <<= 1;
                var echoesOfTheWild = new SkillNode(nodeIndex, "Echoes of the Wild", 12, "Further improves campfire efficiency", (p) =>
                {
                    profile.Talents[TalentID.CampingCampfireEfficiency].Points += 1;
                });

                expandedHorizons.AddChild(barrierOfWarmth);
                luminousEmbers.AddChild(naturesEndowment);
                ancientOutdoors.AddChild(wildsFury);
                agileAssembly.AddChild(echoesOfTheWild);

                // Layer 8: Ultimate node.
                nodeIndex <<= 1;
                var ultimateWayfarer = new SkillNode(nodeIndex, "Ultimate Wayfarer", 13, "Ultimate bonus: boosts all camping skills and unlocks two powerful spells", (p) =>
                {
                    profile.Talents[TalentID.CampingSpells].Points |= 0x800 | 0x1000;
                    profile.Talents[TalentID.CampingTentDuration].Points += 1;
                    profile.Talents[TalentID.CampingCampfireEfficiency].Points += 1;
                    profile.Talents[TalentID.CampingRestoration].Points += 1;
                    profile.Talents[TalentID.CampingTravelBonus].Points += 1;
                });

                foreach (var node in new[] { barrierOfWarmth, echoesOfTheWild, wildsFury, naturesEndowment })
                {
                    node.AddChild(ultimateWayfarer);
                }
            }
        }
    }

    // Command to open the Camping Skill Tree.
    public class CampingSkillTreeCommand
    {
        public static void Initialize()
        {
            CommandSystem.Register("CampTree", AccessLevel.Player, cmd => ShowTree(cmd.Mobile));
        }

        private static void ShowTree(Mobile m)
        {
            if (m is PlayerMobile pm)
            {
                pm.SendMessage("Opening Camping Skill Tree...");
                pm.SendGump(new CampingSkillTree(pm));
            }
            else
            {
                m.SendMessage("You must be a player to use this command.");
            }
        }
    }
}
