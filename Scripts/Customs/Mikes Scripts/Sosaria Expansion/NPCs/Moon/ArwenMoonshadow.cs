using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class DanceOfTheDepthsQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Dance of the Depths"; } }

        public override object Description
        {
            get
            {
                return
                    "You approach *Arwen Moonshadow*, the Mystic Dancer of Moon’s sacred fountain.\n\n" +
                    "Her voice ripples like water under starlight:\n\n" +
                    "“The *Nile Serpent* has defiled our sacred waters. Its coils thrash in rhythm with my steps, but they pull my audience into the depths.\n\n" +
                    "Each whisper it makes carries the sorrow of drowned sailors. I cannot dance while its presence persists.\n\n" +
                    "**Enter the fountain, face the Nile Serpent, and free our waters.**”";
            }
        }

        public override object Refuse { get { return "Then beware the songs that pull at your soul—they may not be mine."; } }

        public override object Uncomplete { get { return "The serpent still coils within? The waters remain restless. Return when the dance can flow unbroken."; } }

        public override object Complete { get { return "The serpent’s song has ended. The waters shimmer once more, and with them, gratitude. Accept this—may it guide your steps."; } }

        public DanceOfTheDepthsQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(NileSerpent), "Nile Serpent", 1));

            AddReward(new BaseReward(typeof(WayfarersLuckhorn), 1, "WayfarersLuckhorn"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Dance of the Depths'!");
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

    public class ArwenMoonshadow : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(DanceOfTheDepthsQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBFisherman()); 
        }

        [Constructable]
        public ArwenMoonshadow()
            : base("the Mystic Dancer", "Arwen Moonshadow")
        {
        }

        public ArwenMoonshadow(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(70, 60, 90);

            Female = true;
            Body = 0x191; // Female Body
            Race = Race.Human;

            Hue = Race.RandomSkinHue(); // Hair and skin
            HairItemID = Race.RandomHair(this);
            HairHue = 1150; // Pale silver hair
        }

        public override void InitOutfit()
        {
            // Unique, themed outfit reflecting water and moonlight
            AddItem(new FancyDress() { Hue = 1153, Name = "Moonshadow Gown" }); // Deep shimmering blue
            AddItem(new Cloak() { Hue = 1157, Name = "Veil of Tides" }); // Midnight blue
            AddItem(new Sandals() { Hue = 1154, Name = "Watersoft Sandals" }); // Pale seafoam
            AddItem(new FlowerGarland() { Hue = 1152, Name = "Tideblossom Wreath" }); // Soft aqua-blue flowers
            AddItem(new BodySash() { Hue = 1156, Name = "Ribbon of Ripples" }); // Gentle cyan sash

            Backpack backpack = new Backpack();
            backpack.Hue = 44;
            backpack.Name = "Dancer's Satchel";
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
