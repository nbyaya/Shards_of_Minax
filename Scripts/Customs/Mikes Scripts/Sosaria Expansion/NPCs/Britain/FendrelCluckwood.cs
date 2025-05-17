using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class CluckOfWarQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Cluck of War"; } }

        public override object Description
        {
            get
            {
                return
                    "*Fendrel Cluckwood*, Castle British’s ever-dedicated poulterer, grips a battered feed scoop, his apron singed and feathers clinging to his sleeves.\n\n" +
                    "“You wouldn’t believe it unless you’d seen it yourself—one of those blasted *RoosterUnitC7Warclucks* made its way into **Preservation Vault 44**. Thought it could lay low in the poultry pens I’d set up down there to provide eggs for the castle kitchens.”\n\n" +
                    "“Only, it’s gone fowl—lays **explosive eggs** now! Nearly roasted me alive when I went to check the feeders this morning.”\n\n" +
                    "“I rigged some harmless metal feeders as a precaution, but it won’t hold long. **I need you to find that rogue rooster and end its shell-shocking rampage** before the whole vault goes up in smoke.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then may the vault's walls hold, for I fear the next egg it lays might be our last breakfast...";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still at large? The guards won’t even *enter* the vault now—they claim they hear it *crowing in code*! This is no ordinary rooster!";
            }
        }

        public override object Complete
        {
            get
            {
                return
                    "You’ve done it? The warcluck is no more?\n\n" +
                    "*Fendrel beams, feathers puffing with pride.*\n\n" +
                    "“You've saved the castle’s kitchens—and perhaps the realm—from a truly eggstreme fate. Please, take this **HeraldSoulguardSurcoat**—wear it proudly, for none shall doubt you’ve braved the clucking inferno!”";
            }
        }

        public CluckOfWarQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(RoosterUnitC7Warcluck), "RoosterUnitC7Warcluck", 1));
            AddReward(new BaseReward(typeof(HeraldSoulguardSurcoat), 1, "HeraldSoulguardSurcoat"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Cluck of War'!");
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

    public class FendrelCluckwood : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(CluckOfWarQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBButcher());
        }

        [Constructable]
        public FendrelCluckwood()
            : base("the Poulterer", "Fendrel Cluckwood")
        {
        }

        public FendrelCluckwood(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 80, 40);

            Female = false;
            Body = 0x190; // Male Human
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = 1153; // Sandy Brown
            FacialHairItemID = 0x203F; // Full beard
            FacialHairHue = HairHue;
        }

        public override void InitOutfit()
        {
            AddItem(new HalfApron() { Hue = 1359, Name = "Feathered Apron" }); // Chicken-feather white
            AddItem(new FancyShirt() { Hue = 2115, Name = "Soot-Singed Shirt" });
            AddItem(new ShortPants() { Hue = 1102, Name = "Eggshell Breeches" });
            AddItem(new Sandals() { Hue = 2401, Name = "Coop-Slick Sandals" });
            AddItem(new StrawHat() { Hue = 1150, Name = "Cluckwood's Coop Cap" });
            AddItem(new SkinningKnife() { Hue = 2118, Name = "Feather Plucker" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1109;
            backpack.Name = "Poulterer's Satchel";
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
