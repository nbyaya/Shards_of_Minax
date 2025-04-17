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
using Server.Mobiles; // for Talent extensions

namespace Server.ACC.CSS.Systems.StealthMagic
{
    public class StealthSkillTree : SuperGump
    {
        private StealthTree tree;
        private Dictionary<SkillNode, Point2D> nodePositions;
        private Dictionary<int, SkillNode> buttonNodeMap;
        private Dictionary<SkillNode, int> edgeThickness;
        private const int buttonSize = 15;
        private int ySpacing = 50, xSpacing = 60;
        private int rootX = 300, rootY = 150;
        private SkillNode selectedNode;

        public StealthSkillTree(PlayerMobile user)
            : base(user, null, 100, 100)
        {
            tree = new StealthTree(user);
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
            layout.Add("title", () => { AddLabel(100, 20, 1153, "Stealth Skill Tree"); });

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

    // A SkillNode class identical in structure to the Lumberjacking version.
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
            if (!profile.Talents.ContainsKey(TalentID.StealthNodes))
                profile.Talents[TalentID.StealthNodes] = new Talent(TalentID.StealthNodes) { Points = 0 };

            return (profile.Talents[TalentID.StealthNodes].Points & BitFlag) != 0;
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
            profile.Talents[TalentID.StealthNodes].Points |= BitFlag;

            player.SendMessage($"{Name} activated!");
            onActivate?.Invoke(player);
            return true;
        }
    }

    // The StealthTree builds the full tree with 8 layers (30 nodes total).
    public class StealthTree
    {
        public SkillNode Root { get; }

