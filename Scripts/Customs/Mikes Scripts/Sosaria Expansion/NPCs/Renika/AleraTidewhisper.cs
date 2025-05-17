using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class DragonsDiminishedRoarQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Dragon’s Diminished Roar"; } }

        public override object Description
        {
            get
            {
                return
                    "You stand before *Alera Tidewhisper*, Tide Priestess of Renika.\n\n" +
                    "Clad in robes that shimmer like moonlight on restless seas, she traces swirling sigils in the air, her voice calm yet heavy with urgency.\n\n" +
                    "“The tides are no longer our allies,” she says, gazing towards the storm-lashed harbor.\n\n" +
                    "“The **AncientDrakon**, long buried within the Mountain Stronghold, writhes in pain. I feel its sorrow through the sea, each wave a cry for release.”\n\n" +
                    "“If we do not silence its anguish, the storms will drown Renika. The sea will no longer heed our calls.”\n\n" +
                    "**Slay the AncientDrakon**, bring me its heart, and calm the fury beneath the waves.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then beware the sea’s wrath. Already, the harbor trembles under the storm’s weight.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "The storms have grown fiercer. Each moment the Drakon lives, our fates sink deeper beneath the waves.";
            }
        }

        public override object Complete
        {
            get
            {
                return "You hold its heart? The sea... calms.\n\n" +
                       "You have not only stilled a beast, but restored harmony to the tides.\n\n" +
                       "Take this: *SearingTouch*. A gift of the sea’s fire, to wield when next the storm calls.";
            }
        }

        public DragonsDiminishedRoarQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(AncientDrakon), "AncientDrakon", 1));
            AddReward(new BaseReward(typeof(SearingTouch), 1, "SearingTouch"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Dragon’s Diminished Roar'!");
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

    public class AleraTidewhisper : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(DragonsDiminishedRoarQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => false;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBMystic());
        }

        [Constructable]
        public AleraTidewhisper()
            : base("the Tide Priestess", "Alera Tidewhisper")
        {
        }

        public AleraTidewhisper(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(90, 85, 40);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = 1002; // Pale Seafoam
            HairItemID = 0x203B; // Long Hair
            HairHue = 1150; // Deep Blue
        }

        public override void InitOutfit()
        {
            AddItem(new MonkRobe() { Hue = 1153, Name = "Robes of the Tidal Moon" }); // Shimmering Oceanic Blue
            AddItem(new Cloak() { Hue = 1266, Name = "Stormcaller’s Veil" }); // Soft Silver
            AddItem(new Sandals() { Hue = 1109, Name = "Seaworn Sandals" }); // Storm-gray
            AddItem(new FlowerGarland() { Hue = 1150, Name = "Garland of Seafoam" }); // Blue-Green

            AddItem(new ArtificerWand() { Hue = 1367, Name = "Tidebinder’s Rod" }); // Coral-hued wand
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
