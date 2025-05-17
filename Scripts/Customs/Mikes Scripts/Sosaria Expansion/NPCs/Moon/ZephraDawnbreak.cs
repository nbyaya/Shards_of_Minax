using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class SerpentsWakeQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Serpent's Wake"; } }

        public override object Description
        {
            get
            {
                return
                    "You meet *Zephra Dawnbreak*, Falconer of Moon’s River Outpost.\n\n" +
                    "“The Nile River Serpent coils in the moonlit shallows, striking at anything that dares the water—" +
                    "including my falcons. I cannot stand by while they vanish to its hunger.\n\n" +
                    "Its scales catch the moonlight, blinding even the boldest hunter. Will you be the one to end its reign?”\n\n" +
                    "**Slay the Nile River Serpent** that threatens Moon’s riverbanks.";
            }
        }

        public override object Refuse { get { return "Then may the winds carry warning to others. Beware the moonlit coils."; } }

        public override object Uncomplete { get { return "The serpent still coils along the river’s edge? My birds grow restless with fear—please hurry."; } }

        public override object Complete { get { return "It is done? The river sighs with relief, and my falcons soar once more. Accept these IronrootGaiters—a gift of balance and strength."; } }

        public SerpentsWakeQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(NileRiverSerpent), "NileRiverSerpent", 1));

            AddReward(new BaseReward(typeof(IronrootGaiters), 1, "IronrootGaiters"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Serpent's Wake'!");
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

    public class ZephraDawnbreak : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(SerpentsWakeQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBAlchemist(this)); 
        }

        [Constructable]
        public ZephraDawnbreak()
            : base("the Falconer", "Zephra Dawnbreak")
        {
        }

        public ZephraDawnbreak(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(85, 75, 50);

            Female = true;
            Body = 0x191; // Female body
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = 1150; // Pale silver hair
        }

        public override void InitOutfit()
        {
            AddItem(new FemaleLeatherChest() { Hue = 2207, Name = "Desertwind Jerkin" }); // Sandy tan leather
            AddItem(new LeatherNinjaPants() { Hue = 2208, Name = "Moonweave Leggings" }); // Pale cream with silver accents
            AddItem(new LeatherGloves() { Hue = 1153, Name = "Falconer's Grip" }); // Deep sky-blue
            AddItem(new Cloak() { Hue = 2405, Name = "Dawnveil Cloak" }); // Soft sunrise orange
            AddItem(new FeatheredHat() { Hue = 2209, Name = "Zephra's Plume" }); // Dusty gold with falcon feathers
            AddItem(new Sandals() { Hue = 2407, Name = "Step of the Horizon" }); // Natural leather hue
            AddItem(new QuarterStaff() { Hue = 2413, Name = "Falconer's Staff" }); // Light wood, engraved with wind motifs

            Backpack backpack = new Backpack();
            backpack.Hue = 32;
            backpack.Name = "Falconry Satchel";
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
