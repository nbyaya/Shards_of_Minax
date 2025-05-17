using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class FreeThePastureQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Free the Pasture"; } }

        public override object Description
        {
            get
            {
                return
                    "You meet *Hilla Embercoat*, a devoted attendant of Devil Guard’s mineral-rich springs.\n\n" +
                    "Her hands are calloused from tending fires, and her robe smells faintly of cedar and earth.\n\n" +
                    "\"The steam... it whispers, you know? I thought it was just the wind, but I see them. Bovine shapes, flickering in the mist.\"\n\n" +
                    "\"There’s a feeding ground beneath the springs, long forgotten. But something’s stirred—a creature, a memory, *the BuriedCow*. It haunts the waters, spoils the warmth with its sorrow.\"\n\n" +
                    "\"I saved a calf once, from a cave-in below. I thought I did good. But now I fear I just woke something worse.\"\n\n" +
                    "\"Please, will you put it to rest? **Slay the BuriedCow** and free the springs of this taint.\"";
            }
        }

        public override object Refuse
        {
            get
            {
                return "I understand. But I fear the waters will grow colder, darker... until it consumes more than just dreams.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "It still lingers? I can feel it... in the steam. Please, don't let it suffer.";
            }
        }

        public override object Complete
        {
            get
            {
                return "You’ve done it? The mist clears, and the warmth returns. Thank you... you've brought peace to more than just the springs.\n\n" +
                       "Take this—*the Skull of the Sixth Moon*. A relic of old, to ward off restless dreams.";
            }
        }

        public FreeThePastureQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(BuriedCow), "BuriedCow", 1));
            AddReward(new BaseReward(typeof(SkullOfTheSixthMoon), 1, "Skull of the Sixth Moon"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Free the Pasture'!");
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

    public class HillaEmbercoat : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(FreeThePastureQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBBard()); // Closest vendor type to a bathhouse attendant, focused on wellness.
        }

        [Constructable]
        public HillaEmbercoat()
            : base("the Bathhouse Attendant", "Hilla Embercoat")
        {
        }

        public HillaEmbercoat(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(70, 60, 75);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = 33770; // Warm, earthy skin tone
            HairItemID = 0x2049; // Long hair
            HairHue = 1153; // Fiery auburn
        }

        public override void InitOutfit()
        {
            AddItem(new MonkRobe() { Hue = 1157, Name = "Ember-Kissed Robe" }); // Deep warm red
            AddItem(new Sandals() { Hue = 2405, Name = "Stonepath Sandals" }); // Soft grey
            AddItem(new HoodedShroudOfShadows() { Hue = 1175, Name = "Mistveil Hood" }); // Pale blue-grey, evokes steam
            AddItem(new HalfApron() { Hue = 2424, Name = "Ashen Apron" }); // Light ash color
            AddItem(new GoldRing() { Name = "Calf's Memory Ring" }); // Keepsake from her past

            Backpack backpack = new Backpack();
            backpack.Hue = 1175;
            backpack.Name = "Bath Attendant's Pack";
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
