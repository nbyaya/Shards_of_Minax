using System;
using System.Collections.Generic;
using System.Drawing;
using Server;
using Server.Commands;
using Server.Gumps;
using Server.Mobiles;
using VitaNex.Items;
using VitaNex.SuperGumps;
using Server.Mobiles; // For TalentProfile access

namespace Server.ACC.CSS.Systems.TrackingMagic
{
    // Revised Tracking Skill Tree Gump using AncientKnowledge (Maxxia Points) as the cost resource.
    public class TrackingSkillTree : SuperGump
    {
        private TrackingTree tree;
        private Dictionary<SkillNode, Point2D> nodePositions;
        private Dictionary<int, SkillNode> buttonNodeMap;
        private Dictionary<SkillNode, int> edgeThickness;
        private const int buttonSize = 15;
        private int ySpacing = 50, xSpacing = 60;
        private int rootX = 300, rootY = 150;
        private SkillNode selectedNode;

        public TrackingSkillTree(PlayerMobile user)
            : base(user, null, 100, 100)
        {
            tree = new TrackingTree(user);
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
            layout.Add("title", () => { AddLabel(100, 20, 1153, "Tracking Skill Tree"); });

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

    // Revised SkillNode – same as the Lumberjacking version but used for Tracking.
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
            if (!profile.Talents.ContainsKey(TalentID.TrackingNodes))
                profile.Talents[TalentID.TrackingNodes] = new Talent(TalentID.TrackingNodes) { Points = 0 };
            return (profile.Talents[TalentID.TrackingNodes].Points & BitFlag) != 0;
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
            profile.Talents[TalentID.TrackingNodes].Points |= BitFlag;

            player.SendMessage($"{Name} activated!");
            onActivate?.Invoke(player);
            return true;
        }
    }

    // Full Tracking tree structure – 30 nodes split into layers.
    public class TrackingTree
    {
        public SkillNode Root { get; }

