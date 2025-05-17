using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class LockUpTheFireScorched : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Lock Up the FireScorchedSkeleton"; } }

        public override object Description
        {
            get
            {
                return
                    "Rowan Ashvale, a respected cloth merchant of West Montor, stands before you, his finely embroidered tunic scorched at the edges.\n\n" +
                    "His hands are blistered, yet steady as he clutches a charred ledger.\n\n" +
                    "“Damn that skeleton. It’s no ordinary beast—it ransacked my caravan near the Gate of Hell, and worse, ruined my finest shipment bound for Britain and Renika.”\n\n" +
                    "“The flames that cling to its bones... they don’t just burn—they *mock*. I saw my cloth twist in its hands like it knew what I valued most.”\n\n" +
                    "**Slay the FireScorchedSkeleton** and bring peace to the trade routes—or my trade may be ashes before summer.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then my losses deepen, and trade suffers. I pray another with steel and courage will rise.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "The roads still burn, do they? My wagons lie in ruins and every sunrise could bring another attack.";
            }
        }

        public override object Complete
        {
            get
            {
                return "Gone? Truly gone?\n\n" +
                       "You’ve done more than avenge my wares—you’ve preserved the lifeblood of West Montor’s trade.\n\n" +
                       "Take this: the *RiverRaftersChest*. It’s made to endure any storm, fire, or flood. May it guard your treasures as you’ve guarded mine.";
            }
        }

        public LockUpTheFireScorched() : base()
        {
            AddObjective(new SlayObjective(typeof(FireScorchedSkeleton), "FireScorchedSkeleton", 1));
            AddReward(new BaseReward(typeof(RiverRaftersChest), 1, "RiverRaftersChest"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Lock Up the FireScorchedSkeleton'!");
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

    public class RowanAshvale : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(LockUpTheFireScorched) }; } }

        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBWeaver()); // Cloth Merchant
        }

        [Constructable]
        public RowanAshvale()
            : base("the Cloth Merchant", "Rowan Ashvale")
        {
        }

        public RowanAshvale(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(80, 75, 30);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = 1023; // Warm, sun-kissed skin tone
            HairItemID = 0x203C; // Short hair
            HairHue = 1153; // Soot-black
            FacialHairItemID = 0x2041; // Full beard
            FacialHairHue = 1153;
        }

        public override void InitOutfit()
        {
            AddItem(new FormalShirt() { Hue = 2418, Name = "Ashvale's Embroidered Shirt" }); // Burnt orange
            AddItem(new LongPants() { Hue = 2101, Name = "Merchant's Trade Trousers" }); // Deep navy
            AddItem(new HalfApron() { Hue = 1161, Name = "Ember-Streaked Apron" }); // Scorched charcoal
            AddItem(new Boots() { Hue = 1109, Name = "Caravan-Worn Boots" }); // Dusty grey
            AddItem(new FeatheredHat() { Hue = 1157, Name = "Ashvale's Trade Plume" }); // Crimson feather on black
            AddItem(new Cloak() { Hue = 2124, Name = "Renikan Sailcloth Cloak" }); // Sea-green, from trade with Renika

            Backpack backpack = new Backpack();
            backpack.Hue = 1161;
            backpack.Name = "Merchant's Ledger Pack";
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
