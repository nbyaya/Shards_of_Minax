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

namespace Server.ACC.CSS.Systems.NinjitsuMagic
{
    // Revised Ninjitsu Skill Tree Gump using AncientKnowledge (Maxxia Points) as the cost resource.
    public class NinjitsuSkillTree : SuperGump
    {
        private NinjitsuTree tree;
        private Dictionary<SkillNode, Point2D> nodePositions;
        private Dictionary<int, SkillNode> buttonNodeMap;
        private Dictionary<SkillNode, int> edgeThickness;
        private const int buttonSize = 15;
        private int ySpacing = 50, xSpacing = 60;
        private int rootX = 300, rootY = 150;
        private SkillNode selectedNode;

        public NinjitsuSkillTree(PlayerMobile user)
            : base(user, null, 100, 100)
        {
            tree = new NinjitsuTree(user);
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
            layout.Add("title", () => { AddLabel(100, 20, 1153, "Ninjitsu Skill Tree"); });

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

    // The common SkillNode class used for both trees.
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
            // For Ninjitsu nodes, we use the NinjitsuNodes talent.
            if (!profile.Talents.ContainsKey(TalentID.NinjitsuNodes))
                profile.Talents[TalentID.NinjitsuNodes] = new Talent(TalentID.NinjitsuNodes) { Points = 0 };

            return (profile.Talents[TalentID.NinjitsuNodes].Points & BitFlag) != 0;
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
            profile.Talents[TalentID.NinjitsuNodes].Points |= BitFlag;

            player.SendMessage($"{Name} activated!");
            onActivate?.Invoke(player);
            return true;
        }
    }

    // Full Ninjitsu tree structure with multiple layers and over 30 nodes.
    public class NinjitsuTree
    {
        public SkillNode Root { get; }

