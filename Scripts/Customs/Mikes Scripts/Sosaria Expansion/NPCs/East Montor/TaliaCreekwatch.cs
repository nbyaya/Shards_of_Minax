using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class TigersProwlQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Tiger’s Prowl"; } }

        public override object Description
        {
            get
            {
                return
                    "Talia Creekwatch, the vigilant ranger of East Montor, sharpens her blade by the flickering light of a campfire.\n\n" +
                    "Her eyes, like storm-hardened steel, scan the distance constantly, and her gear is worn from countless patrols along the creek.\n\n" +
                    "“Every step I take near the water, it watches. It waits. This isn’t just a beast—it’s a shadow that stalks our breath.”\n\n" +
                    "“The **DrakonsTiger** lurks by the creek’s edge, beneath the moon’s glow. I’ve lost good rangers to its claws. I can’t stop it alone.”\n\n" +
                    "“Its eyes burn like embers. Track it, face it, end it. Bring peace back to East Montor’s wilds.”\n\n" +
                    "**Slay the DrakonsTiger** and ensure the creek runs quiet again.";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then beware the creek at dusk. It waits, and it does not forgive hesitation.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "The tiger still roams? I hear its growl in the wind… the creek is no longer safe for any of us.";
            }
        }

        public override object Complete
        {
            get
            {
                return "It’s done? The creek breathes easy once more, and the night has lost its teeth.\n\n" +
                       "You’ve earned more than thanks. Take this—*MakhairaOfAchilles*. Let its edge serve you as fiercely as you’ve served us.";
            }
        }

        public TigersProwlQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(DrakonsTiger), "DrakonsTiger", 1));
            AddReward(new BaseReward(typeof(MakhairaOfAchilles), 1, "MakhairaOfAchilles"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Tiger’s Prowl'!");
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

    public class TaliaCreekwatch : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(TigersProwlQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBRanger()); // Ranger Vendor
        }

        [Constructable]
        public TaliaCreekwatch()
            : base("the Creekwatch Ranger", "Talia Creekwatch")
        {
        }

        public TaliaCreekwatch(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(90, 95, 40);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = 1153; // Deep Forest Green
        }

        public override void InitOutfit()
        {
            AddItem(new StuddedChest() { Hue = 2125, Name = "Creekwatch Vest" }); // Earthy brown-green
            AddItem(new LeatherLegs() { Hue = 1820, Name = "Foreststalker’s Trousers" });
            AddItem(new LeatherGloves() { Hue = 1820, Name = "Tracker’s Grips" });
            AddItem(new FeatheredHat() { Hue = 2101, Name = "Talia’s Ranger Hat" }); // Soft moss green
            AddItem(new Cloak() { Hue = 2105, Name = "Moonlit Cloak" }); // Silvery-green hue
            AddItem(new Boots() { Hue = 1815, Name = "Silentstep Boots" });

            AddItem(new RangersCrossbow() { Hue = 2505, Name = "Creekwatch Bow" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Ranger's Pack";
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
