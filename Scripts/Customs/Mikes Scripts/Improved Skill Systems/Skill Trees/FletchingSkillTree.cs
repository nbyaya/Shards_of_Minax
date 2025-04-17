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

namespace Server.ACC.CSS.Systems.FletchingMagic
{
    public class FletchingSkillTree : SuperGump
    {
        private FletchingTree tree;
        private Dictionary<SkillNode, Point2D> nodePositions;
        private Dictionary<int, SkillNode> buttonNodeMap;
        private Dictionary<SkillNode, int> edgeThickness;
        private const int buttonSize = 15;
        private int ySpacing = 50, xSpacing = 60;
        private int rootX = 300, rootY = 150;
        private SkillNode selectedNode;

        public FletchingSkillTree(PlayerMobile user)
            : base(user, null, 100, 100)
        {
            tree = new FletchingTree(user);
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

            // Position nodes at each level centered horizontally about rootX.
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
            layout.Add("title", () => { AddLabel(100, 20, 1153, "Fletching Skill Tree"); });

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
            if (!profile.Talents.ContainsKey(TalentID.FletchingNodes))
                profile.Talents[TalentID.FletchingNodes] = new Talent(TalentID.FletchingNodes) { Points = 0 };

            return (profile.Talents[TalentID.FletchingNodes].Points & BitFlag) != 0;
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
            profile.Talents[TalentID.FletchingNodes].Points |= BitFlag;

            player.SendMessage($"{Name} activated!");
            onActivate?.Invoke(player);
            return true;
        }
    }

    public class FletchingTree
    {
        public SkillNode Root { get; }

