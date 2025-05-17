using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class OstardCrushQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Ostard Crush"; } }

        public override object Description
        {
            get
            {
                return
                    "You approach *Serin Ashguard*, the dedicated bathhouse attendant of East Montor’s renowned mineral springs.\n\n" +
                    "Her robe shimmers with the hues of the steaming pools, yet her face is etched with frustration.\n\n" +
                    "“Have you ever tried to convince a noble to bathe with **a DrakonsOstard crashing through your spa**?”\n\n" +
                    "“The beast—wild, scaled, mad with the heat—has trampled three terraces already. My guests are terrified. The springs lie still, unattended, and the town murmurs that **we’ve angered the mountain**.”\n\n" +
                    "“The tracks lead into the forbidden caverns of Drakkon. I dare not follow, but you—you look like you could handle a rampaging beast.”\n\n" +
                    "**Find and slay the DrakonsOstard**. Bring peace back to the springs—and let us once more bask in the calm of our waters.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then the baths will remain dry, and my reputation in tatters. May the mountain show more mercy than my guests.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "The beast still roams? I can hear it from here. My guests speak of leaving. The springs feel colder each day.";
            }
        }

        public override object Complete
        {
            get
            {
                return "You’ve done it? The terraces no longer tremble, and the steam flows freely again.\n\n" +
                       "You’ve saved my livelihood—and perhaps more. Who knows what else that beast would have disturbed in those ancient caverns?\n\n" +
                       "**Take this NeroChest**. It’s more than I can offer, but far less than you deserve.";
            }
        }

        public OstardCrushQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(DrakonsOstard), "DrakonsOstard", 1));
            AddReward(new BaseReward(typeof(NeroChest), 1, "NeroChest"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Ostard Crush'!");
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

    public class SerinAshguard : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(OstardCrushQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => false;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBWaiter()); // Closest to a bathhouse attendant
        }

        [Constructable]
        public SerinAshguard()
            : base("the Bathhouse Attendant", "Serin Ashguard")
        {
        }

        public SerinAshguard(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(60, 70, 50);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = 1173; // Warm Copper
        }

        public override void InitOutfit()
        {
            AddItem(new MonkRobe() { Hue = 1153, Name = "Steamweaver's Robe" }); // Pale Blue
            AddItem(new Sandals() { Hue = 2101, Name = "Terrace Slippers" }); // Soft Tan
            AddItem(new BodySash() { Hue = 2401, Name = "Mist-Sash" }); // Grey Mist
            AddItem(new FeatheredHat() { Hue = 1170, Name = "Spa Mistress's Hat" }); // Soft Teal

            Backpack backpack = new Backpack();
            backpack.Hue = 1175; // Aquatic Blue
            backpack.Name = "Bathhouse Satchel";
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
