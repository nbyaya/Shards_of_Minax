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

namespace Server.ACC.CSS.Systems.ForensicsMagic
{
    // Revised Forensics Skill Tree Gump using Maxxia Points (AncientKnowledge) as the cost resource.
    public class ForensicsSkillTree : SuperGump
    {
        private ForensicsTree tree;
        private Dictionary<SkillNode, Point2D> nodePositions;
        private Dictionary<int, SkillNode> buttonNodeMap;
        private Dictionary<SkillNode, int> edgeThickness;
        private const int buttonSize = 15;
        private int ySpacing = 50, xSpacing = 60;
        private int rootX = 300, rootY = 150;
        private SkillNode selectedNode;

        public ForensicsSkillTree(PlayerMobile user)
            : base(user, null, 100, 100)
        {
            tree = new ForensicsTree(user);
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
            layout.Add("title", () => { AddLabel(100, 20, 1153, "Forensics Skill Tree"); });

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

    // Revised SkillNode class (identical in behavior to the Lumberjacking version)
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
            if (!profile.Talents.ContainsKey((TalentID)TalentID.ForensicNodes))
                profile.Talents[(TalentID)TalentID.ForensicNodes] = new Talent((TalentID)TalentID.ForensicNodes) { Points = 0 };

            return (profile.Talents[(TalentID)TalentID.ForensicNodes].Points & BitFlag) != 0;
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
            profile.Talents[(TalentID)TalentID.ForensicNodes].Points |= BitFlag;
            player.SendMessage($"{Name} activated!");
            onActivate?.Invoke(player);
            return true;
        }
    }

    // ForensicsTree builds the full tree with 9 layers (0-8) matching the Lumberjacking example.
    public class ForensicsTree
    {
        public SkillNode Root { get; }

