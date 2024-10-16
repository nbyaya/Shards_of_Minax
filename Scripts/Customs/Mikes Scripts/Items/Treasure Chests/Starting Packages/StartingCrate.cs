using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Regions;


namespace Server.Custom
{
    public class StartingCrate : LockableContainer
    {
        [Constructable]
        public StartingCrate() : base(0xE3D) // Shipping Crate item ID
        {
            Name = "Shipping Crate";
            Hue = Utility.RandomMinMax(1, 1600);

            // Add basic commodities
            AddItemWithProbability(new IronIngot(Utility.RandomMinMax(50, 100)), 0.25);
            AddItemWithProbability(new OakLog(Utility.RandomMinMax(50, 100)), 0.25);
            AddItemWithProbability(new Board(Utility.RandomMinMax(50, 100)), 0.25);
            AddItemWithProbability(new SackFlour(Utility.RandomMinMax(1, 5)), 0.15);
            AddItemWithProbability(new Apple(Utility.RandomMinMax(10, 20)), 0.10);
            AddItemWithProbability(new BreadLoaf(Utility.RandomMinMax(5, 10)), 0.10);
            AddItemWithProbability(new FishSteak(Utility.RandomMinMax(5, 10)), 0.10);
            AddItemWithProbability(new Bottle(Utility.RandomMinMax(10, 20)), 0.10);
            AddItemWithProbability(new ToolKit(), 0.10);
            AddItemWithProbability(new Scissors(), 0.10);
            AddItemWithProbability(new MortarPestle(), 0.10);
            AddItemWithProbability(new Dagger(), 0.10);
			AddItemWithProbability(new IronIngot(Utility.RandomMinMax(1, 50)), 0.07);
			AddItemWithProbability(new DullCopperIngot(Utility.RandomMinMax(1, 50)), 0.07);
			AddItemWithProbability(new ShadowIronIngot(Utility.RandomMinMax(1, 50)), 0.07);
			AddItemWithProbability(new CopperIngot(Utility.RandomMinMax(1, 50)), 0.07);
			AddItemWithProbability(new BronzeIngot(Utility.RandomMinMax(1, 50)), 0.07);
			AddItemWithProbability(new GoldIngot(Utility.RandomMinMax(1, 50)), 0.07);
			AddItemWithProbability(new AgapiteIngot(Utility.RandomMinMax(1, 50)), 0.07);
			AddItemWithProbability(new VeriteIngot(Utility.RandomMinMax(1, 50)), 0.07);
			AddItemWithProbability(new ValoriteIngot(Utility.RandomMinMax(1, 50)), 0.07);
			AddItemWithProbability(new BoltOfCloth(Utility.RandomMinMax(1, 50)) { Hue = Utility.RandomMinMax(1, 2000) }, 0.07);
			AddItemWithProbability(new Cloth(Utility.RandomMinMax(1, 50)) { Hue = Utility.RandomMinMax(1, 2000) }, 0.07);
			AddItemWithProbability(new UncutCloth(Utility.RandomMinMax(1, 50)) { Hue = Utility.RandomMinMax(1, 2000) }, 0.07);
			AddItemWithProbability(new Cotton(Utility.RandomMinMax(1, 50)), 0.07);
			AddItemWithProbability(new Wool(Utility.RandomMinMax(1, 50)), 0.07);
			AddItemWithProbability(new Flax(Utility.RandomMinMax(1, 50)), 0.07);
			AddItemWithProbability(new SpoolOfThread(), 0.07);
			AddItemWithProbability(new OakLog(Utility.RandomMinMax(1, 50)), 0.07);
			AddItemWithProbability(new AshLog(Utility.RandomMinMax(1, 50)), 0.07);
			AddItemWithProbability(new YewLog(Utility.RandomMinMax(1, 50)), 0.07);
			AddItemWithProbability(new SackFlour(), 0.07);
			AddItemWithProbability(new Board(Utility.RandomMinMax(1, 50)) { Hue = Utility.RandomMinMax(1, 2000) }, 0.07);
			AddItemWithProbability(new BreadLoaf(Utility.RandomMinMax(1, 50)) { Hue = Utility.RandomMinMax(1, 2000) }, 0.07);
			AddItemWithProbability(new CheeseWheel(Utility.RandomMinMax(1, 50)) { Hue = Utility.RandomMinMax(1, 1600) }, 0.07);
			AddItemWithProbability(new Bottle(Utility.RandomMinMax(1, 50)) { Hue = Utility.RandomMinMax(1, 1600) }, 0.07);
			AddItemWithProbability(new Bacon(Utility.RandomMinMax(1, 50)) { Hue = Utility.RandomMinMax(1, 1600) }, 0.07);
			AddItemWithProbability(new Ham(Utility.RandomMinMax(1, 50)) { Hue = Utility.RandomMinMax(1, 1600) }, 0.07);
			AddItemWithProbability(new Sausage(Utility.RandomMinMax(1, 50)) { Hue = Utility.RandomMinMax(1, 1600) }, 0.07);

            // Add captain's logs
            AddItemWithProbability(CreateCaptainsLog(), 0.30);
        }

