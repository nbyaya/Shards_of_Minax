using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class CrimsonPeltQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Crimson Pelt"; } }

        public override object Description
        {
            get
            {
                return
                    "You approach *Thalia Herbwhisper*, the gentle yet resolute herbalist of Dawn.\n\n" +
                    "She tends a cluster of withered plants, her hands stained with dark red sap, her brow furrowed with worry.\n\n" +
                    "“My crimson blossoms… ruined. Trampled, poisoned, cursed. That foul fox, *Bloodbane*, haunts my fields still—drawn to the scent, mocking my craft.”\n\n" +
                    "“The beast’s pelt drinks moonlight, and where it treads, no herb may heal. I tracked it to the **Doom dungeon**, to a fissure deep within. If it lives, my tinctures die, and Dawn’s health fades with them.”\n\n" +
                    "“Bring me its pelt, so my blossoms may breathe once more.”\n\n" +
                    "**Slay the Bloodbane Fox**, and reclaim the fields.";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then I must hope another will end its hunt… though my blossoms may not survive the wait.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still it prowls? The blossoms wither further, and I feel the fox’s curse in the very air. Please… before it’s too late.";
            }
        }

        public override object Complete
        {
            get
            {
                return "You’ve done it! The crimson petals stir once more, and my tinctures will heal again.\n\n" +
                       "Take this, *ScripturewovenRobe*. It bears threads steeped in the wisdom of blossoms now reborn. May it guide your path as you’ve restored mine.";
            }
        }

        public CrimsonPeltQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(BloodbaneFox), "Bloodbane Fox", 1)); // Monster type already defined in your scripts
            AddReward(new BaseReward(typeof(ScripturewovenRobe), 1, "ScripturewovenRobe")); // Reward item already defined
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Crimson Pelt'!");
            Owner.PlaySound(CompleteSound);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class ThaliaHerbwhisper : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(CrimsonPeltQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBHerbalist());
        }

        [Constructable]
        public ThaliaHerbwhisper()
            : base("the Herbalist", "Thalia Herbwhisper")
        {
        }

        public ThaliaHerbwhisper(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 85, 35);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = 1359; // Rich auburn
        }

        public override void InitOutfit()
        {
            AddItem(new HoodedShroudOfShadows() { Hue = 1157, Name = "Veil of Crimson Blossoms" }); // Deep crimson, herbalist's symbolic hood
            AddItem(new FancyDress() { Hue = 2117, Name = "Petalwoven Dress" }); // Rose gold hue
            AddItem(new Sandals() { Hue = 2411, Name = "Sap-Stained Sandals" }); // Dark sap green
            AddItem(new BodySash() { Hue = 2212, Name = "Blossom Keeper's Sash" }); // Pale lavender

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Herbal Satchel";
            AddItem(backpack);

            AddItem(new MagicWand() { Hue = 1260, Name = "Tincture Wand" }); // Herbalist's tool, subtly enchanted
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
