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

namespace Server.ACC.CSS.Systems.InscribeMagic
{
    // Revised Inscription Skill Tree Gump using Maxxia Points (AncientKnowledge) as the cost resource.
    public class InscriptionSkillTree : SuperGump
    {
        private InscriptionTree tree;
        private Dictionary<SkillNode, Point2D> nodePositions;
        private Dictionary<int, SkillNode> buttonNodeMap;
        private Dictionary<SkillNode, int> edgeThickness;
        private const int buttonSize = 15;
        private int ySpacing = 50, xSpacing = 60;
        private int rootX = 300, rootY = 150;
        private SkillNode selectedNode;

        public InscriptionSkillTree(PlayerMobile user)
            : base(user, null, 100, 100)
        {
            tree = new InscriptionTree(user);
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

            // Center nodes at each level around rootX.
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
            layout.Add("title", () => { AddLabel(100, 20, 1153, "Inscription Skill Tree"); });

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

    // Revised SkillNode for Inscription that uses Maxxia Points (AncientKnowledge) as the cost resource.
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
            if (!profile.Talents.ContainsKey(TalentID.InscribeNodes))
                profile.Talents[TalentID.InscribeNodes] = new Talent(TalentID.InscribeNodes) { Points = 0 };

            return (profile.Talents[TalentID.InscribeNodes].Points & BitFlag) != 0;
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
            profile.Talents[TalentID.InscribeNodes].Points |= BitFlag;

            player.SendMessage($"{Name} activated!");
            onActivate?.Invoke(player);
            return true;
        }
    }

    // Full Inscription tree structure with 30 nodes (mirroring the Lumberjacking tree)
    public class InscriptionTree
    {
        public SkillNode Root { get; }

