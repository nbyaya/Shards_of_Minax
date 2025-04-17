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

namespace Server.ACC.CSS.Systems.TailoringMagic
{
    public class TailoringSkillTree : SuperGump
    {
        private TailoringTree tree;
        private Dictionary<TailoringSkillNode, Point2D> nodePositions;
        private Dictionary<int, TailoringSkillNode> buttonNodeMap;
        private Dictionary<TailoringSkillNode, int> edgeThickness;
        private const int buttonSize = 15;
        private int ySpacing = 50, xSpacing = 60;
        private int rootX = 300, rootY = 150;
        private TailoringSkillNode selectedNode;

        public TailoringSkillTree(PlayerMobile user)
            : base(user, null, 100, 100)
        {
            tree = new TailoringTree(user);
            nodePositions = new Dictionary<TailoringSkillNode, Point2D>();
            buttonNodeMap = new Dictionary<int, TailoringSkillNode>();
            edgeThickness = new Dictionary<TailoringSkillNode, int>();

            CalculateNodePositions(tree.Root, rootX, rootY, 0);
            InitializeEdgeThickness();

            User.SendGump(this);
        }

        private void CalculateNodePositions(TailoringSkillNode root, int x, int y, int depth)
        {
            if (root == null)
                return;

            var levelNodes = new Dictionary<int, List<TailoringSkillNode>>();
            var queue = new Queue<(TailoringSkillNode node, int level)>();
            var visited = new HashSet<TailoringSkillNode>();

            queue.Enqueue((root, 0));

            while (queue.Count > 0)
            {
                var (node, level) = queue.Dequeue();
                if (!visited.Add(node))
                    continue;

                if (!levelNodes.ContainsKey(level))
                    levelNodes[level] = new List<TailoringSkillNode>();

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
            layout.Add("title", () => { AddLabel(100, 20, 1153, "Tailoring Skill Tree"); });

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
            if (buttonNodeMap.TryGetValue(button.ButtonID, out TailoringSkillNode node))
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

    public class TailoringSkillNode
    {
        public int BitFlag { get; }
        public string Name { get; }
        public int Cost { get; }
        public string Description { get; }
        public List<TailoringSkillNode> Children { get; }
        public TailoringSkillNode Parent { get; private set; }
        private readonly Action<PlayerMobile> onActivate;

        public TailoringSkillNode(int bitFlag, string name, int cost, string description = "", Action<PlayerMobile> onActivate = null)
        {
            BitFlag = bitFlag;
            Name = name;
            Cost = cost;
            Description = description;
            Children = new List<TailoringSkillNode>();
            this.onActivate = onActivate;
        }

        public void AddChild(TailoringSkillNode child)
        {
            child.Parent = this;
            Children.Add(child);
        }

        public bool IsActivated(PlayerMobile player)
        {
            var profile = player.AcquireTalents();
            if (!profile.Talents.ContainsKey(TalentID.TailoringNodes))
                profile.Talents[TalentID.TailoringNodes] = new Talent(TalentID.TailoringNodes) { Points = 0 };

            return (profile.Talents[TalentID.TailoringNodes].Points & BitFlag) != 0;
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
            profile.Talents[TalentID.TailoringNodes].Points |= BitFlag;

            player.SendMessage($"{Name} activated!");
            onActivate?.Invoke(player);
            return true;
        }
    }

    public class TailoringTree
    {
        public TailoringSkillNode Root { get; }

        public TailoringTree(PlayerMobile player)
        {
            var profile = player.AcquireTalents();
            int nodeIndex = 0x01;

            // Layer 0: Root Node – Unlocks basic tailoring spells.
            Root = new TailoringSkillNode(nodeIndex, "Thread of Beginnings", 5, "Unlocks basic tailoring spells", (p) =>
            {
                profile.Talents[TalentID.TailoringSpells].Points |= 0x01;
            });

            // Layer 1: Basic bonuses.
            nodeIndex <<= 1;
            // Changed: Fabric Sense now unlocks spell flag 0x02 instead of a passive quality bonus.
            var fabricSense = new TailoringSkillNode(nodeIndex, "Fabric Sense", 6, "Unlocks spell: Fabric Insight", (p) =>
            {
                profile.Talents[TalentID.TailoringSpells].Points |= 0x02;
            });

            nodeIndex <<= 1;
            var swiftStitch = new TailoringSkillNode(nodeIndex, "Swift Stitch", 6, "Reduces tailoring crafting time", (p) =>
            {
                profile.Talents[TalentID.TailoringEfficiency].Points += 1;
            });

            nodeIndex <<= 1;
            var patternMastery = new TailoringSkillNode(nodeIndex, "Pattern Mastery", 6, "Unlocks bonus design patterns", (p) =>
            {
                profile.Talents[TalentID.TailoringSpells].Points |= 0x04;
            });

            nodeIndex <<= 1;
            var abundantYarn = new TailoringSkillNode(nodeIndex, "Abundant Yarn", 6, "Increases yarn yield", (p) =>
            {
                profile.Talents[TalentID.TailoringQuality].Points += 1;
            });

            Root.AddChild(fabricSense);
            Root.AddChild(swiftStitch);
            Root.AddChild(patternMastery);
            Root.AddChild(abundantYarn);

            // Layer 2: Advanced bonuses.
            nodeIndex <<= 1;
            var clothWhisper = new TailoringSkillNode(nodeIndex, "Cloth Whisper", 7, "Unlocks additional tailoring spells", (p) =>
            {
                profile.Talents[TalentID.TailoringSpells].Points |= 0x08;
            });

            nodeIndex <<= 1;
            // Changed: Seamless Stitch now unlocks spell flag 0x2000 instead of a passive efficiency bonus.
            var seamlessStitch = new TailoringSkillNode(nodeIndex, "Seamless Stitch", 7, "Unlocks spell: Seamless Weave", (p) =>
            {
                profile.Talents[TalentID.TailoringSpells].Points |= 0x2000;
            });

            nodeIndex <<= 1;
            var arcaneFabric = new TailoringSkillNode(nodeIndex, "Arcane Fabric", 7, "Unlocks advanced tailoring spells", (p) =>
            {
                profile.Talents[TalentID.TailoringSpells].Points |= 0x10;
            });

            nodeIndex <<= 1;
            var vibrantDye = new TailoringSkillNode(nodeIndex, "Vibrant Dye", 7, "Enhances color quality of crafted items", (p) =>
            {
                profile.Talents[TalentID.TailoringQuality].Points += 1;
            });

            fabricSense.AddChild(clothWhisper);
            swiftStitch.AddChild(seamlessStitch);
            patternMastery.AddChild(arcaneFabric);
            abundantYarn.AddChild(vibrantDye);

            // Layer 3: Further improvements.
            nodeIndex <<= 1;
            // Changed: Bountiful Bolt now unlocks spell flag 0x4000 instead of a passive quality bonus.
            var bountifulBolt = new TailoringSkillNode(nodeIndex, "Bountiful Bolt", 8, "Unlocks spell: Bolt of Plenty", (p) =>
            {
                profile.Talents[TalentID.TailoringSpells].Points |= 0x4000;
            });

            nodeIndex <<= 1;
            var razorEdge = new TailoringSkillNode(nodeIndex, "Razor Edge", 8, "Improves precision in cutting fabric", (p) =>
            {
                profile.Talents[TalentID.TailoringEfficiency].Points += 1;
            });

            nodeIndex <<= 1;
            var sturdySeam = new TailoringSkillNode(nodeIndex, "Sturdy Seam", 8, "Unlocks a durability bonus", (p) =>
            {
                profile.Talents[TalentID.TailoringSpells].Points |= 0x20;
            });

            nodeIndex <<= 1;
            var nimbleFingers = new TailoringSkillNode(nodeIndex, "Nimble Fingers", 8, "Enhances crafting speed", (p) =>
            {
                profile.Talents[TalentID.TailoringEfficiency].Points += 1;
            });

            clothWhisper.AddChild(bountifulBolt);
            seamlessStitch.AddChild(razorEdge);
            arcaneFabric.AddChild(sturdySeam);
            vibrantDye.AddChild(nimbleFingers);

            // Layer 4: More advanced enhancements.
            nodeIndex <<= 1;
            var wovenBlessing = new TailoringSkillNode(nodeIndex, "Woven Blessing", 9, "Boosts cloth yield further", (p) =>
            {
                profile.Talents[TalentID.TailoringQuality].Points += 1;
            });

            nodeIndex <<= 1;
            var faeThread = new TailoringSkillNode(nodeIndex, "Fae Thread", 9, "Unlocks fae-related bonus spells", (p) =>
            {
                profile.Talents[TalentID.TailoringSpells].Points |= 0x40;
            });

            nodeIndex <<= 1;
            var elderWeave = new TailoringSkillNode(nodeIndex, "Elder Weave", 9, "Unlocks elder fabric spells", (p) =>
            {
                profile.Talents[TalentID.TailoringSpells].Points |= 0x80;
            });

            nodeIndex <<= 1;
            var chromaticSurge = new TailoringSkillNode(nodeIndex, "Chromatic Surge", 9, "Enhances color infusion", (p) =>
            {
                profile.Talents[TalentID.TailoringQuality].Points += 1;
            });

            bountifulBolt.AddChild(wovenBlessing);
            razorEdge.AddChild(faeThread);
            sturdySeam.AddChild(elderWeave);
            nimbleFingers.AddChild(chromaticSurge);

            // Layer 5: Expert-level nodes.
            nodeIndex <<= 1;
            var primevalEfficiency = new TailoringSkillNode(nodeIndex, "Primeval Efficiency", 10, "Boosts overall crafting speed", (p) =>
            {
                profile.Talents[TalentID.TailoringEfficiency].Points += 1;
            });

            nodeIndex <<= 1;
            var bountifulHarvest = new TailoringSkillNode(nodeIndex, "Bountiful Harvest", 10, "Boosts cloth yield", (p) =>
            {
                profile.Talents[TalentID.TailoringQuality].Points += 1;
            });

            nodeIndex <<= 1;
            var designMastery = new TailoringSkillNode(nodeIndex, "Design Mastery", 10, "Unlocks mastery level spells", (p) =>
            {
                profile.Talents[TalentID.TailoringSpells].Points |= 0x100;
            });

            nodeIndex <<= 1;
            var precisionStitch = new TailoringSkillNode(nodeIndex, "Precision Stitch", 10, "Enhances cutting precision", (p) =>
            {
                profile.Talents[TalentID.TailoringEfficiency].Points += 1;
            });

            wovenBlessing.AddChild(primevalEfficiency);
            faeThread.AddChild(bountifulHarvest);
            elderWeave.AddChild(designMastery);
            chromaticSurge.AddChild(precisionStitch);

            // Layer 6: Mastery nodes.
            nodeIndex <<= 1;
            var expandedPerception = new TailoringSkillNode(nodeIndex, "Expanded Perception", 11, "Enhances pattern recognition", (p) =>
            {
                profile.Talents[TalentID.TailoringQuality].Points += 1;
            });

            nodeIndex <<= 1;
            // Changed: Mystic Fabric now unlocks spell flag 0x8000 instead of a passive quality bonus.
            var mysticFabric = new TailoringSkillNode(nodeIndex, "Mystic Fabric", 11, "Unlocks spell: Mystic Weave", (p) =>
            {
                profile.Talents[TalentID.TailoringSpells].Points |= 0x8000;
            });

            nodeIndex <<= 1;
            var ancientDesigner = new TailoringSkillNode(nodeIndex, "Ancient Designer", 11, "Unlocks ancient tailoring spells", (p) =>
            {
                profile.Talents[TalentID.TailoringSpells].Points |= 0x200;
            });

            nodeIndex <<= 1;
            var threadTransformation = new TailoringSkillNode(nodeIndex, "Thread Transformation", 11, "Enhances crafting efficiency with magic", (p) =>
            {
                profile.Talents[TalentID.TailoringEfficiency].Points += 1;
            });

            primevalEfficiency.AddChild(expandedPerception);
            bountifulHarvest.AddChild(mysticFabric);
            designMastery.AddChild(ancientDesigner);
            precisionStitch.AddChild(threadTransformation);

            // Layer 7: Final, pinnacle bonuses.
            nodeIndex <<= 1;
            var wovenBarrier = new TailoringSkillNode(nodeIndex, "Woven Barrier", 12, "Provides a protective fabric shield", (p) =>
            {
                profile.Talents[TalentID.TailoringSpells].Points |= 0x400;
            });

            nodeIndex <<= 1;
            var creativeEndowment = new TailoringSkillNode(nodeIndex, "Creative Endowment", 12, "Further increases cloth yield", (p) =>
            {
                profile.Talents[TalentID.TailoringQuality].Points += 1;
            });

            nodeIndex <<= 1;
            var patternFury = new TailoringSkillNode(nodeIndex, "Pattern Fury", 12, "Boosts cutting power", (p) =>
            {
                profile.Talents[TalentID.TailoringEfficiency].Points += 1;
            });

            nodeIndex <<= 1;
            var dyeOfTheWild = new TailoringSkillNode(nodeIndex, "Dye of the Wild", 12, "Enhances color infusion with wild magic", (p) =>
            {
                profile.Talents[TalentID.TailoringQuality].Points += 1;
            });

            expandedPerception.AddChild(wovenBarrier);
            mysticFabric.AddChild(creativeEndowment);
            ancientDesigner.AddChild(patternFury);
            threadTransformation.AddChild(dyeOfTheWild);

            // Layer 8: Ultimate node.
            nodeIndex <<= 1;
            var ultimateDesigner = new TailoringSkillNode(nodeIndex, "Ultimate Designer", 13, "Ultimate bonus: boosts all tailoring skills", (p) =>
            {
                profile.Talents[TalentID.TailoringSpells].Points |= 0x800 | 0x1000;
                profile.Talents[TalentID.TailoringEfficiency].Points += 1;
                profile.Talents[TalentID.TailoringQuality].Points += 1;
            });

            foreach (var node in new[] { wovenBarrier, dyeOfTheWild, patternFury, creativeEndowment })
            {
                node.AddChild(ultimateDesigner);
            }
        }
    }

    public class TailoringSkillTreeCommand
    {
        public static void Initialize()
        {
            CommandSystem.Register("TailorTree", AccessLevel.Player, cmd => ShowTree(cmd.Mobile));
        }

        private static void ShowTree(Mobile m)
        {
            if (m is PlayerMobile pm)
            {
                pm.SendMessage("Opening Tailoring Skill Tree...");
                pm.SendGump(new TailoringSkillTree(pm));
            }
            else
            {
                m.SendMessage("You must be a player to use this command.");
            }
        }
    }
}
