using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class GuardedSecretsQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Guarded Secrets"; } }

        public override object Description
        {
            get
            {
                return
                    "You find *Selric Wavebinder*, a meticulous scribe of Moon’s ethereal archives.\n\n" +
                    "He stands beneath a canopy of parchment scrolls, ink-stained fingers trembling as he gestures toward the shadowed palace beyond.\n\n" +
                    "“The records… centuries of decrees, prophecies, treaties—all at risk. A Royal Guard's spirit stalks the halls, blade drawn. But not for flesh. For *memory*.”\n\n" +
                    "“Its sword rends ink from the page, erases names from history. I have seen whole edicts fade before my eyes.”\n\n" +
                    "“I beg you, slay this spectral Royal Guard before it unbinds the past. The fates of Moon, perhaps of Sosaria, are penned in those scrolls.”\n\n" +
                    "**Destroy the Spectral Royal Guard** and preserve the written truth.";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then beware, traveler. A world without memory is a world adrift.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "The ink still bleeds... I can feel the tremors in the parchment. It still hunts.";
            }
        }

        public override object Complete
        {
            get
            {
                return "You have done it... The air is lighter, and the words remain.\n\n" +
                       "Take this: *Scriptcutter*. Forged to sever illusions and protect the truth—let it serve your hand as it has served our scrolls.";
            }
        }

        public GuardedSecretsQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(RoyalGuard), "Royal Guard", 1));
            AddReward(new BaseReward(typeof(Scriptcutter), 1, "Scriptcutter"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Guarded Secrets'!");
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

    public class SelricWavebinder : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(GuardedSecretsQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBHouseDeed()); 
        }
		
        [Constructable]
        public SelricWavebinder()
            : base("the Scribe of Moon", "Selric Wavebinder")
        {
        }

        public SelricWavebinder(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(70, 75, 60);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = 1023; // Pale, moonlit skin tone
            HairItemID = Race.RandomHair(this);
            HairHue = 1150; // Midnight blue
            FacialHairItemID = Race.RandomFacialHair(this);
            FacialHairHue = 1150;
        }

        public override void InitOutfit()
        {
            AddItem(new FancyShirt() { Hue = 1109, Name = "Inkthread Shirt" }); // Deep gray
            AddItem(new ElvenPants() { Hue = 1154, Name = "Moonwoven Trousers" }); // Pale silver-blue
            AddItem(new Cloak() { Hue = 1150, Name = "Scrollkeeper's Cloak" }); // Midnight blue
            AddItem(new BodySash() { Hue = 1175, Name = "Sash of Bound Secrets" }); // Ethereal silver
            AddItem(new Shoes() { Hue = 1151, Name = "Silent Step Shoes" }); // Dusk gray
            AddItem(new WizardsHat() { Hue = 1150, Name = "Archivist’s Cowl" }); // Midnight blue

            AddItem(new ScribeSword() { Hue = 1175, Name = "Inkfang" }); // Decorative, not for combat

            Backpack backpack = new Backpack();
            backpack.Hue = 1152;
            backpack.Name = "Scrollbinder's Pack";
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
