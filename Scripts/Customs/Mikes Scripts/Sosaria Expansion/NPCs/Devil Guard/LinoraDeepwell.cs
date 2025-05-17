using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class SlitherbaneQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Slitherbane"; } }

        public override object Description
        {
            get
            {
                return
                    "Linora Deepwell, Hydrologist of Devil Guard, approaches with a grave expression. Her hands are stained with mineral salts, her eyes reflecting worry.\n\n" +
                    "\"The groundwater that sustains Devil Guard is under threat. A creature—the CorrodedSlith—lurks in the Mines of Minax, its body leaking vile toxins into our springs.\"\n\n" +
                    "\"My own family’s orchard withered from poisoned wells years ago... I won't let it happen here.\"\n\n" +
                    "**Slay the CorrodedSlith** and bring peace to the waters of Devil Guard.";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then we all drink poison a little longer. May the springs not run dry before your return.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still the Slith lives? The water tastes heavier now... more bitter. We don't have much time.";
            }
        }

        public override object Complete
        {
            get
            {
                return "The springs run clearer already... you've done more than kill a beast. You've preserved life here.\n\n" +
                       "Take these: **Gloves of the Orchard's Grasp**. My family once cultivated with these, before the poison came. May they now bring growth to your hands.";
            }
        }

        public SlitherbaneQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(CorrodedSlith), "CorrodedSlith", 1));
            AddReward(new BaseReward(typeof(GlovesOfTheOrchardsGrasp), 1, "Gloves of the Orchard's Grasp"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Slitherbane'!");
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

    public class LinoraDeepwell : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(SlitherbaneQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBHerbalist());
        }

        [Constructable]
        public LinoraDeepwell()
            : base("the Hydrologist", "Linora Deepwell")
        {
        }

        public LinoraDeepwell(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 90, 50);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = 1030; // Pale
            HairItemID = 0x2049; // Long Hair
            HairHue = 1108; // Ashen Silver
        }

        public override void InitOutfit()
        {
            AddItem(new ElvenShirt() { Hue = 1260, Name = "Aquifer's Silk" }); // Water-blue
            AddItem(new Skirt() { Hue = 1154, Name = "Tidewoven Skirt" }); // Deep Sea
            AddItem(new LeatherGloves() { Hue = 1271, Name = "Springkeeper's Grips" }); // Earthy tone
            AddItem(new HoodedShroudOfShadows() { Hue = 1150, Name = "Wellshroud" }); // Shadowy, deep hue
            AddItem(new Sandals() { Hue = 1161, Name = "Waders of the Depths" });

            AddItem(new GnarledStaff() { Hue = 1193, Name = "Flowbinder" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1160;
            backpack.Name = "Hydrologist's Satchel";
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
