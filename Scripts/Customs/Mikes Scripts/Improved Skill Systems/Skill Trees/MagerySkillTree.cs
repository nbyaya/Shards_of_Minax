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

namespace Server.ACC.CSS.Systems.MageryMagic
{
    // Revised Magery Skill Tree Gump using AncientKnowledge (Maxxia Points) as the cost resource.
    public class MagerySkillTree : SuperGump
    {
        private MageryTree tree;
        private Dictionary<SkillNode, Point2D> nodePositions;
        private Dictionary<int, SkillNode> buttonNodeMap;
        private Dictionary<SkillNode, int> edgeThickness;
        private const int buttonSize = 15;
        private int ySpacing = 50, xSpacing = 60;
        private int rootX = 300, rootY = 150;
        private SkillNode selectedNode;

        public MagerySkillTree(PlayerMobile user)
            : base(user, null, 100, 100)
        {
            tree = new MageryTree(user);
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

            // Position nodes on each level, centered around rootX.
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
            layout.Add("title", () => { AddLabel(100, 20, 1153, "Magery Skill Tree"); });

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

    // Revised SkillNode class used by both skill trees.
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
            if (!profile.Talents.ContainsKey(TalentID.MageryNodes))
                profile.Talents[TalentID.MageryNodes] = new Talent(TalentID.MageryNodes) { Points = 0 };

            return (profile.Talents[TalentID.MageryNodes].Points & BitFlag) != 0;
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
            profile.Talents[TalentID.MageryNodes].Points |= BitFlag;

            player.SendMessage($"{Name} activated!");
            onActivate?.Invoke(player);
            return true;
        }
    }

    // Full Magery tree structure with multiple layers and 30 nodes.
    public class MageryTree
    {
        public SkillNode Root { get; }

