using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class BoundFoesQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Bound Foes"; } }

        public override object Description
        {
            get
            {
                return
                    "Trader? Wanderer? Then listen closely.\n\n" +
                    "I’m Farid Nightstride, guard to these desert caravans, and lately we’ve had trouble in the southern outposts. " +
                    "Desert Hoppers—vile things with legs like springs—are breeding fast and gnawing our wagons apart. " +
                    "They leap the walls, strike at night, and vanish into the dunes.\n\n" +
                    "**Hunt down 10 Desert Hoppers** near the southern trade routes and give us peace under the stars.";
            }
        }

        public override object Refuse { get { return "Then mind your own wheels, stranger. They won't last long."; } }

        public override object Uncomplete { get { return "The hoppers still plague us. They’re fast—have you tracked them well?"; } }

        public override object Complete { get { return "Good work. You’ve saved more than you know. Take this—carved from the sands, blessed by firelight."; } }

        public BoundFoesQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(DesertHopper), "Desert Hoppers", 10));

            AddReward(new BaseReward(typeof(HearthfieldShade), 1, "HearthfieldShade"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Bound Foes'!");
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

    public class FaridNightstride : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(BoundFoesQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBHairStylist()); 
        }

        [Constructable]
        public FaridNightstride() : base("Farid Nightstride", "Caravan Guard")
        {
            Title = "Caravan Guard";
			Body = 0x190; // Male human

            // Outfit
            AddItem(new LeatherNinjaJacket { Hue = 2305, Name = "Nightstride Vest" }); // Dark sand color, lightweight for mobility
            AddItem(new LeatherShorts { Hue = 2413, Name = "Dune-Walker Breeches" }); // Slightly faded olive hue
            AddItem(new LeatherGloves { Hue = 2210, Name = "Sunhide Gloves" }); // Desert-worn leather
            AddItem(new NinjaTabi { Hue = 2106, Name = "Silent-Step Boots" }); // Practical, soft-soled footwear
            AddItem(new ClothNinjaHood { Hue = 2403, Name = "Veil of the Watcher" }); // Light, breathable headwrap
            AddItem(new BodySash { Hue = 2209, Name = "Sash of Vigil" }); // Deep orange, symbolic of his watch over the sands

            // Weapon for flair
            AddItem(new Wakizashi { Hue = 2412, Name = "Caravan's Edge" }); // Curved blade, crafted for quick desert skirmishes

            SetStr(90, 100);
            SetDex(95, 105);
            SetInt(60, 70);

            SetDamage(6, 12);
            SetHits(250, 270);
        }

        public FaridNightstride(Serial serial) : base(serial) { }

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
