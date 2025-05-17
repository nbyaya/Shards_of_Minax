using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class MoltenMenaceQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Molten Menace"; } }

        public override object Description
        {
            get
            {
                return
                    "You find *Calder Ashthorne*, the renowned potter of Fawn, standing outside his kiln, arms crossed, gaze fixed on a twisted shard of obsidian displayed nearby.\n\n" +
                    "“Have you ever seen clay melt before it touches flame? That’s what the **Glazeborn** does—it sears the very earth, warping my deposits into worthless sludge.”\n\n" +
                    "“My finest pottery is ruined, my name blackened. This creature, born of magma and fury, haunts the clay-rich wilds, boiling the land to feed its molten hunger.”\n\n" +
                    "“I’ve nothing but my hands and wheel—but you, you can end this. **Slay the Glazeborn** before it destroys the last of Fawn’s clay beds.”\n\n" +
                    "“Bring me peace, and I shall give you this: a *BeggarsLuckyBandana*. It may not shine, but it’s seen many a fortune turn.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then let my kiln grow cold, and my name fade with the last shard of unspoiled clay.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "The Glazeborn still roams? I can feel it… the earth near my workshop trembles with its rage.";
            }
        }

        public override object Complete
        {
            get
            {
                return "It is done? The clay cools once more.\n\n" +
                       "Take this, with my thanks—*the BeggarsLuckyBandana*. May it turn tides in your favor, as you have turned them in mine.\n\n" +
                       "And look here—this shard from its horn. A reminder of what fury once threatened Fawn.";
            }
        }

        public MoltenMenaceQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(Glazeborn), "Glazeborn", 1));
            AddReward(new BaseReward(typeof(BeggarsLuckyBandana), 1, "BeggarsLuckyBandana"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Molten Menace'!");
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

    public class CalderAshthorne : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(MoltenMenaceQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBMage()); // Closest vendor to a potter
        }

        [Constructable]
        public CalderAshthorne()
            : base("the Potter", "Calder Ashthorne")
        {
        }

        public CalderAshthorne(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 80, 25);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = 1153; // Soot-black
            FacialHairItemID = 0x2041; // Medium Beard
            FacialHairHue = 1153;
        }

        public override void InitOutfit()
        {
            AddItem(new FancyShirt() { Hue = 2412, Name = "Kiln-Touched Shirt" }); // Ash-gray
            AddItem(new LongPants() { Hue = 2101, Name = "Clay-Spattered Pants" }); // Earthy brown
            AddItem(new HalfApron() { Hue = 2932, Name = "Potter's Apron" }); // Deep red, kiln-colored
            AddItem(new Sandals() { Hue = 2405, Name = "Dusty Sandals" }); // Fired clay hue
            AddItem(new Bandana() { Hue = 1157, Name = "Ashthorne’s Wrap" }); // Charcoal gray, tied for working comfort

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Clay Satchel";
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
