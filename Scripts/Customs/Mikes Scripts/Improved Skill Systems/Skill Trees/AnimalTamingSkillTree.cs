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

namespace Server.ACC.CSS.Systems.AnimalTamingMagic
{
    // Revised Animal Taming Skill Tree Gump using AncientKnowledge as the cost resource.
    public class AnimalTamingSkillTree : SuperGump
    {
        private AnimalTamingTree tree;
        private Dictionary<SkillNode, Point2D> nodePositions;
        private Dictionary<int, SkillNode> buttonNodeMap;
        private Dictionary<SkillNode, int> edgeThickness;
        private const int buttonSize = 15;
        private int ySpacing = 50, xSpacing = 60;
        private int rootX = 300, rootY = 150;
        private SkillNode selectedNode;

        public AnimalTamingSkillTree(PlayerMobile user)
            : base(user, null, 100, 100)
        {
            tree = new AnimalTamingTree(user);
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

            // Center each level’s nodes around rootX.
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
            layout.Add("title", () => { AddLabel(100, 20, 1153, "Animal Taming Skill Tree"); });

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

            // Display the node's description.
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

    // Revised SkillNode using AncientKnowledge (Maxxia Points) for costs.
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
            if (!profile.Talents.ContainsKey(TalentID.AnimalTamingNodes))
                profile.Talents[TalentID.AnimalTamingNodes] = new Talent(TalentID.AnimalTamingNodes) { Points = 0 };
            return (profile.Talents[TalentID.AnimalTamingNodes].Points & BitFlag) != 0;
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
            profile.Talents[TalentID.AnimalTamingNodes].Points |= BitFlag;
            player.SendMessage($"{Name} activated!");
            onActivate?.Invoke(player);
            return true;
        }
    }

    // Full Animal Taming tree structure with multiple layers and 30 nodes.
    public class AnimalTamingTree
    {
        public SkillNode Root { get; }

