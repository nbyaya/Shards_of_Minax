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

namespace Server.ACC.CSS.Systems.SpiritSpeakMagic
{
    // Revised Spirit Speak Skill Tree Gump using AncientKnowledge (Maxxia Points) as the cost.
    public class SpiritSpeakSkillTree : SuperGump
    {
        private SpiritSpeakTree tree;
        private Dictionary<SkillNode, Point2D> nodePositions;
        private Dictionary<int, SkillNode> buttonNodeMap;
        private Dictionary<SkillNode, int> edgeThickness;
        private const int buttonSize = 15;
        private int ySpacing = 50, xSpacing = 60;
        private int rootX = 300, rootY = 150;
        private SkillNode selectedNode;

        public SpiritSpeakSkillTree(PlayerMobile user)
            : base(user, null, 100, 100)
        {
            tree = new SpiritSpeakTree(user);
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
            layout.Add("title", () => { AddLabel(100, 20, 1153, "Spirit Speak Skill Tree"); });

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

    // Revised SkillNode class (same as in Lumberjacking) used for Spirit Speak.
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
            if (!profile.Talents.ContainsKey(TalentID.SpiritSpeakNodes))
                profile.Talents[TalentID.SpiritSpeakNodes] = new Talent(TalentID.SpiritSpeakNodes) { Points = 0 };

            return (profile.Talents[TalentID.SpiritSpeakNodes].Points & BitFlag) != 0;
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
            profile.Talents[TalentID.SpiritSpeakNodes].Points |= BitFlag;

            player.SendMessage($"{Name} activated!");
            onActivate?.Invoke(player);
            return true;
        }
    }

    // Full Spirit Speak tree structure with 30 nodes (8 layers + ultimate) and extra passive bonus nodes.
    public class SpiritSpeakTree
    {
        public SkillNode Root { get; }

