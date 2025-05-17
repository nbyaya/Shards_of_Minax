using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class OozeAtTheGatesQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Ooze at the Gates"; } }

        public override object Description
        {
            get
            {
                return
                    "You approach *Nara Seabreeze*, Harbor Navigator of Pirate Isle, standing at the edge of the docks with salt-swept hair and a chart rolled tightly in hand.\n\n" +
                    "Her sea-blue eyes dart anxiously between the waves and the ink marks of her map.\n\n" +
                    "“You see that channel? We can’t pass it. The seabed’s not right anymore. That *thing*... the ooze—it shifts everything. Boats disappear, anchors don’t hold, and the sea sings wrong.”\n\n" +
                    "“I’ve lost three small vessels, good sailors too, just trying to chart it. No point now—my maps are useless while that slime festers.”\n\n" +
                    "“You look like someone who knows how to handle a blade better than a compass. **Slay the DesolateOoze** that clogs the ship channel, or we’re all adrift.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then we’ll wait for the tide to drag the ooze elsewhere—if it doesn’t drag us first.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still no passage? I’ve set another marker... but it’s fading fast. The ooze stirs the sea, and the sea stirs the bones.";
            }
        }

        public override object Complete
        {
            get
            {
                return "You’ve cleared the channel? The sea feels right again. Charts hold, anchors bite. You’ve done more than slay a monster—you’ve given us back the tide.\n\n" +
                       "Here—an *AlienArtifaxChest* we dredged up last year. Too strange for most, but perhaps it holds something worthy of you.";
            }
        }

        public OozeAtTheGatesQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(DesolateOoze), "DesolateOoze", 1));
            AddReward(new BaseReward(typeof(AlienArtifaxChest), 1, "AlienArtifaxChest"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x59, "You've completed 'Ooze at the Gates'!");
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

    public class NaraSeabreeze : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(OozeAtTheGatesQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBShipwright(this));
        }

        [Constructable]
        public NaraSeabreeze()
            : base("the Harbor Navigator", "Nara Seabreeze")
        {
        }

        public NaraSeabreeze(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(90, 80, 50);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = 1150; // Sea-salt silver-blue
        }

        public override void InitOutfit()
        {
            AddItem(new FancyShirt() { Hue = 1153, Name = "Navigator's Tunic" }); // Deep sea blue
            AddItem(new Skirt() { Hue = 1154, Name = "Tide-Swept Skirt" }); // Ocean green
            AddItem(new BodySash() { Hue = 2101, Name = "Compass Bearer's Sash" }); // Storm grey
            AddItem(new FeatheredHat() { Hue = 1157, Name = "Mariner's Plume" }); // Mist white
            AddItem(new Sandals() { Hue = 2401, Name = "Salt-Stained Sandals" }); // Driftwood brown

            AddItem(new GnarledStaff() { Hue = 1156, Name = "Wavecaller’s Rod" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1152;
            backpack.Name = "Navigator’s Charts";
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
