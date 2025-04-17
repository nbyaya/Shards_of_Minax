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
using Server.Mobiles; // for TalentProfile and TalentID

namespace Server.ACC.CSS.Systems.TasteIDMagic
{
    // TasteID Skill Tree Gump
    public class TasteIDSkillTree : SuperGump
    {
        private TasteIDTree tree;
        private Dictionary<SkillNode, Point2D> nodePositions;
        private Dictionary<int, SkillNode> buttonNodeMap;
        private Dictionary<SkillNode, int> edgeThickness;
        private const int buttonSize = 15;
        private int ySpacing = 50, xSpacing = 60;
        private int rootX = 300, rootY = 150;
        private SkillNode selectedNode;

        public TasteIDSkillTree(PlayerMobile user)
            : base(user, null, 100, 100)
        {
            tree = new TasteIDTree(user);
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
            layout.Add("title", () => { AddLabel(100, 20, 1153, "Connoisseur’s Taste Skill Tree"); });

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

    // SkillNode class (used by both Lumberjacking and TasteID trees)
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
            if (!profile.Talents.ContainsKey(TalentID.TasteIDNodes))
                profile.Talents[TalentID.TasteIDNodes] = new Talent(TalentID.TasteIDNodes) { Points = 0 };

            return (profile.Talents[TalentID.TasteIDNodes].Points & BitFlag) != 0;
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
            profile.Talents[TalentID.TasteIDNodes].Points |= BitFlag;
            player.SendMessage($"{Name} activated!");
            onActivate?.Invoke(player);
            return true;
        }
    }

    // The TasteID tree structure
    public class TasteIDTree
    {
        public SkillNode Root { get; }

