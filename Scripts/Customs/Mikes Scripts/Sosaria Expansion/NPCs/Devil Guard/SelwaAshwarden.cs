using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class ExtinguishWispQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Extinguish the Wisp"; } }

        public override object Description
        {
            get
            {
                return
                    "Selwa Ashwarden, keeper of the healing springs, approaches with a furrowed brow and hands scented of lavender and ash.\n\n" +
                    "\"The springs no longer soothe as they should. Last harvest, I sensed the taint—embers in the water, heat that doesn't heal but burns. The miners whispered of a CoalWisp, rising from the old tunnels of Minax. Now, it poisons what we cherish.\"\n\n" +
                    "\"I am no warrior, but my rituals shield our people from its cinders. Still, the source must be severed. **Slay the CoalWisp** and return peace to our waters.\"";
            }
        }

        public override object Refuse
        {
            get
            {
                return "\"Then may the embers spare you, for they grow fiercer by the day... and the springs grow colder.\"";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "\"The Wisp still flickers? I feel it, even now. The waters cry for cleansing.\"";
            }
        }

        public override object Complete
        {
            get
            {
                return "\"The flames wane. You’ve given breath back to the springs—and hope to Devil Guard.\"\n\n" +
                       "**Take this: TheFrozenFang.** Born of frost, it is my thanks for bringing balance to fire and water.\"";
            }
        }

        public ExtinguishWispQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(CoalWisp), "CoalWisp", 1));
            AddReward(new BaseReward(typeof(TheFrozenFang), 1, "TheFrozenFang"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Extinguish the Wisp'!");
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

    public class SelwaAshwarden : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(ExtinguishWispQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBHealer());
        }

        [Constructable]
        public SelwaAshwarden()
            : base("the Spa Healer", "Selwa Ashwarden")
        {
        }

        public SelwaAshwarden(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(70, 80, 90);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = 1153; // Pale skin tone
            HairItemID = Race.RandomHair(this);
            HairHue = 1150; // Silver-blue
        }

        public override void InitOutfit()
        {
            AddItem(new Robe() { Hue = 1266, Name = "Ashen Spa Robe" }); // Light gray
            AddItem(new Sandals() { Hue = 2401, Name = "Cinderwalk Sandals" }); // Dark ember
            AddItem(new BodySash() { Hue = 1150, Name = "Mistweave Sash" }); // Silver-blue
            AddItem(new HoodedShroudOfShadows() { Hue = 1154, Name = "Smokeveil Hood" }); // Dark smoke
            AddItem(new QuarterStaff() { Hue = 1153, Name = "Emberbane Staff" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Healer's Satchel";
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
