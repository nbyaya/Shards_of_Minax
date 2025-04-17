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

namespace Server.ACC.CSS.Systems.MacingMagic
{
    // Revised Macing Skill Tree Gump using Maxxia Points for cost.
    public class MacingSkillTree : SuperGump
    {
        private MacingTree tree;
        private Dictionary<SkillNode, Point2D> nodePositions;
        private Dictionary<int, SkillNode> buttonNodeMap;
        private Dictionary<SkillNode, int> edgeThickness;
        private const int buttonSize = 15;
        private int ySpacing = 50, xSpacing = 60;
        private int rootX = 300, rootY = 150;
        private SkillNode selectedNode;

        public MacingSkillTree(PlayerMobile user)
            : base(user, null, 100, 100)
        {
            tree = new MacingTree(user);
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
            layout.Add("title", () => { AddLabel(100, 20, 1153, "Macing Skill Tree"); });

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

    // Revised SkillNode (identical in structure to the Lumberjacking version)
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
            if (!profile.Talents.ContainsKey(TalentID.MacingNodes))
                profile.Talents[TalentID.MacingNodes] = new Talent(TalentID.MacingNodes) { Points = 0 };

            return (profile.Talents[TalentID.MacingNodes].Points & BitFlag) != 0;
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
            profile.Talents[TalentID.MacingNodes].Points |= BitFlag;

            player.SendMessage($"{Name} activated!");
            onActivate?.Invoke(player);
            return true;
        }
    }

    // MacingTree builds the full 30-node tree with nine layers.
    public class MacingTree
    {
        public SkillNode Root { get; }

