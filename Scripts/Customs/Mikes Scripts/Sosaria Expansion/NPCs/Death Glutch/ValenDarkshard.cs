using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class FlickerOfMaliceQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Flicker of Malice"; } }

        public override object Description
        {
            get
            {
                return
                    "You stand before *Valen Darkshard*, the Candle Maker of Death Glutch.\n\n" +
                    "He stares at you from behind a veil of melted wax, his fingers stained with soot and amber.\n\n" +
                    "“You ever walk these streets by night? Feel that chill? That isn’t wind. It’s *fear*.”\n\n" +
                    "“My candles—they keep the dark at bay. Not just shadows, mind you, but *things* that thrive when the light dies.”\n\n" +
                    "“There’s a *Hexling* loose. Slipped from Malidor’s ruins, they say. Little beast’s been snuffing out my lanterns, night after night. Travelers stumble, never reach home. Some never return.”\n\n" +
                    "**“Slay the Hexling. Bring back proof. Or soon, no light will burn here at all.”**";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then keep to the fires, stranger. For when they go out... nothing will save you.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still flickers? Still falls dark? Then we're not safe yet.";
            }
        }

        public override object Complete
        {
            get
            {
                return "It’s dead? The light holds? Then take this—*MirageChest*, I call it. It’ll keep your goods safe, hidden in plain sight, just like my lanterns hide us from the dark.\n\n" +
                       "**You’ve given Death Glutch another night to breathe.**";
            }
        }

        public FlickerOfMaliceQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(Hexling), "Hexling", 1));
            AddReward(new BaseReward(typeof(MirageChest), 1, "MirageChest"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Flicker of Malice'!");
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

    public class ValenDarkshard : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(FlickerOfMaliceQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBBeekeeper());
        }

        [Constructable]
        public ValenDarkshard()
            : base("the Candle Maker", "Valen Darkshard")
        {
        }

        public ValenDarkshard(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 60, 50);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = 1001; // Pale as wax
            HairItemID = 0x2047; // Long hair
            HairHue = 1109; // Ash gray
            FacialHairItemID = 0x203F; // Long beard
            FacialHairHue = 1109;
        }

        public override void InitOutfit()
        {
            AddItem(new HoodedShroudOfShadows() { Hue = 1150, Name = "Wax-Drenched Shroud" }); // Dark candle-stained
            AddItem(new LeatherGloves() { Hue = 2413, Name = "Soot-Stained Gloves" }); // Blackened leather
            AddItem(new Sandals() { Hue = 2210, Name = "Flickering Sandals" }); // Ember-glow sandals
            AddItem(new BodySash() { Hue = 1154, Name = "Glowthread Sash" }); // Blue-glow, like magical flame
            AddItem(new HalfApron() { Hue = 2309, Name = "Candle Maker's Apron" }); // Wax-stained

            AddItem(new WitchBurningTorch() { Hue = 1161, Name = "Lantern of Truth" }); // His special lantern

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Chandler's Pack";
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
