using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class StopGoblinArsonistQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Stop the GoblinArsonist"; } }

        public override object Description
        {
            get
            {
                return
                    "You find *Piper Willowfoot*, a nimble messenger of West Montor, perched on a stone fence, her boots dusted from long travel.\n\n" +
                    "Her eyes spark with urgency, a satchel of singed letters hanging from her side.\n\n" +
                    "“They’re burning everything... posthouses, watchtowers, barns. My routes—*our lifelines*—are turning to ash.”\n\n" +
                    "“It’s the **GoblinArsonist**, no doubt. I caught a glimpse—red eyes, smoke trailing from its steps. It’s holed up in the **Gate of Hell**, fanning flames with glee.”\n\n" +
                    "“I’ve slipped past Devil Guard patrols, delivered letters through warzones... but I can’t outrun fire.”\n\n" +
                    "**Find and slay the GoblinArsonist**. Before trade stops. Before we lose more than letters.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "“If you won’t help, then stay out of the smoke. I’ll find someone who cares enough to act.”";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "“The fires still rage? Every hour wasted is another route lost. Please, stop that beast.”";
            }
        }

        public override object Complete
        {
            get
            {
                return "“You did it? The flames are dying?”\n\n" +
                       "“You’ve saved more than trade—you’ve saved our hope. Take this: *CarpentersStalwartTunic*. May it protect you as you’ve protected us.”\n\n" +
                       "“And thank you. Truly.”";
            }
        }

        public StopGoblinArsonistQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(GoblinArsonist), "GoblinArsonist", 1));
            AddReward(new BaseReward(typeof(CarpentersStalwartTunic), 1, "CarpentersStalwartTunic"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Stop the GoblinArsonist'!");
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

    public class PiperWillowfoot : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(StopGoblinArsonistQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBCarpenter());
        }

        [Constructable]
        public PiperWillowfoot()
            : base("the Swift Messenger", "Piper Willowfoot")
        {
        }

        public PiperWillowfoot(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 90, 50);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = 1055; // Light tan skin tone
            HairItemID = 0x203B; // Long Hair
            HairHue = 2115; // Sun-touched blonde
        }

        public override void InitOutfit()
        {
            AddItem(new FancyShirt() { Hue = 2129, Name = "Messenger’s Breeze Shirt" }); // Soft teal
            AddItem(new LeatherShorts() { Hue = 2413, Name = "Travel-Worn Breeches" }); // Faded brown
            AddItem(new HalfApron() { Hue = 1153, Name = "Courier's Satchel Strap" }); // Deep navy
            AddItem(new Sandals() { Hue = 2309, Name = "Fleetfoot Sandals" }); // Dust brown
            AddItem(new Cloak() { Hue = 1157, Name = "Ashcloak of the Road" }); // Smoky grey
            AddItem(new Bandana() { Hue = 2118, Name = "Windrunner’s Wrap" }); // Sky blue

            Backpack backpack = new Backpack();
            backpack.Hue = 1137;
            backpack.Name = "Messenger’s Pouch";
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
