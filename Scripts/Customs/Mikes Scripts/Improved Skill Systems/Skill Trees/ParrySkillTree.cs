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

namespace Server.ACC.CSS.Systems.ParryMagic
{
    public class ParrySkillTree : SuperGump
    {
        private ParryTree tree;
        private Dictionary<SkillNode, Point2D> nodePositions;
        private Dictionary<int, SkillNode> buttonNodeMap;
        private Dictionary<SkillNode, int> edgeThickness;
        private const int buttonSize = 15;
        private int ySpacing = 50, xSpacing = 60;
        private int rootX = 300, rootY = 150;
        private SkillNode selectedNode;

        public ParrySkillTree(PlayerMobile user)
            : base(user, null, 100, 100)
        {
            tree = new ParryTree(user);
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
            layout.Add("title", () => { AddLabel(100, 20, 1153, "Parry Skill Tree"); });

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

    // The SkillNode class used for the Parry tree.
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
            if (!profile.Talents.ContainsKey(TalentID.ParryNodes))
                profile.Talents[TalentID.ParryNodes] = new Talent(TalentID.ParryNodes) { Points = 0 };

            return (profile.Talents[TalentID.ParryNodes].Points & BitFlag) != 0;
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
            profile.Talents[TalentID.ParryNodes].Points |= BitFlag;

            player.SendMessage($"{Name} activated!");
            onActivate?.Invoke(player);
            return true;
        }
    }

    // The full Parry tree structure with 30 nodes.
    public class ParryTree
    {
        public SkillNode Root { get; }