        public StealthTree(PlayerMobile player)
        {
            var profile = player.AcquireTalents();
            int nodeIndex = 0x01;

            // Layer 0: Root Node – Unlocks basic stealth spells.
            Root = new SkillNode(nodeIndex, "Shadow's Call", 5, "Unlocks basic stealth spells", (p) =>
            {
                profile.Talents[TalentID.StealthSpells].Points |= 0x01;
            });

            // Layer 1: Basic bonuses.
            nodeIndex <<= 1;
            var nightVision = new SkillNode(nodeIndex, "Night Vision", 6, "Enhances your ability to see in the dark", (p) =>
            {
                profile.Talents[TalentID.StealthSpells].Points |= 0x02;
            });

            nodeIndex <<= 1;
            var silentFootfalls = new SkillNode(nodeIndex, "Silent Footfalls", 6, "Increases allowed stealth steps", (p) =>
            {
                // Remains a passive bonus.
                profile.Talents[TalentID.StealthStepsBonus].Points += 1;
            });

            nodeIndex <<= 1;
            var cloakOfShadows = new SkillNode(nodeIndex, "Cloak of Shadows", 6, "Reduces chance to be detected", (p) =>
            {
                // Remains a passive bonus.
                profile.Talents[TalentID.StealthDetectionBonus].Points += 1;
            });

            nodeIndex <<= 1;
            var stealthyManeuvers = new SkillNode(nodeIndex, "Stealthy Maneuvers", 6, "Unlocks a bonus stealth ability", (p) =>
            {
                profile.Talents[TalentID.StealthSpells].Points |= 0x04;
            });

            Root.AddChild(nightVision);
            Root.AddChild(silentFootfalls);
            Root.AddChild(cloakOfShadows);
            Root.AddChild(stealthyManeuvers);

            // Layer 2: Advanced stealth spells and passive bonuses.
            nodeIndex <<= 1;
            var shadowMeld = new SkillNode(nodeIndex, "Shadow Meld", 7, "Unlocks additional stealth spells", (p) =>
            {
                profile.Talents[TalentID.StealthSpells].Points |= 0x08;
            });

            nodeIndex <<= 1;
            var quickEscape = new SkillNode(nodeIndex, "Quick Escape", 7, "Unlocks an escape spell", (p) =>
            {
                // Converted from a passive bonus to unlock spell 0x400.
                profile.Talents[TalentID.StealthSpells].Points |= 0x400;
            });

            nodeIndex <<= 1;
            var ghostWalk = new SkillNode(nodeIndex, "Ghost Walk", 7, "Increases movement speed while stealthed", (p) =>
            {
                // Remains a passive bonus.
                profile.Talents[TalentID.StealthSpeedBonus].Points += 1;
            });

            nodeIndex <<= 1;
            var evasiveManeuvers = new SkillNode(nodeIndex, "Evasive Maneuvers", 7, "Improves dodge chance while stealthed", (p) =>
            {
                // Remains a passive bonus.
                profile.Talents[TalentID.StealthDodgeBonus].Points += 1;
            });

            nightVision.AddChild(shadowMeld);
            silentFootfalls.AddChild(quickEscape);
            cloakOfShadows.AddChild(ghostWalk);
            stealthyManeuvers.AddChild(evasiveManeuvers);

            // Layer 3: Further passive bonuses.
            nodeIndex <<= 1;
            var phantomPresence = new SkillNode(nodeIndex, "Phantom Presence", 8, "Unlocks advanced stealth spells", (p) =>
            {
                profile.Talents[TalentID.StealthSpells].Points |= 0x10;
            });

            nodeIndex <<= 1;
            var darkenedAura = new SkillNode(nodeIndex, "Darkened Aura", 8, "Unlocks a defensive stealth spell", (p) =>
            {
                // Converted from a detection bonus to unlock spell 0x800.
                profile.Talents[TalentID.StealthSpells].Points |= 0x800;
            });

            nodeIndex <<= 1;
            var whisperingWind = new SkillNode(nodeIndex, "Whispering Wind", 8, "Further increases allowed stealth steps", (p) =>
            {
                // Remains a passive bonus.
                profile.Talents[TalentID.StealthStepsBonus].Points += 1;
            });

            nodeIndex <<= 1;
            var spectralShift = new SkillNode(nodeIndex, "Spectral Shift", 8, "Provides a defensive bonus when stealthed", (p) =>
            {
                // Remains a passive bonus.
                profile.Talents[TalentID.StealthDefenseBonus].Points += 1;
            });

            shadowMeld.AddChild(phantomPresence);
            quickEscape.AddChild(darkenedAura);
            ghostWalk.AddChild(whisperingWind);
            evasiveManeuvers.AddChild(spectralShift);

            // Layer 4: More advanced magical enhancements.
            nodeIndex <<= 1;
            var moonlitVeil = new SkillNode(nodeIndex, "Moonlit Veil", 9, "Unlocks an additional stealth spell", (p) =>
            {
                // Converted from a detection bonus to unlock spell 0x1000.
                profile.Talents[TalentID.StealthSpells].Points |= 0x1000;
            });

            nodeIndex <<= 1;
            var silentKiller = new SkillNode(nodeIndex, "Silent Killer", 9, "Unlocks a high-damage stealth strike", (p) =>
            {
                profile.Talents[TalentID.StealthSpells].Points |= 0x20;
            });

            nodeIndex <<= 1;
            var veilOfShadows = new SkillNode(nodeIndex, "Veil of Shadows", 9, "Increases invisibility duration", (p) =>
            {
                // Remains a passive bonus.
                profile.Talents[TalentID.StealthRecoveryBonus].Points += 1;
            });

            nodeIndex <<= 1;
            var spectralAgility = new SkillNode(nodeIndex, "Spectral Agility", 9, "Boosts movement speed while stealthed", (p) =>
            {
                // Remains a passive bonus.
                profile.Talents[TalentID.StealthSpeedBonus].Points += 1;
            });

            phantomPresence.AddChild(moonlitVeil);
            darkenedAura.AddChild(silentKiller);
            whisperingWind.AddChild(veilOfShadows);
            spectralShift.AddChild(spectralAgility);

            // Layer 5: Expert-level nodes.
            nodeIndex <<= 1;
            var umbralEfficiency = new SkillNode(nodeIndex, "Umbral Efficiency", 10, "Unlocks an efficiency spell", (p) =>
            {
                // Converted from a general bonus to unlock spell 0x2000.
                profile.Talents[TalentID.StealthSpells].Points |= 0x2000;
            });

            nodeIndex <<= 1;
            var nightProwler = new SkillNode(nodeIndex, "Night Prowler", 10, "Increases chance to avoid traps", (p) =>
            {
                // Remains a passive bonus.
                profile.Talents[TalentID.StealthDodgeBonus].Points += 1;
            });

            nodeIndex <<= 1;
            var eclipseEdge = new SkillNode(nodeIndex, "Eclipse Edge", 10, "Unlocks a bonus stealth spell", (p) =>
            {
                profile.Talents[TalentID.StealthSpells].Points |= 0x40;
            });

            nodeIndex <<= 1;
            var stalkersPersistence = new SkillNode(nodeIndex, "Stalker's Persistence", 10, "Provides extra defensive bonuses", (p) =>
            {
                // Remains a passive bonus.
                profile.Talents[TalentID.StealthDefenseBonus].Points += 1;
            });

            moonlitVeil.AddChild(umbralEfficiency);
            silentKiller.AddChild(nightProwler);
            veilOfShadows.AddChild(eclipseEdge);
            spectralAgility.AddChild(stalkersPersistence);

            // Layer 6: Mastery nodes.
            nodeIndex <<= 1;
            var ghostlyPerception = new SkillNode(nodeIndex, "Ghostly Perception", 11, "Unlocks a perception spell", (p) =>
            {
                // Converted from a detection bonus to unlock spell 0x4000.
                profile.Talents[TalentID.StealthSpells].Points |= 0x4000;
            });

            nodeIndex <<= 1;
            var shadowsReflex = new SkillNode(nodeIndex, "Shadow's Reflex", 11, "Increases reaction speed", (p) =>
            {
                // Remains a passive bonus.
                profile.Talents[TalentID.StealthSpeedBonus].Points += 1;
            });

            nodeIndex <<= 1;
            var umbralMastery = new SkillNode(nodeIndex, "Umbral Mastery", 11, "Unlocks advanced stealth spells", (p) =>
            {
                profile.Talents[TalentID.StealthSpells].Points |= 0x80;
            });

            nodeIndex <<= 1;
            var stealthTransformation = new SkillNode(nodeIndex, "Stealth Transformation", 11, "Improves overall stealth capabilities", (p) =>
            {
                // Remains a passive bonus.
                profile.Talents[TalentID.StealthNodes].Points += 1;
            });

            umbralEfficiency.AddChild(ghostlyPerception);
            nightProwler.AddChild(shadowsReflex);
            eclipseEdge.AddChild(umbralMastery);
            stalkersPersistence.AddChild(stealthTransformation);

            // Layer 7: Final, pinnacle bonuses.
            nodeIndex <<= 1;
            var silentSentinel = new SkillNode(nodeIndex, "Silent Sentinel", 12, "Unlocks a defensive stealth spell", (p) =>
            {
                // Converted from a defensive bonus to unlock spell 0x8000.
                profile.Talents[TalentID.StealthSpells].Points |= 0x8000;
            });

            nodeIndex <<= 1;
            var phantomEndowment = new SkillNode(nodeIndex, "Phantom Endowment", 12, "Further increases stealth potential", (p) =>
            {
                // Remains a passive bonus.
                profile.Talents[TalentID.StealthNodes].Points += 1;
            });

            nodeIndex <<= 1;
            var creepingDread = new SkillNode(nodeIndex, "Creeping Dread", 12, "Enhances dodge chance", (p) =>
            {
                // Remains a passive bonus.
                profile.Talents[TalentID.StealthDodgeBonus].Points += 1;
            });

            nodeIndex <<= 1;
            var nightmareEcho = new SkillNode(nodeIndex, "Nightmare Echo", 12, "Unlocks a bonus stealth spell", (p) =>
            {
                profile.Talents[TalentID.StealthSpells].Points |= 0x100;
            });

            ghostlyPerception.AddChild(silentSentinel);
            shadowsReflex.AddChild(phantomEndowment);
            umbralMastery.AddChild(creepingDread);
            stealthTransformation.AddChild(nightmareEcho);

            // Layer 8: Ultimate node.
            nodeIndex <<= 1;
            var ultimateShadowmaster = new SkillNode(nodeIndex, "Ultimate Shadowmaster", 13, "Ultimate bonus: boosts all stealth abilities", (p) =>
            {
                profile.Talents[TalentID.StealthSpells].Points |= 0x200;
                profile.Talents[TalentID.StealthStepsBonus].Points += 1;
                profile.Talents[TalentID.StealthDetectionBonus].Points += 1;
                profile.Talents[TalentID.StealthSpeedBonus].Points += 1;
                profile.Talents[TalentID.StealthRecoveryBonus].Points += 1;
                profile.Talents[TalentID.StealthDodgeBonus].Points += 1;
                profile.Talents[TalentID.StealthDefenseBonus].Points += 1;
            });

            silentSentinel.AddChild(ultimateShadowmaster);
            phantomEndowment.AddChild(ultimateShadowmaster);
            creepingDread.AddChild(ultimateShadowmaster);
            nightmareEcho.AddChild(ultimateShadowmaster);
        }
    }
}
