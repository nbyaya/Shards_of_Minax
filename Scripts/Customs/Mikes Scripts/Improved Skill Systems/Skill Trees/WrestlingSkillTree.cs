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

namespace Server.ACC.CSS.Systems.WrestlingMagic
{
    public class WrestlingSkillTree : SuperGump
    {
        private WrestlingTree tree;
        private Dictionary<SkillNode, Point2D> nodePositions;
        private Dictionary<int, SkillNode> buttonNodeMap;
        private Dictionary<SkillNode, int> edgeThickness;
        private const int buttonSize = 15;
        private int ySpacing = 50, xSpacing = 60;
        private int rootX = 300, rootY = 150;
        private SkillNode selectedNode;

        public WrestlingSkillTree(PlayerMobile user)
            : base(user, null, 100, 100)
        {
            tree = new WrestlingTree(user);
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
            layout.Add("title", () => { AddLabel(100, 20, 1153, "Wrestling Skill Tree"); });

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

    // Generic SkillNode class used by the wrestling tree.
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
            if (!profile.Talents.ContainsKey(TalentID.WrestlingNodes))
                profile.Talents[TalentID.WrestlingNodes] = new Talent(TalentID.WrestlingNodes) { Points = 0 };

            return (profile.Talents[TalentID.WrestlingNodes].Points & BitFlag) != 0;
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
            profile.Talents[TalentID.WrestlingNodes].Points |= BitFlag;

            player.SendMessage($"{Name} activated!");
            onActivate?.Invoke(player);
            return true;
        }
    }

    // The WrestlingTree builds the full tree (30 nodes: 1 root, 7 layers of 4 nodes each, plus an ultimate node).
    public class WrestlingTree
    {
        public SkillNode Root { get; }

