using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class StoneheartsEndQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Stoneheart's End"; } }

        public override object Description
        {
            get
            {
                return
                    "You find **Valeria Dawnshard**, the renowned bard of Fawn, seated by the riverside. Her harp rests silent on her lap, and a stave of petrified wood leans against her shoulder.\n\n" +
                    "She gazes at the waters with a troubled look, her voice hushed:\n\n" +
                    "“Have you heard the silence? Even the river songs falter... the **Onyxith** is near.”\n\n" +
                    "“Each time it stirs, the ground trembles, and my ballads crumble. If the Onyxith petrifies the riverbank, Fawn’s heart will harden with it. The songs will be lost, as will we.”\n\n" +
                    "**Vanquish the Onyxith**, before stone claims the river and silence claims the city.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "“Then let the Onyxith rumble, and the silence spread. May you never need song to soothe your spirit.”";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "“Still it trembles? The earth groans louder each night... I fear I will forget the sound of music.”";
            }
        }

        public override object Complete
        {
            get
            {
                return "**The earth calms, and Valeria's eyes light up once more.**\n\n" +
                       "“You’ve done it! The Onyxith’s rumble no longer drowns our voices. The river sings again.”\n\n" +
                       "“Take this: *Shogun’s Authoritative Surcoat*. May its presence remind all who see it that you gave Fawn back its voice.”";
            }
        }

        public StoneheartsEndQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(Onyxith), "the Onyxith", 1));
            AddReward(new BaseReward(typeof(ShogunsAuthoritativeSurcoat), 1, "Shogun’s Authoritative Surcoat"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Stoneheart's End'!");
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

    public class ValeriaDawnshard : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(StoneheartsEndQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => false;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBBard());
        }

        [Constructable]
        public ValeriaDawnshard()
            : base("the Bard of Fawn", "Valeria Dawnshard")
        {
        }

        public ValeriaDawnshard(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(70, 85, 60);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = 0x203C; // Long hair
            HairHue = 1166; // Deep Amethyst
        }

        public override void InitOutfit()
        {
            AddItem(new FancyShirt() { Hue = 1359, Name = "Velvet Songweaver's Shirt" }); // Deep violet
            AddItem(new Skirt() { Hue = 1271, Name = "Riverflow Skirt" }); // Pale azure
            AddItem(new BodySash() { Hue = 1153, Name = "Lament's Embrace" }); // Ethereal blue
            AddItem(new Sandals() { Hue = 2117, Name = "Stone-Touched Sandals" }); // Stone-grey
            AddItem(new Cloak() { Hue = 1150, Name = "Echo's Cloak" }); // Phantom white

            AddItem(new GnarledStaff() { Hue = 1109, Name = "Stave of Petrified Song" }); // Petrified wood
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
