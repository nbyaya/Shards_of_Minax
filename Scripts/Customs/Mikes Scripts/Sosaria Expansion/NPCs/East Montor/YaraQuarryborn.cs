using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class InfernosEndQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Inferno’s End"; } }

        public override object Description
        {
            get
            {
                return
                    "Before you stands *Yara Quarryborn*, the formidable stonecutter of East Montor, her arms dusted with fine grit, eyes blazing like molten rock.\n\n" +
                    "She clenches a broken chisel, scorched and warped. Her voice is a growl of frustration and fear.\n\n" +
                    "“I’ve worked those quarries since I could lift a hammer. But these last days… *fire ants*, they say. No, these are no ants. Not like any I’ve seen.”\n\n" +
                    "“They came from deep beneath, from a molten vein I never dared follow. And their queen… she’s burrowed into the heart of my livelihood. Set it ablaze.”\n\n" +
                    "“If she isn’t stopped, the whole of East Montor might burn from beneath. Slay the *InfernoAnt Queen* in the **Caves of Drakkon**. Bring peace to my quarry—or watch it all turn to slag.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then the fire will spread. And when my stones crumble, East Montor might follow.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still she nests? My tools grow hot just from lying near the quarry’s edge. I won’t last much longer.";
            }
        }

        public override object Complete
        {
            get
            {
                return "The flames are gone? The hive stilled? You’ve saved more than stone—you’ve saved *all that we build upon it*.\n\n" +
                       "Here, take this *PileOfChains*. My quarrymen found them after she fell. They seem tied to her hive. Maybe they’ll serve you better.";
            }
        }

        public InfernosEndQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(InfernoAnt), "InfernoAnt Queen", 1));
            AddReward(new BaseReward(typeof(PileOfChains), 1, "PileOfChains"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Inferno’s End'!");
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

    public class YaraQuarryborn : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(InfernosEndQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBStoneCrafter());
        }

        [Constructable]
        public YaraQuarryborn()
            : base("the Stonecutter", "Yara Quarryborn")
        {
        }

        public YaraQuarryborn(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(90, 100, 50);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = 1825; // Deep ember red
        }

        public override void InitOutfit()
        {
            AddItem(new StuddedBustierArms() { Hue = 2420, Name = "Ashen Armguard" }); // Smoky grey
            AddItem(new StuddedLegs() { Hue = 2411, Name = "Cinderwoven Leggings" }); // Charcoal
            AddItem(new HalfApron() { Hue = 2075, Name = "Forge-Kissed Apron" }); // Burnt orange
            AddItem(new LeatherGloves() { Hue = 2405, Name = "Stonegrip Gloves" }); // Quarry-grey
            AddItem(new LeatherCap() { Hue = 2101, Name = "Vein-Seeker's Helm" }); // Dusky bronze
            AddItem(new Boots() { Hue = 1813, Name = "Magma-Tread Boots" }); // Darkened soil

            AddItem(new SmithSmasher() { Hue = 2511, Name = "Quarryborn’s Mallet" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1155; // Slightly scorched look
            backpack.Name = "Stonecutter's Pack";
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
