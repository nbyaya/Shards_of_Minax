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

namespace Server.ACC.CSS.Systems.BeggingMagic
{
    // Revised Begging Skill Tree Gump using AncientKnowledge (Maxxia Points) as the cost resource.
    public class BeggingSkillTree : SuperGump
    {
        private BeggingTree tree;
        private Dictionary<SkillNode, Point2D> nodePositions;
        private Dictionary<int, SkillNode> buttonNodeMap;
        private Dictionary<SkillNode, int> edgeThickness;
        private const int buttonSize = 15;
        private int ySpacing = 50, xSpacing = 60;
        private int rootX = 300, rootY = 150;
        private SkillNode selectedNode;

        public BeggingSkillTree(PlayerMobile user)
            : base(user, null, 100, 100)
        {
            tree = new BeggingTree(user);
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

            // This HashSet will ensure each node is only placed once.
            var visited = new HashSet<SkillNode>();

            queue.Enqueue((root, 0));

            while (queue.Count > 0)
            {
                var (node, level) = queue.Dequeue();

                // If we've already visited this node, skip it.
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

            // Now position each level's nodes centered around rootX
            foreach (var kvp in levelNodes)
            {
                int level = kvp.Key;
                var nodes = kvp.Value;

                // Spread them out horizontally based on how many nodes are in this level
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
            layout.Add("title", () => { AddLabel(100, 20, 1153, "Begging Skill Tree"); });

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
                    // Adjust y coordinate as needed to display below the node name.
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

    // SkillNode class for Begging. It functions the same as in Lumberjacking but uses BeggingNodes and BeggingSpells talents.
    public class SkillNode
    {
        public int BitFlag { get; }
        public string Name { get; }
        public int Cost { get; }
        public string Description { get; } // New property for description.
        public List<SkillNode> Children { get; }
        public SkillNode Parent { get; private set; }
        private readonly Action<PlayerMobile> onActivate;

        // Modified constructor to accept a description.
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
            if (!profile.Talents.ContainsKey(TalentID.BeggingNodes))
                profile.Talents[TalentID.BeggingNodes] = new Talent(TalentID.BeggingNodes) { Points = 0 };

            return (profile.Talents[TalentID.BeggingNodes].Points & BitFlag) != 0;
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

            // Use AncientKnowledge (Maxxia Points) for unlocking.
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
            profile.Talents[TalentID.BeggingNodes].Points |= BitFlag;

            player.SendMessage($"{Name} activated!");
            onActivate?.Invoke(player);
            return true;
        }
    }

    // The full Begging tree structure.
    public class BeggingTree
    {
        public SkillNode Root { get; }

        public BeggingTree(PlayerMobile player)
        {
            var profile = player.AcquireTalents();
            int nodeIndex = 0x01;

            // Layer 0: Root Node – Unlocks basic begging spells.
            Root = new SkillNode(nodeIndex, "Call of the Beggar's Way", 5, "Unlocks basic begging spells", (p) =>
            {
                profile.Talents[TalentID.BeggingSpells].Points |= 0x01;
            });

            // Layer 1: Basic bonuses.
            nodeIndex <<= 1;
            var silverTongue = new SkillNode(nodeIndex, "Silver Tongue", 6, "Increases persuasion success chance", (p) =>
            {
                profile.Talents[TalentID.BeggingPersuasion].Points += 1;
            });

            nodeIndex <<= 1;
            // Changed from a passive bonus to unlocking a spell (0x02)
            var quickAppeal = new SkillNode(nodeIndex, "Quick Appeal", 6, "Unlocks bonus begging spell", (p) =>
            {
                profile.Talents[TalentID.BeggingSpells].Points |= 0x02;
            });

            nodeIndex <<= 1;
            var empatheticGaze = new SkillNode(nodeIndex, "Empathetic Gaze", 6, "Unlocks bonus begging spell", (p) =>
            {
                profile.Talents[TalentID.BeggingSpells].Points |= 0x04;
            });

            nodeIndex <<= 1;
            var heartfeltRequest = new SkillNode(nodeIndex, "Heartfelt Request", 6, "Increases chance for bonus rewards", (p) =>
            {
                profile.Talents[TalentID.BeggingLuck].Points += 1;
            });

            // Attach Layer 1 nodes.
            Root.AddChild(silverTongue);
            Root.AddChild(quickAppeal);
            Root.AddChild(empatheticGaze);
            Root.AddChild(heartfeltRequest);

            // Layer 2: Advanced active/passive bonuses.
            nodeIndex <<= 1;
            var charmingDemeanor = new SkillNode(nodeIndex, "Charming Demeanor", 7, "Unlocks additional begging spells", (p) =>
            {
                profile.Talents[TalentID.BeggingSpells].Points |= 0x08;
            });

            nodeIndex <<= 1;
            var smoothTalk = new SkillNode(nodeIndex, "Smooth Talk", 7, "Enhances persuasion skills", (p) =>
            {
                profile.Talents[TalentID.BeggingPersuasion].Points += 1;
            });

            nodeIndex <<= 1;
            var subtleBegging = new SkillNode(nodeIndex, "Subtle Begging", 7, "Boosts karmic rewards", (p) =>
            {
                profile.Talents[TalentID.BeggingKarma].Points += 1;
            });

            nodeIndex <<= 1;
            var inspiringPresence = new SkillNode(nodeIndex, "Inspiring Presence", 7, "Unlocks bonus begging spells", (p) =>
            {
                profile.Talents[TalentID.BeggingSpells].Points |= 0x10;
            });

            // Attach Layer 2 nodes.
            silverTongue.AddChild(charmingDemeanor);
            quickAppeal.AddChild(smoothTalk);
            empatheticGaze.AddChild(subtleBegging);
            heartfeltRequest.AddChild(inspiringPresence);

            // Layer 3: Further active abilities and bonuses.
            nodeIndex <<= 1;
            // Changed from increasing luck bonus to unlocking a spell (0x1000)
            var goldenRequest = new SkillNode(nodeIndex, "Golden Request", 8, "Unlocks bonus begging spell", (p) =>
            {
                profile.Talents[TalentID.BeggingSpells].Points |= 0x1000;
            });

            nodeIndex <<= 1;
            var fortunesFavor = new SkillNode(nodeIndex, "Fortune's Favor", 8, "Unlocks extra begging spells", (p) =>
            {
                profile.Talents[TalentID.BeggingSpells].Points |= 0x20;
            });

            nodeIndex <<= 1;
            var benevolentGesture = new SkillNode(nodeIndex, "Benevolent Gesture", 8, "Increases karmic gains", (p) =>
            {
                profile.Talents[TalentID.BeggingKarma].Points += 1;
            });

            nodeIndex <<= 1;
            var pitysPower = new SkillNode(nodeIndex, "Pity's Power", 8, "Boosts persuasion power", (p) =>
            {
                profile.Talents[TalentID.BeggingPersuasion].Points += 1;
            });

            // Attach Layer 3 nodes.
            charmingDemeanor.AddChild(goldenRequest);
            smoothTalk.AddChild(fortunesFavor);
            subtleBegging.AddChild(benevolentGesture);
            inspiringPresence.AddChild(pitysPower);

            // Layer 4: More advanced magical enhancements.
            nodeIndex <<= 1;
            var prosperousAppeal = new SkillNode(nodeIndex, "Prosperous Appeal", 9, "Enhances begging spells further", (p) =>
            {
                profile.Talents[TalentID.BeggingSpells].Points |= 0x40;
            });

            nodeIndex <<= 1;
            // Changed from boosting luck to unlocking a spell (0x2000)
            var wealthWhisperer = new SkillNode(nodeIndex, "Wealth Whisperer", 9, "Unlocks bonus begging spell", (p) =>
            {
                profile.Talents[TalentID.BeggingSpells].Points |= 0x2000;
            });

            nodeIndex <<= 1;
            // Changed from boosting karma to unlocking a spell (0x4000)
            var luckOfTheStreets = new SkillNode(nodeIndex, "Luck of the Streets", 9, "Unlocks bonus begging spell", (p) =>
            {
                profile.Talents[TalentID.BeggingSpells].Points |= 0x4000;
            });

            nodeIndex <<= 1;
            // Changed from boosting persuasion to unlocking a spell (0x8000)
            var slyRequest = new SkillNode(nodeIndex, "Sly Request", 9, "Unlocks bonus begging spell", (p) =>
            {
                profile.Talents[TalentID.BeggingSpells].Points |= 0x8000;
            });

            // Attach Layer 4 nodes.
            goldenRequest.AddChild(prosperousAppeal);
            fortunesFavor.AddChild(wealthWhisperer);
            benevolentGesture.AddChild(luckOfTheStreets);
            pitysPower.AddChild(slyRequest);

            // Layer 5: Expert-level nodes.
            nodeIndex <<= 1;
            var charismaticPresence = new SkillNode(nodeIndex, "Charismatic Presence", 10, "Boosts persuasion significantly", (p) =>
            {
                profile.Talents[TalentID.BeggingPersuasion].Points += 1;
            });

            nodeIndex <<= 1;
            var opportuneTiming = new SkillNode(nodeIndex, "Opportune Timing", 10, "Unlocks extra begging spells", (p) =>
            {
                profile.Talents[TalentID.BeggingSpells].Points |= 0x80;
            });

            nodeIndex <<= 1;
            var gildedMercy = new SkillNode(nodeIndex, "Gilded Mercy", 10, "Enhances luck with generosity", (p) =>
            {
                profile.Talents[TalentID.BeggingLuck].Points += 1;
            });

            nodeIndex <<= 1;
            var luckyBreak = new SkillNode(nodeIndex, "Lucky Break", 10, "Improves karmic fortune", (p) =>
            {
                profile.Talents[TalentID.BeggingKarma].Points += 1;
            });

            // Attach Layer 5 nodes.
            prosperousAppeal.AddChild(charismaticPresence);
            wealthWhisperer.AddChild(opportuneTiming);
            luckOfTheStreets.AddChild(gildedMercy);
            slyRequest.AddChild(luckyBreak);

            // Layer 6: Mastery nodes.
            nodeIndex <<= 1;
            var masterPersuasion = new SkillNode(nodeIndex, "Master Persuasion", 11, "Master-level boost to persuasion", (p) =>
            {
                profile.Talents[TalentID.BeggingPersuasion].Points += 1;
            });

            nodeIndex <<= 1;
            var miraculousLuck = new SkillNode(nodeIndex, "Miraculous Luck", 11, "Master-level luck enhancement", (p) =>
            {
                profile.Talents[TalentID.BeggingLuck].Points += 1;
            });

            nodeIndex <<= 1;
            var divineSolicitation = new SkillNode(nodeIndex, "Divine Solicitation", 11, "Unlocks divine begging spells", (p) =>
            {
                profile.Talents[TalentID.BeggingSpells].Points |= 0x100;
            });

            nodeIndex <<= 1;
            var cunningAppeal = new SkillNode(nodeIndex, "Cunning Appeal", 11, "Master-level boost to karmic gains", (p) =>
            {
                profile.Talents[TalentID.BeggingKarma].Points += 1;
            });

            // Attach Layer 6 nodes.
            charismaticPresence.AddChild(masterPersuasion);
            opportuneTiming.AddChild(miraculousLuck);
            gildedMercy.AddChild(divineSolicitation);
            luckyBreak.AddChild(cunningAppeal);

            // Layer 7: Final, pinnacle bonuses.
            nodeIndex <<= 1;
            var royalRequest = new SkillNode(nodeIndex, "Royal Request", 12, "Unlocks premium begging spells", (p) =>
            {
                profile.Talents[TalentID.BeggingSpells].Points |= 0x200;
            });

            nodeIndex <<= 1;
            var nobleBegging = new SkillNode(nodeIndex, "Noble Begging", 12, "Boosts luck to a regal level", (p) =>
            {
                profile.Talents[TalentID.BeggingLuck].Points += 1;
            });

            nodeIndex <<= 1;
            var opulentPlea = new SkillNode(nodeIndex, "Opulent Plea", 12, "Enhances persuasion with opulence", (p) =>
            {
                profile.Talents[TalentID.BeggingPersuasion].Points += 1;
            });

            nodeIndex <<= 1;
            var majesticSupplication = new SkillNode(nodeIndex, "Majestic Supplication", 12, "Maximizes karmic benefits", (p) =>
            {
                profile.Talents[TalentID.BeggingKarma].Points += 1;
            });

            // Attach Layer 7 nodes.
            masterPersuasion.AddChild(royalRequest);
            miraculousLuck.AddChild(nobleBegging);
            divineSolicitation.AddChild(opulentPlea);
            cunningAppeal.AddChild(majesticSupplication);

            // Layer 8: Ultimate node.
            nodeIndex <<= 1;
            var ultimateBeggar = new SkillNode(nodeIndex, "Ultimate Beggar", 13, "Grants final bonus: unlocks all bonus spells and improves all passive bonuses", (p) =>
            {
                profile.Talents[TalentID.BeggingSpells].Points |= 0x400 | 0x800;
                profile.Talents[TalentID.BeggingPersuasion].Points += 1;
                profile.Talents[TalentID.BeggingLuck].Points += 1;
                profile.Talents[TalentID.BeggingKarma].Points += 1;
            });

            // Attach the ultimate node to all Layer 7 nodes.
            foreach (var node in new[] { royalRequest, nobleBegging, opulentPlea, majesticSupplication })
            {
                node.AddChild(ultimateBeggar);
            }
        }
    }

    // Command to open the Begging Skill Tree.
    public class BeggingSkillTreeCommand
    {
        public static void Initialize()
        {
            CommandSystem.Register("BegTree", AccessLevel.Player, cmd => ShowTree(cmd.Mobile));
        }

        private static void ShowTree(Mobile m)
        {
            if (m is PlayerMobile pm)
            {
                pm.SendMessage("Opening Begging Skill Tree...");
                pm.SendGump(new BeggingSkillTree(pm));
            }
            else
            {
                m.SendMessage("You must be a player to use this command.");
            }
        }
    }
}
