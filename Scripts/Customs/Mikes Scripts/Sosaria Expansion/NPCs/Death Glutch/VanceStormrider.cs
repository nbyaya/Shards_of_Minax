using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class WingsOfWoeQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Wings of Woe"; } }

        public override object Description
        {
            get
            {
                return
                    "You approach *Vance Stormrider*, the weathered Gryphon Trainer of Death Glutch.\n\n" +
                    "He stands tall beside a juvenile gryphon, its wings twitching nervously. His cloak flutters despite the still air, marked with symbols of old skyward tribes.\n\n" +
                    "“They’re not hatching right,” Vance mutters, voice gravelly. “The eggs, they... pulse. Like they’re listening to something foul.”\n\n" +
                    "“A beast—some foul **Enchantment Dragon**—has been tampering with the nests. Left its runes burned into the shells.”\n\n" +
                    "“You find it, in that cursed ruin... Malidor’s wreck of an academy. Slay it. Before my gryphons grow to curse the skies.”\n\n" +
                    "**Slay the Enchantment Dragon** in Malidor Witches Academy, and bring peace to the skies of Death Glutch.";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then hope the hatchlings don’t turn on us. I’ll watch the skies alone.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still alive, is it? I feel it through the eggs... it’s getting bolder.";
            }
        }

        public override object Complete
        {
            get
            {
                return "It’s done? The skies breathe easier, and my eggs... still.\n\n" +
                       "Take this. Fish may feed the body, but today you’ve fed the future of the skies.";
            }
        }

        public WingsOfWoeQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(EnchantmentDragon), "Enchantment Dragon", 1));
            AddReward(new BaseReward(typeof(FishBasket), 1, "FishBasket"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Wings of Woe'!");
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

    public class VanceStormrider : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(WingsOfWoeQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBAnimalTrainer()); // Fits Gryphon Trainer profession
        }

        [Constructable]
        public VanceStormrider()
            : base("the Gryphon Trainer", "Vance Stormrider")
        {
        }

        public VanceStormrider(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(100, 80, 50);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = 1150; // Storm-grey
            FacialHairItemID = Race.RandomFacialHair(this);
            FacialHairHue = 1150;
        }

        public override void InitOutfit()
        {
            AddItem(new LeatherDo() { Hue = 1109, Name = "Stormhide Vest" }); // Dark storm-grey
            AddItem(new LeatherLegs() { Hue = 1107, Name = "Windswept Greaves" }); // Blue-grey
            AddItem(new FeatheredHat() { Hue = 1175, Name = "Gryphonplume Cap" }); // Sky-blue with feathers
            AddItem(new Cloak() { Hue = 2101, Name = "Skywatcher’s Cloak" }); // Deep midnight blue
            AddItem(new LeatherGloves() { Hue = 1819, Name = "Claw-Worn Gloves" }); // Earthy brown
            AddItem(new Boots() { Hue = 1824, Name = "Ridgewalkers" }); // Rugged mountain boots

            AddItem(new QuarterStaff() { Hue = 2405, Name = "Stormrider’s Staff" }); // Weathered wood staff

            Backpack backpack = new Backpack();
            backpack.Hue = 2000;
            backpack.Name = "Trainer's Pack";
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
