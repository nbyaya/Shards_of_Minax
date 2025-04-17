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

namespace Server.ACC.CSS.Systems.TacticsMagic
{
    // Revised Tactics Skill Tree Gump using AncientKnowledge (Maxxia Points) as the cost resource.
    public class TacticsSkillTree : SuperGump
    {
        private TacticsTree tree;
        private Dictionary<SkillNode, Point2D> nodePositions;
        private Dictionary<int, SkillNode> buttonNodeMap;
        private Dictionary<SkillNode, int> edgeThickness;
        private const int buttonSize = 15;
        private int ySpacing = 50, xSpacing = 60;
        private int rootX = 300, rootY = 150;
        private SkillNode selectedNode;

        public TacticsSkillTree(PlayerMobile user)
            : base(user, null, 100, 100)
        {
            tree = new TacticsTree(user);
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

            // Center each level's nodes around rootX.
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
            layout.Add("title", () => { AddLabel(100, 20, 1153, "Tactics Skill Tree"); });

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
                        text = $"<BASEFONT COLOR=#FF0000>{selectedNode.Name} Locked! Unlock the prerequisite node first.</BASEFONT>";

                    AddHtml(100, 50, 300, 40, text, false, false);
                }
            });

            // Display the node's description.
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

    // The SkillNode class – same structure as the Lumberjacking example.
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
            if (!profile.Talents.ContainsKey(TalentID.TacticsNodes))
                profile.Talents[TalentID.TacticsNodes] = new Talent(TalentID.TacticsNodes) { Points = 0 };

            return (profile.Talents[TalentID.TacticsNodes].Points & BitFlag) != 0;
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
                player.SendMessage($"{Name} is locked! Unlock the prerequisite node first.");
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
            profile.Talents[TalentID.TacticsNodes].Points |= BitFlag;

            player.SendMessage($"{Name} activated!");
            onActivate?.Invoke(player);
            return true;
        }
    }

    // The full Tactics tree structure.
    public class TacticsTree
    {
        public SkillNode Root { get; }

        public TacticsTree(PlayerMobile player)
        {
            var profile = player.AcquireTalents();
            int nodeIndex = 0x01;

            // Layer 0: Root Node – Unlocks basic tactics spells.
            Root = new SkillNode(nodeIndex, "Commander's Call", 5, "Unlocks basic tactical maneuvers", (p) =>
            {
                // Spell unlock using 0x01.
                profile.Talents[TalentID.TacticsSpells].Points |= 0x01;
            });

            // Layer 1: Basic tactical bonuses.
            nodeIndex <<= 1;
            // Flanking Maneuver now unlocks a spell (0x02)
            var flankingManeuver = new SkillNode(nodeIndex, "Flanking Maneuver", 6, "Unlocks a tactical spell", (p) =>
            {
                profile.Talents[TalentID.TacticsSpells].Points |= 0x02;
            });

            nodeIndex <<= 1;
            var defensiveStance = new SkillNode(nodeIndex, "Defensive Stance", 6, "Boosts defensive capabilities", (p) =>
            {
                // Remains passive.
                profile.Talents[TalentID.TacticsPassive].Points += 1;
            });

            nodeIndex <<= 1;
            // Quick Deployment remains a spell unlock (0x04)
            var quickDeployment = new SkillNode(nodeIndex, "Quick Deployment", 6, "Reduces delay in executing tactics", (p) =>
            {
                profile.Talents[TalentID.TacticsSpells].Points |= 0x04;
            });

            nodeIndex <<= 1;
            var fieldAwareness = new SkillNode(nodeIndex, "Field Awareness", 6, "Increases battlefield perception", (p) =>
            {
                // Remains passive.
                profile.Talents[TalentID.TacticsPassive].Points += 1;
            });

            Root.AddChild(flankingManeuver);
            Root.AddChild(defensiveStance);
            Root.AddChild(quickDeployment);
            Root.AddChild(fieldAwareness);

            // Layer 2: Advanced tactical maneuvers.
            nodeIndex <<= 1;
            // Ambush Tactics remains a spell unlock (0x08)
            var ambushTactics = new SkillNode(nodeIndex, "Ambush Tactics", 7, "Unlocks a surprise attack spell", (p) =>
            {
                profile.Talents[TalentID.TacticsSpells].Points |= 0x08;
            });

            nodeIndex <<= 1;
            // Covering Fire remains a spell unlock (0x10)
            var coveringFire = new SkillNode(nodeIndex, "Covering Fire", 7, "Unlocks a suppressive fire ability", (p) =>
            {
                profile.Talents[TalentID.TacticsSpells].Points |= 0x10;
            });

            nodeIndex <<= 1;
            // Strategic Withdrawal remains a spell unlock (0x20)
            var strategicWithdrawal = new SkillNode(nodeIndex, "Strategic Withdrawal", 7, "Unlocks evasive maneuvers", (p) =>
            {
                profile.Talents[TalentID.TacticsSpells].Points |= 0x20;
            });

            nodeIndex <<= 1;
            var battlefieldControl = new SkillNode(nodeIndex, "Battlefield Control", 7, "Increases control over combat flow", (p) =>
            {
                // Remains passive.
                profile.Talents[TalentID.TacticsPassive].Points += 1;
            });

            flankingManeuver.AddChild(ambushTactics);
            defensiveStance.AddChild(coveringFire);
            quickDeployment.AddChild(strategicWithdrawal);
            fieldAwareness.AddChild(battlefieldControl);

            // Layer 3: Coordination and counter tactics.
            nodeIndex <<= 1;
            // Coordinated Strike now unlocks a spell (0x800)
            var coordinatedStrike = new SkillNode(nodeIndex, "Coordinated Strike", 8, "Unlocks an offensive tactical spell", (p) =>
            {
                profile.Talents[TalentID.TacticsSpells].Points |= 0x800;
            });

            nodeIndex <<= 1;
            // Counter Maneuver now unlocks a spell (0x1000)
            var counterManeuver = new SkillNode(nodeIndex, "Counter Maneuver", 8, "Unlocks a defensive tactical spell", (p) =>
            {
                profile.Talents[TalentID.TacticsSpells].Points |= 0x1000;
            });

            nodeIndex <<= 1;
            var commandPresence = new SkillNode(nodeIndex, "Command Presence", 8, "Boosts leadership and morale", (p) =>
            {
                // Remains passive.
                profile.Talents[TalentID.TacticsPassive].Points += 1;
            });

            nodeIndex <<= 1;
            var rapidReorganization = new SkillNode(nodeIndex, "Rapid Reorganization", 8, "Quickly adapts strategy mid-battle", (p) =>
            {
                // Remains passive.
                profile.Talents[TalentID.TacticsPassive].Points += 1;
            });

            ambushTactics.AddChild(coordinatedStrike);
            coveringFire.AddChild(counterManeuver);
            strategicWithdrawal.AddChild(commandPresence);
            battlefieldControl.AddChild(rapidReorganization);

            // Layer 4: Tactical execution improvements.
            nodeIndex <<= 1;
            var flawlessExecution = new SkillNode(nodeIndex, "Flawless Execution", 9, "Optimizes tactical efficiency", (p) =>
            {
                // Remains passive.
                profile.Talents[TalentID.TacticsPassive].Points += 1;
            });

            nodeIndex <<= 1;
            // Adaptive Tactics now unlocks a spell (0x2000)
            var adaptiveTactics = new SkillNode(nodeIndex, "Adaptive Tactics", 9, "Unlocks a reactive tactical spell", (p) =>
            {
                profile.Talents[TalentID.TacticsSpells].Points |= 0x2000;
            });

            nodeIndex <<= 1;
            var fortifiedPosition = new SkillNode(nodeIndex, "Fortified Position", 9, "Strengthens defensive formations", (p) =>
            {
                // Remains passive.
                profile.Talents[TalentID.TacticsPassive].Points += 1;
            });

            nodeIndex <<= 1;
            // Inspiring Rally remains a spell unlock (0x40)
            var inspiringRally = new SkillNode(nodeIndex, "Inspiring Rally", 9, "Unlocks rallying cries and buffs", (p) =>
            {
                profile.Talents[TalentID.TacticsSpells].Points |= 0x40;
            });

            coordinatedStrike.AddChild(flawlessExecution);
            counterManeuver.AddChild(adaptiveTactics);
            commandPresence.AddChild(fortifiedPosition);
            rapidReorganization.AddChild(inspiringRally);

            // Layer 5: Expert tactical adjustments.
            nodeIndex <<= 1;
            var precisionTargeting = new SkillNode(nodeIndex, "Precision Targeting", 10, "Boosts accuracy in combat", (p) =>
            {
                // Remains passive.
                profile.Talents[TalentID.TacticsPassive].Points += 1;
            });

            nodeIndex <<= 1;
            // Battle Rhythm remains a spell unlock (0x80)
            var battleRhythm = new SkillNode(nodeIndex, "Battle Rhythm", 10, "Improves timing and execution", (p) =>
            {
                profile.Talents[TalentID.TacticsSpells].Points |= 0x80;
            });

            nodeIndex <<= 1;
            // Tactical Mastery remains a spell unlock (0x100)
            var tacticalMastery = new SkillNode(nodeIndex, "Tactical Mastery", 10, "Unlocks advanced tactical spells", (p) =>
            {
                profile.Talents[TalentID.TacticsSpells].Points |= 0x100;
            });

            nodeIndex <<= 1;
            // Decisive Strike now unlocks a spell (0x4000)
            var decisiveStrike = new SkillNode(nodeIndex, "Decisive Strike", 10, "Unlocks an offensive burst spell", (p) =>
            {
                profile.Talents[TalentID.TacticsSpells].Points |= 0x4000;
            });

            flawlessExecution.AddChild(precisionTargeting);
            adaptiveTactics.AddChild(battleRhythm);
            fortifiedPosition.AddChild(tacticalMastery);
            inspiringRally.AddChild(decisiveStrike);

            // Layer 6: Strategic insight.
            nodeIndex <<= 1;
            // Insightful Analysis now unlocks a spell (0x8000)
            var insightfulAnalysis = new SkillNode(nodeIndex, "Insightful Analysis", 11, "Unlocks a strategic insight spell", (p) =>
            {
                profile.Talents[TalentID.TacticsSpells].Points |= 0x8000;
            });

            nodeIndex <<= 1;
            var rapidAdjustment = new SkillNode(nodeIndex, "Rapid Adjustment", 11, "Quickly shifts tactics", (p) =>
            {
                // Remains passive.
                profile.Talents[TalentID.TacticsPassive].Points += 1;
            });

            nodeIndex <<= 1;
            var unyieldingResolve = new SkillNode(nodeIndex, "Unyielding Resolve", 11, "Boosts defense in adversity", (p) =>
            {
                // Remains passive.
                profile.Talents[TalentID.TacticsPassive].Points += 1;
            });

            nodeIndex <<= 1;
            var dynamicFormation = new SkillNode(nodeIndex, "Dynamic Formation", 11, "Enhances group synergy", (p) =>
            {
                // Remains passive.
                profile.Talents[TalentID.TacticsPassive].Points += 1;
            });

            precisionTargeting.AddChild(insightfulAnalysis);
            battleRhythm.AddChild(rapidAdjustment);
            tacticalMastery.AddChild(unyieldingResolve);
            decisiveStrike.AddChild(dynamicFormation);

            // Layer 7: Pinnacle tactical bonuses.
            nodeIndex <<= 1;
            var steadfastCommand = new SkillNode(nodeIndex, "Steadfast Command", 12, "Greatly improves leadership", (p) =>
            {
                // Remains passive.
                profile.Talents[TalentID.TacticsPassive].Points += 1;
            });

            nodeIndex <<= 1;
            var tacticalSupremacy = new SkillNode(nodeIndex, "Tactical Supremacy", 12, "Dominates the battlefield with strategy", (p) =>
            {
                // Remains passive.
                profile.Talents[TalentID.TacticsPassive].Points += 1;
            });

            nodeIndex <<= 1;
            // Master Strategist remains a spell unlock (0x200)
            var masterStrategist = new SkillNode(nodeIndex, "Master Strategist", 12, "Unlocks unique strategic spells", (p) =>
            {
                profile.Talents[TalentID.TacticsSpells].Points |= 0x200;
            });

            nodeIndex <<= 1;
            var lastStand = new SkillNode(nodeIndex, "Last Stand", 12, "Boosts survival in dire moments", (p) =>
            {
                // Remains passive.
                profile.Talents[TalentID.TacticsPassive].Points += 1;
            });

            insightfulAnalysis.AddChild(steadfastCommand);
            rapidAdjustment.AddChild(tacticalSupremacy);
            unyieldingResolve.AddChild(masterStrategist);
            dynamicFormation.AddChild(lastStand);

            // Layer 8: Ultimate node.
            nodeIndex <<= 1;
            // Ultimate Strategist now unlocks a spell (0x400)
            var ultimateStrategist = new SkillNode(nodeIndex, "Ultimate Strategist", 13, "Ultimate bonus: enhances all tactical abilities", (p) =>
            {
                profile.Talents[TalentID.TacticsSpells].Points |= 0x400;
            });

            steadfastCommand.AddChild(ultimateStrategist);
            tacticalSupremacy.AddChild(ultimateStrategist);
            masterStrategist.AddChild(ultimateStrategist);
            lastStand.AddChild(ultimateStrategist);
        }
    }

    // Command to open the Tactics Skill Tree.
    public class TacticsSkillTreeCommand
    {
        public static void Initialize()
        {
            CommandSystem.Register("TacticsTree", AccessLevel.Player, cmd => ShowTree(cmd.Mobile));
        }

        private static void ShowTree(Mobile m)
        {
            if (m is PlayerMobile pm)
            {
                pm.SendMessage("Opening Tactics Skill Tree...");
                pm.SendGump(new TacticsSkillTree(pm));
            }
            else
            {
                m.SendMessage("You must be a player to use this command.");
            }
        }
    }
}
