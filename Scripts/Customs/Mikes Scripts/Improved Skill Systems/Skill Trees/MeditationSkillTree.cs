using System;
using System.Collections.Generic;
using System.Drawing;
using Server;
using Server.Commands;
using Server.Gumps;
using Server.Mobiles;
using VitaNex.Items;
using VitaNex.SuperGumps;

namespace Server.ACC.CSS.Systems.MeditationMagic
{
    // Revised Meditation Skill Tree Gump using AncientKnowledge (Maxxia Points) for unlocking nodes.
    public class MeditationSkillTree : SuperGump
    {
        private MeditationTree tree;
        private Dictionary<SkillNode, Point2D> nodePositions;
        private Dictionary<int, SkillNode> buttonNodeMap;
        private Dictionary<SkillNode, int> edgeThickness;
        private const int buttonSize = 15;
        private int ySpacing = 50, xSpacing = 60;
        private int rootX = 300, rootY = 150;
        private SkillNode selectedNode;

        public MeditationSkillTree(PlayerMobile user)
            : base(user, null, 100, 100)
        {
            tree = new MeditationTree(user);
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
            layout.Add("title", () => { AddLabel(100, 20, 1153, "Meditation Skill Tree"); });

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

    // Revised SkillNode (shared by both trees) that uses AncientKnowledge (Maxxia Points) for cost.
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
            if (!profile.Talents.ContainsKey(TalentID.MeditationNodes))
                profile.Talents[TalentID.MeditationNodes] = new Talent(TalentID.MeditationNodes) { Points = 0 };

            return (profile.Talents[TalentID.MeditationNodes].Points & BitFlag) != 0;
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
            profile.Talents[TalentID.MeditationNodes].Points |= BitFlag;
            player.SendMessage($"{Name} activated!");
            onActivate?.Invoke(player);
            return true;
        }
    }

    // Full Meditation tree structure – 30 nodes arranged in 9 layers.
    public class MeditationTree
    {
        public SkillNode Root { get; }

