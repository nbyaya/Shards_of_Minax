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

namespace Server.ACC.CSS.Systems.MiningMagic
{
    public class MiningSkillTree : SuperGump
    {
        private MiningTree tree;
        private Dictionary<SkillNode, Point2D> nodePositions;
        private Dictionary<int, SkillNode> buttonNodeMap;
        private Dictionary<SkillNode, int> edgeThickness;
        private const int buttonSize = 15;
        private int ySpacing = 50, xSpacing = 60;
        private int rootX = 300, rootY = 150;
        private SkillNode selectedNode;

        public MiningSkillTree(PlayerMobile user)
            : base(user, null, 100, 100)
        {
            tree = new MiningTree(user);
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
            queue.Enqueue((root, 0));

            while (queue.Count > 0)
            {
                var (node, level) = queue.Dequeue();

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
            layout.Add("title", () => { AddLabel(100, 20, 1153, "Mining Skill Tree"); });

            layout.Add("selectedNodeText", () =>
            {
                if (selectedNode != null)
                {
                    PlayerMobile player = User as PlayerMobile;
                    string text;

                    if (selectedNode.IsActivated(player))
                        text = $"<BASEFONT COLOR=#FFFFFF>{selectedNode.Name}</BASEFONT>";
                    else if (selectedNode.CanBeActivated(player))
                        text = $"<BASEFONT COLOR=#FFFF00>Click to unlock {selectedNode.Name} (Cost: {selectedNode.Cost} Maxxia points)</BASEFONT>";
                    else
                        text = $"<BASEFONT COLOR=#FF0000>{selectedNode.Name} Locked! Unlock the previous node first.</BASEFONT>";

                    AddHtml(100, 50, 300, 40, text, false, false);
                }
            });

            // New layout element for the node's description.
            layout.Add("selectedNodeDescription", () =>
            {
                if (selectedNode != null)
                {
                    string descriptionText = $"<BASEFONT COLOR=#FF0000>{selectedNode.Description}</BASEFONT>";
                    // Adjust the Y coordinate as needed to position below the node name.
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

    // Revised SkillNode that uses Ancient Knowledge points for cost and updates mining talents.
    public class SkillNode
    {
        public int BitFlag { get; }
        public string Name { get; }
        public int Cost { get; }
        public string Description { get; } // New property for the node description.
        public List<SkillNode> Children { get; }
        public SkillNode Parent { get; private set; }
        private readonly Action<PlayerMobile> onActivate;

        // Constructor updated to accept a description.
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
            if (!profile.Talents.ContainsKey(TalentID.MiningNodes))
                profile.Talents[TalentID.MiningNodes] = new Talent(TalentID.MiningNodes) { Points = 0 };

            return (profile.Talents[TalentID.MiningNodes].Points & BitFlag) != 0;
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
                player.SendMessage("You have no Ancient Knowledge points available.");
                return false;
            }

            if (ancientKnowledge.Points < Cost)
            {
                player.SendMessage($"You need {Cost} Ancient Knowledge points to unlock {Name}.");
                return false;
            }

            ancientKnowledge.Points -= Cost;
            profile.Talents[TalentID.MiningNodes].Points |= BitFlag;

            player.SendMessage($"{Name} activated!");
            onActivate?.Invoke(player);
            return true;
        }
    }

    // Full Mining tree structure with nine layers (0 to 8) and four nodes per layer.
    public class MiningTree
    {
        public SkillNode Root { get; }

        public MiningTree(PlayerMobile player)
        {
            var profile = player.AcquireTalents();
            int nodeIndex = 0x01;

            // Layer 0: Root Node – Unlocks basic mining spell.
            Root = new SkillNode(nodeIndex, "Call of the Earth", 5, "Unlocks a basic mining spell", (p) =>
            {
                profile.Talents[TalentID.MiningSpells].Points |= 0x01;
            });

            // Layer 1: Basic bonuses and one spell unlock.
            nodeIndex <<= 1;
            var oreInsight = new SkillNode(nodeIndex, "Ore Insight", 6, "Improves your ore detection abilities", (p) =>
            {
                profile.Talents[TalentID.MiningEfficiency].Points += 1;
            });

            nodeIndex <<= 1;
            var stoneSense = new SkillNode(nodeIndex, "Stone Sense", 6, "Enhances your ability to sense stone deposits", (p) =>
            {
                profile.Talents[TalentID.MiningRange].Points += 1;
            });

            nodeIndex <<= 1;
            var minerFortitude = new SkillNode(nodeIndex, "Miner's Fortitude", 6, "Increases your mining yield", (p) =>
            {
                profile.Talents[TalentID.MiningYield].Points += 1;
            });

            nodeIndex <<= 1;
            var veinSeer = new SkillNode(nodeIndex, "Vein Seer", 6, "Unlocks a spell to reveal hidden veins", (p) =>
            {
                profile.Talents[TalentID.MiningSpells].Points |= 0x02;
            });

            Root.AddChild(oreInsight);
            Root.AddChild(stoneSense);
            Root.AddChild(minerFortitude);
            Root.AddChild(veinSeer);

            // Layer 2: Unlock additional spells and bonus yield.
            nodeIndex <<= 1;
            var earthsEmbrace = new SkillNode(nodeIndex, "Earth's Embrace", 7, "Opens up additional mining spells", (p) =>
            {
                profile.Talents[TalentID.MiningSpells].Points |= 0x04;
            });

            nodeIndex <<= 1;
            var crystalGlimmer = new SkillNode(nodeIndex, "Crystal Glimmer", 7, "Reveals the sparkle of hidden crystals", (p) =>
            {
                profile.Talents[TalentID.MiningSpells].Points |= 0x08;
            });

            // Changed from passive to spell unlock.
            nodeIndex <<= 1;
            var deeperDelve = new SkillNode(nodeIndex, "Deeper Delve", 7, "Unlocks a spell that enhances your mining yield", (p) =>
            {
                profile.Talents[TalentID.MiningSpells].Points |= 0x800;
            });

            nodeIndex <<= 1;
            var subterraneanFocus = new SkillNode(nodeIndex, "Subterranean Focus", 7, "Enhances your mining efficiency", (p) =>
            {
                profile.Talents[TalentID.MiningEfficiency].Points += 1;
            });

            oreInsight.AddChild(earthsEmbrace);
            stoneSense.AddChild(crystalGlimmer);
            minerFortitude.AddChild(deeperDelve);
            veinSeer.AddChild(subterraneanFocus);

            // Layer 3: Further efficiency and yield improvements.
            // Convert two of these nodes into spell unlocks.
            nodeIndex <<= 1;
            var rapidExtraction = new SkillNode(nodeIndex, "Rapid Extraction", 8, "Unlocks a spell for faster ore extraction", (p) =>
            {
                profile.Talents[TalentID.MiningSpells].Points |= 0x200;
            });

            nodeIndex <<= 1;
            var mineralMagnet = new SkillNode(nodeIndex, "Mineral Magnet", 8, "Unlocks a spell that attracts more minerals", (p) =>
            {
                profile.Talents[TalentID.MiningSpells].Points |= 0x400;
            });

            nodeIndex <<= 1;
            var hardenedPick = new SkillNode(nodeIndex, "Hardened Pick", 8, "Improves your pick's durability", (p) =>
            {
                profile.Talents[TalentID.MiningEfficiency].Points += 1;
            });

            nodeIndex <<= 1;
            var deepResonance = new SkillNode(nodeIndex, "Deep Resonance", 8, "Extends your mining range", (p) =>
            {
                profile.Talents[TalentID.MiningRange].Points += 1;
            });

            earthsEmbrace.AddChild(rapidExtraction);
            crystalGlimmer.AddChild(mineralMagnet);
            deeperDelve.AddChild(hardenedPick);
            subterraneanFocus.AddChild(deepResonance);

            // Layer 4: Advanced magical enhancements.
            nodeIndex <<= 1;
            var elementalWhisper = new SkillNode(nodeIndex, "Elemental Whisper", 9, "Unlocks elemental mining magic", (p) =>
            {
                profile.Talents[TalentID.MiningSpells].Points |= 0x10;
            });

            nodeIndex <<= 1;
            var rockResonance = new SkillNode(nodeIndex, "Rock Resonance", 9, "Boosts your connection with stone", (p) =>
            {
                profile.Talents[TalentID.MiningRange].Points += 1;
            });

            nodeIndex <<= 1;
            var gemGlitter = new SkillNode(nodeIndex, "Gem Glitter", 9, "Unlocks a spell to reveal hidden gems within rock", (p) =>
            {
                profile.Talents[TalentID.MiningSpells].Points |= 0x20;
            });

            nodeIndex <<= 1;
            var echoesOfDepths = new SkillNode(nodeIndex, "Echoes of the Depths", 9, "Increases yield from deep mining", (p) =>
            {
                profile.Talents[TalentID.MiningYield].Points += 1;
            });

            rapidExtraction.AddChild(elementalWhisper);
            mineralMagnet.AddChild(rockResonance);
            hardenedPick.AddChild(gemGlitter);
            deepResonance.AddChild(echoesOfDepths);

            // Layer 5: Expert-level nodes.
            nodeIndex <<= 1;
            var preciseProspecting = new SkillNode(nodeIndex, "Precise Prospecting", 10, "Refines your ore prospecting skills", (p) =>
            {
                profile.Talents[TalentID.MiningYield].Points += 1;
            });

            nodeIndex <<= 1;
            var efficientDigging = new SkillNode(nodeIndex, "Efficient Digging", 10, "Improves your digging speed", (p) =>
            {
                profile.Talents[TalentID.MiningEfficiency].Points += 1;
            });

            nodeIndex <<= 1;
            var oreOracle = new SkillNode(nodeIndex, "Ore Oracle", 10, "Unlocks a spell granting insight into ore locations", (p) =>
            {
                profile.Talents[TalentID.MiningSpells].Points |= 0x40;
            });

            nodeIndex <<= 1;
            var subterraneanMastery = new SkillNode(nodeIndex, "Subterranean Mastery", 10, "Enhances your mining range", (p) =>
            {
                profile.Talents[TalentID.MiningRange].Points += 1;
            });

            elementalWhisper.AddChild(preciseProspecting);
            rockResonance.AddChild(efficientDigging);
            gemGlitter.AddChild(oreOracle);
            echoesOfDepths.AddChild(subterraneanMastery);

            // Layer 6: Mastery nodes.
            nodeIndex <<= 1;
            var expandedExcavation = new SkillNode(nodeIndex, "Expanded Excavation", 11, "Broadens your excavation capabilities", (p) =>
            {
                profile.Talents[TalentID.MiningEfficiency].Points += 1;
            });

            nodeIndex <<= 1;
            var mysticDigging = new SkillNode(nodeIndex, "Mystic Digging", 11, "Imbues your digging with mystical power", (p) =>
            {
                profile.Talents[TalentID.MiningYield].Points += 1;
            });

            nodeIndex <<= 1;
            var ancientOreSeer = new SkillNode(nodeIndex, "Ancient Ore Seer", 11, "Unlocks ancient ore detection spells", (p) =>
            {
                profile.Talents[TalentID.MiningSpells].Points |= 0x80;
            });

            nodeIndex <<= 1;
            var deepCoreInsight = new SkillNode(nodeIndex, "Deep Core Insight", 11, "Enhances your deep mining range", (p) =>
            {
                profile.Talents[TalentID.MiningRange].Points += 1;
            });

            preciseProspecting.AddChild(expandedExcavation);
            efficientDigging.AddChild(mysticDigging);
            oreOracle.AddChild(ancientOreSeer);
            subterraneanMastery.AddChild(deepCoreInsight);

            // Layer 7: Pinnacle bonus nodes.
            // All four nodes in this layer are now spell unlocks.
            nodeIndex <<= 1;
            var titansGrip = new SkillNode(nodeIndex, "Titan's Grip", 12, "Unlocks a spell that boosts mining efficiency", (p) =>
            {
                profile.Talents[TalentID.MiningSpells].Points |= 0x1000;
            });

            nodeIndex <<= 1;
            var minersEndowment = new SkillNode(nodeIndex, "Miner's Endowment", 12, "Unlocks a spell that increases mining yield", (p) =>
            {
                profile.Talents[TalentID.MiningSpells].Points |= 0x2000;
            });

            nodeIndex <<= 1;
            var cavesFury = new SkillNode(nodeIndex, "Cave's Fury", 12, "Unlocks a spell that fuels mining power with raw fury", (p) =>
            {
                profile.Talents[TalentID.MiningSpells].Points |= 0x4000;
            });

            nodeIndex <<= 1;
            var echoesOfDeep = new SkillNode(nodeIndex, "Echoes of the Deep", 12, "Unlocks a spell that enhances range with deep resonance", (p) =>
            {
                profile.Talents[TalentID.MiningSpells].Points |= 0x8000;
            });

            expandedExcavation.AddChild(titansGrip);
            mysticDigging.AddChild(minersEndowment);
            ancientOreSeer.AddChild(cavesFury);
            deepCoreInsight.AddChild(echoesOfDeep);

            // Layer 8: Ultimate node (attached to all Layer 7 nodes).
            nodeIndex <<= 1;
            var ultimateMiner = new SkillNode(nodeIndex, "Ultimate Miner", 13, "Grants the ultimate mining bonus: increased yield, efficiency, range, and spell power", (p) =>
            {
                profile.Talents[TalentID.MiningYield].Points += 1;
                profile.Talents[TalentID.MiningEfficiency].Points += 1;
                profile.Talents[TalentID.MiningRange].Points += 1;
                profile.Talents[TalentID.MiningSpells].Points |= 0x100;
            });

            titansGrip.AddChild(ultimateMiner);
            minersEndowment.AddChild(ultimateMiner);
            cavesFury.AddChild(ultimateMiner);
            echoesOfDeep.AddChild(ultimateMiner);
        }
    }

    // Command to open the Mining Skill Tree.
    public class MiningSkillTreeCommand
    {
        public static void Initialize()
        {
            CommandSystem.Register("MiningTree", AccessLevel.Player, cmd => ShowTree(cmd.Mobile));
        }

        private static void ShowTree(Mobile m)
        {
            if (m is PlayerMobile pm)
            {
                pm.SendMessage("Opening Mining Skill Tree...");
                pm.SendGump(new MiningSkillTree(pm));
            }
            else
            {
                m.SendMessage("You must be a player to use this command.");
            }
        }
    }
}