        public MacingTree(PlayerMobile player)
        {
            var profile = player.AcquireTalents();
            int nodeIndex = 0x01;

            // Layer 0: Root Node – Unlocks basic macing spells.
            Root = new SkillNode(nodeIndex, "Call of the Strike", 5, "Unlocks basic macing spells", (p) =>
            {
                profile.Talents[TalentID.MacingSpells].Points |= 0x01; // Spell bit 0x01
            });

            // Layer 1: Basic nodes.
            nodeIndex <<= 1;
            // Converted to spell unlock: Mace Grip now unlocks a spell.
            var maceGrip = new SkillNode(nodeIndex, "Mace Grip", 6, "Unlocks a macing spell", (p) =>
            {
                profile.Talents[TalentID.MacingSpells].Points |= 0x02; // Spell bit 0x02
            });

            nodeIndex <<= 1;
            // Swift Swing remains a bonus.
            var swiftSwing = new SkillNode(nodeIndex, "Swift Swing", 6, "Increases attack speed", (p) =>
            {
                profile.Talents[TalentID.MacingSpeedBonus].Points += 1;
            });

            nodeIndex <<= 1;
            // Balanced Swing remains as a spell unlock.
            var balancedSwing = new SkillNode(nodeIndex, "Balanced Swing", 6, "Improves accuracy and control, unlocking additional spells", (p) =>
            {
                profile.Talents[TalentID.MacingSpells].Points |= 0x04; // Spell bit 0x04
            });

            nodeIndex <<= 1;
            // Heavy Swing remains a bonus.
            var heavySwing = new SkillNode(nodeIndex, "Heavy Swing", 6, "Increases damage output", (p) =>
            {
                profile.Talents[TalentID.MacingDamageBonus].Points += 1;
            });

            Root.AddChild(maceGrip);
            Root.AddChild(swiftSwing);
            Root.AddChild(balancedSwing);
            Root.AddChild(heavySwing);

            // Layer 2: Advanced nodes.
            nodeIndex <<= 1;
            // Crushing Blow remains as a spell unlock.
            var crushingBlow = new SkillNode(nodeIndex, "Crushing Blow", 7, "Unlocks advanced macing spells", (p) =>
            {
                profile.Talents[TalentID.MacingSpells].Points |= 0x08; // Spell bit 0x08
            });

            nodeIndex <<= 1;
            // Momentum Strike remains a bonus.
            var momentumStrike = new SkillNode(nodeIndex, "Momentum Strike", 7, "Enhances critical hit chance", (p) =>
            {
                profile.Talents[TalentID.MacingCriticalBonus].Points += 1;
            });

            nodeIndex <<= 1;
            // Converted to spell unlock: Armor Shatter now unlocks a spell.
            var armorShatter = new SkillNode(nodeIndex, "Armor Shatter", 7, "Unlocks a macing spell", (p) =>
            {
                profile.Talents[TalentID.MacingSpells].Points |= 0x10; // Spell bit 0x10
            });

            nodeIndex <<= 1;
            // Iron Will remains a bonus.
            var ironWill = new SkillNode(nodeIndex, "Iron Will", 7, "Boosts physical defense", (p) =>
            {
                profile.Talents[TalentID.MacingDefenseBonus].Points += 1;
            });

            maceGrip.AddChild(crushingBlow);
            swiftSwing.AddChild(momentumStrike);
            balancedSwing.AddChild(armorShatter);
            heavySwing.AddChild(ironWill);

            // Layer 3: Further nodes.
            nodeIndex <<= 1;
            // Converted: Blunt Mastery now uses spell bit 0x20.
            var bluntMastery = new SkillNode(nodeIndex, "Blunt Mastery", 8, "Unlocks a macing spell", (p) =>
            {
                profile.Talents[TalentID.MacingSpells].Points |= 0x20; // Spell bit 0x20
            });

            nodeIndex <<= 1;
            // Spiritual Impact remains a bonus.
            var spiritualImpact = new SkillNode(nodeIndex, "Spiritual Impact", 8, "Enhances chance to stun opponents", (p) =>
            {
                profile.Talents[TalentID.MacingStunChance].Points += 1;
            });

            nodeIndex <<= 1;
            // Converted: Echoing Strike now unlocks a spell.
            var echoingStrike = new SkillNode(nodeIndex, "Echoing Strike", 8, "Unlocks a macing spell", (p) =>
            {
                profile.Talents[TalentID.MacingSpells].Points |= 0x40; // Spell bit 0x40
            });

            nodeIndex <<= 1;
            // Steel Resolve remains a bonus.
            var steelResolve = new SkillNode(nodeIndex, "Steel Resolve", 8, "Improves damage mitigation", (p) =>
            {
                profile.Talents[TalentID.MacingDamageReduction].Points += 1;
            });

            crushingBlow.AddChild(bluntMastery);
            momentumStrike.AddChild(spiritualImpact);
            armorShatter.AddChild(echoingStrike);
            ironWill.AddChild(steelResolve);

            // Layer 4: Magical enhancements.
            nodeIndex <<= 1;
            // Converted: Guardian's Ward now uses spell bit 0x80.
            var guardiansWard = new SkillNode(nodeIndex, "Guardian's Ward", 9, "Unlocks defensive macing spells", (p) =>
            {
                profile.Talents[TalentID.MacingSpells].Points |= 0x80; // Spell bit 0x80
            });

            nodeIndex <<= 1;
            // Brutal Force remains a bonus.
            var brutalForce = new SkillNode(nodeIndex, "Brutal Force", 9, "Further increases damage", (p) =>
            {
                profile.Talents[TalentID.MacingDamageBonus].Points += 1;
            });

            nodeIndex <<= 1;
            // Resolute Stance remains a bonus.
            var resoluteStance = new SkillNode(nodeIndex, "Resolute Stance", 9, "Improves parry and blocking", (p) =>
            {
                profile.Talents[TalentID.MacingParryBonus].Points += 1;
            });

            nodeIndex <<= 1;
            // Converted: Unyielding Might now uses spell bit 0x100.
            var unyieldingMight = new SkillNode(nodeIndex, "Unyielding Might", 9, "Unlocks a macing spell", (p) =>
            {
                profile.Talents[TalentID.MacingSpells].Points |= 0x100; // Spell bit 0x100
            });

            bluntMastery.AddChild(guardiansWard);
            spiritualImpact.AddChild(brutalForce);
            echoingStrike.AddChild(resoluteStance);
            steelResolve.AddChild(unyieldingMight);

            // Layer 5: Expert-level nodes.
            nodeIndex <<= 1;
            // Converted: Primeval Fury now uses spell bit 0x200.
            var primevalFury = new SkillNode(nodeIndex, "Primeval Fury", 10, "Unlocks a macing spell", (p) =>
            {
                profile.Talents[TalentID.MacingSpells].Points |= 0x200; // Spell bit 0x200
            });

            nodeIndex <<= 1;
            // Savage Momentum remains a bonus.
            var savageMomentum = new SkillNode(nodeIndex, "Savage Momentum", 10, "Enhances attack speed further", (p) =>
            {
                profile.Talents[TalentID.MacingSpeedBonus].Points += 1;
            });

            nodeIndex <<= 1;
            // Converted: Concussive Impact now uses spell bit 0x400.
            var concussiveImpact = new SkillNode(nodeIndex, "Concussive Impact", 10, "Unlocks a macing spell", (p) =>
            {
                profile.Talents[TalentID.MacingSpells].Points |= 0x400; // Spell bit 0x400
            });

            nodeIndex <<= 1;
            // Mighty Strike remains a bonus.
            var mightyStrike = new SkillNode(nodeIndex, "Mighty Strike", 10, "Increases damage significantly", (p) =>
            {
                profile.Talents[TalentID.MacingDamageBonus].Points += 1;
            });

            guardiansWard.AddChild(primevalFury);
            brutalForce.AddChild(savageMomentum);
            resoluteStance.AddChild(concussiveImpact);
            unyieldingMight.AddChild(mightyStrike);

            // Layer 6: Mastery nodes.
            nodeIndex <<= 1;
            // Expanded Reach remains a bonus.
            var expandedReach = new SkillNode(nodeIndex, "Expanded Reach", 11, "Increases attack range", (p) =>
            {
                profile.Talents[TalentID.MacingRangeBonus].Points += 1;
            });

            nodeIndex <<= 1;
            // Mystic Impact remains a bonus.
            var mysticImpact = new SkillNode(nodeIndex, "Mystic Impact", 11, "Boosts spell power of macing spells", (p) =>
            {
                profile.Talents[TalentID.MacingSpellPowerBonus].Points += 1;
            });

            nodeIndex <<= 1;
            // Converted: Ancient Pummeling now uses spell bit 0x800.
            var ancientPummeling = new SkillNode(nodeIndex, "Ancient Pummeling", 11, "Unlocks advanced macing spells", (p) =>
            {
                profile.Talents[TalentID.MacingSpells].Points |= 0x800; // Spell bit 0x800
            });

            nodeIndex <<= 1;
            // Converted: Transformative Force now uses spell bit 0x1000.
            var transformativeForce = new SkillNode(nodeIndex, "Transformative Force", 11, "Unlocks a macing spell", (p) =>
            {
                profile.Talents[TalentID.MacingSpells].Points |= 0x1000; // Spell bit 0x1000
            });

            primevalFury.AddChild(expandedReach);
            savageMomentum.AddChild(mysticImpact);
            concussiveImpact.AddChild(ancientPummeling);
            mightyStrike.AddChild(transformativeForce);

            // Layer 7: Pinnacle bonuses.
            nodeIndex <<= 1;
            // Converted: Bastion of Force now uses spell bit 0x2000.
            var bastionOfForce = new SkillNode(nodeIndex, "Bastion of Force", 12, "Unlocks a macing spell", (p) =>
            {
                profile.Talents[TalentID.MacingSpells].Points |= 0x2000; // Spell bit 0x2000
            });

            nodeIndex <<= 1;
            // Nature's Retaliation remains a bonus.
            var naturesRetaliation = new SkillNode(nodeIndex, "Nature's Retaliation", 12, "Grants passive counter-attack bonus", (p) =>
            {
                profile.Talents[TalentID.MacingSecondaryAttack].Points += 1;
            });

            nodeIndex <<= 1;
            // Converted: Unstoppable Charge now uses spell bit 0x4000.
            var unstoppableCharge = new SkillNode(nodeIndex, "Unstoppable Charge", 12, "Unlocks a macing spell", (p) =>
            {
                profile.Talents[TalentID.MacingSpells].Points |= 0x4000; // Spell bit 0x4000
            });

            nodeIndex <<= 1;
            // Phantom Strike remains a bonus.
            var phantomStrike = new SkillNode(nodeIndex, "Phantom Strike", 12, "Chance to bypass enemy armor", (p) =>
            {
                profile.Talents[TalentID.MacingArmorPenetrationBonus].Points += 1;
            });

            expandedReach.AddChild(bastionOfForce);
            mysticImpact.AddChild(naturesRetaliation);
            ancientPummeling.AddChild(unstoppableCharge);
            transformativeForce.AddChild(phantomStrike);

            // Layer 8: Ultimate node.
            nodeIndex <<= 1;
            // Converted: Ultimate Macebearer now uses spell bit 0x8000.
            var ultimateMacebearer = new SkillNode(nodeIndex, "Ultimate Macebearer", 13, "Ultimate bonus: boosts all macing skills", (p) =>
            {
                profile.Talents[TalentID.MacingSpells].Points |= 0x8000; // Spell bit 0x8000
                // Grant bonus to all passive talents:
                profile.Talents[TalentID.MacingDamageBonus].Points += 1;
                profile.Talents[TalentID.MacingSpeedBonus].Points += 1;
                profile.Talents[TalentID.MacingDefenseBonus].Points += 1;
                profile.Talents[TalentID.MacingCriticalBonus].Points += 1;
                profile.Talents[TalentID.MacingArmorPenetrationBonus].Points += 1;
                profile.Talents[TalentID.MacingParryBonus].Points += 1;
                profile.Talents[TalentID.MacingRangeBonus].Points += 1;
                profile.Talents[TalentID.MacingSpellPowerBonus].Points += 1;
            });

            bastionOfForce.AddChild(ultimateMacebearer);
        }
    }
}
