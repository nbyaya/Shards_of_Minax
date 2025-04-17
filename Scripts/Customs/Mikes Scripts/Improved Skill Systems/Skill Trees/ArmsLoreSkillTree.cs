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

namespace Server.ACC.CSS.Systems.ArmsLoreMagic
{
    // Revised Arms Lore Skill Tree Gump using AncientKnowledge (Maxxia Points) for unlocking nodes.
    public class ArmsLoreSkillTree : SuperGump
    {
        private ArmsLoreTree tree;
        private Dictionary<SkillNode, Point2D> nodePositions;
        private Dictionary<int, SkillNode> buttonNodeMap;
        private Dictionary<SkillNode, int> edgeThickness;
        private const int buttonSize = 15;
        private int ySpacing = 50, xSpacing = 60;
        private int rootX = 300, rootY = 150;
        private SkillNode selectedNode;

        public ArmsLoreSkillTree(PlayerMobile user)
            : base(user, null, 100, 100)
        {
            tree = new ArmsLoreTree(user);
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

            // Ensure each node is only placed once.
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

            // Position nodes on each level centered around rootX.
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
            layout.Add("title", () => { AddLabel(100, 20, 1153, "Arms Lore Skill Tree"); });

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

            // New element to display the selected node's description.
            layout.Add("selectedNodeDescription", () =>
            {
                if (selectedNode != null)
                {
                    string descText = $"<BASEFONT COLOR=#FF0000>{selectedNode.Description}</BASEFONT>";
                    // Adjust Y-coordinate as needed (here set to 90) to show below the node name.
                    AddHtml(100, 90, 300, 40, descText, false, false);
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

    // Revised SkillNode that uses AncientKnowledge (Maxxia Points) for unlocking.
    public class SkillNode
    {
        public int BitFlag { get; }
        public string Name { get; }
        public int Cost { get; }
        public string Description { get; }  // New property for node description.
        public List<SkillNode> Children { get; }
        public SkillNode Parent { get; private set; }
        private readonly Action<PlayerMobile> onActivate;

        // Modified constructor accepts a description.
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
            if (!profile.Talents.ContainsKey(TalentID.MartialManualNodes))
                profile.Talents[TalentID.MartialManualNodes] = new Talent(TalentID.MartialManualNodes) { Points = 0 };

            return (profile.Talents[TalentID.MartialManualNodes].Points & BitFlag) != 0;
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
            profile.Talents[TalentID.MartialManualNodes].Points |= BitFlag;

            player.SendMessage($"{Name} activated!");
            onActivate?.Invoke(player);
            return true;
        }
    }

    // Full Arms Lore tree structure with the same layered node structure as your Lumberjacking tree.
    public class ArmsLoreTree
    {
        public SkillNode Root { get; }

        public ArmsLoreTree(PlayerMobile player)
        {
            var profile = player.AcquireTalents();
            int nodeIndex = 0x01;

            // Layer 0: Root Node – Unlocks basic Martial Manual spells.
            Root = new SkillNode(nodeIndex, "Echoes of Battle", 5, "Unlocks basic Martial Manual spells", (p) =>
            {
                profile.Talents[TalentID.MartialManualSpells].Points |= 0x01;
            });

            // Layer 1: Basic bonuses / spell unlocks.
            nodeIndex <<= 1;
            // Changed from passive bonus to spell unlock 0x02.
            var weaponInsight = new SkillNode(nodeIndex, "Weapon Insight", 6, "Unlocks Martial Manual spell (0x02)", (p) =>
            {
                profile.Talents[TalentID.MartialManualSpells].Points |= 0x02;
            });

            nodeIndex <<= 1;
            var armorPerception = new SkillNode(nodeIndex, "Armor Perception", 6, "Passive bonus: Increases defense", (p) =>
            {
                profile.Talents[TalentID.MartialManualDefenseBonus].Points += 1;
            });

            nodeIndex <<= 1;
            var strikeForesight = new SkillNode(nodeIndex, "Strike Foresight", 6, "Unlocks additional Martial Manual spell bits (0x04)", (p) =>
            {
                profile.Talents[TalentID.MartialManualSpells].Points |= 0x04;
            });

            nodeIndex <<= 1;
            var guardedStance = new SkillNode(nodeIndex, "Guarded Stance", 6, "Passive bonus: Increases accuracy", (p) =>
            {
                profile.Talents[TalentID.MartialManualAccuracyBonus].Points += 1;
            });

            // Attach Layer 1 nodes.
            Root.AddChild(weaponInsight);
            Root.AddChild(armorPerception);
            Root.AddChild(strikeForesight);
            Root.AddChild(guardedStance);

            // Layer 2: Intermediate bonuses / spell unlocks.
            nodeIndex <<= 1;
            var keenEdge = new SkillNode(nodeIndex, "Keen Edge", 7, "Unlocks Martial Manual spell (0x08)", (p) =>
            {
                profile.Talents[TalentID.MartialManualSpells].Points |= 0x08;
            });

            nodeIndex <<= 1;
            // Changed from passive bonus to spell unlock 0x800.
            var reinforcedMail = new SkillNode(nodeIndex, "Reinforced Mail", 7, "Unlocks Martial Manual spell (0x800)", (p) =>
            {
                profile.Talents[TalentID.MartialManualSpells].Points |= 0x800;
            });

            nodeIndex <<= 1;
            var swiftRecovery = new SkillNode(nodeIndex, "Swift Recovery", 7, "Passive bonus: Increases speed", (p) =>
            {
                profile.Talents[TalentID.MartialManualSpeedBonus].Points += 1;
            });

            nodeIndex <<= 1;
            var battleFocus = new SkillNode(nodeIndex, "Battle Focus", 7, "Unlocks additional Martial Manual spells (0x10)", (p) =>
            {
                profile.Talents[TalentID.MartialManualSpells].Points |= 0x10;
            });

            // Attach Layer 2 nodes.
            weaponInsight.AddChild(keenEdge);
            armorPerception.AddChild(reinforcedMail);
            strikeForesight.AddChild(swiftRecovery);
            guardedStance.AddChild(battleFocus);

            // Layer 3: Advanced techniques.
            nodeIndex <<= 1;
            // Changed from passive bonus to spell unlock 0x1000.
            var mightySwing = new SkillNode(nodeIndex, "Mighty Swing", 8, "Unlocks Martial Manual spell (0x1000)", (p) =>
            {
                profile.Talents[TalentID.MartialManualSpells].Points |= 0x1000;
            });

            nodeIndex <<= 1;
            var defendersInstinct = new SkillNode(nodeIndex, "Defender's Instinct", 8, "Passive bonus: Increases defense", (p) =>
            {
                profile.Talents[TalentID.MartialManualDefenseBonus].Points += 1;
            });

            nodeIndex <<= 1;
            var ironWill = new SkillNode(nodeIndex, "Iron Will", 8, "Passive bonus: Increases accuracy", (p) =>
            {
                profile.Talents[TalentID.MartialManualAccuracyBonus].Points += 1;
            });

            nodeIndex <<= 1;
            var precisionStrike = new SkillNode(nodeIndex, "Precision Strike", 8, "Unlocks additional spell bits (0x20)", (p) =>
            {
                profile.Talents[TalentID.MartialManualSpells].Points |= 0x20;
            });

            // Attach Layer 3 nodes.
            keenEdge.AddChild(mightySwing);
            reinforcedMail.AddChild(defendersInstinct);
            swiftRecovery.AddChild(ironWill);
            battleFocus.AddChild(precisionStrike);

            // Layer 4: Enhanced abilities.
            nodeIndex <<= 1;
            // Changed from passive bonus to spell unlock 0x2000.
            var furyUnleashed = new SkillNode(nodeIndex, "Fury Unleashed", 9, "Unlocks Martial Manual spell (0x2000)", (p) =>
            {
                profile.Talents[TalentID.MartialManualSpells].Points |= 0x2000;
            });

            nodeIndex <<= 1;
            var stalwartDefense = new SkillNode(nodeIndex, "Stalwart Defense", 9, "Passive bonus: Further increases defense", (p) =>
            {
                profile.Talents[TalentID.MartialManualDefenseBonus].Points += 1;
            });

            nodeIndex <<= 1;
            var rapidReflexes = new SkillNode(nodeIndex, "Rapid Reflexes", 9, "Passive bonus: Further increases speed", (p) =>
            {
                profile.Talents[TalentID.MartialManualSpeedBonus].Points += 1;
            });

            nodeIndex <<= 1;
            var deadlyAccuracy = new SkillNode(nodeIndex, "Deadly Accuracy", 9, "Passive bonus: Further increases accuracy", (p) =>
            {
                profile.Talents[TalentID.MartialManualAccuracyBonus].Points += 1;
            });

            // Attach Layer 4 nodes.
            mightySwing.AddChild(furyUnleashed);
            defendersInstinct.AddChild(stalwartDefense);
            ironWill.AddChild(rapidReflexes);
            precisionStrike.AddChild(deadlyAccuracy);

            // Layer 5: Expert-level nodes.
            nodeIndex <<= 1;
            var savagePower = new SkillNode(nodeIndex, "Savage Power", 10, "Passive bonus: Boosts damage further", (p) =>
            {
                profile.Talents[TalentID.MartialManualDamageBonus].Points += 1;
            });

            nodeIndex <<= 1;
            // Changed from passive bonus to spell unlock 0x4000.
            var unyieldingSpirit = new SkillNode(nodeIndex, "Unyielding Spirit", 10, "Unlocks Martial Manual spell (0x4000)", (p) =>
            {
                profile.Talents[TalentID.MartialManualSpells].Points |= 0x4000;
            });

            nodeIndex <<= 1;
            var flawlessTechnique = new SkillNode(nodeIndex, "Flawless Technique", 10, "Unlocks further Martial Manual spells (0x40)", (p) =>
            {
                profile.Talents[TalentID.MartialManualSpells].Points |= 0x40;
            });

            nodeIndex <<= 1;
            var battleMastery = new SkillNode(nodeIndex, "Battle Mastery", 10, "Passive bonus: Boosts accuracy further", (p) =>
            {
                profile.Talents[TalentID.MartialManualAccuracyBonus].Points += 1;
            });

            // Attach Layer 5 nodes.
            furyUnleashed.AddChild(savagePower);
            stalwartDefense.AddChild(unyieldingSpirit);
            rapidReflexes.AddChild(flawlessTechnique);
            deadlyAccuracy.AddChild(battleMastery);

            // Layer 6: Mastery nodes.
            nodeIndex <<= 1;
            var criticalSurge = new SkillNode(nodeIndex, "Critical Surge", 11, "Passive bonus: Further increases damage", (p) =>
            {
                profile.Talents[TalentID.MartialManualDamageBonus].Points += 1;
            });

            nodeIndex <<= 1;
            var forcefulImpact = new SkillNode(nodeIndex, "Forceful Impact", 11, "Unlocks additional Martial Manual spells (0x80)", (p) =>
            {
                profile.Talents[TalentID.MartialManualSpells].Points |= 0x80;
            });

            nodeIndex <<= 1;
            // Changed from passive bonus to spell unlock 0x8000.
            var resilientArmor = new SkillNode(nodeIndex, "Resilient Armor", 11, "Unlocks Martial Manual spell (0x8000)", (p) =>
            {
                profile.Talents[TalentID.MartialManualSpells].Points |= 0x8000;
            });

            nodeIndex <<= 1;
            var adaptiveTactics = new SkillNode(nodeIndex, "Adaptive Tactics", 11, "Passive bonus: Further increases speed", (p) =>
            {
                profile.Talents[TalentID.MartialManualSpeedBonus].Points += 1;
            });

            // Attach Layer 6 nodes.
            savagePower.AddChild(criticalSurge);
            unyieldingSpirit.AddChild(forcefulImpact);
            battleMastery.AddChild(resilientArmor);
            flawlessTechnique.AddChild(adaptiveTactics);

            // Layer 7: Pinnacle bonuses.
            nodeIndex <<= 1;
            var legendaryMight = new SkillNode(nodeIndex, "Legendary Might", 12, "Passive bonus: Further increases damage", (p) =>
            {
                profile.Talents[TalentID.MartialManualDamageBonus].Points += 1;
            });

            nodeIndex <<= 1;
            var imperviousGuard = new SkillNode(nodeIndex, "Impervious Guard", 12, "Passive bonus: Further increases defense", (p) =>
            {
                profile.Talents[TalentID.MartialManualDefenseBonus].Points += 1;
            });

            nodeIndex <<= 1;
            var surgicalPrecision = new SkillNode(nodeIndex, "Surgical Precision", 12, "Passive bonus: Further increases accuracy", (p) =>
            {
                profile.Talents[TalentID.MartialManualAccuracyBonus].Points += 1;
            });

            nodeIndex <<= 1;
            var unbreakableWill = new SkillNode(nodeIndex, "Unbreakable Will", 12, "Unlocks further Martial Manual spells (0x100)", (p) =>
            {
                profile.Talents[TalentID.MartialManualSpells].Points |= 0x100;
            });

            // Attach Layer 7 nodes.
            criticalSurge.AddChild(legendaryMight);
            forcefulImpact.AddChild(imperviousGuard);
            resilientArmor.AddChild(surgicalPrecision);
            adaptiveTactics.AddChild(unbreakableWill);

            // Layer 8: Ultimate node.
            nodeIndex <<= 1;
            var ultimateWarrior = new SkillNode(nodeIndex, "Ultimate Warrior", 13, "Ultimate bonus: Boosts all Martial Manual spells and passive bonuses", (p) =>
            {
                profile.Talents[TalentID.MartialManualSpells].Points |= 0x200 | 0x400;
                profile.Talents[TalentID.MartialManualDamageBonus].Points += 1;
                profile.Talents[TalentID.MartialManualDefenseBonus].Points += 1;
                profile.Talents[TalentID.MartialManualAccuracyBonus].Points += 1;
                profile.Talents[TalentID.MartialManualSpeedBonus].Points += 1;
            });

            // Attach the ultimate node to all Layer 7 nodes.
            legendaryMight.AddChild(ultimateWarrior);
            imperviousGuard.AddChild(ultimateWarrior);
            surgicalPrecision.AddChild(ultimateWarrior);
            unbreakableWill.AddChild(ultimateWarrior);
        }
    }

    // Command to open the Arms Lore Skill Tree.
    public class ArmsLoreSkillTreeCommand
    {
        public static void Initialize()
        {
            CommandSystem.Register("ArmsTree", AccessLevel.Player, cmd => ShowTree(cmd.Mobile));
        }

        private static void ShowTree(Mobile m)
        {
            if (m is PlayerMobile pm)
            {
                pm.SendMessage("Opening Arms Lore Skill Tree...");
                pm.SendGump(new ArmsLoreSkillTree(pm));
            }
            else
            {
                m.SendMessage("You must be a player to use this command.");
            }
        }
    }
}
