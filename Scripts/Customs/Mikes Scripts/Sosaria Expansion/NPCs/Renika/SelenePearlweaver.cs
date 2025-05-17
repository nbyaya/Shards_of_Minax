using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class WormOfTheDeepQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Worm of the Deep"; } }

        public override object Description
        {
            get
            {
                return
                    "*Selene Pearlweaver*, Renika’s most acclaimed tailor, stands draped in moonlit silks, her delicate hands clenched in frustration.\n\n" +
                    "“My latest gown—‘Midnight's Embrace’—was to be the crowning jewel of my career. Its heart, an **Obsidian Gem**, stolen by that horrid **Worm of the Deep**. It slithered from the Mountain Stronghold’s depths, crushing my dreams beneath its coils.”\n\n" +
                    "“I source my silks from Moon’s caravans, my needles are carved from the bones of sea-beasts—but nothing can replace that gem.”\n\n" +
                    "“You are my only hope. Slay the beast, and bring me my gem. Without it, the gown—and my reputation—are ruined.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Without that gem, my designs will fade into obscurity. I only hope someone else can brave that vile stronghold.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "The worm still lives? My scissors tremble... every stitch I sew is hollow without that gem's promise.";
            }
        }

        public override object Complete
        {
            get
            {
                return "The gem! You have it! My hands can breathe life into the gown once more.\n\n" +
                       "You’ve not just slain a beast—you’ve salvaged a legacy. Please, take these **WitchesBindingGloves**. May they hold your fate as firmly as your blade held mine.";
            }
        }

        public WormOfTheDeepQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(ObsidianWorm), "ObsidianWorm", 1));
            AddReward(new BaseReward(typeof(WitchesBindingGloves), 1, "WitchesBindingGloves"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Worm of the Deep'!");
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

    public class SelenePearlweaver : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(WormOfTheDeepQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBTailor());
        }

        [Constructable]
        public SelenePearlweaver()
            : base("the Tailor", "Selene Pearlweaver")
        {
        }

        public SelenePearlweaver(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(70, 60, 60);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = 0x203B; // Long Hair
            HairHue = 1153; // Pearl-white
        }

        public override void InitOutfit()
        {
            AddItem(new EveningGown() { Hue = 1150, Name = "Pearl-Silk Gown" }); // Soft Silver
            AddItem(new FancyShirt() { Hue = 2404, Name = "Moonlace Undergarment" }); // Moonlit Blue
            AddItem(new Sandals() { Hue = 2101, Name = "Seafoam Sandals" }); // Pale Aqua
            AddItem(new BodySash() { Hue = 2401, Name = "Gemweaver's Sash" }); // Midnight Violet
            AddItem(new Cloak() { Hue = 1157, Name = "Duskweave Cloak" }); // Dark Twilight
            AddItem(new FeatheredHat() { Hue = 2106, Name = "Silken Plume Hat" }); // Ivory with Sea Blue Feathers
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