        public TasteIDTree(PlayerMobile player)
        {
            var profile = player.AcquireTalents();
            int nodeIndex = 0x01;

            // Layer 0: Root Node – Unlocks basic TasteID spells.
            Root = new SkillNode(nodeIndex, "Savor the Essence", 5, "Unlocks basic TasteID spells", (p) =>
            {
                // Unlock basic TasteID spells (set bit 0x01)
                profile.Talents[TalentID.TasteIDSpells].Points |= 0x01;
            });

            // Layer 1: Basic bonuses.
            nodeIndex <<= 1;
            var palateAwakening = new SkillNode(nodeIndex, "Palate Awakening", 6, "Unlocks an additional taste spell", (p) =>
            {
                // Now unlocks spell 0x02 instead of a bonus
                profile.Talents[TalentID.TasteIDSpells].Points |= 0x02;
            });

            nodeIndex <<= 1;
            var flavorFocus = new SkillNode(nodeIndex, "Flavor Focus", 6, "Enhances your ability to discern ingredients", (p) =>
            {
                profile.Talents[TalentID.TasteIDAnalysis].Points += 1;
            });

            nodeIndex <<= 1;
            var culinaryMastery = new SkillNode(nodeIndex, "Culinary Mastery", 6, "Unlocks a bonus taste spell", (p) =>
            {
                profile.Talents[TalentID.TasteIDSpells].Points |= 0x04;
            });

            nodeIndex <<= 1;
            var savorNature = new SkillNode(nodeIndex, "Savor Nature", 6, "Enhances detection of natural flavors", (p) =>
            {
                profile.Talents[TalentID.TasteIDRefinement].Points += 1;
            });

            Root.AddChild(palateAwakening);
            Root.AddChild(flavorFocus);
            Root.AddChild(culinaryMastery);
            Root.AddChild(savorNature);

            // Layer 2: Advanced magical and practical bonuses.
            nodeIndex <<= 1;
            var spiceWhisper = new SkillNode(nodeIndex, "Spice Whisper", 7, "Unlocks additional taste spells", (p) =>
            {
                profile.Talents[TalentID.TasteIDSpells].Points |= 0x08;
            });

            nodeIndex <<= 1;
            var aromaFlow = new SkillNode(nodeIndex, "Aroma Flow", 7, "Unlocks further taste spells", (p) =>
            {
                // Changed: now unlocks spell 0x2000 instead of bonus analysis
                profile.Talents[TalentID.TasteIDSpells].Points |= 0x2000;
            });

            nodeIndex <<= 1;
            var umamiRevelation = new SkillNode(nodeIndex, "Umami Revelation", 7, "Unlocks advanced taste spells", (p) =>
            {
                profile.Talents[TalentID.TasteIDSpells].Points |= 0x10;
            });

            nodeIndex <<= 1;
            var herbalInsight = new SkillNode(nodeIndex, "Herbal Insight", 7, "Unlocks an ancient taste spell", (p) =>
            {
                // Changed: now unlocks spell 0x4000 instead of bonus sensitivity
                profile.Talents[TalentID.TasteIDSpells].Points |= 0x4000;
            });

            palateAwakening.AddChild(spiceWhisper);
            flavorFocus.AddChild(aromaFlow);
            culinaryMastery.AddChild(umamiRevelation);
            savorNature.AddChild(herbalInsight);

            // Layer 3: Further bonuses.
            nodeIndex <<= 1;
            var savoryGrove = new SkillNode(nodeIndex, "Savory Grove", 8, "Enhances flavor yield", (p) =>
            {
                profile.Talents[TalentID.TasteIDRefinement].Points += 1;
            });

            nodeIndex <<= 1;
            var zestSurge = new SkillNode(nodeIndex, "Zest Surge", 8, "Improves taste analysis", (p) =>
            {
                profile.Talents[TalentID.TasteIDAnalysis].Points += 1;
            });

            nodeIndex <<= 1;
            var epicureanResilience = new SkillNode(nodeIndex, "Epicurean Resilience", 8, "Unlocks a defensive taste bonus", (p) =>
            {
                profile.Talents[TalentID.TasteIDSpells].Points |= 0x20;
            });

            nodeIndex <<= 1;
            var refinedReflex = new SkillNode(nodeIndex, "Refined Reflex", 8, "Increases sensory reaction", (p) =>
            {
                profile.Talents[TalentID.TasteIDSensitivity].Points += 1;
            });

            spiceWhisper.AddChild(savoryGrove);
            aromaFlow.AddChild(zestSurge);
            umamiRevelation.AddChild(epicureanResilience);
            herbalInsight.AddChild(refinedReflex);

            // Layer 4: More advanced enhancements.
            nodeIndex <<= 1;
            var tastefulBlessing = new SkillNode(nodeIndex, "Tasteful Blessing", 9, "Enhances flavor yield further", (p) =>
            {
                profile.Talents[TalentID.TasteIDRefinement].Points += 1;
            });

            nodeIndex <<= 1;
            var spicesGrace = new SkillNode(nodeIndex, "Spice's Grace", 9, "Unlocks gourmet taste spells", (p) =>
            {
                profile.Talents[TalentID.TasteIDSpells].Points |= 0x40;
            });

            nodeIndex <<= 1;
            var vintageEssence = new SkillNode(nodeIndex, "Vintage Essence", 9, "Unlocks ancient taste spells", (p) =>
            {
                profile.Talents[TalentID.TasteIDSpells].Points |= 0x80;
            });

            nodeIndex <<= 1;
            var aromaticBoost = new SkillNode(nodeIndex, "Aromatic Boost", 9, "Unlocks an ultimate taste spell", (p) =>
            {
                // Changed: now unlocks spell 0x8000 instead of bonus sensitivity
                profile.Talents[TalentID.TasteIDSpells].Points |= 0x8000;
            });

            savoryGrove.AddChild(tastefulBlessing);
            zestSurge.AddChild(spicesGrace);
            epicureanResilience.AddChild(vintageEssence);
            refinedReflex.AddChild(aromaticBoost);

            // Layer 5: Expert-level nodes.
            nodeIndex <<= 1;
            var primeFlavor = new SkillNode(nodeIndex, "Prime Flavor", 10, "Boosts overall taste analysis", (p) =>
            {
                profile.Talents[TalentID.TasteIDAnalysis].Points += 1;
            });

            nodeIndex <<= 1;
            var bountifulFeast = new SkillNode(nodeIndex, "Bountiful Feast", 10, "Enhances flavor yield", (p) =>
            {
                profile.Talents[TalentID.TasteIDRefinement].Points += 1;
            });

            nodeIndex <<= 1;
            var recipeMastery = new SkillNode(nodeIndex, "Recipe Mastery", 10, "Unlocks mastery-level taste spells", (p) =>
            {
                profile.Talents[TalentID.TasteIDSpells].Points |= 0x100;
            });

            nodeIndex <<= 1;
            var momentumOfTaste = new SkillNode(nodeIndex, "Momentum of Taste", 10, "Further improves taste analysis", (p) =>
            {
                profile.Talents[TalentID.TasteIDAnalysis].Points += 1;
            });

            tastefulBlessing.AddChild(primeFlavor);
            spicesGrace.AddChild(bountifulFeast);
            vintageEssence.AddChild(recipeMastery);
            aromaticBoost.AddChild(momentumOfTaste);

            // Layer 6: Mastery nodes.
            nodeIndex <<= 1;
            var expandedPalate = new SkillNode(nodeIndex, "Expanded Palate", 11, "Enhances overall sensory awareness", (p) =>
            {
                profile.Talents[TalentID.TasteIDSensitivity].Points += 1;
            });

            nodeIndex <<= 1;
            var mysticMorsel = new SkillNode(nodeIndex, "Mystic Morsel", 11, "Boosts flavor yield magically", (p) =>
            {
                profile.Talents[TalentID.TasteIDRefinement].Points += 1;
            });

            nodeIndex <<= 1;
            var ancientConnoisseur = new SkillNode(nodeIndex, "Ancient Connoisseur", 11, "Unlocks ancient taste spells", (p) =>
            {
                profile.Talents[TalentID.TasteIDSpells].Points |= 0x200;
            });

            nodeIndex <<= 1;
            var culinaryTransformation = new SkillNode(nodeIndex, "Culinary Transformation", 11, "Improves taste analysis with magic", (p) =>
            {
                profile.Talents[TalentID.TasteIDAnalysis].Points += 1;
            });

            primeFlavor.AddChild(expandedPalate);
            bountifulFeast.AddChild(mysticMorsel);
            recipeMastery.AddChild(ancientConnoisseur);
            momentumOfTaste.AddChild(culinaryTransformation);

            // Layer 7: Final, pinnacle bonuses.
            nodeIndex <<= 1;
            var gastronomicBarrier = new SkillNode(nodeIndex, "Gastronomic Barrier", 12, "Unlocks a protective taste barrier", (p) =>
            {
                profile.Talents[TalentID.TasteIDSpells].Points |= 0x400;
            });

            nodeIndex <<= 1;
            var naturesEndorsement = new SkillNode(nodeIndex, "Nature's Endorsement", 12, "Further increases flavor yield", (p) =>
            {
                profile.Talents[TalentID.TasteIDRefinement].Points += 1;
            });

            nodeIndex <<= 1;
            var feastsFury = new SkillNode(nodeIndex, "Feast's Fury", 12, "Enhances taste analysis", (p) =>
            {
                profile.Talents[TalentID.TasteIDAnalysis].Points += 1;
            });

            nodeIndex <<= 1;
            var echoesOfFlavor = new SkillNode(nodeIndex, "Echoes of Flavor", 12, "Boosts sensory range", (p) =>
            {
                profile.Talents[TalentID.TasteIDSensitivity].Points += 1;
            });

            expandedPalate.AddChild(gastronomicBarrier);
            mysticMorsel.AddChild(naturesEndorsement);
            ancientConnoisseur.AddChild(feastsFury);
            culinaryTransformation.AddChild(echoesOfFlavor);

            // Layer 8: Ultimate node.
            nodeIndex <<= 1;
            var ultimateConnoisseur = new SkillNode(nodeIndex, "Ultimate Connoisseur", 13, "Ultimate bonus: boosts all taste skills", (p) =>
            {
                // Grants a final bonus: unlock additional spells and add to all bonus talents.
                profile.Talents[TalentID.TasteIDSpells].Points |= 0x800 | 0x1000;
                profile.Talents[TalentID.TasteIDSensitivity].Points += 1;
                profile.Talents[TalentID.TasteIDAnalysis].Points += 1;
                profile.Talents[TalentID.TasteIDRefinement].Points += 1;
            });

            foreach (var node in new[] { gastronomicBarrier, echoesOfFlavor, feastsFury, naturesEndorsement })
            {
                node.AddChild(ultimateConnoisseur);
            }
        }
    }

    // Command to open the TasteID Skill Tree.
    public class TasteIDSkillTreeCommand
    {
        public static void Initialize()
        {
            CommandSystem.Register("TasteTree", AccessLevel.Player, cmd => ShowTree(cmd.Mobile));
        }

        private static void ShowTree(Mobile m)
        {
            if (m is PlayerMobile pm)
            {
                pm.SendMessage("Opening TasteID Skill Tree...");
                pm.SendGump(new TasteIDSkillTree(pm));
            }
            else
            {
                m.SendMessage("You must be a player to use this command.");
            }
        }
    }
}