        public ParryTree(PlayerMobile player)
        {
            var profile = player.AcquireTalents();
            int nodeIndex = 0x01;

            // Layer 0: Root Node – Unlocks basic parry spells.
            Root = new SkillNode(nodeIndex, "Call of the Blade", 5, "Unlocks basic parry spells", (p) =>
            {
                profile.Talents[TalentID.ParrySpells].Points |= 0x01;
            });

            // Layer 1: Basic bonuses / Spell unlocks.
            nodeIndex <<= 1;
            var swiftGuard = new SkillNode(nodeIndex, "Swift Guard", 6, "Unlocks swift guard spell", (p) =>
            {
                profile.Talents[TalentID.ParrySpells].Points |= 0x02;
            });

            nodeIndex <<= 1;
            var ironWall = new SkillNode(nodeIndex, "Iron Wall", 6, "Unlocks iron wall spell", (p) =>
            {
                profile.Talents[TalentID.ParrySpells].Points |= 0x80;
            });

            nodeIndex <<= 1;
            var deflectingParry = new SkillNode(nodeIndex, "Deflecting Parry", 6, "Unlocks deflection spells", (p) =>
            {
                profile.Talents[TalentID.ParrySpells].Points |= 0x04;
            });

            nodeIndex <<= 1;
            var riposteInstinct = new SkillNode(nodeIndex, "Riposte Instinct", 6, "Unlocks riposte instinct spell", (p) =>
            {
                profile.Talents[TalentID.ParrySpells].Points |= 0x10;
            });

            Root.AddChild(swiftGuard);
            Root.AddChild(ironWall);
            Root.AddChild(deflectingParry);
            Root.AddChild(riposteInstinct);

            // Layer 2: Advanced bonuses.
            nodeIndex <<= 1;
            var bladeDance = new SkillNode(nodeIndex, "Blade Dance", 7, "Unlocks advanced parry moves", (p) =>
            {
                profile.Talents[TalentID.ParrySpells].Points |= 0x08;
            });

            nodeIndex <<= 1;
            var counterStance = new SkillNode(nodeIndex, "Counter Stance", 7, "Unlocks counter stance spell", (p) =>
            {
                profile.Talents[TalentID.ParrySpells].Points |= 0x400;
            });

            nodeIndex <<= 1;
            var guardiansResolve = new SkillNode(nodeIndex, "Guardian's Resolve", 7, "Improves reaction to attacks", (p) =>
            {
                profile.Talents[TalentID.ParryBlock].Points += 1;
            });

            nodeIndex <<= 1;
            var defendersInsight = new SkillNode(nodeIndex, "Defender's Insight", 7, "Improves reaction to attacks", (p) =>
            {
                profile.Talents[TalentID.ParryAgility].Points += 1;
            });

            swiftGuard.AddChild(bladeDance);
            ironWall.AddChild(counterStance);
            deflectingParry.AddChild(guardiansResolve);
            riposteInstinct.AddChild(defendersInsight);

            // Layer 3: Yield and efficiency improvements.
            nodeIndex <<= 1;
            var vigilantWatch = new SkillNode(nodeIndex, "Vigilant Watch", 8, "Unlocks vigilant watch spell", (p) =>
            {
                profile.Talents[TalentID.ParrySpells].Points |= 0x800;
            });

            nodeIndex <<= 1;
            var reinforcedGuard = new SkillNode(nodeIndex, "Reinforced Guard", 8, "Unlocks reinforced guard spell", (p) =>
            {
                profile.Talents[TalentID.ParrySpells].Points |= 0x1000;
            });

            nodeIndex <<= 1;
            var counterStrike = new SkillNode(nodeIndex, "Counter Strike", 8, "Unlocks counter strike spells", (p) =>
            {
                profile.Talents[TalentID.ParrySpells].Points |= 0x20;
            });

            nodeIndex <<= 1;
            var swiftRecovery = new SkillNode(nodeIndex, "Swift Recovery", 8, "Improves recovery after parry", (p) =>
            {
                profile.Talents[TalentID.ParryStamina].Points += 1;
            });

            bladeDance.AddChild(vigilantWatch);
            counterStance.AddChild(reinforcedGuard);
            guardiansResolve.AddChild(counterStrike);
            defendersInsight.AddChild(swiftRecovery);

            // Layer 4: More advanced magical enhancements.
            nodeIndex <<= 1;
            var resoluteDefense = new SkillNode(nodeIndex, "Resolute Defense", 9, "Unlocks resolute defense spell", (p) =>
            {
                profile.Talents[TalentID.ParrySpells].Points |= 0x2000;
            });

            nodeIndex <<= 1;
            var rapidReflexes = new SkillNode(nodeIndex, "Rapid Reflexes", 9, "Unlocks rapid parry spells", (p) =>
            {
                profile.Talents[TalentID.ParrySpells].Points |= 0x40;
            });

            nodeIndex <<= 1;
            var mightyRiposte = new SkillNode(nodeIndex, "Mighty Riposte", 9, "Improves counterattack damage", (p) =>
            {
                profile.Talents[TalentID.ParryCounter].Points += 1;
            });

            nodeIndex <<= 1;
            var enduringStance = new SkillNode(nodeIndex, "Enduring Stance", 9, "Unlocks enduring stance spell", (p) =>
            {
                profile.Talents[TalentID.ParrySpells].Points |= 0x4000;
            });

            vigilantWatch.AddChild(resoluteDefense);
            reinforcedGuard.AddChild(rapidReflexes);
            counterStrike.AddChild(mightyRiposte);
            swiftRecovery.AddChild(enduringStance);

            // Layer 5: Expert-level nodes.
            nodeIndex <<= 1;
            var primeParry = new SkillNode(nodeIndex, "Prime Parry", 10, "Boosts overall parry efficiency", (p) =>
            {
                profile.Talents[TalentID.ParryAgility].Points += 1;
            });

            nodeIndex <<= 1;
            var fortifiedDefense = new SkillNode(nodeIndex, "Fortified Defense", 10, "Enhances blocking capabilities", (p) =>
            {
                profile.Talents[TalentID.ParryBlock].Points += 1;
            });

            nodeIndex <<= 1;
            var arcaneDeflection = new SkillNode(nodeIndex, "Arcane Deflection", 10, "Unlocks mastery parry spells", (p) =>
            {
                profile.Talents[TalentID.ParrySpells].Points |= 0x100;
            });

            nodeIndex <<= 1;
            var counterMomentum = new SkillNode(nodeIndex, "Counter Momentum", 10, "Increases counterattack momentum", (p) =>
            {
                profile.Talents[TalentID.ParryCounter].Points += 1;
            });

            resoluteDefense.AddChild(primeParry);
            rapidReflexes.AddChild(fortifiedDefense);
            mightyRiposte.AddChild(arcaneDeflection);
            enduringStance.AddChild(counterMomentum);

            // Layer 6: Mastery nodes.
            nodeIndex <<= 1;
            var expandedAwareness = new SkillNode(nodeIndex, "Expanded Awareness", 11, "Enhances spatial awareness", (p) =>
            {
                profile.Talents[TalentID.ParryAgility].Points += 1;
            });

            nodeIndex <<= 1;
            var staminaSurge = new SkillNode(nodeIndex, "Stamina Surge", 11, "Boosts parry endurance", (p) =>
            {
                profile.Talents[TalentID.ParryStamina].Points += 1;
            });

            nodeIndex <<= 1;
            var mysticGuard = new SkillNode(nodeIndex, "Mystic Guard", 11, "Unlocks mystic parry spells", (p) =>
            {
                profile.Talents[TalentID.ParrySpells].Points |= 0x200;
            });

            nodeIndex <<= 1;
            var flowingCounter = new SkillNode(nodeIndex, "Flowing Counter", 11, "Improves fluid counterattacks", (p) =>
            {
                profile.Talents[TalentID.ParryCounter].Points += 1;
            });

            primeParry.AddChild(expandedAwareness);
            fortifiedDefense.AddChild(staminaSurge);
            arcaneDeflection.AddChild(mysticGuard);
            counterMomentum.AddChild(flowingCounter);

            // Layer 7: Final, pinnacle bonuses.
            nodeIndex <<= 1;
            var shieldOfValor = new SkillNode(nodeIndex, "Shield of Valor", 12, "Provides a protective shield bonus", (p) =>
            {
                profile.Talents[TalentID.ParryBlock].Points += 1;
            });

            nodeIndex <<= 1;
            var endlessVigor = new SkillNode(nodeIndex, "Endless Vigor", 12, "Increases parry stamina", (p) =>
            {
                profile.Talents[TalentID.ParryStamina].Points += 1;
            });

            nodeIndex <<= 1;
            var ragingCounter = new SkillNode(nodeIndex, "Raging Counter", 12, "Enhances counter power", (p) =>
            {
                profile.Talents[TalentID.ParryCounter].Points += 1;
            });

            nodeIndex <<= 1;
            var agileEvasion = new SkillNode(nodeIndex, "Agile Evasion", 12, "Boosts agility for parrying", (p) =>
            {
                profile.Talents[TalentID.ParryAgility].Points += 1;
            });

            expandedAwareness.AddChild(shieldOfValor);
            staminaSurge.AddChild(endlessVigor);
            mysticGuard.AddChild(ragingCounter);
            flowingCounter.AddChild(agileEvasion);

            // Layer 8: Ultimate node.
            nodeIndex <<= 1;
            var ultimateParry = new SkillNode(nodeIndex, "Ultimate Parry", 13, "Ultimate bonus: boosts all parry skills", (p) =>
            {
                profile.Talents[TalentID.ParrySpells].Points |= 0x8000;
                profile.Talents[TalentID.ParryBlock].Points += 1;
                profile.Talents[TalentID.ParryAgility].Points += 1;
                profile.Talents[TalentID.ParryCounter].Points += 1;
                profile.Talents[TalentID.ParryStamina].Points += 1;
            });

            // Attach ultimate node to all Layer 7 nodes.
            foreach (var node in new[] { shieldOfValor, endlessVigor, ragingCounter, agileEvasion })
            {
                node.AddChild(ultimateParry);
            }
        }
    }

    // Command to open the Parry Skill Tree.
    public class ParrySkillTreeCommand
    {
        public static void Initialize()
        {
            CommandSystem.Register("ParryTree", AccessLevel.Player, cmd => ShowTree(cmd.Mobile));
        }

        private static void ShowTree(Mobile m)
        {
            if (m is PlayerMobile pm)
            {
                pm.SendMessage("Opening Parry Skill Tree...");
                pm.SendGump(new ParrySkillTree(pm));
            }
            else
            {
                m.SendMessage("You must be a player to use this command.");
            }
        }
    }
}
