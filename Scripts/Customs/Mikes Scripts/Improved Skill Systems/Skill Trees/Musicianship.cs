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

namespace Server.ACC.CSS.Systems.MusicianshipMagic
{
    // Revised Musicianship Skill Tree Gump using AncientKnowledge (Maxxia Points) for costs.
    public class MusicianshipSkillTree : SuperGump
    {
        private MusicianshipTree tree;
        private Dictionary<SkillNode, Point2D> nodePositions;
        private Dictionary<int, SkillNode> buttonNodeMap;
        private Dictionary<SkillNode, int> edgeThickness;
        private const int buttonSize = 15;
        private int ySpacing = 50, xSpacing = 60;
        private int rootX = 300, rootY = 150;
        private SkillNode selectedNode;

        public MusicianshipSkillTree(PlayerMobile user)
            : base(user, null, 100, 100)
        {
            tree = new MusicianshipTree(user);
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
            layout.Add("title", () => { AddLabel(100, 20, 1153, "Musicianship Skill Tree"); });

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

    // Revised SkillNode that uses AncientKnowledge (Maxxia Points) for cost.
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
            if (!profile.Talents.ContainsKey(TalentID.MusicianshipNodes))
                profile.Talents[TalentID.MusicianshipNodes] = new Talent(TalentID.MusicianshipNodes) { Points = 0 };

            return (profile.Talents[TalentID.MusicianshipNodes].Points & BitFlag) != 0;
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
            profile.Talents[TalentID.MusicianshipNodes].Points |= BitFlag;

            player.SendMessage($"{Name} activated!");
            onActivate?.Invoke(player);
            return true;
        }
    }

    // Full Musicianship tree structure with 30 nodes (8 layers).
    public class MusicianshipTree
    {
        public SkillNode Root { get; }

