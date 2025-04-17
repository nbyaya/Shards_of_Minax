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

namespace Server.ACC.CSS.Systems.RemoveTrapMagic
{
    // Revised Remove Trap Skill Tree Gump using AncientKnowledge (Maxxia Points) as the cost resource.
    public class RemoveTrapSkillTree : SuperGump
    {
        private RemoveTrapTree tree;
        private Dictionary<SkillNode, Point2D> nodePositions;
        private Dictionary<int, SkillNode> buttonNodeMap;
        private Dictionary<SkillNode, int> edgeThickness;
        private const int buttonSize = 15;
        private int ySpacing = 50, xSpacing = 60;
        private int rootX = 300, rootY = 150;
        private SkillNode selectedNode;

        public RemoveTrapSkillTree(PlayerMobile user)
            : base(user, null, 100, 100)
        {
            tree = new RemoveTrapTree(user);
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

            // Ensure each node is placed only once.
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

            // Position nodes of each level centered around rootX.
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
            layout.Add("title", () => { AddLabel(100, 20, 1153, "Remove Trap Skill Tree"); });

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
    }

    // Revised SkillNode that uses AncientKnowledge (Maxxia Points) for cost.
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
            if (!profile.Talents.ContainsKey(TalentID.RemoveTrapNodes))
                profile.Talents[TalentID.RemoveTrapNodes] = new Talent(TalentID.RemoveTrapNodes) { Points = 0 };

            return (profile.Talents[TalentID.RemoveTrapNodes].Points & BitFlag) != 0;
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
            // Mark this node as activated.
            profile.Talents[TalentID.RemoveTrapNodes].Points |= BitFlag;