        public MeditationTree(PlayerMobile player)
        {
            var profile = player.AcquireTalents();

            // ---------- Layer 0: Root Node ----------
            // Spell node: Unlocks basic meditation spells (0x01)
            Root = new SkillNode(0x01, "Whispering Calm", 5, "Unlocks spell: Whispering Calm", (p) =>
            {
                profile.Talents[TalentID.MeditationSpells].Points |= 0x01;
            });

            // ---------- Layer 1: Basic nodes ----------
            // Spell node: Unlocks spell: Serene Mind (0x02)
            var sereneMind = new SkillNode(0x02, "Serene Mind", 6, "Unlocks spell: Serene Mind", (p) =>
            {
                profile.Talents[TalentID.MeditationSpells].Points |= 0x02;
            });

            // Spell node: Unlocks spell: Focused Gaze (0x04)
            var focusedGaze = new SkillNode(0x04, "Focused Gaze", 6, "Unlocks spell: Focused Gaze", (p) =>
            {
                profile.Talents[TalentID.MeditationSpells].Points |= 0x04;
            });

            // Spell node: Unlocks spell: Tranquility Burst (0x08)
            var innerBalance = new SkillNode(0x08, "Inner Balance", 6, "Unlocks spell: Tranquility Burst", (p) =>
            {
                profile.Talents[TalentID.MeditationSpells].Points |= 0x08;
            });

            // Bonus node (passive bonus – increased mana regen or other effect)
            var mentalClarity = new SkillNode(0x10000, "Mental Clarity", 6, "Passively increases mana regeneration", (p) =>
            {
                profile.Talents[TalentID.MeditationRecovery].Points += 1;
            });

            Root.AddChild(sereneMind);
            Root.AddChild(focusedGaze);
            Root.AddChild(innerBalance);
            Root.AddChild(mentalClarity);

            // ---------- Layer 2: Advanced magical and practical nodes ----------
            // Spell node: Unlocks spell: Echoes of Silence (0x10)
            var echoesOfSilence = new SkillNode(0x10, "Echoes of Silence", 7, "Unlocks spell: Echoes of Silence", (p) =>
            {
                profile.Talents[TalentID.MeditationSpells].Points |= 0x10;
            });

            // Spell node: Unlocks spell: Deep Concentration (0x20)
            var deepConcentration = new SkillNode(0x20, "Deep Concentration", 7, "Unlocks spell: Deep Concentration", (p) =>
            {
                profile.Talents[TalentID.MeditationSpells].Points |= 0x20;
            });

            // Spell node: Unlocks spell: Arcane Insight (0x40)
            var arcaneInsight = new SkillNode(0x40, "Arcane Insight", 7, "Unlocks spell: Arcane Insight", (p) =>
            {
                profile.Talents[TalentID.MeditationSpells].Points |= 0x40;
            });

            // Bonus node
            var steadyPulse = new SkillNode(0x20000, "Steady Pulse", 7, "Further increases mana regeneration", (p) =>
            {
                profile.Talents[TalentID.MeditationRecovery].Points += 1;
            });

            sereneMind.AddChild(echoesOfSilence);
            focusedGaze.AddChild(deepConcentration);
            innerBalance.AddChild(arcaneInsight);
            mentalClarity.AddChild(steadyPulse);

            // ---------- Layer 3: Further nodes ----------
            // Spell node: Unlocks spell: Mindful Reflection (0x80)
            var mindfulReflection = new SkillNode(0x80, "Mindful Reflection", 8, "Unlocks spell: Mindful Reflection", (p) =>
            {
                profile.Talents[TalentID.MeditationSpells].Points |= 0x80;
            });

            // Bonus node
            var calmResolve = new SkillNode(0x40000, "Calm Resolve", 8, "Further improves meditation success chance", (p) =>
            {
                profile.Talents[TalentID.MeditationFocus].Points += 1;
            });

            // Spell node: Unlocks spell: Mystic Resonance (0x100)
            var mysticResonance = new SkillNode(0x100, "Mystic Resonance", 8, "Unlocks spell: Mystic Resonance", (p) =>
            {
                profile.Talents[TalentID.MeditationSpells].Points |= 0x100;
            });

            // Bonus node
            var soulsHarmony = new SkillNode(0x80000, "Soul's Harmony", 8, "Passively enhances overall meditation benefits", (p) =>
            {
                profile.Talents[TalentID.MeditationRecovery].Points += 1;
            });

            echoesOfSilence.AddChild(mindfulReflection);
            deepConcentration.AddChild(calmResolve);
            arcaneInsight.AddChild(mysticResonance);
            steadyPulse.AddChild(soulsHarmony);

            // ---------- Layer 4: More advanced magical enhancements ----------
            // Spell node: Unlocks spell: Tranquil Vortex (0x200)
            var tranquilVortex = new SkillNode(0x200, "Tranquil Vortex", 9, "Unlocks spell: Tranquil Vortex", (p) =>
            {
                profile.Talents[TalentID.MeditationSpells].Points |= 0x200;
            });

            // Bonus node
            var psychicSurge = new SkillNode(0x100000, "Psychic Surge", 9, "Further improves mental focus", (p) =>
            {
                profile.Talents[TalentID.MeditationFocus].Points += 1;
            });

            // Spell node: Unlocks spell: Enlightened Aura (0x400)
            var enlightenedAura = new SkillNode(0x400, "Enlightened Aura", 9, "Unlocks spell: Enlightened Aura", (p) =>
            {
                profile.Talents[TalentID.MeditationSpells].Points |= 0x400;
            });

            // Bonus node
            var zenMastery = new SkillNode(0x200000, "Zen Mastery", 9, "Further boosts mana recovery", (p) =>
            {
                profile.Talents[TalentID.MeditationRecovery].Points += 1;
            });

            mindfulReflection.AddChild(tranquilVortex);
            calmResolve.AddChild(psychicSurge);
            mysticResonance.AddChild(enlightenedAura);
            soulsHarmony.AddChild(zenMastery);

            // ---------- Layer 5: Expert-level nodes ----------
            // Bonus nodes
            var primeMeditativeness = new SkillNode(0x400000, "Prime Meditativeness", 10, "Boosts overall meditation efficiency", (p) =>
            {
                profile.Talents[TalentID.MeditationFocus].Points += 1;
            });

            var rejuvenatingCalm = new SkillNode(0x800000, "Rejuvenating Calm", 10, "Increases mana recovery speed", (p) =>
            {
                profile.Talents[TalentID.MeditationRecovery].Points += 1;
            });

            // Spell node: Unlocks spell: Mind Over Matter (0x800)
            var mindOverMatter = new SkillNode(0x800, "Mind Over Matter", 10, "Unlocks spell: Mind Over Matter", (p) =>
            {
                profile.Talents[TalentID.MeditationSpells].Points |= 0x800;
            });

            // Bonus node
            var focusedMomentum = new SkillNode(0x1000000, "Focused Momentum", 10, "Further enhances meditation bonuses", (p) =>
            {
                profile.Talents[TalentID.MeditationFocus].Points += 1;
            });

            tranquilVortex.AddChild(primeMeditativeness);
            psychicSurge.AddChild(rejuvenatingCalm);
            enlightenedAura.AddChild(mindOverMatter);
            zenMastery.AddChild(focusedMomentum);

            // ---------- Layer 6: Mastery nodes ----------
            // Bonus nodes
            var expandedAwareness = new SkillNode(0x2000000, "Expanded Awareness", 11, "Increases overall mental acuity", (p) =>
            {
                profile.Talents[TalentID.MeditationFocus].Points += 1;
            });

            var mysticClarity = new SkillNode(0x4000000, "Mystic Clarity", 11, "Boosts passive mana regeneration", (p) =>
            {
                profile.Talents[TalentID.MeditationRecovery].Points += 1;
            });

            // Spell node: Unlocks spell: Ancient Meditation (0x1000)
            var ancientMeditation = new SkillNode(0x1000, "Ancient Meditation", 11, "Unlocks spell: Ancient Meditation", (p) =>
            {
                profile.Talents[TalentID.MeditationSpells].Points |= 0x1000;
            });

            // Bonus node
            var temporalStillness = new SkillNode(0x8000000, "Temporal Stillness", 11, "Reduces meditation cooldown", (p) =>
            {
                profile.Talents[TalentID.MeditationFocus].Points += 1;
            });

            primeMeditativeness.AddChild(expandedAwareness);
            rejuvenatingCalm.AddChild(mysticClarity);
            mindOverMatter.AddChild(ancientMeditation);
            focusedMomentum.AddChild(temporalStillness);

            // ---------- Layer 7: Final, pinnacle nodes ----------
            // Spell node: Unlocks spell: Barrier of Tranquility (0x2000)
            var barrierOfTranquility = new SkillNode(0x2000, "Barrier of Tranquility", 12, "Unlocks spell: Barrier of Tranquility", (p) =>
            {
                profile.Talents[TalentID.MeditationSpells].Points |= 0x2000;
            });

            // Spell node: Unlocks spell: Inner Sanctuary (0x4000)
            var innerSanctuary = new SkillNode(0x4000, "Inner Sanctuary", 12, "Unlocks spell: Inner Sanctuary", (p) =>
            {
                profile.Talents[TalentID.MeditationSpells].Points |= 0x4000;
            });

            // Bonus node
            var furyOfSilence = new SkillNode(0x10000000, "Fury of Silence", 12, "Boosts mental resilience", (p) =>
            {
                profile.Talents[TalentID.MeditationFocus].Points += 1;
            });

            // Spell node: Unlocks spell: Echoing Serenity (0x8000)
            var echoingSerenity = new SkillNode(0x8000, "Echoing Serenity", 12, "Unlocks spell: Echoing Serenity", (p) =>
            {
                profile.Talents[TalentID.MeditationSpells].Points |= 0x8000;
            });

            expandedAwareness.AddChild(barrierOfTranquility);
            mysticClarity.AddChild(innerSanctuary);
            ancientMeditation.AddChild(furyOfSilence);
            temporalStillness.AddChild(echoingSerenity);

            // ---------- Layer 8: Ultimate node (Bonus) ----------
            // This ultimate node now provides an overall bonus without unlocking an additional spell.
            var ultimateAscendance = new SkillNode(0x20000000, "Ultimate Ascendance", 13, "Ultimate bonus: boosts all meditation skills", (p) =>
            {
                profile.Talents[TalentID.MeditationFocus].Points += 1;
                profile.Talents[TalentID.MeditationRecovery].Points += 1;
            });

            // Add ultimateAscendance as a child of each of the four nodes from layer 7.
            foreach (var node in new[] { barrierOfTranquility, innerSanctuary, furyOfSilence, echoingSerenity })
                node.AddChild(ultimateAscendance);
        }
    }
}
