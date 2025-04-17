// File: Server/ACC/CSS/Systems/FencingMagic/FencingSkillTree.cs
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

namespace Server.ACC.CSS.Systems.FencingMagic
{
    public class FencingSkillTree : SuperGump
    {
        private FencingTree tree;
        private Dictionary<SkillNode, Point2D> nodePositions;
        private Dictionary<int, SkillNode> buttonNodeMap;
        private Dictionary<SkillNode, int> edgeThickness;
        private const int buttonSize = 15;
        private int ySpacing = 50, xSpacing = 60;
        private int rootX = 300, rootY = 150;
        private SkillNode selectedNode;

        public FencingSkillTree(PlayerMobile user)
            : base(user, null, 100, 100)
        {
            tree = new FencingTree(user);
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
                    queue.Enqueue((child, level + 1));
            }

            // Position nodes in each level centered on rootX.
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
            layout.Add("title", () => { AddLabel(100, 20, 1153, "Fencing Skill Tree"); });

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

    // Basic SkillNode class used by both trees.
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
            if (!profile.Talents.ContainsKey(TalentID.FencingNodes))
                profile.Talents[TalentID.FencingNodes] = new Talent(TalentID.FencingNodes) { Points = 0 };

            return (profile.Talents[TalentID.FencingNodes].Points & BitFlag) != 0;
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
            profile.Talents[TalentID.FencingNodes].Points |= BitFlag;

