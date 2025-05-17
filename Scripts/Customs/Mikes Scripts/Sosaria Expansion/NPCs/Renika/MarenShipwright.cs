using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class StormOfScalesQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Storm of Scales"; } }

        public override object Description
        {
            get
            {
                return
                    "You find *Maren Shipwright*, hammer poised over a plank of shimmering driftwood, her eyes fixed on the horizon beyond Renika's bustling docks.\n\n" +
                    "Her voice carries the salt of the sea:\n\n" +
                    "“A storm’s brewing. Not of wind or wave, but scale and fury.”\n\n" +
                    "“The *AncientHiryu* haunts the Mountain Stronghold, its scales the stuff of legend—stronger than iron, lighter than air. I need them, for the flagship of our coastal guard. Without them, our ships remain vulnerable... as they were when my grandmother faced the typhoon.”\n\n" +
                    "**Slay the AncientHiryu** and bring back its indestructible scales. Only then can we brace our hulls against what’s to come.";
            }
        }

        public override object Refuse
        {
            get
            {
                return "The sea waits for no one. If the scales remain in that beast's hide, then may the gods protect our fleet.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "The beast still soars? Then the storms to come will break our ships as easily as twigs.";
            }
        }

        public override object Complete
        {
            get
            {
                return "You’ve done it... These scales are finer than any I’ve seen.\n\n" +
                       "The hull will hold, thanks to you.\n\n" +
                       "Take this: *StarlightWizardsHat*. May it guide your course as surely as the stars led my grandmother to safe harbor.";
            }
        }

        public StormOfScalesQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(AncientHiryu), "AncientHiryu", 1));
            AddReward(new BaseReward(typeof(StarlightWizardsHat), 1, "StarlightWizardsHat"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Storm of Scales'!");
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

    public class MarenShipwright : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(StormOfScalesQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBShipwright(this));
        }

        [Constructable]
        public MarenShipwright()
            : base("the Master Shipwright", "Maren Shipwright")
        {
        }

        public MarenShipwright(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(90, 85, 30);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = 2101; // Seafoam Green
        }

        public override void InitOutfit()
        {
            AddItem(new FancyShirt() { Hue = 1154, Name = "Skyweave Blouse" }); // Azure-blue
            AddItem(new LongPants() { Hue = 1364, Name = "Stormproof Trousers" }); // Deep ocean
            AddItem(new HalfApron() { Hue = 1175, Name = "Dockworker's Apron" }); // Sea-salt white
            AddItem(new FeatheredHat() { Hue = 1153, Name = "Tidecaller’s Hat" }); // Cerulean
            AddItem(new Boots() { Hue = 1109, Name = "Wavewalker Boots" }); // Salt-washed gray
            AddItem(new Cutlass() { Hue = 0, Name = "Keelblade" }); // Standard steel with a nautical name

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Shipwright's Satchel";
            AddItem(backpack);
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
