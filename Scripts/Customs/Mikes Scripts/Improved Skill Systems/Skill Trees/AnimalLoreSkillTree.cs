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
    // Revised Animal Lore Skill Tree Gump using AncientKnowledge (Maxxia Points) as the cost resource.
    public class AnimalLoreSkillTree : SuperGump
    {
        private AnimalLoreTree tree;
        private Dictionary<SkillNode, Point2D> nodePositions;
        private Dictionary<int, SkillNode> buttonNodeMap;
        private Dictionary<SkillNode, int> edgeThickness;
        private const int buttonSize = 15;
        private int ySpacing = 50, xSpacing = 60;
        private int rootX = 300, rootY = 150;
        private SkillNode selectedNode;

        public AnimalLoreSkillTree(PlayerMobile user)
            : base(user, null, 100, 100)
        {
            tree = new AnimalLoreTree(user);
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
                    queue.Enqueue((child, level + 1));
            }

            // Position each level’s nodes centered on rootX.
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

            // New layout element to display the node’s description.
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

    // Revised SkillNode that uses AncientKnowledge (Maxxia Points) for costs.
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

    // Full Animal Lore tree structure with multiple layers and 30 nodes.
    public class AnimalLoreTree
    {
        public SkillNode Root { get; }

        public AnimalLoreTree(PlayerMobile player)
        {
            var profile = player.AcquireTalents();
            // We'll use the lower 16 bits (the ones given) for spell unlock nodes.
            // Passive nodes get bit flags starting at 0x10000 so as not to conflict.
            int passiveBit = 0x10000;

            // Layer 0: Root Node – Spell node.
            Root = new SkillNode(0x01, "Call of the Wild", 5, "Unlocks basic Animal Lore spells and abilities", (p) =>
            {
                profile.Talents[TalentID.AnimalLoreSpells].Points |= 0x01;
            });

            // Layer 1: Four nodes.
            // Spell node:
            var beastSense = new SkillNode(0x02, "Beast Sense", 6, "Unlocks spell: Beast Sense", (p) =>
            {
                profile.Talents[TalentID.AnimalLoreSpells].Points |= 0x02;
            });
            // Passive nodes:
            var naturalBond = new SkillNode(passiveBit, "Natural Bond", 6, "Improves your ability to communicate with animals", (p) =>
            {
                profile.Talents[TalentID.MinionDamageBonus].Points += 1;
            });
            passiveBit <<= 1;
            var wildInstincts = new SkillNode(passiveBit, "Wild Instincts", 6, "Sharpens your natural instincts", (p) =>
            {
                profile.Talents[TalentID.MinionDamageBonus].Points += 1;
            });
            passiveBit <<= 1;
            var feralTouch = new SkillNode(passiveBit, "Feral Touch", 6, "Improves your handling and taming of beasts", (p) =>
            {
                profile.Talents[TalentID.MinionDamageBonus].Points += 1;
            });
            passiveBit <<= 1;

            Root.AddChild(beastSense);
            Root.AddChild(naturalBond);
            Root.AddChild(wildInstincts);
            Root.AddChild(feralTouch);

            // Layer 2: Four nodes.
            // Spell nodes:
            var roarOfTheWild = new SkillNode(0x04, "Roar of the Wild", 7, "Unlocks spell: Roar of the Wild", (p) =>
            {
                profile.Talents[TalentID.AnimalLoreSpells].Points |= 0x04;
            });
            var trackingProwess = new SkillNode(0x08, "Tracking Prowess", 7, "Unlocks spell: Tracking Prowess", (p) =>
            {
                profile.Talents[TalentID.AnimalLoreSpells].Points |= 0x08;
            });
            // Passive nodes:
            var primalCommunication = new SkillNode(passiveBit, "Primal Communication", 7, "Unlocks bonus abilities to understand animals", (p) =>
            {
                profile.Talents[TalentID.MinionDamageBonus].Points += 1;
            });
            passiveBit <<= 1;
            var savageBond = new SkillNode(passiveBit, "Savage Bond", 7, "Strengthens your connection with beasts", (p) =>
            {
                profile.Talents[TalentID.MinionDamageBonus].Points += 1;
            });
            passiveBit <<= 1;

            beastSense.AddChild(roarOfTheWild);
            naturalBond.AddChild(primalCommunication);
            wildInstincts.AddChild(trackingProwess);
            feralTouch.AddChild(savageBond);

            // Layer 3: Four nodes.
            // Spell nodes:
            var predatorsGrace = new SkillNode(0x10, "Predator's Grace", 8, "Unlocks spell: Predator's Grace", (p) =>
            {
                profile.Talents[TalentID.AnimalLoreSpells].Points |= 0x10;
            });
            // Passive node:
            var gentleWhisper = new SkillNode(passiveBit, "Gentle Whisper", 8, "Calms even the wildest beasts", (p) =>
            {
                profile.Talents[TalentID.MinionDamageBonus].Points += 1;
            });
            passiveBit <<= 1;
            // Spell node:
            var wildFortitude = new SkillNode(0x20, "Wild Fortitude", 8, "Unlocks spell: Wild Fortitude", (p) =>
            {
                profile.Talents[TalentID.AnimalLoreSpells].Points |= 0x20;
            });
            // Passive node:
            var spiritKinship = new SkillNode(passiveBit, "Spirit Kinship", 8, "Deepens your connection with animal spirits", (p) =>
            {
                profile.Talents[TalentID.MinionDamageBonus].Points += 1;
            });
            passiveBit <<= 1;

            roarOfTheWild.AddChild(predatorsGrace);
            trackingProwess.AddChild(gentleWhisper);
            primalCommunication.AddChild(wildFortitude);
            savageBond.AddChild(spiritKinship);

            // Layer 4: Four nodes.
            // Spell nodes:
            var callOfThePack = new SkillNode(0x40, "Call of the Pack", 9, "Unlocks spell: Call of the Pack", (p) =>
            {
                profile.Talents[TalentID.AnimalLoreSpells].Points |= 0x40;
            });
            var naturesEcho = new SkillNode(0x80, "Nature's Echo", 9, "Unlocks spell: Nature's Echo", (p) =>
            {
                profile.Talents[TalentID.AnimalLoreSpells].Points |= 0x80;
            });
            // Passive nodes:
            var tamersTouch = new SkillNode(passiveBit, "Tamer's Touch", 9, "Unlocks advanced taming techniques", (p) =>
            {
                profile.Talents[TalentID.MinionDamageBonus].Points += 1;
            });
            passiveBit <<= 1;
            var wildInsight = new SkillNode(passiveBit, "Wild Insight", 9, "Enhances your natural instincts", (p) =>
            {
                profile.Talents[TalentID.MinionDamageBonus].Points += 1;
            });
            passiveBit <<= 1;

            predatorsGrace.AddChild(callOfThePack);
            gentleWhisper.AddChild(naturesEcho);
            wildFortitude.AddChild(tamersTouch);
            spiritKinship.AddChild(wildInsight);

            // Layer 5: Four nodes.
            // Spell nodes:
            var primevalEmpathy = new SkillNode(0x100, "Primeval Empathy", 10, "Unlocks spell: Primeval Empathy", (p) =>
            {
                profile.Talents[TalentID.AnimalLoreSpells].Points |= 0x100;
            });
            var animalMastery = new SkillNode(0x200, "Animal Mastery", 10, "Unlocks spell: Animal Mastery", (p) =>
            {
                profile.Talents[TalentID.AnimalLoreSpells].Points |= 0x200;
            });
            // Passive nodes:
            var beastlyBond = new SkillNode(passiveBit, "Beastly Bond", 10, "Strengthens your bond with creatures", (p) =>
            {
                profile.Talents[TalentID.MinionDamageBonus].Points += 1;
            });
            passiveBit <<= 1;
            var wildMomentum = new SkillNode(passiveBit, "Wild Momentum", 10, "Increases your animal handling speed", (p) =>
            {
                profile.Talents[TalentID.MinionDamageBonus].Points += 1;
            });
            passiveBit <<= 1;

            callOfThePack.AddChild(primevalEmpathy);
            naturesEcho.AddChild(beastlyBond);
            tamersTouch.AddChild(animalMastery);
            wildInsight.AddChild(wildMomentum);

            // Layer 6: Four nodes.
            // Spell nodes:
            var expandedSenses = new SkillNode(0x400, "Expanded Senses", 11, "Unlocks spell: Expanded Senses", (p) =>
            {
                profile.Talents[TalentID.AnimalLoreSpells].Points |= 0x400;
            });
            // Passive node:
            var mysticMenagerie = new SkillNode(passiveBit, "Mystic Menagerie", 11, "Boosts your Animal Lore spells", (p) =>
            {
                profile.Talents[TalentID.MinionDamageBonus].Points += 1;
            });
            passiveBit <<= 1;
            // Spell node:
            var ancientBestiary = new SkillNode(0x800, "Ancient Bestiary", 11, "Unlocks spell: Ancient Bestiary", (p) =>
            {
                profile.Talents[TalentID.AnimalLoreSpells].Points |= 0x800;
            });
            // Passive node:
            var feralTransformation = new SkillNode(passiveBit, "Feral Transformation", 11, "Improves your handling with beasts", (p) =>
            {
                profile.Talents[TalentID.MinionDamageBonus].Points += 1;
            });
            passiveBit <<= 1;

            primevalEmpathy.AddChild(expandedSenses);
            beastlyBond.AddChild(mysticMenagerie);
            animalMastery.AddChild(ancientBestiary);
            wildMomentum.AddChild(feralTransformation);

            // Layer 7: Four nodes.
            // Spell nodes:
            var barkOfTheBeast = new SkillNode(0x1000, "Bark of the Beast", 12, "Unlocks spell: Bark of the Beast", (p) =>
            {
                profile.Talents[TalentID.AnimalLoreSpells].Points |= 0x1000;
            });
            var naturesEndowment = new SkillNode(0x2000, "Nature's Endowment", 12, "Unlocks spell: Nature's Endowment", (p) =>
            {
                profile.Talents[TalentID.AnimalLoreSpells].Points |= 0x2000;
            });
            var savageFury = new SkillNode(0x4000, "Savage Fury", 12, "Unlocks spell: Savage Fury", (p) =>
            {
                profile.Talents[TalentID.AnimalLoreSpells].Points |= 0x4000;
            });
            // Passive node:
            var echoesOfTheWild = new SkillNode(passiveBit, "Echoes of the Wild", 12, "Enhances your wild instincts", (p) =>
            {
                profile.Talents[TalentID.MinionDamageBonus].Points += 1;
            });
            passiveBit <<= 1;

            expandedSenses.AddChild(barkOfTheBeast);
            mysticMenagerie.AddChild(naturesEndowment);
            ancientBestiary.AddChild(savageFury);
            feralTransformation.AddChild(echoesOfTheWild);

            // Layer 8: Ultimate node.
            var ultimateBeastmaster = new SkillNode(0x8000, "Ultimate Beastmaster", 13, "Unlocks spell: Ultimate Beastmaster. Ultimate bonus: boosts all Animal Lore abilities", (p) =>
            {
                profile.Talents[TalentID.AnimalLoreSpells].Points |= 0x8000;
                profile.Talents[TalentID.MinionDamageBonus].Points += 1;
                profile.Talents[TalentID.MinionDamageBonus].Points += 1;
                profile.Talents[TalentID.MinionDamageBonus].Points += 1;
                profile.Talents[TalentID.MinionDamageBonus].Points += 1;
            });

            barkOfTheBeast.AddChild(ultimateBeastmaster);
            echoesOfTheWild.AddChild(ultimateBeastmaster);
            savageFury.AddChild(ultimateBeastmaster);
            naturesEndowment.AddChild(ultimateBeastmaster);
        }
    }
}
