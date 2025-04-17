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

namespace Server.ACC.CSS.Systems.EvalIntMagic
{
    // This gump displays the EvalInt skill tree.
    public class EvalIntSkillTree : SuperGump
    {
        private EvalIntTree tree;
        private Dictionary<SkillNode, Point2D> nodePositions;
        private Dictionary<int, SkillNode> buttonNodeMap;
        private Dictionary<SkillNode, int> edgeThickness;
        private const int buttonSize = 15;
        private int ySpacing = 50, xSpacing = 60;
        private int rootX = 300, rootY = 150;
        private SkillNode selectedNode;

        public EvalIntSkillTree(PlayerMobile user)
            : base(user, null, 100, 100)
        {
            tree = new EvalIntTree(user);
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
            layout.Add("title", () => { AddLabel(100, 20, 1153, "EvalInt Skill Tree"); });

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

    // A simple SkillNode class. (This mirrors the Lumberjacking version but uses EvalInt talent IDs.)
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
            if (!profile.Talents.ContainsKey(TalentID.EvalIntNodes))
                profile.Talents[TalentID.EvalIntNodes] = new Talent(TalentID.EvalIntNodes) { Points = 0 };

            return (profile.Talents[TalentID.EvalIntNodes].Points & BitFlag) != 0;
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
            profile.Talents[TalentID.EvalIntNodes].Points |= BitFlag;
            player.SendMessage($"{Name} activated!");
            onActivate?.Invoke(player);
            return true;
        }
    }

    // This class builds the full tree.
    public class EvalIntTree
    {
        public SkillNode Root { get; }

