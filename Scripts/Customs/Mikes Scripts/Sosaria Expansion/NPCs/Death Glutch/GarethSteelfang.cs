using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class SteedOfShadowsQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Steed of Shadows"; } }

        public override object Description
        {
            get
            {
                return
                    "You approach *Captain Gareth Steelfang*, Cavalry Instructor of Death Glutch, standing tall amidst scorched banners.\n\n" +
                    "His armor bears the marks of a thousand drills, but it’s the grief in his eyes that speaks louder.\n\n" +
                    "“There was a time when the steeds of Malidor rode with honor. I trained them myself—noble beasts, fierce and proud.”\n\n" +
                    "“But now, one of them has returned... twisted. **The Malidor Warhorse**—undead, cursed, and rampaging through the old parade grounds near that damned academy.”\n\n" +
                    "“It mocks everything we stood for. And I will not see its shadow tarnish the memory of my riders.”\n\n" +
                    "“Bring it down. Let it find peace—or destruction. And bring me proof. The land won’t rest while it rides.”\n\n" +
                    "*End the rampage of the undead Malidor Warhorse haunting the academy’s grounds.*";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then may its hooves thunder until the earth breaks beneath us all.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still it rides? My sleep grows shorter with each night it roams.";
            }
        }

        public override object Complete
        {
            get
            {
                return "You’ve done it... The ground no longer trembles, and the banners need not bow in shame.\n\n" +
                       "*Take this: FlamePlateGorget.* Worn once by my bravest, it shall now guard you.\n\n" +
                       "And this silver spur—keep it, in memory of what we’ve lost, and of what we’ll never lose again.";
            }
        }

        public SteedOfShadowsQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(MalidorWarhorse), "Malidor Warhorse", 1));
            AddReward(new BaseReward(typeof(FlamePlateGorget), 1, "FlamePlateGorget"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Steed of Shadows'!");
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

    public class GarethSteelfang : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(SteedOfShadowsQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBKeeperOfChivalry());
        }

        [Constructable]
        public GarethSteelfang()
            : base("the Cavalry Instructor", "Captain Gareth Steelfang")
        {
        }

        public GarethSteelfang(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(100, 80, 75);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = 1147; // Iron-gray
            FacialHairItemID = Race.RandomFacialHair(this);
            FacialHairHue = 1147;
        }

        public override void InitOutfit()
        {
            AddItem(new PlateChest() { Hue = 1157, Name = "Steelfang's Chestplate" }); // Midnight steel
            AddItem(new PlateLegs() { Hue = 1157, Name = "Marching Greaves" });
            AddItem(new PlateGloves() { Hue = 1154, Name = "Drillmaster's Gauntlets" });
            AddItem(new PlateHelm() { Hue = 1157, Name = "Instructor's Crest" });
            AddItem(new Cloak() { Hue = 1153, Name = "Banner of the Fallen Riders" }); // Blood-red
            AddItem(new BodySash() { Hue = 1109, Name = "Silver Spur Sash" }); // Pale silver

            AddItem(new Lance() { Hue = 2406, Name = "Ceremonial Lance of Malidor" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1175;
            backpack.Name = "Cavalry Pack";
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
