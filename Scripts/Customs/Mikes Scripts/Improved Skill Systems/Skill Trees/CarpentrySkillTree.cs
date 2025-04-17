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

namespace Server.ACC.CSS.Systems.CarpentryMagic
{
    public class CarpentrySkillTree : SuperGump
    {
        private CarpentryTree tree;
        private Dictionary<SkillNode, Point2D> nodePositions;
        private Dictionary<int, SkillNode> buttonNodeMap;
        private Dictionary<SkillNode, int> edgeThickness;
        private const int buttonSize = 15;
        private int ySpacing = 50, xSpacing = 60;
        private int rootX = 300, rootY = 150;
        private SkillNode selectedNode;

        public CarpentrySkillTree(PlayerMobile user)
            : base(user, null, 100, 100)
        {
            tree = new CarpentryTree(user);
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
            layout.Add("title", () => { AddLabel(100, 20, 1153, "Carpentry Skill Tree"); });

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
            if (!profile.Talents.ContainsKey(TalentID.CarpentryNodes))
                profile.Talents[TalentID.CarpentryNodes] = new Talent(TalentID.CarpentryNodes) { Points = 0 };

            return (profile.Talents[TalentID.CarpentryNodes].Points & BitFlag) != 0;
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
            profile.Talents[TalentID.CarpentryNodes].Points |= BitFlag;

            player.SendMessage($"{Name} activated!");
            onActivate?.Invoke(player);
            return true;
        }
    }

    public class CarpentryTree
    {
        public SkillNode Root { get; }