        public MageryTree(PlayerMobile player)
        {
            var profile = player.AcquireTalents();
            int nodeIndex = 0x01;

            // Layer 0: Root Node – Unlocks basic magery spells.
            Root = new SkillNode(nodeIndex, "Arcane Awakening", 5, "Unlocks basic magery spells", (p) =>
            {
                profile.Talents[TalentID.MagerySpells].Points |= 0x01;
            });

            // Layer 1: Basic bonuses and spell unlocks.
            nodeIndex <<= 1;
            // Convert Mystic Focus to a spell unlock (0x800)
            var mysticFocus = new SkillNode(0x800, "Mystic Focus", 6, "Unlocks a new magery spell", (p) =>
            {
                profile.Talents[TalentID.MagerySpells].Points |= 0x800;
            });

            nodeIndex <<= 1;
            // Elemental Affinity becomes spell unlock (0x02)
            var elementalAffinity = new SkillNode(0x02, "Elemental Affinity", 6, "Unlocks elemental spells", (p) =>
            {
                profile.Talents[TalentID.MagerySpells].Points |= 0x02;
            });

            nodeIndex <<= 1;
            // Runic Empowerment becomes spell unlock (0x04)
            var runicEmpowerment = new SkillNode(0x04, "Runic Empowerment", 6, "Unlocks bonus runic spells", (p) =>
            {
                profile.Talents[TalentID.MagerySpells].Points |= 0x04;
            });

            nodeIndex <<= 1;
            // Convert Etheric Insight to a spell unlock (0x1000)
            var ethericInsight = new SkillNode(0x1000, "Etheric Insight", 6, "Unlocks a new magery spell", (p) =>
            {
                profile.Talents[TalentID.MagerySpells].Points |= 0x1000;
            });

            Root.AddChild(mysticFocus);
            Root.AddChild(elementalAffinity);
            Root.AddChild(runicEmpowerment);
            Root.AddChild(ethericInsight);

            // Layer 2: Advanced spell unlocks and bonuses.
            nodeIndex <<= 1;
            // Spell Weaving becomes spell unlock (0x08)
            var spellWeaving = new SkillNode(0x08, "Spell Weaving", 7, "Unlocks advanced magery spells", (p) =>
            {
                profile.Talents[TalentID.MagerySpells].Points |= 0x08;
            });

            nodeIndex <<= 1;
            // Convert Arcane Efficiency to a spell unlock (0x2000)
            var arcaneEfficiency = new SkillNode(0x2000, "Arcane Efficiency", 7, "Unlocks a new magery spell", (p) =>
            {
                profile.Talents[TalentID.MagerySpells].Points |= 0x2000;
            });

            nodeIndex <<= 1;
            // Convert Sigil Mastery to a spell unlock (0x4000)
            var sigilMastery = new SkillNode(0x4000, "Sigil Mastery", 7, "Unlocks a new magery spell", (p) =>
            {
                profile.Talents[TalentID.MagerySpells].Points |= 0x4000;
            });

            nodeIndex <<= 1;
            // Convert Mind Over Matter to a spell unlock (0x8000)
            var mindOverMatter = new SkillNode(0x8000, "Mind Over Matter", 7, "Unlocks a new magery spell", (p) =>
            {
                profile.Talents[TalentID.MagerySpells].Points |= 0x8000;
            });

            mysticFocus.AddChild(spellWeaving);
            elementalAffinity.AddChild(arcaneEfficiency);
            runicEmpowerment.AddChild(sigilMastery);
            ethericInsight.AddChild(mindOverMatter);

            // Layer 3: Further improvements.
            nodeIndex <<= 1;
            // Mystic Surge remains passive.
            var mysticSurge = new SkillNode(nodeIndex, "Mystic Surge", 8, "Enhances spell damage", (p) =>
            {
                profile.Talents[TalentID.MagerySpellPower].Points += 1;
            });

            nodeIndex <<= 1;
            // Focusing Runes remains passive.
            var focusingRunes = new SkillNode(nodeIndex, "Focusing Runes", 8, "Increases casting speed further", (p) =>
            {
                profile.Talents[TalentID.MageryCastSpeed].Points += 1;
            });

            nodeIndex <<= 1;
            // Enchanted Resonance becomes spell unlock (0x10)
            var enchantedResonance = new SkillNode(0x10, "Enchanted Resonance", 8, "Unlocks resonant spells", (p) =>
            {
                profile.Talents[TalentID.MagerySpells].Points |= 0x10;
            });

            nodeIndex <<= 1;
            // Arcane Shielding remains passive.
            var arcaneShielding = new SkillNode(nodeIndex, "Arcane Shielding", 8, "Boosts magic resistance", (p) =>
            {
                profile.Talents[TalentID.MageryMagicResist].Points += 1;
            });

            spellWeaving.AddChild(mysticSurge);
            arcaneEfficiency.AddChild(focusingRunes);
            sigilMastery.AddChild(enchantedResonance);
            mindOverMatter.AddChild(arcaneShielding);

            // Layer 4: More refined bonuses.
            nodeIndex <<= 1;
            // Eldritch Wisdom remains passive.
            var eldritchWisdom = new SkillNode(nodeIndex, "Eldritch Wisdom", 9, "Enhances magical knowledge", (p) =>
            {
                profile.Talents[TalentID.MageryXPBonus].Points += 1;
            });

            nodeIndex <<= 1;
            // Mana Torrent remains passive.
            var manaTorrent = new SkillNode(nodeIndex, "Mana Torrent", 9, "Accelerates mana regeneration", (p) =>
            {
                profile.Talents[TalentID.MageryManaRegen].Points += 1;
            });

            nodeIndex <<= 1;
            // Spell Synergy becomes spell unlock (0x20)
            var spellSynergy = new SkillNode(0x20, "Spell Synergy", 9, "Combines spells for greater effect", (p) =>
            {
                profile.Talents[TalentID.MagerySpells].Points |= 0x20;
            });

            nodeIndex <<= 1;
            // Temporal Distortion remains passive.
            var temporalDistortion = new SkillNode(nodeIndex, "Temporal Distortion", 9, "Reduces casting delays", (p) =>
            {
                profile.Talents[TalentID.MageryCastSpeed].Points += 1;
            });

            mysticSurge.AddChild(eldritchWisdom);
            focusingRunes.AddChild(manaTorrent);
            enchantedResonance.AddChild(spellSynergy);
            arcaneShielding.AddChild(temporalDistortion);

            // Layer 5: Expert-level improvements.
            nodeIndex <<= 1;
            // Primal Conduit remains passive.
            var primalConduit = new SkillNode(nodeIndex, "Primal Conduit", 10, "Increases overall spell power", (p) =>
            {
                profile.Talents[TalentID.MagerySpellPower].Points += 1;
            });

            nodeIndex <<= 1;
            // Sorcerers Insight remains passive.
            var sorcerersInsight = new SkillNode(nodeIndex, "Sorcerer's Insight", 10, "Enhances magical insight", (p) =>
            {
                profile.Talents[TalentID.MageryXPBonus].Points += 1;
            });

            nodeIndex <<= 1;
            // Arcane Convergence becomes spell unlock (0x40)
            var arcaneConvergence = new SkillNode(0x40, "Arcane Convergence", 10, "Unlocks convergent spells", (p) =>
            {
                profile.Talents[TalentID.MagerySpells].Points |= 0x40;
            });

            nodeIndex <<= 1;
            // Mystic Rejuvenation remains passive.
            var mysticRejuvenation = new SkillNode(nodeIndex, "Mystic Rejuvenation", 10, "Improves mana regeneration", (p) =>
            {
                profile.Talents[TalentID.MageryManaRegen].Points += 1;
            });

            eldritchWisdom.AddChild(primalConduit);
            manaTorrent.AddChild(sorcerersInsight);
            spellSynergy.AddChild(arcaneConvergence);
            temporalDistortion.AddChild(mysticRejuvenation);

            // Layer 6: Mastery nodes.
            nodeIndex <<= 1;
            // Essence of the Archmage remains passive.
            var essenceOfTheArchmage = new SkillNode(nodeIndex, "Essence of the Archmage", 11, "Passively enhances all magery abilities", (p) =>
            {
                profile.Talents[TalentID.MageryNodes].Points += 1;
            });

            nodeIndex <<= 1;
            // Runic Transmutation becomes spell unlock (0x80)
            var runicTransmutation = new SkillNode(0x80, "Runic Transmutation", 11, "Unlocks transmutation spells", (p) =>
            {
                profile.Talents[TalentID.MagerySpells].Points |= 0x80;
            });

            nodeIndex <<= 1;
            // Celestial Alignment remains passive.
            var celestialAlignment = new SkillNode(nodeIndex, "Celestial Alignment", 11, "Further increases casting speed", (p) =>
            {
                profile.Talents[TalentID.MageryCastSpeed].Points += 1;
            });

            nodeIndex <<= 1;
            // Magical Fortitude remains passive.
            var magicalFortitude = new SkillNode(nodeIndex, "Magical Fortitude", 11, "Enhances magic resistance", (p) =>
            {
                profile.Talents[TalentID.MageryMagicResist].Points += 1;
            });

            primalConduit.AddChild(essenceOfTheArchmage);
            sorcerersInsight.AddChild(runicTransmutation);
            arcaneConvergence.AddChild(celestialAlignment);
            mysticRejuvenation.AddChild(magicalFortitude);

            // Layer 7: Pinnacle bonuses.
            nodeIndex <<= 1;
            // Aetherial Barrier becomes spell unlock (0x100)
            var aetherialBarrier = new SkillNode(0x100, "Aetherial Barrier", 12, "Unlocks barrier spells", (p) =>
            {
                profile.Talents[TalentID.MagerySpells].Points |= 0x100;
            });

            nodeIndex <<= 1;
            // Luminous Vigor remains passive.
            var luminousVigor = new SkillNode(nodeIndex, "Luminous Vigor", 12, "Boosts maximum mana", (p) =>
            {
                profile.Talents[TalentID.MageryManaPool].Points += 1;
            });

            nodeIndex <<= 1;
            // Infinite Wisdom remains passive.
            var infiniteWisdom = new SkillNode(nodeIndex, "Infinite Wisdom", 12, "Increases magical experience gain", (p) =>
            {
                profile.Talents[TalentID.MageryXPBonus].Points += 1;
            });

            nodeIndex <<= 1;
            // Spellbind Mastery becomes spell unlock (0x200)
            var spellbindMastery = new SkillNode(0x200, "Spellbind Mastery", 12, "Unlocks ultimate spellcasting abilities", (p) =>
            {
                profile.Talents[TalentID.MagerySpells].Points |= 0x200;
            });

            essenceOfTheArchmage.AddChild(aetherialBarrier);
            runicTransmutation.AddChild(luminousVigor);
            celestialAlignment.AddChild(infiniteWisdom);
            magicalFortitude.AddChild(spellbindMastery);

            // Layer 8: Ultimate node.
            nodeIndex <<= 1;
            // Archmage Ascension becomes spell unlock (0x400 only)
            var archmageAscension = new SkillNode(0x400, "Archmage Ascension", 13, "Ultimate bonus: greatly enhances all magery abilities", (p) =>
            {
                profile.Talents[TalentID.MagerySpells].Points |= 0x400;
                profile.Talents[TalentID.MageryManaRegen].Points += 1;
                profile.Talents[TalentID.MageryCastSpeed].Points += 1;
                profile.Talents[TalentID.MagerySpellPower].Points += 1;
                profile.Talents[TalentID.MageryManaPool].Points += 1;
                profile.Talents[TalentID.MageryMagicResist].Points += 1;
                profile.Talents[TalentID.MageryXPBonus].Points += 1;
            });

            aetherialBarrier.AddChild(archmageAscension);
            luminousVigor.AddChild(archmageAscension);
            infiniteWisdom.AddChild(archmageAscension);
            spellbindMastery.AddChild(archmageAscension);
        }
    }

    // Command to open the Magery Skill Tree.
    public class MagerySkillTreeCommand
    {
        public static void Initialize()
        {
            CommandSystem.Register("MageryTree", AccessLevel.Player, cmd => ShowTree(cmd.Mobile));
        }

        private static void ShowTree(Mobile m)
        {
            if (m is PlayerMobile pm)
            {
                pm.SendMessage("Opening Magery Skill Tree...");
                pm.SendGump(new MagerySkillTree(pm));
            }
            else
            {
                m.SendMessage("You must be a player to use this command.");
            }
        }
    }
}
