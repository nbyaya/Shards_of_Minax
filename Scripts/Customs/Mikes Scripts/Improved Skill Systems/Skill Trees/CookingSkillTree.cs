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

namespace Server.ACC.CSS.Systems.CookingMagic
{
    public class CookingSkillTree : SuperGump
    {
        private CookingTree tree;
        private Dictionary<SkillNode, Point2D> nodePositions;
        private Dictionary<int, SkillNode> buttonNodeMap;
        private Dictionary<SkillNode, int> edgeThickness;
        private const int buttonSize = 15;
        private int ySpacing = 50, xSpacing = 60;
        private int rootX = 300, rootY = 150;
        private SkillNode selectedNode;

        public CookingSkillTree(PlayerMobile user)
            : base(user, null, 100, 100)
        {
            tree = new CookingTree(user);
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
            layout.Add("title", () => { AddLabel(100, 20, 1153, "Culinarian Skill Tree"); });

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

    // The SkillNode class (used for both cooking and lumberjacking trees)
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
            if (!profile.Talents.ContainsKey(TalentID.CookingNodes))
                profile.Talents[TalentID.CookingNodes] = new Talent(TalentID.CookingNodes) { Points = 0 };

            return (profile.Talents[TalentID.CookingNodes].Points & BitFlag) != 0;
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
            profile.Talents[TalentID.CookingNodes].Points |= BitFlag;

            player.SendMessage($"{Name} activated!");
            onActivate?.Invoke(player);
            return true;
        }
    }

    // The full Cooking tree structure – 30 nodes over nine layers.
    public class CookingTree
    {
        public SkillNode Root { get; }

