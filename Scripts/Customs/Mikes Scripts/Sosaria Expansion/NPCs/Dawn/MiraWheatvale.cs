using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class VanquishShadowSteedQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Vanquish the Shadow Steed"; } }

        public override object Description
        {
            get
            {
                return
                    "You meet *Mira Wheatvale*, a farmer whose eyes are red from sleepless nights.\n\n" +
                    "Her hands are calloused from toil, yet one clutches tightly to a charm of sunstone, trembling slightly.\n\n" +
                    "“The fields aren't safe anymore,” she says, voice raw. “First it was the goats... now it's everything. Crops wilting, animals gone mad with fear. I saw the prints—it’s hooves, but not like any beast I know. They *burned* the earth. Drained it.”\n\n" +
                    "“After the last lunar eclipse, folks started whispering. Said something slipped from the vaults below Doom Dungeon, and now it rides here, in the moonlight.”\n\n" +
                    "“I can't lose more. The *Abyssal Horse* must be stopped.”\n\n" +
                    "**Slay the Shadow Steed**, bring back peace to Dawn's eastern fields—and protect what little light we have left.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then may the gods shield us, for we cannot stand against this darkness alone.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still it rides? My fields rot under its shadow. Please—there is little time.";
            }
        }

        public override object Complete
        {
            get
            {
                return "You’ve done it? Truly? The hoofprints... they’ll fade now, won’t they?\n\n" +
                       "*Mira clasps your hands, pressing a sunstone crest into your palm.*\n\n" +
                       "“Take this. It’s been in my family for generations, given in thanks to those who guard the dawn.”\n\n" +
                       "“The light returns because of you.”";
            }
        }

        public VanquishShadowSteedQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(AbyssalHorse), "Abyssal Horse", 1));
            AddReward(new BaseReward(typeof(SunwardensCrest), 1, "SunwardensCrest"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Vanquish the Shadow Steed'!");
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

    public class MiraWheatvale : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(VanquishShadowSteedQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBFarmer());
        }

        [Constructable]
        public MiraWheatvale()
            : base("the Goat Herder", "Mira Wheatvale")
        {
        }

        public MiraWheatvale(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(80, 85, 30);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = 0x203C; // Long hair
            HairHue = 1153; // Sun-kissed blonde
        }

        public override void InitOutfit()
        {
            AddItem(new FancyShirt() { Hue = 2112, Name = "Harvest-Blush Shirt" }); // Warm pink hue
            AddItem(new LongPants() { Hue = 2407, Name = "Dawn-Breaker Trousers" }); // Earthy brown hue
            AddItem(new HalfApron() { Hue = 2309, Name = "Sunwoven Apron" }); // Golden hue
            AddItem(new StrawHat() { Hue = 2118, Name = "Fieldshade Hat" }); // Pale tan hue
            AddItem(new Sandals() { Hue = 2213, Name = "Duststep Sandals" }); // Light beige hue

            AddItem(new ShepherdsCrook() { Hue = 2101, Name = "Wheatvale Crook" }); // Wooden, hand-carved
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
