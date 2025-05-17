using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class RoyalMountQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Royal Mount"; } }

        public override object Description
        {
            get
            {
                return
                    "You stand before *Gaius Quarrymaster*, Mineral Surveyor of Moon’s windswept quarries.\n\n" +
                    "He leans heavily on a pickaxe, the head etched with ancient runes, eyes weary yet burning with conviction.\n\n" +
                    "“We were never meant to cut so deep… not this close to the Pyramid.”\n\n" +
                    "“My crews and I, we’ve been mapping the **Royal Mount**, a rare vein of silverfold stone. The ancients called it *Dream Ore*—said to resonate with the stars and the voices beneath. The stone sings when struck right, guides our tools, even clears fog from the mind.”\n\n" +
                    "“But two weeks past, that *thing* emerged. Not from a cave, not from a tomb—**it galloped out of a shimmer in the air**. A steed, bone-white and blazing with runes, faster than thought. It trampled our work. Left symbols glowing on the quarry walls—sigils I don’t understand, but they *move* when the moon wanes.”\n\n" +
                    "“Old scholars say the Pharaoh of the Pyramid once rode a steed whose hooves never touched sand, bound to him by Void-chains. This must be it. **The Pharaoh’s Steed.**”\n\n" +
                    "“I’m no warrior. I’m a surveyor. I measure veins, I read stone. But this beast... it's warping the land. If it etches one more sigil into the Mount, we might awaken something deeper than ore.”\n\n" +
                    "**Slay the Pharaoh’s Steed** before the quarries collapse—or worse, open a gate to the Pyramid’s curse.";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then may the quarry hold fast. But I fear each night brings us closer to collapse—or calling something that won’t return to sleep.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still it rides? The sigils deepen. I see them in my dreams now... they speak to something below the Mount. Something waiting.";
            }
        }

        public override object Complete
        {
            get
            {
                return "So... the hooves have stilled. And the Mount is quiet again.\n\n" +
                       "You’ve done more than slay a beast. You’ve **preserved the soul of the stone**—and possibly the fate of Moon itself.\n\n" +
                       "Take this: *Pathcleaver.* Not forged, but grown from the very ore we mined. Let it carve your way as surely as you’ve carved peace from peril.";
            }
        }

        public RoyalMountQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(PharaohsSteed), "Pharaoh’s Steed", 1));
            AddReward(new BaseReward(typeof(Pathcleaver), 1, "Pathcleaver"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Royal Mount'!");
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

    public class GaiusQuarrymaster : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(RoyalMountQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBBarkeeper()); 
        }

        [Constructable]
        public GaiusQuarrymaster()
            : base("the Mineral Surveyor", "Gaius Quarrymaster")
        {
        }

        public GaiusQuarrymaster(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(85, 90, 30);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = 1109; // Dust-gray
            FacialHairItemID = Race.RandomFacialHair(this);
            FacialHairHue = 1109;
        }

        public override void InitOutfit()
        {
            AddItem(new LeatherDo() { Hue = 2405, Name = "Quarryman's Tunic" }); // Quarry-grey
            AddItem(new StuddedLegs() { Hue = 2413, Name = "Vein-Seeker Greaves" });
            AddItem(new LeatherGloves() { Hue = 2309, Name = "Chalk-Dusted Gloves" });
            AddItem(new LeatherCap() { Hue = 1824, Name = "Surveyor's Helm" });
            AddItem(new HalfApron() { Hue = 1819, Name = "Stonebinder's Apron" });
            AddItem(new Boots() { Hue = 1812, Name = "Ore-Stompers" });

            AddItem(new Pickaxe() { Hue = 2500, Name = "Miner's Measure" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Quarry Pack";
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
