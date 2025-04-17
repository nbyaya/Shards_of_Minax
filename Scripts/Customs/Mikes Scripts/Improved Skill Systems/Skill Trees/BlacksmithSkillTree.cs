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

namespace Server.ACC.CSS.Systems.BlacksmithMagic
{
    // Revised Blacksmith Skill Tree Gump using AncientKnowledge (Maxxia Points) for cost.
    public class BlacksmithSkillTree : SuperGump
    {
        private BlacksmithTree tree;
        private Dictionary<SkillNode, Point2D> nodePositions;
        private Dictionary<int, SkillNode> buttonNodeMap;
        private Dictionary<SkillNode, int> edgeThickness;
        private const int buttonSize = 15;
        private int ySpacing = 50, xSpacing = 60;
        private int rootX = 300, rootY = 150;
        private SkillNode selectedNode;

        public BlacksmithSkillTree(PlayerMobile user)
            : base(user, null, 100, 100)
        {
            tree = new BlacksmithTree(user);
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
            layout.Add("title", () => { AddLabel(100, 20, 1153, "Blacksmith Skill Tree"); });

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

    // A generic SkillNode used by both systems.
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
            if (!profile.Talents.ContainsKey(TalentID.BlacksmithNodes))
                profile.Talents[TalentID.BlacksmithNodes] = new Talent(TalentID.BlacksmithNodes) { Points = 0 };

            return (profile.Talents[TalentID.BlacksmithNodes].Points & BitFlag) != 0;
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

            // Use AncientKnowledge (Maxxia Points) for unlocking.
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
            profile.Talents[TalentID.BlacksmithNodes].Points |= BitFlag;

            player.SendMessage($"{Name} activated!");
            onActivate?.Invoke(player);
            return true;
        }
    }

    // The full Blacksmith tree structure with multiple layers and 30 nodes.
    public class BlacksmithTree
    {
        public SkillNode Root { get; }

