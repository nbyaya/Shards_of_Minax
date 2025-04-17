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

namespace Server.ACC.CSS.Systems.PastoraliconMagic
{
    // Revised Pastoralicon (Herding) Skill Tree Gump using Maxxia Points (AncientKnowledge) as the cost.
    public class PastoraliconSkillTree : SuperGump
    {
        private PastoraliconTree tree;
        private Dictionary<SkillNode, Point2D> nodePositions;
        private Dictionary<int, SkillNode> buttonNodeMap;
        private Dictionary<SkillNode, int> edgeThickness;
        private const int buttonSize = 15;
        private int ySpacing = 50, xSpacing = 60;
        private int rootX = 300, rootY = 150;
        private SkillNode selectedNode;

        public PastoraliconSkillTree(PlayerMobile user)
            : base(user, null, 100, 100)
        {
            tree = new PastoraliconTree(user);
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
            layout.Add("title", () => { AddLabel(100, 20, 1153, "Pastoralicon Skill Tree"); });

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

    // Revised SkillNode class used by both trees.
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
            if (!profile.Talents.ContainsKey(TalentID.PastoraliconNodes))
                profile.Talents[TalentID.PastoraliconNodes] = new Talent(TalentID.PastoraliconNodes) { Points = 0 };

            return (profile.Talents[TalentID.PastoraliconNodes].Points & BitFlag) != 0;
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
            profile.Talents[TalentID.PastoraliconNodes].Points |= BitFlag;

            player.SendMessage($"{Name} activated!");

            onActivate?.Invoke(player);
            return true;
        }
    }

    // The full Pastoralicon (Herding) tree structure with 9 layers (30 nodes)
    public class PastoraliconTree
    {
        public SkillNode Root { get; }

