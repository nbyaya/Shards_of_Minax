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

namespace Server.ACC.CSS.Systems.DiscordanceMagic
{
    // Revised Discordance Skill Tree Gump using Maxxia Points (AncientKnowledge) as the cost resource.
    public class DiscordanceSkillTree : SuperGump
    {
        private DiscordanceTree tree;
        private Dictionary<SkillNode, Point2D> nodePositions;
        private Dictionary<int, SkillNode> buttonNodeMap;
        private Dictionary<SkillNode, int> edgeThickness;
        private const int buttonSize = 15;
        private int ySpacing = 50, xSpacing = 60;
        private int rootX = 300, rootY = 150;
        private SkillNode selectedNode;

        public DiscordanceSkillTree(PlayerMobile user)
            : base(user, null, 100, 100)
        {
            tree = new DiscordanceTree(user);
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

            // Create level-order groups
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

            // Position nodes on each level, centered at rootX
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
            layout.Add("title", () => { AddLabel(100, 20, 1153, "Discordance Skill Tree"); });

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

    // SkillNode class for Discordance (nearly identical to the Lumberjacking version)
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
            if (!profile.Talents.ContainsKey(TalentID.DiscordanceNodes))
                profile.Talents[TalentID.DiscordanceNodes] = new Talent(TalentID.DiscordanceNodes) { Points = 0 };

            return (profile.Talents[TalentID.DiscordanceNodes].Points & BitFlag) != 0;
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
            profile.Talents[TalentID.DiscordanceNodes].Points |= BitFlag;
            player.SendMessage($"{Name} activated!");
            onActivate?.Invoke(player);
            return true;
        }
    }

    // DiscordanceTree builds the full node structure (8 layers) – similar in number to the Lumberjacking tree.
    public class DiscordanceTree
    {
        public SkillNode Root { get; }