        public BlacksmithTree(PlayerMobile player)
        {
            var profile = player.AcquireTalents();
            int nodeIndex = 0x01;

            // Layer 0: Root Node – Unlocks basic blacksmith spells.
            Root = new SkillNode(nodeIndex, "Hammer of the Forge", 5, "Unlocks spell: Hammer of the Forge", (p) =>
            {
                profile.Talents[TalentID.BlacksmithSpells].Points |= 0x01;
            });

            // Layer 1: Four nodes become spell unlocks.
            nodeIndex <<= 1; // 0x02
            var flameInitiation = new SkillNode(nodeIndex, "Flame Initiation", 6, "Unlocks spell: Flame Initiation", (p) =>
            {
                profile.Talents[TalentID.BlacksmithSpells].Points |= 0x02;
            });

            nodeIndex <<= 1; // 0x04
            var metalSense = new SkillNode(nodeIndex, "Metal Sense", 6, "Unlocks spell: Metal Sense", (p) =>
            {
                profile.Talents[TalentID.BlacksmithSpells].Points |= 0x04;
            });

            nodeIndex <<= 1; // 0x08
            var efficientForging = new SkillNode(nodeIndex, "Efficient Forging", 6, "Unlocks spell: Efficient Forging", (p) =>
            {
                profile.Talents[TalentID.BlacksmithSpells].Points |= 0x08;
            });

            nodeIndex <<= 1; // 0x10
            var anvilMastery = new SkillNode(nodeIndex, "Anvil Mastery", 6, "Unlocks spell: Anvil Mastery", (p) =>
            {
                profile.Talents[TalentID.BlacksmithSpells].Points |= 0x10;
            });

            Root.AddChild(flameInitiation);
            Root.AddChild(metalSense);
            Root.AddChild(efficientForging);
            Root.AddChild(anvilMastery);

            // Layer 2: Next four nodes become spell unlocks.
            nodeIndex <<= 1; // 0x20
            var sparkOfCreation = new SkillNode(nodeIndex, "Spark of Creation", 7, "Unlocks spell: Spark of Creation", (p) =>
            {
                profile.Talents[TalentID.BlacksmithSpells].Points |= 0x20;
            });

            nodeIndex <<= 1; // 0x40
            var ironWill = new SkillNode(nodeIndex, "Iron Will", 7, "Unlocks spell: Iron Will", (p) =>
            {
                profile.Talents[TalentID.BlacksmithSpells].Points |= 0x40;
            });

            nodeIndex <<= 1; // 0x80
            var moltenMight = new SkillNode(nodeIndex, "Molten Might", 7, "Unlocks spell: Molten Might", (p) =>
            {
                profile.Talents[TalentID.BlacksmithSpells].Points |= 0x80;
            });

            nodeIndex <<= 1; // 0x100
            var steelResolve = new SkillNode(nodeIndex, "Steel Resolve", 7, "Unlocks spell: Steel Resolve", (p) =>
            {
                profile.Talents[TalentID.BlacksmithSpells].Points |= 0x100;
            });

            flameInitiation.AddChild(sparkOfCreation);
            metalSense.AddChild(ironWill);
            efficientForging.AddChild(moltenMight);
            anvilMastery.AddChild(steelResolve);

            // Layer 3: Next four nodes become spell unlocks.
            nodeIndex <<= 1; // 0x200
            var burningPrecision = new SkillNode(nodeIndex, "Burning Precision", 8, "Unlocks spell: Burning Precision", (p) =>
            {
                profile.Talents[TalentID.BlacksmithSpells].Points |= 0x200;
            });

            nodeIndex <<= 1; // 0x400
            var refinedHeat = new SkillNode(nodeIndex, "Refined Heat", 8, "Unlocks spell: Refined Heat", (p) =>
            {
                profile.Talents[TalentID.BlacksmithSpells].Points |= 0x400;
            });

            nodeIndex <<= 1; // 0x800
            var temperedEdge = new SkillNode(nodeIndex, "Tempered Edge", 8, "Unlocks spell: Tempered Edge", (p) =>
            {
                profile.Talents[TalentID.BlacksmithSpells].Points |= 0x800;
            });

            nodeIndex <<= 1; // 0x1000
            var resilientMetal = new SkillNode(nodeIndex, "Resilient Metal", 8, "Unlocks spell: Resilient Metal", (p) =>
            {
                profile.Talents[TalentID.BlacksmithSpells].Points |= 0x1000;
            });

            sparkOfCreation.AddChild(burningPrecision);
            ironWill.AddChild(refinedHeat);
            moltenMight.AddChild(temperedEdge);
            steelResolve.AddChild(resilientMetal);

            // Layer 4: In this layer the first three nodes become spell unlocks.
            nodeIndex <<= 1; // 0x2000
            var blazingArtistry = new SkillNode(nodeIndex, "Blazing Artistry", 9, "Unlocks spell: Blazing Artistry", (p) =>
            {
                profile.Talents[TalentID.BlacksmithSpells].Points |= 0x2000;
            });

            nodeIndex <<= 1; // 0x4000
            var infernoStrength = new SkillNode(nodeIndex, "Inferno Strength", 9, "Unlocks spell: Inferno Strength", (p) =>
            {
                profile.Talents[TalentID.BlacksmithSpells].Points |= 0x4000;
            });

            nodeIndex <<= 1; // 0x8000
            var radiantForge = new SkillNode(nodeIndex, "Radiant Forge", 9, "Unlocks spell: Radiant Forge", (p) =>
            {
                profile.Talents[TalentID.BlacksmithSpells].Points |= 0x8000;
            });

            // The remaining node in Layer 4 stays as a bonus.
            nodeIndex <<= 1; // 0x10000
            var perfectedMetals = new SkillNode(nodeIndex, "Perfected Metals", 9, "Enhances quality of finished items", (p) =>
            {
                profile.Talents[TalentID.BlacksmithQuality].Points += 1;
            });

            burningPrecision.AddChild(blazingArtistry);
            refinedHeat.AddChild(infernoStrength);
            temperedEdge.AddChild(radiantForge);
            resilientMetal.AddChild(perfectedMetals);

            // Layer 5: Expert-level nodes (passive bonuses).
            nodeIndex <<= 1; // 0x20000
            var primeForge = new SkillNode(nodeIndex, "Prime Forge", 10, "Boosts overall forging efficiency", (p) =>
            {
                profile.Talents[TalentID.BlacksmithEfficiency].Points += 1;
            });

            nodeIndex <<= 1; // 0x40000
            var masterHammer = new SkillNode(nodeIndex, "Master Hammer", 10, "Improves weapon strength", (p) =>
            {
                profile.Talents[TalentID.BlacksmithStrength].Points += 1;
            });

            nodeIndex <<= 1; // 0x80000
            var arcaneAnvil = new SkillNode(nodeIndex, "Arcane Anvil", 10, "Enhances forging bonuses", (p) =>
            {
                profile.Talents[TalentID.BlacksmithEfficiency].Points += 1;
            });

            nodeIndex <<= 1; // 0x100000
            var rapidQuench = new SkillNode(nodeIndex, "Rapid Quench", 10, "Increases forging speed", (p) =>
            {
                profile.Talents[TalentID.BlacksmithEfficiency].Points += 1;
            });

            blazingArtistry.AddChild(primeForge);
            infernoStrength.AddChild(masterHammer);
            radiantForge.AddChild(arcaneAnvil);
            perfectedMetals.AddChild(rapidQuench);

            // Layer 6: Mastery nodes (passive bonuses).
            nodeIndex <<= 1; // 0x200000
            var focusedFlame = new SkillNode(nodeIndex, "Focused Flame", 11, "Enhances control over forge heat", (p) =>
            {
                // Bonus effect implemented elsewhere.
            });

            nodeIndex <<= 1; // 0x400000
            var superiorMetallurgy = new SkillNode(nodeIndex, "Superior Metallurgy", 11, "Improves quality of forged items", (p) =>
            {
                profile.Talents[TalentID.BlacksmithQuality].Points += 1;
            });

            nodeIndex <<= 1; // 0x800000
            var mysticalSmithing = new SkillNode(nodeIndex, "Mystical Smithing", 11, "Enhances forging bonuses", (p) =>
            {
                profile.Talents[TalentID.BlacksmithEfficiency].Points += 1;
            });

            nodeIndex <<= 1; // 0x1000000
            var swiftStrike = new SkillNode(nodeIndex, "Swift Strike", 11, "Improves the speed of weapon creation", (p) =>
            {
                profile.Talents[TalentID.BlacksmithEfficiency].Points += 1;
            });

            primeForge.AddChild(focusedFlame);
            masterHammer.AddChild(superiorMetallurgy);
            arcaneAnvil.AddChild(mysticalSmithing);
            rapidQuench.AddChild(swiftStrike);

            // Layer 7: Final, pinnacle bonuses (passive bonuses).
            nodeIndex <<= 1; // 0x2000000
            var adamantBarrier = new SkillNode(nodeIndex, "Adamant Barrier", 12, "Provides a protective forging bonus", (p) =>
            {
                profile.Talents[TalentID.BlacksmithStrength].Points += 1;
            });

            nodeIndex <<= 1; // 0x4000000
            var legendaryQuality = new SkillNode(nodeIndex, "Legendary Quality", 12, "Greatly improves item quality", (p) =>
            {
                profile.Talents[TalentID.BlacksmithQuality].Points += 1;
            });

            nodeIndex <<= 1; // 0x8000000
            var forgingFury = new SkillNode(nodeIndex, "Forging Fury", 12, "Boosts forging power dramatically", (p) =>
            {
                profile.Talents[TalentID.BlacksmithStrength].Points += 1;
            });

            nodeIndex <<= 1; // 0x10000000
            var relentlessHammer = new SkillNode(nodeIndex, "Relentless Hammer", 12, "Enhances overall forging speed", (p) =>
            {
                profile.Talents[TalentID.BlacksmithEfficiency].Points += 1;
            });

            focusedFlame.AddChild(adamantBarrier);
            superiorMetallurgy.AddChild(legendaryQuality);
            mysticalSmithing.AddChild(forgingFury);
            swiftStrike.AddChild(relentlessHammer);

            // Layer 8: Ultimate node (passive bonus).
            nodeIndex <<= 1; // 0x20000000
            var ultimateSmith = new SkillNode(nodeIndex, "Ultimate Smith", 13, "Ultimate bonus: boosts all blacksmith skills", (p) =>
            {
                profile.Talents[TalentID.BlacksmithEfficiency].Points += 1;
                profile.Talents[TalentID.BlacksmithStrength].Points += 1;
                profile.Talents[TalentID.BlacksmithQuality].Points += 1;
            });

            // Attach the ultimate node to all Layer 7 nodes.
            foreach (var node in new[] { adamantBarrier, relentlessHammer, forgingFury, legendaryQuality })
            {
                node.AddChild(ultimateSmith);
            }
        }
    }

    // Command to open the Blacksmith Skill Tree.
    public class BlacksmithSkillTreeCommand
    {
        public static void Initialize()
        {
            CommandSystem.Register("SmithTree", AccessLevel.Player, cmd => ShowTree(cmd.Mobile));
        }

        private static void ShowTree(Mobile m)
        {
            if (m is PlayerMobile pm)
            {
                pm.SendMessage("Opening Blacksmith Skill Tree...");
                pm.SendGump(new BlacksmithSkillTree(pm));
            }
            else
            {
                m.SendMessage("You must be a player to use this command.");
            }
        }
    }
}
