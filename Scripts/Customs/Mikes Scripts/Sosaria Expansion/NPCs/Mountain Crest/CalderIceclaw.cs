using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class ClawsInTheColdQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Claws in the Cold"; } }

        public override object Description
        {
            get
            {
                return
                    "You approach *Calder Iceclaw*, renowned Predator Tracker of Mountain Crest.\n\n" +
                    "His breath frosts in the air as he sharpens a peculiar, claw-marked spear, eyes sharp with focus but weighed by worry.\n\n" +
                    "“A beast stalks these peaks, one I can’t track—not fully. Its claws, they freeze the ground, leaving marks that vanish in seconds.”\n\n" +
                    "“I’ve tracked hydras, wyverns, beasts that don’t bleed... but this? This is different. **Something colder than death** stalks the Ice Cavern, and travelers go missing.”\n\n" +
                    "**Find the Iceclaw Predator** and bring it down before the snows swallow more souls.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then tread carefully, traveler. It knows these paths better than you.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still it prowls? I feel it in the wind. The mountain mourns the lost.";
            }
        }

        public override object Complete
        {
            get
            {
                return "The claw’s been silenced… You’ve done what I could not.\n\n" +
                       "Take this: **DragonHoardChest**—may its treasure match the courage you’ve shown.\n\n" +
                       "I’ll remember this. Sosaria will remember.";
            }
        }

        public ClawsInTheColdQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(IceclawPredator), "Iceclaw Predator", 1));
            AddReward(new BaseReward(typeof(DragonHoardChest), 1, "DragonHoardChest"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Claws in the Cold'!");
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

    public class CalderIceclaw : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(ClawsInTheColdQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBBaker()); // Ranger fits a beast-tracking, wilderness expert.
        }

        [Constructable]
        public CalderIceclaw()
            : base("the Predator Tracker", "Calder Iceclaw")
        {
        }

        public CalderIceclaw(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(95, 100, 75);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = 1150; // Frost-white hair
            FacialHairItemID = Race.RandomFacialHair(this);
            FacialHairHue = 1150;
        }

        public override void InitOutfit()
        {
            AddItem(new FurSarong() { Hue = 1150, Name = "Frostfang Pelt" });
            AddItem(new StuddedChest() { Hue = 1109, Name = "Glacierhide Vest" });
            AddItem(new StuddedGloves() { Hue = 1102, Name = "Icebound Grips" });
            AddItem(new LeatherLegs() { Hue = 1108, Name = "Snowstalker Greaves" });
            AddItem(new FurBoots() { Hue = 1107, Name = "Tundra Treads" });
            AddItem(new BearMask() { Hue = 1106, Name = "Clawmark Helm" });

            AddItem(new BladedStaff() { Hue = 1152, Name = "Frostfang Spear" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1153;
            backpack.Name = "Tracker’s Pack";
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
