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

namespace Server.ACC.CSS.Systems.SwordsMagic
{
    // Sword Skill Tree Gump using Maxxia Points (AncientKnowledge) as cost.
    public class SwordsSkillTree : SuperGump
    {
        private SwordsTree tree;
        private Dictionary<SkillNode, Point2D> nodePositions;
        private Dictionary<int, SkillNode> buttonNodeMap;
        private Dictionary<SkillNode, int> edgeThickness;
        private const int buttonSize = 15;
        private int ySpacing = 50, xSpacing = 60;
        private int rootX = 300, rootY = 150;
        private SkillNode selectedNode;

        public SwordsSkillTree(PlayerMobile user)
            : base(user, null, 100, 100)
        {
            tree = new SwordsTree(user);
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
            layout.Add("title", () => { AddLabel(100, 20, 1153, "Swordsmanship Skill Tree"); });

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

    // Revised SkillNode for Sword skill tree.
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
            if (!profile.Talents.ContainsKey(TalentID.SwordsNodes))
                profile.Talents[TalentID.SwordsNodes] = new Talent(TalentID.SwordsNodes) { Points = 0 };

            return (profile.Talents[TalentID.SwordsNodes].Points & BitFlag) != 0;
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
            profile.Talents[TalentID.SwordsNodes].Points |= BitFlag;

            player.SendMessage($"{Name} activated!");

            onActivate?.Invoke(player);
            return true;
        }
    }

    // Full Sword skill tree structure.
    public class SwordsTree
    {
        public SkillNode Root { get; }