        public MusicianshipTree(PlayerMobile player)
        {
            var profile = player.AcquireTalents();
            int nodeIndex = 0x01;

            // Layer 0: Root Node – Unlocks basic musicianship abilities.
            Root = new SkillNode(nodeIndex, "Call of the Stage", 5, "Unlocks basic musicianship abilities and spells", (p) =>
            {
                // Unlock basic spells.
                profile.Talents[TalentID.MusicianshipSpells].Points |= 0x01;
            });

            // Layer 1: Basic bonuses and spells.
            nodeIndex <<= 1;
            var rhythmicPerception = new SkillNode(nodeIndex, "Rhythmic Perception", 6, "Unlocks a minor melody spell", (p) =>
            {
                profile.Talents[TalentID.MusicianshipSpells].Points |= 0x02;
            });

            nodeIndex <<= 1;
            var melodicInsight = new SkillNode(nodeIndex, "Melodic Insight", 6, "Improves your musical intuition", (p) =>
            {
                profile.Talents[TalentID.MusicianshipTechnique].Points += 1;
            });

            nodeIndex <<= 1;
            var harmonicConvergence = new SkillNode(nodeIndex, "Harmonic Convergence", 6, "Unlocks bonus melodic spells", (p) =>
            {
                profile.Talents[TalentID.MusicianshipSpells].Points |= 0x04;
            });

            nodeIndex <<= 1;
            var soundMastery = new SkillNode(nodeIndex, "Sound Mastery", 6, "Enhances your performance delivery", (p) =>
            {
                profile.Talents[TalentID.MusicianshipResonance].Points += 1;
            });

            // Attach Layer 1 nodes to Root.
            Root.AddChild(rhythmicPerception);
            Root.AddChild(melodicInsight);
            Root.AddChild(harmonicConvergence);
            Root.AddChild(soundMastery);

            // Layer 2: Advanced bonuses and spells.
            nodeIndex <<= 1;
            var stageWhisper = new SkillNode(nodeIndex, "Stage Whisper", 7, "Unlocks additional performance spells", (p) =>
            {
                profile.Talents[TalentID.MusicianshipSpells].Points |= 0x08;
            });

            nodeIndex <<= 1;
            var tempoFlow = new SkillNode(nodeIndex, "Tempo Flow", 7, "Improves your timing precision", (p) =>
            {
                profile.Talents[TalentID.MusicianshipTechnique].Points += 1;
            });

            nodeIndex <<= 1;
            var chordalBrilliance = new SkillNode(nodeIndex, "Chordal Brilliance", 7, "Unlocks advanced musical spells", (p) =>
            {
                profile.Talents[TalentID.MusicianshipSpells].Points |= 0x10;
            });

            nodeIndex <<= 1;
            var acousticEchoes = new SkillNode(nodeIndex, "Acoustic Echoes", 7, "Enhances performance range", (p) =>
            {
                profile.Talents[TalentID.MusicianshipPerformance].Points += 1;
            });

            rhythmicPerception.AddChild(stageWhisper);
            melodicInsight.AddChild(tempoFlow);
            harmonicConvergence.AddChild(chordalBrilliance);
            soundMastery.AddChild(acousticEchoes);

            // Layer 3: Further improvements.
            nodeIndex <<= 1;
            var resonantVibrato = new SkillNode(nodeIndex, "Resonant Vibrato", 8, "Boosts your tonal vibrato", (p) =>
            {
                profile.Talents[TalentID.MusicianshipResonance].Points += 1;
            });

            nodeIndex <<= 1;
            var syncopatedBeats = new SkillNode(nodeIndex, "Syncopated Beats", 8, "Refines your rhythmic skills", (p) =>
            {
                profile.Talents[TalentID.MusicianshipTechnique].Points += 1;
            });

            nodeIndex <<= 1;
            var orchestralCommand = new SkillNode(nodeIndex, "Orchestral Command", 8, "Unlocks a commanding musical spell", (p) =>
            {
                profile.Talents[TalentID.MusicianshipSpells].Points |= 0x20;
            });

            nodeIndex <<= 1;
            var dynamicCrescendo = new SkillNode(nodeIndex, "Dynamic Crescendo", 8, "Improves your performance delivery", (p) =>
            {
                profile.Talents[TalentID.MusicianshipPerformance].Points += 1;
            });

            stageWhisper.AddChild(resonantVibrato);
            tempoFlow.AddChild(syncopatedBeats);
            chordalBrilliance.AddChild(orchestralCommand);
            acousticEchoes.AddChild(dynamicCrescendo);

            // Layer 4: More advanced enhancements.
            nodeIndex <<= 1;
            var sonorousAria = new SkillNode(nodeIndex, "Sonorous Aria", 9, "Unlocks a vocal spell", (p) =>
            {
                profile.Talents[TalentID.MusicianshipSpells].Points |= 0x2000;
            });

            nodeIndex <<= 1;
            var improvisationalGenius = new SkillNode(nodeIndex, "Improvisational Genius", 9, "Unlocks improvisational bonus spells", (p) =>
            {
                profile.Talents[TalentID.MusicianshipSpells].Points |= 0x40;
            });

            nodeIndex <<= 1;
            var virtuososEdge = new SkillNode(nodeIndex, "Virtuoso's Edge", 9, "Unlocks elite musical spells", (p) =>
            {
                profile.Talents[TalentID.MusicianshipSpells].Points |= 0x80;
            });

            nodeIndex <<= 1;
            var harmonicSurge = new SkillNode(nodeIndex, "Harmonic Surge", 9, "Boosts your performance range", (p) =>
            {
                profile.Talents[TalentID.MusicianshipPerformance].Points += 1;
            });

            resonantVibrato.AddChild(sonorousAria);
            syncopatedBeats.AddChild(improvisationalGenius);
            orchestralCommand.AddChild(virtuososEdge);
            dynamicCrescendo.AddChild(harmonicSurge);

            // Layer 5: Expert-level nodes.
            nodeIndex <<= 1;
            var primevalRhythm = new SkillNode(nodeIndex, "Primeval Rhythm", 10, "Enhances overall musical efficiency", (p) =>
            {
                profile.Talents[TalentID.MusicianshipTechnique].Points += 1;
            });

            nodeIndex <<= 1;
            var bountifulEncore = new SkillNode(nodeIndex, "Bountiful Encore", 10, "Unlocks a performance spell", (p) =>
            {
                profile.Talents[TalentID.MusicianshipSpells].Points |= 0x4000;
            });

            nodeIndex <<= 1;
            var maestrosMastery = new SkillNode(nodeIndex, "Maestro's Mastery", 10, "Unlocks mastery level musical spells", (p) =>
            {
                profile.Talents[TalentID.MusicianshipSpells].Points |= 0x100;
            });

            nodeIndex <<= 1;
            var acceleratedTempo = new SkillNode(nodeIndex, "Accelerated Tempo", 10, "Increases your playing speed", (p) =>
            {
                profile.Talents[TalentID.MusicianshipTechnique].Points += 1;
            });

            sonorousAria.AddChild(primevalRhythm);
            improvisationalGenius.AddChild(bountifulEncore);
            virtuososEdge.AddChild(maestrosMastery);
            harmonicSurge.AddChild(acceleratedTempo);

            // Layer 6: Mastery nodes.
            nodeIndex <<= 1;
            var expandedHarmony = new SkillNode(nodeIndex, "Expanded Harmony", 11, "Enhances musical range", (p) =>
            {
                profile.Talents[TalentID.MusicianshipPerformance].Points += 1;
            });

            nodeIndex <<= 1;
            var mysticMelody = new SkillNode(nodeIndex, "Mystic Melody", 11, "Unlocks an ethereal musical spell", (p) =>
            {
                profile.Talents[TalentID.MusicianshipSpells].Points |= 0x8000;
            });

            nodeIndex <<= 1;
            var ancientVirtuoso = new SkillNode(nodeIndex, "Ancient Virtuoso", 11, "Unlocks ancient musical spells", (p) =>
            {
                profile.Talents[TalentID.MusicianshipSpells].Points |= 0x200;
            });

            nodeIndex <<= 1;
            var rhythmicTransformation = new SkillNode(nodeIndex, "Rhythmic Transformation", 11, "Increases musical efficiency with magic", (p) =>
            {
                profile.Talents[TalentID.MusicianshipTechnique].Points += 1;
            });

            primevalRhythm.AddChild(expandedHarmony);
            bountifulEncore.AddChild(mysticMelody);
            maestrosMastery.AddChild(ancientVirtuoso);
            acceleratedTempo.AddChild(rhythmicTransformation);

            // Layer 7: Final, pinnacle bonuses.
            nodeIndex <<= 1;
            var baroqueBarrier = new SkillNode(nodeIndex, "Baroque Barrier", 12, "Provides a protective sonic barrier", (p) =>
            {
                profile.Talents[TalentID.MusicianshipSpells].Points |= 0x400;
            });

            nodeIndex <<= 1;
            var resonantEndowment = new SkillNode(nodeIndex, "Resonant Endowment", 12, "Further increases bonus performance effects", (p) =>
            {
                profile.Talents[TalentID.MusicianshipResonance].Points += 1;
            });

            nodeIndex <<= 1;
            var forteFury = new SkillNode(nodeIndex, "Forte Fury", 12, "Boosts your playing power", (p) =>
            {
                profile.Talents[TalentID.MusicianshipTechnique].Points += 1;
            });

            nodeIndex <<= 1;
            var echoesOfTheStage = new SkillNode(nodeIndex, "Echoes of the Stage", 12, "Enhances performance range with power", (p) =>
            {
                profile.Talents[TalentID.MusicianshipPerformance].Points += 1;
            });

            expandedHarmony.AddChild(baroqueBarrier);
            mysticMelody.AddChild(resonantEndowment);
            ancientVirtuoso.AddChild(forteFury);
            rhythmicTransformation.AddChild(echoesOfTheStage);

            // Layer 8: Ultimate node.
            nodeIndex <<= 1;
            var ultimateVirtuoso = new SkillNode(nodeIndex, "Ultimate Virtuoso", 13, "Ultimate bonus: boosts all musicianship abilities", (p) =>
            {
                profile.Talents[TalentID.MusicianshipSpells].Points |= (0x800 | 0x1000);
                profile.Talents[TalentID.MusicianshipPerformance].Points += 1;
                profile.Talents[TalentID.MusicianshipTechnique].Points += 1;
                profile.Talents[TalentID.MusicianshipResonance].Points += 1;
            });

            // Attach the ultimate node to all Layer 7 nodes.
            foreach (var node in new[] { baroqueBarrier, echoesOfTheStage, forteFury, resonantEndowment })
            {
                node.AddChild(ultimateVirtuoso);
            }
        }
    }

    // Command to open the Musicianship Skill Tree.
    public class MusicianshipSkillTreeCommand
    {
        public static void Initialize()
        {
            CommandSystem.Register("MusicTree", AccessLevel.Player, cmd => ShowTree(cmd.Mobile));
        }

        private static void ShowTree(Mobile m)
        {
            if (m is PlayerMobile pm)
            {
                pm.SendMessage("Opening Musicianship Skill Tree...");
                pm.SendGump(new MusicianshipSkillTree(pm));
            }
            else
            {
                m.SendMessage("You must be a player to use this command.");
            }
        }
    }
}
