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
using Server.Mobiles; // For Talent access

namespace Server.ACC.CSS.Systems.DetectHiddenMagic
{
    public class DetectHiddenSkillTree : SuperGump
    {
        private DetectHiddenTree tree;
        private Dictionary<SkillNode, Point2D> nodePositions;
        private Dictionary<int, SkillNode> buttonNodeMap;
        private Dictionary<SkillNode, int> edgeThickness;
        private const int buttonSize = 15;
        private int ySpacing = 50, xSpacing = 60;
        private int rootX = 300, rootY = 150;
        private SkillNode selectedNode;

        public DetectHiddenSkillTree(PlayerMobile user)
            : base(user, null, 100, 100)
        {
            tree = new DetectHiddenTree(user);
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
            layout.Add("title", () => { AddLabel(100, 20, 1153, "Detect Hidden Skill Tree"); });

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

    // Basic SkillNode class reused by both systems.
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
            if (!profile.Talents.ContainsKey(TalentID.DetectHiddenNodes))
                profile.Talents[TalentID.DetectHiddenNodes] = new Talent(TalentID.DetectHiddenNodes) { Points = 0 };

            return (profile.Talents[TalentID.DetectHiddenNodes].Points & BitFlag) != 0;
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
            profile.Talents[TalentID.DetectHiddenNodes].Points |= BitFlag;

            player.SendMessage($"{Name} activated!");
            onActivate?.Invoke(player);
            return true;
        }
    }

    // The Detect Hidden tree structure – 30 nodes in 9 layers.
    public class DetectHiddenTree
    {
        public SkillNode Root { get; }

