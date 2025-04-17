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

namespace Server.ACC.CSS.Systems.ProvocationMagic
{
    // Provocation Skill Tree Gump using Maxxia Points for unlocking nodes.
    public class ProvocationSkillTree : SuperGump
    {
        private ProvocationTree tree;
        private Dictionary<SkillNode, Point2D> nodePositions;
        private Dictionary<int, SkillNode> buttonNodeMap;
        private Dictionary<SkillNode, int> edgeThickness;
        private const int buttonSize = 15;
        private int ySpacing = 50, xSpacing = 60;
        private int rootX = 300, rootY = 150;
        private SkillNode selectedNode;

        public ProvocationSkillTree(PlayerMobile user)
            : base(user, null, 100, 100)
        {
            tree = new ProvocationTree(user);
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

            // This HashSet will ensure each node is only placed once.
            var visited = new HashSet<SkillNode>();

            queue.Enqueue((root, 0));

            while (queue.Count > 0)
            {
                var (node, level) = queue.Dequeue();

                // If we've already visited this node, skip it.
                if (!visited.Add(node))
                    continue;

                if (!levelNodes.ContainsKey(level))
                    levelNodes[level] = new List<SkillNode>();

                levelNodes[level].Add(node);

                foreach (var child in node.Children)
                    queue.Enqueue((child, level + 1));
            }

            // Position each level's nodes centered around rootX
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
            layout.Add("title", () => { AddLabel(100, 20, 1153, "Provocation Skill Tree"); });

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

    // SkillNode class used for the Provocation tree.
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
            if (!profile.Talents.ContainsKey(TalentID.ProvocationNodes))
                profile.Talents[TalentID.ProvocationNodes] = new Talent(TalentID.ProvocationNodes) { Points = 0 };

            return (profile.Talents[TalentID.ProvocationNodes].Points & BitFlag) != 0;
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

            // Use AncientKnowledge (Maxxia Points) to unlock.
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
            profile.Talents[TalentID.ProvocationNodes].Points |= BitFlag;

            player.SendMessage($"{Name} activated!");
            onActivate?.Invoke(player);
            return true;
        }
    }

    // Full Provocation tree structure with 30 nodes across 9 layers.
    public class ProvocationTree
    {
        public SkillNode Root { get; }

