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

namespace Server.ACC.CSS.Systems.HidingMagic
{
    public class HidingSkillTree : SuperGump
    {
        private HidingTree tree;
        private Dictionary<SkillNode, Point2D> nodePositions;
        private Dictionary<int, SkillNode> buttonNodeMap;
        private Dictionary<SkillNode, int> edgeThickness;
        private const int buttonSize = 15;
        private int ySpacing = 50, xSpacing = 60;
        private int rootX = 300, rootY = 150;
        private SkillNode selectedNode;

        public HidingSkillTree(PlayerMobile user)
            : base(user, null, 100, 100)
        {
            tree = new HidingTree(user);
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
            layout.Add("title", () => { AddLabel(100, 20, 1153, "Hiding Skill Tree"); });

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
            if (!profile.Talents.ContainsKey(TalentID.HidingNodes))
                profile.Talents[TalentID.HidingNodes] = new Talent(TalentID.HidingNodes) { Points = 0 };

            return (profile.Talents[TalentID.HidingNodes].Points & BitFlag) != 0;
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
            profile.Talents[TalentID.HidingNodes].Points |= BitFlag;

            player.SendMessage($"{Name} activated!");
            onActivate?.Invoke(player);
            return true;
        }
    }

    public class HidingTree
    {
        public SkillNode Root { get; }