        public CookingTree(PlayerMobile player)
        {
            var profile = player.AcquireTalents();
            int nodeIndex = 0x01;

            // Layer 0: Root Node – unlocks basic cooking spells.
            Root = new SkillNode(nodeIndex, "Call of the Kitchen", 5, "Unlocks basic culinary spells", (p) =>
            {
                // Unlock spell: 0x01
                profile.Talents[TalentID.CookingSpells].Points |= 0x01;
            });

            // Layer 1: Four nodes; all converted to spell unlocks.
            nodeIndex <<= 1; // becomes 0x02
            var ingredientInsight = new SkillNode(nodeIndex, "Ingredient Insight", 6, "Unlocks spell: Ingredient Insight", (p) =>
            {
                profile.Talents[TalentID.CookingSpells].Points |= 0x02;
            });

            nodeIndex <<= 1; // becomes 0x04
            var knifeMastery = new SkillNode(nodeIndex, "Knife Mastery", 6, "Unlocks spell: Knife Mastery", (p) =>
            {
                profile.Talents[TalentID.CookingSpells].Points |= 0x04;
            });

            nodeIndex <<= 1; // becomes 0x08
            var flavorFusion = new SkillNode(nodeIndex, "Flavor Fusion", 6, "Unlocks spell: Flavor Fusion", (p) =>
            {
                profile.Talents[TalentID.CookingSpells].Points |= 0x08;
            });

            nodeIndex <<= 1; // becomes 0x10
            var bountifulHarvest = new SkillNode(nodeIndex, "Bountiful Harvest", 6, "Unlocks spell: Bountiful Harvest", (p) =>
            {
                profile.Talents[TalentID.CookingSpells].Points |= 0x10;
            });

            Root.AddChild(ingredientInsight);
            Root.AddChild(knifeMastery);
            Root.AddChild(flavorFusion);
            Root.AddChild(bountifulHarvest);

            // Layer 2: Four nodes; all become spell unlocks.
            nodeIndex <<= 1; // becomes 0x20
            var spiceWhisper = new SkillNode(nodeIndex, "Spice Whisper", 7, "Unlocks spell: Spice Whisper", (p) =>
            {
                profile.Talents[TalentID.CookingSpells].Points |= 0x20;
            });

            nodeIndex <<= 1; // becomes 0x40
            var sizzlingHeat = new SkillNode(nodeIndex, "Sizzling Heat", 7, "Unlocks spell: Sizzling Heat", (p) =>
            {
                profile.Talents[TalentID.CookingSpells].Points |= 0x40;
            });

            nodeIndex <<= 1; // becomes 0x80
            var aromaticAlchemy = new SkillNode(nodeIndex, "Aromatic Alchemy", 7, "Unlocks spell: Aromatic Alchemy", (p) =>
            {
                profile.Talents[TalentID.CookingSpells].Points |= 0x80;
            });

            nodeIndex <<= 1; // becomes 0x100
            var herbHarmony = new SkillNode(nodeIndex, "Herb Harmony", 7, "Unlocks spell: Herb Harmony", (p) =>
            {
                profile.Talents[TalentID.CookingSpells].Points |= 0x100;
            });

            ingredientInsight.AddChild(spiceWhisper);
            knifeMastery.AddChild(sizzlingHeat);
            flavorFusion.AddChild(aromaticAlchemy);
            bountifulHarvest.AddChild(herbHarmony);

            // Layer 3: Four nodes; all become spell unlocks.
            nodeIndex <<= 1; // becomes 0x200
            var savorySurge = new SkillNode(nodeIndex, "Savory Surge", 8, "Unlocks spell: Savory Surge", (p) =>
            {
                profile.Talents[TalentID.CookingSpells].Points |= 0x200;
            });

            nodeIndex <<= 1; // becomes 0x400
            var rapidSear = new SkillNode(nodeIndex, "Rapid Sear", 8, "Unlocks spell: Rapid Sear", (p) =>
            {
                profile.Talents[TalentID.CookingSpells].Points |= 0x400;
            });

            nodeIndex <<= 1; // becomes 0x800
            var goldenGriddle = new SkillNode(nodeIndex, "Golden Griddle", 8, "Unlocks spell: Golden Griddle", (p) =>
            {
                profile.Talents[TalentID.CookingSpells].Points |= 0x800;
            });

            nodeIndex <<= 1; // becomes 0x1000
            var precisePrep = new SkillNode(nodeIndex, "Precise Preparation", 8, "Unlocks spell: Precise Preparation", (p) =>
            {
                profile.Talents[TalentID.CookingSpells].Points |= 0x1000;
            });

            spiceWhisper.AddChild(savorySurge);
            sizzlingHeat.AddChild(rapidSear);
            aromaticAlchemy.AddChild(goldenGriddle);
            herbHarmony.AddChild(precisePrep);

            // Layer 4: Four nodes; the first three become spell unlocks and the fourth remains passive.
            nodeIndex <<= 1; // becomes 0x2000
            var marinationMastery = new SkillNode(nodeIndex, "Marination Mastery", 9, "Unlocks spell: Marination Mastery", (p) =>
            {
                profile.Talents[TalentID.CookingSpells].Points |= 0x2000;
            });

            nodeIndex <<= 1; // becomes 0x4000
            var flameFocus = new SkillNode(nodeIndex, "Flame Focus", 9, "Unlocks spell: Flame Focus", (p) =>
            {
                profile.Talents[TalentID.CookingSpells].Points |= 0x4000;
            });

            nodeIndex <<= 1; // becomes 0x8000
            var herbalHarmony_L4 = new SkillNode(nodeIndex, "Herbal Harmony", 9, "Unlocks spell: Herbal Harmony", (p) =>
            {
                profile.Talents[TalentID.CookingSpells].Points |= 0x8000;
            });

            nodeIndex <<= 1; // next flag would be 0x10000, so leave this node as passive.
            var quickQuench = new SkillNode(nodeIndex, "Quick Quench", 9, "Boosts rapid cooking", (p) =>
            {
                profile.Talents[TalentID.CookingSpeed].Points += 1;
            });

            marinationMastery.AddChild(flameFocus);
            goldenGriddle.AddChild(herbalHarmony_L4);
            precisePrep.AddChild(quickQuench);

            // Layer 5: Expert-level nodes remain passive.
            nodeIndex <<= 1; // now 0x10000 (unused for spells)
            var primePrep = new SkillNode(nodeIndex, "Prime Prep", 10, "Boosts overall efficiency", (p) =>
            {
                profile.Talents[TalentID.CookingEfficiency].Points += 1;
            });

            nodeIndex <<= 1;
            var bountifulBanquet = new SkillNode(nodeIndex, "Bountiful Banquet", 10, "Increases ingredient yields", (p) =>
            {
                profile.Talents[TalentID.CookingFlavor].Points += 1;
            });

            nodeIndex <<= 1;
            var recipeRevelation = new SkillNode(nodeIndex, "Recipe Revelation", 10, "Enhances recipe mastery", (p) =>
            {
                profile.Talents[TalentID.CookingEfficiency].Points += 1;
            });

            nodeIndex <<= 1;
            var culinaryCadence = new SkillNode(nodeIndex, "Culinary Cadence", 10, "Enhances cooking speed", (p) =>
            {
                profile.Talents[TalentID.CookingSpeed].Points += 1;
            });

            flameFocus.AddChild(primePrep);
            herbalHarmony_L4.AddChild(bountifulBanquet);
            quickQuench.AddChild(recipeRevelation);
            rapidSear.AddChild(culinaryCadence);

            // Layer 6: Advanced mastery nodes remain passive.
            nodeIndex <<= 1;
            var expandedPalate = new SkillNode(nodeIndex, "Expanded Palate", 11, "Enhances taste perception", (p) =>
            {
                profile.Talents[TalentID.CookingFlavor].Points += 1;
            });

            nodeIndex <<= 1;
            var mysticMarinade = new SkillNode(nodeIndex, "Mystic Marinade", 11, "Boosts efficiency with secret ingredients", (p) =>
            {
                profile.Talents[TalentID.CookingEfficiency].Points += 1;
            });

            nodeIndex <<= 1;
            var ancientCulinarian = new SkillNode(nodeIndex, "Ancient Culinarian", 11, "Draws on age-old culinary wisdom", (p) =>
            {
                profile.Talents[TalentID.CookingEfficiency].Points += 1;
            });

            nodeIndex <<= 1;
            var thermalTransformation = new SkillNode(nodeIndex, "Thermal Transformation", 11, "Further increases cooking speed", (p) =>
            {
                profile.Talents[TalentID.CookingSpeed].Points += 1;
            });

            primePrep.AddChild(expandedPalate);
            bountifulBanquet.AddChild(mysticMarinade);
            recipeRevelation.AddChild(ancientCulinarian);
            culinaryCadence.AddChild(thermalTransformation);

            // Layer 7: Pinnacle bonuses remain passive.
            nodeIndex <<= 1;
            var spiceShield = new SkillNode(nodeIndex, "Spice Shield", 12, "Grants a protective barrier", (p) =>
            {
                profile.Talents[TalentID.CookingFlavor].Points += 1;
            });

            nodeIndex <<= 1;
            var naturesNurture = new SkillNode(nodeIndex, "Nature's Nurture", 12, "Further increases ingredient quality", (p) =>
            {
                profile.Talents[TalentID.CookingFlavor].Points += 1;
            });

            nodeIndex <<= 1;
            var flameFury = new SkillNode(nodeIndex, "Flame Fury", 12, "Boosts culinary power", (p) =>
            {
                profile.Talents[TalentID.CookingEfficiency].Points += 1;
            });

            nodeIndex <<= 1;
            var echoesOfAroma = new SkillNode(nodeIndex, "Echoes of Aroma", 12, "Enhances sensory speed", (p) =>
            {
                profile.Talents[TalentID.CookingSpeed].Points += 1;
            });

            expandedPalate.AddChild(spiceShield);
            mysticMarinade.AddChild(naturesNurture);
            ancientCulinarian.AddChild(flameFury);
            thermalTransformation.AddChild(echoesOfAroma);

            // Layer 8: Ultimate node remains passive.
            nodeIndex <<= 1;
            var ultimateCulinarian = new SkillNode(nodeIndex, "Ultimate Culinarian", 13, "Ultimate bonus: boosts all culinary skills", (p) =>
            {
                profile.Talents[TalentID.CookingEfficiency].Points += 1;
                profile.Talents[TalentID.CookingSpeed].Points += 1;
                profile.Talents[TalentID.CookingFlavor].Points += 1;
            });

            foreach (var node in new[] { spiceShield, echoesOfAroma, flameFury, naturesNurture })
                node.AddChild(ultimateCulinarian);
        }
    }

    // Command to open the Cooking Skill Tree.
    public class CookingSkillTreeCommand
    {
        public static void Initialize()
        {
            CommandSystem.Register("CookingTree", AccessLevel.Player, cmd => ShowTree(cmd.Mobile));
        }

        private static void ShowTree(Mobile m)
        {
            if (m is PlayerMobile pm)
            {
                pm.SendMessage("Opening Culinarian Skill Tree...");
                pm.SendGump(new CookingSkillTree(pm));
            }
            else
            {
                m.SendMessage("You must be a player to use this command.");
            }
        }
    }
}