        public ForensicsTree(PlayerMobile player)
        {
            var profile = player.AcquireTalents();
            int nodeIndex = 0x01;

            // Layer 0: Root Node – Spell node 0x01.
            Root = new SkillNode(nodeIndex, "Eye of the Investigator", 5, "Unlocks basic forensic spells", (p) =>
            {
                profile.Talents[(TalentID)TalentID.ForensicSpells].Points |= 0x01;
            });

            // Layer 1: 4 nodes, 2 spell nodes.
            nodeIndex <<= 1; // next flag for tree tracking (used in ForensicNodes)
            var traceDetection = new SkillNode(nodeIndex, "Trace Detection", 6, "Unlocks forensic spell (0x02)", (p) =>
            {
                profile.Talents[(TalentID)TalentID.ForensicSpells].Points |= 0x02;
            });

            nodeIndex <<= 1;
            var evidenceCollection = new SkillNode(nodeIndex, "Evidence Collection", 6, "Increases evaluation efficiency", (p) =>
            {
                profile.Talents[(TalentID)TalentID.ForensicEfficiency].Points += 1;
            });

            nodeIndex <<= 1;
            var chainOfCustody = new SkillNode(nodeIndex, "Chain of Custody", 6, "Unlocks forensic spell (0x04)", (p) =>
            {
                profile.Talents[(TalentID)TalentID.ForensicSpells].Points |= 0x04;
            });

            nodeIndex <<= 1;
            var crimeSceneAnalysis = new SkillNode(nodeIndex, "Crime Scene Analysis", 6, "Enhances evidence interpretation", (p) =>
            {
                profile.Talents[(TalentID)TalentID.ForensicRevelation].Points += 1;
            });

            Root.AddChild(traceDetection);
            Root.AddChild(evidenceCollection);
            Root.AddChild(chainOfCustody);
            Root.AddChild(crimeSceneAnalysis);

            // Layer 2: 4 nodes, 2 spell nodes.
            nodeIndex <<= 1;
            var forensicInsight = new SkillNode(nodeIndex, "Forensic Insight", 7, "Unlocks forensic spell (0x08)", (p) =>
            {
                profile.Talents[(TalentID)TalentID.ForensicSpells].Points |= 0x08;
            });

            nodeIndex <<= 1;
            var methodicalObservation = new SkillNode(nodeIndex, "Methodical Observation", 7, "Improves evaluation efficiency further", (p) =>
            {
                profile.Talents[(TalentID)TalentID.ForensicEfficiency].Points += 1;
            });

            nodeIndex <<= 1;
            var analyticalReasoning = new SkillNode(nodeIndex, "Analytical Reasoning", 7, "Unlocks forensic spell (0x10)", (p) =>
            {
                profile.Talents[(TalentID)TalentID.ForensicSpells].Points |= 0x10;
            });

            nodeIndex <<= 1;
            var preciseSampling = new SkillNode(nodeIndex, "Precise Sampling", 7, "Increases detection range further", (p) =>
            {
                profile.Talents[(TalentID)TalentID.ForensicInsight].Points += 1;
            });

            forensicInsight.AddChild(methodicalObservation);
            evidenceCollection.AddChild(preciseSampling);
            chainOfCustody.AddChild(analyticalReasoning);

            // Layer 3: 4 nodes, 2 spell nodes.
            nodeIndex <<= 1;
            var detailedReconstruction = new SkillNode(nodeIndex, "Detailed Reconstruction", 8, "Unlocks forensic spell (0x40)", (p) =>
            {
                profile.Talents[(TalentID)TalentID.ForensicSpells].Points |= 0x40;
            });

            nodeIndex <<= 1;
            var subtleClues = new SkillNode(nodeIndex, "Subtle Clues", 8, "Further improves evaluation efficiency", (p) =>
            {
                profile.Talents[(TalentID)TalentID.ForensicEfficiency].Points += 1;
            });

            nodeIndex <<= 1;
            var witnessExamination = new SkillNode(nodeIndex, "Witness Examination", 8, "Unlocks forensic spell (0x20)", (p) =>
            {
                profile.Talents[(TalentID)TalentID.ForensicSpells].Points |= 0x20;
            });

            nodeIndex <<= 1;
            var logicalDeduction = new SkillNode(nodeIndex, "Logical Deduction", 8, "Improves analytical efficiency", (p) =>
            {
                profile.Talents[(TalentID)TalentID.ForensicEfficiency].Points += 1;
            });

            forensicInsight.AddChild(detailedReconstruction);
            methodicalObservation.AddChild(subtleClues);
            analyticalReasoning.AddChild(witnessExamination);
            preciseSampling.AddChild(logicalDeduction);

            // Layer 4: 4 nodes, 2 spell nodes.
            nodeIndex <<= 1;
            var dnaProfiling = new SkillNode(nodeIndex, "DNA Profiling", 9, "Enhances evidence interpretation", (p) =>
            {
                profile.Talents[(TalentID)TalentID.ForensicRevelation].Points += 1;
            });

            nodeIndex <<= 1;
            var fingerprintMastery = new SkillNode(nodeIndex, "Fingerprint Mastery", 9, "Unlocks forensic spell (0x80)", (p) =>
            {
                profile.Talents[(TalentID)TalentID.ForensicSpells].Points |= 0x80;
            });

            nodeIndex <<= 1;
            var ballisticAnalysis = new SkillNode(nodeIndex, "Ballistic Analysis", 9, "Unlocks forensic spell (0x100)", (p) =>
            {
                profile.Talents[(TalentID)TalentID.ForensicSpells].Points |= 0x100;
            });

            nodeIndex <<= 1;
            var toxicologyExpertise = new SkillNode(nodeIndex, "Toxicology Expertise", 9, "Improves evaluation efficiency", (p) =>
            {
                profile.Talents[(TalentID)TalentID.ForensicEfficiency].Points += 1;
            });

            detailedReconstruction.AddChild(dnaProfiling);
            subtleClues.AddChild(fingerprintMastery);
            witnessExamination.AddChild(ballisticAnalysis);
            logicalDeduction.AddChild(toxicologyExpertise);

            // Layer 5: 4 nodes, 2 spell nodes.
            nodeIndex <<= 1;
            var advancedReconstruction = new SkillNode(nodeIndex, "Advanced Reconstruction", 10, "Unlocks forensic spell (0x400)", (p) =>
            {
                profile.Talents[(TalentID)TalentID.ForensicSpells].Points |= 0x400;
            });

            nodeIndex <<= 1;
            var evidencePreservation = new SkillNode(nodeIndex, "Evidence Preservation", 10, "Enhances evaluation efficiency", (p) =>
            {
                profile.Talents[(TalentID)TalentID.ForensicEfficiency].Points += 1;
            });

            nodeIndex <<= 1;
            var forensicMastery = new SkillNode(nodeIndex, "Forensic Mastery", 10, "Unlocks forensic spell (0x200)", (p) =>
            {
                profile.Talents[(TalentID)TalentID.ForensicSpells].Points |= 0x200;
            });

            nodeIndex <<= 1;
            var deductiveMomentum = new SkillNode(nodeIndex, "Deductive Momentum", 10, "Further improves evaluation efficiency", (p) =>
            {
                profile.Talents[(TalentID)TalentID.ForensicEfficiency].Points += 1;
            });

            dnaProfiling.AddChild(advancedReconstruction);
            subtleClues.AddChild(evidencePreservation);
            ballisticAnalysis.AddChild(forensicMastery);
            toxicologyExpertise.AddChild(deductiveMomentum);

            // Layer 6: 4 nodes, 2 spell nodes.
            nodeIndex <<= 1;
            var heightenedPerception = new SkillNode(nodeIndex, "Heightened Perception", 11, "Increases detection range", (p) =>
            {
                profile.Talents[(TalentID)TalentID.ForensicInsight].Points += 1;
            });

            nodeIndex <<= 1;
            var subtleNuance = new SkillNode(nodeIndex, "Subtle Nuance", 11, "Unlocks forensic spell (0x1000)", (p) =>
            {
                profile.Talents[(TalentID)TalentID.ForensicSpells].Points |= 0x1000;
            });

            nodeIndex <<= 1;
            var caseBreaker = new SkillNode(nodeIndex, "Case Breaker", 11, "Unlocks forensic spell (0x800)", (p) =>
            {
                profile.Talents[(TalentID)TalentID.ForensicSpells].Points |= 0x800;
            });

            nodeIndex <<= 1;
            var interrogationProwess = new SkillNode(nodeIndex, "Interrogation Prowess", 11, "Further improves evaluation efficiency", (p) =>
            {
                profile.Talents[(TalentID)TalentID.ForensicEfficiency].Points += 1;
            });

            advancedReconstruction.AddChild(heightenedPerception);
            evidencePreservation.AddChild(subtleNuance);
            forensicMastery.AddChild(caseBreaker);
            deductiveMomentum.AddChild(interrogationProwess);

            // Layer 7: 4 nodes, 2 spell nodes.
            nodeIndex <<= 1;
            var criticalBarrier = new SkillNode(nodeIndex, "Critical Barrier", 12, "Unlocks forensic spell (0x2000)", (p) =>
            {
                profile.Talents[(TalentID)TalentID.ForensicSpells].Points |= 0x2000;
            });

            nodeIndex <<= 1;
            var insightfulObservation = new SkillNode(nodeIndex, "Insightful Observation", 12, "Boosts evidence interpretation", (p) =>
            {
                profile.Talents[(TalentID)TalentID.ForensicRevelation].Points += 1;
            });

            nodeIndex <<= 1;
            var crimeSolvingInstinct = new SkillNode(nodeIndex, "Crime Solving Instinct", 12, "Unlocks forensic spell (0x4000)", (p) =>
            {
                profile.Talents[(TalentID)TalentID.ForensicSpells].Points |= 0x4000;
            });

            nodeIndex <<= 1;
            var finalAnalysis = new SkillNode(nodeIndex, "Final Analysis", 12, "Increases detection range", (p) =>
            {
                profile.Talents[(TalentID)TalentID.ForensicInsight].Points += 1;
            });

            heightenedPerception.AddChild(criticalBarrier);
            subtleNuance.AddChild(insightfulObservation);
            caseBreaker.AddChild(crimeSolvingInstinct);
            interrogationProwess.AddChild(finalAnalysis);

            // Layer 8: Ultimate node, 1 spell node.
            nodeIndex <<= 1;
            var ultimateForensics = new SkillNode(nodeIndex, "Ultimate Forensics", 13, "Unlocks forensic spell (0x8000) and boosts all forensic skills", (p) =>
            {
                profile.Talents[(TalentID)TalentID.ForensicSpells].Points |= 0x8000;
                profile.Talents[(TalentID)TalentID.ForensicInsight].Points += 1;
                profile.Talents[(TalentID)TalentID.ForensicEfficiency].Points += 1;
                profile.Talents[(TalentID)TalentID.ForensicRevelation].Points += 1;
            });

            foreach (var node in new[] { criticalBarrier, finalAnalysis, crimeSolvingInstinct, insightfulObservation })
            {
                node.AddChild(ultimateForensics);
            }
        }
    }

    // Command to open the Forensics Skill Tree.
    public class ForensicsSkillTreeCommand
    {
        public static void Initialize()
        {
            CommandSystem.Register("ForensicsTree", AccessLevel.Player, cmd => ShowTree(cmd.Mobile));
        }

        private static void ShowTree(Mobile m)
        {
            if (m is PlayerMobile pm)
            {
                pm.SendMessage("Opening Forensics Skill Tree...");
                pm.SendGump(new ForensicsSkillTree(pm));
            }
            else
            {
                m.SendMessage("You must be a player to use this command.");
            }
        }
    }
}
