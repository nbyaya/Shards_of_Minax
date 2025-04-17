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

namespace Server.ACC.CSS.Systems.AnimalLoreMagic
{
    public class AnimalLoreSkillTree : SuperGump
    {
        private AnimalLoreTree tree;
        private Dictionary<AnimalLoreSkillNode, Point2D> nodePositions;
        private Dictionary<int, AnimalLoreSkillNode> buttonNodeMap;
        private Dictionary<AnimalLoreSkillNode, int> edgeThickness;
        private const int buttonSize = 15;
        private int ySpacing = 50, xSpacing = 60;
        private int rootX = 300, rootY = 150;
        private AnimalLoreSkillNode selectedNode;

        public AnimalLoreSkillTree(PlayerMobile user)
            : base(user, null, 100, 100)
        {
            tree = new AnimalLoreTree(user);
            nodePositions = new Dictionary<AnimalLoreSkillNode, Point2D>();
            buttonNodeMap = new Dictionary<int, AnimalLoreSkillNode>();
            edgeThickness = new Dictionary<AnimalLoreSkillNode, int>();

            CalculateNodePositions(tree.Root, rootX, rootY, 0);
            InitializeEdgeThickness();

            User.SendGump(this);
        }

        private void CalculateNodePositions(AnimalLoreSkillNode root, int x, int y, int depth)
        {
            if (root == null)
                return;

            var levelNodes = new Dictionary<int, List<AnimalLoreSkillNode>>();
            var queue = new Queue<(AnimalLoreSkillNode node, int level)>();
            var visited = new HashSet<AnimalLoreSkillNode>();

            queue.Enqueue((root, 0));

            while (queue.Count > 0)
            {
                var (node, level) = queue.Dequeue();

                if (!visited.Add(node))
                    continue;

                if (!levelNodes.ContainsKey(level))
                    levelNodes[level] = new List<AnimalLoreSkillNode>();

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
            layout.Add("title", () => { AddLabel(100, 20, 1153, "Animal Lore Skill Tree"); });

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
            if (buttonNodeMap.TryGetValue(button.ButtonID, out AnimalLoreSkillNode node))
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

    public class AnimalLoreSkillNode
    {
        public int BitFlag { get; }
        public string Name { get; }
        public int Cost { get; }
        public string Description { get; }
        public List<AnimalLoreSkillNode> Children { get; }
        public AnimalLoreSkillNode Parent { get; private set; }
        private readonly Action<PlayerMobile> onActivate;

        public AnimalLoreSkillNode(int bitFlag, string name, int cost, string description = "", Action<PlayerMobile> onActivate = null)
        {
            BitFlag = bitFlag;
            Name = name;
            Cost = cost;
            Description = description;
            Children = new List<AnimalLoreSkillNode>();
            this.onActivate = onActivate;
        }

        public void AddChild(AnimalLoreSkillNode child)
        {
            child.Parent = this;
            Children.Add(child);
        }

        public bool IsActivated(PlayerMobile player)
        {
            var profile = player.AcquireTalents();
            if (!profile.Talents.ContainsKey(TalentID.AnimalLoreNodes))
                profile.Talents[TalentID.AnimalLoreNodes] = new Talent(TalentID.AnimalLoreNodes) { Points = 0 };

            return (profile.Talents[TalentID.AnimalLoreNodes].Points & BitFlag) != 0;
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
            profile.Talents[TalentID.AnimalLoreNodes].Points |= BitFlag;

            player.SendMessage($"{Name} activated!");
            onActivate?.Invoke(player);
            return true;
        }
    }

    public class AnimalLoreTree
    {
        public AnimalLoreSkillNode Root { get; }

        public AnimalLoreTree(PlayerMobile player)
        {
            var profile = player.AcquireTalents();
            int nodeIndex = 0x01;

            // Layer 0: Root Node – Unlocks basic Animal Lore spells.
            Root = new AnimalLoreSkillNode(nodeIndex, "Call of the Wild", 5, "Unlocks basic Animal Lore spells", (p) =>
            {
                profile.Talents[TalentID.AnimalLoreSpells].Points |= 0x01;
            });

            // Layer 1: Basic bonuses.
            nodeIndex <<= 1;
            var beastSense = new AnimalLoreSkillNode(nodeIndex, "Beast Sense", 6, "Increases detection range of animals", (p) =>
            {
                profile.Talents[TalentID.AnimalLoreRange].Points += 1;
            });

            nodeIndex <<= 1;
            var feralAgility = new AnimalLoreSkillNode(nodeIndex, "Feral Agility", 6, "Enhances movement speed when near animals", (p) =>
            {
                profile.Talents[TalentID.AnimalLoreAgility].Points += 1;
            });

            nodeIndex <<= 1;
            var predatorInstinct = new AnimalLoreSkillNode(nodeIndex, "Predator's Instinct", 6, "Unlocks bonus animal lore spells", (p) =>
            {
                profile.Talents[TalentID.AnimalLoreSpells].Points |= 0x04;
            });

            nodeIndex <<= 1;
            var natureBond = new AnimalLoreSkillNode(nodeIndex, "Nature's Bond", 6, "Improves animal taming success", (p) =>
            {
                profile.Talents[TalentID.AnimalLoreTaming].Points += 1;
            });

            Root.AddChild(beastSense);
            Root.AddChild(feralAgility);
            Root.AddChild(predatorInstinct);
            Root.AddChild(natureBond);

            // Layer 2: Advanced magical and practical bonuses.
            nodeIndex <<= 1;
            var primalWhisper = new AnimalLoreSkillNode(nodeIndex, "Primal Whisper", 7, "Unlocks additional animal lore spells", (p) =>
            {
                profile.Talents[TalentID.AnimalLoreSpells].Points |= 0x08;
            });

            nodeIndex <<= 1;
            var packTactics = new AnimalLoreSkillNode(nodeIndex, "Pack Tactics", 7, "Enhances group animal interactions", (p) =>
            {
                profile.Talents[TalentID.AnimalLoreGroup].Points += 1;
            });

            nodeIndex <<= 1;
            var beastlyMight = new AnimalLoreSkillNode(nodeIndex, "Beastly Might", 7, "Unlocks advanced animal lore spells", (p) =>
            {
                profile.Talents[TalentID.AnimalLoreSpells].Points |= 0x10;
            });

            nodeIndex <<= 1;
            var wildEmpathy = new AnimalLoreSkillNode(nodeIndex, "Wild Empathy", 7, "Increases animal friendliness", (p) =>
            {
                profile.Talents[TalentID.AnimalLoreEmpathy].Points += 1;
            });

            beastSense.AddChild(primalWhisper);
            feralAgility.AddChild(packTactics);
            predatorInstinct.AddChild(beastlyMight);
            natureBond.AddChild(wildEmpathy);

            // Layer 3: Further bonuses.
            nodeIndex <<= 1;
            var savageResilience = new AnimalLoreSkillNode(nodeIndex, "Savage Resilience", 8, "Boosts health regeneration in the wild", (p) =>
            {
                profile.Talents[TalentID.AnimalLoreHealth].Points += 1;
            });

            nodeIndex <<= 1;
            var huntersFocus = new AnimalLoreSkillNode(nodeIndex, "Hunter's Focus", 8, "Improves spell casting concentration", (p) =>
            {
                profile.Talents[TalentID.AnimalLoreFocus].Points += 1;
            });

            nodeIndex <<= 1;
            var alphaRoar = new AnimalLoreSkillNode(nodeIndex, "Alpha Roar", 8, "Unlocks a powerful roar spell", (p) =>
            {
                profile.Talents[TalentID.AnimalLoreSpells].Points |= 0x20;
            });

            nodeIndex <<= 1;
            var camouflage = new AnimalLoreSkillNode(nodeIndex, "Camouflage", 8, "Grants a bonus to stealth detection", (p) =>
            {
                profile.Talents[TalentID.AnimalLoreStealth].Points += 1;
            });

            primalWhisper.AddChild(savageResilience);
            packTactics.AddChild(huntersFocus);
            beastlyMight.AddChild(alphaRoar);
            wildEmpathy.AddChild(camouflage);

            // Layer 4: More advanced magical enhancements.
            nodeIndex <<= 1;
            var spiritOfThePack = new AnimalLoreSkillNode(nodeIndex, "Spirit of the Pack", 9, "Enhances group coordination", (p) =>
            {
                profile.Talents[TalentID.AnimalLoreGroup].Points += 1;
            });

            nodeIndex <<= 1;
            var callOfTheHunt = new AnimalLoreSkillNode(nodeIndex, "Call of the Hunt", 9, "Unlocks a bonus hunting spell", (p) =>
            {
                profile.Talents[TalentID.AnimalLoreSpells].Points |= 0x40;
            });

            nodeIndex <<= 1;
            var instinctiveReflexes = new AnimalLoreSkillNode(nodeIndex, "Instinctive Reflexes", 9, "Improves reaction speed in combat", (p) =>
            {
                profile.Talents[TalentID.AnimalLoreFocus].Points += 1;
            });

            nodeIndex <<= 1;
            var wildProwess = new AnimalLoreSkillNode(nodeIndex, "Wild Prowess", 9, "Increases damage with animal companions", (p) =>
            {
                profile.Talents[TalentID.AnimalLoreDamage].Points += 1;
            });

            spiritOfThePack.AddChild(callOfTheHunt);
            huntersFocus.AddChild(instinctiveReflexes);
            // Attach wildProwess to spiritOfThePack for this example.
            spiritOfThePack.AddChild(wildProwess);

            // Layer 5: Expert-level nodes.
            nodeIndex <<= 1;
            var predatorEfficiency = new AnimalLoreSkillNode(nodeIndex, "Predator Efficiency", 10, "Boosts overall combat efficiency", (p) =>
            {
                profile.Talents[TalentID.AnimalLoreAgility].Points += 1;
            });

            nodeIndex <<= 1;
            var primalHarvest = new AnimalLoreSkillNode(nodeIndex, "Primal Harvest", 10, "Enhances rewards from hunts", (p) =>
            {
                profile.Talents[TalentID.AnimalLoreHealth].Points += 1;
            });

            nodeIndex <<= 1;
            var animalMastery = new AnimalLoreSkillNode(nodeIndex, "Animal Mastery", 10, "Unlocks mastery level animal lore spells", (p) =>
            {
                profile.Talents[TalentID.AnimalLoreSpells].Points |= 0x100;
            });

            nodeIndex <<= 1;
            var feralMomentum = new AnimalLoreSkillNode(nodeIndex, "Feral Momentum", 10, "Increases movement and attack speed", (p) =>
            {
                profile.Talents[TalentID.AnimalLoreAgility].Points += 1;
            });

            callOfTheHunt.AddChild(predatorEfficiency);
            instinctiveReflexes.AddChild(primalHarvest);
            wildProwess.AddChild(animalMastery);
            wildProwess.AddChild(feralMomentum);

            // Layer 6: Mastery nodes.
            nodeIndex <<= 1;
            var enhancedPerception = new AnimalLoreSkillNode(nodeIndex, "Enhanced Perception", 11, "Heightens senses for detecting prey", (p) =>
            {
                profile.Talents[TalentID.AnimalLoreFocus].Points += 1;
            });

            nodeIndex <<= 1;
            var mysticFamiliar = new AnimalLoreSkillNode(nodeIndex, "Mystic Familiar", 11, "Boosts effectiveness of summoned creatures", (p) =>
            {
                profile.Talents[TalentID.AnimalLoreSummon].Points += 1;
            });

            nodeIndex <<= 1;
            var ancientBeastmaster = new AnimalLoreSkillNode(nodeIndex, "Ancient Beastmaster", 11, "Unlocks ancient animal lore spells", (p) =>
            {
                profile.Talents[TalentID.AnimalLoreSpells].Points |= 0x200;
            });

            nodeIndex <<= 1;
            var natureTransformation = new AnimalLoreSkillNode(nodeIndex, "Nature's Transformation", 11, "Enhances physical abilities with nature", (p) =>
            {
                profile.Talents[TalentID.AnimalLoreDamage].Points += 1;
            });

            predatorEfficiency.AddChild(enhancedPerception);
            primalHarvest.AddChild(mysticFamiliar);
            animalMastery.AddChild(ancientBeastmaster);
            feralMomentum.AddChild(natureTransformation);

            // Layer 7: Final, pinnacle bonuses.
            nodeIndex <<= 1;
            var furBarrier = new AnimalLoreSkillNode(nodeIndex, "Fur Barrier", 12, "Provides a protective barrier", (p) =>
            {
                profile.Talents[TalentID.AnimalLoreSpells].Points |= 0x400;
            });

            nodeIndex <<= 1;
            var wildEndowment = new AnimalLoreSkillNode(nodeIndex, "Wild Endowment", 12, "Further enhances animal affinity", (p) =>
            {
                profile.Talents[TalentID.AnimalLoreHealth].Points += 1;
            });

            nodeIndex <<= 1;
            var savageFury = new AnimalLoreSkillNode(nodeIndex, "Savage Fury", 12, "Boosts physical attack power", (p) =>
            {
                profile.Talents[TalentID.AnimalLoreDamage].Points += 1;
            });

            nodeIndex <<= 1;
            var echoesOfTheWild = new AnimalLoreSkillNode(nodeIndex, "Echoes of the Wild", 12, "Enhances speed and agility", (p) =>
            {
                profile.Talents[TalentID.AnimalLoreAgility].Points += 1;
            });

            enhancedPerception.AddChild(furBarrier);
            mysticFamiliar.AddChild(wildEndowment);
            ancientBeastmaster.AddChild(savageFury);
            natureTransformation.AddChild(echoesOfTheWild);

            // Layer 8: Ultimate node.
            nodeIndex <<= 1;
            var ultimateBeastmaster = new AnimalLoreSkillNode(nodeIndex, "Ultimate Beastmaster", 13, "Ultimate bonus: boosts all animal lore skills", (p) =>
            {
                profile.Talents[TalentID.AnimalLoreSpells].Points |= 0x800 | 0x1000;
                profile.Talents[TalentID.AnimalLoreAgility].Points += 1;
                profile.Talents[TalentID.AnimalLoreFocus].Points += 1;
                profile.Talents[TalentID.AnimalLoreHealth].Points += 1;
            });

            furBarrier.AddChild(ultimateBeastmaster);
            wildEndowment.AddChild(ultimateBeastmaster);
            savageFury.AddChild(ultimateBeastmaster);
            echoesOfTheWild.AddChild(ultimateBeastmaster);
        }
    }

    // Command to open the Animal Lore Skill Tree.
    public class AnimalLoreSkillTreeCommand
    {
        public static void Initialize()
        {
            CommandSystem.Register("AnimalLoreTree", AccessLevel.Player, cmd => ShowTree(cmd.Mobile));
        }

        private static void ShowTree(Mobile m)
        {
            if (m is PlayerMobile pm)
            {
                pm.SendMessage("Opening Animal Lore Skill Tree...");
                pm.SendGump(new AnimalLoreSkillTree(pm));
            }
            else
            {
                m.SendMessage("You must be a player to use this command.");
            }
        }
    }
}