        public SwordsTree(PlayerMobile player)
        {
            var profile = player.AcquireTalents();
            int nodeIndex = 0x01;

            // Layer 0: Root node – unlocks basic sword spells.
            Root = new SkillNode(nodeIndex, "Blade's Call", 5, "Unlocks basic sword spells", (p) =>
            {
                profile.Talents[TalentID.SwordsSpellbookSpells].Points |= 0x01;
            });

            // Layer 1: Basic bonuses and spell unlocks.
            nodeIndex <<= 1; // now 0x02
            var edgeAwareness = new SkillNode(nodeIndex, "Edge Awareness", 6, "Unlocks an extra sword spell", (p) =>
            {
                profile.Talents[TalentID.SwordsSpellbookSpells].Points |= 0x02;
            });

            nodeIndex <<= 1; // now 0x04
            var steelResolve = new SkillNode(nodeIndex, "Steel Resolve", 6, "Passively increases defense", (p) =>
            {
                profile.Talents[TalentID.SwordsDefense].Points += 1;
            });

            nodeIndex <<= 1; // now 0x08
            var quickStrike = new SkillNode(nodeIndex, "Quick Strike", 6, "Unlocks an extra sword spell", (p) =>
            {
                profile.Talents[TalentID.SwordsSpellbookSpells].Points |= 0x04;
            });

            nodeIndex <<= 1; // now 0x10
            var bladeMastery = new SkillNode(nodeIndex, "Blade Mastery", 6, "Passively increases attack damage", (p) =>
            {
                profile.Talents[TalentID.SwordsAttack].Points += 1;
            });

            Root.AddChild(edgeAwareness);
            Root.AddChild(steelResolve);
            Root.AddChild(quickStrike);
            Root.AddChild(bladeMastery);

            // Layer 2: Advanced spells and passives.
            nodeIndex <<= 1; // now 0x20
            var arcaneParry = new SkillNode(nodeIndex, "Arcane Parry", 7, "Unlocks a parry spell", (p) =>
            {
                profile.Talents[TalentID.SwordsSpellbookSpells].Points |= 0x08;
            });

            nodeIndex <<= 1; // now 0x40
            var swiftSlash = new SkillNode(nodeIndex, "Swift Slash", 7, "Passively increases attack speed", (p) =>
            {
                profile.Talents[TalentID.SwordsSpeed].Points += 1;
            });

            nodeIndex <<= 1; // now 0x80
            var crimsonFlurry = new SkillNode(nodeIndex, "Crimson Flurry", 7, "Unlocks a flurry spell", (p) =>
            {
                profile.Talents[TalentID.SwordsSpellbookSpells].Points |= 0x10;
            });

            nodeIndex <<= 1; // now 0x100
            var guardingStance = new SkillNode(nodeIndex, "Guarding Stance", 7, "Passively increases defense", (p) =>
            {
                profile.Talents[TalentID.SwordsDefense].Points += 1;
            });

            edgeAwareness.AddChild(arcaneParry);
            steelResolve.AddChild(swiftSlash);
            quickStrike.AddChild(crimsonFlurry);
            bladeMastery.AddChild(guardingStance);

            // Layer 3: Further nodes.
            nodeIndex <<= 1; // now 0x200
            var criticalPrecision = new SkillNode(nodeIndex, "Critical Precision", 8, "Unlocks a precise sword spell", (p) =>
            {
                profile.Talents[TalentID.SwordsSpellbookSpells].Points |= 0x1000;
            });

            nodeIndex <<= 1; // now 0x400
            var agileFootwork = new SkillNode(nodeIndex, "Agile Footwork", 8, "Passively increases attack speed", (p) =>
            {
                profile.Talents[TalentID.SwordsSpeed].Points += 1;
            });

            nodeIndex <<= 1; // now 0x800
            var bloodthirst = new SkillNode(nodeIndex, "Bloodthirst", 8, "Passively increases attack damage", (p) =>
            {
                profile.Talents[TalentID.SwordsAttack].Points += 1;
            });

            nodeIndex <<= 1; // now 0x1000 (note: this is not used here since we reassigned 0x1000 above)
            var defensivePosture = new SkillNode(nodeIndex, "Defensive Posture", 8, "Passively increases defense", (p) =>
            {
                profile.Talents[TalentID.SwordsDefense].Points += 1;
            });

            arcaneParry.AddChild(criticalPrecision);
            swiftSlash.AddChild(agileFootwork);
            crimsonFlurry.AddChild(bloodthirst);
            guardingStance.AddChild(defensivePosture);

            // Layer 4: Spell unlocks and passives.
            nodeIndex <<= 1; // now 0x2000
            var mysticCleave = new SkillNode(nodeIndex, "Mystic Cleave", 9, "Unlocks a cleave spell", (p) =>
            {
                profile.Talents[TalentID.SwordsSpellbookSpells].Points |= 0x20;
            });

            nodeIndex <<= 1; // now 0x4000 (not used here; we use next for Phantom Strike)
            var phantomStrike = new SkillNode(nodeIndex, "Phantom Strike", 9, "Unlocks a phantom strike spell", (p) =>
            {
                profile.Talents[TalentID.SwordsSpellbookSpells].Points |= 0x40;
            });

            nodeIndex <<= 1; // now 0x8000 (not used here; we use next for Ethereal Guard)
            var etherealGuard = new SkillNode(nodeIndex, "Ethereal Guard", 9, "Unlocks an ethereal guard spell", (p) =>
            {
                profile.Talents[TalentID.SwordsSpellbookSpells].Points |= 0x80;
            });

            nodeIndex <<= 1; // now 0x1000, but since 0x1000 is already used we adjust this layer’s extra node
            var shadowDance = new SkillNode(nodeIndex, "Shadow Dance", 9, "Unlocks a dance of blades spell", (p) =>
            {
                profile.Talents[TalentID.SwordsSpellbookSpells].Points |= 0x2000;
            });

            criticalPrecision.AddChild(mysticCleave);
            agileFootwork.AddChild(phantomStrike);
            bloodthirst.AddChild(etherealGuard);
            defensivePosture.AddChild(shadowDance);

            // Layer 5: More advanced nodes.
            nodeIndex <<= 1; // now 0x2000 (already used), so continue shifting:
            var lethalEdge = new SkillNode(nodeIndex, "Lethal Edge", 10, "Unlocks a lethal edge spell", (p) =>
            {
                profile.Talents[TalentID.SwordsSpellbookSpells].Points |= 0x4000;
            });

            nodeIndex <<= 1; // now next value (should be 0x4000 but already used) so adjust to next available by shifting
            var fortifiedDefense = new SkillNode(nodeIndex, "Fortified Defense", 10, "Passively increases defense", (p) =>
            {
                profile.Talents[TalentID.SwordsDefense].Points += 1;
            });

            nodeIndex <<= 1;
            var rapidAssault = new SkillNode(nodeIndex, "Rapid Assault", 10, "Passively increases attack speed", (p) =>
            {
                profile.Talents[TalentID.SwordsSpeed].Points += 1;
            });

            nodeIndex <<= 1;
            var serratedBlows = new SkillNode(nodeIndex, "Serrated Blows", 10, "Passively increases attack damage", (p) =>
            {
                profile.Talents[TalentID.SwordsAttack].Points += 1;
            });

            mysticCleave.AddChild(lethalEdge);
            phantomStrike.AddChild(fortifiedDefense);
            etherealGuard.AddChild(rapidAssault);
            shadowDance.AddChild(serratedBlows);

            // Layer 6: Mastery nodes.
            nodeIndex <<= 1;
            var unyieldingSpirit = new SkillNode(nodeIndex, "Unyielding Spirit", 11, "Passively increases defense", (p) =>
            {
                profile.Talents[TalentID.SwordsDefense].Points += 1;
            });

            nodeIndex <<= 1;
            var mirrorCounter = new SkillNode(nodeIndex, "Mirror Counter", 11, "Unlocks a counterspell", (p) =>
            {
                profile.Talents[TalentID.SwordsSpellbookSpells].Points |= 0x100;
            });

            nodeIndex <<= 1;
            var bladeOfLegends = new SkillNode(nodeIndex, "Blade of Legends", 11, "Unlocks a legendary blade spell", (p) =>
            {
                profile.Talents[TalentID.SwordsSpellbookSpells].Points |= 0x8000;
            });

            nodeIndex <<= 1;
            var sonicSlash = new SkillNode(nodeIndex, "Sonic Slash", 11, "Passively increases attack speed", (p) =>
            {
                profile.Talents[TalentID.SwordsSpeed].Points += 1;
            });

            lethalEdge.AddChild(unyieldingSpirit);
            fortifiedDefense.AddChild(mirrorCounter);
            rapidAssault.AddChild(bladeOfLegends);
            serratedBlows.AddChild(sonicSlash);

            // Layer 7: Pinnacle nodes.
            nodeIndex <<= 1;
            var aegisOfValor = new SkillNode(nodeIndex, "Aegis of Valor", 12, "Passively increases defense", (p) =>
            {
                profile.Talents[TalentID.SwordsDefense].Points += 1;
            });

            nodeIndex <<= 1;
            var vortexOfSteel = new SkillNode(nodeIndex, "Vortex of Steel", 12, "Unlocks a devastating sword spell", (p) =>
            {
                profile.Talents[TalentID.SwordsSpellbookSpells].Points |= 0x200;
            });

            nodeIndex <<= 1;
            var tempestStrike = new SkillNode(nodeIndex, "Tempest Strike", 12, "Passively increases attack damage", (p) =>
            {
                profile.Talents[TalentID.SwordsAttack].Points += 1;
            });

            nodeIndex <<= 1;
            var guardiansMight = new SkillNode(nodeIndex, "Guardian's Might", 12, "Passively increases defense", (p) =>
            {
                profile.Talents[TalentID.SwordsDefense].Points += 1;
            });

            unyieldingSpirit.AddChild(aegisOfValor);
            mirrorCounter.AddChild(vortexOfSteel);
            bladeOfLegends.AddChild(tempestStrike);
            sonicSlash.AddChild(guardiansMight);

            // Layer 8: Ultimate node.
            nodeIndex <<= 1;
            var swordmastersAscension = new SkillNode(nodeIndex, "Swordmaster's Ascension", 13, "Ultimate bonus: unlocks final spells and boosts all sword skills", (p) =>
            {
                profile.Talents[TalentID.SwordsSpellbookSpells].Points |= (0x400 | 0x800);
                profile.Talents[TalentID.SwordsAttack].Points += 1;
                profile.Talents[TalentID.SwordsDefense].Points += 1;
                profile.Talents[TalentID.SwordsSpeed].Points += 1;
            });

            aegisOfValor.AddChild(swordmastersAscension);
            vortexOfSteel.AddChild(swordmastersAscension);
            tempestStrike.AddChild(swordmastersAscension);
            guardiansMight.AddChild(swordmastersAscension);
        }
    }

    // Command to open the Sword Skill Tree.
    public class SwordsSkillTreeCommand
    {
        public static void Initialize()
        {
            CommandSystem.Register("SwordTree", AccessLevel.Player, cmd => ShowTree(cmd.Mobile));
        }

        private static void ShowTree(Mobile m)
        {
            if (m is PlayerMobile pm)
            {
                pm.SendMessage("Opening Swordsmanship Skill Tree...");
                pm.SendGump(new SwordsSkillTree(pm));
            }
            else
            {
                m.SendMessage("You must be a player to use this command.");
            }
        }
    }
}