        public ProvocationTree(PlayerMobile player)
        {
            var profile = player.AcquireTalents();
            int nodeIndex = 0x01;

            // Layer 0: Root Node – Unlocks basic provocation spells.
            Root = new SkillNode(nodeIndex, "Call to Discord", 5, "Unlocks basic provocation spells", (p) =>
            {
                // Unlock basic provocation spells.
                profile.Talents[TalentID.ProvocationSpells].Points |= 0x01;
            });

            // Layer 1: Basic enhancements.
            nodeIndex <<= 1;
            var vocalIntonation = new SkillNode(nodeIndex, "Vocal Intonation", 6, "Enhances your tone for inciting anger", (p) =>
            {
                // Increase incitement power.
                profile.Talents[TalentID.ProvocationRange].Points += 1;
            });

            nodeIndex <<= 1;
            var rhythmicCommand = new SkillNode(nodeIndex, "Rhythmic Command", 6, "Unlocks an advanced provocation spell", (p) =>
            {
                // Unlock the additional provocation spell (0x8000).
                profile.Talents[TalentID.ProvocationSpells].Points |= 0x8000;
            });

            nodeIndex <<= 1;
            var commandingPresence = new SkillNode(nodeIndex, "Commanding Presence", 6, "Reduces the difficulty of provoking", (p) =>
            {
                // Unlocks a passive bonus: reduce provoke diff.
                profile.Talents[TalentID.ProvocationRange].Points += 2;
            });

            nodeIndex <<= 1;
            var fieryOratory = new SkillNode(nodeIndex, "Fiery Oratory", 6, "Boosts the power of your provocation spells", (p) =>
            {
                // Unlock bonus spell power.
                profile.Talents[TalentID.ProvocationSpells].Points |= 0x02;
            });

            // Attach Layer 1 nodes to Root.
            Root.AddChild(vocalIntonation);
            Root.AddChild(rhythmicCommand);
            Root.AddChild(commandingPresence);
            Root.AddChild(fieryOratory);

            // Layer 2: Advanced vocal and magical bonuses.
            nodeIndex <<= 1;
            var cacophonousEcho = new SkillNode(nodeIndex, "Cacophonous Echo", 7, "Unleashes an echo that stuns foes", (p) =>
            {
                profile.Talents[TalentID.ProvocationSpells].Points |= 0x04;
            });

            nodeIndex <<= 1;
            var resonantFury = new SkillNode(nodeIndex, "Resonant Fury", 7, "Amplifies the anger of your targets", (p) =>
            {
                profile.Talents[TalentID.ProvocationCooldownReduction].Points += 1;
            });

            nodeIndex <<= 1;
            var oratorsMight = new SkillNode(nodeIndex, "Orator's Might", 7, "Enhances the volume of your spells", (p) =>
            {
                profile.Talents[TalentID.ProvocationSpells].Points |= 0x08;
            });

            nodeIndex <<= 1;
            var shatteringVoice = new SkillNode(nodeIndex, "Shattering Voice", 7, "Breaks the enemy’s morale", (p) =>
            {
                profile.Talents[TalentID.ProvocationBonus].Points += 1;
            });

            // Attach Layer 2 nodes.
            vocalIntonation.AddChild(cacophonousEcho);
            rhythmicCommand.AddChild(resonantFury);
            commandingPresence.AddChild(oratorsMight);
            fieryOratory.AddChild(shatteringVoice);

            // Layer 3: Further incitement improvements.
            nodeIndex <<= 1;
            var thunderousClamor = new SkillNode(nodeIndex, "Thunderous Clamor", 8, "Stuns opponents with a roar", (p) =>
            {
                profile.Talents[TalentID.ProvocationSpells].Points |= 0x10;
            });

            nodeIndex <<= 1;
            var incendiaryTone = new SkillNode(nodeIndex, "Incendiary Tone", 8, "Adds a burning effect to provoked targets", (p) =>
            {
                profile.Talents[TalentID.ProvocationRange].Points += 1;
            });

            nodeIndex <<= 1;
            var stridentBeat = new SkillNode(nodeIndex, "Strident Beat", 8, "Improves the rhythm to agitate more effectively", (p) =>
            {
                profile.Talents[TalentID.ProvocationBonus].Points += 1;
            });

            nodeIndex <<= 1;
            var vocalDynamo = new SkillNode(nodeIndex, "Vocal Dynamo", 8, "Enhances overall provocation power", (p) =>
            {
                profile.Talents[TalentID.ProvocationSpells].Points |= 0x20;
            });

            // Attach Layer 3 nodes.
            cacophonousEcho.AddChild(thunderousClamor);
            resonantFury.AddChild(incendiaryTone);
            oratorsMight.AddChild(stridentBeat);
            shatteringVoice.AddChild(vocalDynamo);

            // Layer 4: Magical enhancements.
            nodeIndex <<= 1;
            var melodicInsurrection = new SkillNode(nodeIndex, "Melodic Insurrection", 9, "Rouses a riot with your music", (p) =>
            {
                profile.Talents[TalentID.ProvocationCooldownReduction].Points += 1;
            });

            nodeIndex <<= 1;
            var discordantHymn = new SkillNode(nodeIndex, "Discordant Hymn", 9, "Unleashes dissonance among foes", (p) =>
            {
                profile.Talents[TalentID.ProvocationSpells].Points |= 0x40;
            });

            nodeIndex <<= 1;
            var riotousRefrain = new SkillNode(nodeIndex, "Riotous Refrain", 9, "Bolsters the anger in the field", (p) =>
            {
                profile.Talents[TalentID.ProvocationBonus].Points += 1;
            });

            nodeIndex <<= 1;
            var sonicBarrage = new SkillNode(nodeIndex, "Sonic Barrage", 9, "Unleashes a barrage of sonic energy", (p) =>
            {
                profile.Talents[TalentID.ProvocationSpells].Points |= 0x80;
            });

            // Attach Layer 4 nodes.
            thunderousClamor.AddChild(melodicInsurrection);
            incendiaryTone.AddChild(discordantHymn);
            stridentBeat.AddChild(riotousRefrain);
            vocalDynamo.AddChild(sonicBarrage);

            // Layer 5: Expert-level nodes.
            nodeIndex <<= 1;
            var masterOfProvocation = new SkillNode(nodeIndex, "Master of Provocation", 10, "Significantly boosts incitement power", (p) =>
            {
                profile.Talents[TalentID.ProvocationRange].Points += 1;
            });

            nodeIndex <<= 1;
            var incitementMastery = new SkillNode(nodeIndex, "Incitement Mastery", 10, "Further improves your provocation spells", (p) =>
            {
                profile.Talents[TalentID.ProvocationSpells].Points |= 0x100;
            });

            nodeIndex <<= 1;
            var vocalOnslaught = new SkillNode(nodeIndex, "Vocal Onslaught", 10, "Unleashes relentless provocation", (p) =>
            {
                profile.Talents[TalentID.ProvocationBonus].Points += 1;
            });

            nodeIndex <<= 1;
            var orchestralMayhem = new SkillNode(nodeIndex, "Orchestral Mayhem", 10, "Drastically increases your inciting power", (p) =>
            {
                profile.Talents[TalentID.ProvocationSpells].Points |= 0x200;
            });

            // Attach Layer 5 nodes.
            melodicInsurrection.AddChild(masterOfProvocation);
            discordantHymn.AddChild(incitementMastery);
            riotousRefrain.AddChild(vocalOnslaught);
            sonicBarrage.AddChild(orchestralMayhem);

            // Layer 6: Mastery nodes.
            nodeIndex <<= 1;
            var harmoniousFury = new SkillNode(nodeIndex, "Harmonious Fury", 11, "Calms your targets into a frenzy", (p) =>
            {
                profile.Talents[TalentID.ProvocationBonus].Points += 1;
            });

            nodeIndex <<= 1;
            var chaoticCrescendo = new SkillNode(nodeIndex, "Chaotic Crescendo", 11, "Increases the chaos of your incitement", (p) =>
            {
                profile.Talents[TalentID.ProvocationSpells].Points |= 0x400;
            });

            nodeIndex <<= 1;
            var rhythmicDomination = new SkillNode(nodeIndex, "Rhythmic Domination", 11, "Grants you near-total control over provoked foes", (p) =>
            {
                profile.Talents[TalentID.ProvocationRange].Points += 1;
            });

            nodeIndex <<= 1;
            var melodicAnarchy = new SkillNode(nodeIndex, "Melodic Anarchy", 11, "Sows disarray among enemy ranks", (p) =>
            {
                profile.Talents[TalentID.ProvocationSpells].Points |= 0x800;
            });

            // Attach Layer 6 nodes.
            masterOfProvocation.AddChild(harmoniousFury);
            incitementMastery.AddChild(chaoticCrescendo);
            vocalOnslaught.AddChild(rhythmicDomination);
            orchestralMayhem.AddChild(melodicAnarchy);

            // Layer 7: Final, pinnacle bonuses.
            nodeIndex <<= 1;
            var sonicOverload = new SkillNode(nodeIndex, "Sonic Overload", 12, "Overwhelms foes with sound", (p) =>
            {
                profile.Talents[TalentID.ProvocationSpells].Points |= 0x1000;
            });

            nodeIndex <<= 1;
            var voiceOfChaos = new SkillNode(nodeIndex, "Voice of Chaos", 12, "Maximizes your provocation impact", (p) =>
            {
                profile.Talents[TalentID.ProvocationRange].Points += 1;
            });

            nodeIndex <<= 1;
            var instigativeSurge = new SkillNode(nodeIndex, "Instigative Surge", 12, "Increases your provocation speed", (p) =>
            {
                profile.Talents[TalentID.ProvocationSpells].Points |= 0x2000;
            });

            nodeIndex <<= 1;
            var echoesOfDissent = new SkillNode(nodeIndex, "Echoes of Dissent", 12, "Extends the duration of incited anger", (p) =>
            {
                profile.Talents[TalentID.ProvocationCooldownReduction].Points += 1;
            });

            // Attach Layer 7 nodes.
            harmoniousFury.AddChild(sonicOverload);
            chaoticCrescendo.AddChild(voiceOfChaos);
            rhythmicDomination.AddChild(instigativeSurge);
            melodicAnarchy.AddChild(echoesOfDissent);

            // Layer 8: Ultimate node.
            nodeIndex <<= 1;
            var ultimateProvoker = new SkillNode(nodeIndex, "Ultimate Provoker", 13, "Grants a final bonus: massively boosts all provocation abilities", (p) =>
            {
                profile.Talents[TalentID.ProvocationSpells].Points |= 0x4000;
                profile.Talents[TalentID.ProvocationBonus].Points += 1;
            });

            // Attach the ultimate node to all Layer 7 nodes.
            foreach (var node in new[] { sonicOverload, voiceOfChaos, instigativeSurge, echoesOfDissent })
            {
                node.AddChild(ultimateProvoker);
            }
        }
    }

    // Command to open the Provocation Skill Tree.
    public class ProvocationSkillTreeCommand
    {
        public static void Initialize()
        {
            CommandSystem.Register("ProvocTree", AccessLevel.Player, cmd => ShowTree(cmd.Mobile));
        }

        private static void ShowTree(Mobile m)
        {
            if (m is PlayerMobile pm)
            {
                pm.SendMessage("Opening Provocation Skill Tree...");
                pm.SendGump(new ProvocationSkillTree(pm));
            }
            else
            {
                m.SendMessage("You must be a player to use this command.");
            }
        }
    }
}