        public PastoraliconTree(PlayerMobile player)
        {
            var profile = player.AcquireTalents();
            int nodeIndex = 0x01;

            // Layer 0: Root Node – Unlocks basic herding spells.
            Root = new SkillNode(nodeIndex, "Call of the Flock", 5, "Unlocks basic herding spells", (p) =>
            {
                // Unlock basic herding spells.
                profile.Talents[TalentID.PastoraliconSpells].Points |= 0x01;
            });

            // Layer 1: Basic bonuses.
            nodeIndex <<= 1;
            var shepherdsInsight = new SkillNode(nodeIndex, "Shepherd's Insight", 6, "Increases flock awareness", (p) =>
            {
                profile.Talents[TalentID.MinionDamageBonus].Points += 1;
            });

            nodeIndex <<= 1;
            var gentleHand = new SkillNode(nodeIndex, "Gentle Hand", 6, "Improves animal handling", (p) =>
            {
                profile.Talents[TalentID.MinionDamageBonus].Points += 1;
            });

            nodeIndex <<= 1;
            var woollyWisdom = new SkillNode(nodeIndex, "Woolly Wisdom", 6, "Unlocks bonus herding spells", (p) =>
            {
                profile.Talents[TalentID.PastoraliconSpells].Points |= 0x04;
            });

            nodeIndex <<= 1;
            var naturesProvision = new SkillNode(nodeIndex, "Nature's Provision", 6, "Increases yield from livestock", (p) =>
            {
                profile.Talents[TalentID.MinionDamageBonus].Points += 1;
            });

            Root.AddChild(shepherdsInsight);
            Root.AddChild(gentleHand);
            Root.AddChild(woollyWisdom);
            Root.AddChild(naturesProvision);

            // Layer 2: Advanced magical and practical bonuses.
            nodeIndex <<= 1;
            var animalEmpathy = new SkillNode(nodeIndex, "Animal Empathy", 7, "Unlocks additional spells", (p) =>
            {
                profile.Talents[TalentID.PastoraliconSpells].Points |= 0x08;
            });

            nodeIndex <<= 1;
            var quickCommand = new SkillNode(nodeIndex, "Quick Command", 7, "Improves handling speed", (p) =>
            {
                profile.Talents[TalentID.MinionDamageBonus].Points += 1;
            });

            nodeIndex <<= 1;
            var mysticBond = new SkillNode(nodeIndex, "Mystic Bond", 7, "Unlocks advanced herding spells", (p) =>
            {
                profile.Talents[TalentID.PastoraliconSpells].Points |= 0x10;
            });

            nodeIndex <<= 1;
            var fieldsBlessing = new SkillNode(nodeIndex, "Field's Blessing", 7, "Increases flock guidance", (p) =>
            {
                profile.Talents[TalentID.MinionDamageBonus].Points += 1;
            });

            shepherdsInsight.AddChild(animalEmpathy);
            gentleHand.AddChild(quickCommand);
            woollyWisdom.AddChild(mysticBond);
            naturesProvision.AddChild(fieldsBlessing);

            // Layer 3: Further bonuses.
            nodeIndex <<= 1;
            var flockFortitude = new SkillNode(nodeIndex, "Flock Fortitude", 8, "Enhances livestock yield", (p) =>
            {
                profile.Talents[TalentID.MinionDamageBonus].Points += 1;
            });

            nodeIndex <<= 1;
            var swiftPacing = new SkillNode(nodeIndex, "Swift Pacing", 8, "Further improves handling speed", (p) =>
            {
                profile.Talents[TalentID.MinionDamageBonus].Points += 1;
            });

            nodeIndex <<= 1;
            var bovineBrilliance = new SkillNode(nodeIndex, "Bovine Brilliance", 8, "Unlocks a fortitude bonus", (p) =>
            {
                profile.Talents[TalentID.PastoraliconSpells].Points |= 0x20;
            });

            nodeIndex <<= 1;
            var naturalGrace = new SkillNode(nodeIndex, "Natural Grace", 8, "Improves reaction to animal cues", (p) =>
            {
                profile.Talents[TalentID.MinionDamageBonus].Points += 1;
            });

            animalEmpathy.AddChild(flockFortitude);
            quickCommand.AddChild(swiftPacing);
            mysticBond.AddChild(bovineBrilliance);
            fieldsBlessing.AddChild(naturalGrace);

            // Layer 4: More advanced magical enhancements.
            nodeIndex <<= 1;
            var meadowsBoon = new SkillNode(nodeIndex, "Meadow's Boon", 9, "Enhances livestock yield further", (p) =>
            {
                profile.Talents[TalentID.MinionDamageBonus].Points += 1;
            });

            nodeIndex <<= 1;
            var faunasFavor = new SkillNode(nodeIndex, "Fauna's Favor", 9, "Unlocks fauna-related bonus spells", (p) =>
            {
                profile.Talents[TalentID.PastoraliconSpells].Points |= 0x40;
            });

            nodeIndex <<= 1;
            var elderPastures = new SkillNode(nodeIndex, "Elder Pastures", 9, "Unlocks elder animal spells", (p) =>
            {
                profile.Talents[TalentID.PastoraliconSpells].Points |= 0x80;
            });

            nodeIndex <<= 1;
            var openRange = new SkillNode(nodeIndex, "Open Range", 9, "Boosts herd guidance", (p) =>
            {
                profile.Talents[TalentID.MinionDamageBonus].Points += 1;
            });

            flockFortitude.AddChild(meadowsBoon);
            swiftPacing.AddChild(faunasFavor);
            bovineBrilliance.AddChild(elderPastures);
            naturalGrace.AddChild(openRange);

            // Layer 5: Expert-level nodes.
            nodeIndex <<= 1;
            var primevalHerding = new SkillNode(nodeIndex, "Primeval Herding", 10, "Boosts overall handling efficiency", (p) =>
            {
                profile.Talents[TalentID.MinionDamageBonus].Points += 1;
            });

            nodeIndex <<= 1;
            var abundantGrazing = new SkillNode(nodeIndex, "Abundant Grazing", 10, "Boosts yield from livestock", (p) =>
            {
                profile.Talents[TalentID.MinionDamageBonus].Points += 1;
            });

            nodeIndex <<= 1;
            var livestockMastery = new SkillNode(nodeIndex, "Livestock Mastery", 10, "Unlocks mastery-level spells", (p) =>
            {
                profile.Talents[TalentID.PastoraliconSpells].Points |= 0x100;
            });

            nodeIndex <<= 1;
            var herdMomentum = new SkillNode(nodeIndex, "Herd Momentum", 10, "Increases command speed", (p) =>
            {
                profile.Talents[TalentID.MinionDamageBonus].Points += 1;
            });

            meadowsBoon.AddChild(primevalHerding);
            faunasFavor.AddChild(abundantGrazing);
            elderPastures.AddChild(livestockMastery);
            openRange.AddChild(herdMomentum);

            // Layer 6: Mastery nodes.
            nodeIndex <<= 1;
            var expandedPerception = new SkillNode(nodeIndex, "Expanded Perception", 11, "Enhances flock awareness", (p) =>
            {
                profile.Talents[TalentID.MinionDamageBonus].Points += 1;
            });

            nodeIndex <<= 1;
            var mysticPasture = new SkillNode(nodeIndex, "Mystic Pasture", 11, "Boosts yield with magic", (p) =>
            {
                profile.Talents[TalentID.MinionDamageBonus].Points += 1;
            });

            nodeIndex <<= 1;
            var ancientShepherd = new SkillNode(nodeIndex, "Ancient Shepherd", 11, "Unlocks ancient herding spells", (p) =>
            {
                profile.Talents[TalentID.PastoraliconSpells].Points |= 0x200;
            });

            nodeIndex <<= 1;
            var herdingTransformation = new SkillNode(nodeIndex, "Herding Transformation", 11, "Increases magical efficiency", (p) =>
            {
                profile.Talents[TalentID.MinionDamageBonus].Points += 1;
            });

            primevalHerding.AddChild(expandedPerception);
            abundantGrazing.AddChild(mysticPasture);
            livestockMastery.AddChild(ancientShepherd);
            herdMomentum.AddChild(herdingTransformation);

            // Layer 7: Pinnacle bonuses.
            nodeIndex <<= 1;
            var barrierOfFleece = new SkillNode(nodeIndex, "Barrier of Fleece", 12, "Provides a protective barrier", (p) =>
            {
                profile.Talents[TalentID.PastoraliconSpells].Points |= 0x400;
            });

            nodeIndex <<= 1;
            var naturesEndowment = new SkillNode(nodeIndex, "Nature's Endowment", 12, "Further increases yield", (p) =>
            {
                profile.Talents[TalentID.MinionDamageBonus].Points += 1;
            });

            nodeIndex <<= 1;
            var pastoralFury = new SkillNode(nodeIndex, "Pastoral Fury", 12, "Boosts herding power", (p) =>
            {
                profile.Talents[TalentID.MinionDamageBonus].Points += 1;
            });

            nodeIndex <<= 1;
            var wildEchoes = new SkillNode(nodeIndex, "Wild Echoes", 12, "Enhances guidance with wild energy", (p) =>
            {
                profile.Talents[TalentID.MinionDamageBonus].Points += 1;
            });

            expandedPerception.AddChild(barrierOfFleece);
            mysticPasture.AddChild(naturesEndowment);
            ancientShepherd.AddChild(pastoralFury);
            herdingTransformation.AddChild(wildEchoes);

            // Layer 8: Ultimate node.
            nodeIndex <<= 1;
            var ultimateShepherd = new SkillNode(nodeIndex, "Ultimate Shepherd", 13, "Ultimate bonus: boosts all herding skills", (p) =>
            {
                profile.Talents[TalentID.PastoraliconSpells].Points |= 0x800 | 0x1000;
                profile.Talents[TalentID.MinionDamageBonus].Points += 1;
                profile.Talents[TalentID.MinionDamageBonus].Points += 1;
                profile.Talents[TalentID.MinionDamageBonus].Points += 1;
            });

            foreach (var node in new[] { barrierOfFleece, wildEchoes, pastoralFury, naturesEndowment })
            {
                node.AddChild(ultimateShepherd);
            }
        }
    }

    // Command to open the Pastoralicon Skill Tree.
    public class PastoraliconSkillTreeCommand
    {
        public static void Initialize()
        {
            CommandSystem.Register("PastoralTree", AccessLevel.Player, cmd => ShowTree(cmd.Mobile));
        }

        private static void ShowTree(Mobile m)
        {
            if (m is PlayerMobile pm)
            {
                pm.SendMessage("Opening Pastoralicon Skill Tree...");
                pm.SendGump(new PastoraliconSkillTree(pm));
            }
            else
            {
                m.SendMessage("You must be a player to use this command.");
            }
        }
    }
}
