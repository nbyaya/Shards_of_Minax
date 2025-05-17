using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class StagOfTheMistQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Stag of the Mist"; } }

        public override object Description
        {
            get
            {
                return
                    "You find *Galen Frostbrook*, a renowned weaver of Fawn, staring forlornly at a half-woven tapestry.\n\n" +
                    "The air around him feels damp, cold, as if unseen mist lingers.\n\n" +
                    "“It’s the *Marnstag*, I know it.”\n\n" +
                    "“Every night, its fog rolls down from the hills, slipping into my workshop. My threads grow wet, my patterns blur... but worse, the fog carries whispers. Songs of winter, of ice, of endings.”\n\n" +
                    "“They say its antlers summon the mist, that they’re touched by winter magic. If someone could bring down the beast, perhaps the fog would lift, and my work could breathe again.”\n\n" +
                    "**Hunt down the Marnstag**, before its mist consumes more than just cloth.";
            }
        }

        public override object Refuse
        {
            get
            {
                return "I see... then may the threads hold, though each night they grow colder.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still it roams? The fog thickens each dusk. My loom is nearly silent now, save for the drip of water from soaked threads.";
            }
        }

        public override object Complete
        {
            get
            {
                return "You’ve done it? The air feels clearer already... no mist tonight.\n\n" +
                       "Thank you, truly. Take this *SnowSculpture*. I wove it from the last threads touched by that cursed fog—transformed by your courage into something beautiful.";
            }
        }

        public StagOfTheMistQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(Marnstag), "Marnstag", 1)); // Assuming Marnstag is predefined
            AddReward(new BaseReward(typeof(SnowSculpture), 1, "SnowSculpture")); // Assuming SnowSculpture is predefined
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Stag of the Mist'!");
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

    public class GalenFrostbrook : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(StagOfTheMistQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBWeaver());
        }

        [Constructable]
        public GalenFrostbrook()
            : base("the Mist-Touched Weaver", "Galen Frostbrook")
        {
        }

        public GalenFrostbrook(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(80, 85, 40);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = 1150; // Frosty white
            FacialHairItemID = Race.RandomFacialHair(this);
            FacialHairHue = 1150;
        }

        public override void InitOutfit()
        {
            AddItem(new HoodedShroudOfShadows() { Hue = 1152, Name = "Frostshroud Cloak" }); // Icy blue
            AddItem(new FancyShirt() { Hue = 1153, Name = "Mistsilk Tunic" });
            AddItem(new LongPants() { Hue = 1150, Name = "Snowwoven Leggings" });
            AddItem(new Boots() { Hue = 1109, Name = "Chillstep Boots" });
            AddItem(new BodySash() { Hue = 1152, Name = "Threadbinder's Sash" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Weaver's Pack";
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
