using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class BindTheBinderQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Bind the Binder"; } }

        public override object Description
        {
            get
            {
                return
                    "You find yourself face to face with *Ratham Scaleweaver*, a broad-shouldered man clad in furs and scales, eyes glinting like burnished ore.\n\n" +
                    "“That creature... it wasn't always a monster.”\n\n" +
                    "“We trained binders to control the elementals, to work the ore safely. But the Orebinder—he got too strong. Now, he’s twisted, shackling them with cursed chains forged from silver ore pulled too deep.”\n\n" +
                    "“They scream when they mine. Not just in pain—but in rage. If he’s not stopped, they’ll tear this whole mountain apart.”\n\n" +
                    "**Slay the Orebinder** and free the elementals. Bring me proof, and I’ll reward you with a hide only the forest’s kin can grant.”";
            }
        }

        public override object Refuse
        {
            get { return "Then the chains stay tight, and the mountain weeps."; }
        }

        public override object Uncomplete
        {
            get { return "The Orebinder still holds them? The chains must be shattered, or we’re all at risk."; }
        }

        public override object Complete
        {
            get
            {
                return
                    "It’s done? The chains are broken?\n\n" +
                    "You’ve freed them—and spared us all from ruin. Take this: *HideOfTheForestKin*. May it shield you as you’ve shielded us.";
            }
        }

        public BindTheBinderQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(Orebinder), "Orebinder", 1));
            AddReward(new BaseReward(typeof(HideOfTheForestKin), 1, "HideOfTheForestKin"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Bind the Binder'!");
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

    public class RathamScaleweaver : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(BindTheBinderQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBAnimalTrainer());
        }

        [Constructable]
        public RathamScaleweaver()
            : base("the Beast Handler", "Ratham Scaleweaver")
        {
        }

        public RathamScaleweaver(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(100, 100, 50);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = 1325; // Weathered skin tone
            HairItemID = 0x2047; // Long hair
            HairHue = 1154; // Deep brown
            FacialHairItemID = 0x203F; // Thick beard
            FacialHairHue = 1154;
        }

        public override void InitOutfit()
        {
            AddItem(new StuddedChest() { Hue = 2412, Name = "Scale-Lined Vest" }); // Dark steel-blue
            AddItem(new StuddedArms() { Hue = 2101, Name = "Ore-Forged Bracers" }); // Ashen silver
            AddItem(new LeatherLegs() { Hue = 2502, Name = "Chain-Scored Breeches" }); // Tarnished bronze
            AddItem(new LeatherGloves() { Hue = 2413, Name = "Binder’s Grip" }); // Dark iron
            AddItem(new BearMask() { Hue = 1825, Name = "Beast Handler's Helm" }); // Slate gray
            AddItem(new Cloak() { Hue = 1109, Name = "Elemental Hide Cloak" }); // Dust-gray

            AddItem(new Pitchfork() { Hue = 2418, Name = "Tamer’s Goad" }); // Glinting silver

            Backpack backpack = new Backpack();
            backpack.Hue = 1175;
            backpack.Name = "Beast Handler's Pack";
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
