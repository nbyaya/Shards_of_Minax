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

namespace Server.ACC.CSS.Systems.VeterinaryMagic
{
    // Revised Veterinary Skill Tree Gump using AncientKnowledge (Maxxia Points) as the cost resource.
    public class VeterinarySkillTree : SuperGump
    {
        private VeterinaryTree tree;
        private Dictionary<SkillNode, Point2D> nodePositions;
        private Dictionary<int, SkillNode> buttonNodeMap;
        private Dictionary<SkillNode, int> edgeThickness;
        private const int buttonSize = 15;
        private int ySpacing = 50, xSpacing = 60;
        private int rootX = 300, rootY = 150;
        private SkillNode selectedNode;

        public VeterinarySkillTree(PlayerMobile user)
            : base(user, null, 100, 100)
        {
            tree = new VeterinaryTree(user);
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
            layout.Add("title", () => { AddLabel(100, 20, 1153, "Veterinary Skill Tree"); });

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

            // New layout element to display the node's description.
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

    // The basic SkillNode implementation.
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
            // Ensure VeterinaryNodes talent exists.
            if (!profile.Talents.ContainsKey(TalentID.VeterinaryNodes))
                profile.Talents[TalentID.VeterinaryNodes] = new Talent(TalentID.VeterinaryNodes) { Points = 0 };

            return (profile.Talents[TalentID.VeterinaryNodes].Points & BitFlag) != 0;
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

            // Use AncientKnowledge points for unlocking.
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
            profile.Talents[TalentID.VeterinaryNodes].Points |= BitFlag;

            player.SendMessage($"{Name} activated!");
            onActivate?.Invoke(player);
            return true;
        }
    }

    // The full Veterinary tree structure with multiple layers and 30 nodes.
    public class VeterinaryTree
    {
        public SkillNode Root { get; }

