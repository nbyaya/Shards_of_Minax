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

namespace Server.ACC.CSS.Systems.ChivalryMagic
{
    // Chivalry Skill Tree Gump – similar to the Lumberjacking one.
    public class ChivalrySkillTree : SuperGump
    {
        private ChivalryTree tree;
        private Dictionary<ChivalrySkillNode, Point2D> nodePositions;
        private Dictionary<int, ChivalrySkillNode> buttonNodeMap;
        private Dictionary<ChivalrySkillNode, int> edgeThickness;
        private const int buttonSize = 15;
        private int ySpacing = 50, xSpacing = 60;
        private int rootX = 300, rootY = 150;
        private ChivalrySkillNode selectedNode;

        public ChivalrySkillTree(PlayerMobile user)
            : base(user, null, 100, 100)
        {
            tree = new ChivalryTree(user);
            nodePositions = new Dictionary<ChivalrySkillNode, Point2D>();
            buttonNodeMap = new Dictionary<int, ChivalrySkillNode>();
            edgeThickness = new Dictionary<ChivalrySkillNode, int>();

            CalculateNodePositions(tree.Root, rootX, rootY, 0);
            InitializeEdgeThickness();

            User.SendGump(this);
        }

        private void CalculateNodePositions(ChivalrySkillNode root, int x, int y, int depth)
        {
            if (root == null)
                return;

            var levelNodes = new Dictionary<int, List<ChivalrySkillNode>>();
            var queue = new Queue<(ChivalrySkillNode node, int level)>();
            var visited = new HashSet<ChivalrySkillNode>();

            queue.Enqueue((root, 0));

            while (queue.Count > 0)
            {
                var (node, level) = queue.Dequeue();

                if (!visited.Add(node))
                    continue;

                if (!levelNodes.ContainsKey(level))
                    levelNodes[level] = new List<ChivalrySkillNode>();

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
            layout.Add("title", () => { AddLabel(100, 20, 1153, "Chivalry Skill Tree"); });

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
            if (buttonNodeMap.TryGetValue(button.ButtonID, out ChivalrySkillNode node))
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

    // Skill node class for Chivalry.
    public class ChivalrySkillNode
    {
        public int BitFlag { get; }
        public string Name { get; }
        public int Cost { get; }
        public string Description { get; }
        public List<ChivalrySkillNode> Children { get; }
        public ChivalrySkillNode Parent { get; private set; }
        private readonly Action<PlayerMobile> onActivate;

        public ChivalrySkillNode(int bitFlag, string name, int cost, string description = "", Action<PlayerMobile> onActivate = null)
        {
            BitFlag = bitFlag;
            Name = name;
            Cost = cost;
            Description = description;
            Children = new List<ChivalrySkillNode>();
            this.onActivate = onActivate;
        }

        public void AddChild(ChivalrySkillNode child)
        {
            child.Parent = this;
            Children.Add(child);
        }

        public bool IsActivated(PlayerMobile player)
        {
            var profile = player.AcquireTalents();
            if (!profile.Talents.ContainsKey(TalentID.ChivalryNodes))
                profile.Talents[TalentID.ChivalryNodes] = new Talent(TalentID.ChivalryNodes) { Points = 0 };

            return (profile.Talents[TalentID.ChivalryNodes].Points & BitFlag) != 0;
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
            profile.Talents[TalentID.ChivalryNodes].Points |= BitFlag;

            player.SendMessage($"{Name} activated!");
            onActivate?.Invoke(player);
            return true;
        }
    }

    // The full Chivalry tree structure – 30 nodes over 9 layers.
    public class ChivalryTree
    {
        public ChivalrySkillNode Root { get; }

        public ChivalryTree(PlayerMobile player)
        {
            var profile = player.AcquireTalents();
            int nodeIndex = 0x01;

            // Layer 0: Root Node – Unlocks basic chivalry spells.
            Root = new ChivalrySkillNode(nodeIndex, "Oath of the Knight", 5, "Unlocks basic chivalry spells", (p) =>
            {
                profile.Talents[TalentID.ChivalrySpells].Points |= 0x01;
            });

            // Layer 1: Basic bonuses.
            nodeIndex <<= 1;
            var shieldMastery = new ChivalrySkillNode(nodeIndex, "Shield Mastery", 6, "Enhances defensive capabilities", (p) =>
            {
                profile.Talents[TalentID.ChivalryDefense].Points += 1;
            });

            nodeIndex <<= 1;
            var swordsmanship = new ChivalrySkillNode(nodeIndex, "Swordsmanship", 6, "Unlocks an offensive spell", (p) =>
            {
                profile.Talents[TalentID.ChivalrySpells].Points |= 0x40;
            });

            nodeIndex <<= 1;
            var holyFocus = new ChivalrySkillNode(nodeIndex, "Holy Focus", 6, "Unlocks a healing spell", (p) =>
            {
                profile.Talents[TalentID.ChivalrySpells].Points |= 0x80;
            });

            nodeIndex <<= 1;
            var valorBeacon = new ChivalrySkillNode(nodeIndex, "Valor's Beacon", 6, "Passively increases aura of courage", (p) =>
            {
                // (Passive bonus – effect to be implemented elsewhere)
            });

            Root.AddChild(shieldMastery);
            Root.AddChild(swordsmanship);
            Root.AddChild(holyFocus);
            Root.AddChild(valorBeacon);

            // Layer 2: Advanced bonuses.
            nodeIndex <<= 1;
            var knightlyResolve = new ChivalrySkillNode(nodeIndex, "Knightly Resolve", 7, "Unlocks additional spells", (p) =>
            {
                profile.Talents[TalentID.ChivalrySpells].Points |= 0x02;
            });

            nodeIndex <<= 1;
            var lancePrecision = new ChivalrySkillNode(nodeIndex, "Lance Precision", 7, "Further enhances offensive strikes", (p) =>
            {
                profile.Talents[TalentID.ChivalryDamage].Points += 1;
            });

            nodeIndex <<= 1;
            var blessedArmor = new ChivalrySkillNode(nodeIndex, "Blessed Armor", 7, "Unlocks a defensive spell", (p) =>
            {
                profile.Talents[TalentID.ChivalrySpells].Points |= 0x100;
            });

            nodeIndex <<= 1;
            var divineFavor = new ChivalrySkillNode(nodeIndex, "Divine Favor", 7, "Improves healing spells", (p) =>
            {
                profile.Talents[TalentID.ChivalryHealing].Points += 1;
            });

            shieldMastery.AddChild(knightlyResolve);
            swordsmanship.AddChild(lancePrecision);
            holyFocus.AddChild(blessedArmor);
            valorBeacon.AddChild(divineFavor);

            // Layer 3: Further enhancements.
            nodeIndex <<= 1;
            var righteousStrike = new ChivalrySkillNode(nodeIndex, "Righteous Strike", 8, "Boosts damage with holy power", (p) =>
            {
                profile.Talents[TalentID.ChivalryDamage].Points += 1;
            });

            nodeIndex <<= 1;
            var steadfastGuard = new ChivalrySkillNode(nodeIndex, "Steadfast Guard", 8, "Increases damage mitigation", (p) =>
            {
                profile.Talents[TalentID.ChivalryDefense].Points += 1;
            });

            nodeIndex <<= 1;
            var healingLight = new ChivalrySkillNode(nodeIndex, "Healing Light", 8, "Unlocks a healing spell", (p) =>
            {
                profile.Talents[TalentID.ChivalrySpells].Points |= 0x200;
            });

            nodeIndex <<= 1;
            var inspiringPresence = new ChivalrySkillNode(nodeIndex, "Inspiring Presence", 8, "Unlocks an inspirational spell", (p) =>
            {
                profile.Talents[TalentID.ChivalrySpells].Points |= 0x400;
            });

            knightlyResolve.AddChild(righteousStrike);
            lancePrecision.AddChild(steadfastGuard);
            blessedArmor.AddChild(healingLight);
            divineFavor.AddChild(inspiringPresence);

            // Layer 4: More advanced enhancements.
            nodeIndex <<= 1;
            var sacredCharge = new ChivalrySkillNode(nodeIndex, "Sacred Charge", 9, "Unlocks bonus chivalry spells", (p) =>
            {
                profile.Talents[TalentID.ChivalrySpells].Points |= 0x04;
            });

            nodeIndex <<= 1;
            var fortifiedSpirit = new ChivalrySkillNode(nodeIndex, "Fortified Spirit", 9, "Enhances overall defense", (p) =>
            {
                profile.Talents[TalentID.ChivalryDefense].Points += 1;
            });

            nodeIndex <<= 1;
            var piercingSmite = new ChivalrySkillNode(nodeIndex, "Piercing Smite", 9, "Unlocks an offensive spell", (p) =>
            {
                profile.Talents[TalentID.ChivalrySpells].Points |= 0x800;
            });

            nodeIndex <<= 1;
            var rejuvenatingAura = new ChivalrySkillNode(nodeIndex, "Rejuvenating Aura", 9, "Boosts healing and regeneration", (p) =>
            {
                profile.Talents[TalentID.ChivalryHealing].Points += 1;
            });

            righteousStrike.AddChild(sacredCharge);
            steadfastGuard.AddChild(fortifiedSpirit);
            healingLight.AddChild(piercingSmite);
            inspiringPresence.AddChild(rejuvenatingAura);

            // Layer 5: Expert-level nodes.
            nodeIndex <<= 1;
            var valorUnleashed = new ChivalrySkillNode(nodeIndex, "Valor Unleashed", 10, "Boosts all offensive capabilities", (p) =>
            {
                profile.Talents[TalentID.ChivalryDamage].Points += 1;
            });

            nodeIndex <<= 1;
            var shieldOfFaith = new ChivalrySkillNode(nodeIndex, "Shield of Faith", 10, "Enhances defensive capabilities significantly", (p) =>
            {
                profile.Talents[TalentID.ChivalryDefense].Points += 1;
            });

            nodeIndex <<= 1;
            var divineIntervention = new ChivalrySkillNode(nodeIndex, "Divine Intervention", 10, "Unlocks master healing spells", (p) =>
            {
                profile.Talents[TalentID.ChivalrySpells].Points |= 0x08;
            });

            nodeIndex <<= 1;
            var gallantStride = new ChivalrySkillNode(nodeIndex, "Gallant Stride", 10, "Unlocks a movement spell", (p) =>
            {
                profile.Talents[TalentID.ChivalrySpells].Points |= 0x1000;
            });

            sacredCharge.AddChild(valorUnleashed);
            fortifiedSpirit.AddChild(shieldOfFaith);
            piercingSmite.AddChild(divineIntervention);
            rejuvenatingAura.AddChild(gallantStride);

            // Layer 6: Mastery nodes.
            nodeIndex <<= 1;
            var unyieldingValor = new ChivalrySkillNode(nodeIndex, "Unyielding Valor", 11, "Unlocks an offensive spell", (p) =>
            {
                profile.Talents[TalentID.ChivalrySpells].Points |= 0x2000;
            });

            nodeIndex <<= 1;
            var impenetrableWall = new ChivalrySkillNode(nodeIndex, "Impenetrable Wall", 11, "Further boosts defensive strength", (p) =>
            {
                profile.Talents[TalentID.ChivalryDefense].Points += 1;
            });

            nodeIndex <<= 1;
            var miracleWorker = new ChivalrySkillNode(nodeIndex, "Miracle Worker", 11, "Improves healing miracles", (p) =>
            {
                profile.Talents[TalentID.ChivalryHealing].Points += 1;
            });

            nodeIndex <<= 1;
            var tacticalMastery = new ChivalrySkillNode(nodeIndex, "Tactical Mastery", 11, "Unlocks a tactical spell", (p) =>
            {
                profile.Talents[TalentID.ChivalrySpells].Points |= 0x4000;
            });

            valorUnleashed.AddChild(unyieldingValor);
            shieldOfFaith.AddChild(impenetrableWall);
            divineIntervention.AddChild(miracleWorker);
            gallantStride.AddChild(tacticalMastery);

            // Layer 7: Final, pinnacle bonuses.
            nodeIndex <<= 1;
            var holyAegis = new ChivalrySkillNode(nodeIndex, "Holy Aegis", 12, "Grants a powerful protective barrier", (p) =>
            {
                profile.Talents[TalentID.ChivalryDefense].Points += 1;
            });

            nodeIndex <<= 1;
            var ferventCharge = new ChivalrySkillNode(nodeIndex, "Fervent Charge", 12, "Further increases offensive strikes", (p) =>
            {
                profile.Talents[TalentID.ChivalryDamage].Points += 1;
            });

            nodeIndex <<= 1;
            var sanctifiedTouch = new ChivalrySkillNode(nodeIndex, "Sanctified Touch", 12, "Unlocks a healing spell", (p) =>
            {
                profile.Talents[TalentID.ChivalrySpells].Points |= 0x8000;
            });

            nodeIndex <<= 1;
            var beaconOfHope = new ChivalrySkillNode(nodeIndex, "Beacon of Hope", 12, "Empowers allies with resilience", (p) =>
            {
                // (Passive bonus – effect to be implemented elsewhere)
            });

            unyieldingValor.AddChild(holyAegis);
            impenetrableWall.AddChild(ferventCharge);
            miracleWorker.AddChild(sanctifiedTouch);
            tacticalMastery.AddChild(beaconOfHope);

            // Layer 8: Ultimate node.
            nodeIndex <<= 1;
            var ultimatePaladin = new ChivalrySkillNode(nodeIndex, "Ultimate Paladin", 13, "Ultimate bonus: Enhances all chivalry skills", (p) =>
            {
                profile.Talents[TalentID.ChivalrySpells].Points |= 0x10 | 0x20;
                profile.Talents[TalentID.ChivalryDefense].Points += 1;
                profile.Talents[TalentID.ChivalryDamage].Points += 1;
                profile.Talents[TalentID.ChivalryHealing].Points += 1;
            });

            holyAegis.AddChild(ultimatePaladin);
            ferventCharge.AddChild(ultimatePaladin);
            sanctifiedTouch.AddChild(ultimatePaladin);
            beaconOfHope.AddChild(ultimatePaladin);
        }
    }

    // Command to open the Chivalry Skill Tree.
    public class ChivalrySkillTreeCommand
    {
        public static void Initialize()
        {
            CommandSystem.Register("ChivalryTree", AccessLevel.Player, cmd => ShowTree(cmd.Mobile));
        }

        private static void ShowTree(Mobile m)
        {
            if (m is PlayerMobile pm)
            {
                pm.SendMessage("Opening Chivalry Skill Tree...");
                pm.SendGump(new ChivalrySkillTree(pm));
            }
            else
            {
                m.SendMessage("You must be a player to use this command.");
            }
        }
    }
}