        public TrackingTree(PlayerMobile player)
        {
            var profile = player.AcquireTalents();
            int nodeIndex = 0x01;

            // Layer 0: Root Node – Unlocks basic tracking spells.
            // Spell 1: 0x01
            Root = new SkillNode(nodeIndex, "Eyes of the Wild", 5, "Unlocks basic tracking spells", (p) =>
            {
                profile.Talents[TalentID.TrackingSpells].Points |= 0x01;
            });

            // Layer 1: Basic bonuses.
            nodeIndex <<= 1;
            // Spell 2: 0x02 (changed from bonus to spell unlock)
            var trailSense = new SkillNode(nodeIndex, "Trail Sense", 6, "Unlocks a tracking spell", (p) =>
            {
                profile.Talents[TalentID.TrackingSpells].Points |= 0x02;
            });

            nodeIndex <<= 1;
            // Remains passive: +1 tracking stealth
            var silentStep = new SkillNode(nodeIndex, "Silent Step", 6, "Enhances detection of hidden movement", (p) =>
            {
                profile.Talents[TalentID.TrackingStealth].Points += 1;
            });

            nodeIndex <<= 1;
            // Remains passive: +1 tracking detection
            var pathfindersIntuition = new SkillNode(nodeIndex, "Pathfinder's Intuition", 6, "Improves detection of subtle clues", (p) =>
            {
                profile.Talents[TalentID.TrackingDetection].Points += 1;
            });

            nodeIndex <<= 1;
            // Spell 3: 0x04
            var predatorsFocus = new SkillNode(nodeIndex, "Predator's Focus", 6, "Unlocks an extra tracking spell", (p) =>
            {
                profile.Talents[TalentID.TrackingSpells].Points |= 0x04;
            });

            Root.AddChild(trailSense);
            Root.AddChild(silentStep);
            Root.AddChild(pathfindersIntuition);
            Root.AddChild(predatorsFocus);

            // Layer 2: Advanced magical and practical bonuses.
            nodeIndex <<= 1;
            // Spell 4: 0x08
            var naturesClues = new SkillNode(nodeIndex, "Nature's Clues", 7, "Unlocks additional tracking spells", (p) =>
            {
                profile.Talents[TalentID.TrackingSpells].Points |= 0x08;
            });

            nodeIndex <<= 1;
            // Spell 5: 0x10 (converted from bonus)
            var windWhisper = new SkillNode(nodeIndex, "Wind Whisper", 7, "Unlocks a tracking spell", (p) =>
            {
                profile.Talents[TalentID.TrackingSpells].Points |= 0x10;
            });

            nodeIndex <<= 1;
            // Remains passive: +1 tracking stealth
            var shadowVeil = new SkillNode(nodeIndex, "Shadow Veil", 7, "Further enhances hidden detection", (p) =>
            {
                profile.Talents[TalentID.TrackingStealth].Points += 1;
            });

            nodeIndex <<= 1;
            // Remains passive: +1 tracking detection
            var beastsInstinct = new SkillNode(nodeIndex, "Beast's Instinct", 7, "Improves detection of subtle clues", (p) =>
            {
                profile.Talents[TalentID.TrackingDetection].Points += 1;
            });

            trailSense.AddChild(naturesClues);
            silentStep.AddChild(shadowVeil);
            pathfindersIntuition.AddChild(beastsInstinct);
            predatorsFocus.AddChild(windWhisper);

            // Layer 3: Further enhancements.
            nodeIndex <<= 1;
            // Spell 6: 0x20 (reassigned from original 0x10)
            var huntersEye = new SkillNode(nodeIndex, "Hunter's Eye", 8, "Unlocks a tracking spell", (p) =>
            {
                profile.Talents[TalentID.TrackingSpells].Points |= 0x20;
            });

            nodeIndex <<= 1;
            // Spell 15: 0x4000 (converted from bonus)
            var swiftPursuit = new SkillNode(nodeIndex, "Swift Pursuit", 8, "Unlocks a tracking spell", (p) =>
            {
                profile.Talents[TalentID.TrackingSpells].Points |= 0x4000;
            });

            nodeIndex <<= 1;
            // Remains passive: +1 tracking stealth
            var silentPredator = new SkillNode(nodeIndex, "Silent Predator", 8, "Further boosts hidden detection", (p) =>
            {
                profile.Talents[TalentID.TrackingStealth].Points += 1;
            });

            nodeIndex <<= 1;
            // Remains passive: +1 tracking detection
            var furtiveSteps = new SkillNode(nodeIndex, "Furtive Steps", 8, "Enhances clue detection", (p) =>
            {
                profile.Talents[TalentID.TrackingDetection].Points += 1;
            });

            naturesClues.AddChild(huntersEye);
            windWhisper.AddChild(swiftPursuit);
            shadowVeil.AddChild(silentPredator);
            beastsInstinct.AddChild(furtiveSteps);

            // Layer 4: More advanced magical enhancements.
            nodeIndex <<= 1;
            // Spell 7: 0x40 (reassigned from original 0x20)
            var primalTracker = new SkillNode(nodeIndex, "Primal Tracker", 9, "Unlocks a tracking spell", (p) =>
            {
                profile.Talents[TalentID.TrackingSpells].Points |= 0x40;
            });

            nodeIndex <<= 1;
            // Remains passive: +1 tracking range
            var rangersEndurance = new SkillNode(nodeIndex, "Ranger's Endurance", 9, "Further increases tracking range", (p) =>
            {
                profile.Talents[TalentID.TrackingRange].Points += 1;
            });

            nodeIndex <<= 1;
            // Spell 16: 0x8000 (converted from bonus)
            var ghostWalker = new SkillNode(nodeIndex, "Ghost Walker", 9, "Unlocks a tracking spell", (p) =>
            {
                profile.Talents[TalentID.TrackingSpells].Points |= 0x8000;
            });

            nodeIndex <<= 1;
            // Remains passive: +1 tracking detection
            var clairvoyant = new SkillNode(nodeIndex, "Clairvoyant", 9, "Improves subtle clue detection", (p) =>
            {
                profile.Talents[TalentID.TrackingDetection].Points += 1;
            });

            huntersEye.AddChild(primalTracker);
            swiftPursuit.AddChild(rangersEndurance);
            silentPredator.AddChild(ghostWalker);
            furtiveSteps.AddChild(clairvoyant);

            // Layer 5: Expert-level nodes.
            nodeIndex <<= 1;
            // Spell 8: 0x80 (reassigned from original 0x40)
            var predatorsInstinct = new SkillNode(nodeIndex, "Predator's Instinct", 10, "Unlocks a tracking spell", (p) =>
            {
                profile.Talents[TalentID.TrackingSpells].Points |= 0x80;
            });

            nodeIndex <<= 1;
            // Remains passive: +1 tracking range
            var fleetfoot = new SkillNode(nodeIndex, "Fleetfoot", 10, "Further increases tracking range", (p) =>
            {
                profile.Talents[TalentID.TrackingRange].Points += 1;
            });

            nodeIndex <<= 1;
            // Remains passive: +1 tracking stealth
            var silentProwler = new SkillNode(nodeIndex, "Silent Prowler", 10, "Further boosts hidden detection", (p) =>
            {
                profile.Talents[TalentID.TrackingStealth].Points += 1;
            });

            nodeIndex <<= 1;
            // Remains passive: +1 tracking detection
            var sharpSenses = new SkillNode(nodeIndex, "Sharp Senses", 10, "Further enhances clue detection", (p) =>
            {
                profile.Talents[TalentID.TrackingDetection].Points += 1;
            });

            primalTracker.AddChild(predatorsInstinct);
            rangersEndurance.AddChild(fleetfoot);
            ghostWalker.AddChild(silentProwler);
            clairvoyant.AddChild(sharpSenses);

            // Layer 6: Mastery nodes.
            nodeIndex <<= 1;
            // Spell 9: 0x100 (reassigned from original 0x80)
            var eaglesEye = new SkillNode(nodeIndex, "Eagle Eye", 11, "Unlocks a tracking spell", (p) =>
            {
                profile.Talents[TalentID.TrackingSpells].Points |= 0x100;
            });

            nodeIndex <<= 1;
            // Remains passive: +1 tracking range
            var boundlessHorizon = new SkillNode(nodeIndex, "Boundless Horizon", 11, "Increases tracking range further", (p) =>
            {
                profile.Talents[TalentID.TrackingRange].Points += 1;
            });

            nodeIndex <<= 1;
            // Remains passive: +1 tracking stealth
            var veiledPresence = new SkillNode(nodeIndex, "Veiled Presence", 11, "Enhances hidden detection further", (p) =>
            {
                profile.Talents[TalentID.TrackingStealth].Points += 1;
            });

            nodeIndex <<= 1;
            // Remains passive: +1 tracking detection
            var intuitiveTracking = new SkillNode(nodeIndex, "Intuitive Tracking", 11, "Improves clue detection further", (p) =>
            {
                profile.Talents[TalentID.TrackingDetection].Points += 1;
            });

            predatorsInstinct.AddChild(eaglesEye);
            fleetfoot.AddChild(boundlessHorizon);
            silentProwler.AddChild(veiledPresence);
            sharpSenses.AddChild(intuitiveTracking);

            // Layer 7: Final, pinnacle bonuses.
            nodeIndex <<= 1;
            // Spell 10: 0x200 (reassigned from original 0x100)
            var spiritOfTheHunt = new SkillNode(nodeIndex, "Spirit of the Hunt", 12, "Unlocks a tracking spell", (p) =>
            {
                profile.Talents[TalentID.TrackingSpells].Points |= 0x200;
            });

            nodeIndex <<= 1;
            // Spell 11: 0x400 (converted from bonus)
            var naturesEndowment = new SkillNode(nodeIndex, "Nature's Endowment", 12, "Unlocks a tracking spell", (p) =>
            {
                profile.Talents[TalentID.TrackingSpells].Points |= 0x400;
            });

            nodeIndex <<= 1;
            // Spell 12: 0x800 (converted from bonus)
            var silentShadows = new SkillNode(nodeIndex, "Silent Shadows", 12, "Unlocks a tracking spell", (p) =>
            {
                profile.Talents[TalentID.TrackingSpells].Points |= 0x800;
            });

            nodeIndex <<= 1;
            // Spell 13: 0x1000 (converted from bonus)
            var farsight = new SkillNode(nodeIndex, "Farsight", 12, "Unlocks a tracking spell", (p) =>
            {
                profile.Talents[TalentID.TrackingSpells].Points |= 0x1000;
            });

            predatorsInstinct.AddChild(spiritOfTheHunt);
            rangersEndurance.AddChild(naturesEndowment);
            ghostWalker.AddChild(silentShadows);
            clairvoyant.AddChild(farsight);

            // Layer 8: Ultimate node.
            nodeIndex <<= 1;
            // Spell 14: 0x2000 (changed to unlock a single spell)
            var ultimateTracker = new SkillNode(nodeIndex, "Ultimate Tracker", 13, "Ultimate bonus: boosts all tracking spells and passive abilities", (p) =>
            {
                profile.Talents[TalentID.TrackingSpells].Points |= 0x2000;
            });

            // Attach the ultimate node to all Layer 7 nodes.
            foreach (var node in new[] { spiritOfTheHunt, naturesEndowment, silentShadows, farsight })
            {
                node.AddChild(ultimateTracker);
            }
        }
    }

    // Command to open the Tracking Skill Tree.
    public class TrackingSkillTreeCommand
    {
        public static void Initialize()
        {
            CommandSystem.Register("TrackTree", AccessLevel.Player, cmd => ShowTree(cmd.Mobile));
        }

        private static void ShowTree(Mobile m)
        {
            if (m is PlayerMobile pm)
            {
                pm.SendMessage("Opening Tracking Skill Tree...");
                pm.SendGump(new TrackingSkillTree(pm));
            }
            else
            {
                m.SendMessage("You must be a player to use this command.");
            }
        }
    }
}