        public FletchingTree(PlayerMobile player)
        {
            var profile = player.AcquireTalents();
            int nodeIndex = 0x01;

            // --- Layer 0: Root Node ---
            // Spell Node 1: Call of the Archer – Unlocks basic fletching spells (0x01)
            Root = new SkillNode(nodeIndex, "Call of the Archer", 5, "Unlocks basic fletching spells", (p) =>
            {
                profile.Talents[TalentID.FletchingSpells].Points |= 0x01;
            });

            // --- Layer 1: Basic bonuses and spells ---
            nodeIndex <<= 1;
            // Spell Node 2: Eagle Eye – now unlocks precision arrow spells (0x02)
            var eagleEye = new SkillNode(nodeIndex, "Eagle Eye", 6, "Unlocks precision arrow spells", (p) =>
            {
                profile.Talents[TalentID.FletchingSpells].Points |= 0x02;
            });

            nodeIndex <<= 1;
            // Spell Node 13 (converted): Swift Fletching – now unlocks swift fletching spells (0x1000)
            var swiftFletching = new SkillNode(nodeIndex, "Swift Fletching", 6, "Unlocks swift fletching spells", (p) =>
            {
                profile.Talents[TalentID.FletchingSpells].Points |= 0x1000;
            });

            nodeIndex <<= 1;
            // Spell Node 3: Arrow Mastery – Unlocks bonus arrow spells (0x04)
            var arrowMastery = new SkillNode(nodeIndex, "Arrow Mastery", 6, "Unlocks bonus arrow spells", (p) =>
            {
                profile.Talents[TalentID.FletchingSpells].Points |= 0x04;
            });

            nodeIndex <<= 1;
            // Passive Node: Quiver's Bounty – Increases arrow yield
            var quiversBounty = new SkillNode(nodeIndex, "Quiver's Bounty", 6, "Increases arrow yield", (p) =>
            {
                profile.Talents[TalentID.FletchingYield].Points += 1;
            });

            Root.AddChild(eagleEye);
            Root.AddChild(swiftFletching);
            Root.AddChild(arrowMastery);
            Root.AddChild(quiversBounty);

            // --- Layer 2: Advanced magical and practical bonuses ---
            nodeIndex <<= 1;
            // Spell Node 4: Whispering Wood – Unlocks additional arrow spells (0x08)
            var whisperingWood = new SkillNode(nodeIndex, "Whispering Wood", 7, "Unlocks additional arrow spells", (p) =>
            {
                profile.Talents[TalentID.FletchingSpells].Points |= 0x08;
            });

            nodeIndex <<= 1;
            // Passive Node: Featherlight Craft – Improves crafting speed further
            var featherlightCraft = new SkillNode(nodeIndex, "Featherlight Craft", 7, "Improves crafting speed further", (p) =>
            {
                profile.Talents[TalentID.FletchingSpeed].Points += 1;
            });

            nodeIndex <<= 1;
            // Spell Node 5: Mystic Flight – Unlocks advanced arrow spells (0x10)
            var mysticFlight = new SkillNode(nodeIndex, "Mystic Flight", 7, "Unlocks advanced arrow spells", (p) =>
            {
                profile.Talents[TalentID.FletchingSpells].Points |= 0x10;
            });

            nodeIndex <<= 1;
            // Spell Node 14 (converted): Eagle's Grace – Unlocks graceful arrow spells (0x2000)
            var eaglesGrace = new SkillNode(nodeIndex, "Eagle's Grace", 7, "Unlocks graceful arrow spells", (p) =>
            {
                profile.Talents[TalentID.FletchingSpells].Points |= 0x2000;
            });

            eagleEye.AddChild(whisperingWood);
            swiftFletching.AddChild(featherlightCraft);
            arrowMastery.AddChild(mysticFlight);
            quiversBounty.AddChild(eaglesGrace);

            // --- Layer 3: Further improvements ---
            nodeIndex <<= 1;
            // Passive Node: Bountiful Quiver – Enhances arrow yield
            var bountifulQuiver = new SkillNode(nodeIndex, "Bountiful Quiver", 8, "Enhances arrow yield", (p) =>
            {
                profile.Talents[TalentID.FletchingYield].Points += 1;
            });

            nodeIndex <<= 1;
            // Passive Node: Rapid Nock – Increases crafting speed
            var rapidNock = new SkillNode(nodeIndex, "Rapid Nock", 8, "Increases crafting speed", (p) =>
            {
                profile.Talents[TalentID.FletchingSpeed].Points += 1;
            });

            nodeIndex <<= 1;
            // Spell Node 6: Steady Hand – Unlocks defensive arrow spells (0x20)
            var steadyHand = new SkillNode(nodeIndex, "Steady Hand", 8, "Unlocks defensive arrow spells", (p) =>
            {
                profile.Talents[TalentID.FletchingSpells].Points |= 0x20;
            });

            nodeIndex <<= 1;
            // Spell Node 15 (converted): Keen Focus – Unlocks focused arrow spells (0x4000)
            var keenFocus = new SkillNode(nodeIndex, "Keen Focus", 8, "Unlocks focused arrow spells", (p) =>
            {
                profile.Talents[TalentID.FletchingSpells].Points |= 0x4000;
            });

            whisperingWood.AddChild(bountifulQuiver);
            featherlightCraft.AddChild(rapidNock);
            mysticFlight.AddChild(steadyHand);
            eaglesGrace.AddChild(keenFocus);

            // --- Layer 4: More advanced magical enhancements ---
            nodeIndex <<= 1;
            // Spell Node 16 (converted): Quiver's Blessing – Unlocks blessed arrow spells (0x8000)
            var quiversBlessing = new SkillNode(nodeIndex, "Quiver's Blessing", 9, "Unlocks blessed arrow spells", (p) =>
            {
                profile.Talents[TalentID.FletchingSpells].Points |= 0x8000;
            });

            nodeIndex <<= 1;
            // Spell Node 7: Feather's Touch – Unlocks bonus arrow spells (0x40)
            var feathersTouch = new SkillNode(nodeIndex, "Feather's Touch", 9, "Unlocks bonus arrow spells", (p) =>
            {
                profile.Talents[TalentID.FletchingSpells].Points |= 0x40;
            });

            nodeIndex <<= 1;
            // Spell Node 8: Arcane Arrow – Unlocks mystical arrow spells (0x80)
            var arcaneArrow = new SkillNode(nodeIndex, "Arcane Arrow", 9, "Unlocks mystical arrow spells", (p) =>
            {
                profile.Talents[TalentID.FletchingSpells].Points |= 0x80;
            });

            nodeIndex <<= 1;
            // Passive Node: Eagle's Roar – Boosts arrow accuracy
            var eaglesRoar = new SkillNode(nodeIndex, "Eagle's Roar", 9, "Boosts arrow accuracy", (p) =>
            {
                profile.Talents[TalentID.FletchingAccuracy].Points += 1;
            });

            bountifulQuiver.AddChild(quiversBlessing);
            rapidNock.AddChild(feathersTouch);
            steadyHand.AddChild(arcaneArrow);
            keenFocus.AddChild(eaglesRoar);

            // --- Layer 5: Expert-level nodes ---
            nodeIndex <<= 1;
            // Passive Node: Primeval Precision – Boosts overall arrow accuracy
            var primevalPrecision = new SkillNode(nodeIndex, "Primeval Precision", 10, "Boosts overall arrow accuracy", (p) =>
            {
                profile.Talents[TalentID.FletchingAccuracy].Points += 1;
            });

            nodeIndex <<= 1;
            // Passive Node: Bountiful Barrage – Boosts arrow yield
            var bountifulBarrage = new SkillNode(nodeIndex, "Bountiful Barrage", 10, "Boosts arrow yield", (p) =>
            {
                profile.Talents[TalentID.FletchingYield].Points += 1;
            });

            nodeIndex <<= 1;
            // Spell Node 9: Arrow Mastery II – Unlocks mastery arrow spells (0x100)
            var arrowMasteryII = new SkillNode(nodeIndex, "Arrow Mastery II", 10, "Unlocks mastery arrow spells", (p) =>
            {
                profile.Talents[TalentID.FletchingSpells].Points |= 0x100;
            });

            nodeIndex <<= 1;
            // Passive Node: Nocking Momentum – Increases crafting speed
            var nockingMomentum = new SkillNode(nodeIndex, "Nocking Momentum", 10, "Increases crafting speed", (p) =>
            {
                profile.Talents[TalentID.FletchingSpeed].Points += 1;
            });

            quiversBlessing.AddChild(primevalPrecision);
            feathersTouch.AddChild(bountifulBarrage);
            arcaneArrow.AddChild(arrowMasteryII);
            eaglesRoar.AddChild(nockingMomentum);

            // --- Layer 6: Mastery nodes ---
            nodeIndex <<= 1;
            // Passive Node: Expanded Range – Enhances fletching range
            var expandedRange = new SkillNode(nodeIndex, "Expanded Range", 11, "Enhances fletching range", (p) =>
            {
                profile.Talents[TalentID.FletchingRange].Points += 1;
            });

            nodeIndex <<= 1;
            // Passive Node: Mystic Quiver – Boosts arrow yield with magic
            var mysticQuiver = new SkillNode(nodeIndex, "Mystic Quiver", 11, "Boosts arrow yield with magic", (p) =>
            {
                profile.Talents[TalentID.FletchingYield].Points += 1;
            });

            nodeIndex <<= 1;
            // Spell Node 10: Ancient Archer – Unlocks ancient arrow spells (0x200)
            var ancientArcher = new SkillNode(nodeIndex, "Ancient Archer", 11, "Unlocks ancient arrow spells", (p) =>
            {
                profile.Talents[TalentID.FletchingSpells].Points |= 0x200;
            });

            nodeIndex <<= 1;
            // Passive Node: Rapid Reload – Increases crafting speed
            var rapidReload = new SkillNode(nodeIndex, "Rapid Reload", 11, "Increases crafting speed", (p) =>
            {
                profile.Talents[TalentID.FletchingSpeed].Points += 1;
            });

            primevalPrecision.AddChild(expandedRange);
            bountifulBarrage.AddChild(mysticQuiver);
            arrowMasteryII.AddChild(ancientArcher);
            nockingMomentum.AddChild(rapidReload);

            // --- Layer 7: Pinnacle bonuses ---
            nodeIndex <<= 1;
            // Spell Node 11: Shield of Feathers – Unlocks protective arrow spells (0x400)
            var shieldOfFeathers = new SkillNode(nodeIndex, "Shield of Feathers", 12, "Unlocks protective arrow spells", (p) =>
            {
                profile.Talents[TalentID.FletchingSpells].Points |= 0x400;
            });

            nodeIndex <<= 1;
            // Passive Node: Quiver's Endowment – Further increases arrow yield
            var quiversEndowment = new SkillNode(nodeIndex, "Quiver's Endowment", 12, "Further increases arrow yield", (p) =>
            {
                profile.Talents[TalentID.FletchingYield].Points += 1;
            });

            nodeIndex <<= 1;
            // Passive Node: Archer's Fury – Boosts arrow accuracy
            var archersFury = new SkillNode(nodeIndex, "Archer's Fury", 12, "Boosts arrow accuracy", (p) =>
            {
                profile.Talents[TalentID.FletchingAccuracy].Points += 1;
            });

            nodeIndex <<= 1;
            // Passive Node: Echoes of The Wind – Enhances fletching range
            var echoesOfTheWind = new SkillNode(nodeIndex, "Echoes of The Wind", 12, "Enhances fletching range", (p) =>
            {
                profile.Talents[TalentID.FletchingRange].Points += 1;
            });

            expandedRange.AddChild(shieldOfFeathers);
            mysticQuiver.AddChild(quiversEndowment);
            ancientArcher.AddChild(archersFury);
            rapidReload.AddChild(echoesOfTheWind);

            // --- Layer 8: Ultimate node ---
            nodeIndex <<= 1;
            // Spell Node 12: Ultimate Fletcher – Ultimate bonus now unlocks a single spell (0x800)
            var ultimateFletcher = new SkillNode(nodeIndex, "Ultimate Fletcher", 13, "Ultimate bonus: unlocks complete spell power", (p) =>
            {
                profile.Talents[TalentID.FletchingSpells].Points |= 0x800;
                profile.Talents[TalentID.FletchingAccuracy].Points += 1;
                profile.Talents[TalentID.FletchingSpeed].Points += 1;
                profile.Talents[TalentID.FletchingYield].Points += 1;
                profile.Talents[TalentID.FletchingRange].Points += 1;
            });

            // Attach the ultimate node to all layer 7 nodes.
            foreach (var node in new[] { shieldOfFeathers, echoesOfTheWind, archersFury, quiversEndowment })
            {
                node.AddChild(ultimateFletcher);
            }
        }
    }

    public class FletchingSkillTreeCommand
    {
        public static void Initialize()
        {
            CommandSystem.Register("FletchTree", AccessLevel.Player, cmd => ShowTree(cmd.Mobile));
        }

        private static void ShowTree(Mobile m)
        {
            if (m is PlayerMobile pm)
            {
                pm.SendMessage("Opening Fletching Skill Tree...");
                pm.SendGump(new FletchingSkillTree(pm));
            }
            else
            {
                m.SendMessage("You must be a player to use this command.");
            }
        }
    }
}