        public InscriptionTree(PlayerMobile player)
        {
            var profile = player.AcquireTalents();
            int nodeIndex = 0x01;

            // Layer 0: Root Node – Unlocks basic inscription spells.
            Root = new SkillNode(nodeIndex, "Call of the Quill", 5, "Unlocks basic inscription spells", (p) =>
            {
                profile.Talents[TalentID.InscribeSpells].Points |= 0x01;
            });

            // Layer 1: Basic bonuses and spell unlock.
            nodeIndex <<= 1;
            // Changed Steady Hand to unlock spell 0x02.
            var steadyHand = new SkillNode(nodeIndex, "Steady Hand", 6, "Unlocks inscription spell 0x02", (p) =>
            {
                profile.Talents[TalentID.InscribeSpells].Points |= 0x02;
            });

            nodeIndex <<= 1;
            var inkMastery = new SkillNode(nodeIndex, "Ink Mastery", 6, "Improves ink efficiency", (p) =>
            {
                profile.Talents[TalentID.InscribeEfficiency].Points += 1;
            });

            nodeIndex <<= 1;
            var scrollScribe = new SkillNode(nodeIndex, "Scroll Scribe", 6, "Unlocks bonus scroll crafting", (p) =>
            {
                profile.Talents[TalentID.InscribeSpells].Points |= 0x04;
            });

            nodeIndex <<= 1;
            var parchmentPrep = new SkillNode(nodeIndex, "Parchment Preparation", 6, "Enhances parchment quality", (p) =>
            {
                profile.Talents[TalentID.InscribeYield].Points += 1;
            });

            Root.AddChild(steadyHand);
            Root.AddChild(inkMastery);
            Root.AddChild(scrollScribe);
            Root.AddChild(parchmentPrep);

            // Layer 2: Advanced magical and practical bonuses.
            nodeIndex <<= 1;
            var mysticInks = new SkillNode(nodeIndex, "Mystic Inks", 7, "Unlocks additional inscription spells", (p) =>
            {
                profile.Talents[TalentID.InscribeSpells].Points |= 0x08;
            });

            nodeIndex <<= 1;
            var etherealScript = new SkillNode(nodeIndex, "Ethereal Script", 7, "Improves spell glyph potency", (p) =>
            {
                profile.Talents[TalentID.InscribeEfficiency].Points += 1;
            });

            nodeIndex <<= 1;
            var arcaneGlyphs = new SkillNode(nodeIndex, "Arcane Glyphs", 7, "Unlocks advanced inscription spells", (p) =>
            {
                profile.Talents[TalentID.InscribeSpells].Points |= 0x10;
            });

            nodeIndex <<= 1;
            var enchantedScrolls = new SkillNode(nodeIndex, "Enchanted Scrolls", 7, "Enhances scroll durability", (p) =>
            {
                profile.Talents[TalentID.InscribeYield].Points += 1;
            });

            steadyHand.AddChild(mysticInks);
            inkMastery.AddChild(etherealScript);
            scrollScribe.AddChild(arcaneGlyphs);
            parchmentPrep.AddChild(enchantedScrolls);

            // Layer 3: Further improvements.
            nodeIndex <<= 1;
            var illuminatedManuscripts = new SkillNode(nodeIndex, "Illuminated Manuscripts", 8, "Boosts spell clarity", (p) =>
            {
                profile.Talents[TalentID.InscribeAccuracy].Points += 1;
            });

            nodeIndex <<= 1;
            var magicalMargins = new SkillNode(nodeIndex, "Magical Margins", 8, "Enhances magical efficiency", (p) =>
            {
                profile.Talents[TalentID.InscribeEfficiency].Points += 1;
            });

            nodeIndex <<= 1;
            var scribesPrecision = new SkillNode(nodeIndex, "Scribe's Precision", 8, "Unlocks precision-based spells", (p) =>
            {
                profile.Talents[TalentID.InscribeSpells].Points |= 0x20;
            });

            nodeIndex <<= 1;
            var elegantCalligraphy = new SkillNode(nodeIndex, "Elegant Calligraphy", 8, "Improves crafting outcomes", (p) =>
            {
                profile.Talents[TalentID.InscribeYield].Points += 1;
            });

            mysticInks.AddChild(illuminatedManuscripts);
            etherealScript.AddChild(magicalMargins);
            arcaneGlyphs.AddChild(scribesPrecision);
            enchantedScrolls.AddChild(elegantCalligraphy);

            // Layer 4: More advanced magical enhancements.
            nodeIndex <<= 1;
            var runicResonance = new SkillNode(nodeIndex, "Runic Resonance", 9, "Enhances rune power", (p) =>
            {
                profile.Talents[TalentID.InscribeYield].Points += 1;
            });

            nodeIndex <<= 1;
            var inkOfInsight = new SkillNode(nodeIndex, "Ink of Insight", 9, "Unlocks insight spells", (p) =>
            {
                profile.Talents[TalentID.InscribeSpells].Points |= 0x40;
            });

            nodeIndex <<= 1;
            var sigilOfPower = new SkillNode(nodeIndex, "Sigil of Power", 9, "Boosts inscription power", (p) =>
            {
                profile.Talents[TalentID.InscribeAccuracy].Points += 1;
            });

            nodeIndex <<= 1;
            var scrollOfSecrets = new SkillNode(nodeIndex, "Scroll of Secrets", 9, "Unlocks secret spells", (p) =>
            {
                profile.Talents[TalentID.InscribeSpells].Points |= 0x80;
            });

            illuminatedManuscripts.AddChild(runicResonance);
            magicalMargins.AddChild(inkOfInsight);
            scribesPrecision.AddChild(sigilOfPower);
            elegantCalligraphy.AddChild(scrollOfSecrets);

            // Layer 5: Expert-level nodes.
            nodeIndex <<= 1;
            var masterScribe = new SkillNode(nodeIndex, "Master Scribe", 10, "Enhances overall inscription skill", (p) =>
            {
                profile.Talents[TalentID.InscribeEfficiency].Points += 1;
            });

            nodeIndex <<= 1;
            var eloquentExpressions = new SkillNode(nodeIndex, "Eloquent Expressions", 10, "Boosts inscription accuracy", (p) =>
            {
                profile.Talents[TalentID.InscribeAccuracy].Points += 1;
            });

            nodeIndex <<= 1;
            var glyphGuardian = new SkillNode(nodeIndex, "Glyph Guardian", 10, "Unlocks guardian spells", (p) =>
            {
                profile.Talents[TalentID.InscribeSpells].Points |= 0x100;
            });

            nodeIndex <<= 1;
            var spellbindingStrokes = new SkillNode(nodeIndex, "Spellbinding Strokes", 10, "Enhances magical inscriptions", (p) =>
            {
                profile.Talents[TalentID.InscribeYield].Points += 1;
            });

            runicResonance.AddChild(masterScribe);
            inkOfInsight.AddChild(eloquentExpressions);
            sigilOfPower.AddChild(glyphGuardian);
            scrollOfSecrets.AddChild(spellbindingStrokes);

            // Layer 6: Mastery nodes.
            nodeIndex <<= 1;
            var ancientScripts = new SkillNode(nodeIndex, "Ancient Scripts", 11, "Unlocks additional spells", (p) =>
            {
                profile.Talents[TalentID.InscribeSpells].Points |= 0x200;
            });

            nodeIndex <<= 1;
            var celestialInscriptions = new SkillNode(nodeIndex, "Celestial Inscriptions", 11, "Boosts magical precision", (p) =>
            {
                profile.Talents[TalentID.InscribeAccuracy].Points += 1;
            });

            nodeIndex <<= 1;
            var oraclesOath = new SkillNode(nodeIndex, "Oracle's Oath", 11, "Unlocks prophetic spells", (p) =>
            {
                profile.Talents[TalentID.InscribeSpells].Points |= 0x400;
            });

            nodeIndex <<= 1;
            var phantomPhonetics = new SkillNode(nodeIndex, "Phantom Phonetics", 11, "Enhances verbal incantations", (p) =>
            {
                profile.Talents[TalentID.InscribeEfficiency].Points += 1;
            });

            masterScribe.AddChild(ancientScripts);
            eloquentExpressions.AddChild(celestialInscriptions);
            glyphGuardian.AddChild(oraclesOath);
            spellbindingStrokes.AddChild(phantomPhonetics);

            // Layer 7: Final, pinnacle bonuses.
            nodeIndex <<= 1;
            var eternalEpistles = new SkillNode(nodeIndex, "Eternal Epistles", 12, "Grants a protective inscription bonus", (p) =>
            {
                profile.Talents[TalentID.InscribeYield].Points += 1;
            });

            nodeIndex <<= 1;
            var divineDraft = new SkillNode(nodeIndex, "Divine Draft", 12, "Unlocks divine inscription spells", (p) =>
            {
                profile.Talents[TalentID.InscribeSpells].Points |= 0x800;
            });

            nodeIndex <<= 1;
            // Changed Infinite Inscriptions to unlock spell 0x4000.
            var infiniteInscriptions = new SkillNode(nodeIndex, "Infinite Inscriptions", 12, "Unlocks inscription spell 0x4000", (p) =>
            {
                profile.Talents[TalentID.InscribeSpells].Points |= 0x4000;
            });

            nodeIndex <<= 1;
            // Changed Sage's Script to unlock spell 0x8000.
            var sagesScript = new SkillNode(nodeIndex, "Sage's Script", 12, "Unlocks inscription spell 0x8000", (p) =>
            {
                profile.Talents[TalentID.InscribeSpells].Points |= 0x8000;
            });

            eternalEpistles.AddChild(divineDraft);
            divineDraft.AddChild(infiniteInscriptions);
            infiniteInscriptions.AddChild(sagesScript);

            // Layer 8: Ultimate node.
            nodeIndex <<= 1;
            var ultimateIlluminator = new SkillNode(nodeIndex, "Ultimate Illuminator", 13, "Ultimate bonus: boosts all inscription skills and unlocks spells 0x1000 & 0x2000", (p) =>
            {
                profile.Talents[TalentID.InscribeSpells].Points |= 0x1000 | 0x2000;
                profile.Talents[TalentID.InscribeAccuracy].Points += 1;
                profile.Talents[TalentID.InscribeEfficiency].Points += 1;
                profile.Talents[TalentID.InscribeYield].Points += 1;
            });

            eternalEpistles.AddChild(ultimateIlluminator);
        }
    }

    // Command to open the Inscription Skill Tree.
    public class InscriptionSkillTreeCommand
    {
        public static void Initialize()
        {
            CommandSystem.Register("InscribeTree", AccessLevel.Player, cmd => ShowTree(cmd.Mobile));
        }

        private static void ShowTree(Mobile m)
        {
            if (m is PlayerMobile pm)
            {
                pm.SendMessage("Opening Inscription Skill Tree...");
                pm.SendGump(new InscriptionSkillTree(pm));
            }
            else
            {
                m.SendMessage("You must be a player to use this command.");
            }
        }
    }
}