        public VeterinaryTree(PlayerMobile player)
        {
            var profile = player.AcquireTalents();
            int nodeIndex = 0x01;

            // Layer 0: Root Node – Unlocks basic veterinary spells.
            Root = new SkillNode(nodeIndex, "Call of the Wild", 5, "Unlocks basic veterinary spells", (p) =>
            {
                // Unlock basic spells.
                profile.Talents[TalentID.VeterinarySpells] = new Talent(TalentID.VeterinarySpells) { Points = 0x01 };
            });

            // Layer 1: Basic bonuses.
            nodeIndex <<= 1;
            var animalEmpathy = new SkillNode(nodeIndex, "Animal Empathy", 6, "Increases taming success rate", (p) =>
            {
                profile.Talents[TalentID.MinionDamageBonus] = new Talent(TalentID.VeterinaryEmpathy) { Points = 1 };
            });

            nodeIndex <<= 1;
            var healingTouch = new SkillNode(nodeIndex, "Healing Touch", 6, "Improves pet healing abilities", (p) =>
            {
                profile.Talents[TalentID.MinionDamageBonus] = new Talent(TalentID.VeterinaryHealing) { Points = 1 };
            });

            nodeIndex <<= 1;
            var beastBonding = new SkillNode(nodeIndex, "Beast Bonding", 6, "Strengthens your bond with pets", (p) =>
            {
                profile.Talents[TalentID.MinionDamageBonus] = new Talent(TalentID.VeterinaryBonding) { Points = 1 };
            });

            nodeIndex <<= 1;
            var wildInstincts = new SkillNode(nodeIndex, "Wild Instincts", 6, "Enhances pet reflexes", (p) =>
            {
                profile.Talents[TalentID.MinionDamageBonus] = new Talent(TalentID.VeterinarySpeed) { Points = 1 };
            });

            Root.AddChild(animalEmpathy);
            Root.AddChild(healingTouch);
            Root.AddChild(beastBonding);
            Root.AddChild(wildInstincts);

            // Layer 2: Unlock additional spells and improve pet stats.
            nodeIndex <<= 1;
            var soothingPresence = new SkillNode(nodeIndex, "Soothing Presence", 7, "Unlocks additional calming spells", (p) =>
            {
                profile.Talents[TalentID.VeterinarySpells].Points |= 0x02;
            });

            nodeIndex <<= 1;
            // Converted from passive bonus to spell unlock (new bit 0x200)
            var vigorBoost = new SkillNode(nodeIndex, "Vigor Boost", 7, "Unlocks a veterinary spell (Stamina Enchantment)", (p) =>
            {
                profile.Talents[TalentID.VeterinarySpells].Points |= 0x200;
            });

            nodeIndex <<= 1;
            // Converted from passive bonus to spell unlock (new bit 0x400)
            var keenSenses = new SkillNode(nodeIndex, "Keen Senses", 7, "Unlocks a veterinary spell (Sense of Detection)", (p) =>
            {
                profile.Talents[TalentID.VeterinarySpells].Points |= 0x400;
            });

            nodeIndex <<= 1;
            var primalRoar = new SkillNode(nodeIndex, "Primal Roar", 7, "Unlocks offensive veterinary spells", (p) =>
            {
                profile.Talents[TalentID.VeterinarySpells].Points |= 0x04;
            });

            animalEmpathy.AddChild(soothingPresence);
            healingTouch.AddChild(vigorBoost);
            beastBonding.AddChild(keenSenses);
            wildInstincts.AddChild(primalRoar);

            // Layer 3: Further unlocks (replacing some passive bonuses with spell unlocks).
            nodeIndex <<= 1;
            // Converted from passive bonus to spell unlock (new bit 0x800)
            var nurturingSpirit = new SkillNode(nodeIndex, "Nurturing Spirit", 8, "Unlocks a veterinary spell (Healing Resonance)", (p) =>
            {
                profile.Talents[TalentID.VeterinarySpells].Points |= 0x800;
            });

            nodeIndex <<= 1;
            // Converted from passive bonus to spell unlock (new bit 0x1000)
            var steadyHands = new SkillNode(nodeIndex, "Steady Hands", 8, "Unlocks a veterinary spell (Precision Touch)", (p) =>
            {
                profile.Talents[TalentID.VeterinarySpells].Points |= 0x1000;
            });

            nodeIndex <<= 1;
            var wildResilience = new SkillNode(nodeIndex, "Wild Resilience", 8, "Increases pet vitality", (p) =>
            {
                profile.Talents[TalentID.MinionDamageBonus] = new Talent(TalentID.VeterinarySpeed) { Points = 1 };
            });

            nodeIndex <<= 1;
            var callOfTheHerd = new SkillNode(nodeIndex, "Call of the Herd", 8, "Enhances ability to summon support", (p) =>
            {
                profile.Talents[TalentID.MinionDamageBonus] = new Talent(TalentID.VeterinarySpeed) { Points = 1 };
            });

            soothingPresence.AddChild(nurturingSpirit);
            vigorBoost.AddChild(steadyHands);
            keenSenses.AddChild(wildResilience);
            primalRoar.AddChild(callOfTheHerd);

            // Layer 4: Advanced magical enhancements.
            nodeIndex <<= 1;
            // Converted from passive bonus to spell unlock (new bit 0x2000)
            var ancientWisdom = new SkillNode(nodeIndex, "Ancient Wisdom", 9, "Unlocks a veterinary spell (Primal Insight)", (p) =>
            {
                profile.Talents[TalentID.VeterinarySpells].Points |= 0x2000;
            });

            nodeIndex <<= 1;
            var sacredPaws = new SkillNode(nodeIndex, "Sacred Paws", 9, "Unlocks advanced healing spells for pets", (p) =>
            {
                profile.Talents[TalentID.VeterinarySpells].Points |= 0x08;
            });

            nodeIndex <<= 1;
            var feralFocus = new SkillNode(nodeIndex, "Feral Focus", 9, "Enhances pet combat abilities", (p) =>
            {
                profile.Talents[TalentID.MinionDamageBonus] = new Talent(TalentID.VeterinarySpeed) { Points = 1 };
            });

            nodeIndex <<= 1;
            var soaringSpirit = new SkillNode(nodeIndex, "Soaring Spirit", 9, "Increases pet agility", (p) =>
            {
                profile.Talents[TalentID.MinionDamageBonus] = new Talent(TalentID.VeterinarySpeed) { Points = 1 };
            });

            nurturingSpirit.AddChild(ancientWisdom);
            steadyHands.AddChild(sacredPaws);
            wildResilience.AddChild(feralFocus);
            callOfTheHerd.AddChild(soaringSpirit);

            // Layer 5: Expert-level nodes.
            nodeIndex <<= 1;
            // Converted from passive bonus to spell unlock (new bit 0x4000)
            var primevalCare = new SkillNode(nodeIndex, "Primeval Care", 10, "Unlocks a veterinary spell (Vital Surge)", (p) =>
            {
                profile.Talents[TalentID.VeterinarySpells].Points |= 0x4000;
            });

            nodeIndex <<= 1;
            var bondOfTheWild = new SkillNode(nodeIndex, "Bond of the Wild", 10, "Deepens pet loyalty", (p) =>
            {
                profile.Talents[TalentID.MinionDamageBonus] = new Talent(TalentID.VeterinarySpeed) { Points = 1 };
            });

            nodeIndex <<= 1;
            var beastMastery = new SkillNode(nodeIndex, "Beast Mastery", 10, "Unlocks advanced veterinary spells", (p) =>
            {
                profile.Talents[TalentID.VeterinarySpells].Points |= 0x10;
            });

            nodeIndex <<= 1;
            var harmonyOfNature = new SkillNode(nodeIndex, "Harmony of Nature", 10, "Improves synergy between pet and master", (p) =>
            {
                profile.Talents[TalentID.MinionDamageBonus] = new Talent(TalentID.VeterinarySpeed) { Points = 1 };
            });

            ancientWisdom.AddChild(primevalCare);
            sacredPaws.AddChild(bondOfTheWild);
            feralFocus.AddChild(beastMastery);
            soaringSpirit.AddChild(harmonyOfNature);

            // Layer 6: Mastery nodes.
            nodeIndex <<= 1;
            // Converted from passive bonus to spell unlock (new bit 0x8000)
            var expandedEmpathy = new SkillNode(nodeIndex, "Expanded Empathy", 11, "Unlocks a veterinary spell (Empathic Surge)", (p) =>
            {
                profile.Talents[TalentID.VeterinarySpells].Points |= 0x8000;
            });

            nodeIndex <<= 1;
            var mysticMenagerie = new SkillNode(nodeIndex, "Mystic Menagerie", 11, "Unlocks magical pet enhancements", (p) =>
            {
                profile.Talents[TalentID.MinionDamageBonus] = new Talent(TalentID.VeterinarySpeed) { Points = 1 };
            });

            nodeIndex <<= 1;
            var ancientBond = new SkillNode(nodeIndex, "Ancient Bond", 11, "Unlocks deeper veterinary spells", (p) =>
            {
                profile.Talents[TalentID.VeterinarySpells].Points |= 0x20;
            });

            nodeIndex <<= 1;
            var rapidRecovery = new SkillNode(nodeIndex, "Rapid Recovery", 11, "Speeds up pet recovery", (p) =>
            {
                profile.Talents[TalentID.MinionDamageBonus] = new Talent(TalentID.VeterinarySpeed) { Points = 1 };
            });

            primevalCare.AddChild(expandedEmpathy);
            bondOfTheWild.AddChild(mysticMenagerie);
            beastMastery.AddChild(ancientBond);
            harmonyOfNature.AddChild(rapidRecovery);

            // Layer 7: Final, pinnacle bonuses.
            nodeIndex <<= 1;
            var guardiansGrace = new SkillNode(nodeIndex, "Guardian's Grace", 12, "Enhances pet defenses", (p) =>
            {
                profile.Talents[TalentID.MinionDamageBonus] = new Talent(TalentID.VeterinarySpeed) { Points = 1 };
            });

            nodeIndex <<= 1;
            var wildSurge = new SkillNode(nodeIndex, "Wild Surge", 12, "Increases pet attack speed", (p) =>
            {
                profile.Talents[TalentID.MinionDamageBonus] = new Talent(TalentID.VeterinarySpeed) { Points = 1 };
            });

            nodeIndex <<= 1;
            var feralInsight = new SkillNode(nodeIndex, "Feral Insight", 12, "Unlocks insight spells for pets", (p) =>
            {
                profile.Talents[TalentID.VeterinarySpells].Points |= 0x40;
            });
            // (Corrected below: use nodeIndex instead of 'node')
            nodeIndex <<= 1;
            var naturesEndowment = new SkillNode(nodeIndex, "Nature's Endowment", 12, "Boosts overall pet stats", (p) =>
            {
                profile.Talents[TalentID.MinionDamageBonus] = new Talent(TalentID.VeterinarySpeed) { Points = 1 };
            });

            expandedEmpathy.AddChild(guardiansGrace);
            mysticMenagerie.AddChild(wildSurge);
            ancientBond.AddChild(feralInsight);
            rapidRecovery.AddChild(naturesEndowment);

            // Layer 8: Ultimate node.
            nodeIndex <<= 1;
            var ultimateVeterinarian = new SkillNode(nodeIndex, "Ultimate Veterinarian", 13, "Ultimate bonus: boosts all veterinary abilities", (p) =>
            {
                // Unlock two more spell bits.
                profile.Talents[TalentID.VeterinarySpells].Points |= 0x80 | 0x100;
                profile.Talents[TalentID.MinionDamageBonus].Points += 1;
                profile.Talents[TalentID.MinionDamageBonus].Points += 1;
                profile.Talents[TalentID.MinionDamageBonus].Points += 1;
                profile.Talents[TalentID.MinionDamageBonus].Points += 1;
                profile.Talents[TalentID.MinionDamageBonus].Points += 1;
                profile.Talents[TalentID.MinionDamageBonus].Points += 1;
            });

            guardiansGrace.AddChild(ultimateVeterinarian);
            wildSurge.AddChild(ultimateVeterinarian);
            feralInsight.AddChild(ultimateVeterinarian);
            naturesEndowment.AddChild(ultimateVeterinarian);
        }
    }

    // Command to open the Veterinary Skill Tree.
    public class VeterinarySkillTreeCommand
    {
        public static void Initialize()
        {
            CommandSystem.Register("VetTree", AccessLevel.Player, cmd => ShowTree(cmd.Mobile));
        }

        private static void ShowTree(Mobile m)
        {
            if (m is PlayerMobile pm)
            {
                pm.SendMessage("Opening Veterinary Skill Tree...");
                pm.SendGump(new VeterinarySkillTree(pm));
            }
            else
            {
                m.SendMessage("You must be a player to use this command.");
            }
        }
    }
}
