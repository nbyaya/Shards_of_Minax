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

namespace Server.ACC.CSS.Systems.NecromancyMagic
{
    // Revised Necromancy Skill Tree Gump using AncientKnowledge (Maxxia Points) as the cost resource.
    public class NecromancySkillTree : SuperGump
    {
        private NecromancyTree tree;
        private Dictionary<SkillNode, Point2D> nodePositions;
        private Dictionary<int, SkillNode> buttonNodeMap;
        private Dictionary<SkillNode, int> edgeThickness;
        private const int buttonSize = 15;
        private int ySpacing = 50, xSpacing = 60;
        private int rootX = 300, rootY = 150;
        private SkillNode selectedNode;

        public NecromancySkillTree(PlayerMobile user)
            : base(user, null, 100, 100)
        {
            tree = new NecromancyTree(user);
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

            // Position nodes centered around rootX per level.
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
            layout.Add("title", () => { AddLabel(100, 20, 1153, "Necromancy Skill Tree"); });

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

    // SkillNode used for Necromancy – note that it works exactly like your Lumberjacking version.
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
            if (!profile.Talents.ContainsKey(TalentID.NecromancyNodes))
                profile.Talents[TalentID.NecromancyNodes] = new Talent(TalentID.NecromancyNodes) { Points = 0 };

            return (profile.Talents[TalentID.NecromancyNodes].Points & BitFlag) != 0;
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
            profile.Talents[TalentID.NecromancyNodes].Points |= BitFlag;

            player.SendMessage($"{Name} activated!");
            onActivate?.Invoke(player);
            return true;
        }
    }

    // The full Necromancy tree structure with 30 nodes over 9 layers.
    public class NecromancyTree
    {
        public SkillNode Root { get; }

