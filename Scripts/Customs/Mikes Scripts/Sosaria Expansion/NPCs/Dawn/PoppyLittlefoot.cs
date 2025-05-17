using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class CaptainOfShadowsQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Captain of Shadows"; } }

        public override object Description
        {
            get
            {
                return
                    "You find *Poppy Littlefoot*, the Miller’s Apprentice of Dawn, fretting beside the old waterwheel.\n\n" +
                    "Her flour-dusted hands tremble, clutching a lantern flickering against the wind.\n\n" +
                    "“Listen—do you hear them? The shanties... they echo under the mill at midnight.”\n\n" +
                    "“My master tried to restart the wheel, but something below... something *wrong*... stopped it. There’s a **captain**, they say, once hung for piracy, now bound in shadows beneath us. The wheel won’t turn, and the grain won’t grind until he’s gone.”\n\n" +
                    "**Slay the DoomsCaptain** in the depths of the Doom Dungeon. His phantom sailors must follow him to the grave once more.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "“I understand... but I’ll pray the shanties fade before we all starve.”";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "“Still the wheel creaks in fear. The captain’s laugh... I heard it last night.”";
            }
        }

        public override object Complete
        {
            get
            {
                return "“The mill turns! Oh, bless you! No more midnight songs, no more shadowed figures by the wheel.”\n\n" +
                       "“Take this: *FingersOfTheDustbound*. A relic my master kept hidden, fearing it. Perhaps now, in your hands, it can be more than a curse.”";
            }
        }

        public CaptainOfShadowsQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(DoomsCaptain), "DoomsCaptain", 1));
            AddReward(new BaseReward(typeof(FingersOfTheDustbound), 1, "FingersOfTheDustbound"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Captain of Shadows'!");
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

    public class PoppyLittlefoot : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(CaptainOfShadowsQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBMiller());
        }

        [Constructable]
        public PoppyLittlefoot()
            : base("the Miller’s Apprentice", "Poppy Littlefoot")
        {
        }

        public PoppyLittlefoot(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(50, 45, 25);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = 1153; // Sandy-blonde
        }

        public override void InitOutfit()
        {
            AddItem(new FancyShirt() { Hue = 1157, Name = "Windspun Blouse" }); // Soft cream
            AddItem(new Skirt() { Hue = 2500, Name = "Mill-Dust Skirt" }); // Pale gray
            AddItem(new HalfApron() { Hue = 2101, Name = "Flour-Flecked Apron" }); // Light tan
            AddItem(new Sandals() { Hue = 2419, Name = "Threshing Sandals" }); // Weathered brown
            AddItem(new Cloak() { Hue = 1161, Name = "Lanternlight Cloak" }); // Soft gold, for evening warmth
            AddItem(new Bandana() { Hue = 1150, Name = "Wheatshade Bandana" }); // Pale yellow, keeps flour out of her hair

            Backpack backpack = new Backpack();
            backpack.Hue = 1102;
            backpack.Name = "Apprentice's Satchel";
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