        public WrestlingTree(PlayerMobile player)
        {
            var profile = player.AcquireTalents();
            int nodeIndex = 0x01;

            // Layer 0: Root Node – Unlocks basic wrestling moves.
            Root = new SkillNode(nodeIndex, "Call of the Arena", 5, "Unlocks basic wrestling moves", (p) =>
            {
                profile.Talents[TalentID.WrestlingSpells].Points |= 0x01;
            });

            // Layer 1: Basic bonuses/spell unlocks.
            nodeIndex <<= 1;
            var grappleGrip = new SkillNode(nodeIndex, "Grapple Grip", 6, "Unlocks a new wrestling spell", (p) =>
            {
                profile.Talents[TalentID.WrestlingSpells].Points |= 0x02;
            });

            nodeIndex <<= 1;
            var agileFootwork = new SkillNode(nodeIndex, "Agile Footwork", 6, "Unlocks a new wrestling spell", (p) =>
            {
                profile.Talents[TalentID.WrestlingSpells].Points |= 0x400;
            });

            nodeIndex <<= 1;
            var slickMoves = new SkillNode(nodeIndex, "Slick Moves", 6, "Unlocks a new wrestling spell", (p) =>
            {
                profile.Talents[TalentID.WrestlingSpells].Points |= 0x04;
            });

            nodeIndex <<= 1;
            var ringPresence = new SkillNode(nodeIndex, "Ring Presence", 6, "Unlocks a new wrestling spell", (p) =>
            {
                profile.Talents[TalentID.WrestlingSpells].Points |= 0x800;
            });

            Root.AddChild(grappleGrip);
            Root.AddChild(agileFootwork);
            Root.AddChild(slickMoves);
            Root.AddChild(ringPresence);

            // Layer 2: Advanced moves and bonuses.
            nodeIndex <<= 1;
            var crowdRoar = new SkillNode(nodeIndex, "Crowd Roar", 7, "Unlocks a new wrestling spell", (p) =>
            {
                profile.Talents[TalentID.WrestlingSpells].Points |= 0x08;
            });

            nodeIndex <<= 1;
            var momentumShift = new SkillNode(nodeIndex, "Momentum Shift", 7, "Unlocks a new wrestling spell", (p) =>
            {
                profile.Talents[TalentID.WrestlingSpells].Points |= 0x1000;
            });

            nodeIndex <<= 1;
            var bodySlam = new SkillNode(nodeIndex, "Body Slam", 7, "Unlocks a new wrestling spell", (p) =>
            {
                profile.Talents[TalentID.WrestlingSpells].Points |= 0x10;
            });

            nodeIndex <<= 1;
            var matMastery = new SkillNode(nodeIndex, "Mat Mastery", 7, "Unlocks a new wrestling spell", (p) =>
            {
                profile.Talents[TalentID.WrestlingSpells].Points |= 0x2000;
            });

            grappleGrip.AddChild(crowdRoar);
            agileFootwork.AddChild(momentumShift);
            slickMoves.AddChild(bodySlam);
            ringPresence.AddChild(matMastery);

            // Layer 3: Specialized moves and defenses.
            nodeIndex <<= 1;
            var pinningPower = new SkillNode(nodeIndex, "Pinning Power", 8, "Boosts pinning ability", (p) =>
            {
                profile.Talents[TalentID.WrestlingPower].Points += 1;
            });

            nodeIndex <<= 1;
            var takedownTechnique = new SkillNode(nodeIndex, "Takedown Technique", 8, "Improves takedown efficiency", (p) =>
            {
                profile.Talents[TalentID.WrestlingTechnique].Points += 1;
            });

            nodeIndex <<= 1;
            var submissionSavvy = new SkillNode(nodeIndex, "Submission Savvy", 8, "Unlocks a new wrestling spell", (p) =>
            {
                profile.Talents[TalentID.WrestlingSpells].Points |= 0x20;
            });

            nodeIndex <<= 1;
            var grapplingGuard = new SkillNode(nodeIndex, "Grappling Guard", 8, "Enhances defensive abilities", (p) =>
            {
                profile.Talents[TalentID.WrestlingAgility].Points += 1;
            });

            crowdRoar.AddChild(pinningPower);
            momentumShift.AddChild(takedownTechnique);
            bodySlam.AddChild(submissionSavvy);
            matMastery.AddChild(grapplingGuard);

            // Layer 4: Intermediate enhancements.
            nodeIndex <<= 1;
            var ringCommander = new SkillNode(nodeIndex, "Ring Commander", 9, "Enhances stamina", (p) =>
            {
                profile.Talents[TalentID.WrestlingStamina].Points += 1;
            });

            nodeIndex <<= 1;
            var adrenalineRush = new SkillNode(nodeIndex, "Adrenaline Rush", 9, "Unlocks a new wrestling spell", (p) =>
            {
                profile.Talents[TalentID.WrestlingSpells].Points |= 0x40;
            });

            nodeIndex <<= 1;
            var wrestlingWisdom = new SkillNode(nodeIndex, "Wrestling Wisdom", 9, "Unlocks a new wrestling spell", (p) =>
            {
                profile.Talents[TalentID.WrestlingSpells].Points |= 0x4000;
            });

            nodeIndex <<= 1;
            var ironWill = new SkillNode(nodeIndex, "Iron Will", 9, "Increases resilience", (p) =>
            {
                profile.Talents[TalentID.WrestlingStamina].Points += 1;
            });

            ringCommander.AddChild(adrenalineRush);
            adrenalineRush.AddChild(wrestlingWisdom);
            grapplingGuard.AddChild(ironWill);

            // Layer 5: Advanced power moves.
            nodeIndex <<= 1;
            var ultimateGrip = new SkillNode(nodeIndex, "Ultimate Grip", 10, "Unlocks a new wrestling spell", (p) =>
            {
                profile.Talents[TalentID.WrestlingSpells].Points |= 0x8000;
            });

            nodeIndex <<= 1;
            var brutalForce = new SkillNode(nodeIndex, "Brutal Force", 10, "Increases damage", (p) =>
            {
                profile.Talents[TalentID.WrestlingPower].Points += 1;
            });

            nodeIndex <<= 1;
            var pinMastery = new SkillNode(nodeIndex, "Pin Mastery", 10, "Unlocks a new wrestling spell", (p) =>
            {
                profile.Talents[TalentID.WrestlingSpells].Points |= 0x80;
            });

            nodeIndex <<= 1;
            var momentumSurge = new SkillNode(nodeIndex, "Momentum Surge", 10, "Further boosts momentum", (p) =>
            {
                profile.Talents[TalentID.WrestlingPower].Points += 1;
            });

            ringCommander.AddChild(ultimateGrip);
            brutalForce.AddChild(pinMastery);
            wrestlingWisdom.AddChild(momentumSurge);

            // Layer 6: Expert techniques.
            nodeIndex <<= 1;
            var expandedReach = new SkillNode(nodeIndex, "Expanded Reach", 11, "Increases grappling range", (p) =>
            {
                profile.Talents[TalentID.WrestlingAgility].Points += 1;
            });

            nodeIndex <<= 1;
            var explosiveImpact = new SkillNode(nodeIndex, "Explosive Impact", 11, "Boosts move impact", (p) =>
            {
                profile.Talents[TalentID.WrestlingPower].Points += 1;
            });

            nodeIndex <<= 1;
            var ancientTechnique = new SkillNode(nodeIndex, "Ancient Technique", 11, "Unlocks a new wrestling spell", (p) =>
            {
                profile.Talents[TalentID.WrestlingSpells].Points |= 0x100;
            });

            nodeIndex <<= 1;
            var reboundReflex = new SkillNode(nodeIndex, "Rebound Reflex", 11, "Enhances counter-attack", (p) =>
            {
                profile.Talents[TalentID.WrestlingAgility].Points += 1;
            });

            ultimateGrip.AddChild(expandedReach);
            pinMastery.AddChild(explosiveImpact);
            momentumSurge.AddChild(ancientTechnique);
            expandedReach.AddChild(reboundReflex);

            // Layer 7: Mastery bonuses.
            nodeIndex <<= 1;
            var ringGuardian = new SkillNode(nodeIndex, "Ring Guardian", 12, "Provides defensive bonus", (p) =>
            {
                profile.Talents[TalentID.WrestlingStamina].Points += 1;
            });

            nodeIndex <<= 1;
            var crowdAdoration = new SkillNode(nodeIndex, "Crowd Adoration", 12, "Enhances technical skill", (p) =>
            {
                profile.Talents[TalentID.WrestlingTechnique].Points += 1;
            });

            nodeIndex <<= 1;
            var savageResilience = new SkillNode(nodeIndex, "Savage Resilience", 12, "Increases health bonus", (p) =>
            {
                profile.Talents[TalentID.WrestlingStamina].Points += 1;
            });

            nodeIndex <<= 1;
            var fightingInstinct = new SkillNode(nodeIndex, "Fighting Instinct", 12, "Boosts critical chance", (p) =>
            {
                profile.Talents[TalentID.WrestlingAgility].Points += 1;
            });

            ringGuardian.AddChild(crowdAdoration);
            crowdAdoration.AddChild(savageResilience);
            savageResilience.AddChild(fightingInstinct);

            // Layer 8: Ultimate node.
            nodeIndex <<= 1;
            var championOfTheRing = new SkillNode(nodeIndex, "Champion of the Ring", 13, "Unlocks a new wrestling spell", (p) =>
            {
                profile.Talents[TalentID.WrestlingSpells].Points |= 0x200;
            });

            ringGuardian.AddChild(championOfTheRing);
        }
    }

    // Command to open the Wrestling Skill Tree.
    public class WrestlingSkillTreeCommand
    {
        public static void Initialize()
        {
            CommandSystem.Register("WrestleTree", AccessLevel.Player, cmd => ShowTree(cmd.Mobile));
        }

        private static void ShowTree(Mobile m)
        {
            if (m is PlayerMobile pm)
            {
                pm.SendMessage("Opening Wrestling Skill Tree...");
                pm.SendGump(new WrestlingSkillTree(pm));
            }
            else
            {
                m.SendMessage("You must be a player to use this command.");
            }
        }
    }
}