        public AnimalTamingTree(PlayerMobile player)
        {
            var profile = player.AcquireTalents();
            int nodeIndex = 0x01;

            // Layer 0: Root Node – Unlocks basic taming spells.
            Root = new SkillNode(nodeIndex, "Call of the Wild", 5, "Unlocks basic animal taming spells", (p) =>
            {
                // Unlock basic taming spells.
                profile.Talents[TalentID.AnimalTamingSpells].Points |= 0x01;
            });

            // Layer 1: Basic bonuses.
            nodeIndex <<= 1;
            var beastBond = new SkillNode(nodeIndex, "Beast Bond", 6, "Unlocks an additional taming spell", (p) =>
            {
                // Unlock spell 0x02.
                profile.Talents[TalentID.AnimalTamingSpells].Points |= 0x02;
            });

            nodeIndex <<= 1;
            var feralInsight = new SkillNode(nodeIndex, "Feral Insight", 6, "Improves taming success chance", (p) =>
            {
                // Increase taming chance.
                profile.Talents[TalentID.MinionDamageBonus].Points += 1;
            });

            nodeIndex <<= 1;
            var predatoryProwess = new SkillNode(nodeIndex, "Predatory Prowess", 6, "Unlocks bonus taming spells", (p) =>
            {
                // Unlock spell 0x04.
                profile.Talents[TalentID.AnimalTamingSpells].Points |= 0x04;
            });

            nodeIndex <<= 1;
            var naturalAffinity = new SkillNode(nodeIndex, "Natural Affinity", 6, "Passively boosts pet affinity", (p) =>
            {
                // Increase pet bonding.
                profile.Talents[TalentID.MinionDamageBonus].Points += 1;
            });

            Root.AddChild(beastBond);
            Root.AddChild(feralInsight);
            Root.AddChild(predatoryProwess);
            Root.AddChild(naturalAffinity);

            // Layer 2: Advanced bonuses.
            nodeIndex <<= 1;
            var packInstinct = new SkillNode(nodeIndex, "Pack Instinct", 7, "Unlocks an additional taming spell", (p) =>
            {
                // Unlock spell 0x200.
                profile.Talents[TalentID.AnimalTamingSpells].Points |= 0x200;
            });

            nodeIndex <<= 1;
            var huntersGuile = new SkillNode(nodeIndex, "Hunter's Guile", 7, "Passively increases taming chance", (p) =>
            {
                profile.Talents[TalentID.MinionDamageBonus].Points += 1;
            });

            nodeIndex <<= 1;
            var wildCall = new SkillNode(nodeIndex, "Wild Call", 7, "Unlocks additional taming spells", (p) =>
            {
                // Unlock spell 0x08.
                profile.Talents[TalentID.AnimalTamingSpells].Points |= 0x08;
            });

            nodeIndex <<= 1;
            var savageTactics = new SkillNode(nodeIndex, "Savage Tactics", 7, "Improves pet combat bonuses", (p) =>
            {
                profile.Talents[TalentID.MinionDamageBonus].Points += 1;
            });

            beastBond.AddChild(packInstinct);
            feralInsight.AddChild(huntersGuile);
            predatoryProwess.AddChild(wildCall);
            naturalAffinity.AddChild(savageTactics);

            // Layer 3: Further passive enhancements.
            nodeIndex <<= 1;
            var feralFortitude = new SkillNode(nodeIndex, "Feral Fortitude", 8, "Increases pet stamina", (p) =>
            {
                profile.Talents[TalentID.MinionDamageBonus].Points += 1;
            });

            nodeIndex <<= 1;
            var beastMastery = new SkillNode(nodeIndex, "Beast Mastery", 8, "Unlocks bonus taming spells", (p) =>
            {
                // Unlock spell 0x10.
                profile.Talents[TalentID.AnimalTamingSpells].Points |= 0x10;
            });

            nodeIndex <<= 1;
            var wildAgility = new SkillNode(nodeIndex, "Wild Agility", 8, "Unlocks an additional taming spell", (p) =>
            {
                // Unlock spell 0x400.
                profile.Talents[TalentID.AnimalTamingSpells].Points |= 0x400;
            });

            nodeIndex <<= 1;
            var primalBond = new SkillNode(nodeIndex, "Primal Bond", 8, "Strengthens the bond between you and your pet", (p) =>
            {
                profile.Talents[TalentID.MinionDamageBonus].Points += 1;
            });

            packInstinct.AddChild(feralFortitude);
            huntersGuile.AddChild(beastMastery);
            wildCall.AddChild(wildAgility);
            savageTactics.AddChild(primalBond);

            // Layer 4: More advanced magical enhancements.
            nodeIndex <<= 1;
            var alphasCommand = new SkillNode(nodeIndex, "Alpha's Command", 9, "Passively increases pet obedience", (p) =>
            {
                profile.Talents[TalentID.MinionDamageBonus].Points += 1;
            });

            nodeIndex <<= 1;
            var naturesRoar = new SkillNode(nodeIndex, "Nature's Roar", 9, "Unlocks superior taming spells", (p) =>
            {
                // Unlock spell 0x20.
                profile.Talents[TalentID.AnimalTamingSpells].Points |= 0x20;
            });

            nodeIndex <<= 1;
            var untamedSpirit = new SkillNode(nodeIndex, "Untamed Spirit", 9, "Unlocks an additional taming spell", (p) =>
            {
                // Unlock spell 0x2000.
                profile.Talents[TalentID.AnimalTamingSpells].Points |= 0x2000;
            });

            nodeIndex <<= 1;
            var predatoryInstinct = new SkillNode(nodeIndex, "Predatory Instinct", 9, "Further improves taming chance", (p) =>
            {
                profile.Talents[TalentID.MinionDamageBonus].Points += 1;
            });

            feralFortitude.AddChild(alphasCommand);
            beastMastery.AddChild(naturesRoar);
            wildAgility.AddChild(untamedSpirit);
            primalBond.AddChild(predatoryInstinct);

            // Layer 5: Expert-level nodes.
            nodeIndex <<= 1;
            var swiftPursuit = new SkillNode(nodeIndex, "Swift Pursuit", 10, "Passively increases pet speed", (p) =>
            {
                profile.Talents[TalentID.MinionDamageBonus].Points += 1;
            });

            nodeIndex <<= 1;
            var enduringHeart = new SkillNode(nodeIndex, "Enduring Heart", 10, "Unlocks an additional taming spell", (p) =>
            {
                // Unlock spell 0x4000.
                profile.Talents[TalentID.AnimalTamingSpells].Points |= 0x4000;
            });

            nodeIndex <<= 1;
            var unyieldingWill = new SkillNode(nodeIndex, "Unyielding Will", 10, "Unlocks advanced taming spells", (p) =>
            {
                // Unlock spell 0x40.
                profile.Talents[TalentID.AnimalTamingSpells].Points |= 0x40;
            });

            nodeIndex <<= 1;
            var cunningManeuver = new SkillNode(nodeIndex, "Cunning Maneuver", 10, "Improves tactical pet control", (p) =>
            {
                profile.Talents[TalentID.MinionDamageBonus].Points += 1;
            });

            alphasCommand.AddChild(swiftPursuit);
            naturesRoar.AddChild(enduringHeart);
            untamedSpirit.AddChild(unyieldingWill);
            predatoryInstinct.AddChild(cunningManeuver);

            // Layer 6: Mastery nodes.
            nodeIndex <<= 1;
            var mastersGrace = new SkillNode(nodeIndex, "Master's Grace", 11, "Unlocks bonus taming spells", (p) =>
            {
                // Unlock spell 0x80.
                profile.Talents[TalentID.AnimalTamingSpells].Points |= 0x80;
            });

            nodeIndex <<= 1;
            var primalAwareness = new SkillNode(nodeIndex, "Primal Awareness", 11, "Unlocks an additional taming spell", (p) =>
            {
                // Unlock spell 0x8000.
                profile.Talents[TalentID.AnimalTamingSpells].Points |= 0x8000;
            });

            nodeIndex <<= 1;
            var ferociousStrength = new SkillNode(nodeIndex, "Ferocious Strength", 11, "Passively increases pet combat strength", (p) =>
            {
                profile.Talents[TalentID.MinionDamageBonus].Points += 1;
            });

            nodeIndex <<= 1;
            var inexorableLoyalty = new SkillNode(nodeIndex, "Inexorable Loyalty", 11, "Deepens the bond with your pet", (p) =>
            {
                profile.Talents[TalentID.MinionDamageBonus].Points += 1;
            });

            swiftPursuit.AddChild(mastersGrace);
            enduringHeart.AddChild(primalAwareness);
            unyieldingWill.AddChild(ferociousStrength);
            cunningManeuver.AddChild(inexorableLoyalty);

            // Layer 7: Final, pinnacle bonuses.
            nodeIndex <<= 1;
            var roaringMight = new SkillNode(nodeIndex, "Roaring Might", 12, "Significantly increases pet power", (p) =>
            {
                profile.Talents[TalentID.MinionDamageBonus].Points += 1;
            });

            nodeIndex <<= 1;
            var primalFury = new SkillNode(nodeIndex, "Primal Fury", 12, "Unlocks elite taming spells", (p) =>
            {
                // Unlock spell 0x100.
                profile.Talents[TalentID.AnimalTamingSpells].Points |= 0x100;
            });

            nodeIndex <<= 1;
            var guardiansPresence = new SkillNode(nodeIndex, "Guardian's Presence", 12, "Passively protects your pet", (p) =>
            {
                profile.Talents[TalentID.MinionDamageBonus].Points += 1;
            });

            nodeIndex <<= 1;
            var sovereignInstinct = new SkillNode(nodeIndex, "Sovereign's Instinct", 12, "Enhances overall taming instincts", (p) =>
            {
                profile.Talents[TalentID.MinionDamageBonus].Points += 1;
            });

            mastersGrace.AddChild(roaringMight);
            primalAwareness.AddChild(primalFury);
            ferociousStrength.AddChild(guardiansPresence);
            inexorableLoyalty.AddChild(sovereignInstinct);

            // Layer 8: Ultimate node.
            nodeIndex <<= 1;
            var ultimateBeastmaster = new SkillNode(nodeIndex, "Ultimate Beastmaster", 13, "Ultimate bonus: boosts all animal taming abilities", (p) =>
            {
                // Grants final bonuses to spells and all passive abilities.
                profile.Talents[TalentID.AnimalTamingSpells].Points |= 0x800 | 0x1000;
                profile.Talents[TalentID.MinionDamageBonus].Points += 1;
                profile.Talents[TalentID.MinionDamageBonus].Points += 1;
                profile.Talents[TalentID.MinionDamageBonus].Points += 1;
                profile.Talents[TalentID.MinionDamageBonus].Points += 1;
            });

            // Attach the ultimate node to all Layer 7 nodes.
            foreach (var node in new[] { roaringMight, sovereignInstinct, primalFury, guardiansPresence })
            {
                node.AddChild(ultimateBeastmaster);
            }
        }
    }

    // Command to open the Animal Taming Skill Tree.
    public class AnimalTamingSkillTreeCommand
    {
        public static void Initialize()
        {
            CommandSystem.Register("AnimalTamingTree", AccessLevel.Player, cmd => ShowTree(cmd.Mobile));
        }

        private static void ShowTree(Mobile m)
        {
            if (m is PlayerMobile pm)
            {
                pm.SendMessage("Opening Animal Taming Skill Tree...");
                pm.SendGump(new AnimalTamingSkillTree(pm));
            }
            else
            {
                m.SendMessage("You must be a player to use this command.");
            }
        }
    }
}