        public NecromancyTree(PlayerMobile player)
        {
            var profile = player.AcquireTalents();
            int nodeIndex = 0x01;

            // Layer 0: Root Node – unlocks basic necromancy spells.
            Root = new SkillNode(nodeIndex, "Whisper of the Damned", 5, "Unlocks basic necromancy spells", (p) =>
            {
                profile.Talents[TalentID.NecromancySpells].Points |= 0x01;
            });

            // Layer 1: Basic bonuses / Spell unlocks.
            nodeIndex <<= 1; // becomes 0x02
            var boneSense = new SkillNode(nodeIndex, "Bone Sense", 6, "Unlocks spell: Bone Sense", (p) =>
            {
                profile.Talents[TalentID.NecromancySpells].Points |= 0x02;
            });

            nodeIndex <<= 1; // becomes 0x04
            var grimEfficiency = new SkillNode(nodeIndex, "Grim Efficiency", 6, "Enhances casting efficiency", (p) =>
            {
                profile.Talents[TalentID.NecromancyEfficiency].Points += 1;
            });

            nodeIndex <<= 1; // becomes 0x08
            var darkConversion = new SkillNode(nodeIndex, "Dark Conversion", 6, "Unlocks spell: Dark Conversion", (p) =>
            {
                profile.Talents[TalentID.NecromancySpells].Points |= 0x04;
            });

            nodeIndex <<= 1; // becomes 0x10
            var necroticBounty = new SkillNode(nodeIndex, "Necrotic Bounty", 6, "Increases necrotic energy yield", (p) =>
            {
                profile.Talents[TalentID.NecromancyYield].Points += 1;
            });

            Root.AddChild(boneSense);
            Root.AddChild(grimEfficiency);
            Root.AddChild(darkConversion);
            Root.AddChild(necroticBounty);

            // Layer 2: Advanced magical and practical bonuses.
            nodeIndex <<= 1; // becomes 0x20 (but used here only for node ordering)
            var phantomWhisper = new SkillNode(nodeIndex, "Phantom Whisper", 7, "Unlocks spell: Phantom Whisper", (p) =>
            {
                profile.Talents[TalentID.NecromancySpells].Points |= 0x08;
            });

            nodeIndex <<= 1; // becomes 0x40
            var corpseFlow = new SkillNode(nodeIndex, "Corpse Flow", 7, "Further improves casting efficiency", (p) =>
            {
                profile.Talents[TalentID.NecromancyEfficiency].Points += 1;
            });

            nodeIndex <<= 1; // becomes 0x80
            var arcaneOssuary = new SkillNode(nodeIndex, "Arcane Ossuary", 7, "Unlocks spell: Arcane Ossuary", (p) =>
            {
                profile.Talents[TalentID.NecromancySpells].Points |= 0x10;
            });

            nodeIndex <<= 1; // becomes 0x100
            var deepMourning = new SkillNode(nodeIndex, "Deep Mourning", 7, "Further increases undead detection range", (p) =>
            {
                profile.Talents[TalentID.NecromancyRange].Points += 1;
            });

            boneSense.AddChild(phantomWhisper);
            grimEfficiency.AddChild(corpseFlow);
            darkConversion.AddChild(arcaneOssuary);
            necroticBounty.AddChild(deepMourning);

            // Layer 3: Yield and potency improvements.
            nodeIndex <<= 1; // becomes 0x200
            var bountifulGrave = new SkillNode(nodeIndex, "Bountiful Grave", 8, "Enhances necrotic energy yield", (p) =>
            {
                profile.Talents[TalentID.NecromancyYield].Points += 1;
            });

            nodeIndex <<= 1; // becomes 0x400
            var ectoplasmicSap = new SkillNode(nodeIndex, "Ectoplasmic Sap", 8, "Unlocks spell: Ectoplasmic Sap", (p) =>
            {
                profile.Talents[TalentID.NecromancySpells].Points |= 0x20;
            });

            nodeIndex <<= 1; // becomes 0x800
            var ghoulFortitude = new SkillNode(nodeIndex, "Ghoul Fortitude", 8, "Unlocks spell: Ghoul Fortitude", (p) =>
            {
                profile.Talents[TalentID.NecromancySpells].Points |= 0x40;
            });

            nodeIndex <<= 1; // becomes 0x1000
            var deathlyReflexes = new SkillNode(nodeIndex, "Deathly Reflexes", 8, "Increases reaction speed", (p) =>
            {
                profile.Talents[TalentID.NecromancyEfficiency].Points += 1;
            });

            phantomWhisper.AddChild(bountifulGrave);
            corpseFlow.AddChild(ectoplasmicSap);
            arcaneOssuary.AddChild(ghoulFortitude);
            deepMourning.AddChild(deathlyReflexes);

            // Layer 4: More advanced magical enhancements.
            nodeIndex <<= 1; // becomes 0x2000
            var mournersBlessing = new SkillNode(nodeIndex, "Mourner's Blessing", 9, "Unlocks spell: Mourner's Blessing", (p) =>
            {
                profile.Talents[TalentID.NecromancySpells].Points |= 0x80;
            });

            nodeIndex <<= 1; // becomes 0x4000
            var spectralGrace = new SkillNode(nodeIndex, "Spectral Grace", 9, "Unlocks spell: Spectral Grace", (p) =>
            {
                profile.Talents[TalentID.NecromancySpells].Points |= 0x100;
            });

            nodeIndex <<= 1; // becomes 0x8000
            var ancientRemains = new SkillNode(nodeIndex, "Ancient Remains", 9, "Unlocks spell: Ancient Remains", (p) =>
            {
                profile.Talents[TalentID.NecromancySpells].Points |= 0x200;
            });

            nodeIndex <<= 1; // becomes 0x10000
            var wraithSurge = new SkillNode(nodeIndex, "Wraith Surge", 9, "Unlocks spell: Wraith Surge", (p) =>
            {
                profile.Talents[TalentID.NecromancySpells].Points |= 0x400;
            });

            bountifulGrave.AddChild(mournersBlessing);
            ectoplasmicSap.AddChild(spectralGrace);
            ghoulFortitude.AddChild(ancientRemains);
            deathlyReflexes.AddChild(wraithSurge);

            // Layer 5: Expert-level nodes.
            nodeIndex <<= 1; // becomes 0x20000
            var primevalEfficiency = new SkillNode(nodeIndex, "Primeval Efficiency", 10, "Unlocks spell: Primeval Efficiency", (p) =>
            {
                profile.Talents[TalentID.NecromancySpells].Points |= 0x800;
            });

            nodeIndex <<= 1; // becomes 0x40000
            var bountifulHarvest = new SkillNode(nodeIndex, "Bountiful Harvest", 10, "Unlocks spell: Bountiful Harvest", (p) =>
            {
                profile.Talents[TalentID.NecromancySpells].Points |= 0x1000;
            });

            nodeIndex <<= 1; // becomes 0x80000
            var boneMastery = new SkillNode(nodeIndex, "Bone Mastery", 10, "Unlocks spell: Bone Mastery", (p) =>
            {
                profile.Talents[TalentID.NecromancySpells].Points |= 0x2000;
            });

            nodeIndex <<= 1; // becomes 0x100000
            var rapidDecay = new SkillNode(nodeIndex, "Rapid Decay", 10, "Unlocks spell: Rapid Decay", (p) =>
            {
                profile.Talents[TalentID.NecromancySpells].Points |= 0x4000;
            });

            mournersBlessing.AddChild(primevalEfficiency);
            spectralGrace.AddChild(bountifulHarvest);
            ancientRemains.AddChild(boneMastery);
            wraithSurge.AddChild(rapidDecay);

            // Layer 6: Mastery nodes.
            nodeIndex <<= 1; // becomes 0x200000
            var expandedPerception = new SkillNode(nodeIndex, "Expanded Perception", 11, "Enhances perception of the undead", (p) =>
            {
                profile.Talents[TalentID.NecromancyRange].Points += 1;
            });

            nodeIndex <<= 1; // becomes 0x400000
            var mysticOssicle = new SkillNode(nodeIndex, "Mystic Ossicle", 11, "Boosts energy yield with magic", (p) =>
            {
                profile.Talents[TalentID.NecromancyYield].Points += 1;
            });

            nodeIndex <<= 1; // becomes 0x800000
            var ancientHarbinger = new SkillNode(nodeIndex, "Ancient Harbinger", 11, "Unlocks spell: Ancient Harbinger", (p) =>
            {
                profile.Talents[TalentID.NecromancySpells].Points |= 0x8000;
            });

            nodeIndex <<= 1; // becomes 0x1000000
            var transmutation = new SkillNode(nodeIndex, "Transmutation", 11, "Increases conversion efficiency", (p) =>
            {
                profile.Talents[TalentID.NecromancyEfficiency].Points += 1;
            });

            primevalEfficiency.AddChild(expandedPerception);
            bountifulHarvest.AddChild(mysticOssicle);
            boneMastery.AddChild(ancientHarbinger);
            rapidDecay.AddChild(transmutation);

            // Layer 7: Final, pinnacle bonuses.
            nodeIndex <<= 1; // becomes 0x2000000
            var boneBarrier = new SkillNode(nodeIndex, "Bone Barrier", 12, "Provides a protective barrier", (p) =>
            {
                // Changed: no spell unlock here – bonus only.
                profile.Talents[TalentID.NecromancyRange].Points += 1;
            });

            nodeIndex <<= 1; // becomes 0x4000000
            var soulEndowment = new SkillNode(nodeIndex, "Soul Endowment", 12, "Further increases necrotic energy yield", (p) =>
            {
                profile.Talents[TalentID.NecromancyYield].Points += 1;
            });

            nodeIndex <<= 1; // becomes 0x8000000
            var necroticFury = new SkillNode(nodeIndex, "Necrotic Fury", 12, "Boosts spell potency", (p) =>
            {
                profile.Talents[TalentID.NecromancyEfficiency].Points += 1;
            });

            nodeIndex <<= 1; // becomes 0x10000000
            var echoesOfTheGrave = new SkillNode(nodeIndex, "Echoes of the Grave", 12, "Enhances spell range", (p) =>
            {
                profile.Talents[TalentID.NecromancyRange].Points += 1;
            });

            expandedPerception.AddChild(boneBarrier);
            mysticOssicle.AddChild(soulEndowment);
            ancientHarbinger.AddChild(necroticFury);
            transmutation.AddChild(echoesOfTheGrave);

            // Layer 8: Ultimate node.
            nodeIndex <<= 1; // becomes 0x20000000
            var ultimateHarbinger = new SkillNode(nodeIndex, "Ultimate Harbinger", 13, "Ultimate bonus: boosts all necromancy skills", (p) =>
            {
                // Removed spell unlock bits here.
                profile.Talents[TalentID.NecromancyRange].Points += 1;
                profile.Talents[TalentID.NecromancyEfficiency].Points += 1;
                profile.Talents[TalentID.NecromancyYield].Points += 1;
                profile.Talents[TalentID.NecromancySummon].Points += 1;
            });

            // Attach the ultimate node to all Layer 7 nodes.
            foreach (var node in new[] { boneBarrier, echoesOfTheGrave, necroticFury, soulEndowment })
            {
                node.AddChild(ultimateHarbinger);
            }
        }
    }

    // Command to open the Necromancy Skill Tree.
    public class NecromancySkillTreeCommand
    {
        public static void Initialize()
        {
            CommandSystem.Register("NecroTree", AccessLevel.Player, cmd => ShowTree(cmd.Mobile));
        }

        private static void ShowTree(Mobile m)
        {
            if (m is PlayerMobile pm)
            {
                pm.SendMessage("Opening Necromancy Skill Tree...");
                pm.SendGump(new NecromancySkillTree(pm));
            }
            else
            {
                m.SendMessage("You must be a player to use this command.");
            }
        }
    }
}
