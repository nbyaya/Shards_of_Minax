using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class WingsOfDreadQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Wings of Dread"; } }

        public override object Description
        {
            get
            {
                return
                    "You find *Eamon Brightwood*, a painter in Fawn, gazing sorrowfully at a canvas that seems to darken with every stroke.\n\n" +
                    "He turns, his eyes sunken, fingers stained with dull pigment.\n\n" +
                    "“The Faelgrim flies again,” he murmurs. “When its wings cut across the night, my colors die... my murals fade within days of its shadow.”\n\n" +
                    "“I've heard its feathers shimmer with cursed pigment, a hue that devours light. I need you to stop it. Only then can my art breathe again.”\n\n" +
                    "**Vanquish the Faelgrim**, and bring an end to the midnight dread it casts upon Fawn.";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then I shall watch my canvases wither, and Fawn will lose what little color remains.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "The Faelgrim still soars, staining the skies and my soul alike. Will you end this torment?";
            }
        }

        public override object Complete
        {
            get
            {
                return "You’ve done it? The skies feel lighter, and already, I see hues returning to my brush.\n\n" +
                       "Accept this: **GlassSwordOfValor**. It was crafted for purity, like light through glass—unbroken, untainted. As your blade cut down dread, may this sword guard your spirit.";
            }
        }

        public WingsOfDreadQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(Faelgrim), "Faelgrim", 1));
            AddReward(new BaseReward(typeof(GlassSwordOfValor), 1, "GlassSwordOfValor"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Wings of Dread'!");
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

    public class EamonBrightwood : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(WingsOfDreadQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => false;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBGlassblower());
        }

        [Constructable]
        public EamonBrightwood()
            : base("the Gloom-Touched Painter", "Eamon Brightwood")
        {
        }

        public EamonBrightwood(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 65, 45);

            Female = false;
            Body = 0x190; // Male Human
            Race = Race.Human;

            Hue = 1002; // Pale
            HairItemID = 0x203B; // Long Hair
            HairHue = 1150; // Night-blue
        }

        public override void InitOutfit()
        {
            AddItem(new FancyShirt() { Hue = 1165, Name = "Painter’s Indigo Blouse" }); // Deep blue
            AddItem(new LongPants() { Hue = 1175, Name = "Pigment-Stained Trousers" }); // Muted violet
            AddItem(new Shoes() { Hue = 2406, Name = "Studio Slippers" }); // Soft grey
            AddItem(new HalfApron() { Hue = 2125, Name = "Canvas Apron" }); // Faded white
            AddItem(new Cloak() { Hue = 2101, Name = "Gloomweave Cloak" }); // Dark charcoal
            AddItem(new FeatheredHat() { Hue = 1153, Name = "Brightwood's Plume" }); // Midnight teal feather

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Artisan’s Pack";
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
