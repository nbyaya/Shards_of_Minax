using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class EagleOfTheIceQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Eagle of the Ice"; } }

        public override object Description
        {
            get
            {
                return
                    "Before you stands *Arion Skygazer*, the renowned scout of Mountain Crest, clad in sky-hued garb that flutters like falcon wings.\n\n" +
                    "His sharp eyes scan the horizon, but a shadow darkens his usually serene gaze.\n\n" +
                    "“You’ve come at a dire time. My sky-hawks—trained to navigate these peaks and relay messages between clans—are vanishing. Plucked from the skies by a beast known as the **Glacial Eagle**.”\n\n" +
                    "“This isn’t just about birds. Without my scouts, Mountain Crest falls silent to the rest of Sosaria. We’re isolated. Vulnerable.”\n\n" +
                    "“This eagle strikes from above, its wings churning snow and cold as it hunts. You’ll need sky-ward defenses to bring it down.”\n\n" +
                    "**Slay the Glacial Eagle** and restore the winds to my hawks. Bring me proof, and you’ll have earned the trust of the Crest.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then watch the skies darken with loss. The Crest cannot stand without its wings.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still the eagle flies? The peaks are silent. No hawks return.";
            }
        }

        public override object Complete
        {
            get
            {
                return "The skies are ours again. You’ve done more than slay a beast—you’ve freed the winds.\n\n" +
                       "Take this, the *SilverMirror*. It reflects not just faces, but intentions. May it guide your flight as you’ve guided mine.";
            }
        }

        public EagleOfTheIceQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(GlacialEagle), "Glacial Eagle", 1));
            AddReward(new BaseReward(typeof(SilverMirror), 1, "SilverMirror"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Eagle of the Ice'!");
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

    public class ArionSkygazer : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(EagleOfTheIceQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => false;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBRanger());
        }

        [Constructable]
        public ArionSkygazer()
            : base("the Sky-Scout", "Arion Skygazer")
        {
        }

        public ArionSkygazer(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 85, 50);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = 1150; // Ice-White Hair
            FacialHairItemID = 0x203E; // Long Beard
            FacialHairHue = 1150;
        }

        public override void InitOutfit()
        {
            AddItem(new FancyShirt() { Hue = 1152, Name = "Skyweave Shirt" }); // Frosty Blue
            AddItem(new LeatherLegs() { Hue = 1102, Name = "Hawkhide Trousers" });
            AddItem(new Cloak() { Hue = 1153, Name = "Wings of the Crest" }); // Pale Sky Cloak
            AddItem(new LeatherGloves() { Hue = 1109, Name = "Clawcatcher's Grips" });
            AddItem(new FeatheredHat() { Hue = 1151, Name = "Hawkfeather Helm" });
            AddItem(new Boots() { Hue = 2101, Name = "Windwalkers" });

            AddItem(new CompositeBow() { Hue = 2105, Name = "Stormreach Bow" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Scout's Pack";
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
