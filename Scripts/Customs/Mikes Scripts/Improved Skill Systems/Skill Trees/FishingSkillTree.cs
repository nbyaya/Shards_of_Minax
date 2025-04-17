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

namespace Server.ACC.CSS.Systems.FishingMagic
{
    public class FishingSkillTree : SuperGump
    {
        private FishingTree tree;
        private Dictionary<SkillNode, Point2D> nodePositions;
        private Dictionary<int, SkillNode> buttonNodeMap;
        private Dictionary<SkillNode, int> edgeThickness;
        private const int buttonSize = 15;
        private int ySpacing = 50, xSpacing = 60;
        private int rootX = 300, rootY = 150;
        private SkillNode selectedNode;

        public FishingSkillTree(PlayerMobile user)
            : base(user, null, 100, 100)
        {
            tree = new FishingTree(user);
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
                    queue.Enqueue((child, level + 1));
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
            layout.Add("title", () => { AddLabel(100, 20, 1153, "Fishing Skill Tree"); });

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

            // New element to display the node description just below the node name.
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

    public class FishingTree
    {
        public SkillNode Root { get; }

        public FishingTree(PlayerMobile player)
        {
            var profile = player.AcquireTalents();
            int nodeIndex = 0x01;

            // Layer 0: Root Node – Unlocks basic fishing spells.
            Root = new SkillNode(nodeIndex, "Call of the Tides", 5, "Unlocks basic fishing spells", (p) =>
            {
                profile.Talents[TalentID.FishingSpells].Points |= 0x01;
            });

            // Layer 1: Basic bonuses and spell unlocks.
            nodeIndex <<= 1;
            var waterWhisper = new SkillNode(nodeIndex, "Water's Whisper", 6, "Unlocks a fluid fishing spell", (p) =>
            {
                profile.Talents[TalentID.FishingSpells].Points |= 0x02;
            });

            nodeIndex <<= 1;
            var steadyHand = new SkillNode(nodeIndex, "Steady Hand", 6, "Improves fishing efficiency", (p) =>
            {
                profile.Talents[TalentID.FishingEfficiency].Points += 1;
            });

            nodeIndex <<= 1;
            var tidalInsight = new SkillNode(nodeIndex, "Tidal Insight", 6, "Unlocks bonus fishing spells", (p) =>
            {
                profile.Talents[TalentID.FishingSpells].Points |= 0x04;
            });

            nodeIndex <<= 1;
            var deepFocus = new SkillNode(nodeIndex, "Deep Focus", 6, "Extends fishing range", (p) =>
            {
                profile.Talents[TalentID.FishingRange].Points += 1;
            });

            Root.AddChild(waterWhisper);
            Root.AddChild(steadyHand);
            Root.AddChild(tidalInsight);
            Root.AddChild(deepFocus);

            // Layer 2: Advanced bonuses and spells.
            nodeIndex <<= 1;
            var currentsMurmur = new SkillNode(nodeIndex, "Currents' Murmur", 7, "Unlocks additional fishing spells", (p) =>
            {
                profile.Talents[TalentID.FishingSpells].Points |= 0x08;
            });

            nodeIndex <<= 1;
            var swiftReeling = new SkillNode(nodeIndex, "Swift Reeling", 7, "Boosts fishing efficiency further", (p) =>
            {
                profile.Talents[TalentID.FishingEfficiency].Points += 1;
            });

            nodeIndex <<= 1;
            var mysticLure = new SkillNode(nodeIndex, "Mystic Lure", 7, "Unlocks magical fishing abilities", (p) =>
            {
                profile.Talents[TalentID.FishingSpells].Points |= 0x10;
            });

            nodeIndex <<= 1;
            var expansiveWaters = new SkillNode(nodeIndex, "Expansive Waters", 7, "Extends your fishing range", (p) =>
            {
                profile.Talents[TalentID.FishingRange].Points += 1;
            });

            waterWhisper.AddChild(currentsMurmur);
            steadyHand.AddChild(swiftReeling);
            tidalInsight.AddChild(mysticLure);
            deepFocus.AddChild(expansiveWaters);

            // Layer 3: Further yield and efficiency improvements.
            nodeIndex <<= 1;
            var bountifulCatch = new SkillNode(nodeIndex, "Bountiful Catch", 8, "Boosts overall fish yield", (p) =>
            {
                profile.Talents[TalentID.FishingYield].Points += 1;
            });

            nodeIndex <<= 1;
            var steadyCast = new SkillNode(nodeIndex, "Steady Cast", 8, "Improves casting efficiency", (p) =>
            {
                profile.Talents[TalentID.FishingEfficiency].Points += 1;
            });

            nodeIndex <<= 1;
            var anglersFortitude = new SkillNode(nodeIndex, "Angler's Fortitude", 8, "Unlocks a fortitude bonus spell", (p) =>
            {
                profile.Talents[TalentID.FishingSpells].Points |= 0x20;
            });

            nodeIndex <<= 1;
            var luckyStrike = new SkillNode(nodeIndex, "Lucky Strike", 8, "Unlocks a luck-enhancing fishing spell", (p) =>
            {
                profile.Talents[TalentID.FishingSpells].Points |= 0x2000;
            });

            currentsMurmur.AddChild(bountifulCatch);
            swiftReeling.AddChild(steadyCast);
            mysticLure.AddChild(anglersFortitude);
            expansiveWaters.AddChild(luckyStrike);

            // Layer 4: More advanced magical enhancements.
            nodeIndex <<= 1;
            var tidalBlessing = new SkillNode(nodeIndex, "Tidal Blessing", 9, "Further increases fish yield", (p) =>
            {
                profile.Talents[TalentID.FishingYield].Points += 1;
            });

            nodeIndex <<= 1;
            var enchantedBait = new SkillNode(nodeIndex, "Enchanted Bait", 9, "Unlocks advanced fishing spells", (p) =>
            {
                profile.Talents[TalentID.FishingSpells].Points |= 0x40;
            });

            nodeIndex <<= 1;
            var oceansMight = new SkillNode(nodeIndex, "Ocean's Might", 9, "Empowers your fishing spells", (p) =>
            {
                profile.Talents[TalentID.FishingSpells].Points |= 0x80;
            });

            nodeIndex <<= 1;
            var farReach = new SkillNode(nodeIndex, "Far Reach", 9, "Further extends your fishing range", (p) =>
            {
                profile.Talents[TalentID.FishingRange].Points += 1;
            });

            bountifulCatch.AddChild(tidalBlessing);
            steadyCast.AddChild(enchantedBait);
            anglersFortitude.AddChild(oceansMight);
            expansiveWaters.AddChild(farReach);

            // Layer 5: Expert-level nodes.
            nodeIndex <<= 1;
            var primeCatch = new SkillNode(nodeIndex, "Prime Catch", 10, "Unlocks a prime catch fishing spell", (p) =>
            {
                profile.Talents[TalentID.FishingSpells].Points |= 0x4000;
            });

            nodeIndex <<= 1;
            var bumperHarvest = new SkillNode(nodeIndex, "Bumper Harvest", 10, "Significantly boosts fish yield", (p) =>
            {
                profile.Talents[TalentID.FishingYield].Points += 1;
            });

            nodeIndex <<= 1;
            var masterAngler = new SkillNode(nodeIndex, "Master Angler", 10, "Unlocks mastery level fishing spells", (p) =>
            {
                profile.Talents[TalentID.FishingSpells].Points |= 0x100;
            });

            nodeIndex <<= 1;
            var rapidReel = new SkillNode(nodeIndex, "Rapid Reel", 10, "Improves reeling speed", (p) =>
            {
                profile.Talents[TalentID.FishingEfficiency].Points += 1;
            });

            tidalBlessing.AddChild(primeCatch);
            enchantedBait.AddChild(bumperHarvest);
            oceansMight.AddChild(masterAngler);
            farReach.AddChild(rapidReel);

            // Layer 6: Mastery nodes.
            nodeIndex <<= 1;
            var extendedHorizon = new SkillNode(nodeIndex, "Extended Horizon", 11, "Broadens your fishing range", (p) =>
            {
                profile.Talents[TalentID.FishingRange].Points += 1;
            });

            nodeIndex <<= 1;
            var mysticBounty = new SkillNode(nodeIndex, "Mystic Bounty", 11, "Unlocks a mystical bounty fishing spell", (p) =>
            {
                profile.Talents[TalentID.FishingSpells].Points |= 0x1000;
            });

            nodeIndex <<= 1;
            var ancientAngler = new SkillNode(nodeIndex, "Ancient Angler", 11, "Unlocks ancient fishing spells", (p) =>
            {
                profile.Talents[TalentID.FishingSpells].Points |= 0x200;
            });

            nodeIndex <<= 1;
            var precisionCasting = new SkillNode(nodeIndex, "Precision Casting", 11, "Increases casting precision", (p) =>
            {
                profile.Talents[TalentID.FishingEfficiency].Points += 1;
            });

            primeCatch.AddChild(extendedHorizon);
            bumperHarvest.AddChild(mysticBounty);
            masterAngler.AddChild(ancientAngler);
            rapidReel.AddChild(precisionCasting);

            // Layer 7: Final, pinnacle bonuses.
            nodeIndex <<= 1;
            var protectiveNet = new SkillNode(nodeIndex, "Protective Net", 12, "Grants a magical protective net", (p) =>
            {
                profile.Talents[TalentID.FishingSpells].Points |= 0x400;
            });

            nodeIndex <<= 1;
            var overflowingWaters = new SkillNode(nodeIndex, "Overflowing Waters", 12, "Greatly increases fish yield", (p) =>
            {
                profile.Talents[TalentID.FishingYield].Points += 1;
            });

            nodeIndex <<= 1;
            var luckyAngler = new SkillNode(nodeIndex, "Lucky Angler", 12, "Unlocks a lucky angler fishing spell", (p) =>
            {
                profile.Talents[TalentID.FishingSpells].Points |= 0x8000;
            });

            nodeIndex <<= 1;
            var vastExpanse = new SkillNode(nodeIndex, "Vast Expanse", 12, "Maximizes your fishing range", (p) =>
            {
                profile.Talents[TalentID.FishingRange].Points += 1;
            });

            extendedHorizon.AddChild(protectiveNet);
            mysticBounty.AddChild(overflowingWaters);
            ancientAngler.AddChild(luckyAngler);
            precisionCasting.AddChild(vastExpanse);

            // Layer 8: Ultimate node.
            nodeIndex <<= 1;
            var ultimateFisher = new SkillNode(nodeIndex, "Ultimate Fisher", 13, "Ultimate bonus: enhances all fishing skills and unlocks an ultimate fishing spell", (p) =>
            {
                profile.Talents[TalentID.FishingSpells].Points |= 0x800;
                profile.Talents[TalentID.FishingRange].Points += 1;
                profile.Talents[TalentID.FishingEfficiency].Points += 1;
                profile.Talents[TalentID.FishingYield].Points += 1;
                profile.Talents[TalentID.FishingLuck].Points += 1;
            });

            protectiveNet.AddChild(ultimateFisher);
            overflowingWaters.AddChild(ultimateFisher);
            luckyAngler.AddChild(ultimateFisher);
            vastExpanse.AddChild(ultimateFisher);
        }
    }

    public class SkillNode
    {
        public int BitFlag { get; }
        public string Name { get; }
        public int Cost { get; }
        public string Description { get; } // New property for node description.
        public List<SkillNode> Children { get; }
        public SkillNode Parent { get; private set; }
        private readonly Action<PlayerMobile> onActivate;

        // Modified constructor accepts a description parameter.
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
            if (!profile.Talents.ContainsKey(TalentID.FishingNodes))
                profile.Talents[TalentID.FishingNodes] = new Talent(TalentID.FishingNodes) { Points = 0 };

            return (profile.Talents[TalentID.FishingNodes].Points & BitFlag) != 0;
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
            profile.Talents[TalentID.FishingNodes].Points |= BitFlag;

            player.SendMessage($"{Name} activated!");
            onActivate?.Invoke(player);
            return true;
        }
    }
}
