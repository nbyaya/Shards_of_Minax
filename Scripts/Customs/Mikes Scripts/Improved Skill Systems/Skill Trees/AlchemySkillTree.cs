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

namespace Server.ACC.CSS.Systems.AlchemyMagic
{
    // Revised Alchemy Skill Tree Gump using AncientKnowledge (Maxxia Points) as the cost resource.
    public class AlchemySkillTree : SuperGump
    {
        private AlchemyTree tree;
        private Dictionary<SkillNode, Point2D> nodePositions;
        private Dictionary<int, SkillNode> buttonNodeMap;
        private Dictionary<SkillNode, int> edgeThickness;
        private const int buttonSize = 15;
        private int ySpacing = 50, xSpacing = 60;
        private int rootX = 300, rootY = 150;
        private SkillNode selectedNode;

        public AlchemySkillTree(PlayerMobile user)
            : base(user, null, 100, 100)
        {
            tree = new AlchemyTree(user);
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

            // Ensure each node is only placed once.
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

            // Position each level's nodes centered on rootX.
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
            layout.Add("title", () => { AddLabel(100, 20, 1153, "Alchemy Skill Tree"); });

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

    // Revised SkillNode used for both unlocking alchemy spells and granting passive bonuses.
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
            if (!profile.Talents.ContainsKey(TalentID.AlchemyNodes))
                profile.Talents[TalentID.AlchemyNodes] = new Talent(TalentID.AlchemyNodes) { Points = 0 };

            return (profile.Talents[TalentID.AlchemyNodes].Points & BitFlag) != 0;
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
            profile.Talents[TalentID.AlchemyNodes].Points |= BitFlag;

            player.SendMessage($"{Name} activated!");
            onActivate?.Invoke(player);
            return true;
        }
    }

    // Full Alchemy tree structure with 30 nodes (9 layers).
    public class AlchemyTree
    {
        public SkillNode Root { get; }

