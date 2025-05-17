using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class AbominationArisenQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Abomination Arisen"; } }

        public override object Description
        {
            get
            {
                return
                    "You stand before *Petra Glacierfall*, Naturalist of Mountain Crest, her coat shimmering like frost at dawn.\n\n" +
                    "She gazes towards the looming Ice Cavern, voice low and urgent:\n\n" +
                    "“The Glacial Abomination… it should not exist. I’ve spent years cataloging these lands, their balance, their beauty. But now, something unnatural stirs in the lower galleries—twisted by ancient magic, or perhaps something worse.”\n\n" +
                    "“Its roars chill the blood. They carry across the peaks, warping the behavior of every beast from here to Dawn. I can feel the land suffering.”\n\n" +
                    "“I beg you, venture into the Ice Cavern and **slay the Glacial Abomination**. Restore the balance, or this whole region may fall to frost and fear.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then the mountains will continue to cry out in pain. I hope you reconsider, before the cold claims us all.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still it roams? Then the winds will howl, and the snow will not cease. You must act swiftly.";
            }
        }

        public override object Complete
        {
            get
            {
                return "You’ve done it... The echoes have stopped, and the land breathes easier now.\n\n" +
                       "**The balance is restored**, thanks to your strength and courage.\n\n" +
                       "Take this: *AnglersSeabreezeCloak*. May it keep you warm on the coldest journeys.";
            }
        }

        public AbominationArisenQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(GlacialAbomination), "Glacial Abomination", 1));
            AddReward(new BaseReward(typeof(AnglersSeabreezeCloak), 1, "AnglersSeabreezeCloak"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Abomination Arisen'!");
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

    public class PetraGlacierfall : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(AbominationArisenQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBHerbalist()); // Closest to her Naturalist role
        }

        [Constructable]
        public PetraGlacierfall()
            : base("the Naturalist", "Petra Glacierfall")
        {
        }

        public PetraGlacierfall(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 80, 30);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = 1150; // Pale, frost-kissed skin tone
            HairItemID = 0x203B; // Long hair
            HairHue = 1152; // Icy silver
        }

        public override void InitOutfit()
        {
            AddItem(new FancyDress() { Hue = 1153, Name = "Frostweave Gown" }); // Pale blue shimmer
            AddItem(new Cloak() { Hue = 1154, Name = "Glacier's Embrace Cloak" }); // Deep ice-blue
            AddItem(new Boots() { Hue = 1109, Name = "Snowstride Boots" }); // Dusty white-grey
            AddItem(new BodySash() { Hue = 1157, Name = "Seal of the Crest" }); // A sash with a crest symbol
            AddItem(new FeatheredHat() { Hue = 1153, Name = "Aurora Plume Hat" }); // Matches dress, adorned with a white feather

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Naturalist's Satchel";
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
