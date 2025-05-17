using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class WebOfShadowsQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Web of Shadows"; } }

        public override object Description
        {
            get
            {
                return
                    "Before you stands *Sylas Thorne*, a seasoned apiarist of Fawn, his eyes shadowed with worry despite the sweet hum of bees nearby.\n\n" +
                    "He tightens a leather glove, his fingers stained with honey and soot.\n\n" +
                    "“There’s a sickness in the hives, traveler. Not of bees—but of something dark and crawling. **Nyxra**, the shadow-weaver, coils beneath the hives, her webs souring the air, tainting the honey. I’ve heard her whisper in the smoke, seen the bitterness creep into our sweetest combs.”\n\n" +
                    "“I’ve tried flushing her out with smoke horns, but she’s too quick, too clever. The hives weaken each day she weaves. Fawn’s light is dimming, and I need a hand steady enough to silence her threads.”\n\n" +
                    "**Slay the Nyxra**, and let the sweetness return.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then may the hives suffer, and the sweetness of Fawn turn to ash.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "She still weaves? The smoke grows thick, and the bees die listless. Please, end her shadow.";
            }
        }

        public override object Complete
        {
            get
            {
                return "So she’s gone? The air feels lighter... and the honey, sweeter.\n\n" +
                       "Take this—*PopStarsTrove*. A gift from the hives and me. May it bring you joy as you've returned it to us.";
            }
        }

        public WebOfShadowsQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(Nyxra), "Nyxra", 1));
            AddReward(new BaseReward(typeof(PopStarsTrove), 1, "PopStarsTrove"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Web of Shadows'!");
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

    public class SylasThorne : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(WebOfShadowsQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBBeekeeper());
        }

        [Constructable]
        public SylasThorne()
            : base("the Apiarist", "Sylas Thorne")
        {
        }

        public SylasThorne(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 80, 30);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = 1153; // Smoky grey
            FacialHairItemID = Race.RandomFacialHair(this);
            FacialHairHue = 1153;
        }

        public override void InitOutfit()
        {
            AddItem(new Robe() { Hue = 2207, Name = "Thorne's Smokeward Robe" }); // Pale honey-gold
            AddItem(new HalfApron() { Hue = 2213, Name = "Beekeeper's Guard" }); // Deep amber
            AddItem(new StrawHat() { Hue = 1150, Name = "Hive-Caller's Hat" }); // Smoky grey
            AddItem(new Sandals() { Hue = 1194, Name = "Hivepath Sandals" }); // Soot-black
            AddItem(new LeatherGloves() { Hue = 2406, Name = "Honey-Stained Gloves" }); // Warm brown

            Backpack backpack = new Backpack();
            backpack.Hue = 1175; // Dusty gold
            backpack.Name = "Apiary Satchel";
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
