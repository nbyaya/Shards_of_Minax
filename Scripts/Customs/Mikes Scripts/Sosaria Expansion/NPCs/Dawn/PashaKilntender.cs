using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class ClayWardenQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Clay Warden"; } }

        public override object Description
        {
            get
            {
                return
                    "You find yourself in the warm, earthy studio of *Pasha Kilntender*, Dawn’s beloved potter.\n\n" +
                    "Her hands tremble, speckled with clay, eyes fixed on the kiln’s smoke curling into unnatural shapes.\n\n" +
                    "“It perches there now, that *thing*. A gargoyle—but no stone I’ve ever fired. It came in the night, roosting on my kiln, whispering to the clay...”\n\n" +
                    "“Now my pots twist as they bake. Handles like claws. Faces where none should be. It’s cursed my craft, cursed my home.”\n\n" +
                    "“They call it the **Cult Gargoyle**. Born of the Doom dungeon’s foul rites, drawn to fire and form. It sleeps by day, but when the kiln glows at midnight, it stirs—and my dreams rot with it.”\n\n" +
                    "**Slay the Cult Gargoyle** that haunts my kiln. Let me shape beauty again, not horror.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then the kiln shall stay cold, and the clay forever unformed. But I will not let it shape me.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still it clings to the chimney? The clay twists worse by the day. My fingers ache, not from work, but from its will.";
            }
        }

        public override object Complete
        {
            get
            {
                return "You have freed my fire from its grasp.\n\n" +
                       "The gargoyle’s curse is broken, and my craft is my own again.\n\n" +
                       "Take this: *FrondsOfTheDreamingTouch*. May it lend grace to your hands as you gave strength to mine.";
            }
        }

        public ClayWardenQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(CultGargoyle), "Cult Gargoyle", 1));
            AddReward(new BaseReward(typeof(FrondsOfTheDreamingTouch), 1, "FrondsOfTheDreamingTouch"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Clay Warden'!");
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

    public class PashaKilntender : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(ClayWardenQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBCarpenter()); // Closest fit for a potter in available vendor types.
        }

        [Constructable]
        public PashaKilntender()
            : base("the Potter", "Pasha Kilntender")
        {
        }

        public PashaKilntender(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(80, 70, 40);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = 1137; // Earthen brown
        }

        public override void InitOutfit()
        {
            AddItem(new FancyDress() { Hue = 2208, Name = "Kiln-Warmed Dress" }); // Clay red
            AddItem(new HalfApron() { Hue = 2424, Name = "Ash-Stained Apron" }); // Soot-grey
            AddItem(new Sandals() { Hue = 2401, Name = "Ember-Step Sandals" }); // Burnt orange
            AddItem(new FeatheredHat() { Hue = 1824, Name = "Smoke-Plume Hat" }); // Dark grey plume
            AddItem(new LeatherGloves() { Hue = 2434, Name = "Potter’s Touch" }); // Pale beige gloves

            Backpack backpack = new Backpack();
            backpack.Hue = 1153; // Soft clay
            backpack.Name = "Clay-Carrier's Pack";
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
