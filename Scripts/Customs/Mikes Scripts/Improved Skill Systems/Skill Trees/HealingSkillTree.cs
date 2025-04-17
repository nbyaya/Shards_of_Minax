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

namespace Server.ACC.CSS.Systems.HealingMagic
{
    // Revised Healing Skill Tree Gump using Maxxia Points (AncientKnowledge) as the cost resource.
    public class HealingSkillTree : SuperGump
    {
        private HealingTree tree;
        private Dictionary<HealingNode, Point2D> nodePositions;
        private Dictionary<int, HealingNode> buttonNodeMap;
        private Dictionary<HealingNode, int> edgeThickness;
        private const int buttonSize = 15;
        private int ySpacing = 50, xSpacing = 60;
        private int rootX = 300, rootY = 150;
        private HealingNode selectedNode;

        public HealingSkillTree(PlayerMobile user)
            : base(user, null, 100, 100)
        {
            tree = new HealingTree(user);
            nodePositions = new Dictionary<HealingNode, Point2D>();
            buttonNodeMap = new Dictionary<int, HealingNode>();
            edgeThickness = new Dictionary<HealingNode, int>();

            CalculateNodePositions(tree.Root, rootX, rootY, 0);
            InitializeEdgeThickness();

            User.SendGump(this);
        }

