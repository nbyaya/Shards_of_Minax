using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class SerpentsChartQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Serpent's Chart"; } }

        public override object Description
        {
            get
            {
                return
                    "You meet *Dax Stormsail*, renowned mapmaker of Renika, seated at a cluttered desk covered with half-burned maps.\n\n" +
                    "His fingers trace torn parchment, eyes squinting at a partially finished chart.\n\n" +
                    "“You ever try to map a mountain’s rage? I have. The *CragSerpent*—it devours more than men, it devours my maps.”\n\n" +
                    "“I chart the peaks to guide lost sailors, to find paths others won’t dare. But this serpent, it’s no mere beast. Its scales shred the winds, its breath stirs avalanches. Every path I draw vanishes beneath its fury.”\n\n" +
                    "“I once survived a wreck by reading mountains like waves. Now, I need those peaks to speak clearly again.”\n\n" +
                    "**Slay the CragSerpent**. Let me finish my *Serpent’s Chart* before the peaks bury us all.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then I’ll watch the serpent unmake what I cannot remake. The peaks are patient, but storms never are.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still it coils around the mountain? My maps are worthless parchment until its shadow lifts.";
            }
        }

        public override object Complete
        {
            get
            {
                return "It’s done? The serpent sleeps beneath the snow?\n\n" +
                       "Then here—*BrassBell*. My old signal, once used to guide lost ships. Let it guide you through storms of your own.\n\n" +
                       "Thanks to you, the mountains will speak again—and I will listen.";
            }
        }

        public SerpentsChartQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(CragSerpent), "CragSerpent", 1));
            AddReward(new BaseReward(typeof(BrassBell), 1, "BrassBell"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Serpent's Chart'!");
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

    public class DaxStormsail : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(SerpentsChartQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => false;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBMapmaker());
        }

        [Constructable]
        public DaxStormsail()
            : base("the Mountain Cartographer", "Dax Stormsail")
        {
        }

        public DaxStormsail(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(80, 85, 40);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = 1153; // Weathered silver-blue
            FacialHairItemID = 0x204C; // Short beard
            FacialHairHue = 1153;
        }

        public override void InitOutfit()
        {
            AddItem(new FancyShirt() { Hue = 1150, Name = "Storm-Crested Shirt" }); // Pale storm-gray
            AddItem(new ShortPants() { Hue = 1324, Name = "Chartmaker's Breeches" }); // Sea-blue
            AddItem(new BodySash() { Hue = 2207, Name = "Navigator's Sash" }); // Deep emerald
            AddItem(new Cloak() { Hue = 1175, Name = "Windborne Cloak" }); // Sky teal
            AddItem(new Boots() { Hue = 1109, Name = "Peak-Striders" }); // Slate gray
            AddItem(new FeatheredHat() { Hue = 1171, Name = "Stormsail Plume" }); // Blue-white feathered hat

            AddItem(new GnarledStaff() { Hue = 0x47E, Name = "Wayfinder's Cane" }); // Misty silver staff
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