        public EvalIntTree(PlayerMobile player)
        {
            var profile = player.AcquireTalents();
            int nodeIndex = 0x01;

            // Layer 0: Root Node – Unlocks basic EvalInt spells.
            Root = new SkillNode(nodeIndex, "Eye of the Sage", 5, "Unlocks basic EvalInt spells", (p) =>
            {
                profile.Talents[TalentID.EvalIntSpells].Points |= 0x01;
            });

            // Layer 1: Basic bonuses.
            nodeIndex <<= 1;
            var keenPerception = new SkillNode(nodeIndex, "Keen Perception", 6, "Unlocks additional EvalInt spell", (p) =>
            {
                profile.Talents[TalentID.EvalIntSpells].Points |= 0x02;
            });
            nodeIndex <<= 1;
            var mindsEye = new SkillNode(nodeIndex, "Mind's Eye", 6, "Unlocks enhanced EvalInt spell", (p) =>
            {
                profile.Talents[TalentID.EvalIntSpells].Points |= 0x10;
            });
            nodeIndex <<= 1;
            var cogentCalculation = new SkillNode(nodeIndex, "Cogent Calculation", 6, "Unlocks bonus EvalInt spell", (p) =>
            {
                profile.Talents[TalentID.EvalIntSpells].Points |= 0x04;
            });
            nodeIndex <<= 1;
            var analyticalReasoning = new SkillNode(nodeIndex, "Analytical Reasoning", 6, "Enhances overall EvalInt effectiveness", (p) =>
            {
                // Passive bonus.
            });
            Root.AddChild(keenPerception);
            Root.AddChild(mindsEye);
            Root.AddChild(cogentCalculation);
            Root.AddChild(analyticalReasoning);

            // Layer 2: Advanced bonuses.
            nodeIndex <<= 1;
            var arcaneInsight = new SkillNode(nodeIndex, "Arcane Insight", 7, "Unlocks additional EvalInt spells", (p) =>
            {
                profile.Talents[TalentID.EvalIntSpells].Points |= 0x08;
            });
            nodeIndex <<= 1;
            var focusedIntellect = new SkillNode(nodeIndex, "Focused Intellect", 7, "Unlocks further EvalInt spell", (p) =>
            {
                profile.Talents[TalentID.EvalIntSpells].Points |= 0x40;
            });
            nodeIndex <<= 1;
            var intuitiveJudgement = new SkillNode(nodeIndex, "Intuitive Judgement", 7, "Boosts mana evaluation", (p) =>
            {
                // Passive bonus.
            });
            nodeIndex <<= 1;
            var preciseMetrics = new SkillNode(nodeIndex, "Precise Metrics", 7, "Enhances clarity of evaluations", (p) =>
            {
                // Passive bonus.
            });
            keenPerception.AddChild(arcaneInsight);
            mindsEye.AddChild(focusedIntellect);
            cogentCalculation.AddChild(intuitiveJudgement);
            analyticalReasoning.AddChild(preciseMetrics);

            // Layer 3: Further enhancements.
            nodeIndex <<= 1;
            var sagesIntuition = new SkillNode(nodeIndex, "Sage's Intuition", 8, "Improves evaluation outcomes", (p) =>
            {
                // Passive bonus.
            });
            nodeIndex <<= 1;
            var logicalRigor = new SkillNode(nodeIndex, "Logical Rigor", 8, "Reduces evaluation error margin by 1", (p) =>
            {
                // Passive bonus.
            });
            nodeIndex <<= 1;
            var mentalAgility = new SkillNode(nodeIndex, "Mental Agility", 8, "Speeds up evaluation process", (p) =>
            {
                // Passive bonus.
            });
            nodeIndex <<= 1;
            var reasonedApproach = new SkillNode(nodeIndex, "Reasoned Approach", 8, "Enhances mana evaluation further", (p) =>
            {
                // Passive bonus.
            });
            arcaneInsight.AddChild(sagesIntuition);
            focusedIntellect.AddChild(logicalRigor);
            intuitiveJudgement.AddChild(mentalAgility);
            preciseMetrics.AddChild(reasonedApproach);

            // Layer 4: Advanced magical enhancements.
            nodeIndex <<= 1;
            var cerebralBurst = new SkillNode(nodeIndex, "Cerebral Burst", 9, "Major bonus: boosts EvalInt spells", (p) =>
            {
                profile.Talents[TalentID.EvalIntSpells].Points |= 0x20;
            });
            nodeIndex <<= 1;
            var philosophersStone = new SkillNode(nodeIndex, "Philosopher's Stone", 9, "Enhances mind reading abilities", (p) =>
            {
                // Passive bonus.
            });
            nodeIndex <<= 1;
            var mentalFortitude = new SkillNode(nodeIndex, "Mental Fortitude", 9, "Reduces evaluation error margin by 1", (p) =>
            {
                // Passive bonus.
            });
            nodeIndex <<= 1;
            var cognitiveResonance = new SkillNode(nodeIndex, "Cognitive Resonance", 9, "Unlocks advanced EvalInt spells", (p) =>
            {
                profile.Talents[TalentID.EvalIntSpells].Points |= 0x80;
            });
            sagesIntuition.AddChild(cerebralBurst);
            logicalRigor.AddChild(philosophersStone);
            mentalAgility.AddChild(mentalFortitude);
            reasonedApproach.AddChild(cognitiveResonance);

            // Layer 5: Expert-level nodes.
            nodeIndex <<= 1;
            var primeIntellect = new SkillNode(nodeIndex, "Prime Intellect", 10, "Boosts overall EvalInt skill", (p) =>
            {
                // Passive bonus.
            });
            nodeIndex <<= 1;
            var eurekaMoment = new SkillNode(nodeIndex, "Eureka Moment", 10, "Chance to reveal hidden properties", (p) =>
            {
                // Passive bonus.
            });
            nodeIndex <<= 1;
            var mindMastery = new SkillNode(nodeIndex, "Mind Mastery", 10, "Unlocks master-level EvalInt spells", (p) =>
            {
                profile.Talents[TalentID.EvalIntSpells].Points |= 0x100;
            });
            nodeIndex <<= 1;
            var intellectualMomentum = new SkillNode(nodeIndex, "Intellectual Momentum", 10, "Enhances mental prowess", (p) =>
            {
                // Passive bonus.
            });
            cerebralBurst.AddChild(primeIntellect);
            philosophersStone.AddChild(eurekaMoment);
            mentalFortitude.AddChild(mindMastery);
            cognitiveResonance.AddChild(intellectualMomentum);

            // Layer 6: Mastery nodes.
            nodeIndex <<= 1;
            var expandedCognition = new SkillNode(nodeIndex, "Expanded Cognition", 11, "Greatly enhances mental evaluation", (p) =>
            {
                // Passive bonus.
            });
            nodeIndex <<= 1;
            var mysticMind = new SkillNode(nodeIndex, "Mystic Mind", 11, "Boosts EvalInt spell potency", (p) =>
            {
                // Passive bonus.
            });
            nodeIndex <<= 1;
            var ancientMentalism = new SkillNode(nodeIndex, "Ancient Mentalism", 11, "Unlocks ancient EvalInt spells", (p) =>
            {
                profile.Talents[TalentID.EvalIntSpells].Points |= 0x200;
            });
            nodeIndex <<= 1;
            var mentalTransformation = new SkillNode(nodeIndex, "Mental Transformation", 11, "Enhances overall mental abilities", (p) =>
            {
                // Passive bonus.
            });
            primeIntellect.AddChild(expandedCognition);
            eurekaMoment.AddChild(mysticMind);
            mindMastery.AddChild(ancientMentalism);
            intellectualMomentum.AddChild(mentalTransformation);

            // Layer 7: Final, pinnacle bonuses.
            nodeIndex <<= 1;
            var thoughtBarrier = new SkillNode(nodeIndex, "Thought Barrier", 12, "Provides protection against mental attacks", (p) =>
            {
                profile.Talents[TalentID.EvalIntSpells].Points |= 0x400;
            });
            nodeIndex <<= 1;
            var cerebralEndowment = new SkillNode(nodeIndex, "Cerebral Endowment", 12, "Unlocks a potent EvalInt spell", (p) =>
            {
                profile.Talents[TalentID.EvalIntSpells].Points |= 0x2000;
            });
            nodeIndex <<= 1;
            var intellectsFury = new SkillNode(nodeIndex, "Intellect's Fury", 12, "Unlocks a powerful EvalInt spell", (p) =>
            {
                profile.Talents[TalentID.EvalIntSpells].Points |= 0x4000;
            });
            nodeIndex <<= 1;
            var echoesOfWisdom = new SkillNode(nodeIndex, "Echoes of Wisdom", 12, "Unlocks an ultimate EvalInt spell", (p) =>
            {
                profile.Talents[TalentID.EvalIntSpells].Points |= 0x8000;
            });
            expandedCognition.AddChild(thoughtBarrier);
            mysticMind.AddChild(cerebralEndowment);
            ancientMentalism.AddChild(intellectsFury);
            mentalTransformation.AddChild(echoesOfWisdom);

            // Layer 8: Ultimate node.
            nodeIndex <<= 1;
            var ultimateSage = new SkillNode(nodeIndex, "Ultimate Sage", 13, "Ultimate bonus: boosts all EvalInt abilities", (p) =>
            {
                profile.Talents[TalentID.EvalIntSpells].Points |= 0x800 | 0x1000;
                // Additional bonuses can be applied here.
            });
            thoughtBarrier.AddChild(ultimateSage);
            cerebralEndowment.AddChild(ultimateSage);
            intellectsFury.AddChild(ultimateSage);
            echoesOfWisdom.AddChild(ultimateSage);
        }
    }

    // Command to open the EvalInt Skill Tree.
    public class EvalIntSkillTreeCommand
    {
        public static void Initialize()
        {
            CommandSystem.Register("EvalIntTree", AccessLevel.Player, cmd => ShowTree(cmd.Mobile));
        }

        private static void ShowTree(Mobile m)
        {
            if (m is PlayerMobile pm)
            {
                pm.SendMessage("Opening EvalInt Skill Tree...");
                pm.SendGump(new EvalIntSkillTree(pm));
            }
            else
            {
                m.SendMessage("You must be a player to use this command.");
            }
        }
    }
}
