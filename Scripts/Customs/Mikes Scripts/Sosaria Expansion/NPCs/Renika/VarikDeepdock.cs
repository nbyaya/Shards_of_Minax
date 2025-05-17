using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class SteedOfStoneQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Steed of Stone"; } }

        public override object Description
        {
            get
            {
                return
                    "Varik Deepdock, a grizzled warehouse foreman, eyes you with weary suspicion as he jots down inventory on a worn slate.\n\n" +
                    "“You see those crates? Every one of them's gold to me. Or was... before the MountainSteed came down from the Stronghold.”\n\n" +
                    "“Supply wagons don’t make it through the pass no more. That stone beast tramples 'em, shatters wheels, and scatters goods like dead leaves.”\n\n" +
                    "“I’ve counted every crate on these docks for twenty years. Now the merchants say I’m bleeding them dry. **If we don’t stop that beast, Renika’s trade dies with it.**”\n\n" +
                    "“Bring it down. For the docks. For the trade. For every crate I’ve ever counted.”\n\n" +
                    "**Slay the MountainSteed** in the Mountain Stronghold and restore the flow of goods to Renika.";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then keep away from my docks. I’ve no time for shirkers. Every day that beast breathes, we lose more than coin—we lose trust.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still trampling? The pass stays shut, and the merchants grow restless. You’d best hurry.";
            }
        }

        public override object Complete
        {
            get
            {
                return "You did it? The beast is dead? Ha! The pass will open again, the crates will roll in, and Renika will breathe easy.\n\n" +
                       "Take this: *CetrasBlessing.* It’s not much, but it’s more than the dockmasters will ever give. You've saved more than goods—you’ve saved livelihoods.";
            }
        }

        public SteedOfStoneQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(MountainSteed), "MountainSteed", 1));
            AddReward(new BaseReward(typeof(CetrasBlessing), 1, "CetrasBlessing"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Steed of Stone'!");
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

    public class VarikDeepdock : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(SteedOfStoneQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBBowyer());
        }

        [Constructable]
        public VarikDeepdock()
            : base("the Warehouse Foreman", "Varik Deepdock")
        {
        }

        public VarikDeepdock(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(90, 85, 40);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = 1107; // Salt-washed dark
            FacialHairItemID = 0x203D; // Thick beard
            FacialHairHue = 1107;
        }

        public override void InitOutfit()
        {
            AddItem(new Shirt() { Hue = 1813, Name = "Deepdock Linen Shirt" }); // Sea-grey
            AddItem(new ShortPants() { Hue = 1820, Name = "Dockside Breeches" }); // Weathered blue
            AddItem(new HalfApron() { Hue = 1811, Name = "Tallyman’s Apron" }); // Faded brown
            AddItem(new Boots() { Hue = 2101, Name = "Crate-Stompers" }); // Worn black
            AddItem(new SkullCap() { Hue = 1819, Name = "Foreman’s Cap" }); // Dockside leather
            AddItem(new BodySash() { Hue = 2411, Name = "Ledger Sash" }); // Ink-stained white

            Backpack backpack = new Backpack();
            backpack.Hue = 1175; // Dark navy
            backpack.Name = "Dock Ledger Pack";
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