        public AlchemyTree(PlayerMobile player)
        {
            var profile = player.AcquireTalents();
            int nodeIndex = 0x01;

            // Layer 0: Root Node – Unlocks basic alchemy spells.
            Root = new SkillNode(nodeIndex, "Essence of Beginnings", 5, "Unlocks basic alchemy spells", (p) =>
            {
                // Unlock basic alchemy spells.
                profile.Talents[TalentID.AlchemySpells].Points |= 0x01;
            });

            // Layer 1: Basic bonuses.
            nodeIndex <<= 1;
            var brewInstinct = new SkillNode(nodeIndex, "Brewmaster's Instinct", 6, "Increases brewing speed", (p) =>
            {
                profile.Talents[TalentID.AlchemyEfficiency].Points += 1;
            });

			nodeIndex <<= 1;
			var herbalSense = new SkillNode(nodeIndex, "Herbal Sense", 6, "Unlocks a nature-based spell", (p) =>
			{
				// Instead of increasing ingredient detection, unlock spell 0x02.
				profile.Talents[TalentID.AlchemySpells].Points |= 0x02;
			});

            nodeIndex <<= 1;
            var flaskMastery = new SkillNode(nodeIndex, "Flask Mastery", 6, "Unlocks bonus flask transformation", (p) =>
            {
                profile.Talents[TalentID.AlchemySpells].Points |= 0x04;
            });

            nodeIndex <<= 1;
            var elixirEfficiency = new SkillNode(nodeIndex, "Elixir Efficiency", 6, "Boosts potion potency", (p) =>
            {
                profile.Talents[TalentID.AlchemyPotency].Points += 1;
            });

            Root.AddChild(brewInstinct);
            Root.AddChild(herbalSense);
            Root.AddChild(flaskMastery);
            Root.AddChild(elixirEfficiency);

            // Layer 2: Advanced magical and practical bonuses.
            nodeIndex <<= 1;
            var mysticMixtures = new SkillNode(nodeIndex, "Mystic Mixtures", 7, "Unlocks additional alchemy spells", (p) =>
            {
                profile.Talents[TalentID.AlchemySpells].Points |= 0x08;
            });

            nodeIndex <<= 1;
            var rapidConcoction = new SkillNode(nodeIndex, "Rapid Concoction", 7, "Further increases brewing speed", (p) =>
            {
                profile.Talents[TalentID.AlchemyEfficiency].Points += 1;
            });

            nodeIndex <<= 1;
            var philosophersStone = new SkillNode(nodeIndex, "Philosopher's Stone", 7, "Unlocks advanced potion recipes", (p) =>
            {
                profile.Talents[TalentID.AlchemySpells].Points |= 0x10;
            });

            nodeIndex <<= 1;
            var sagesTouch = new SkillNode(nodeIndex, "Sage's Touch", 7, "Increases ingredient yield", (p) =>
            {
                profile.Talents[TalentID.AlchemyYield].Points += 1;
            });

            brewInstinct.AddChild(mysticMixtures);
            herbalSense.AddChild(rapidConcoction);
            flaskMastery.AddChild(philosophersStone);
            elixirEfficiency.AddChild(sagesTouch);

            // Layer 3: Further potency and efficiency improvements.
            nodeIndex <<= 1;
            var enchantedInfusions = new SkillNode(nodeIndex, "Enchanted Infusions", 8, "Enhances potion potency", (p) =>
            {
                profile.Talents[TalentID.AlchemyPotency].Points += 1;
            });

            nodeIndex <<= 1;
            var arcaneDistillation = new SkillNode(nodeIndex, "Arcane Distillation", 8, "Further improves brewing efficiency", (p) =>
            {
                profile.Talents[TalentID.AlchemyEfficiency].Points += 1;
            });

            nodeIndex <<= 1;
            var bottledResilience = new SkillNode(nodeIndex, "Bottled Resilience", 8, "Unlocks defensive potion effects", (p) =>
            {
                profile.Talents[TalentID.AlchemySpells].Points |= 0x20;
            });

            nodeIndex <<= 1;
            var naturalExtraction = new SkillNode(nodeIndex, "Natural Extraction", 8, "Further increases ingredient yield", (p) =>
            {
                profile.Talents[TalentID.AlchemyYield].Points += 1;
            });

            mysticMixtures.AddChild(enchantedInfusions);
            rapidConcoction.AddChild(arcaneDistillation);
            philosophersStone.AddChild(bottledResilience);
            sagesTouch.AddChild(naturalExtraction);

            // Layer 4: More advanced magical enhancements.
            nodeIndex <<= 1;
            var transmutativeGrace = new SkillNode(nodeIndex, "Transmutative Grace", 9, "Passively reduces brew times", (p) =>
            {
                profile.Talents[TalentID.AlchemyEfficiency].Points += 1;
            });

            nodeIndex <<= 1;
            var feysElixir = new SkillNode(nodeIndex, "Fey's Elixir", 9, "Unlocks fey-related bonus spells", (p) =>
            {
                profile.Talents[TalentID.AlchemySpells].Points |= 0x40;
            });

            nodeIndex <<= 1;
            var alchemicalMastery = new SkillNode(nodeIndex, "Alchemical Mastery", 9, "Unlocks mastery level potions", (p) =>
            {
                profile.Talents[TalentID.AlchemySpells].Points |= 0x80;
            });

            nodeIndex <<= 1;
            var celestialBrew = new SkillNode(nodeIndex, "Celestial Brew", 9, "Improves overall potion effects", (p) =>
            {
                profile.Talents[TalentID.AlchemyPotency].Points += 1;
            });

            enchantedInfusions.AddChild(transmutativeGrace);
            arcaneDistillation.AddChild(feysElixir);
            bottledResilience.AddChild(alchemicalMastery);
            naturalExtraction.AddChild(celestialBrew);

            // Layer 5: Expert-level nodes.
            nodeIndex <<= 1;
            var primevalConcoction = new SkillNode(nodeIndex, "Primeval Concoction", 10, "Further increases brewing efficiency", (p) =>
            {
                profile.Talents[TalentID.AlchemyEfficiency].Points += 1;
            });

            nodeIndex <<= 1;
            var bountifulBrew = new SkillNode(nodeIndex, "Bountiful Brew", 10, "Boosts ingredient yield", (p) =>
            {
                profile.Talents[TalentID.AlchemyYield].Points += 1;
            });

            nodeIndex <<= 1;
            var masterAlchemist = new SkillNode(nodeIndex, "Master Alchemist", 10, "Unlocks ultimate potion spells", (p) =>
            {
                profile.Talents[TalentID.AlchemySpells].Points |= 0x100;
            });

            nodeIndex <<= 1;
            var potionSurge = new SkillNode(nodeIndex, "Potion Surge", 10, "Grants a chance to double potion effects", (p) =>
            {
                profile.Talents[TalentID.AlchemyPotency].Points += 1;
            });

            transmutativeGrace.AddChild(primevalConcoction);
            feysElixir.AddChild(bountifulBrew);
            alchemicalMastery.AddChild(masterAlchemist);
            celestialBrew.AddChild(potionSurge);

            // Layer 6: Mastery nodes.
            nodeIndex <<= 1;
            var expandedAlchemy = new SkillNode(nodeIndex, "Expanded Alchemy", 11, "Enhances recipe discovery", (p) =>
            {
                profile.Talents[TalentID.AlchemyEfficiency].Points += 1;
            });

            nodeIndex <<= 1;
            var mysticMixtureII = new SkillNode(nodeIndex, "Mystic Mixture II", 11, "Further increases ingredient yield", (p) =>
            {
                profile.Talents[TalentID.AlchemyYield].Points += 1;
            });

            nodeIndex <<= 1;
            var ancientElixirs = new SkillNode(nodeIndex, "Ancient Elixirs", 11, "Unlocks ancient potion formulas", (p) =>
            {
                profile.Talents[TalentID.AlchemySpells].Points |= 0x200;
            });

            nodeIndex <<= 1;
            var brewedTranscendence = new SkillNode(nodeIndex, "Brewed Transcendence", 11, "Improves potion duration", (p) =>
            {
                profile.Talents[TalentID.AlchemyPotency].Points += 1;
            });

            primevalConcoction.AddChild(expandedAlchemy);
            bountifulBrew.AddChild(mysticMixtureII);
            masterAlchemist.AddChild(ancientElixirs);
            potionSurge.AddChild(brewedTranscendence);

            // Layer 7: Final, pinnacle bonuses.
            nodeIndex <<= 1;
            var alchemicalAegis = new SkillNode(nodeIndex, "Alchemical Aegis", 12, "Provides a protective potion effect", (p) =>
            {
                profile.Talents[TalentID.AlchemySpells].Points |= 0x400;
            });

			nodeIndex <<= 1;
			var elixirFortitude = new SkillNode(nodeIndex, "Elixir of Fortitude", 12, "Unlocks a fortitude spell", (p) =>
			{
				// Instead of a passive potency bonus, unlock spell 0x4000.
				profile.Talents[TalentID.AlchemySpells].Points |= 0x4000;
			});

            nodeIndex <<= 1;
            var sovereignSpirit = new SkillNode(nodeIndex, "Sovereign Spirit", 12, "Unlocks spirit-based potion effects", (p) =>
            {
                profile.Talents[TalentID.AlchemySpells].Points |= 0x800;
            });

			nodeIndex <<= 1;
			var brewedBrilliance = new SkillNode(nodeIndex, "Brewed Brilliance", 12, "Unlocks a brilliance spell", (p) =>
			{
				// Instead of a passive efficiency bonus, unlock spell 0x8000.
				profile.Talents[TalentID.AlchemySpells].Points |= 0x8000;
			});

            expandedAlchemy.AddChild(alchemicalAegis);
            mysticMixtureII.AddChild(elixirFortitude);
            ancientElixirs.AddChild(sovereignSpirit);
            brewedTranscendence.AddChild(brewedBrilliance);

            // Layer 8: Ultimate node.
            nodeIndex <<= 1;
            var ultimateAlchemist = new SkillNode(nodeIndex, "Ultimate Alchemist", 13, "Ultimate bonus: boosts all alchemy skills", (p) =>
            {
                profile.Talents[TalentID.AlchemySpells].Points |= 0x1000 | 0x2000;
                profile.Talents[TalentID.AlchemyEfficiency].Points += 1;
                profile.Talents[TalentID.AlchemyYield].Points += 1;
                profile.Talents[TalentID.AlchemyPotency].Points += 1;
            });

            // Attach the ultimate node to all Layer 7 nodes.
            foreach (var node in new[] { alchemicalAegis, brewedBrilliance, sovereignSpirit, elixirFortitude })
            {
                node.AddChild(ultimateAlchemist);
            }
        }
    }

    // Command to open the Alchemy Skill Tree.
    public class AlchemySkillTreeCommand
    {
        public static void Initialize()
        {
            CommandSystem.Register("AlchemyTree", AccessLevel.Player, cmd => ShowTree(cmd.Mobile));
        }

        private static void ShowTree(Mobile m)
        {
            if (m is PlayerMobile pm)
            {
                pm.SendMessage("Opening Alchemy Skill Tree...");
                pm.SendGump(new AlchemySkillTree(pm));
            }
            else
            {
                m.SendMessage("You must be a player to use this command.");
            }
        }
    }
}