        public CarpentryTree(PlayerMobile player)
        {
            var profile = player.AcquireTalents();
            int nodeIndex = 0x01;

            // Layer 0: Root Node – Unlocks basic carpentry spells.
            Root = new SkillNode(nodeIndex, "Carpenter's Call", 5, "Unlocks basic carpentry spells", (p) =>
            {
                profile.Talents[TalentID.CarpentrySpells].Points |= 0x01; // Spell 0x01
            });

            // Layer 1: Basic unlocks.
            nodeIndex <<= 1; // becomes 0x02
            var woodWhisper = new SkillNode(nodeIndex, "Wood Whisper", 6, "Unlocks a carpentry spell", (p) =>
            {
                profile.Talents[TalentID.CarpentrySpells].Points |= 0x02; // Spell 0x02
            });

            nodeIndex <<= 1; // becomes 0x04
            var nailPrecision = new SkillNode(nodeIndex, "Nail Precision", 6, "Unlocks a carpentry spell", (p) =>
            {
                profile.Talents[TalentID.CarpentrySpells].Points |= 0x04; // Spell 0x04
            });

            nodeIndex <<= 1; // becomes 0x08
            var planingMastery = new SkillNode(nodeIndex, "Planing Mastery", 6, "Unlocks a carpentry spell", (p) =>
            {
                profile.Talents[TalentID.CarpentrySpells].Points |= 0x08; // Spell 0x08
            });

            nodeIndex <<= 1; // becomes 0x10
            var timberYield = new SkillNode(nodeIndex, "Timber Yield", 6, "Unlocks a carpentry spell", (p) =>
            {
                profile.Talents[TalentID.CarpentrySpells].Points |= 0x10; // Spell 0x10
            });

            Root.AddChild(woodWhisper);
            Root.AddChild(nailPrecision);
            Root.AddChild(planingMastery);
            Root.AddChild(timberYield);

            // Layer 2: Advanced magical and practical unlocks.
            nodeIndex <<= 1; // becomes 0x20
            var forestCrafts = new SkillNode(nodeIndex, "Forest Crafts", 7, "Unlocks a carpentry spell", (p) =>
            {
                profile.Talents[TalentID.CarpentrySpells].Points |= 0x20; // Spell 0x20
            });

            nodeIndex <<= 1; // becomes 0x40
            var carvingMastery = new SkillNode(nodeIndex, "Carving Mastery", 7, "Unlocks a carpentry spell", (p) =>
            {
                profile.Talents[TalentID.CarpentrySpells].Points |= 0x40; // Spell 0x40
            });

            nodeIndex <<= 1; // becomes 0x80
            var arcaneJoinery = new SkillNode(nodeIndex, "Arcane Joinery", 7, "Unlocks a carpentry spell", (p) =>
            {
                profile.Talents[TalentID.CarpentrySpells].Points |= 0x80; // Spell 0x80
            });

            nodeIndex <<= 1; // becomes 0x100
            var extendedReach = new SkillNode(nodeIndex, "Extended Reach", 7, "Unlocks a carpentry spell", (p) =>
            {
                profile.Talents[TalentID.CarpentrySpells].Points |= 0x100; // Spell 0x100
            });

            woodWhisper.AddChild(forestCrafts);
            nailPrecision.AddChild(carvingMastery);
            planingMastery.AddChild(arcaneJoinery);
            timberYield.AddChild(extendedReach);

            // Layer 3: Further yield and efficiency improvements.
            nodeIndex <<= 1; // becomes 0x200
            var grainInsight = new SkillNode(nodeIndex, "Grain Insight", 8, "Unlocks a carpentry spell", (p) =>
            {
                profile.Talents[TalentID.CarpentrySpells].Points |= 0x200; // Spell 0x200
            });

            nodeIndex <<= 1; // becomes 0x400
            var precisionCuts = new SkillNode(nodeIndex, "Precision Cuts", 8, "Unlocks a carpentry spell", (p) =>
            {
                profile.Talents[TalentID.CarpentrySpells].Points |= 0x400; // Spell 0x400
            });

            nodeIndex <<= 1; // becomes 0x800
            var structuralFortitude = new SkillNode(nodeIndex, "Structural Fortitude", 8, "Unlocks a carpentry spell", (p) =>
            {
                profile.Talents[TalentID.CarpentrySpells].Points |= 0x800; // Spell 0x800
            });

            nodeIndex <<= 1; // becomes 0x1000
            var agileAssembly = new SkillNode(nodeIndex, "Agile Assembly", 8, "Unlocks a carpentry spell", (p) =>
            {
                profile.Talents[TalentID.CarpentrySpells].Points |= 0x1000; // Spell 0x1000
            });

            forestCrafts.AddChild(grainInsight);
            carvingMastery.AddChild(precisionCuts);
            arcaneJoinery.AddChild(structuralFortitude);
            extendedReach.AddChild(agileAssembly);

            // Layer 4: More advanced magical enhancements.
            nodeIndex <<= 1; // becomes 0x2000
            var artisanBlessing = new SkillNode(nodeIndex, "Artisan's Blessing", 9, "Unlocks a carpentry spell", (p) =>
            {
                profile.Talents[TalentID.CarpentrySpells].Points |= 0x2000; // Spell 0x2000
            });

            nodeIndex <<= 1; // becomes 0x4000
            var enchantedHammer = new SkillNode(nodeIndex, "Enchanted Hammer", 9, "Unlocks a carpentry spell", (p) =>
            {
                profile.Talents[TalentID.CarpentrySpells].Points |= 0x4000; // Spell 0x4000
            });

            nodeIndex <<= 1; // becomes 0x8000
            var elderCrafting = new SkillNode(nodeIndex, "Elder Crafting", 9, "Unlocks a carpentry spell", (p) =>
            {
                profile.Talents[TalentID.CarpentrySpells].Points |= 0x8000; // Spell 0x8000
            });

            nodeIndex <<= 1; // Next node (for Steady Hands) remains as a passive bonus.
            var steadyHands = new SkillNode(nodeIndex, "Steady Hands", 9, "Boosts crafting range", (p) =>
            {
                profile.Talents[TalentID.CarpentryRange].Points += 1;
            });

            grainInsight.AddChild(artisanBlessing);
            precisionCuts.AddChild(enchantedHammer);
            structuralFortitude.AddChild(elderCrafting);
            agileAssembly.AddChild(steadyHands);

            // Layer 5: Expert-level nodes (passive bonuses remain).
            nodeIndex <<= 1;
            var masterfulEfficiency = new SkillNode(nodeIndex, "Masterful Efficiency", 10, "Boosts overall crafting efficiency", (p) =>
            {
                profile.Talents[TalentID.CarpentryEfficiency].Points += 1;
            });

            nodeIndex <<= 1;
            var bountifulWorkshop = new SkillNode(nodeIndex, "Bountiful Workshop", 10, "Boosts material yield", (p) =>
            {
                profile.Talents[TalentID.CarpentryYield].Points += 1;
            });

            nodeIndex <<= 1;
            var craftingMastery = new SkillNode(nodeIndex, "Crafting Mastery", 10, "Unlocks mastery level spells", (p) =>
            {
                // Remains as a passive bonus.
                profile.Talents[TalentID.CarpentryEfficiency].Points += 1;
            });

            nodeIndex <<= 1;
            var momentumCreation = new SkillNode(nodeIndex, "Momentum of Creation", 10, "Increases crafting speed", (p) =>
            {
                profile.Talents[TalentID.CarpentryEfficiency].Points += 1;
            });

            artisanBlessing.AddChild(masterfulEfficiency);
            enchantedHammer.AddChild(bountifulWorkshop);
            elderCrafting.AddChild(craftingMastery);
            steadyHands.AddChild(momentumCreation);

            // Layer 6: Mastery nodes (passive bonuses remain).
            nodeIndex <<= 1;
            var expandedVision = new SkillNode(nodeIndex, "Expanded Vision", 11, "Enhances spatial awareness", (p) =>
            {
                profile.Talents[TalentID.CarpentryRange].Points += 1;
            });

            nodeIndex <<= 1;
            var mysticBlueprint = new SkillNode(nodeIndex, "Mystic Blueprint", 11, "Boosts material yield with magic", (p) =>
            {
                profile.Talents[TalentID.CarpentryYield].Points += 1;
            });

            nodeIndex <<= 1;
            var ancientArtificer = new SkillNode(nodeIndex, "Ancient Artificer", 11, "Unlocks ancient carpentry spells", (p) =>
            {
                // Remains as a passive bonus.
                profile.Talents[TalentID.CarpentryYield].Points += 1;
            });

            nodeIndex <<= 1;
            var transformationTouch = new SkillNode(nodeIndex, "Transformation Touch", 11, "Increases efficiency with magic", (p) =>
            {
                profile.Talents[TalentID.CarpentryEfficiency].Points += 1;
            });

            masterfulEfficiency.AddChild(expandedVision);
            bountifulWorkshop.AddChild(mysticBlueprint);
            craftingMastery.AddChild(ancientArtificer);
            momentumCreation.AddChild(transformationTouch);

            // Layer 7: Final, pinnacle bonuses (passive bonuses remain).
            nodeIndex <<= 1;
            var ironcladBarrier = new SkillNode(nodeIndex, "Ironclad Barrier", 12, "Provides a protective bonus", (p) =>
            {
                // Passive bonus.
                profile.Talents[TalentID.CarpentryEfficiency].Points += 1;
            });

            nodeIndex <<= 1;
            var workshopEndowment = new SkillNode(nodeIndex, "Workshop Endowment", 12, "Further increases material yield", (p) =>
            {
                profile.Talents[TalentID.CarpentryYield].Points += 1;
            });

            nodeIndex <<= 1;
            var forgeFury = new SkillNode(nodeIndex, "Fury of the Forge", 12, "Boosts crafting efficiency", (p) =>
            {
                profile.Talents[TalentID.CarpentryEfficiency].Points += 1;
            });

            nodeIndex <<= 1;
            var echoesOfCraft = new SkillNode(nodeIndex, "Echoes of Craft", 12, "Enhances crafting range", (p) =>
            {
                profile.Talents[TalentID.CarpentryRange].Points += 1;
            });

            expandedVision.AddChild(ironcladBarrier);
            mysticBlueprint.AddChild(workshopEndowment);
            ancientArtificer.AddChild(forgeFury);
            transformationTouch.AddChild(echoesOfCraft);

            // Layer 8: Ultimate node (passive bonus remains).
            nodeIndex <<= 1;
            var ultimateArtificer = new SkillNode(nodeIndex, "Ultimate Artificer", 13, "Ultimate bonus: boosts all carpentry skills", (p) =>
            {
                profile.Talents[TalentID.CarpentryRange].Points += 1;
                profile.Talents[TalentID.CarpentryEfficiency].Points += 1;
                profile.Talents[TalentID.CarpentryYield].Points += 1;
            });

            foreach (var node in new[] { ironcladBarrier, echoesOfCraft, forgeFury, workshopEndowment })
            {
                node.AddChild(ultimateArtificer);
            }
        }
    }

    // Command to open the Carpentry Skill Tree.
    public class CarpentrySkillTreeCommand
    {
        public static void Initialize()
        {
            CommandSystem.Register("CarpentryTree", AccessLevel.Player, cmd => ShowTree(cmd.Mobile));
        }

        private static void ShowTree(Mobile m)
        {
            if (m is PlayerMobile pm)
            {
                pm.SendMessage("Opening Carpentry Skill Tree...");
                pm.SendGump(new CarpentrySkillTree(pm));
            }
            else
            {
                m.SendMessage("You must be a player to use this command.");
            }
        }
    }
}