        public HidingTree(PlayerMobile player)
        {
            var profile = player.AcquireTalents();
            int nodeIndex = 0x01;

            // Layer 0: Root Node – Unlocks basic hiding spells.
            Root = new SkillNode(nodeIndex, "Call of the Shadows", 5, "Unlocks basic hiding spells", (p) =>
            {
                // Slot 0x01
                if (!p.AcquireTalents().Talents.ContainsKey(TalentID.HidingSpells))
                    p.AcquireTalents().Talents[TalentID.HidingSpells] = new Talent(TalentID.HidingSpells) { Points = 0 };
                p.AcquireTalents().Talents[TalentID.HidingSpells].Points |= 0x01;
            });

            // Layer 1: Basic bonuses.
            nodeIndex <<= 1;
            // Converted to spell unlock: Slot 0x02
            var shadowCamouflage = new SkillNode(nodeIndex, "Shadow's Camouflage", 6, "Unlocks a hidden spell", (p) =>
            {
                p.AcquireTalents().Talents[TalentID.HidingSpells].Points |= 0x02;
            });

            nodeIndex <<= 1;
            // Converted to spell unlock: Slot 0x800
            var silentMovement = new SkillNode(nodeIndex, "Silent Movement", 6, "Unlocks a hidden spell", (p) =>
            {
                p.AcquireTalents().Talents[TalentID.HidingSpells].Points |= 0x800;
            });

            nodeIndex <<= 1;
            // Remains as spell unlock (adjusted): Slot 0x04
            var darknessVeil = new SkillNode(nodeIndex, "Darkness Veil", 6, "Unlocks bonus hiding spells", (p) =>
            {
                p.AcquireTalents().Talents[TalentID.HidingSpells].Points |= 0x04;
            });

            nodeIndex <<= 1;
            // Remains passive bonus.
            var nightsBlessing = new SkillNode(nodeIndex, "Night's Blessing", 6, "Improves your chance to remain unseen", (p) =>
            {
                p.AcquireTalents().Talents[TalentID.HidingEfficiency].Points += 1;
            });

            Root.AddChild(shadowCamouflage);
            Root.AddChild(silentMovement);
            Root.AddChild(darknessVeil);
            Root.AddChild(nightsBlessing);

            // Layer 2: Advanced spells and bonuses.
            nodeIndex <<= 1;
            // Remains as spell unlock: Slot 0x08
            var eclipseVeil = new SkillNode(nodeIndex, "Eclipse Veil", 7, "Unlocks additional hiding spells", (p) =>
            {
                p.AcquireTalents().Talents[TalentID.HidingSpells].Points |= 0x08;
            });

            nodeIndex <<= 1;
            var phantomSteps = new SkillNode(nodeIndex, "Phantom Steps", 7, "Enhances silent movement further", (p) =>
            {
                p.AcquireTalents().Talents[TalentID.HidingEfficiency].Points += 1;
            });

            nodeIndex <<= 1;
            // Remains as spell unlock: Slot 0x10
            var obsidianShroud = new SkillNode(nodeIndex, "Obsidian Shroud", 7, "Unlocks advanced hiding spells", (p) =>
            {
                p.AcquireTalents().Talents[TalentID.HidingSpells].Points |= 0x10;
            });

            nodeIndex <<= 1;
            var moonlightGrace = new SkillNode(nodeIndex, "Moonlight Grace", 7, "Improves stealth capabilities", (p) =>
            {
                p.AcquireTalents().Talents[TalentID.HidingEfficiency].Points += 1;
            });

            shadowCamouflage.AddChild(eclipseVeil);
            silentMovement.AddChild(phantomSteps);
            darknessVeil.AddChild(obsidianShroud);
            nightsBlessing.AddChild(moonlightGrace);

            // Layer 3: Further passive enhancements.
            nodeIndex <<= 1;
            var veiledPresence = new SkillNode(nodeIndex, "Veiled Presence", 8, "Boosts overall stealth ability", (p) =>
            {
                p.AcquireTalents().Talents[TalentID.HidingEfficiency].Points += 1;
            });

            nodeIndex <<= 1;
            // Converted to spell unlock: Slot 0x1000
            var ghostlyQuiet = new SkillNode(nodeIndex, "Ghostly Quiet", 8, "Unlocks a hidden spell", (p) =>
            {
                p.AcquireTalents().Talents[TalentID.HidingSpells].Points |= 0x1000;
            });

            nodeIndex <<= 1;
            // Remains as spell unlock: Slot 0x20
            var umbralWard = new SkillNode(nodeIndex, "Umbral Ward", 8, "Unlocks mastery level hiding spells", (p) =>
            {
                p.AcquireTalents().Talents[TalentID.HidingSpells].Points |= 0x20;
            });

            nodeIndex <<= 1;
            var nocturneReflexes = new SkillNode(nodeIndex, "Nocturne Reflexes", 8, "Enhances reaction speed when hidden", (p) =>
            {
                p.AcquireTalents().Talents[TalentID.HidingEfficiency].Points += 1;
            });

            eclipseVeil.AddChild(veiledPresence);
            phantomSteps.AddChild(ghostlyQuiet);
            obsidianShroud.AddChild(umbralWard);
            moonlightGrace.AddChild(nocturneReflexes);

            // Layer 4: More advanced enhancements.
            nodeIndex <<= 1;
            var shadowsFortitude = new SkillNode(nodeIndex, "Shadow's Fortitude", 9, "Increases stealth resistance", (p) =>
            {
                p.AcquireTalents().Talents[TalentID.HidingEfficiency].Points += 1;
            });

            nodeIndex <<= 1;
            // Remains as spell unlock: Slot 0x40
            var silenceOfTheNight = new SkillNode(nodeIndex, "Silence of the Night", 9, "Unlocks additional stealth spells", (p) =>
            {
                p.AcquireTalents().Talents[TalentID.HidingSpells].Points |= 0x40;
            });

            nodeIndex <<= 1;
            // Converted to spell unlock: Slot 0x2000
            var darkenedAura = new SkillNode(nodeIndex, "Darkened Aura", 9, "Unlocks a hidden spell", (p) =>
            {
                p.AcquireTalents().Talents[TalentID.HidingSpells].Points |= 0x2000;
            });

            nodeIndex <<= 1;
            var lunarGrace = new SkillNode(nodeIndex, "Lunar Grace", 9, "Extends duration of hiding", (p) =>
            {
                p.AcquireTalents().Talents[TalentID.HidingEfficiency].Points += 1;
            });

            veiledPresence.AddChild(shadowsFortitude);
            ghostlyQuiet.AddChild(silenceOfTheNight);
            umbralWard.AddChild(darkenedAura);
            nocturneReflexes.AddChild(lunarGrace);

            // Layer 5: Expert-level nodes.
            nodeIndex <<= 1;
            var stealthEfficiency = new SkillNode(nodeIndex, "Stealth Efficiency", 10, "Boosts overall stealth performance", (p) =>
            {
                p.AcquireTalents().Talents[TalentID.HidingEfficiency].Points += 1;
            });

            nodeIndex <<= 1;
            // Remains as spell unlock: Slot 0x80
            var cloakOfShadows = new SkillNode(nodeIndex, "Cloak of Shadows", 10, "Unlocks high-level hiding spells", (p) =>
            {
                p.AcquireTalents().Talents[TalentID.HidingSpells].Points |= 0x80;
            });

            nodeIndex <<= 1;
            var evasiveManeuvers = new SkillNode(nodeIndex, "Evasive Maneuvers", 10, "Improves dodge chance while hidden", (p) =>
            {
                p.AcquireTalents().Talents[TalentID.HidingEfficiency].Points += 1;
            });

            nodeIndex <<= 1;
            // Converted to spell unlock: Slot 0x4000
            var quickFade = new SkillNode(nodeIndex, "Quick Fade", 10, "Unlocks a hidden spell", (p) =>
            {
                p.AcquireTalents().Talents[TalentID.HidingSpells].Points |= 0x4000;
            });

            shadowsFortitude.AddChild(stealthEfficiency);
            ghostlyQuiet.AddChild(cloakOfShadows);
            darkenedAura.AddChild(evasiveManeuvers);
            lunarGrace.AddChild(quickFade);

            // Layer 6: Mastery nodes.
            nodeIndex <<= 1;
            var expandedConcealment = new SkillNode(nodeIndex, "Expanded Concealment", 11, "Enhances your hidden radius", (p) =>
            {
                p.AcquireTalents().Talents[TalentID.HidingEfficiency].Points += 1;
            });

            nodeIndex <<= 1;
            var mysticShadow = new SkillNode(nodeIndex, "Mystic Shadow", 11, "Enhances magical invisibility", (p) =>
            {
                p.AcquireTalents().Talents[TalentID.HidingEfficiency].Points += 1;
            });

            nodeIndex <<= 1;
            // Remains as spell unlock: Slot 0x100
            var ancientShadows = new SkillNode(nodeIndex, "Ancient Shadows", 11, "Unlocks ancient hiding spells", (p) =>
            {
                p.AcquireTalents().Talents[TalentID.HidingSpells].Points |= 0x100;
            });

            nodeIndex <<= 1;
            // Converted to spell unlock: Slot 0x8000
            var fadeMastery = new SkillNode(nodeIndex, "Fade Mastery", 11, "Unlocks a hidden spell", (p) =>
            {
                p.AcquireTalents().Talents[TalentID.HidingSpells].Points |= 0x8000;
            });

            stealthEfficiency.AddChild(expandedConcealment);
            cloakOfShadows.AddChild(mysticShadow);
            evasiveManeuvers.AddChild(ancientShadows);
            quickFade.AddChild(fadeMastery);

            // Layer 7: Pinnacle bonuses.
            nodeIndex <<= 1;
            // Remains as spell unlock: Slot 0x200
            var barrierOfDarkness = new SkillNode(nodeIndex, "Barrier of Darkness", 12, "Unlocks protective hiding spells", (p) =>
            {
                p.AcquireTalents().Talents[TalentID.HidingSpells].Points |= 0x200;
            });

            nodeIndex <<= 1;
            var hiddenEndowment = new SkillNode(nodeIndex, "Hidden Endowment", 12, "Increases overall hiding success", (p) =>
            {
                p.AcquireTalents().Talents[TalentID.HidingEfficiency].Points += 1;
            });

            nodeIndex <<= 1;
            var shadowsFury = new SkillNode(nodeIndex, "Shadow's Fury", 12, "Enhances offensive capabilities while hidden", (p) =>
            {
                p.AcquireTalents().Talents[TalentID.HidingEfficiency].Points += 1;
            });

            nodeIndex <<= 1;
            var echoesOfSilence = new SkillNode(nodeIndex, "Echoes of Silence", 12, "Extends duration of hidden state", (p) =>
            {
                p.AcquireTalents().Talents[TalentID.HidingEfficiency].Points += 1;
            });

            expandedConcealment.AddChild(barrierOfDarkness);
            mysticShadow.AddChild(hiddenEndowment);
            ancientShadows.AddChild(shadowsFury);
            fadeMastery.AddChild(echoesOfSilence);

            // Layer 8: Ultimate node.
            nodeIndex <<= 1;
            // Converted to spell unlock: Slot 0x400
            var ultimateShadowmaster = new SkillNode(nodeIndex, "Ultimate Shadowmaster", 13, "Ultimate bonus: boosts all hiding abilities", (p) =>
            {
                p.AcquireTalents().Talents[TalentID.HidingSpells].Points |= 0x400;
                p.AcquireTalents().Talents[TalentID.HidingEfficiency].Points += 1;
            });

            foreach (var node in new[] { barrierOfDarkness, echoesOfSilence, shadowsFury, hiddenEndowment })
            {
                node.AddChild(ultimateShadowmaster);
            }
        }
    }

    // Command to open the Hiding Skill Tree.
    public class HidingSkillTreeCommand
    {
        public static void Initialize()
        {
            CommandSystem.Register("HidingTree", AccessLevel.Player, cmd => ShowTree(cmd.Mobile));
        }

        private static void ShowTree(Mobile m)
        {
            if (m is PlayerMobile pm)
            {
                pm.SendMessage("Opening Hiding Skill Tree...");
                pm.SendGump(new HidingSkillTree(pm));
            }
            else
            {
                m.SendMessage("You must be a player to use this command.");
            }
        }
    }
}