            player.SendMessage($"{Name} activated!");
            onActivate?.Invoke(player);
            return true;
        }
    }

    // FencingTree structure with 30 nodes over 9 layers.
    public class FencingTree
    {
        public SkillNode Root { get; }

        public FencingTree(PlayerMobile player)
        {
            var profile = player.AcquireTalents();
            int nodeIndex = 0x01;

            // Layer 0: Root Node – Unlocks basic fencing spells.
            Root = new SkillNode(nodeIndex, "Call of the Blade", 5, "Unlocks basic fencing spells", (p) =>
            {
                profile.Talents[TalentID.FencingSpells].Points |= 0x01;
            });

            // Layer 1: Convert basic bonuses into spell unlocks.
            nodeIndex <<= 1; // now 0x02
            var quickParry = new SkillNode(nodeIndex, "Quick Parry", 6, "Unlocks spell (0x02)", (p) =>
            {
                profile.Talents[TalentID.FencingSpells].Points |= 0x02;
            });

            nodeIndex <<= 1; // now 0x04
            var preciseStrike = new SkillNode(nodeIndex, "Precise Strike", 6, "Unlocks spell (0x04)", (p) =>
            {
                profile.Talents[TalentID.FencingSpells].Points |= 0x04;
            });

            nodeIndex <<= 1; // now 0x08
            var riposteMastery = new SkillNode(nodeIndex, "Riposte Mastery", 6, "Unlocks spell (0x08)", (p) =>
            {
                profile.Talents[TalentID.FencingSpells].Points |= 0x08;
            });

            nodeIndex <<= 1; // now 0x10
            var stanceAwareness = new SkillNode(nodeIndex, "Stance Awareness", 6, "Unlocks spell (0x10)", (p) =>
            {
                profile.Talents[TalentID.FencingSpells].Points |= 0x10;
            });

            Root.AddChild(quickParry);
            Root.AddChild(preciseStrike);
            Root.AddChild(riposteMastery);
            Root.AddChild(stanceAwareness);

            // Layer 2: Advanced magical and practical bonuses converted.
            nodeIndex <<= 1; // now 0x20
            var bladeDance = new SkillNode(nodeIndex, "Blade Dance", 7, "Unlocks spell (0x20)", (p) =>
            {
                profile.Talents[TalentID.FencingSpells].Points |= 0x20;
            });

            nodeIndex <<= 1; // now 0x40
            var shadowStep = new SkillNode(nodeIndex, "Shadow Step", 7, "Unlocks spell (0x40)", (p) =>
            {
                profile.Talents[TalentID.FencingSpells].Points |= 0x40;
            });

            nodeIndex <<= 1; // now 0x80
            var lethalEdge = new SkillNode(nodeIndex, "Lethal Edge", 7, "Unlocks spell (0x80)", (p) =>
            {
                profile.Talents[TalentID.FencingSpells].Points |= 0x80;
            });

            nodeIndex <<= 1; // now 0x100
            var steadyGuard = new SkillNode(nodeIndex, "Steady Guard", 7, "Unlocks spell (0x100)", (p) =>
            {
                profile.Talents[TalentID.FencingSpells].Points |= 0x100;
            });

            quickParry.AddChild(bladeDance);
            preciseStrike.AddChild(shadowStep);
            riposteMastery.AddChild(lethalEdge);
            stanceAwareness.AddChild(steadyGuard);

            // Layer 3: Further bonuses converted.
            nodeIndex <<= 1; // now 0x200
            var flurryOfBlows = new SkillNode(nodeIndex, "Flurry of Blows", 8, "Unlocks spell (0x200)", (p) =>
            {
                profile.Talents[TalentID.FencingSpells].Points |= 0x200;
            });

            nodeIndex <<= 1; // now 0x400
            var precisionThrust = new SkillNode(nodeIndex, "Precision Thrust", 8, "Unlocks spell (0x400)", (p) =>
            {
                profile.Talents[TalentID.FencingSpells].Points |= 0x400;
            });

            nodeIndex <<= 1; // now 0x800
            var guardingPosture = new SkillNode(nodeIndex, "Guarding Posture", 8, "Unlocks spell (0x800)", (p) =>
            {
                profile.Talents[TalentID.FencingSpells].Points |= 0x800;
            });

            nodeIndex <<= 1; // now 0x1000
            var agileFootwork = new SkillNode(nodeIndex, "Agile Footwork", 8, "Unlocks spell (0x1000)", (p) =>
            {
                profile.Talents[TalentID.FencingSpells].Points |= 0x1000;
            });

            bladeDance.AddChild(flurryOfBlows);
            shadowStep.AddChild(precisionThrust);
            lethalEdge.AddChild(guardingPosture);
            steadyGuard.AddChild(agileFootwork);

            // Layer 4: More advanced magical enhancements.
            // Only three of these four become spell unlocks.
            nodeIndex <<= 1; // now 0x2000
            var bladesGrace = new SkillNode(nodeIndex, "Blade's Grace", 9, "Unlocks spell (0x2000)", (p) =>
            {
                profile.Talents[TalentID.FencingSpells].Points |= 0x2000;
            });

            nodeIndex <<= 1; // now 0x4000
            var counterAttack = new SkillNode(nodeIndex, "Counter Attack", 9, "Unlocks spell (0x4000)", (p) =>
            {
                profile.Talents[TalentID.FencingSpells].Points |= 0x4000;
            });

            nodeIndex <<= 1; // now 0x8000
            var ironWill = new SkillNode(nodeIndex, "Iron Will", 9, "Unlocks spell (0x8000)", (p) =>
            {
                profile.Talents[TalentID.FencingSpells].Points |= 0x8000;
            });

            nodeIndex <<= 1; // next available bit – keep this as a passive bonus.
            var swiftRecovery = new SkillNode(nodeIndex, "Swift Recovery", 9, "Enhances recovery speed", (p) =>
            {
                profile.Talents[TalentID.FencingSpeed].Points += 1;
            });

            flurryOfBlows.AddChild(bladesGrace);
            precisionThrust.AddChild(counterAttack);
            guardingPosture.AddChild(ironWill);
            agileFootwork.AddChild(swiftRecovery);

            // Layer 5: Expert-level nodes (remain unchanged).
            nodeIndex <<= 1;
            var unyieldingStrike = new SkillNode(nodeIndex, "Unyielding Strike", 10, "Increases damage potential", (p) =>
            {
                profile.Talents[TalentID.FencingDamage].Points += 1;
            });

            nodeIndex <<= 1;
            var keenInstinct = new SkillNode(nodeIndex, "Keen Instinct", 10, "Boosts accuracy", (p) =>
            {
                profile.Talents[TalentID.FencingAccuracy].Points += 1;
            });

            nodeIndex <<= 1;
            var duelistsPoise = new SkillNode(nodeIndex, "Duelist's Poise", 10, "Unlocks advanced fencing spells", (p) =>
            {
                // Retained as a passive bonus here.
                profile.Talents[TalentID.FencingAccuracy].Points += 1;
            });

            nodeIndex <<= 1;
            var momentumShift = new SkillNode(nodeIndex, "Momentum Shift", 10, "Enhances speed in combat", (p) =>
            {
                profile.Talents[TalentID.FencingSpeed].Points += 1;
            });

            bladesGrace.AddChild(unyieldingStrike);
            counterAttack.AddChild(keenInstinct);
            ironWill.AddChild(duelistsPoise);
            swiftRecovery.AddChild(momentumShift);

            // Layer 6: Mastery nodes (remain unchanged).
            nodeIndex <<= 1;
            var expandedVision = new SkillNode(nodeIndex, "Expanded Vision", 11, "Improves situational awareness", (p) =>
            {
                profile.Talents[TalentID.FencingAccuracy].Points += 1;
            });

            nodeIndex <<= 1;
            var evasiveManeuvers = new SkillNode(nodeIndex, "Evasive Maneuvers", 11, "Further boosts evasion", (p) =>
            {
                profile.Talents[TalentID.FencingEvasion].Points += 1;
            });

            nodeIndex <<= 1;
            var masterSwordsman = new SkillNode(nodeIndex, "Master Swordsman", 11, "Unlocks ultimate fencing spells", (p) =>
            {
                // Retained as a passive bonus.
                profile.Talents[TalentID.FencingAccuracy].Points += 1;
            });

            nodeIndex <<= 1;
            var rapidAssault = new SkillNode(nodeIndex, "Rapid Assault", 11, "Increases damage output", (p) =>
            {
                profile.Talents[TalentID.FencingDamage].Points += 1;
            });

            unyieldingStrike.AddChild(expandedVision);
            keenInstinct.AddChild(evasiveManeuvers);
            duelistsPoise.AddChild(masterSwordsman);
            momentumShift.AddChild(rapidAssault);

            // Layer 7: Final, pinnacle bonuses (remain unchanged).
            nodeIndex <<= 1;
            var shieldingAura = new SkillNode(nodeIndex, "Shielding Aura", 12, "Provides a protective barrier", (p) =>
            {
                // Retained as a passive bonus.
                profile.Talents[TalentID.FencingEvasion].Points += 1;
            });

            nodeIndex <<= 1;
            var battleFocus = new SkillNode(nodeIndex, "Battle Focus", 12, "Enhances damage", (p) =>
            {
                profile.Talents[TalentID.FencingDamage].Points += 1;
            });

            nodeIndex <<= 1;
            var flashingSpeed = new SkillNode(nodeIndex, "Flashing Speed", 12, "Boosts combat speed", (p) =>
            {
                profile.Talents[TalentID.FencingSpeed].Points += 1;
            });

            nodeIndex <<= 1;
            var criticalPrecision = new SkillNode(nodeIndex, "Critical Precision", 12, "Further increases accuracy", (p) =>
            {
                profile.Talents[TalentID.FencingAccuracy].Points += 1;
            });

            expandedVision.AddChild(shieldingAura);
            evasiveManeuvers.AddChild(battleFocus);
            masterSwordsman.AddChild(flashingSpeed);
            rapidAssault.AddChild(criticalPrecision);

            // Layer 8: Ultimate node (remain unchanged).
            nodeIndex <<= 1;
            var ultimateDuelist = new SkillNode(nodeIndex, "Ultimate Duelist", 13, "Ultimate bonus: boosts all fencing skills", (p) =>
            {
                profile.Talents[TalentID.FencingSpells].Points |= 0x800 | 0x1000;
                profile.Talents[TalentID.FencingAccuracy].Points += 1;
                profile.Talents[TalentID.FencingEvasion].Points += 1;
                profile.Talents[TalentID.FencingSpeed].Points += 1;
                profile.Talents[TalentID.FencingDamage].Points += 1;
            });

            shieldingAura.AddChild(ultimateDuelist);
            battleFocus.AddChild(ultimateDuelist);
            flashingSpeed.AddChild(ultimateDuelist);
            criticalPrecision.AddChild(ultimateDuelist);
        }
    }
}