        public DiscordanceTree(PlayerMobile player)
        {
            var profile = player.AcquireTalents();
            int nodeIndex = 0x01;

            // Layer 0: Root Node – Unlocks basic discordance spells.
            Root = new SkillNode(nodeIndex, "Resonant Dissonance", 5, "Unlocks basic discordance abilities", (p) =>
            {
                profile.Talents[TalentID.DiscordanceSpells].Points |= 0x01;
            });

            // Layer 1: Basic bonuses.
            nodeIndex <<= 1; // 0x02
            var sonicRipple = new SkillNode(nodeIndex, "Sonic Ripple", 6, "Unlocks spell: Sonic Ripple", (p) =>
            {
                profile.Talents[TalentID.DiscordanceSpells].Points |= 0x02;
            });
            nodeIndex <<= 1; // 0x04
            var dissonantChord = new SkillNode(nodeIndex, "Dissonant Chord", 6, "Improves discordance effectiveness", (p) =>
            {
                profile.Talents[TalentID.DiscordanceEffect].Points += 1;
            });
            nodeIndex <<= 1; // 0x08
            var echoMastery = new SkillNode(nodeIndex, "Echo Mastery", 6, "Unlocks bonus echo spells", (p) =>
            {
                profile.Talents[TalentID.DiscordanceSpells].Points |= 0x04;
            });
            nodeIndex <<= 1; // 0x10
            var harmonicSurge = new SkillNode(nodeIndex, "Harmonic Surge", 6, "Boosts passive resistance reduction", (p) =>
            {
                profile.Talents[TalentID.DiscordancePassive].Points += 1;
            });
            Root.AddChild(sonicRipple);
            Root.AddChild(dissonantChord);
            Root.AddChild(echoMastery);
            Root.AddChild(harmonicSurge);

            // Layer 2: Advanced bonuses.
            nodeIndex <<= 1; // 0x20
            var reverberation = new SkillNode(nodeIndex, "Reverberation", 7, "Unlocks additional discordance spells", (p) =>
            {
                profile.Talents[TalentID.DiscordanceSpells].Points |= 0x08;
            });
            nodeIndex <<= 1; // 0x40
            var attenuatedEcho = new SkillNode(nodeIndex, "Attenuated Echo", 7, "Enhances effect duration", (p) =>
            {
                profile.Talents[TalentID.DiscordanceEffect].Points += 1;
            });
            nodeIndex <<= 1; // 0x80
            var spectralChord = new SkillNode(nodeIndex, "Spectral Chord", 7, "Unlocks advanced discordance spells", (p) =>
            {
                profile.Talents[TalentID.DiscordanceSpells].Points |= 0x10;
            });
            nodeIndex <<= 1; // 0x100
            var vibrationalShift = new SkillNode(nodeIndex, "Vibrational Shift", 7, "Unlocks spell: Vibrational Shift", (p) =>
            {
                profile.Talents[TalentID.DiscordanceSpells].Points |= 0x1000;
            });
            sonicRipple.AddChild(reverberation);
            dissonantChord.AddChild(attenuatedEcho);
            echoMastery.AddChild(spectralChord);
            harmonicSurge.AddChild(vibrationalShift);

            // Layer 3: Further improvements.
            nodeIndex <<= 1; // 0x200
            var resonantBoost = new SkillNode(nodeIndex, "Resonant Boost", 8, "Enhances discordance potency", (p) =>
            {
                profile.Talents[TalentID.DiscordanceEffect].Points += 1;
            });
            nodeIndex <<= 1; // 0x400
            var discordantFlow = new SkillNode(nodeIndex, "Discordant Flow", 8, "Improves casting speed", (p) =>
            {
                profile.Talents[TalentID.DiscordanceCastSpeed].Points += 1;
            });
            nodeIndex <<= 1; // 0x800
            var echoResilience = new SkillNode(nodeIndex, "Echo Resilience", 8, "Unlocks spell: Echo Resilience", (p) =>
            {
                profile.Talents[TalentID.DiscordanceSpells].Points |= 0x2000;
            });
            nodeIndex <<= 1; // 0x1000
            var riftEdge = new SkillNode(nodeIndex, "Rift Edge", 8, "Unlocks minor spell enhancements", (p) =>
            {
                profile.Talents[TalentID.DiscordanceSpells].Points |= 0x20;
            });
            reverberation.AddChild(resonantBoost);
            attenuatedEcho.AddChild(discordantFlow);
            spectralChord.AddChild(echoResilience);
            vibrationalShift.AddChild(riftEdge);

            // Layer 4: More advanced.
            nodeIndex <<= 1; // 0x2000
            var sonicBarrier = new SkillNode(nodeIndex, "Sonic Barrier", 9, "Unlocks spell: Sonic Barrier", (p) =>
            {
                profile.Talents[TalentID.DiscordanceSpells].Points |= 0x4000;
            });
            nodeIndex <<= 1; // 0x4000
            var dissonanceSurge = new SkillNode(nodeIndex, "Dissonance Surge", 9, "Unlocks surge spells", (p) =>
            {
                profile.Talents[TalentID.DiscordanceSpells].Points |= 0x40;
            });
            nodeIndex <<= 1; // 0x8000
            var spectralMastery = new SkillNode(nodeIndex, "Spectral Mastery", 9, "Unlocks master-level discordance spells", (p) =>
            {
                profile.Talents[TalentID.DiscordanceSpells].Points |= 0x80;
            });
            nodeIndex <<= 1; // 0x10000 (bit value not used for spells)
            var harmonicFortitude = new SkillNode(nodeIndex, "Harmonic Fortitude", 9, "Boosts passive benefits", (p) =>
            {
                profile.Talents[TalentID.DiscordancePassive].Points += 1;
            });
            resonantBoost.AddChild(sonicBarrier);
            discordantFlow.AddChild(dissonanceSurge);
            echoResilience.AddChild(spectralMastery);
            riftEdge.AddChild(harmonicFortitude);

            // Layer 5: Expert nodes.
            nodeIndex <<= 1; // next available bit (not used for spells)
            var intensifiedResonance = new SkillNode(nodeIndex, "Intensified Resonance", 10, "Boosts overall discordance effectiveness", (p) =>
            {
                profile.Talents[TalentID.DiscordanceEffect].Points += 1;
            });
            nodeIndex <<= 1;
            var echoAmplification = new SkillNode(nodeIndex, "Echo Amplification", 10, "Enhances spell amplification", (p) =>
            {
                profile.Talents[TalentID.DiscordanceSpells].Points |= 0x100;
            });
            nodeIndex <<= 1;
            var discordanceMastery = new SkillNode(nodeIndex, "Discordance Mastery", 10, "Unlocks elite discordance spells", (p) =>
            {
                profile.Talents[TalentID.DiscordanceSpells].Points |= 0x200;
            });
            nodeIndex <<= 1;
            var cadenceFlow = new SkillNode(nodeIndex, "Cadence Flow", 10, "Unlocks spell: Cadence Flow", (p) =>
            {
                profile.Talents[TalentID.DiscordanceSpells].Points |= 0x8000;
            });
            sonicBarrier.AddChild(intensifiedResonance);
            dissonanceSurge.AddChild(echoAmplification);
            spectralMastery.AddChild(discordanceMastery);
            harmonicFortitude.AddChild(cadenceFlow);

            // Layer 6: Mastery nodes. (All remain passive.)
            nodeIndex <<= 1;
            var spatialAwareness = new SkillNode(nodeIndex, "Spatial Awareness", 11, "Enhances positional advantage", (p) =>
            {
                profile.Talents[TalentID.DiscordanceRange].Points += 1;
            });
            nodeIndex <<= 1;
            var sonicPrecision = new SkillNode(nodeIndex, "Sonic Precision", 11, "Improves spell accuracy", (p) =>
            {
                profile.Talents[TalentID.DiscordanceEffect].Points += 1;
            });
            nodeIndex <<= 1;
            var discordantInsight = new SkillNode(nodeIndex, "Discordant Insight", 11, "Unlocks insight-based abilities", (p) =>
            {
                profile.Talents[TalentID.DiscordancePassive].Points += 1;
            });
            nodeIndex <<= 1;
            var tempoTransformation = new SkillNode(nodeIndex, "Tempo Transformation", 11, "Increases casting efficiency", (p) =>
            {
                profile.Talents[TalentID.DiscordanceCastSpeed].Points += 1;
            });
            intensifiedResonance.AddChild(spatialAwareness);
            echoAmplification.AddChild(sonicPrecision);
            discordanceMastery.AddChild(discordantInsight);
            cadenceFlow.AddChild(tempoTransformation);

            // Layer 7: Pinnacle nodes.
            nodeIndex <<= 1;
            var reverberantShield = new SkillNode(nodeIndex, "Reverberant Shield", 12, "Provides a protective sonic barrier", (p) =>
            {
                profile.Talents[TalentID.DiscordanceSpells].Points |= 0x400;
            });
            nodeIndex <<= 1;
            var discordantEndowment = new SkillNode(nodeIndex, "Discordant Endowment", 12, "Boosts passive discordance bonuses", (p) =>
            {
                profile.Talents[TalentID.DiscordancePassive].Points += 1;
            });
            nodeIndex <<= 1;
            var resoundingForce = new SkillNode(nodeIndex, "Resounding Force", 12, "Enhances spell power", (p) =>
            {
                profile.Talents[TalentID.DiscordanceEffect].Points += 1;
            });
            nodeIndex <<= 1;
            var echoingStride = new SkillNode(nodeIndex, "Echoing Stride", 12, "Improves casting range", (p) =>
            {
                profile.Talents[TalentID.DiscordanceCastSpeed].Points += 1;
            });
            spatialAwareness.AddChild(reverberantShield);
            sonicPrecision.AddChild(discordantEndowment);
            discordantInsight.AddChild(resoundingForce);
            tempoTransformation.AddChild(echoingStride);

            // Layer 8: Ultimate node.
            nodeIndex <<= 1;
            var ultimateDiscordance = new SkillNode(nodeIndex, "Ultimate Discordance", 13, "Ultimate bonus: boosts all discordance abilities", (p) =>
            {
                profile.Talents[TalentID.DiscordanceSpells].Points |= 0x800;
                profile.Talents[TalentID.DiscordanceRange].Points += 1;
                profile.Talents[TalentID.DiscordanceEffect].Points += 1;
                profile.Talents[TalentID.DiscordanceCastSpeed].Points += 1;
                profile.Talents[TalentID.DiscordancePassive].Points += 1;
            });
            foreach (var node in new[] { reverberantShield, echoingStride, resoundingForce, discordantEndowment })
            {
                node.AddChild(ultimateDiscordance);
            }
        }
    }

    // Command to open the Discordance Skill Tree.
    public class DiscordanceSkillTreeCommand
    {
        public static void Initialize()
        {
            CommandSystem.Register("DiscordTree", AccessLevel.Player, cmd => ShowTree(cmd.Mobile));
        }

        private static void ShowTree(Mobile m)
        {
            if (m is PlayerMobile pm)
            {
                pm.SendMessage("Opening Discordance Skill Tree...");
                pm.SendGump(new DiscordanceSkillTree(pm));
            }
            else
            {
                m.SendMessage("You must be a player to use this command.");
            }
        }
    }
}
