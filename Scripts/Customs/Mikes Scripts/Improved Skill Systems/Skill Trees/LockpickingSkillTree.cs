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

namespace Server.ACC.CSS.Systems.LockpickingMagic
{
    // Revised Lockpicking Skill Tree Gump using AncientKnowledge (Maxxia Points) as the cost resource.
    public class LockpickingSkillTree : SuperGump
    {
        private LockpickingTree tree;
        private Dictionary<SkillNode, Point2D> nodePositions;
        private Dictionary<int, SkillNode> buttonNodeMap;
        private Dictionary<SkillNode, int> edgeThickness;
        private const int buttonSize = 15;
        private int ySpacing = 50, xSpacing = 60;
        private int rootX = 300, rootY = 150;
        private SkillNode selectedNode;

        public LockpickingSkillTree(PlayerMobile user)
            : base(user, null, 100, 100)
        {
            tree = new LockpickingTree(user);
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
            layout.Add("title", () => { AddLabel(100, 20, 1153, "Lockpicking Skill Tree"); });

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

    // Revised SkillNode used by the Lockpicking tree.
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
            if (!profile.Talents.ContainsKey(TalentID.LockpickingNodes))
                profile.Talents[TalentID.LockpickingNodes] = new Talent(TalentID.LockpickingNodes) { Points = 0 };

            return (profile.Talents[TalentID.LockpickingNodes].Points & BitFlag) != 0;
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
            profile.Talents[TalentID.LockpickingNodes].Points |= BitFlag;

            player.SendMessage($"{Name} activated!");
            onActivate?.Invoke(player);
            return true;
        }
    }

    // Full Lockpicking tree structure with 30 nodes (9 layers).
    public class LockpickingTree
    {
        public SkillNode Root { get; }