        public NinjitsuTree(PlayerMobile player)
        {
            var profile = player.AcquireTalents();
            int nodeIndex = 0x01;

            // Layer 0: Root Node – Unlocks basic ninjitsu spells.
            Root = new SkillNode(nodeIndex, "Shadow Initiation", 5, "Unlocks the basic ninjitsu techniques", (p) =>
            {
                // Unlock basic ninjitsu spells (set bit flag 0x01 in NinjitsuSpells).
                if (!p.AcquireTalents().Talents.ContainsKey(TalentID.NinjitsuSpells))
                    p.AcquireTalents().Talents[TalentID.NinjitsuSpells] = new Talent(TalentID.NinjitsuSpells);
                p.AcquireTalents().Talents[TalentID.NinjitsuSpells].Points |= 0x01;
            });

            // Layer 1: Basic passive bonuses.
            nodeIndex <<= 1;
            var silentFootsteps = new SkillNode(nodeIndex, "Silent Footsteps", 6, "Increases stealth by making your steps quieter", (p) =>
            {
                // Increase passive stealth bonus.
                p.AcquireTalents().Talents[TalentID.NinjitsuStealth].Points += 1;
            });

            nodeIndex <<= 1;
            var quickReflexes = new SkillNode(nodeIndex, "Quick Reflexes", 6, "Enhances reaction speed", (p) =>
            {
                // Increase bonus speed.
                p.AcquireTalents().Talents[TalentID.NinjitsuSpeed].Points += 1;
            });

            nodeIndex <<= 1;
            var keenSenses = new SkillNode(nodeIndex, "Keen Senses", 6, "Improves target detection accuracy", (p) =>
            {
                // Increase detection/precision.
                p.AcquireTalents().Talents[TalentID.NinjitsuPrecision].Points += 1;
            });

            nodeIndex <<= 1;
            var agileStrike = new SkillNode(nodeIndex, "Agile Strike", 6, "Boosts base damage with swift attacks", (p) =>
            {
                // Increase base damage or attack bonus.
                p.AcquireTalents().Talents[TalentID.NinjitsuPower].Points += 1;
            });

            Root.AddChild(silentFootsteps);
            Root.AddChild(quickReflexes);
            Root.AddChild(keenSenses);
            Root.AddChild(agileStrike);

            // Layer 2: Unlock early spells and further passives.
            nodeIndex <<= 1;
            var smokeScreen = new SkillNode(nodeIndex, "Smoke Screen", 7, "Unleashes a smoke bomb to obscure vision", (p) =>
            {
                p.AcquireTalents().Talents[TalentID.NinjitsuSpells].Points |= 0x02;
            });

            nodeIndex <<= 1;
            var nimbleEvasion = new SkillNode(nodeIndex, "Nimble Evasion", 7, "Increases your ability to dodge attacks", (p) =>
            {
                p.AcquireTalents().Talents[TalentID.NinjitsuEvasion].Points += 1;
            });

            nodeIndex <<= 1;
            var cunningAmbush = new SkillNode(nodeIndex, "Cunning Ambush", 7, "Enhances damage on surprise attacks", (p) =>
            {
                p.AcquireTalents().Talents[TalentID.NinjitsuAmbush].Points += 1;
            });

            nodeIndex <<= 1;
            var stealthMastery = new SkillNode(nodeIndex, "Stealth Mastery", 7, "Unlocks deeper stealth spells", (p) =>
            {
                p.AcquireTalents().Talents[TalentID.NinjitsuSpells].Points |= 0x04;
            });

            silentFootsteps.AddChild(smokeScreen);
            quickReflexes.AddChild(nimbleEvasion);
            keenSenses.AddChild(cunningAmbush);
            agileStrike.AddChild(stealthMastery);

            // Layer 3: Advanced spells and bonuses.
            nodeIndex <<= 1;
            var shadowStep = new SkillNode(nodeIndex, "Shadow Step", 8, "Allows you to teleport short distances", (p) =>
            {
                p.AcquireTalents().Talents[TalentID.NinjitsuSpells].Points |= 0x08;
            });

            nodeIndex <<= 1;
            var bladeDance = new SkillNode(nodeIndex, "Blade Dance", 8, "Increases attack speed dramatically", (p) =>
            {
                p.AcquireTalents().Talents[TalentID.NinjitsuSpeed].Points += 1;
            });

            nodeIndex <<= 1;
            var silentKill = new SkillNode(nodeIndex, "Silent Kill", 8, "Boosts critical strike potential", (p) =>
            {
                p.AcquireTalents().Talents[TalentID.NinjitsuPrecision].Points += 1;
            });

            nodeIndex <<= 1;
            var ghostlyPresence = new SkillNode(nodeIndex, "Ghostly Presence", 8, "Enhances overall stealth capabilities", (p) =>
            {
                p.AcquireTalents().Talents[TalentID.NinjitsuStealth].Points += 1;
            });

            smokeScreen.AddChild(shadowStep);
            nimbleEvasion.AddChild(bladeDance);
            cunningAmbush.AddChild(silentKill);
            stealthMastery.AddChild(ghostlyPresence);

            // Layer 4: More unlocking and passives.
            nodeIndex <<= 1;
            // Modified: Now unlocks spell flag 0x4000 instead of a passive bonus.
            var nightsVeil = new SkillNode(nodeIndex, "Night's Veil", 9, "Unlocks a spell that enhances evasion in darkness", (p) =>
            {
                p.AcquireTalents().Talents[TalentID.NinjitsuSpells].Points |= 0x4000;
            });

            nodeIndex <<= 1;
            var phantomStrike = new SkillNode(nodeIndex, "Phantom Strike", 9, "Unleashes a sudden, ghostly attack", (p) =>
            {
                p.AcquireTalents().Talents[TalentID.NinjitsuSpells].Points |= 0x10;
            });

            nodeIndex <<= 1;
            var shurikenFlurry = new SkillNode(nodeIndex, "Shuriken Flurry", 9, "Increases ranged attack bonus", (p) =>
            {
                p.AcquireTalents().Talents[TalentID.NinjitsuPrecision].Points += 1;
            });

            nodeIndex <<= 1;
            var eclipse = new SkillNode(nodeIndex, "Eclipse", 9, "Passively increases stealth effectiveness", (p) =>
            {
                p.AcquireTalents().Talents[TalentID.NinjitsuStealth].Points += 1;
            });

            shadowStep.AddChild(nightsVeil);
            bladeDance.AddChild(phantomStrike);
            silentKill.AddChild(shurikenFlurry);
            ghostlyPresence.AddChild(eclipse);

            // Layer 5: Expert-level nodes.
            nodeIndex <<= 1;
            var vanish = new SkillNode(nodeIndex, "Vanish", 10, "Unlocks a spell to completely vanish from sight", (p) =>
            {
                p.AcquireTalents().Talents[TalentID.NinjitsuSpells].Points |= 0x20;
            });

            nodeIndex <<= 1;
            var spectralClones = new SkillNode(nodeIndex, "Spectral Clones", 10, "Creates decoys to confuse enemies", (p) =>
            {
                p.AcquireTalents().Talents[TalentID.NinjitsuSpells].Points |= 0x40;
            });

            nodeIndex <<= 1;
            var crimsonDagger = new SkillNode(nodeIndex, "Crimson Dagger", 10, "Unleashes a high-damage blade technique", (p) =>
            {
                p.AcquireTalents().Talents[TalentID.NinjitsuSpells].Points |= 0x80;
            });

            nodeIndex <<= 1;
            var windOfShadows = new SkillNode(nodeIndex, "Wind of Shadows", 10, "Passively increases movement speed", (p) =>
            {
                p.AcquireTalents().Talents[TalentID.NinjitsuSpeed].Points += 1;
            });

            nightsVeil.AddChild(vanish);
            phantomStrike.AddChild(spectralClones);
            shurikenFlurry.AddChild(crimsonDagger);
            eclipse.AddChild(windOfShadows);

            // Layer 6: Mastery nodes.
            nodeIndex <<= 1;
            var umbraMastery = new SkillNode(nodeIndex, "Umbra Mastery", 11, "Passively boosts all ninjitsu abilities", (p) =>
            {
                p.AcquireTalents().Talents[TalentID.NinjitsuPower].Points += 1;
            });

            nodeIndex <<= 1;
            var darkInsight = new SkillNode(nodeIndex, "Dark Insight", 11, "Unlocks advanced detection spells", (p) =>
            {
                p.AcquireTalents().Talents[TalentID.NinjitsuSpells].Points |= 0x100;
            });

            nodeIndex <<= 1;
            var silentFury = new SkillNode(nodeIndex, "Silent Fury", 11, "Increases overall attack bonus", (p) =>
            {
                p.AcquireTalents().Talents[TalentID.NinjitsuPower].Points += 1;
            });

            nodeIndex <<= 1;
            var shadowsEdge = new SkillNode(nodeIndex, "Shadow's Edge", 11, "Unlocks a hidden blade ability", (p) =>
            {
                p.AcquireTalents().Talents[TalentID.NinjitsuSpells].Points |= 0x200;
            });

            vanish.AddChild(umbraMastery);
            spectralClones.AddChild(darkInsight);
            crimsonDagger.AddChild(silentFury);
            windOfShadows.AddChild(shadowsEdge);

            // Layer 7: Final bonus nodes.
            nodeIndex <<= 1;
            // Modified: Now unlocks spell flag 0x8000 instead of a passive bonus.
            var phantomGuard = new SkillNode(nodeIndex, "Phantom Guard", 12, "Unlocks a defensive spell", (p) =>
            {
                p.AcquireTalents().Talents[TalentID.NinjitsuSpells].Points |= 0x8000;
            });

            nodeIndex <<= 1;
            var silentThunder = new SkillNode(nodeIndex, "Silent Thunder", 12, "Unlocks an area stun spell", (p) =>
            {
                p.AcquireTalents().Talents[TalentID.NinjitsuSpells].Points |= 0x400;
            });

            nodeIndex <<= 1;
            var midnightAssault = new SkillNode(nodeIndex, "Midnight Assault", 12, "Passively increases surprise attack damage", (p) =>
            {
                p.AcquireTalents().Talents[TalentID.NinjitsuAmbush].Points += 1;
            });

            nodeIndex <<= 1;
            var voidStrike = new SkillNode(nodeIndex, "Void Strike", 12, "Unlocks a spell that deals void damage", (p) =>
            {
                p.AcquireTalents().Talents[TalentID.NinjitsuSpells].Points |= 0x800;
            });

            phantomGuard.AddChild(silentThunder);
            silentFury.AddChild(midnightAssault);
            shadowsEdge.AddChild(voidStrike);

            // Layer 8: Ultimate node.
            nodeIndex <<= 1;
            var masterOfShadows = new SkillNode(nodeIndex, "Master of Shadows", 13, "Ultimate bonus: boosts all ninjitsu spells and passives", (p) =>
            {
                // Grants final bonus to spells and passives.
                p.AcquireTalents().Talents[TalentID.NinjitsuSpells].Points |= 0x1000 | 0x2000;
                p.AcquireTalents().Talents[TalentID.NinjitsuStealth].Points += 1;
                p.AcquireTalents().Talents[TalentID.NinjitsuSpeed].Points += 1;
                p.AcquireTalents().Talents[TalentID.NinjitsuPower].Points += 1;
            });

            // Attach the ultimate node to all Layer 7 nodes.
            foreach (var node in new[] { phantomGuard, silentThunder, midnightAssault, voidStrike })
            {
                node.AddChild(masterOfShadows);
            }
        }
    }

    // Command to open the Ninjitsu Skill Tree.
    public class NinjitsuSkillTreeCommand
    {
        public static void Initialize()
        {
            CommandSystem.Register("NinjaTree", AccessLevel.Player, cmd => ShowTree(cmd.Mobile));
        }

        private static void ShowTree(Mobile m)
        {
            if (m is PlayerMobile pm)
            {
                pm.SendMessage("Opening Ninjitsu Skill Tree...");
                pm.SendGump(new NinjitsuSkillTree(pm));
            }
            else
            {
                m.SendMessage("You must be a player to use this command.");
            }
        }
    }
}