        public SpiritSpeakTree(PlayerMobile player)
        {
            var profile = player.AcquireTalents();
            int nodeIndex = 0x01;

            // Layer 0: Root Node – Unlocks basic spirit speak spells.
            Root = new SkillNode(nodeIndex, "Whisper of the Spirits", 5, "Unlocks basic spirit speak spells", (p) =>
            {
                // Unlock basic spell: 0x01
                profile.Talents[TalentID.SpiritSpeakSpells].Points |= 0x01;
            });

            // Layer 1: Basic bonuses.
            nodeIndex <<= 1; // 0x02
            var etherealPerception = new SkillNode(nodeIndex, "Ethereal Perception", 6, "Unlocks a spirit spell (0x02)", (p) =>
            {
                profile.Talents[TalentID.SpiritSpeakSpells].Points |= 0x02;
            });

            nodeIndex <<= 1; // 0x04
            var spectralGuidance = new SkillNode(nodeIndex, "Spectral Guidance", 6, "Improves spell accuracy", (p) =>
            {
                // Passive bonus remains.
                profile.Talents[TalentID.MinionDamageBonus].Points += 1;
            });

            nodeIndex <<= 1; // 0x08
            var mysticChannel = new SkillNode(nodeIndex, "Mystic Channel", 6, "Unlocks bonus channeling spell (0x04)", (p) =>
            {
                profile.Talents[TalentID.SpiritSpeakSpells].Points |= 0x04;
            });

            nodeIndex <<= 1; // 0x10
            var ancientResonance = new SkillNode(nodeIndex, "Ancient Resonance", 6, "Unlocks a spirit spell (0x80)", (p) =>
            {
                profile.Talents[TalentID.SpiritSpeakSpells].Points |= 0x80;
            });

            Root.AddChild(etherealPerception);
            Root.AddChild(spectralGuidance);
            Root.AddChild(mysticChannel);
            Root.AddChild(ancientResonance);

            // Layer 2: Advanced magical bonuses.
            nodeIndex <<= 1; // 0x20
            var ghostlyEchoes = new SkillNode(nodeIndex, "Ghostly Echoes", 7, "Unlocks additional spirit spells (0x08)", (p) =>
            {
                profile.Talents[TalentID.SpiritSpeakSpells].Points |= 0x08;
            });

            nodeIndex <<= 1; // 0x40
            var wraithFlow = new SkillNode(nodeIndex, "Wraith Flow", 7, "Improves casting speed", (p) =>
            {
                profile.Talents[TalentID.MinionDamageBonus].Points += 1;
            });

            nodeIndex <<= 1; // 0x80
            var phantomArts = new SkillNode(nodeIndex, "Phantom Arts", 7, "Unlocks advanced spirit speak spells (0x10)", (p) =>
            {
                profile.Talents[TalentID.SpiritSpeakSpells].Points |= 0x10;
            });

            nodeIndex <<= 1; // 0x100
            var soulBond = new SkillNode(nodeIndex, "Soul Bond", 7, "Increases spirit link range", (p) =>
            {
                profile.Talents[TalentID.MinionDamageBonus].Points += 1;
            });

            etherealPerception.AddChild(ghostlyEchoes);
            spectralGuidance.AddChild(wraithFlow);
            mysticChannel.AddChild(phantomArts);
            ancientResonance.AddChild(soulBond);

            // Layer 3: Further enhancements.
            nodeIndex <<= 1; // 0x200
            var celestialChorus = new SkillNode(nodeIndex, "Celestial Chorus", 8, "Unlocks a spirit spell (0x2000)", (p) =>
            {
                profile.Talents[TalentID.SpiritSpeakSpells].Points |= 0x2000;
            });

            nodeIndex <<= 1; // 0x400
            var veilOfShadows = new SkillNode(nodeIndex, "Veil of Shadows", 8, "Improves spell casting speed further", (p) =>
            {
                profile.Talents[TalentID.MinionDamageBonus].Points += 1;
            });

            nodeIndex <<= 1; // 0x800
            var spiritShield = new SkillNode(nodeIndex, "Spirit Shield", 8, "Unlocks a protective barrier spell (0x20)", (p) =>
            {
                profile.Talents[TalentID.SpiritSpeakSpells].Points |= 0x20;
            });

            nodeIndex <<= 1; // 0x1000
            var auraOfCalm = new SkillNode(nodeIndex, "Aura of Calm", 8, "Reduces mana costs", (p) =>
            {
                profile.Talents[TalentID.MinionDamageBonus].Points += 1;
            });

            ghostlyEchoes.AddChild(celestialChorus);
            wraithFlow.AddChild(veilOfShadows);
            phantomArts.AddChild(spiritShield);
            soulBond.AddChild(auraOfCalm);

            // Layer 4: More advanced magical enhancements.
            nodeIndex <<= 1; // 0x2000
            var divineConduit = new SkillNode(nodeIndex, "Divine Conduit", 9, "Unlocks a spirit spell (0x4000)", (p) =>
            {
                profile.Talents[TalentID.SpiritSpeakSpells].Points |= 0x4000;
            });

            nodeIndex <<= 1; // 0x4000
            var oraclesGrace = new SkillNode(nodeIndex, "Oracle's Grace", 9, "Unlocks oracle bonus spells (0x40)", (p) =>
            {
                profile.Talents[TalentID.SpiritSpeakSpells].Points |= 0x40;
            });

            nodeIndex <<= 1; // 0x8000
            var spectralFortitude = new SkillNode(nodeIndex, "Spectral Fortitude", 9, "Improves defensive magic", (p) =>
            {
                profile.Talents[TalentID.MinionDamageBonus].Points += 1;
            });

            nodeIndex <<= 1; // 0x10000
            var harmonyOfSpheres = new SkillNode(nodeIndex, "Harmony of the Spheres", 9, "Boosts spirit speak range", (p) =>
            {
                profile.Talents[TalentID.MinionDamageBonus].Points += 1;
            });

            celestialChorus.AddChild(divineConduit);
            veilOfShadows.AddChild(oraclesGrace);
            spiritShield.AddChild(spectralFortitude);
            auraOfCalm.AddChild(harmonyOfSpheres);

            // Layer 5: Expert-level nodes.
            nodeIndex <<= 1;
            var primordialFocus = new SkillNode(nodeIndex, "Primordial Focus", 10, "Boosts overall spell efficiency", (p) =>
            {
                profile.Talents[TalentID.MinionDamageBonus].Points += 1;
            });

            nodeIndex <<= 1;
            var soulHarvest = new SkillNode(nodeIndex, "Soul Harvest", 10, "Boosts spirit spell yield", (p) =>
            {
                profile.Talents[TalentID.MinionDamageBonus].Points += 1;
            });

            nodeIndex <<= 1;
            var etherealMastery = new SkillNode(nodeIndex, "Ethereal Mastery", 10, "Unlocks mastery level spirit spells (0x100)", (p) =>
            {
                profile.Talents[TalentID.SpiritSpeakSpells].Points |= 0x100;
            });

            nodeIndex <<= 1;
            var mysticMomentum = new SkillNode(nodeIndex, "Mystic Momentum", 10, "Increases channeling momentum", (p) =>
            {
                profile.Talents[TalentID.MinionDamageBonus].Points += 1;
            });

            divineConduit.AddChild(primordialFocus);
            oraclesGrace.AddChild(soulHarvest);
            spectralFortitude.AddChild(etherealMastery);
            harmonyOfSpheres.AddChild(mysticMomentum);

            // Layer 6: Mastery nodes.
            nodeIndex <<= 1;
            var expandedInsight = new SkillNode(nodeIndex, "Expanded Insight", 11, "Enhances magical awareness", (p) =>
            {
                profile.Talents[TalentID.MinionDamageBonus].Points += 1;
            });

            nodeIndex <<= 1;
            var mysticEcho = new SkillNode(nodeIndex, "Mystic Echo", 11, "Boosts spell yield with magic", (p) =>
            {
                profile.Talents[TalentID.MinionDamageBonus].Points += 1;
            });

            nodeIndex <<= 1;
            var ancientCommunion = new SkillNode(nodeIndex, "Ancient Communion", 11, "Unlocks ancient spirit spells (0x200)", (p) =>
            {
                profile.Talents[TalentID.SpiritSpeakSpells].Points |= 0x200;
            });

            nodeIndex <<= 1;
            var arcaneTransformation = new SkillNode(nodeIndex, "Arcane Transformation", 11, "Increases channeling efficiency", (p) =>
            {
                profile.Talents[TalentID.MinionDamageBonus].Points += 1;
            });

            primordialFocus.AddChild(expandedInsight);
            soulHarvest.AddChild(mysticEcho);
            etherealMastery.AddChild(ancientCommunion);
            mysticMomentum.AddChild(arcaneTransformation);

            // Layer 7: Final, pinnacle bonuses.
            nodeIndex <<= 1;
            var spectralBarrier = new SkillNode(nodeIndex, "Spectral Barrier", 12, "Provides a protective magical barrier (0x400)", (p) =>
            {
                profile.Talents[TalentID.SpiritSpeakSpells].Points |= 0x400;
            });

            nodeIndex <<= 1;
            var etherealEndowment = new SkillNode(nodeIndex, "Ethereal Endowment", 12, "Further increases spirit spell yield", (p) =>
            {
                profile.Talents[TalentID.MinionDamageBonus].Points += 1;
            });

            nodeIndex <<= 1;
            var phantomFury = new SkillNode(nodeIndex, "Phantom Fury", 12, "Unlocks a spirit spell (0x8000)", (p) =>
            {
                profile.Talents[TalentID.SpiritSpeakSpells].Points |= 0x8000;
            });

            nodeIndex <<= 1;
            var echoesOfEternity = new SkillNode(nodeIndex, "Echoes of Eternity", 12, "Enhances spirit speak range", (p) =>
            {
                profile.Talents[TalentID.MinionDamageBonus].Points += 1;
            });

            expandedInsight.AddChild(spectralBarrier);
            mysticEcho.AddChild(etherealEndowment);
            ancientCommunion.AddChild(phantomFury);
            arcaneTransformation.AddChild(echoesOfEternity);

            // Layer 8: Ultimate node.
            nodeIndex <<= 1;
            var ultimateSpiritmaster = new SkillNode(nodeIndex, "Ultimate Spiritmaster", 13, "Ultimate bonus: boosts all spirit speak skills", (p) =>
            {
                // Unlocks two spells: 0x800 and 0x1000
                profile.Talents[TalentID.SpiritSpeakSpells].Points |= 0x800 | 0x1000;
                profile.Talents[TalentID.MinionDamageBonus].Points += 1;
                profile.Talents[TalentID.MinionDamageBonus].Points += 1;
                profile.Talents[TalentID.MinionDamageBonus].Points += 1;
            });

            // Attach the ultimate node to all Layer 7 nodes.
            foreach (var node in new[] { spectralBarrier, echoesOfEternity, phantomFury, etherealEndowment })
            {
                node.AddChild(ultimateSpiritmaster);
            }
        }
    }
}
