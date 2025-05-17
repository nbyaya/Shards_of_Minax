using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class SunderTheShadeQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Sunder the Shade"; } }

        public override object Description
        {
            get
            {
                return
                    "You stand before *Serene Nightbreeze*, Seer of Yew’s Moon Circle.\n\n" +
                    "Her robes shimmer faintly under the moonlight, and her eyes hold the quiet depth of countless visions.\n\n" +
                    "“The **BlightedShadow** has crawled from the Catastrophe's depths, and its presence dims our moon portals. I feel the darkness gnawing at the edges of my sight... my dreams twist into shadows that don’t belong to me.”\n\n" +
                    "“I’ve seen you in my visions, traveler. You alone can cleanse this blight.”\n\n" +
                    "**Venture into Catastrophe**, find the **BlightedShadow**, and banish it to restore the moon’s light.”\n\n" +
                    "“Only then can I continue the rites of my ancestors, and Yew may once more bask in the lunar glow.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then the shadows will thicken, and Yew will lose its guiding light. May the forest spirits shield us until you return.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "The BlightedShadow still taints the light. My visions falter, and darkness creeps ever closer to the heart of Yew.";
            }
        }

        public override object Complete
        {
            get
            {
                return "You’ve done it—the shadow has lifted, and I can feel the moon’s gentle pull once more.\n\n" +
                       "Take this, *PathboundSilence*. Let it remind you of your steps through darkness and the light you reclaimed for us all.";
            }
        }

        public SunderTheShadeQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(BlightedShadow), "BlightedShadow", 1));
            AddReward(new BaseReward(typeof(PathboundSilence), 1, "PathboundSilence"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Sunder the Shade'!");
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

    public class SereneNightbreeze : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(SunderTheShadeQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBCarpenter()); 
        }

        [Constructable]
        public SereneNightbreeze()
            : base("the Seer of the Moon Circle", "Serene Nightbreeze")
        {
        }

        public SereneNightbreeze(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 80, 90);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = 1153; // Pale moonlight hue
            HairItemID = 0x2049; // Long hair
            HairHue = 1150; // Silvery white
        }

        public override void InitOutfit()
        {
            AddItem(new FancyDress() { Hue = 1152, Name = "Moonweave Robe" });
            AddItem(new Cloak() { Hue = 1150, Name = "Nightbreeze Mantle" });
            AddItem(new Sandals() { Hue = 1109, Name = "Dreamwalkers" });
            AddItem(new WizardsHat() { Hue = 1153, Name = "Seer's Lunar Crown" });
            AddItem(new Scepter() { Hue = 2101, Name = "Starbind Rod" });

            Backpack backpack = new Backpack();
            backpack.Hue = 0;
            backpack.Name = "Moonlit Satchel";
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