        private void CalculateNodePositions(HealingNode root, int x, int y, int depth)
        {
            if (root == null)
                return;

            var levelNodes = new Dictionary<int, List<HealingNode>>();
            var queue = new Queue<(HealingNode node, int level)>();
            var visited = new HashSet<HealingNode>();

            queue.Enqueue((root, 0));

            while (queue.Count > 0)
            {
                var (node, level) = queue.Dequeue();
                if (!visited.Add(node))
                    continue;

                if (!levelNodes.ContainsKey(level))
                    levelNodes[level] = new List<HealingNode>();

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
            layout.Add("title", () => { AddLabel(100, 20, 1153, "Healing Skill Tree"); });

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
            if (buttonNodeMap.TryGetValue(button.ButtonID, out HealingNode node))
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

    // HealingNode: similar to SkillNode but for healing.
    public class HealingNode
    {
        public int BitFlag { get; }
        public string Name { get; }
        public int Cost { get; }
        public string Description { get; }
        public List<HealingNode> Children { get; }
        public HealingNode Parent { get; private set; }
        private readonly Action<PlayerMobile> onActivate;

        public HealingNode(int bitFlag, string name, int cost, string description = "", Action<PlayerMobile> onActivate = null)
        {
            BitFlag = bitFlag;
            Name = name;
            Cost = cost;
            Description = description;
            Children = new List<HealingNode>();
            this.onActivate = onActivate;
        }

        public void AddChild(HealingNode child)
        {
            child.Parent = this;
            Children.Add(child);
        }

        public bool IsActivated(PlayerMobile player)
        {
            var profile = player.AcquireTalents();
            if (!profile.Talents.ContainsKey(TalentID.HealingNodes))
                profile.Talents[TalentID.HealingNodes] = new Talent(TalentID.HealingNodes) { Points = 0 };

            return (profile.Talents[TalentID.HealingNodes].Points & BitFlag) != 0;
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
            profile.Talents[TalentID.HealingNodes].Points |= BitFlag;

            player.SendMessage($"{Name} activated!");
            onActivate?.Invoke(player);
            return true;
        }
    }

    // HealingTree: Builds the full 30-node tree (layers 0 through 8) for healing.
    public class HealingTree
    {
        public HealingNode Root { get; }

        public HealingTree(PlayerMobile player)
        {
            var profile = player.AcquireTalents();
            int nodeIndex = 0x01;

            // Layer 0: Root Node – spell unlock 0x01.
            Root = new HealingNode(nodeIndex, "Essence of Vitality", 5, "Unlocks basic healing spells", (p) =>
            {
                profile.Talents[TalentID.HealingSpells].Points |= 0x01;
            });

            // Layer 1: Four nodes.
            nodeIndex <<= 1; // now 0x02
            var touchOfRenewal = new HealingNode(nodeIndex, "Touch of Renewal", 6, "Unlocks spell (0x02)", (p) =>
            {
                profile.Talents[TalentID.HealingSpells].Points |= 0x02;
            });

            nodeIndex <<= 1; // now 0x04
            var swiftSalve = new HealingNode(nodeIndex, "Swift Salve", 6, "Unlocks spell (0x04)", (p) =>
            {
                profile.Talents[TalentID.HealingSpells].Points |= 0x04;
            });

            nodeIndex <<= 1; // now 0x08
            var restorativeHands = new HealingNode(nodeIndex, "Restorative Hands", 6, "Unlocks spell (0x08)", (p) =>
            {
                profile.Talents[TalentID.HealingSpells].Points |= 0x08;
            });

            nodeIndex <<= 1; // now 0x10
            var herbalWisdom = new HealingNode(nodeIndex, "Herbal Wisdom", 6, "Unlocks spell (0x10)", (p) =>
            {
                profile.Talents[TalentID.HealingSpells].Points |= 0x10;
            });

            Root.AddChild(touchOfRenewal);
            Root.AddChild(swiftSalve);
            Root.AddChild(restorativeHands);
            Root.AddChild(herbalWisdom);

            // Layer 2: Four nodes.
            nodeIndex <<= 1; // now 0x20
            var mendWounds = new HealingNode(nodeIndex, "Mend Wounds", 7, "Unlocks Mend Wounds spell (0x20)", (p) =>
            {
                profile.Talents[TalentID.HealingSpells].Points |= 0x20;
            });

            nodeIndex <<= 1; // now 0x40
            var rapidRecovery = new HealingNode(nodeIndex, "Rapid Recovery", 7, "Unlocks Rapid Recovery spell (0x40)", (p) =>
            {
                profile.Talents[TalentID.HealingSpells].Points |= 0x40;
            });

            nodeIndex <<= 1; // now 0x80
            var cleansingLight = new HealingNode(nodeIndex, "Cleansing Light", 7, "Unlocks Cleansing Light spell (0x80)", (p) =>
            {
                profile.Talents[TalentID.HealingSpells].Points |= 0x80;
            });

            nodeIndex <<= 1; // now 0x100
            var purifyingTouch = new HealingNode(nodeIndex, "Purifying Touch", 7, "Unlocks Purifying Touch spell (0x100)", (p) =>
            {
                profile.Talents[TalentID.HealingSpells].Points |= 0x100;
            });

            touchOfRenewal.AddChild(mendWounds);
            swiftSalve.AddChild(rapidRecovery);
            restorativeHands.AddChild(cleansingLight);
            herbalWisdom.AddChild(purifyingTouch);

            // Layer 3: Four nodes.
            nodeIndex <<= 1; // now 0x200
            var surgeOfLife = new HealingNode(nodeIndex, "Surge of Life", 8, "Unlocks spell (0x200)", (p) =>
            {
                profile.Talents[TalentID.HealingSpells].Points |= 0x200;
            });

            nodeIndex <<= 1; // now 0x400
            var divineIntervention = new HealingNode(nodeIndex, "Divine Intervention", 8, "Unlocks spell (0x400)", (p) =>
            {
                profile.Talents[TalentID.HealingSpells].Points |= 0x400;
            });

            nodeIndex <<= 1; // now 0x800
            var soothingAura = new HealingNode(nodeIndex, "Soothing Aura", 8, "Unlocks spell (0x800)", (p) =>
            {
                profile.Talents[TalentID.HealingSpells].Points |= 0x800;
            });

            nodeIndex <<= 1; // now 0x1000
            var healingResilience = new HealingNode(nodeIndex, "Healing Resilience", 8, "Unlocks spell (0x1000)", (p) =>
            {
                profile.Talents[TalentID.HealingSpells].Points |= 0x1000;
            });

            mendWounds.AddChild(surgeOfLife);
            rapidRecovery.AddChild(divineIntervention);
            cleansingLight.AddChild(soothingAura);
            purifyingTouch.AddChild(healingResilience);

            // Layer 4: Four nodes – first three become spell unlocks.
            nodeIndex <<= 1; // now 0x2000
            var auraOfRestoration = new HealingNode(nodeIndex, "Aura of Restoration", 9, "Unlocks spell (0x2000)", (p) =>
            {
                profile.Talents[TalentID.HealingSpells].Points |= 0x2000;
            });

            nodeIndex <<= 1; // now 0x4000
            var blessedRecovery = new HealingNode(nodeIndex, "Blessed Recovery", 9, "Unlocks spell (0x4000)", (p) =>
            {
                profile.Talents[TalentID.HealingSpells].Points |= 0x4000;
            });

            nodeIndex <<= 1; // now 0x8000
            var mysticRegeneration = new HealingNode(nodeIndex, "Mystic Regeneration", 9, "Unlocks spell (0x8000)", (p) =>
            {
                profile.Talents[TalentID.HealingSpells].Points |= 0x8000;
            });

            // The fourth node of layer 4 remains as a bonus (unchanged).
            nodeIndex <<= 1; 
            var sacredElixir = new HealingNode(nodeIndex, "Sacred Elixir", 9, "Unlocks bonus healing spell", (p) =>
            {
                // Original passive bonus remains.
                profile.Talents[TalentID.HealingEfficiency].Points += 1;
            });

            surgeOfLife.AddChild(auraOfRestoration);
            divineIntervention.AddChild(blessedRecovery);
            soothingAura.AddChild(mysticRegeneration);
            healingResilience.AddChild(sacredElixir);

            // Layer 5: Four nodes remain passive.
            nodeIndex <<= 1;
            var primevalRestoration = new HealingNode(nodeIndex, "Primeval Restoration", 10, "Boosts healing power significantly", (p) =>
            {
                profile.Talents[TalentID.HealingPower].Points += 1;
            });

            nodeIndex <<= 1;
            var healersGrace = new HealingNode(nodeIndex, "Healer's Grace", 10, "Greatly improves healing cast speed", (p) =>
            {
                profile.Talents[TalentID.HealingCastSpeed].Points += 1;
            });

            nodeIndex <<= 1;
            var masterHealer = new HealingNode(nodeIndex, "Master Healer", 10, "Enhances healing mastery", (p) =>
            {
                // Passive effect.
            });

            nodeIndex <<= 1;
            var healingMomentum = new HealingNode(nodeIndex, "Healing Momentum", 10, "Enhances healing efficiency", (p) =>
            {
                profile.Talents[TalentID.HealingEfficiency].Points += 1;
            });

            auraOfRestoration.AddChild(primevalRestoration);
            blessedRecovery.AddChild(healersGrace);
            mysticRegeneration.AddChild(masterHealer);
            sacredElixir.AddChild(healingMomentum);

            // Layer 6: Four nodes remain passive.
            nodeIndex <<= 1;
            var expandedVitality = new HealingNode(nodeIndex, "Expanded Vitality", 11, "Greatly enhances healing power", (p) =>
            {
                profile.Talents[TalentID.HealingPower].Points += 1;
            });

            nodeIndex <<= 1;
            var mysticMending = new HealingNode(nodeIndex, "Mystic Mending", 11, "Boosts healing cast speed", (p) =>
            {
                profile.Talents[TalentID.HealingCastSpeed].Points += 1;
            });

            nodeIndex <<= 1;
            var ancientCure = new HealingNode(nodeIndex, "Ancient Cure", 11, "Enhances ancient healing", (p) =>
            {
                // Passive effect.
            });

            nodeIndex <<= 1;
            var divineTouch = new HealingNode(nodeIndex, "Divine Touch", 11, "Improves healing efficiency", (p) =>
            {
                profile.Talents[TalentID.HealingEfficiency].Points += 1;
            });

            primevalRestoration.AddChild(expandedVitality);
            healersGrace.AddChild(mysticMending);
            masterHealer.AddChild(ancientCure);
            healingMomentum.AddChild(divineTouch);

            // Layer 7: Four nodes remain passive.
            nodeIndex <<= 1;
            var protectiveWard = new HealingNode(nodeIndex, "Protective Ward", 12, "Grants a protective barrier", (p) =>
            {
                // Passive effect.
            });

            nodeIndex <<= 1;
            var eternalRenewal = new HealingNode(nodeIndex, "Eternal Renewal", 12, "Further boosts healing power", (p) =>
            {
                profile.Talents[TalentID.HealingPower].Points += 1;
            });

            nodeIndex <<= 1;
            var healingFrenzy = new HealingNode(nodeIndex, "Healing Frenzy", 12, "Dramatically increases cast speed", (p) =>
            {
                profile.Talents[TalentID.HealingCastSpeed].Points += 1;
            });

            nodeIndex <<= 1;
            var vitalSurge = new HealingNode(nodeIndex, "Vital Surge", 12, "Enhances overall healing efficiency", (p) =>
            {
                profile.Talents[TalentID.HealingEfficiency].Points += 1;
            });

            expandedVitality.AddChild(protectiveWard);
            mysticMending.AddChild(eternalRenewal);
            ancientCure.AddChild(healingFrenzy);
            divineTouch.AddChild(vitalSurge);

            // Layer 8: Ultimate node remains passive.
            nodeIndex <<= 1;
            var ultimateHealer = new HealingNode(nodeIndex, "Ultimate Healer", 13, "Ultimate bonus: boosts all healing abilities", (p) =>
            {
                profile.Talents[TalentID.HealingPower].Points += 1;
                profile.Talents[TalentID.HealingCastSpeed].Points += 1;
                profile.Talents[TalentID.HealingEfficiency].Points += 1;
            });

            foreach (var node in new[] { protectiveWard, vitalSurge, healingFrenzy, eternalRenewal })
                node.AddChild(ultimateHealer);
        }
    }

    // Command to open the Healing Skill Tree.
    public class HealingSkillTreeCommand
    {
        public static void Initialize()
        {
            CommandSystem.Register("HealingTree", AccessLevel.Player, cmd => ShowTree(cmd.Mobile));
        }

        private static void ShowTree(Mobile m)
        {
            if (m is PlayerMobile pm)
            {
                pm.SendMessage("Opening Healing Skill Tree...");
                pm.SendGump(new HealingSkillTree(pm));
            }
            else
            {
                m.SendMessage("You must be a player to use this command.");
            }
        }
    }
}
