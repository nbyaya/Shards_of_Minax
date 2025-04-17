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

namespace Server.ACC.CSS.Systems.ArcheryMagic
{
    // Revised Archery Skill Tree Gump using AncientKnowledge (Maxxia Points) as the cost resource.
    public class ArcherySkillTree : SuperGump
    {
        private ArcheryTree tree;
        private Dictionary<SkillNode, Point2D> nodePositions;
        private Dictionary<int, SkillNode> buttonNodeMap;
        private Dictionary<SkillNode, int> edgeThickness;
        private const int buttonSize = 15;
        private int ySpacing = 50, xSpacing = 60;
        private int rootX = 300, rootY = 150;
        private SkillNode selectedNode;

        public ArcherySkillTree(PlayerMobile user)
            : base(user, null, 100, 100)
        {
            tree = new ArcheryTree(user);
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
            layout.Add("title", () => { AddLabel(100, 20, 1153, "Archery Skill Tree"); });

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

    // Revised SkillNode class used by the Archery skill tree.
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
            if (!profile.Talents.ContainsKey(TalentID.ArcheryNodes))
                profile.Talents[TalentID.ArcheryNodes] = new Talent(TalentID.ArcheryNodes) { Points = 0 };

            return (profile.Talents[TalentID.ArcheryNodes].Points & BitFlag) != 0;
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
            profile.Talents[TalentID.ArcheryNodes].Points |= BitFlag;

            player.SendMessage($"{Name} activated!");
            onActivate?.Invoke(player);
            return true;
        }
    }

    // ArcheryTree builds the full 30-node tree with 9 layers.
    public class ArcheryTree
    {
        public SkillNode Root { get; }