            player.SendMessage($"{Name} activated!");
            onActivate?.Invoke(player);
            return true;
        }
    }

    // Full Remove Trap tree structure with 30 nodes (layers 0 to 8).
    public class RemoveTrapTree
    {
        public SkillNode Root { get; }

        public RemoveTrapTree(PlayerMobile player)
        {
            var profile = player.AcquireTalents();
            int nodeIndex = 0x01;

            // Layer 0: Root Node – Unlocks basic trap removal spells.
            Root = new SkillNode(nodeIndex, "Eye of the Sentinel", 5, "Unlocks basic trap removal spells", (p) =>
            {
                // Set bit 0x01 in RemoveTrapSpells to unlock basic spells.
                profile.Talents[TalentID.RemoveTrapSpells].Points |= 0x01;
            });

            // Layer 1: Basic nodes.
            nodeIndex <<= 1;
            var trapAwareness = new SkillNode(nodeIndex, "Trap Awareness", 6, "Increases trap detection chance", (p) =>
            {
                profile.Talents[TalentID.RemoveTrapDetection].Points += 1;
            });

            nodeIndex <<= 1;
            // Changed to spell unlock (0x1000)
            var tinkersInsight = new SkillNode(nodeIndex, "Tinker's Insight", 6, "Unlocks additional trap removal spell", (p) =>
            {
                profile.Talents[TalentID.RemoveTrapSpells].Points |= 0x1000;
            });

            nodeIndex <<= 1;
            var steadyHands = new SkillNode(nodeIndex, "Steady Hands", 6, "Improves disarming speed", (p) =>
            {
                profile.Talents[TalentID.RemoveTrapSpeed].Points += 1;
            });

            nodeIndex <<= 1;
            var trapproof = new SkillNode(nodeIndex, "Trapproof", 6, "Passively reduces chance of trap triggering", (p) =>
            {
                // This bonus might later reduce negative effects.
                profile.Talents[TalentID.RemoveTrapKitEfficiency].Points += 1;
            });

            Root.AddChild(trapAwareness);
            Root.AddChild(tinkersInsight);
            Root.AddChild(steadyHands);
            Root.AddChild(trapproof);

            // Layer 2: Unlock additional trap spells and bonuses.
            nodeIndex <<= 1;
            var mechanismMastery = new SkillNode(nodeIndex, "Mechanism Mastery", 7, "Unlocks additional trap removal spells", (p) =>
            {
                profile.Talents[TalentID.RemoveTrapSpells].Points |= 0x02;
            });

            nodeIndex <<= 1;
            var quickReflexes = new SkillNode(nodeIndex, "Quick Reflexes", 7, "Reduces disarm delay", (p) =>
            {
                profile.Talents[TalentID.RemoveTrapSpeed].Points += 1;
            });

            nodeIndex <<= 1;
            // Changed to spell unlock (0x2000)
            var silentApproach = new SkillNode(nodeIndex, "Silent Approach", 7, "Unlocks additional trap removal spell", (p) =>
            {
                profile.Talents[TalentID.RemoveTrapSpells].Points |= 0x2000;
            });

            nodeIndex <<= 1;
            var carefulHandling = new SkillNode(nodeIndex, "Careful Handling", 7, "Passively reduces chance to trigger traps", (p) =>
            {
                profile.Talents[TalentID.RemoveTrapDetection].Points += 1;
            });

            trapAwareness.AddChild(mechanismMastery);
            steadyHands.AddChild(quickReflexes);
            tinkersInsight.AddChild(silentApproach);
            trapproof.AddChild(carefulHandling);

            // Layer 3: More advanced skills.
            nodeIndex <<= 1;
            // Changed to spell unlock (0x4000)
            var preciseManipulation = new SkillNode(nodeIndex, "Precise Manipulation", 8, "Unlocks additional trap removal spell", (p) =>
            {
                profile.Talents[TalentID.RemoveTrapSpells].Points |= 0x4000;
            });

            nodeIndex <<= 1;
            var eagleVision = new SkillNode(nodeIndex, "Eagle Vision", 8, "Greatly enhances trap detection range", (p) =>
            {
                profile.Talents[TalentID.RemoveTrapDetection].Points += 1;
            });

            nodeIndex <<= 1;
            var calmUnderPressure = new SkillNode(nodeIndex, "Calm Under Pressure", 8, "Passively improves overall trap removal", (p) =>
            {
                profile.Talents[TalentID.RemoveTrapSpells].Points |= 0x04;
            });

            nodeIndex <<= 1;
            var trapAnalysis = new SkillNode(nodeIndex, "Trap Analysis", 8, "Reveals detailed trap information", (p) =>
            {
                // This could later be used to show extra info in the UI.
                profile.Talents[TalentID.RemoveTrapSpells].Points |= 0x08;
            });

            mechanismMastery.AddChild(preciseManipulation);
            quickReflexes.AddChild(eagleVision);
            silentApproach.AddChild(calmUnderPressure);
            carefulHandling.AddChild(trapAnalysis);

            // Layer 4: Intermediate advanced skills.
            nodeIndex <<= 1;
            var explosiveCounter = new SkillNode(nodeIndex, "Explosive Counter", 9, "Unlocks counter trap removal spells", (p) =>
            {
                profile.Talents[TalentID.RemoveTrapSpells].Points |= 0x10;
            });

            nodeIndex <<= 1;
            var rapidDisarm = new SkillNode(nodeIndex, "Rapid Disarm", 9, "Further reduces disarm delay", (p) =>
            {
                profile.Talents[TalentID.RemoveTrapSpeed].Points += 1;
            });

            nodeIndex <<= 1;
            var guardianBypass = new SkillNode(nodeIndex, "Guardian Bypass", 9, "Reduces chance of trap guardian spawn", (p) =>
            {
                profile.Talents[TalentID.RemoveTrapKitEfficiency].Points += 1;
            });

            nodeIndex <<= 1;
            // Changed to spell unlock (0x8000)
            var mechanicsLuck = new SkillNode(nodeIndex, "Mechanic's Luck", 9, "Unlocks additional trap removal spell", (p) =>
            {
                profile.Talents[TalentID.RemoveTrapSpells].Points |= 0x8000;
            });

            preciseManipulation.AddChild(explosiveCounter);
            eagleVision.AddChild(rapidDisarm);
            calmUnderPressure.AddChild(guardianBypass);
            trapAnalysis.AddChild(mechanicsLuck);

            // Layer 5: Expert-level nodes.
            nodeIndex <<= 1;
            var masterDisarmer = new SkillNode(nodeIndex, "Master Disarmer", 10, "Greatly enhances trap removal spells", (p) =>
            {
                profile.Talents[TalentID.RemoveTrapSpells].Points |= 0x20;
            });

            nodeIndex <<= 1;
            var trapNullification = new SkillNode(nodeIndex, "Trap Nullification", 10, "Chance to permanently disable traps", (p) =>
            {
                profile.Talents[TalentID.RemoveTrapSuccessChance].Points += 1;
            });

            nodeIndex <<= 1;
            var swiftRecovery = new SkillNode(nodeIndex, "Swift Recovery", 10, "Reduces cooldown after disarming", (p) =>
            {
                // Could be used in cooldown calculations.
                profile.Talents[TalentID.RemoveTrapSpells].Points |= 0x40;
            });

            nodeIndex <<= 1;
            var efficientTinkering = new SkillNode(nodeIndex, "Efficient Tinkering", 10, "Improves trap removal kit efficiency", (p) =>
            {
                profile.Talents[TalentID.RemoveTrapKitEfficiency].Points += 1;
            });

            explosiveCounter.AddChild(masterDisarmer);
            rapidDisarm.AddChild(trapNullification);
            guardianBypass.AddChild(swiftRecovery);
            mechanicsLuck.AddChild(efficientTinkering);

            // Layer 6: Further mastery nodes.
            nodeIndex <<= 1;
            var advancedTrapLore = new SkillNode(nodeIndex, "Advanced Trap Lore", 11, "Unlocks further trap removal spells", (p) =>
            {
                profile.Talents[TalentID.RemoveTrapSpells].Points |= 0x80;
            });

            nodeIndex <<= 1;
            var enhancedKitUsage = new SkillNode(nodeIndex, "Enhanced Kit Usage", 11, "Boosts trap removal kit performance", (p) =>
            {
                profile.Talents[TalentID.RemoveTrapKitEfficiency].Points += 1;
            });

            nodeIndex <<= 1;
            var trapSense = new SkillNode(nodeIndex, "Trap Sense", 11, "Heightens the detection of traps", (p) =>
            {
                profile.Talents[TalentID.RemoveTrapDetection].Points += 1;
            });

            nodeIndex <<= 1;
            var mechanicalPrecision = new SkillNode(nodeIndex, "Mechanical Precision", 11, "Improves the precision of your actions", (p) =>
            {
                profile.Talents[TalentID.RemoveTrapSuccessChance].Points += 1;
            });

            masterDisarmer.AddChild(advancedTrapLore);
            trapNullification.AddChild(enhancedKitUsage);
            swiftRecovery.AddChild(trapSense);
            efficientTinkering.AddChild(mechanicalPrecision);

            // Layer 7: Pinnacle skills.
            nodeIndex <<= 1;
            var trapMastery = new SkillNode(nodeIndex, "Trap Mastery", 12, "Significantly boosts trap removal spells", (p) =>
            {
                profile.Talents[TalentID.RemoveTrapSpells].Points |= 0x100;
            });

            nodeIndex <<= 1;
            var disarmingSurge = new SkillNode(nodeIndex, "Disarming Surge", 12, "Greatly increases disarm success chance", (p) =>
            {
                profile.Talents[TalentID.RemoveTrapSuccessChance].Points += 1;
            });

            nodeIndex <<= 1;
            var systemOverride = new SkillNode(nodeIndex, "System Override", 12, "Unlocks ultimate trap removal spells", (p) =>
            {
                profile.Talents[TalentID.RemoveTrapSpells].Points |= 0x200;
            });

            nodeIndex <<= 1;
            var nimbleFingers = new SkillNode(nodeIndex, "Nimble Fingers", 12, "Enhances overall speed and efficiency", (p) =>
            {
                profile.Talents[TalentID.RemoveTrapSpeed].Points += 1;
            });

            advancedTrapLore.AddChild(trapMastery);
            enhancedKitUsage.AddChild(disarmingSurge);
            trapSense.AddChild(systemOverride);
            mechanicalPrecision.AddChild(nimbleFingers);

            // Layer 8: Ultimate node.
            nodeIndex <<= 1;
            var ultimateDemolition = new SkillNode(nodeIndex, "Ultimate Demolition", 13, "Ultimate bonus: drastically enhances all trap removal abilities", (p) =>
            {
                profile.Talents[TalentID.RemoveTrapSpells].Points |= 0x400 | 0x800;
                profile.Talents[TalentID.RemoveTrapDetection].Points += 1;
                profile.Talents[TalentID.RemoveTrapSpeed].Points += 1;
                profile.Talents[TalentID.RemoveTrapSuccessChance].Points += 1;
                profile.Talents[TalentID.RemoveTrapKitEfficiency].Points += 1;
            });

            // Attach ultimate node to all Layer 7 nodes.
            foreach (var node in new[] { trapMastery, disarmingSurge, systemOverride, nimbleFingers })
            {
                node.AddChild(ultimateDemolition);
            }
        }
    }

    // Command to open the Remove Trap Skill Tree.
    public class RemoveTrapSkillTreeCommand
    {
        public static void Initialize()
        {
            CommandSystem.Register("TrapTree", AccessLevel.Player, cmd => ShowTree(cmd.Mobile));
        }

        private static void ShowTree(Mobile m)
        {
            if (m is PlayerMobile pm)
            {
                pm.SendMessage("Opening Remove Trap Skill Tree...");
                pm.SendGump(new RemoveTrapSkillTree(pm));
            }
            else
            {
                m.SendMessage("You must be a player to use this command.");
            }
        }
    }
}