        private void AddItemWithProbability(Item item, double probability)
        {
            if (Utility.RandomDouble() < probability)
                DropItem(item);
        }

        private Item CreateCaptainsLog()
        {
            string[] logs = new string[]
            {
                "Departure scheduled for dawn. All goods accounted for.",
                "Encountered rough seas, lost several crates overboard.",
                "Arrived at port, unloading commenced.",
                "Inspection by customs revealed no discrepancies.",
                "Crew morale is high, weather favorable.",
                "Unexpected delay due to storm, will dock tomorrow.",
                "Order from the harbor master to dock at the secondary pier.",
                "Half of the ale shipment spoiled, issued refund.",
                "The new batch of tools is highly praised by the blacksmith.",
                "Loaded additional wood planks for the upcoming repairs.",
				"Picked up cargo in Minoc, heading to Vesper.",
				"The ship's hull needs minor repairs after the last storm.",
				"Crew reported sightings of sea serpents near Nujel'm.",
				"Delivered special order of reagents to Moonglow.",
				"Unexpected stowaway found hiding among the crates.",
				"Shipment of iron ingots successfully delivered to Britain.",
				"Anchor damaged, had to stop for repairs at Buccaneer's Den.",
				"Severe weather forced us to take shelter in a nearby cove.",
				"Lost contact with the lookout, presumed overboard.",
				"Smuggled goods hidden in false bottom of crate.",
				"Crew member caught stealing provisions, disciplined accordingly.",
				"Received orders to reroute to Trinsic due to increased demand.",
				"Ran into a pirate ship but managed to escape unharmed.",
				"Delivered a message from the governor along with our usual cargo.",
				"Dock workers on strike, had to unload the cargo ourselves.",
				"Crew morale low, considering extra shore leave at next port.",
				"Mystery illness spreading among the crew, seeking a healer.",
				"Port fees have increased, cutting into our profits.",
				"Picked up rare spices in Papua, selling for a high price in Britain.",
				"Crew reported strange noises coming from the cargo hold at night.",
				"Crate of rare books damaged in transit, compensating the buyer.",
				"Found a hidden compartment in one of the crates, contents unknown.",
				"Trade winds favorable, making good time to the next port.",
				"Received a commendation from the harbormaster for timely delivery.",
				"Strange lights seen in the sky, crew on edge.",
				"Delivered enchanted tools to a wizard in Magincia.",
				"Crew member went missing during a shore leave, presumed deserted.",
				"Caught a large fish, had a feast on deck.",
				"Goods inspected by royal officials, everything in order.",
				"Made a new trade agreement with the merchants of Yew.",
				"Lost a crate of fine cloth overboard during a storm.",
				"Sailed through a dense fog, nearly lost our way.",
				"Suspicious activity reported at the last port, increased security.",
				"Crew member found a hidden map, planning to investigate further.",
				"Took on extra cargo at the last minute, ship slightly overloaded.",
				"Saw a ghost ship in the distance, crew unnerved.",
				"Cargo of fresh fruits and vegetables, delivering to Skara Brae.",
				"Encountered a school of dolphins, considered a good omen.",
				"Rescued a stranded fisherman, providing him with passage.",
				"Delivered a shipment of medical supplies to the healer's guild.",
				"Crew member injured during unloading, seeking medical attention.",
				"Delayed by heavy fog, arrived later than expected.",
				"Found a smuggler's den, reported it to the authorities.",
				"Cargo of wine and ale, delivered to the tavern in Jhelom.",
				"Picked up a mysterious package, instructed not to open it.",
				"Had to jettison some cargo to escape a whirlpool.",
				"Crew member found a rare shell, keeping it as a good luck charm.",
				"Delivered a consignment of magical scrolls to the mage tower.",
				"Port under quarantine, had to dock at a nearby island.",
				"Navigated through a narrow strait, challenging but successful.",
				"Crew member claims to have seen a mermaid, others skeptical.",
				"Shipment of exotic pets, bound for a noble's menagerie.",
				"Received a tip about a hidden treasure, planning to investigate.",
				"Unusually high tides, had to wait to dock safely.",
				"Crew found a message in a bottle, contents unreadable.",
				"Delivered a new shipment of armor to the blacksmith's guild.",
				"Encountered a rival trader, outbid them for a lucrative deal.",
				"Crew member came down with scurvy, increasing citrus rations.",
				"Strange symbols found carved into the ship's hull, source unknown.",
				"Picked up a cargo of fireworks, delivering for a festival.",
				"Encountered a dense patch of seaweed, slowed us down.",
				"Delivered a crate of fine silks to the tailor in Vesper.",
				"Took on additional provisions, expecting a long journey.",
				"Crew member claims to have seen a kraken, others dismiss it.",
				"Docked in a secluded cove for a brief rest.",
				"Cargo inspected by customs, no contraband found.",
				"Picked up a shipment of enchanted staves, delivering to a wizard.",
				"Encountered a shipwreck, salvaged some useful materials.",
				"Crew member injured during a bar fight, confined to quarters.",
				"Strong winds helped us make good time to the next port.",
				"Delivered a consignment of gems to the jeweler in Minoc.",
				"Found a stowaway rat, set traps to catch it.",
				"Crew reported seeing a sea serpent, kept a safe distance.",
				"Delivered a shipment of rare herbs to the alchemist in Britain.",
				"Caught in a sudden squall, some minor damage to the sails.",
				"Crew member found a hidden diary, contains mysterious entries.",
				"Delivered a load of construction materials to the fort in Cove.",
				"Crew member claimed to have seen a ghost, others are skeptical.",
				"Made a detour to avoid a pirate-infested area.",
				"Cargo of spices and perfumes, delivering to the market in Moonglow.",
				"Crew celebrated a successful delivery with a feast.",
				"Encountered a friendly merchant ship, exchanged news and supplies.",
				"Strange markings found on the crates, origin unknown.",
				"Delivered a batch of freshly baked bread to the local bakery.",
				"Crew member caught a rare fish, selling it at the next port.",
				"Picked up a shipment of enchanted lanterns, bound for a noble's estate.",
				"Crew member found a rare coin, keeping it as a lucky charm.",
				"Delivered a new batch of musical instruments to the bard's guild.",
				"Caught in a sudden storm, lost a crate overboard.",
				"Crew member found a hidden compartment in their bunk, contents unknown.",
				"Delivered a shipment of healing potions to the infirmary.",
				"Crew member claims to have seen a dragon, others skeptical.",
				"Made good time despite rough seas, arriving ahead of schedule.",
				"Delivered a crate of rare books to the scholar's guild.",
				"Crew member found a hidden map, planning to investigate further.",
				"The cook's stew has started to glow. Crew refusing to eat it.",
				"Found a stowaway. It's the first mate's parrot, and it's been swearing at everyone.",
				"Discovered a leak in the hull. Patched it with the first mate's socks.",
				"Crew insists they saw a mermaid. It turned out to be a floating cabbage.",
				"Lost another crate overboard. Blame it on the invisible sea monsters.",
				"The rum barrels are empty. Crew morale is plummeting rapidly.",
				"Docked at the wrong port. No one noticed until we tried to unload the sheep.",
				"Captain's hat fell overboard. Crew insists it was a sign to turn back.",
				"Caught the cabin boy drawing a mustache on my portrait.",
				"First mate insists he can hear the cheese talking. Ordered him to lay off the ale.",
				"Discovered the source of the mysterious smell: the cook's 'special' fish stew.",
				"Crew tried to mutiny. Bribed them with extra rum rations.",
				"Saw a ghost ship. It waved. Crew pretended not to notice.",
				"The anchor is missing. Suspect it was stolen by fish.",
				"Crew has started a betting pool on how many crates we'll lose this trip.",
				"Found a map to buried treasure. Turns out it was a child's drawing.",
				"Ship's cat caught a rat. Crew threw it a party.",
				"Sailor claims he saw a sea serpent. It was just the cook's pet eel.",
				"The captain's chair broke during a storm. Now using a crate as a throne.",
				"Crew dared the first mate to eat a worm. He did. Now he's sick.",
				"Discovered the cabin boy has been writing a pirate romance novel.",
				"Crew refused to swab the deck until I promised no more fish stew.",
				"First mate insists the ship is haunted by the ghost of a chicken.",
				"Caught the parrot trying to steal my hat. Again.",
				"Crew insists the ship's figurehead winked at them. Clearly losing their minds.",
				"Found a barrel of rum hidden in the lifeboat. Crew denies all knowledge.",
				"Cook tried a new recipe. It moved. Crew refused to eat it.",
				"Crew insists they saw a sea monster. It was a floating log.",
				"Discovered the anchor chain has been tied in knots. Suspect mermaids.",
				"First mate's hair turned green after the last storm. Blame it on the cook's stew.",
				"The compass is spinning wildly. Crew believes it's cursed.",
				"Caught the cabin boy teaching the parrot to sing sea shanties.",
				"Crew insists the ship's bell is haunted. It rings itself at midnight.",
				"Found the ship's cat napping in the captain's bed. It refuses to move.",
				"Crew dared the first mate to swim with sharks. He refused. They called him a chicken.",
				"Discovered the cabin boy has been using the ship's log as a diary.",
				"First mate insists the stars are spelling out messages. Crew thinks he's gone mad.",
				"Crew found a mysterious crate. It contained nothing but old socks.",
				"Caught the parrot trying to pick the lock on the rum cabinet.",
				"Crew insists the ship's wheel is possessed. It turns itself at night.",
				"First mate claims he can speak whale. Crew is not impressed.",
				"Discovered the ship's biscuits are harder than the anchor.",
				"Crew found a treasure map. It led to the ship's bathroom.",
				"Caught the cabin boy hiding in a crate to avoid swabbing the deck.",
				"Crew insists the moon is following us. First mate agrees. I need new crew.",
				"Discovered the ship's figurehead has been replaced with a carved pumpkin.",
				"Crew found a message in a bottle. It was a grocery list.",
				"First mate insists the sea is singing to him. Crew is worried.",
				"Caught the parrot wearing the captain's hat. It looked better on him.",
				"Crew insists the ship's lanterns are blinking in code. First mate agrees.",
				"Found the cabin boy trying to train a seagull to fetch.",
				"Crew is convinced the ship's cat can predict the weather.",
				"First mate claims he saw a flying fish. Crew thinks he's drunk.",
				"Discovered the ship's log is missing pages. Suspect the parrot."
            };

            return new SimpleNote
            {
                NoteString = logs[Utility.Random(logs.Length)],
                TitleString = "Captain's Log"
            };
        }

        public StartingCrate(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