        public ArcheryTree(PlayerMobile player)
        {
            var profile = player.AcquireTalents();
            int nodeIndex = 0x01;

            // Layer 0: Root Node – Unlocks basic archery spells.
            Root = new SkillNode(nodeIndex, "Call of the Bow", 5, "Unlocks basic archery spells", (p) =>
            {
                profile.Talents[TalentID.ArcherySpells].Points |= 0x01;
            });

            // Layer 1: Basic bonuses.
            nodeIndex <<= 1;
            var eagleEye = new SkillNode(nodeIndex, "Eagle Eye", 6, "Increases accuracy", (p) =>
            {
                profile.Talents[TalentID.ArcheryAccuracy].Points += 1;
            });

            nodeIndex <<= 1;
            var steadyGrip = new SkillNode(nodeIndex, "Steady Grip", 6, "Improves draw speed", (p) =>
            {
                profile.Talents[TalentID.ArcheryDrawSpeed].Points += 1;
            });

            nodeIndex <<= 1;
            var quiverMastery = new SkillNode(nodeIndex, "Quiver Mastery", 6, "Enhances arrow recovery", (p) =>
            {
                // Unlock bonus spell bit and add passive recovery bonus.
                profile.Talents[TalentID.ArcherySpells].Points |= 0x04;
                profile.Talents[TalentID.ArcheryArrowRecovery].Points += 1;
            });

            nodeIndex <<= 1;
            var lightfoot = new SkillNode(nodeIndex, "Lightfoot", 6, "Grants nimble movement while aiming", (p) =>
            {
                // Passive bonus; implementation can affect movement/aiming.
                profile.Talents[TalentID.ArcheryCritical].Points += 1;
            });

            Root.AddChild(eagleEye);
            Root.AddChild(steadyGrip);
            Root.AddChild(quiverMastery);
            Root.AddChild(lightfoot);

            // Layer 2: Advanced magical and practical bonuses.
            nodeIndex <<= 1;
            var flaringArrows = new SkillNode(nodeIndex, "Flaring Arrows", 7, "Unlocks a fire-based arrow spell", (p) =>
            {
                profile.Talents[TalentID.ArcherySpells].Points |= 0x08;
            });

            nodeIndex <<= 1;
            var rapidRelease = new SkillNode(nodeIndex, "Rapid Release", 7, "Further improves draw speed", (p) =>
            {
                profile.Talents[TalentID.ArcheryDrawSpeed].Points += 1;
            });

            nodeIndex <<= 1;
            var piercingShot = new SkillNode(nodeIndex, "Piercing Shot", 7, "Unlocks a piercing arrow spell", (p) =>
            {
                profile.Talents[TalentID.ArcherySpells].Points |= 0x10;
            });

            nodeIndex <<= 1;
            var windWhisper = new SkillNode(nodeIndex, "Wind Whisper", 7, "Increases accuracy further", (p) =>
            {
                profile.Talents[TalentID.ArcheryAccuracy].Points += 1;
            });

            eagleEye.AddChild(flaringArrows);
            steadyGrip.AddChild(rapidRelease);
            quiverMastery.AddChild(piercingShot);
            lightfoot.AddChild(windWhisper);

            // Layer 3: Passive ability improvements.
            nodeIndex <<= 1;
            var bountyOfTheHunt = new SkillNode(nodeIndex, "Bounty of the Hunt", 8, "Enhances arrow damage", (p) =>
            {
                profile.Talents[TalentID.ArcheryDamage].Points += 1;
            });

            nodeIndex <<= 1;
            var serpentsReflex = new SkillNode(nodeIndex, "Serpent's Reflex", 8, "Improves critical chance", (p) =>
            {
                profile.Talents[TalentID.ArcheryCritical].Points += 1;
            });

            nodeIndex <<= 1;
            var silentFlight = new SkillNode(nodeIndex, "Silent Flight", 8, "Reduces draw delay", (p) =>
            {
                profile.Talents[TalentID.ArcheryDrawSpeed].Points += 1;
            });

            nodeIndex <<= 1;
            var hawkeyesVigil = new SkillNode(nodeIndex, "Hawkeye's Vigil", 8, "Increases arrow recovery chance", (p) =>
            {
                profile.Talents[TalentID.ArcheryArrowRecovery].Points += 1;
            });

            flaringArrows.AddChild(bountyOfTheHunt);
            rapidRelease.AddChild(serpentsReflex);
            piercingShot.AddChild(silentFlight);
            windWhisper.AddChild(hawkeyesVigil);

            // Layer 4: Magical enhancements.
            nodeIndex <<= 1;
            var flamingArrows = new SkillNode(nodeIndex, "Flaming Arrows", 9, "Adds fire damage to arrows", (p) =>
            {
                profile.Talents[TalentID.ArcherySpells].Points |= 0x20;
            });

            nodeIndex <<= 1;
            var iceboundShot = new SkillNode(nodeIndex, "Icebound Shot", 9, "Adds ice damage to arrows", (p) =>
            {
                profile.Talents[TalentID.ArcherySpells].Points |= 0x40;
            });

            nodeIndex <<= 1;
            var thunderbolt = new SkillNode(nodeIndex, "Thunderbolt", 9, "Adds lightning damage to arrows", (p) =>
            {
                profile.Talents[TalentID.ArcherySpells].Points |= 0x80;
            });

            nodeIndex <<= 1;
            var shadowStrike = new SkillNode(nodeIndex, "Shadow Strike", 9, "Grants a stealth bonus to shots", (p) =>
            {
                // Passive bonus – for example, a slight boost to overall damage.
                profile.Talents[TalentID.ArcheryDamage].Points += 1;
            });

            bountyOfTheHunt.AddChild(flamingArrows);
            serpentsReflex.AddChild(iceboundShot);
            silentFlight.AddChild(thunderbolt);
            hawkeyesVigil.AddChild(shadowStrike);

            // Layer 5: Expert-level nodes.
            nodeIndex <<= 1;
            var primevalAim = new SkillNode(nodeIndex, "Primeval Aim", 10, "Boosts overall accuracy", (p) =>
            {
                profile.Talents[TalentID.ArcheryAccuracy].Points += 1;
            });

            nodeIndex <<= 1;
            var bountifulVolley = new SkillNode(nodeIndex, "Bountiful Volley", 10, "Increases arrow damage", (p) =>
            {
                profile.Talents[TalentID.ArcheryDamage].Points += 1;
            });

            nodeIndex <<= 1;
            var masterShot = new SkillNode(nodeIndex, "Master Shot", 10, "Unlocks mastery archery spells", (p) =>
            {
                profile.Talents[TalentID.ArcherySpells].Points |= 0x100;
            });

            nodeIndex <<= 1;
            var momentumOfTheWind = new SkillNode(nodeIndex, "Momentum of the Wind", 10, "Further improves draw speed", (p) =>
            {
                profile.Talents[TalentID.ArcheryDrawSpeed].Points += 1;
            });

            flamingArrows.AddChild(primevalAim);
            iceboundShot.AddChild(bountifulVolley);
            thunderbolt.AddChild(masterShot);
            shadowStrike.AddChild(momentumOfTheWind);

            // Layer 6: Mastery nodes.
            nodeIndex <<= 1;
            var expandedFocus = new SkillNode(nodeIndex, "Expanded Focus", 11, "Enhances spatial awareness", (p) =>
            {
                profile.Talents[TalentID.ArcheryAccuracy].Points += 1;
            });

            nodeIndex <<= 1;
            var mysticQuiver = new SkillNode(nodeIndex, "Mystic Quiver", 11, "Boosts arrow recovery", (p) =>
            {
                profile.Talents[TalentID.ArcheryArrowRecovery].Points += 1;
            });

            nodeIndex <<= 1;
            var ancientMarksman = new SkillNode(nodeIndex, "Ancient Marksman", 11, "Unlocks ancient archery spells", (p) =>
            {
                profile.Talents[TalentID.ArcherySpells].Points |= 0x200;
            });

            nodeIndex <<= 1;
            var arrowTransformation = new SkillNode(nodeIndex, "Arrow Transformation", 11, "Increases arrow velocity", (p) =>
            {
                // Here you might adjust the arrow speed bonus.
                profile.Talents[TalentID.ArcheryDamage].Points += 1; // Example bonus
            });

            primevalAim.AddChild(expandedFocus);
            bountifulVolley.AddChild(mysticQuiver);
            masterShot.AddChild(ancientMarksman);
            momentumOfTheWind.AddChild(arrowTransformation);

            // Layer 7: Pinnacle bonuses.
            nodeIndex <<= 1;
            var barrageBarrier = new SkillNode(nodeIndex, "Barrage Barrier", 12, "Provides a protective bonus", (p) =>
            {
                // Passive bonus; could reduce ranged damage taken.
            });

            nodeIndex <<= 1;
            var naturesEndowment = new SkillNode(nodeIndex, "Nature's Endowment", 12, "Further increases arrow recovery", (p) =>
            {
                profile.Talents[TalentID.ArcheryArrowRecovery].Points += 1;
            });

            nodeIndex <<= 1;
            var furyOfTheFalcon = new SkillNode(nodeIndex, "Fury of the Falcon", 12, "Boosts arrow damage", (p) =>
            {
                profile.Talents[TalentID.ArcheryDamage].Points += 1;
            });

            nodeIndex <<= 1;
            var echoesOfTheWind = new SkillNode(nodeIndex, "Echoes of the Wind", 12, "Enhances accuracy", (p) =>
            {
                profile.Talents[TalentID.ArcheryAccuracy].Points += 1;
            });

            expandedFocus.AddChild(barrageBarrier);
            mysticQuiver.AddChild(naturesEndowment);
            ancientMarksman.AddChild(furyOfTheFalcon);
            arrowTransformation.AddChild(echoesOfTheWind);

            // Layer 8: Ultimate node.
            nodeIndex <<= 1;
            var ultimateArcher = new SkillNode(nodeIndex, "Ultimate Archer", 13, "Ultimate bonus: boosts all archery skills", (p) =>
            {
                profile.Talents[TalentID.ArcherySpells].Points |= 0x800 | 0x1000;
                profile.Talents[TalentID.ArcheryAccuracy].Points += 1;
                profile.Talents[TalentID.ArcheryDamage].Points += 1;
                profile.Talents[TalentID.ArcheryDrawSpeed].Points += 1;
                profile.Talents[TalentID.ArcheryArrowRecovery].Points += 1;
            });

            foreach (var node in new[] { barrageBarrier, echoesOfTheWind, furyOfTheFalcon, naturesEndowment })
            {
                node.AddChild(ultimateArcher);
            }
        }
    }

    // Command to open the Archery Skill Tree.
    public class ArcherySkillTreeCommand
    {
        public static void Initialize()
        {
            CommandSystem.Register("ArcheryTree", AccessLevel.Player, cmd => ShowTree(cmd.Mobile));
        }

        private static void ShowTree(Mobile m)
        {
            if (m is PlayerMobile pm)
            {
                pm.SendMessage("Opening Archery Skill Tree...");
                pm.SendGump(new ArcherySkillTree(pm));
            }
            else
            {
                m.SendMessage("You must be a player to use this command.");
            }
        }
    }
}