        public DetectHiddenTree(PlayerMobile player)
        {
            var profile = player.AcquireTalents();
            int nodeIndex = 0x01;

            // Layer 0: Root Node – Unlocks basic detect hidden spells.
            Root = new SkillNode(nodeIndex, "Eye of the Seeker", 5, "Unlocks basic detect hidden spells", (p) =>
            {
                // Unlocks spell: 0x01
                profile.Talents[TalentID.DetectHiddenSpells].Points |= 0x01;
            });

            // Layer 1: Basic bonuses.
            nodeIndex <<= 1;
            var keenVision = new SkillNode(nodeIndex, "Keen Vision", 6, "Increases detection range", (p) =>
            {
                profile.Talents[TalentID.DetectHiddenRange].Points += 1;
            });

            nodeIndex <<= 1;
            var shadowDiscernment = new SkillNode(nodeIndex, "Shadow Discernment", 6, "Improves chance to reveal hidden foes", (p) =>
            {
                profile.Talents[TalentID.DetectHiddenChance].Points += 1;
            });

            nodeIndex <<= 1;
            // Converted from passive to spell unlock: Silent Steps now unlocks spell 0x02.
            var silentSteps = new SkillNode(nodeIndex, "Silent Steps", 6, "Unlocks a hidden spell", (p) =>
            {
                profile.Talents[TalentID.DetectHiddenSpells].Points |= 0x02;
            });

            nodeIndex <<= 1;
            var etherealInsight = new SkillNode(nodeIndex, "Ethereal Insight", 6, "Unlocks an additional detection spell", (p) =>
            {
                profile.Talents[TalentID.DetectHiddenSpells].Points |= 0x04;
            });

            Root.AddChild(keenVision);
            Root.AddChild(shadowDiscernment);
            Root.AddChild(silentSteps);
            Root.AddChild(etherealInsight);

            // Layer 2: Advanced bonuses.
            nodeIndex <<= 1;
            var phantomSight = new SkillNode(nodeIndex, "Phantom Sight", 7, "Further increases detection range", (p) =>
            {
                profile.Talents[TalentID.DetectHiddenRange].Points += 1;
            });

            nodeIndex <<= 1;
            // Converted from passive to spell unlock: Mystic Awareness now unlocks spell 0x400.
            var mysticAwareness = new SkillNode(nodeIndex, "Mystic Awareness", 7, "Unlocks an advanced spell", (p) =>
            {
                profile.Talents[TalentID.DetectHiddenSpells].Points |= 0x400;
            });

            nodeIndex <<= 1;
            var spectralReveal = new SkillNode(nodeIndex, "Spectral Reveal", 7, "Unlocks an advanced detection spell", (p) =>
            {
                profile.Talents[TalentID.DetectHiddenSpells].Points |= 0x08;
            });

            nodeIndex <<= 1;
            var umbralPerception = new SkillNode(nodeIndex, "Umbral Perception", 7, "Additional reduction in enemy stealth", (p) =>
            {
                profile.Talents[TalentID.DetectHiddenStealthReduction].Points += 1;
            });

            keenVision.AddChild(phantomSight);
            shadowDiscernment.AddChild(mysticAwareness);
            etherealInsight.AddChild(spectralReveal);
            silentSteps.AddChild(umbralPerception);

            // Layer 3: Further improvements.
            nodeIndex <<= 1;
            var allSeeingEye = new SkillNode(nodeIndex, "All-Seeing Eye", 8, "Greatly increases detection range", (p) =>
            {
                profile.Talents[TalentID.DetectHiddenRange].Points += 1;
            });

            nodeIndex <<= 1;
            var eclipseVision = new SkillNode(nodeIndex, "Eclipse Vision", 8, "Significantly improves detection chance", (p) =>
            {
                profile.Talents[TalentID.DetectHiddenChance].Points += 1;
            });

            nodeIndex <<= 1;
            var veiledTruth = new SkillNode(nodeIndex, "Veiled Truth", 8, "Unlocks a hidden ability spell", (p) =>
            {
                profile.Talents[TalentID.DetectHiddenSpells].Points |= 0x10;
            });

            nodeIndex <<= 1;
            // Converted from passive to spell unlock: Nocturnal Reflexes now unlocks spell 0x800.
            var nocturnalReflexes = new SkillNode(nodeIndex, "Nocturnal Reflexes", 8, "Unlocks a hidden spell", (p) =>
            {
                profile.Talents[TalentID.DetectHiddenSpells].Points |= 0x800;
            });

            phantomSight.AddChild(allSeeingEye);
            mysticAwareness.AddChild(eclipseVision);
            spectralReveal.AddChild(veiledTruth);
            umbralPerception.AddChild(nocturnalReflexes);

            // Layer 4:
            nodeIndex <<= 1;
            var clairvoyantsGaze = new SkillNode(nodeIndex, "Clairvoyant's Gaze", 9, "Further increases detection range", (p) =>
            {
                profile.Talents[TalentID.DetectHiddenRange].Points += 1;
            });

            nodeIndex <<= 1;
            // Converted from passive to spell unlock: Shadowbane now unlocks spell 0x1000.
            var shadowbane = new SkillNode(nodeIndex, "Shadowbane", 9, "Unlocks a hidden spell", (p) =>
            {
                profile.Talents[TalentID.DetectHiddenSpells].Points |= 0x1000;
            });

            nodeIndex <<= 1;
            var specterChanneling = new SkillNode(nodeIndex, "Specter Channeling", 9, "Unlocks an advanced detection spell", (p) =>
            {
                profile.Talents[TalentID.DetectHiddenSpells].Points |= 0x20;
            });

            nodeIndex <<= 1;
            var obsidianVeil = new SkillNode(nodeIndex, "Obsidian Veil", 9, "Further reduces enemy stealth", (p) =>
            {
                profile.Talents[TalentID.DetectHiddenStealthReduction].Points += 1;
            });

            allSeeingEye.AddChild(clairvoyantsGaze);
            eclipseVision.AddChild(shadowbane);
            veiledTruth.AddChild(specterChanneling);
            nocturnalReflexes.AddChild(obsidianVeil);

            // Layer 5:
            nodeIndex <<= 1;
            var luminousFocus = new SkillNode(nodeIndex, "Luminous Focus", 10, "Improves detection range", (p) =>
            {
                profile.Talents[TalentID.DetectHiddenRange].Points += 1;
            });

            nodeIndex <<= 1;
            // Converted from passive to spell unlock: Ghostly Aura now unlocks spell 0x2000.
            var ghostlyAura = new SkillNode(nodeIndex, "Ghostly Aura", 10, "Unlocks a hidden spell", (p) =>
            {
                profile.Talents[TalentID.DetectHiddenSpells].Points |= 0x2000;
            });

            nodeIndex <<= 1;
            var revealersMight = new SkillNode(nodeIndex, "Revealer's Might", 10, "Unlocks a mastery detection spell", (p) =>
            {
                profile.Talents[TalentID.DetectHiddenSpells].Points |= 0x40;
            });

            nodeIndex <<= 1;
            var umbralDissolution = new SkillNode(nodeIndex, "Umbral Dissolution", 10, "Further reduces enemy stealth", (p) =>
            {
                profile.Talents[TalentID.DetectHiddenStealthReduction].Points += 1;
            });

            clairvoyantsGaze.AddChild(luminousFocus);
            shadowbane.AddChild(ghostlyAura);
            specterChanneling.AddChild(revealersMight);
            obsidianVeil.AddChild(umbralDissolution);

            // Layer 6:
            nodeIndex <<= 1;
            var primalPerception = new SkillNode(nodeIndex, "Primal Perception", 11, "Greatly improves detection range", (p) =>
            {
                profile.Talents[TalentID.DetectHiddenRange].Points += 1;
            });

            nodeIndex <<= 1;
            var arcaneAwareness = new SkillNode(nodeIndex, "Arcane Awareness", 11, "Greatly boosts detection chance", (p) =>
            {
                profile.Talents[TalentID.DetectHiddenChance].Points += 1;
            });

            nodeIndex <<= 1;
            var hiddenMastery = new SkillNode(nodeIndex, "Hidden Mastery", 11, "Unlocks a mastery detection spell", (p) =>
            {
                profile.Talents[TalentID.DetectHiddenSpells].Points |= 0x80;
            });

            nodeIndex <<= 1;
            // Converted from passive to spell unlock: Evasive Shadow now unlocks spell 0x4000.
            var evasiveShadow = new SkillNode(nodeIndex, "Evasive Shadow", 11, "Unlocks a hidden spell", (p) =>
            {
                profile.Talents[TalentID.DetectHiddenSpells].Points |= 0x4000;
            });

            luminousFocus.AddChild(primalPerception);
            ghostlyAura.AddChild(arcaneAwareness);
            revealersMight.AddChild(hiddenMastery);
            umbralDissolution.AddChild(evasiveShadow);

            // Layer 7:
            nodeIndex <<= 1;
            var celestialInsight = new SkillNode(nodeIndex, "Celestial Insight", 12, "Significantly increases detection range", (p) =>
            {
                profile.Talents[TalentID.DetectHiddenRange].Points += 1;
            });

            nodeIndex <<= 1;
            var phantasmalPrecision = new SkillNode(nodeIndex, "Phantasmal Precision", 12, "Significantly boosts detection chance", (p) =>
            {
                profile.Talents[TalentID.DetectHiddenChance].Points += 1;
            });

            nodeIndex <<= 1;
            var spectralDominance = new SkillNode(nodeIndex, "Spectral Dominance", 12, "Unlocks the pinnacle detection spell", (p) =>
            {
                profile.Talents[TalentID.DetectHiddenSpells].Points |= 0x100;
            });

            nodeIndex <<= 1;
            // Converted from passive to spell unlock: Eclipse of Shadows now unlocks spell 0x8000.
            var eclipseOfShadows = new SkillNode(nodeIndex, "Eclipse of Shadows", 12, "Unlocks a hidden spell", (p) =>
            {
                profile.Talents[TalentID.DetectHiddenSpells].Points |= 0x8000;
            });

            primalPerception.AddChild(celestialInsight);
            arcaneAwareness.AddChild(phantasmalPrecision);
            hiddenMastery.AddChild(spectralDominance);
            evasiveShadow.AddChild(eclipseOfShadows);

            // Layer 8: Ultimate Node.
            nodeIndex <<= 1;
            // Modified Ultimate Seer now only unlocks spell 0x200.
            var ultimateSeer = new SkillNode(nodeIndex, "Ultimate Seer", 13, "Ultimate bonus: boosts all Detect Hidden skills", (p) =>
            {
                profile.Talents[TalentID.DetectHiddenSpells].Points |= 0x200;
                profile.Talents[TalentID.DetectHiddenRange].Points += 1;
                profile.Talents[TalentID.DetectHiddenChance].Points += 1;
                profile.Talents[TalentID.DetectHiddenStealthReduction].Points += 1;
            });

            celestialInsight.AddChild(ultimateSeer);
            phantasmalPrecision.AddChild(ultimateSeer);
            spectralDominance.AddChild(ultimateSeer);
            eclipseOfShadows.AddChild(ultimateSeer);
        }
    }
}