        public LockpickingTree(PlayerMobile player)
        {
            var profile = player.AcquireTalents();
            int nodeIndex = 0x01;

            // Layer 0: Root Node – Unlocks basic lockpicking spells.
            Root = new SkillNode(nodeIndex, "Whisper of the Lock", 5, "Unlocks basic lockpicking spells", (p) =>
            {
                // Unlock basic spells (bit 0x01)
                profile.Talents[TalentID.LockpickingSpells].Points |= 0x01;
            });

            // Layer 1: Basic bonuses.
            nodeIndex <<= 1;
            var steadyHand = new SkillNode(nodeIndex, "Steady Hand", 6, "Increases lockpicking success chance", (p) =>
            {
                profile.Talents[TalentID.LockpickingChance].Points += 1;
            });

            nodeIndex <<= 1;
            var silentApproach = new SkillNode(nodeIndex, "Silent Approach", 6, "Improves stealth when picking locks", (p) =>
            {
                profile.Talents[TalentID.LockpickingStealth].Points += 1;
            });

            nodeIndex <<= 1;
            var masterfulTinkering = new SkillNode(nodeIndex, "Masterful Tinkering", 6, "Unlocks bonus lockpicking spells", (p) =>
            {
                profile.Talents[TalentID.LockpickingSpells].Points |= 0x04;
            });

            nodeIndex <<= 1;
            var quickFingers = new SkillNode(nodeIndex, "Quick Fingers", 6, "Reduces lockpick breakage", (p) =>
            {
                profile.Talents[TalentID.LockpickingDurability].Points += 1;
            });

            Root.AddChild(steadyHand);
            Root.AddChild(silentApproach);
            Root.AddChild(masterfulTinkering);
            Root.AddChild(quickFingers);

            // Layer 2: Advanced magical and practical bonuses.
            nodeIndex <<= 1;
            var hiddenMechanisms = new SkillNode(nodeIndex, "Hidden Mechanisms", 7, "Unlocks additional lockpicking spells", (p) =>
            {
                profile.Talents[TalentID.LockpickingSpells].Points |= 0x08;
            });

            nodeIndex <<= 1;
            var sureGrip = new SkillNode(nodeIndex, "Sure Grip", 7, "Improves grip, increasing lock success", (p) =>
            {
                profile.Talents[TalentID.LockpickingChance].Points += 1;
            });

            nodeIndex <<= 1;
            var rapidManipulation = new SkillNode(nodeIndex, "Rapid Manipulation", 7, "Increases lockpicking speed", (p) =>
            {
                profile.Talents[TalentID.LockpickingSpeed].Points += 1;
            });

            nodeIndex <<= 1;
            var stealthyMovement = new SkillNode(nodeIndex, "Stealthy Movement", 7, "Enhances stealth in dangerous areas", (p) =>
            {
                profile.Talents[TalentID.LockpickingStealth].Points += 1;
            });

            steadyHand.AddChild(hiddenMechanisms);
            silentApproach.AddChild(sureGrip);
            masterfulTinkering.AddChild(rapidManipulation);
            quickFingers.AddChild(stealthyMovement);

            // Layer 3: Further improvements.
            nodeIndex <<= 1;
            var cunningContraptions = new SkillNode(nodeIndex, "Cunning Contraptions", 8, "Enhances unlocking bonus spells", (p) =>
            {
                profile.Talents[TalentID.LockpickingSpells].Points |= 0x10;
            });

            nodeIndex <<= 1;
            var precisePerception = new SkillNode(nodeIndex, "Precise Perception", 8, "Improves lockpicking accuracy", (p) =>
            {
                profile.Talents[TalentID.LockpickingChance].Points += 1;
            });

            nodeIndex <<= 1;
            var dexterousFingers = new SkillNode(nodeIndex, "Dexterous Fingers", 8, "Further increases lockpicking speed", (p) =>
            {
                profile.Talents[TalentID.LockpickingSpeed].Points += 1;
            });

            nodeIndex <<= 1;
            var reinforcedPicks = new SkillNode(nodeIndex, "Reinforced Picks", 8, "Increases durability of lockpicks", (p) =>
            {
                profile.Talents[TalentID.LockpickingDurability].Points += 1;
            });

            hiddenMechanisms.AddChild(cunningContraptions);
            sureGrip.AddChild(precisePerception);
            rapidManipulation.AddChild(dexterousFingers);
            stealthyMovement.AddChild(reinforcedPicks);

            // Layer 4: More advanced magical enhancements.
            nodeIndex <<= 1;
            var secretSchematics = new SkillNode(nodeIndex, "Secret Schematics", 9, "Unlocks advanced lockpicking spells", (p) =>
            {
                profile.Talents[TalentID.LockpickingSpells].Points |= 0x20;
            });

            nodeIndex <<= 1;
            var intuitiveMechanics = new SkillNode(nodeIndex, "Intuitive Mechanics", 9, "Improves lockpicking chance significantly", (p) =>
            {
                profile.Talents[TalentID.LockpickingChance].Points += 1;
            });

            nodeIndex <<= 1;
            var lightningReflexes = new SkillNode(nodeIndex, "Lightning Reflexes", 9, "Boosts speed when picking locks", (p) =>
            {
                profile.Talents[TalentID.LockpickingSpeed].Points += 1;
            });

            nodeIndex <<= 1;
            var unbreakableWill = new SkillNode(nodeIndex, "Unbreakable Will", 9, "Greatly reduces lockpick breakage", (p) =>
            {
                profile.Talents[TalentID.LockpickingDurability].Points += 1;
            });

            cunningContraptions.AddChild(secretSchematics);
            precisePerception.AddChild(intuitiveMechanics);
            dexterousFingers.AddChild(lightningReflexes);
            reinforcedPicks.AddChild(unbreakableWill);

            // Layer 5: Expert-level nodes.
            nodeIndex <<= 1;
            var primeLockMastery = new SkillNode(nodeIndex, "Prime Lock Mastery", 10, "Enhances overall lockpicking efficiency", (p) =>
            {
                profile.Talents[TalentID.LockpickingChance].Points += 1;
                profile.Talents[TalentID.LockpickingSpeed].Points += 1;
            });

            nodeIndex <<= 1;
            var silentSabotage = new SkillNode(nodeIndex, "Silent Sabotage", 10, "Increases stealth and reduces noise", (p) =>
            {
                profile.Talents[TalentID.LockpickingStealth].Points += 1;
            });

            nodeIndex <<= 1;
            var locksmithsLore = new SkillNode(nodeIndex, "Locksmith's Lore", 10, "Unlocks mastery level lockpicking spells", (p) =>
            {
                profile.Talents[TalentID.LockpickingSpells].Points |= 0x100;
            });

            nodeIndex <<= 1;
            var fingerOfFate = new SkillNode(nodeIndex, "Finger of Fate", 10, "Boosts critical lockpicking chance", (p) =>
            {
                profile.Talents[TalentID.LockpickingChance].Points += 1;
            });

            secretSchematics.AddChild(primeLockMastery);
            intuitiveMechanics.AddChild(silentSabotage);
            lightningReflexes.AddChild(locksmithsLore);
            unbreakableWill.AddChild(fingerOfFate);

            // Layer 6: Mastery nodes.
            nodeIndex <<= 1;
            var heightenedAwareness = new SkillNode(nodeIndex, "Heightened Awareness", 11, "Enhances environmental awareness", (p) =>
            {
                profile.Talents[TalentID.LockpickingChance].Points += 1;
            });

            nodeIndex <<= 1;
            var swiftTouch = new SkillNode(nodeIndex, "Swift Touch", 11, "Boosts lockpicking speed", (p) =>
            {
                profile.Talents[TalentID.LockpickingSpeed].Points += 1;
            });

            nodeIndex <<= 1;
            var ancientLocksmith = new SkillNode(nodeIndex, "Ancient Locksmith", 11, "Unlocks ancient lockpicking spells", (p) =>
            {
                profile.Talents[TalentID.LockpickingSpells].Points |= 0x200;
            });

            nodeIndex <<= 1;
            var craftsmenPrecision = new SkillNode(nodeIndex, "Craftsman's Precision", 11, "Increases tool durability", (p) =>
            {
                profile.Talents[TalentID.LockpickingDurability].Points += 1;
            });

            primeLockMastery.AddChild(heightenedAwareness);
            silentSabotage.AddChild(swiftTouch);
            locksmithsLore.AddChild(ancientLocksmith);
            fingerOfFate.AddChild(craftsmenPrecision);

            // Layer 7: Final, pinnacle bonuses.
            nodeIndex <<= 1;
            var barrierOfSilence = new SkillNode(nodeIndex, "Barrier of Silence", 12, "Provides a protective silence aura", (p) =>
            {
                // This could later grant defensive bonuses.
                profile.Talents[TalentID.LockpickingStealth].Points += 1;
            });

            nodeIndex <<= 1;
            var unseenAdvantage = new SkillNode(nodeIndex, "Unseen Advantage", 12, "Further increases stealth bonus", (p) =>
            {
                profile.Talents[TalentID.LockpickingStealth].Points += 1;
            });

            nodeIndex <<= 1;
            var mastersIntuition = new SkillNode(nodeIndex, "Master's Intuition", 12, "Enhances lockpicking chance drastically", (p) =>
            {
                profile.Talents[TalentID.LockpickingChance].Points += 1;
            });

            nodeIndex <<= 1;
            var lightningLockdown = new SkillNode(nodeIndex, "Lightning Lockdown", 12, "Grants extraordinary speed in picking locks", (p) =>
            {
                profile.Talents[TalentID.LockpickingSpeed].Points += 1;
            });

            heightenedAwareness.AddChild(barrierOfSilence);
            swiftTouch.AddChild(unseenAdvantage);
            ancientLocksmith.AddChild(mastersIntuition);
            craftsmenPrecision.AddChild(lightningLockdown);

            // Layer 8: Ultimate node.
            nodeIndex <<= 1;
            var ultimateLocksmith = new SkillNode(nodeIndex, "Ultimate Locksmith", 13, "Ultimate bonus: greatly enhances all lockpicking abilities", (p) =>
            {
                // Grant a final bonus to spells and all passive bonuses.
                profile.Talents[TalentID.LockpickingSpells].Points |= 0x800 | 0x1000;
                profile.Talents[TalentID.LockpickingChance].Points += 1;
                profile.Talents[TalentID.LockpickingSpeed].Points += 1;
                profile.Talents[TalentID.LockpickingDurability].Points += 1;
                profile.Talents[TalentID.LockpickingStealth].Points += 1;
            });

            foreach (var node in new[] { barrierOfSilence, lightningLockdown, mastersIntuition, unseenAdvantage })
            {
                node.AddChild(ultimateLocksmith);
            }
        }
    }

    // Command to open the Lockpicking Skill Tree.
    public class LockpickingSkillTreeCommand
    {
        public static void Initialize()
        {
            CommandSystem.Register("LockpickTree", AccessLevel.Player, cmd => ShowTree(cmd.Mobile));
        }

        private static void ShowTree(Mobile m)
        {
            if (m is PlayerMobile pm)
            {
                pm.SendMessage("Opening Lockpicking Skill Tree...");
                pm.SendGump(new LockpickingSkillTree(pm));
            }
            else
            {
                m.SendMessage("You must be a player to use this command.");
            }
        }
    }
}
